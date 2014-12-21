using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DarklandsBusinessObjects;
using DarklandsBusinessObjects.Utils;

namespace UnitTests.DarklandsBusinessObjectsTests
{
    [TestClass]
    public class ByteContainerTests
    {
        //[TestMethod]
        //public void TestGetString()
        //{
        //    var c = new ByteContainer(new byte[] { 0x41, 0x42, 0x43, 0x00 });
        //    Assert.AreEqual("ABC", c.ToString(0, 4));
        //    Assert.AreEqual("BC", c.ToString(1, 3));
        //    Assert.AreEqual("B", c.ToString(1, 1));
        //}

        //[TestMethod]
        //public void TestGetStrings()
        //{
        //    var c = new ByteContainer(new byte[] { 0x41, 0x42, 0x43, 0x00, 0x44, 0x00, 0x45, 0x46, 0x00 });
        //    int index = 0;
        //    var strs = c.GetNullDelimitedStrings(3, ref index);

        //    Assert.IsTrue(3 == strs.Length);
        //    Assert.AreEqual("ABC", strs[0]);
        //    Assert.AreEqual("D", strs[1]);
        //    Assert.AreEqual("EF", strs[2]);
        //}
    }
}
