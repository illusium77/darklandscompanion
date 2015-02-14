using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Utils;
using System.Collections.Generic;
using DarklandsBusinessObjects.Streaming;

namespace UnitTests.DarklandsBusinessObjectsTests
{
    [TestClass]
    public class CoordinateUnitTests
    {
        private static Coordinate GetCoordinate(short x, short y)
        {
            var bytes = new List<byte>(Coordinate.CoordinateSize);
            bytes.AddRange(BitConverter.GetBytes(x));
            bytes.AddRange(BitConverter.GetBytes(y));

            return new Coordinate(new ByteStream(bytes.ToArray()), 0);
        }

        [TestMethod]
        public void TestFromBytes()
        {
            var b = new byte[] { 0x01, 0x00, 0x02, 0x00 };

            var c = new Coordinate(new ByteStream(b), 0);
            Assert.IsTrue(c.X == 1);
            Assert.IsTrue(c.Y == 2);
        }

        [TestMethod]
        public void TestEquals()
        {
            var a = GetCoordinate(1, 2);
            var b = GetCoordinate(1, 2);

            Assert.AreEqual(a, b);
            Assert.IsTrue(a == b); Assert.IsFalse(a != b);

            Assert.IsFalse(a == null); Assert.IsTrue(a != null);
            Assert.IsFalse(null == b); Assert.IsTrue(null != b);

            var c = GetCoordinate(2, 2);
            Assert.IsFalse(c == a); Assert.IsTrue(c != a);
            var d = GetCoordinate(1, 1);
            Assert.IsFalse(d == a); Assert.IsTrue(d != a);
            var e = GetCoordinate(3, 3);
            Assert.IsFalse(e == a); Assert.IsTrue(e != a);
        }

        [TestMethod]
        public void TestDistance()
        {
            var o = GetCoordinate(1, 1);

            var n = GetCoordinate(1, 2);
            Assert.AreEqual(1, o.DistanceTo(n));
            Assert.AreEqual(1, n.DistanceTo(o));

            n = GetCoordinate(1, 101);
            Assert.AreEqual(100, o.DistanceTo(n));
            Assert.AreEqual(100, n.DistanceTo(o));


            n = GetCoordinate(46, 46);
            Assert.AreEqual(64, o.DistanceTo(n));
            Assert.AreEqual(64, n.DistanceTo(o));
        }

        [TestMethod]
        public void TestBearing()
        {
            //var tomb =GetCoordinate(169, 490);
            //var leipzig =GetCoordinate(173, 462);

            //var bering = leipzig.BearingTo(tomb);


            var o = GetCoordinate(1, 1);

            var n = GetCoordinate(1, 0);
            var ne = GetCoordinate(2, 0);
            var e = GetCoordinate(2, 1);
            var se = GetCoordinate(2, 2);
            var s = GetCoordinate(1, 2);
            var sw = GetCoordinate(0, 2);
            var w = GetCoordinate(0, 1);
            var nw = GetCoordinate(0, 0);

            Assert.AreEqual(Bearing.N, o.BearingTo(n));
            Assert.AreEqual(Bearing.NE, o.BearingTo(ne));
            Assert.AreEqual(Bearing.E, o.BearingTo(e));
            Assert.AreEqual(Bearing.SE, o.BearingTo(se));
            Assert.AreEqual(Bearing.S, o.BearingTo(s));
            Assert.AreEqual(Bearing.SW, o.BearingTo(sw));
            Assert.AreEqual(Bearing.W, o.BearingTo(w));
            Assert.AreEqual(Bearing.NW, o.BearingTo(nw));

        }
    }
}
