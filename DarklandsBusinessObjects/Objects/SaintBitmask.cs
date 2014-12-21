using DarklandsBusinessObjects.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class SaintBitmask : StreamObject
    {
        public const int SAINT_BITMASK_SIZE = 20;

        private IList<int> m_saintIds;
        public IList<int> SaintIds
        {
            get
            {
                if (m_saintIds == null)
                {
                    m_saintIds = FromBitmask();

                }
                return m_saintIds;
            }
        }

        public SaintBitmask(ByteStream dataStream, int offset)
            : base(dataStream, offset, SAINT_BITMASK_SIZE)
        {
        }

        public override string ToString()
        {
            return "['#" + SaintIds.Count
                + "' '" + string.Join(", ", SaintIds)
                + "']";
        }

        private IList<int> FromBitmask()
        {
            var saints = new List<int>();
            int count = 0;
            for (int i = 0; i < SAINT_BITMASK_SIZE; i++)
            {
                var mask = 0x80;

                for (int j = 0; j < 8; j++)
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
            if (bytes == null || bytes.Length - offset < SAINT_BITMASK_SIZE)
            {
                throw new ArgumentException("Invalid bitmask", "bitmask");
            }

            return new SaintBitmask(new ByteStream(bytes), offset);
        }
    }
}
