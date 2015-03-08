using System.ComponentModel.DataAnnotations;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class Rgb : StreamObject
    {
        private const int RgbSize = 0x03;
        private const int RgbMin = 0x00;
        public const int RgbMax = 0x3f;

        public Rgb(ByteStream dataStream, int offset)
            : base(dataStream, offset, RgbSize)
        {
        }

        [Range(RgbMin, RgbMax, ErrorMessage = "Valid value range is 0 - 63")]
        public int Red
        {
            get { return this[0x00]; }
            set
            {
                this[0x00] = Validate(value);
                NotifyPropertyChanged();
            }
        }

        [Range(RgbMin, RgbMax, ErrorMessage = "Valid value range is 0 - 63")]
        public int Green
        {
            get { return this[0x01]; }
            set
            {
                this[0x01] = Validate(value);
                NotifyPropertyChanged();
            }
        }

        [Range(RgbMin, RgbMax, ErrorMessage = "Valid value range is 0 - 63")]
        public int Blue
        {
            get { return this[0x02]; }
            set
            {
                this[0x02] = Validate(value);
                NotifyPropertyChanged();
            }
        }

        private static int Validate(int value)
        {
            if (value < RgbMin)
            {
                return RgbMin;
            }
            if (value > RgbMax)
            {
                return RgbMax;
            }

            return value;
        }

        public override string ToString()
        {
            return "[" + Red + ", " + Green + ", " + Blue + "]";
        }
    }
}
