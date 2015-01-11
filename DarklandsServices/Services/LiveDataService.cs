using System;
using System.Collections.Generic;
using System.Linq;
using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Streaming;
using DarklandsBusinessObjects.Utils;
using DarklandsServices.Memory;

namespace DarklandsServices.Services
{
    public static class LiveDataService
    {
        private const int CharacterCount = 4; // maybe read this somewhere to be able to get hanse/schulz data as well?
        private const int SaintsSize = 8;
        private const int FormulaeSize = 40;
        private const int ScreenSize = 10; // guesstimate
        private static long _baseAddress;
        private static readonly List<IMemoryWorker> MemoryWorkers = new List<IMemoryWorker>();
        public static Action<bool> ConnectionMonitor;

        // offset = memory address address - baseAddress
        private const long SaintMonitorOffset = 0x40FEB;
        private const long FormulaeMonitorOffset = 0x40F0D;
        private const long KnownFormulaeOffset = 0x2EB3E;
        private const long KnownSaintsOffset = 0x2EBAC;
        private const long PartyOffset = 0x3BDF5;
        private const long DateOffset = 0x32280;
        private const long EventsOffset = 0x68630; // ?
        private const long CurrentScreenOffset = 0x40FFB; // alternative: 0x40583
        private const int NumberofCharacters = 4;

        // incomplete list of known screen ids (current screen shown to player)
        private static readonly IReadOnlyDictionary<string, ScreenType> ScreenMap
            = new Dictionary<string, ScreenType>
            {
                {"alcem2", ScreenType.Alchemist}
            };

        public static void Connect()
        {
            if (_baseAddress == 0)
            {
                NotifyConnectionStatus(false);

                var worker = new BaseAddressResolver(address =>
                {
                    _baseAddress = address;
                    Console.WriteLine("Base address resolved: " + _baseAddress.ToString("X"));

                    NotifyConnectionStatus(true);
                });

                worker.Start();

                MemoryWorkers.Add(worker);
            }
        }

        private static void CreateAndStartNewMonitor(long offset, int length, Action<byte[]> callback)
        {
            if (_baseAddress == 0)
            {
                throw new InvalidOperationException("Connect to Darkland process first!");
            }

            var worker = new MemorySectionMonitor(_baseAddress + offset, length, bytes => { callback(bytes); });

            worker.Start();
            MemoryWorkers.Add(worker);
        }

        public static void Stop()
        {
            var worker = MemoryWorkers.LastOrDefault();
            while (worker != null)
            {
                MemoryWorkers.Remove(worker);

                worker.Stop();

                worker = MemoryWorkers.LastOrDefault();
            }
        }

        private static void NotifyConnectionStatus(bool isConnected)
        {
            if (ConnectionMonitor != null)
            {
                ConnectionMonitor(isConnected);
            }
        }

        private static IEnumerable<UInt16> GetUInt32S(byte[] bytes)
        {
            var ints = new List<UInt16>();

            var i = 0;
            while (bytes != null && i + 1 < bytes.Length)
            {
                ints.Add(BitConverter.ToUInt16(bytes, i));
                i = i + 2;
            }

            return ints;
        }

        public static ScreenType GetScreen(string screenName)
        {
            if (ScreenMap.ContainsKey(screenName))
            {
                return ScreenMap[screenName];
            }

            return ScreenType.Unknown;
        }

        public static void MonitorFormulae(Action<IEnumerable<Formula>> callback)
        {
            CreateAndStartNewMonitor(FormulaeMonitorOffset, FormulaeSize*CharacterCount, bytes =>
            {
                if (bytes == null || bytes.All(b => b == 0))
                {
                    return;
                }

                callback(ToFormulaeList(bytes));
            });
        }

        public static IEnumerable<Formula> ReadAchemistRecipes()
        {
            if (_baseAddress == 0)
            {
                throw new InvalidOperationException("Connect to Darkland process first!");
            }

            using (var accessor = new MemoryAccessor("dosbox"))
            {
                var bytes = new byte[FormulaeSize*CharacterCount];
                if (accessor.ReadMemory(_baseAddress + FormulaeMonitorOffset, bytes))
                {
                    return ToFormulaeList(bytes);
                }
            }

            return Enumerable.Empty<Formula>();
        }

        private static IEnumerable<Formula> ToFormulaeList(byte[] bytes)
        {
            if (bytes == null
                || bytes.Length < CharacterCount*FormulaeSize
                || bytes.All(b => b == 0))
            {
                return Enumerable.Empty<Formula>();
            }

            var list = new List<Formula>();
            for (var i = 0; i < FormulaeSize*CharacterCount; i += FormulaeSize)
            {
                var name = StringHelper.ConvertToString(
                    bytes.Skip(i).Take(FormulaeSize).ToArray());

                var formula = StaticDataService.Formulae.FirstOrDefault(f =>
                    f.FullName == name);

                if (formula != null)
                {
                    list.Add(formula);
                }
            }

            return list;
        }

        public static void MonitorSaints(Action<IEnumerable<Saint>> callback)
        {
            CreateAndStartNewMonitor(SaintMonitorOffset, SaintsSize, bytes =>
            {
                if (bytes == null || bytes.All(b => b == 0))
                {
                    return;
                }

                var list = from id in GetUInt32S(bytes)
                           let saint = StaticDataService.Saints.FirstOrDefault(s => s.Id == id)
                           where saint != null
                           select saint;

                callback(list);
            });
        }

        public static void MonitorCurrentScreen(Action<string> callback)
        {
            CreateAndStartNewMonitor(CurrentScreenOffset, ScreenSize, bytes =>
            {
                if (bytes == null || bytes.All(b => b == 0))
                {
                    return;
                }

                var menu = StringHelper.ConvertToString(bytes);

                callback(menu);
            });
        }

        public static void MonitorDate(Action<Date> callback)
        {
            CreateAndStartNewMonitor(DateOffset, Date.DateSize, bytes =>
            {
                if (bytes == null || bytes.All(b => b == 0))
                {
                    return;
                }

                var date = new Date(new ByteStream(bytes), 0, false);
                callback(date);
            });
        }

        public static void MonitorMemory(long address, int length, Action<byte[]> callback)
        {
            var offset = address - _baseAddress;

            CreateAndStartNewMonitor(offset, length, bytes =>
            {
                if (bytes == null || bytes.All(b => b == 0))
                {
                    return;
                }

                callback(bytes);
            });
        }

        public static IEnumerable<FormulaeBitmask> ReadKnownFormulae()
        {
            if (_baseAddress == 0)
            {
                throw new InvalidOperationException("Connect to Darkland process first!");
            }

            var formulae = new List<FormulaeBitmask>(NumberofCharacters);
            using (var accessor = new MemoryAccessor("dosbox"))
            {
                for (var i = 0; i < NumberofCharacters; i++)
                {
                    var bytes = new byte[FormulaeBitmask.FormulaeBitmaskSize];
                    if (accessor.ReadMemory(
                        _baseAddress + KnownFormulaeOffset + i*FormulaeBitmask.FormulaeBitmaskSize, bytes))
                    {
                        formulae.Add(FormulaeBitmask.FromBytes(bytes));
                    }
                }
            }

            return formulae;
        }

        public static IEnumerable<SaintBitmask> ReadKnownSaints()
        {
            if (_baseAddress == 0)
            {
                throw new InvalidOperationException("Connect to Darkland process first!");
            }

            var saints = new List<SaintBitmask>(NumberofCharacters);
            using (var accessor = new MemoryAccessor("dosbox"))
            {
                for (var i = 0; i < NumberofCharacters; i++)
                {
                    var bytes = new byte[SaintBitmask.SaintBitmaskSize];
                    if (accessor.ReadMemory(
                        _baseAddress + KnownSaintsOffset + i*SaintBitmask.SaintBitmaskSize, bytes))
                    {
                        saints.Add(SaintBitmask.FromBytes(bytes));
                    }
                }
            }

            return saints;
        }

        public static IEnumerable<Character> ReadParty()
        {
            if (_baseAddress == 0)
            {
                throw new InvalidOperationException("Connect to Darkland process first!");
            }

            var knownSaints = ReadKnownSaints().ToList();
            var knownFormulae = ReadKnownFormulae().ToList();

            var party = new List<Character>(NumberofCharacters);

            using (var accessor = new MemoryAccessor("dosbox"))
            {
                for (var i = 0; i < NumberofCharacters; i++)
                {
                    var bytes = new byte[Character.CharacterSize];
                    if (accessor.ReadMemory(_baseAddress + PartyOffset + i*Character.CharacterSize, bytes))
                    {
                        var character = new Character(new ByteStream(bytes), 0, i);

                        if (knownFormulae.Count() > i)
                        {
                            character.FormulaeBitmask = knownFormulae.ElementAt(i);
                        }
                        if (knownSaints.Count() > i)
                        {
                            character.SaintBitmask = knownSaints.ElementAt(i);
                        }

                        party.Add(character);
                    }
                }
            }

            return party;
        }
    }
}