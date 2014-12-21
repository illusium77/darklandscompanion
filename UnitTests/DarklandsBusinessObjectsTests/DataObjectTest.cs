using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Streaming;

namespace UnitTests.DarklandsBusinessObjectsTests
{
    [TestClass]
    public class DataObjectTest
    {
        class TestDataObject : StreamObject
        {
            public TestDataObject(byte[] data)
                : base(new ByteStream(data), 0, data.Length)
            {
            }
        }

        [TestMethod]
        public void TestIndexer()
        {
            var data = new TestDataObject(new byte[] { 0, 1, 2, 3 });
            Assert.AreEqual(0, data[0]);

            data[0] = 9;
            Assert.AreEqual(9, data[0]);
        }

        [TestMethod]
        public void TestWord()
        {
            var data = new TestDataObject(new byte[] { 0x02, 0x00, 0xe8, 0x03 });
            Assert.AreEqual(2, data.GetWord(0x00));
            Assert.AreEqual(1000, data.GetWord(0x02));

            data.SetWord(0x00, 88);
            Assert.AreEqual(88, data.GetWord(0x00));
        }

        [TestMethod]
        public void TestString()
        {
            var data = new TestDataObject(new byte[] { 0x41, 0x42 });
            Assert.AreEqual("AB", data.GetString(0x00, 2));
        }
    }
}
