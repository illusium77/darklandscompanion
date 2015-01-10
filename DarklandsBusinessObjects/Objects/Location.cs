using DarklandsBusinessObjects.Streaming;
using DarklandsBusinessObjects.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class Location : StreamObject
    {
        public static int LOCATION_SIZE = 0x3a;
        private static int NAME_SIZE = 20;

        public int Id { get; private set; }

        public LocationType Type { get { return (LocationType)GetWord(0x00); } }
        public int UnknownW02 { get { return GetWord(0x02); } }
        public int UnknownW08 { get { return GetWord(0x08); } }
        public int UnknownW0a { get { return GetWord(0x0a); } }
        public int Menu { get { return GetWord(0x0c); } }
        public int UnknownW0e { get { return GetWord(0x0e); } }
        public int ConstantFfB10 { get { return this[0x10]; } }
        public int CitySize { get { return this[0x11]; } }
        public int LocalReputation { get { return GetWord(0x12); } }
        public int UnknownB14 { get { return this[0x14]; } }
        public int Constant0B15 { get { return this[0x15]; } }
        public int Constant19B16 { get { return this[0x16]; } }
        public int Constant19B17 { get { return this[0x17]; } }
        public int Constant19B18 { get { return this[0x18]; } }
        public int InnCacheIndex { get { return GetWord(0x19); } }
        public int Constant0W1a { get { return GetWord(0x1a); } }
        public int UnknownW1c { get { return GetWord(0x1c); } }

        public int Constant0B1e { get { return this[0x1e]; } }
        public int Constant0B1f { get { return this[0x1f]; } }
        public int Constant0B20 { get { return this[0x20]; } }
        public int Constant0B21 { get { return this[0x21]; } }
        public int Constant0B22 { get { return this[0x22]; } }
        public int Constant0B23 { get { return this[0x23]; } }
        public int Constant0B24 { get { return this[0x24]; } }
        public int Constant0B25 { get { return this[0x25]; } }
        public string Name { get { return GetString(0x26, NAME_SIZE); } }

        private Coordinate m_coordinate;
        public Coordinate Coordinate
        {
            get
            {
                if (m_coordinate == null)
                {
                    m_coordinate = new Coordinate(DataStream, BaseOffset + 0x04);
                }
                return m_coordinate;
            }
        }

        public Location(ByteStream dataStream, int offset, int id)
            : base(dataStream, offset, LOCATION_SIZE)
        {
            Id = id;
        }

        public override string ToString()
        {
            return "['0x" + Id.ToString("x")
                + "' '" + Type
                + "' '" + Name
                + "' '" + Coordinate
                + "']";
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (m_coordinate != null)
                {
                    m_coordinate.Dispose();
                }
            }
        }

        public string GetDirections(IEnumerable<Location> locations)
        {
            var nearestCity = (from l in locations
                               where l.Type == LocationType.City
                               orderby l.Coordinate.DistanceTo(Coordinate)
                               select l);

            var n = nearestCity.First();

            return n.Coordinate.BearingTo(Coordinate) + " from " + n.Name;

        }
    }

    //https://web.archive.org/web/20091112194333/http://wallace.net/darklands/formats/darkland.loc.html#structdef-location
    //Structure: location

    //Size 0x3a.

    //0x00: icon: word (enum location_icon)
    //Map image for the location.
    //Note that this basically corresponds to the 'type' of location.
    //0x02: unknown word
    //0 for cities, other locations range from 0x08-0x0e.
    //0x04: coords: struct coordinates
    //Map coordinates.
    //0x08: unknown word
    //Ranges from 1-10.
    //Seems to be 4 or 9 for live Raubritters (1 for dead); perhaps it's a strength?
    //0x0a: unknown word
    //Most range from 1-5, except pagan altars, which are 0x63 (99).
    //0x0c: menu: word (enum menu)
    //Card displayed on entering the location.
    //This is the menu (found in enumerations) (card, screen?) you get when you enter the location.
    //It could be said that this is the closest thing to a "location type" that there is.
    //0x0e: unknown word
    //Always 0x62 except for castles currently occupied by Raubritters (icon=2); those are 0x92.
    //0x10: unknown byte [constant: 0xff]
    //0x11: city_size: byte
    //Size of the city.
    //Cities range from 3 (small) to 8 (Koln); non-cities are always 1.
    //0x12: local_rep: word
    //Local reputation.
    //In this file, this is always zero. The copy of this structure that lives in the saved game files gets non-zero values.
    //Ranges from -150 to 150 (although others claim to have observed numbers outside this range).
    //0x14: unknown byte
    //Zero seems to indicate an "active" site.
    //Ruins of a Raubritter castle get 0x04, as do destroyed villages.
    //Some other locations get 0x20 or 0x24.
    //0x15: unknown (4 bytes) [constant: { 0, 0x19, 0x19, 0x19 }]
    //0x19: inn_cache_idx: word
    //In this file, this is always 0xffff (-1).
    //In a saved game file, if the party stores items at an inn (in a city), this value becomes an index into cache_offsets (found in dksaveXX.sav).
    //0x1a: word [constant: 0x0000]
    //0x1c: unknown word
    //All are zero except for Nurnberg, which is 0xc0.
    //0x1e: unknown (8 bytes) [constant: all 0x00]
    //0x26: name: string(20)
    //Name.
}