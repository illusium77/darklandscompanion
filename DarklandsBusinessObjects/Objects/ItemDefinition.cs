using System.Collections.Generic;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class ItemDefinition : StreamObject
    {
        public const int ItemDefinitionSize = 0x2e;

        public ItemDefinition(ByteStream dataStream, int offset, int id)
            : base(dataStream, offset, ItemDefinitionSize)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public string Name
        {
            get { return GetString(0x00, 20); }
        }

        public string ShortName
        {
            get { return GetString(0x14, 10); }
        }

        public int Type
        {
            get { return GetWord(0x14); }
        }

        public ItemMaskA MaskA
        {
            get { return (ItemMaskA) this[0x20]; }
        }

        public ItemMaskB MaskB
        {
            get { return (ItemMaskB) this[0x21]; }
        }

        public ItemMaskC MaskC
        {
            get { return (ItemMaskC) this[0x22]; }
        }

        public ItemMaskD MaskD
        {
            get { return (ItemMaskD) this[0x23]; }
        }

        public ItemMaskE MaskE
        {
            get { return (ItemMaskE) this[0x24]; }
        }

        public int Weight
        {
            get { return this[0x25]; }
        }

        public int DefaultQualit
        {
            get { return this[0x26]; }
        }

        public int UnknownW28
        {
            get { return GetWord(0x28); }
        }

        public int UnknownW2A
        {
            get { return GetWord(0x2a); }
        }

        public int Value
        {
            get { return GetWord(0x2c); }
        }

        public bool IsQuestItem
        {
            get
            {
                return MaskB.HasFlag(ItemMaskB.IsQuestIndoor)
                       || MaskC.HasFlag(ItemMaskC.IsQuestOutdoor);
            }
        }

        public override string ToString()
        {
            var masks = new List<string>();
            if (MaskA > 0)
            {
                masks.Add(MaskA.ToString());
            }
            if (MaskB > 0)
            {
                masks.Add(MaskB.ToString());
            }
            if (MaskC > 0)
            {
                masks.Add(MaskC.ToString());
            }
            if (MaskD > 0)
            {
                masks.Add(MaskD.ToString());
            }
            if (MaskE > 0)
            {
                masks.Add(MaskE.ToString());
            }

            return "'0x" + Id.ToString("x")
                   + "' '" + Name
                   + "' '" + ShortName
                   + "' '" + string.Join(", ", masks.ToArray())
                   + "'";
        }
    }
}