using DarklandsBusinessObjects.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class FormulaeBitmask : StreamObject
    {
        public const int FORMULAE_BITMASK_SIZE = 22;

        private static byte[] QL_MASKS = new byte[]
        {
            0x01, // QL25
            0x02, // QL35
            0x04, // QL45
        };

        private IList<int> m_formulaeIds;
        public IList<int> FormulaeIds
        {
            get
            {
                if (m_formulaeIds == null)
                {
                    m_formulaeIds = FromBitmask();

                }
                return m_formulaeIds;
            }
        }

        public FormulaeBitmask(ByteStream dataStream, int offset)
            : base(dataStream, offset, FORMULAE_BITMASK_SIZE)
        {
        }

        public bool HasFormula(int id)
        {
            var mask = QL_MASKS[id % 3];
            return (this[id / 3] & mask) == mask;
        }

        public void LearnFormula(int id)
        {
            var mask = QL_MASKS[id % 3];

            var b = this[id / 3];
            var a = b | mask;

            this[id / 3] |= mask;
        }

        public void ForgetFormula(int id)
        {
            var mask = ~QL_MASKS[id % 3];

            this[id / 3] &= mask;
        }

        public override string ToString()
        {
            return "['#" + FormulaeIds.Count
                +  "' '" + string.Join(", ", FormulaeIds)
                + "']";
        }
        
        private IList<int> FromBitmask()
        {
            // each byte represent one type of formula (noxorious aroma, black cloud), masks will tell which QL recipe is known.
            var formulae = new List<int>();
            for (int i = 0; i < FORMULAE_BITMASK_SIZE; i++)
            {
                for (int j = 0; j < QL_MASKS.Length; j++)
                {
                    if ((QL_MASKS[j] & this[i]) == QL_MASKS[j])
                    {
                        formulae.Add(i * 3 + j);
                    }
                }
            }

            return formulae;
        }

        public static FormulaeBitmask FromBytes(byte[] bytes, int offset = 0)
        {
            if (bytes == null || bytes.Length - offset < FORMULAE_BITMASK_SIZE)
            {
                throw new ArgumentException("Invalid bitmask", "bitmask");
            }

            return new FormulaeBitmask(new ByteStream(bytes), offset);
        }
    }
}
