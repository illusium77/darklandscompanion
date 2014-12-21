using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DarklandsServices.Services;

namespace UnitTests.DarklandsServicesTests
{
    [TestClass]
    public class StaticDataServiceTests
    {
        [TestInitialize]
        public void SetPath()
        {
            StaticDataService.SetDarklandsPath(@"D:\Steam\SteamApps\common\Darklands");
        }

        [TestMethod]
        public void TestCityCount()
        {
            var cities = StaticDataService.Cities;

            Assert.AreEqual(0x5c, cities.Count);
        }

        [TestMethod]
        public void TestCity()
        {
            var city = StaticDataService.Cities.Where(cty => cty.Id == 57);
            //var city = StaticDataService.Cities.Where(c => !string.IsNullOrEmpty(c.Cathedral) && !string.IsNullOrEmpty(c.Fortress));

            Assert.AreEqual(1, city.Count());
            var c = city.First();

            Assert.AreEqual(c.Cathedral, "Dom");
            Assert.AreEqual(c.Church, "Neumünsterkirche");
            Assert.AreEqual(c.Description, "the leading city of a sprawling, independent bishopric");
            Assert.AreEqual(c.Fortress, "Marienberg");
            Assert.AreEqual(c.Inn, "Schiftbuerin");
            Assert.AreEqual(c.Kloster, "Kloster");
            Assert.AreEqual(c.Leader, "Prince-Bishop of Würzburg");
            Assert.AreEqual(c.LongName, "Würzburg");
            Assert.AreEqual(c.Market, "Marktplatz");
            Assert.AreEqual(c.Polit, "Domstrasse");
            Assert.AreEqual(c.ShortName, "Würzburg");
            Assert.AreEqual(c.University, "Universitat");
        }

        [TestMethod]
        public void TestCitiesHaveDescriptions()
        {
            var cities = StaticDataService.Cities;

            Assert.IsFalse(cities.Any(c => string.IsNullOrWhiteSpace(c.Description)));
        }

        [TestMethod]
        public void TestFormulaeCount()
        {
            var formulae = StaticDataService.Formulae;

            Assert.AreEqual(0x42 , formulae.Count);
        }

        [TestMethod]
        public void TestItemsCount()
        {
            var items = StaticDataService.ItemDefinitions;

            Assert.AreEqual(0xc8, items.Count);
        }

        [TestMethod]
        public void TestLocationsCount()
        {
            var locs = StaticDataService.Locations;

            Assert.AreEqual(0x19e, locs.Count);
        }

        [TestMethod]
        public void TestSaintsCount()
        {
            var saints = StaticDataService.Saints;

            Assert.AreEqual(0x88, saints.Count);
        }

    }
}
