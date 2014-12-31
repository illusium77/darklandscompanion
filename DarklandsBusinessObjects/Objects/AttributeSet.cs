using DarklandsBusinessObjects.Streaming;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public const int ATTRIBUTE_SET_SIZE = 0x07;

        [Range(1, 99, ErrorMessage="Enter value between 1 and 99.")]
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

        public AttributeSet(ByteStream data, int offset)
            : base(data, offset, ATTRIBUTE_SET_SIZE)
        {
        }
    }

}
