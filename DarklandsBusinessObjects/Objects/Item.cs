using DarklandsBusinessObjects.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class Item : StreamObject
    {
        // https://web.archive.org/web/20091112194440/http://wallace.net/darklands/formats/structures.html#structdef-item

        public const int ITEM_SIZE = 0x06;

        public int Code { get { return GetWord(0x00); } }
        public int Type { get { return this[0x02]; } }
        public int Quality { get { return this[0x03]; } }
        public int Quantity { get { return this[0x04]; } }
        public int Weight { get { return this[0x05]; } }

        public bool IsEmpty
        {
            get
            {
                return Code == 0 && Type == 0 && Quality == 0 && Quantity == 0 && Weight == 0;
            }
        }

        public Item(ByteStream dataStream, int offset)
            : base(dataStream, offset, ITEM_SIZE)
        {
        }
    }
}
