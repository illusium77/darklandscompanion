using System;
using System.Collections.Generic;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class FormulaeBitmask : StreamObject
    {
        public const int FormulaeBitmaskSize = 22;

        private static readonly byte[] QlMasks =
        {
            0x01, // QL25
            0x02, // QL35
            0x04 // QL45
        };

        private IList<int> _formulaeIds;

        public FormulaeBitmask(ByteStream dataStream, int offset)
            : base(dataStream, offset, FormulaeBitmaskSize)
        {
        }

        public IList<int> FormulaeIds
        {
            get { return _formulaeIds ?? (_formulaeIds = FromBitmask()); }
        }

        public bool HasFormula(int id)
        {
            var mask = QlMasks[id%3];
            return (this[id/3] & mask) == mask;
        }

        public void LearnFormula(int id)
        {
            var mask = QlMasks[id%3];
            this[id/3] |= mask;
        }

        public void ForgetFormula(int id)
        {
            var mask = ~QlMasks[id%3];

            this[id/3] &= mask;
        }

        public override string ToString()
        {
            return "['#" + FormulaeIds.Count
                   + "' '" + string.Join(", ", FormulaeIds)
                   + "']";
        }

        private IList<int> FromBitmask()
        {
            // each byte represent one type of formula (noxorious aroma, black cloud), masks will tell which QL recipe is known.
            var formulae = new List<int>();
            for (var i = 0; i < FormulaeBitmaskSize; i++)
            {
                for (var j = 0; j < QlMasks.Length; j++)
                {
                    if ((QlMasks[j] & this[i]) == QlMasks[j])
                    {
                        formulae.Add(i*3 + j);
                    }
                }
            }

            return formulae;
        }

        public static FormulaeBitmask FromBytes(byte[] bytes, int offset = 0)
        {
            if (bytes == null || bytes.Length - offset < FormulaeBitmaskSize)
            {
                throw new ArgumentException("Invalid bitmask", "bytes");
            }

            return new FormulaeBitmask(new ByteStream(bytes), offset);
        }
    }
}