using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class CharacterColors : StreamObject
    {
        private Rgb _firstHi;
        private Rgb _firstLo;
        private Rgb _secondHi;
        private Rgb _secondMed;
        private Rgb _secondLo;
        private Rgb _thirdHi;
        private Rgb _thirdMed;
        private Rgb _thirdLo;

        public const int CharacterColorsSize = 0x18;

        public CharacterColors(ByteStream dataStream, int offset)
            : base(dataStream, offset, CharacterColorsSize)
        {
        }

        public Rgb FirstHi
        {
            get { return _firstHi ?? (_firstHi = new Rgb(DataStream, BaseOffset + 0x00)); }
        }

        public Rgb FirstLo
        {
            get { return _firstLo ?? (_firstLo = new Rgb(DataStream, BaseOffset + 0x03)); }
        }

        public Rgb SecondHi
        {
            get { return _secondHi ?? (_secondHi = new Rgb(DataStream, BaseOffset + 0x06)); }
        }

        public Rgb SecondMed
        {
            get { return _secondMed ?? (_secondMed = new Rgb(DataStream, BaseOffset + 0x09)); }
        }

        public Rgb SecondLo
        {
            get { return _secondLo ?? (_secondLo = new Rgb(DataStream, BaseOffset + 0x0c)); }
        }

        public Rgb ThirdHi
        {
            get { return _thirdHi ?? (_thirdHi = new Rgb(DataStream, BaseOffset + 0x0f)); }
        }

        public Rgb ThirdMed
        {
            get { return _thirdMed ?? (_thirdMed = new Rgb(DataStream, BaseOffset + 0x12)); }
        }

        public Rgb ThirdLo
        {
            get { return _thirdLo ?? (_thirdLo = new Rgb(DataStream, BaseOffset + 0x15)); }
        }


    }
}
