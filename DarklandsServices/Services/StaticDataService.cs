using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Streaming;
using DarklandsBusinessObjects.Utils;
using DarklandsServices.Saints;

namespace DarklandsServices.Services
{
    public static class StaticDataService
    {
        private static readonly IReadOnlyList<RequiredFile> DarklandFiles = new List<RequiredFile>
        {
            new RequiredFile(FileType.City, "darkland.cty"),
            new RequiredFile(FileType.CityDescriptions, "darkland.dsc"),
            new RequiredFile(FileType.List, "darkland.lst"),
            new RequiredFile(FileType.Alchemy, "darkland.alc"),
            new RequiredFile(FileType.Saint, "darkland.snt"),
            new RequiredFile(FileType.Location, "darkland.loc")
        };

        private static List<City> _cities;
        private static List<ItemDefinition> _items;
        private static List<Saint> _saints;
        private static List<Formula> _formulae;
        private static List<Location> _locations;

        public static IEnumerable<string> RequiredFiles
        {
            get { return DarklandFiles.Select(f => f.Name); }
        }

        public static IReadOnlyList<City> Cities
        {
            get
            {
                if (_cities == null)
                {
                    InitializeCty();
                }

                return _cities;
            }
        }

        public static IReadOnlyList<Saint> Saints
        {
            get
            {
                if (_saints == null)
                {
                    InitializeLst();
                }

                return _saints;
            }
        }

        public static IReadOnlyList<ItemDefinition> ItemDefinitions
        {
            get
            {
                if (_items == null)
                {
                    InitializeLst();
                }

                return _items;
            }
        }

        public static IReadOnlyList<Formula> Formulae
        {
            get
            {
                if (_items == null)
                {
                    InitializeLst();
                }

                return _formulae;
            }
        }

        // Note! These are initial 'states' of lacation when starting new game.
        // Check SaveEvent for up-to-date location as these can change as the 
        // game progresses. (RaubritterCastle -> CasteRuin when RB is killed for example)
        public static IReadOnlyList<Location> Locations
        {
            get
            {
                if (_locations == null)
                {
                    InitializeLoc();
                }

                return _locations;
            }
        }

        public static IList<Saint> FindSaints(IEnumerable<int> ids)
        {
            return (from s in Saints
                where ids.Contains(s.Id)
                select s).ToList();
        }

        public static IEnumerable<Saint> FilterSaints(SaintBuffFilter buffFilter, IEnumerable<int> idFilter)
        {
            var saints = idFilter == null
                ? Saints
                : from s in Saints
                    where idFilter.Contains(s.Id)
                    select s;

            return from s in saints
                where s.HasBuff(buffFilter.Name)
                select s;
        }

        public static IEnumerable<ItemDefinition> FindItems(IEnumerable<int> ids)
        {
            return from i in ItemDefinitions
                where ids.Contains(i.Id)
                select i;
        }

        public static IEnumerable<Formula> FindFormulae(IEnumerable<int> ids)
        {
            return from f in Formulae
                where ids.Contains(f.Id)
                select f;
        }

        public static IEnumerable<Location> FindLocations(IEnumerable<int> ids)
        {
            return from l in Locations
                where ids.Contains(l.Id)
                select l;
        }

        public static bool SetDarklandsPath(string path)
        {
            var searchPath = !string.IsNullOrEmpty(path) ? path : Directory.GetCurrentDirectory();
            if (!Directory.Exists(searchPath))
            {
                return false;
            }

            try
            {
                var darklandFiles = Directory.GetFiles(searchPath, "darkland.*", SearchOption.AllDirectories);
                foreach (var file in DarklandFiles)
                {
                    file.FullPath = darklandFiles.FirstOrDefault(
                        f => f.ToLower().EndsWith(file.Name.ToLower()));
                }

                if (DarklandFiles.Any(f => !f.Exists))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static BinaryReader OpenFile(FileType fileType)
        {
            var file = DarklandFiles.FirstOrDefault(f => f.Type == fileType);
            if (file == null)
            {
                throw new InvalidOperationException(
                    "Invalid file type " + fileType);
            }

            if (!file.Exists)
            {
                throw new InvalidOperationException(
                    "Could not locate file " + file.FullPath);
            }

            return new BinaryReader(File.Open(file.FullPath, FileMode.Open));
        }

        private static void InitializeCty()
        {
            int numCities;

            using (var file = OpenFile(FileType.City))
            {
                numCities = file.ReadByte();

                _cities = new List<City>(numCities);

                for (var i = 0; i < numCities; i++)
                {
                    var data = new ByteStream(file.ReadBytes(City.CitySize));
                    _cities.Add(new City(data, 0, i));
                }
            }

            using (var file = OpenFile(FileType.CityDescriptions))
            {
                file.ReadByte(); // 1st byte is the number of cities but value seems to be incorrect

                for (var i = 0; i < numCities; i++)
                {
                    var desc = file.ReadBytes(City.DescriptionSize);
                    _cities[i].Description = StringHelper.ConvertToString(desc);
                }
            }
        }

        private static void InitializeLst()
        {
            using (var file = OpenFile(FileType.List))
            {
                var numItems = file.ReadByte();
                var numSaints = file.ReadByte();
                var numFormulae = file.ReadByte();

                _items = new List<ItemDefinition>(numItems);

                for (var i = 0; i < numItems; i++)
                {
                    var stream = new ByteStream(file.ReadBytes(ItemDefinition.ItemDefinitionSize));
                    _items.Add(new ItemDefinition(stream, 0, i));
                }

                var restOfTheFile = file.ReadBytes((int) file.BaseStream.Length);
                var startIndex = 0;

                var saintLongNames = StringHelper.GetNullDelimitedStrings(
                    restOfTheFile, numSaints, ref startIndex);
                var saintShortNames = StringHelper.GetNullDelimitedStrings(
                    restOfTheFile, numSaints, ref startIndex);
                InitializeSnt(saintLongNames, saintShortNames);

                var formulaeLongNames = StringHelper.GetNullDelimitedStrings(
                    restOfTheFile, numFormulae, ref startIndex);
                var formulaeShortNames = StringHelper.GetNullDelimitedStrings(
                    restOfTheFile, numFormulae, ref startIndex);
                InitializeAlc(formulaeLongNames, formulaeShortNames);
            }
        }

        private static void InitializeAlc(string[] formulaeLongNames, string[] formulaeShortNames)
        {
            using (var file = OpenFile(FileType.Alchemy))
            {
                var numFormulae = file.ReadByte();
                _formulae = new List<Formula>(numFormulae);

                for (var i = 0; i < numFormulae; i++)
                {
                    var stream = new ByteStream(file.ReadBytes(Formula.FormulaSize));
                    _formulae.Add(new Formula(stream, 0, i, formulaeLongNames[i], formulaeShortNames[i]));
                }
            }
        }

        private static void InitializeSnt(string[] saintLongNames, string[] saintShortNames)
        {
            using (var file = OpenFile(FileType.Saint))
            {
                var numSaints = file.ReadByte();
                _saints = new List<Saint>(numSaints);

                for (var i = 0; i < numSaints; i++)
                {
                    var desc = StringHelper.ConvertToString(
                        file.ReadBytes(Saint.DescriptionSize));

                    var clue = SaintClues.GetClueById(i);
                    var buffs = SaintBuffManager.GenerateBuffsFromClue(clue);

                    _saints.Add(new Saint(
                        i, saintLongNames[i], saintShortNames[i], desc, clue, buffs));
                }
            }
        }

        private static void InitializeLoc()
        {
            using (var file = OpenFile(FileType.Location))
            {
                var numLocs = file.ReadInt16();
                _locations = new List<Location>(numLocs);

                for (var i = 0; i < numLocs; i++)
                {
                    var stream = new ByteStream(file.ReadBytes(Location.LocationSize));
                    _locations.Add(new Location(stream, 0, i));
                }
            }
        }

        //private static string s_darklandsFolder = @"Dependencies\";

        private enum FileType
        {
            City,
            CityDescriptions,
            List,
            Alchemy,
            Saint,
            Location
        }

        private class RequiredFile
        {
            public RequiredFile(FileType type, string name)
            {
                Type = type;
                Name = name;
                FullPath = Path.Combine(Directory.GetCurrentDirectory(), Name);
            }

            public FileType Type { get; private set; }
            public string Name { get; private set; }
            public string FullPath { get; set; }

            public bool Exists
            {
                get { return File.Exists(FullPath); }
            }
        }
    }
}