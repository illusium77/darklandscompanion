using DarklandsBusinessObjects.Streaming;
using DarklandsBusinessObjects.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class City : StreamObject
    {
        public const int CITY_SIZE = 0x26e;
        public const int DESCRIPTION_SIZE = 80;
        private const int STRING_SIZE = 32;

        public int Id { get; private set; }

        public string ShortName { get { return GetString(0x00, STRING_SIZE); } }
        public string LongName { get { return GetString(0x20, STRING_SIZE); } }
        public string Leader { get { return GetString(0x6e, STRING_SIZE); } }
        public string Ruler { get { return GetString(0x8e, STRING_SIZE); } }
        public string UnknownSae { get { return GetString(0xae, STRING_SIZE); } }
        public string Polit { get { return GetString(0xce, STRING_SIZE); } }
        public string Townhall { get { return GetString(0xee, STRING_SIZE); } }
        public string Fortress { get { return GetString(0x10e, STRING_SIZE); } }
        public string Cathedral { get { return GetString(0x12e, STRING_SIZE); } }
        public string Church { get { return GetString(0x14e, STRING_SIZE); } }
        public string Market { get { return GetString(0x16e, STRING_SIZE); } }
        public string UnknownS18e { get { return GetString(0x18e, STRING_SIZE); } }
        public string Slum { get { return GetString(0x1ae, STRING_SIZE); } }
        public string UnknownS1ce { get { return GetString(0x1ce, STRING_SIZE); } }
        public string Pawnshop { get { return GetString(0x1ee, STRING_SIZE); } }
        public string Kloster { get { return GetString(0x20e, STRING_SIZE); } }
        public string Inn { get { return GetString(0x22e, STRING_SIZE); } }
        public string University { get { return GetString(0x24e, STRING_SIZE); } }

        public string Description { get; set; }

        private CityData m_cityData;
        public CityData CityData
        {
            get
            {
                if (m_cityData == null)
                {
                    m_cityData = new CityData(DataStream, BaseOffset + 0x40, Id);
                }
                return m_cityData;
            }
        }

        public City(ByteStream data, int offset, int id)
            : base(data, offset, CITY_SIZE)
        {
            Id = id;
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
                if (m_cityData != null)
                {
                    m_cityData.Dispose();
                }
            }
        }

        //https://web.archive.org/web/20091111161830/http://wallace.net/darklands/formats/darkland.cty.html#structdef-city
        //Structure: city

        //Size 0x26e.

        //0x00: short_name: string(32)
        //0x20: full_name: string(32)
        //0x40: city_data: struct city_data (size 0x2e)
        //0x6e: leader_name: string(32)
        //0x8e: ruler_name: string(32)
        //0xae: unknown string(32)
        //TODO: is this non-empty ever? (ditto for other two unknowns)
        //0xce: polit_name: string(32)
        //Name of the political center or town square.
        //TODO: describe what empty values look like
        //0xee: town_hall_name: string(32)
        //0x10e: fortress_name: string(32)
        //Name of the city fortress or castle.
        //0x12e: cathedral_name: string(32)
        //0x14e: church_name: string(32)
        //0x16e: market_name: string(32)
        //Name of the marketplace.
        //0x18e: unknown string(32)
        //Often contains "Munzenplatz". Possibly this is "central square name".
        //0x1ae: slum_name: string(32)
        //0x1ce: unknown string(32)
        //Many places have "Zeughaus", which translates to "armoury"; others end in "-turm" (tower?) or "-tor" (gate?). Quite possible this is for one of the unused "rebellion" codepath.
        //0x1ee: pawnshop_name: string(32)
        //Name of the pawnshop.
        //All pawnshops are named the same; this is either 'Leifhaus' or is empty.
        //0x20e: kloster_name: string(32)
        //Name of the kloster (church law and administration building).
        //0x22e: inn_name: string(32)
        //0x24e: university_name: string(32)
    }
}
