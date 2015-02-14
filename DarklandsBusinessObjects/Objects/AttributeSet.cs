using System.ComponentModel.DataAnnotations;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    //public enum ShortAttributes
    //{
    //    End,
    //    Str,
    //    Agl,
    //    Per,
    //    Int,
    //    Chr,
    //    Df
    //}

    public class AttributeSet : StreamObject
    {
        // https://web.archive.org/web/20091112194440/http://wallace.net/darklands/formats/structures.html#structdef-attribute_set

        private const int AttributeSetSize = 0x07;

        public AttributeSet(ByteStream data, int offset)
            : base(data, offset, AttributeSetSize)
        {
        }

        [Range(1, 99)]
        public int Endurance
        {
            get { return this[0x00]; }
            set
            {
                this[0x00] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Strength
        {
            get { return this[0x01]; }
            set
            {
                this[0x01] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Agility
        {
            get { return this[0x02]; }
            set
            {
                this[0x02] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Perception
        {
            get { return this[0x03]; }
            set
            {
                this[0x03] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Intelligence
        {
            get { return this[0x04]; }
            set
            {
                this[0x04] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int Charisma
        {
            get { return this[0x05]; }
            set
            {
                this[0x05] = value;
                NotifyPropertyChanged();
            }
        }

        [Range(1, 99)]
        public int DivineFavor
        {
            get { return this[0x06]; }
            set
            {
                this[0x06] = value;
                NotifyPropertyChanged();
            }
        }
    }
}