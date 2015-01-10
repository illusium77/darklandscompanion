using DarklandsBusinessObjects.Save;
using DarklandsServices.Services;
using DarklandsUiCommon.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DarklandsConsoleApp
{
    class Program
    {
        private static IReadOnlyDictionary<string, Action<string[]>> s_actions = new Dictionary<string, Action<string[]>>
        {
            {"cities", DoListCities},
            {"saints", DoListSaints},
            {"formulae", DoListFormulae},
            {"items", DoListItems},
            {"locations", DoListLocations},
            {"readsave", DoReadSave},
            {"menu", DoListenMenu},
            {"regex", DoRegex}
        };

        static void Main(string[] args)
        {
            if (args == null || !s_actions.Keys.Contains(args.First().ToLower()))
            {
                Console.WriteLine("Accepted parameters are:");
                Console.WriteLine(string.Join(", ", s_actions.Keys));
            }
            else
            {
                StaticDataService.SetDarklandsPath(@"D:\Steam\SteamApps\common\Darklands");
                s_actions[args.First()](args);
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
                                                      StaticDataService.ItemDefinitions.First(i => i.Id == item.Code).Name);
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

            var stat = "end";
            var statRegex = new Regex(@"((" + stat + @")\s*\+(?<range>\(\d{1,2}-\d{1,2}\)))", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            int i = 0;

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


        //static void Main(string[] args)
        //{

        //foreach (var city in DarklandsService.Cities.OrderBy(c => c.CityData.EntryCoordinate.X))
        //{
        //    Console.WriteLine(city);
        //}

        //foreach (var item in (from i in DarklandsService.Items
        //                      where i.Name.ToLower().Contains("scroll") || i.ShortName.ToLower().Contains("scroll")
        //                      select i))

        //{
        //    Console.WriteLine(item);
        //}

        //foreach (var f in DarklandsService.Formulae)
        //{
        //    Console.WriteLine(f);
        //}

        //foreach (var s in DarklandsService.Saints)
        //{
        //    Console.WriteLine(s);
        //}

        //for (int i = 0; i < DarklandsService.Saints.Count; i++)
        //{
        //    if (!Saint.s_clueBookDescriptions[i].ToLower().StartsWith(DarklandsService.Saints[i].FullName.ToLower()))
        //    {
        //        Console.WriteLine(DarklandsService.Saints[i].FullName);
        //        Console.WriteLine(Saint.s_clueBookDescriptions[i]);
        //    }
        //}

        //var attr = Enum.GetNames(typeof(ShortAttributes));
        //var skills = Enum.GetNames(typeof(ShortSkills));
        //var other = new[]
        //    {
        //        "Local Rep",
        //        "each weapon skill",
        //        "weapon currently in use skill",
        //        "Heal (skill)"
        //    };

        //skills[0x0c] = "R&W";

        //for (int i = 0; i < Saint.s_clueBookDescriptions.Length; i++)
        //{
        //    var s = Saint.s_clueBookDescriptions[i];

        //var hls = s.ToLower().Split(new[] { "heals", "%" }, StringSplitOptions.RemoveEmptyEntries);

        //if (hls.Length == 0 && s.ToLower().Contains("heals"))
        //{
        //    Console.WriteLine("missing %: " + s);
        //}
        //else
        //{
        //    foreach (var hl in hls)
        //    {

        //    }
        //}

        //var heals = s.ToLower().IndexOf("heals");
        //if (heals != -1)
        //{
        //    if (s.Substring(heals + 6, 3) != "str" && s.Substring(heals + 6, 3) != "end")
        //    {
        //        Console.WriteLine("str end: " + s);

        //    }
        //}


        //if (!s.Contains('('))
        //    continue;

        ////if (s.IndexOf("  ") != -1)
        ////    Console.WriteLine("double space: " + s);
        ////if (!s.EndsWith("."))
        ////    Console.WriteLine(". missing from end: " + s);

        //var lastIndex = -1;
        //var tmp = s;
        //while (lastIndex == -1)
        //{
        //    var p = tmp.LastIndexOf(')');
        //    if (p == -1)
        //        break;

        //    if (Char.IsDigit(tmp[p-1]))
        //    {
        //        lastIndex = p;
        //        break;
        //    }

        //    tmp = tmp.Substring(0, p);
        //}
        //if (lastIndex == -1)
        //    continue;

        //var ss = s.Substring(0, lastIndex +1 );
        //ss = ss.Substring(ss.IndexOf("]: ") + 3);
        //var split = ss.Split(new[] { ", ", ". ", ",", "." }, StringSplitOptions.RemoveEmptyEntries);
        //foreach (var t in split)
        //{
        //    var reg = new Regex(@"([a-zA-Z \&]+)\s?([+-])\((\d\d?)-(\d\d?)\)");

        //    if (reg.IsMatch(t))
        //    {
        //        var items = reg.Split(t);

        //        if (items.Length != 6)
        //        {
        //            Console.WriteLine("[regex count:" + t + " ]-> " + s.Substring(0, 15));
        //        }
        //        else
        //        {
        //            var a = int.Parse(items[3].Trim());
        //            var b = int.Parse(items[4].Trim());

        //            if (a >= b)
        //                Console.WriteLine("[a >= b:" + items[1] + " ]-> " + s.Substring(0, 15));

        //        }
        //        //else if (!skills.Contains(items[1].Trim())
        //        //    && !attr.Contains(items[1].Trim())
        //        //    && !other.Any(o => o.StartsWith(items[1].Trim())))
        //        //{
        //        //    Console.WriteLine("[unknown:" + items[1] + " ]-> " + s.Substring(0, 15));
        //        //}
        //    }
        //    else if (t.Contains('('))
        //    {
        //        Console.WriteLine("[no regex:" + t + " ]-> " + s.Substring(0, 15));
        //    }

        //    //if (t.Contains('('))
        //    //{
        //    //    if (t.Count(x => x == ')') != 1)
        //    //        Console.WriteLine(t + " -> " + s);
        //    //}
        //}
        //}


        //foreach (var l in DarklandsService.Locations.Where(l => l.Icon == LocationIcon.City).OrderBy(l =>l.Coordinate.X))
        //{
        //    Console.WriteLine(l);
        //}

        //using (var save = new SaveGame("DKSAVE4.SAV"))
        ////using (var save = new SaveGame("DKSAVE11.SAV"))
        //{
        //    var name = save.Header.Label;

        //    var chars = save.Party.Characters;

        //    Console.WriteLine(save);

        //var chars = save.Party.Characters;

        //var money = save.Header.Money;
        //money.Florings++;
        //save.Header.Money = money;

        //var events = save.Events.Events;

        //foreach (var e in events)
        //{
        //    Console.Write(e);
        //    //if (e.Destination >= 0 && e.Destination < save.Events.Locations.Count)
        //    //{
        //    //    Console.Write("\n");
        //    //    Console.Write("\t(" + save.Events.Locations[e.Destination] + ")");
        //    //}
        //    //if (e.Destination >= 0 && e.Destination < DarklandsService.Locations.Count)
        //    //{
        //    //    Console.Write("\n");
        //    //    Console.Write("\t(" + DarklandsService.Locations[e.Destination] + ")");

        //    //}
        //    Console.Write("\n");
        //}

        //var locs = save.Events.Locations;

        //var vien = save.Events.Locations.First(l => l.Name.ToLower().StartsWith("wien"));

        //var vien2 = DarklandsService.Cities.First(c => c.LongName.ToLower().StartsWith("wien"));


        //foreach (var l in locs)
        //{
        //    Console.WriteLine(l);
        //}
        //save.Save("DKSAVE38_mod.SAV");
        //    }

        //    Console.ReadKey();
        //}

    }
}
