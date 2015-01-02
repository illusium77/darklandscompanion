using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Utils;

namespace UnitTests.DarklandsBusinessObjectsTests
{
    [TestClass]
    public class SaintUnitTests
    {
        [TestMethod]
        public void TestGetKnownSaints()
        {
            var knownSaintFromBytes = SaintBitmask.FromBytes(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }).SaintIds;
            Assert.IsTrue(knownSaintFromBytes.Count == 0);

            knownSaintFromBytes = SaintBitmask.FromBytes(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }).SaintIds;
            Assert.IsTrue(knownSaintFromBytes.Count == 1);
            Assert.IsTrue(knownSaintFromBytes.First() == 0);

            knownSaintFromBytes = SaintBitmask.FromBytes(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }).SaintIds;
            Assert.IsTrue(knownSaintFromBytes.Count == 1);
            Assert.IsTrue(knownSaintFromBytes.First() == 159);

            var saints = SaintBitmask.FromBytes(new byte[] { 0x00, 0xFF, 0x00, 0x12, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            saints.ForgetSaint(10);
            Assert.IsFalse(saints.HasSaint(10));
            saints.ForgetSaint(10);
            Assert.IsFalse(saints.HasSaint(10));
            saints.LearnSaint(10);
            Assert.IsTrue(saints.HasSaint(10));
            saints.LearnSaint(10);
            Assert.IsTrue(saints.HasSaint(10));
        }
    }
}
