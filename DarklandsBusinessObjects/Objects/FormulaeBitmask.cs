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

        public override string ToString()
        {
            return "['#" + FormulaeIds.Count
                +  "' '" + string.Join(", ", FormulaeIds)
                + "']";
        }
        
        private IList<int> FromBitmask()
        {
            var maskQ25 = 0x01;
            var maskQ35 = 0x02;
            var maskQ45 = 0x04;

            var formulae = new List<int>();
            for (int i = 0; i < FORMULAE_BITMASK_SIZE; i++)
            {
                if ((maskQ25 & this[i]) == maskQ25)
                {
                    formulae.Add(i * 3);
                }
                if ((maskQ35 & this[i]) == maskQ35)
                {
                    formulae.Add(i * 3 + 1);
                }
                if ((maskQ45 & this[i]) == maskQ45)
                {
                    formulae.Add(i * 3 + 2);
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
