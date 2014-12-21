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

            //knownSaintFromBytes = SaintBitmask.FromBytes(new byte[] { 0x00, 0x00, 0x00, 0x12, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }).Saints;
            //var knownSaints = (from s in DarklandsService.Saints
            //                  where s.FullName.ToLower().StartsWith("st.cosmas")
            //                  || s.FullName.ToLower().StartsWith("st.damian")
            //                  || s.FullName.ToLower().StartsWith("st.felix")
            //                  || s.FullName.ToLower().StartsWith("st.polycarp")
            //                  select s.Id).OrderBy(i => i).ToList();

            //Assert.AreEqual(knownSaintFromBytes.Count, knownSaints.Count);
            //Assert.IsTrue(knownSaints.SequenceEqual(knownSaintFromBytes));
        }
    }
}
