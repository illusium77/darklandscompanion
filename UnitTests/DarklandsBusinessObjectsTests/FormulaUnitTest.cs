using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DarklandsBusinessObjects.Objects;

namespace UnitTests.DarklandsBusinessObjectsTests
{
    [TestClass]
    public class FormulaUnitTest
    {
        [TestMethod]
        public void TestGetKnownFormulaeIds()
        {
            var bitmask = new byte[] { 0x00, 0x00, 0x05, 0x00, 0x05, 0x04, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x02, 0x00, 0x00 };

            var formulae = FormulaeBitmask.FromBytes(bitmask).FormulaeIds;

            Assert.AreEqual(9, formulae.Count());

            Assert.IsTrue(formulae.Contains(0x06));
            Assert.IsTrue(formulae.Contains(0x08));
            Assert.IsTrue(formulae.Contains(0x0c));
            Assert.IsTrue(formulae.Contains(0x0e));
            Assert.IsTrue(formulae.Contains(0x11));
            Assert.IsTrue(formulae.Contains(0x13));
            Assert.IsTrue(formulae.Contains(0x14));
            Assert.IsTrue(formulae.Contains(0x38));
            Assert.IsTrue(formulae.Contains(0x3a ));
        }
    }
}
