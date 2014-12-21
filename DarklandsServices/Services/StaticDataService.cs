using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Streaming;
using DarklandsBusinessObjects.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsServices.Services
{
    public static class StaticDataService
    {
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
            public FileType Type { get; private set; }
            public string Name { get; private set; }
            public string FullPath { get; set; }

            public bool Exists { get { return File.Exists(FullPath); } }

            public RequiredFile(FileType type, string name)
            {
                Type = type;
                Name = name;
                FullPath = Path.Combine(Directory.GetCurrentDirectory(), Name);
            }
        }

        private static IReadOnlyList<RequiredFile> s_darklandFiles = new List<RequiredFile>
        {
            new RequiredFile (FileType.City, "darkland.cty"),
            new RequiredFile (FileType.CityDescriptions, "darkland.dsc"),
            new RequiredFile (FileType.List, "darkland.lst"),
            new RequiredFile (FileType.Alchemy, "darkland.alc"),
            new RequiredFile (FileType.Saint, "darkland.snt"),
            new RequiredFile (FileType.Location, "darkland.loc"),
        };

        private static List<City> s_cities = null;
        private static List<ItemDefinition> s_items = null;
        private static List<Saint> s_saints = null;
        private static List<Formula> s_formulae = null;
        private static List<Location> s_locations = null;

        public static IEnumerable<string> RequiredFiles
        {
            get
            {
                return s_darklandFiles.Select(f => f.Name);
            }
        }

        public static IReadOnlyList<City> Cities
        {
            get
            {
                if (s_cities == null)
                {
                    InitializeCty();
                }

                return s_cities;
            }
        }

        public static IReadOnlyList<Saint> Saints
        {
            get
            {
                if (s_saints == null)
                {
                    InitializeLst();
                }

                return s_saints;
            }
        }

        public static IList<Saint> FindSaints(IEnumerable<int> ids)
        {
            return (from s in Saints
                    where ids.Contains(s.Id)
                    select s).ToList();
        }

        public static IReadOnlyList<ItemDefinition> ItemDefinitions
        {
            get
            {
                if (s_items == null)
                {
                    InitializeLst();
                }

                return s_items;
            }
        }

        public static IEnumerable<ItemDefinition> FindItems(IEnumerable<int> ids)
        {
            return from i in ItemDefinitions
                   where ids.Contains(i.Id)
                   select i;
        }

        public static IReadOnlyList<Formula> Formulae
        {
            get
            {
                if (s_items == null)
                {
                    InitializeLst();
                }

                return s_formulae;
            }
        }

        public static IEnumerable<Formula> FindFormulae(IEnumerable<int> ids)
        {
            return from f in Formulae
                   where ids.Contains(f.Id)
                   select f;
        }

        // Note! These are initial 'states' of lacation when starting new game.
        // Check SaveEvent for up-to-date location as these can change as the 
        // game progresses. (RaubritterCastle -> CasteRuin when RB is killed for example)
        public static IReadOnlyList<Location> Locations
        {
            get
            {
                if (s_locations == null)
                {
                    InitializeLoc();
                }

                return s_locations;
            }
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
                foreach (var file in s_darklandFiles)
                {
                    file.FullPath = darklandFiles.FirstOrDefault(
                        f => f.ToLower().EndsWith(file.Name.ToLower()));
                }

                if (s_darklandFiles.Any(f => !f.Exists))
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
            var file = s_darklandFiles.FirstOrDefault(f => f.Type == fileType);
            if (!file.Exists)
            {
                throw new InvalidOperationException(
                    "Could not locate file " + file.FullPath);
            }

            return new BinaryReader(File.Open(file.FullPath, FileMode.Open));
        }

        private static void InitializeCty()
        {
            int numCities = 0;

            using (var file = OpenFile(FileType.City))
            {
                numCities = file.ReadByte();

                s_cities = new List<City>(numCities);

                for (int i = 0; i < numCities; i++)
                {
                    var data = new ByteStream(file.ReadBytes(City.CITY_SIZE));
                    s_cities.Add(new City(data, 0, i));
                }
            }

            var t = s_cities.First().CityData.PortDestinations;

            using (var file = OpenFile(FileType.CityDescriptions))
            {
                var falseNumCities = file.ReadByte(); // value is wrong ?

                for (int i = 0; i < numCities; i++)
                {
                    var desc = file.ReadBytes(City.DESCRIPTION_SIZE);
                    s_cities[i].Description = StringHelper.ConvertToString(desc);
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

                s_items = new List<ItemDefinition>(numItems);

                for (int i = 0; i < numItems; i++)
                {
                    var stream = new ByteStream(file.ReadBytes(ItemDefinition.ITEM_DEFINITION_SIZE));
                    s_items.Add(new ItemDefinition(stream, 0, i));
                }

                var restOfTheFile = file.ReadBytes((int)file.BaseStream.Length);
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
                s_formulae = new List<Formula>(numFormulae);

                for (int i = 0; i < numFormulae; i++)
                {
                    var stream = new ByteStream(file.ReadBytes(Formula.FORMULA_SIZE));
                    s_formulae.Add(new Formula(stream, 0, i, formulaeLongNames[i], formulaeShortNames[i]));
                }
            }
        }

        private static void InitializeSnt(string[] saintLongNames, string[] saintShortNames)
        {
            using (var file = OpenFile(FileType.Saint))
            {
                var numSaints = file.ReadByte();
                s_saints = new List<Saint>(numSaints);

                for (int i = 0; i < numSaints; i++)
                {
                    var desc = StringHelper.ConvertToString(
                        file.ReadBytes(Saint.DESCRIPTION_SIZE));

                    s_saints.Add(new Saint(
                        i, saintLongNames[i], saintShortNames[i], desc));
                }
            }
        }

        private static void InitializeLoc()
        {
            using (var file = OpenFile(FileType.Location))
            {
                var numLocs = file.ReadInt16();
                s_locations = new List<Location>(numLocs);

                for (int i = 0; i < numLocs; i++)
                {
                    var stream = new ByteStream(file.ReadBytes(Location.LOCATION_SIZE));
                    s_locations.Add(new Location(stream, 0, i));
                }
            }
        }
    }
}
