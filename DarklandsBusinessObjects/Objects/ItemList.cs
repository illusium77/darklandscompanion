using DarklandsBusinessObjects.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class ItemList : StreamObject
    {
        private const int NUMBER_OF_ITEMS = 64;
        public const int ITEM_LIST_SIZE = NUMBER_OF_ITEMS * Item.ITEM_SIZE;

        private IReadOnlyList<Item> m_items;
        public IReadOnlyList<Item> Items
        {
            get
            {
                if (m_items == null)
                {
                    var items = new List<Item>();
                    for (int i = 0; i < NUMBER_OF_ITEMS; i++)
                    {
                        items.Add(new Item(DataStream, BaseOffset + i * Item.ITEM_SIZE));
                    }

                    m_items = items;
                }
                return m_items;
            }
        }

        public int Count
        {
            get
            {
                return Items.Count(i => !i.IsEmpty);
            }
        }

        public ItemList(ByteStream dataStream, int offset)
            : base(dataStream, offset, ITEM_LIST_SIZE)
        {
        }

        public override string ToString()
        {
            return "['#" + Count
                + "']";
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (m_items != null)
                {
                    foreach (var item in m_items)
                    {
                        item.Dispose();
                    }

                    m_items = null;
                }
            }
        }

    }
}
