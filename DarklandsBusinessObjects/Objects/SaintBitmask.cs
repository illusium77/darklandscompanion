using System;
using System.Collections.Generic;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class SaintBitmask : StreamObject
    {
        public const int SaintBitmaskSize = 20;
        private IList<int> _saintIds;

        public SaintBitmask(ByteStream dataStream, int offset)
            : base(dataStream, offset, SaintBitmaskSize)
        {
        }

        public IList<int> SaintIds
        {
            get { return _saintIds ?? (_saintIds = FromBitmask()); }
        }

        public bool HasSaint(int id)
        {
            var maskByte = id/8;
            var mask = 0x80 >> (id%8);

            return (this[maskByte] & mask) == mask;
        }

        public void LearnSaint(int id)
        {
            var maskByte = id/8;
            var bitmask = 0x80 >> (id%8);

            this[maskByte] |= bitmask;
        }

        public void ForgetSaint(int id)
        {
            var maskByte = id/8;
            var bitmask = ~(0x80 >> (id%8));

            this[maskByte] &= bitmask;
        }

        //public void FlipSaint(int id)
        //{
        //    var maskByte = id / 8;
        //    var bit = 0x80 >> (id % 8);

        //    this[maskByte] ^= bit;
        //}

        public override string ToString()
        {
            return "['#" + SaintIds.Count
                   + "' '" + string.Join(", ", SaintIds)
                   + "']";
        }

        private IList<int> FromBitmask()
        {
            var saints = new List<int>();
            var count = 0;
            for (var i = 0; i < SaintBitmaskSize; i++)
            {
                var mask = 0x80;

                for (var j = 0; j < 8; j++)
                {
                    if ((mask & this[i]) == mask)
                    {
                        saints.Add(count);
                    }

                    mask = mask >> 1;
                    count++;
                }
            }

            return saints;
        }

        public static SaintBitmask FromBytes(byte[] bytes, int offset = 0)
        {
            if (bytes == null || bytes.Length - offset < SaintBitmaskSize)
            {
                throw new ArgumentException("Invalid bitmask", "bytes");
            }

            return new SaintBitmask(new ByteStream(bytes), offset);
        }
    }
}