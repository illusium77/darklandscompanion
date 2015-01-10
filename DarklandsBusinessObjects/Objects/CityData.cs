using System.Collections.Generic;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class CityData : StreamObject
    {
        private const int CityDataSize = 0x2e;
        private const int MaxPorts = 4;
        private const int PortNoDestination = -1;
        private Coordinate _entryCoordinate;
        private Coordinate _exitCoordinate;
        private IReadOnlyList<int> _portDestinations;

        public CityData(ByteStream dataStream, int offset)
            : base(dataStream, offset, CityDataSize)
        {
        }

        public int Id { get; set; }

        public int Size
        {
            get { return GetWord(0x00); }
        }

        public int RiverByte
        {
            get { return GetWord(0x12); }
        }

        public int ConstantW14
        {
            get { return GetWord(0x14); }
        }

        public int Ordinal
        {
            get { return GetWord(0x16); }
        }

        public CityType Type
        {
            get { return (CityType) GetWord(0x18); }
        }

        public int UnknownW1A
        {
            get { return GetWord(0x1a); }
        }

        public int ConstantW01C
        {
            get { return GetWord(0x1c); }
        }

        public CityContent Content
        {
            get { return (CityContent) GetWord(0x1e); }
        }

        public int ConstantW020
        {
            get { return GetWord(0x20); }
        }

        public int BlacksmithQuality
        {
            get { return this[0x22]; }
        }

        public int MerchantQuality
        {
            get { return this[0x23]; }
        }

        public int SwordsmithQuality
        {
            get { return this[0x24]; }
        }

        public int ArmorerQuality
        {
            get { return this[0x25]; }
        }

        public int UnknownQualityB26
        {
            get { return this[0x26]; }
        }

        public int BowyerQuality
        {
            get { return this[0x27]; }
        }

        public int TinkerQuality
        {
            get { return this[0x28]; }
        }

        public int UnknownQualityB29
        {
            get { return this[0x29]; }
        }

        public int ClothierQuality
        {
            get { return this[0x2a]; }
        }

        public int UnknownB2B
        {
            get { return this[0x2b]; }
        }

        public int UnknownB2C
        {
            get { return this[0x2c]; }
        }

        public int UnknownB2D
        {
            get { return this[0x2d]; }
        }

        public Coordinate EntryCoordinate
        {
            get { return _entryCoordinate ?? (_entryCoordinate = new Coordinate(DataStream, BaseOffset + 0x02)); }
        }

        public Coordinate ExitCoordinate
        {
            get { return _exitCoordinate ?? (_exitCoordinate = new Coordinate(DataStream, BaseOffset + 0x06)); }
        }

        public IReadOnlyList<int> PortDestinations
        {
            get
            {
                if (_portDestinations == null)
                {
                    var ports = new List<int>();
                    var index = 0x0a;
                    for (var i = 0; i < MaxPorts; i++)
                    {
                        var port = GetWord(index);
                        if (port != PortNoDestination)
                        {
                            ports.Add(port);
                        }

                        index += 2;
                    }
                    _portDestinations = ports;
                }
                return _portDestinations;
            }
        }

        public override string ToString()
        {
            //return "'" + EntryCoordinate
            return "'" + Type
                   + "' '" + ArmorerQuality
                   + "'";
        }
    }
}