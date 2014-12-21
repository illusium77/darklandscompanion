using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Streaming;
using DarklandsBusinessObjects.Utils;
using DarklandsServices.Memory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsServices.Services
{
    public static class LiveDataService
    {
        private const int CHARACTER_COUNT = 4; // maybe read this somewhere to be able to get hanse/schulz data as well?
        private const int SAINTS_SIZE = 8;
        private const int FORMULAE_SIZE = 40;
        private const int SCREEN_SIZE = 10; // guesstimate

        private static long s_baseAddress = 0;

        private static List<IMemoryWorker> s_memoryWorkers = new List<IMemoryWorker>();
        //private static BaseAddressResolver s_resolver;
        //private static MemorySectionMonitor s_saintMonitor;
        //private static MemorySectionMonitor s_formulaMonitor;
        //private static MemorySectionMonitor s_screenMonitor;

        public static Action<bool> ConnectionMonitor;

        // offset = memory address address - baseAddress
        private static long s_saintMonitorOffset = 0x40FEB;
        private static long s_formulaeMonitorOffset = 0x40F0D;
        private static long s_knownFormulaeOffset = 0x2EB3E;
        private static long s_knownSaintsOffset = 0x2EBAC;
        private static long s_partyOffset = 0x3BDF5;
        private static long s_currentScreenOffset = 0x40FFB; // alternative: 0x40583

        private static IReadOnlyDictionary<string, ScreenType> s_screenMap
            = new Dictionary<string, ScreenType>
        {
            {"alcem2", ScreenType.Alchemist}
        };

        public static void Connect()
        {
            if (s_baseAddress == 0)
            {
                NotifyConnectionStatus(false);

                var worker = new BaseAddressResolver(address =>
                {
                    s_baseAddress = address;
                    Console.WriteLine("Base address resolved: " + s_baseAddress.ToString("X"));

                    NotifyConnectionStatus(true);
                });

                worker.Start();

                s_memoryWorkers.Add(worker);
            }
        }

        private static void CreateAndStartNewMonitor(long offset, int length, Action<byte[]> callback)
        {
            if (s_baseAddress == 0)
            {
                throw new InvalidOperationException("Connect to Darkland process first!");
            }

            var worker = new MemorySectionMonitor(s_baseAddress + offset, length, bytes =>
            {
                callback(bytes);
            });

            worker.Start();
            s_memoryWorkers.Add(worker);
        }

        public static void Stop()
        {
            var worker = s_memoryWorkers.LastOrDefault();
            while (worker != null)
            {
                s_memoryWorkers.Remove(worker);

                worker.Stop();
                worker = null;

                worker = s_memoryWorkers.LastOrDefault();
            }
        }

        private static void NotifyConnectionStatus(bool isConnected)
        {
            if (ConnectionMonitor != null)
            {
                ConnectionMonitor(isConnected);
            }
        }

        private static IEnumerable<UInt16> GetUInt32s(byte[] bytes)
        {
            var ints = new List<UInt16>();

            int i = 0;
            while (bytes != null && i + 1 < bytes.Length)
            {
                ints.Add(BitConverter.ToUInt16(bytes, i));
                i = i + 2;
            }

            return ints;
        }

        public static ScreenType GetScreen(string screenName)
        {
            if (s_screenMap.ContainsKey(screenName))
            {
                return s_screenMap[screenName];
            }

            return ScreenType.Unknown;
        }

        public static void MonitorFormulae(Action<IEnumerable<Formula>> callback)
        {
            CreateAndStartNewMonitor(s_formulaeMonitorOffset, FORMULAE_SIZE * CHARACTER_COUNT, bytes =>
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
            if (s_baseAddress == 0)
            {
                throw new InvalidOperationException("Connect to Darkland process first!");
            }

            using (var accessor = new MemoryAccessor("dosbox"))
            {
                var bytes = new byte[FORMULAE_SIZE * CHARACTER_COUNT];
                if (accessor.ReadMemory(s_baseAddress + s_formulaeMonitorOffset, bytes))
                {
                    return ToFormulaeList(bytes);
                }
            }

            return Enumerable.Empty<Formula>();
        }

        private static IEnumerable<Formula> ToFormulaeList(byte[] bytes)
        {
            if (bytes == null
                || bytes.Length < CHARACTER_COUNT * FORMULAE_SIZE
                || bytes.All(b => b == 0))
            {
                return Enumerable.Empty<Formula>();
            }

            var list = new List<Formula>();
            for (int i = 0; i < FORMULAE_SIZE * CHARACTER_COUNT; i += FORMULAE_SIZE)
            {
                var name = StringHelper.ConvertToString(
                    bytes.Skip(i).Take(FORMULAE_SIZE));

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
            CreateAndStartNewMonitor(s_saintMonitorOffset, SAINTS_SIZE, bytes =>
                {
                    if (bytes == null || bytes.All(b => b == 0))
                    {
                        return;
                    }

                    var list = new List<Saint>();

                    foreach (var id in GetUInt32s(bytes))
                    {
                        var saint = StaticDataService.Saints.FirstOrDefault(s => s.Id == id);
                        if (saint != null)
                        {
                            list.Add(saint);
                        }
                    }

                    callback(list);
                });
        }

        public static void MonitorCurrentScreen(Action<string> callback)
        {
            CreateAndStartNewMonitor(s_currentScreenOffset, SCREEN_SIZE, bytes =>
                {
                    if (bytes == null || bytes.All(b => b == 0))
                    {
                        return;
                    }

                    var menu = StringHelper.ConvertToString(bytes);

                    callback(menu);
                });
        }

        public static IEnumerable<FormulaeBitmask> ReadKnownFormulae()
        {
            if (s_baseAddress == 0)
            {
                throw new InvalidOperationException("Connect to Darkland process first!");
            }

            var numChar = 4;

            var formulae = new List<FormulaeBitmask>(numChar);
            using (var accessor = new MemoryAccessor("dosbox"))
            {
                for (int i = 0; i < numChar; i++)
                {
                    var bytes = new byte[FormulaeBitmask.FORMULAE_BITMASK_SIZE];
                    if (accessor.ReadMemory(
                        s_baseAddress + s_knownFormulaeOffset + i * FormulaeBitmask.FORMULAE_BITMASK_SIZE, bytes))
                    {
                        formulae.Add(FormulaeBitmask.FromBytes(bytes));
                    }
                }
            }

            return formulae;
        }

        public static IEnumerable<SaintBitmask> ReadKnownSaints()
        {
            if (s_baseAddress == 0)
            {
                throw new InvalidOperationException("Connect to Darkland process first!");
            }

            var numChar = 4;

            var saints = new List<SaintBitmask>(numChar);
            using (var accessor = new MemoryAccessor("dosbox"))
            {
                for (int i = 0; i < numChar; i++)
                {
                    var bytes = new byte[SaintBitmask.SAINT_BITMASK_SIZE];
                    if (accessor.ReadMemory(
                        s_baseAddress + s_knownSaintsOffset + i * SaintBitmask.SAINT_BITMASK_SIZE, bytes))
                    {
                        var saintIds = SaintBitmask.FromBytes(bytes).SaintIds;
                        saints.Add(SaintBitmask.FromBytes(bytes));
                    }
                }
            }

            return saints;
        }

        public static IEnumerable<Character> ReadParty()
        {
            if (s_baseAddress == 0)
            {
                throw new InvalidOperationException("Connect to Darkland process first!");
            }

            var numChar = 4;

            var knownSaints = LiveDataService.ReadKnownSaints();
            var knownFormulae = LiveDataService.ReadKnownFormulae();

            var party = new List<Character>(numChar);

            using (var accessor = new MemoryAccessor("dosbox"))
            {
                for (int i = 0; i < numChar; i++)
                {
                    var bytes = new byte[Character.CHARACTER_SIZE];
                    if (accessor.ReadMemory(s_baseAddress + s_partyOffset + i * Character.CHARACTER_SIZE, bytes))
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
