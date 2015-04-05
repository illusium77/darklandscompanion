using System.Collections.Generic;
using System.Linq;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class ItemList : StreamObject
    {
        private const int NumberOfItems = 64;
        private const int ItemListSize = NumberOfItems * Item.ItemSize;
        private IReadOnlyList<Item> _items;

        public ItemList(ByteStream dataStream, int offset)
            : base(dataStream, offset, ItemListSize)
        {
        }

        public IReadOnlyList<Item> Items
        {
            get
            {
                if (_items == null)
                {
                    var items = new List<Item>();
                    for (var i = 0; i < NumberOfItems; i++)
                    {
                        items.Add(new Item(DataStream, BaseOffset + i*Item.ItemSize));
                    }

                    _items = items;
                }
                return _items;
            }
        }

        public int Count
        {
            get { return Items.Count(i => !i.IsEmpty); }
        }

        public void Clear()
        {
            for (var offset = BaseOffset; offset < BaseOffset + ItemListSize; offset++)
            {
                this[offset] = 0;
            }
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
                if (_items != null)
                {
                    foreach (var item in _items)
                    {
                        item.Dispose();
                    }

                    _items = null;
                }
            }
        }
    }
}