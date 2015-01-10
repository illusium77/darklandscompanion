using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class City : StreamObject
    {
        public const int CitySize = 0x26e;
        public const int DescriptionSize = 80;
        private const int StringSize = 32;
        private CityData _cityData;

        public City(ByteStream data, int offset, int id)
            : base(data, offset, CitySize)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public string ShortName
        {
            get { return GetString(0x00, StringSize); }
        }

        public string LongName
        {
            get { return GetString(0x20, StringSize); }
        }

        public string Leader
        {
            get { return GetString(0x6e, StringSize); }
        }

        public string Ruler
        {
            get { return GetString(0x8e, StringSize); }
        }

        public string UnknownSae
        {
            get { return GetString(0xae, StringSize); }
        }

        public string Polit
        {
            get { return GetString(0xce, StringSize); }
        }

        public string Townhall
        {
            get { return GetString(0xee, StringSize); }
        }

        public string Fortress
        {
            get { return GetString(0x10e, StringSize); }
        }

        public string Cathedral
        {
            get { return GetString(0x12e, StringSize); }
        }

        public string Church
        {
            get { return GetString(0x14e, StringSize); }
        }

        public string Market
        {
            get { return GetString(0x16e, StringSize); }
        }

        public string UnknownS18E
        {
            get { return GetString(0x18e, StringSize); }
        }

        public string Slum
        {
            get { return GetString(0x1ae, StringSize); }
        }

        public string UnknownS1Ce
        {
            get { return GetString(0x1ce, StringSize); }
        }

        public string Pawnshop
        {
            get { return GetString(0x1ee, StringSize); }
        }

        public string Kloster
        {
            get { return GetString(0x20e, StringSize); }
        }

        public string Inn
        {
            get { return GetString(0x22e, StringSize); }
        }

        public string University
        {
            get { return GetString(0x24e, StringSize); }
        }

        public string Description { get; set; }

        public CityData CityData
        {
            get { return _cityData ?? (_cityData = new CityData(DataStream, BaseOffset + 0x40)); }
        }

        public override string ToString()
        {
            return "['" + Id.ToString("x")
                   + "' '" + LongName
                   + "' '" + CityData.EntryCoordinate
                   + "']";
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (_cityData != null)
                {
                    _cityData.Dispose();
                }
            }
        }
    }
}