using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Save;
using DarklandsBusinessObjects.Streaming;
using DarklandsServices.Services;
using DarklandsUiCommon.Models;

namespace DarklandsConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args == null || !Actions.Keys.Contains(args.First().ToLower()))
            {
                Console.WriteLine("Accepted parameters are:");
                Console.WriteLine(string.Join(", ", Actions.Keys));
            }
            else
            {
                StaticDataService.SetDarklandsPath(@"D:\Steam\SteamApps\common\Darklands");
                Actions[args.First()](args);
            }

            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }

        private static void DoListCities(string[] args)
        {
            foreach (var l in StaticDataService.Cities)
            {
                Console.WriteLine(l);
            }
        }

        private static void DoListSaints(string[] args)
        {
            if (args.Length == 1 || args[1] == "list")
            {
                foreach (var l in StaticDataService.Saints)
                {
                    Console.WriteLine(l);
                }
            }
            else if (args[1] == "listen")
            {
                StartLiveDataService(() =>
                {
                    LiveDataService.MonitorSaints(saints =>
                    {
                        Console.WriteLine();
                        foreach (var s in saints)
                        {
                            Console.WriteLine(s);
                        }
                    });
                });
            }
        }

        private static void DoListFormulae(string[] args)
        {
            if (args.Length == 1 || args[1] == "list")
            {
                foreach (var l in StaticDataService.Formulae)
                {
                    Console.WriteLine(l);
                }
            }
            else if (args[1] == "listen")
            {
                StartLiveDataService(() =>
                {
                    LiveDataService.MonitorFormulae(formulae =>
                    {
                        Console.WriteLine();
                        foreach (var f in formulae)
                        {
                            Console.WriteLine(f);
                        }
                    });
                });
            }
        }

        private static void DoListItems(string[] args)
        {
            foreach (var l in StaticDataService.ItemDefinitions)
            {
                Console.WriteLine(l);
            }
        }

        private static void DoListLocations(string[] args)
        {
            foreach (var l in StaticDataService.Locations)
            {
                Console.WriteLine(l);
            }
        }

        private static void DoReadSave(string[] args)
        {
            if (args.Length < 2 || !File.Exists(args[1]) || Path.GetExtension(args[1]).ToLower() != ".sav")
            {
                Console.WriteLine("Please provide save game file name.");
                return;
            }

            using (var save = new SaveGame(args[1]))
            {
                if (args.Length == 2)
                {
                    Console.WriteLine(save);
                }
                else
                {
                    switch (args[2].ToLower())
                    {
                        case "header":
                            var loc = save.Events.Locations.FirstOrDefault(
                                l => l.Id == save.Header.LocationId);
                            Console.WriteLine("Location: " + loc);
                            break;
                        case "party":
                            foreach (var c in save.Party.Characters)
                            {
                                Console.WriteLine(c.ShortName + ":");

                                var itemsWithNames = from item in c.ItemList.Items
                                    where !item.IsEmpty
                                    select string.Join(" ",
                                        "\tType:" + item.Type,
                                        "QL:" + item.Quality,
                                        "#:" + item.Quantity,
                                        "Weight:" + item.Weight,
                                        StaticDataService.ItemDefinitions.First(i => i.Id == item.Id).Name);
                                Console.WriteLine(string.Join("\n", itemsWithNames));
                            }
                            break;
                        case "events":
                            foreach (var q in QuestModel.FromEvents(
                                save.Events.Events, save.Events.Locations, StaticDataService.ItemDefinitions))
                            {
                                Console.WriteLine(q);
                            }

                            break;
                        default:
                            Console.WriteLine(save);
                            break;
                    }
                }
            }
        }

        private static void DoListenMenu(string[] args)
        {
            StartLiveDataService(() =>
            {
                LiveDataService.MonitorCurrentScreen(menu =>
                {
                    Console.WriteLine();
                    Console.WriteLine(menu);
                });
            });
        }

        private static void StartLiveDataService(Action callback)
        {
            LiveDataService.ConnectionMonitor += connected =>
            {
                if (connected)
                {
                    Console.WriteLine("Darklands process found.");
                    callback();
                }
                else
                {
                    Console.WriteLine("Searching for Darklands process...");
                }
            };

            LiveDataService.Connect();
        }

        private static void DoRegex(string[] args)
        {
            //var reg = new Regex(@"([a-zA-Z \&]+)\s?([+-])\((\d\d?)-(\d\d?)\)");
            //var reg = new Regex(@"(St.[\w\s]+)\[(\d{1,2}v,\s\d{1,2}-\d{1,2}df,\s\d\d%)\]:\s([\w\s\&\(\)]+\s[+|-]\((\d{1,2}-\d{1,2}\))[\.,$])*");

            const string stat = "end";
            var statRegex = new Regex(@"((" + stat + @")\s*\+(?<range>\(\d{1,2}-\d{1,2}\)))",
                RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            var i = 0;

            var output = from s in StaticDataService.Saints
                let matches = statRegex.Matches(s.Clue)
                where matches.Count > 0
                select new
                {
                    Name = s.ShortName,
                    Ranges = string.Join(", ", from match in matches.Cast<Match>()
                        select match.Groups["range"])
                };

            foreach (var o in output)
            {
                i++;
                Console.WriteLine(i + ". " + o.Name + ": " + o.Ranges);
            }
        }

        private static void DoMonitorMemory(string[] args)
        {
            StartLiveDataService(() =>
            {
                var numEv = 0x50;
                LiveDataService.MonitorMemory(0x8B0A070, numEv*0x40, bytes =>
                {
                    Console.WriteLine(DateTime.Now);
                    var s = new ByteStream(bytes);

                    var ev = new List<Event>(numEv);
                    for (var i = 0; i < numEv; i++)
                    {
                        var offset = i*(Event.EventSize + 0x40 - Event.EventSize);
                        var e = new Event(s, offset);
                        if (e.IsActiveQuest) Console.WriteLine(e.CreateDate + " - " + e.ExpireDate);
                        ev.Add(e);
                    }
                });
            });
        }

        private static readonly IReadOnlyDictionary<string, Action<string[]>> Actions = new Dictionary
            <string, Action<string[]>>
        {
            {"cities", DoListCities},
            {"saints", DoListSaints},
            {"formulae", DoListFormulae},
            {"items", DoListItems},
            {"locations", DoListLocations},
            {"readsave", DoReadSave},
            {"menu", DoListenMenu},
            {"regex", DoRegex},
            {"memory", DoMonitorMemory}
        };
    }
}