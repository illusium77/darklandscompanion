using DarklandsBusinessObjects.Streaming;
using DarklandsBusinessObjects.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class CityData : StreamObject
    {
        public const int CITY_DATA_SIZE = 0x2e;

        public const int MAX_PORTS = 4;
        public const int PORT_NO_DESTINATION = -1;

        public int Id { get; private set; }

        public int Size { get { return GetWord(0x00); } }
        public int RiverByte { get { return GetWord(0x12); } }
        public int ConstantW14 { get { return GetWord(0x14); } }
        public int Ordinal { get { return GetWord(0x16); } }
        public CityType Type { get { return (CityType)GetWord(0x18); } }
        public int UnknownW1a { get { return GetWord(0x1a); } }
        public int ConstantW01c { get { return GetWord(0x1c); } }
        public CityContent Content { get { return (CityContent)GetWord(0x1e); } }
        public int ConstantW020 { get { return GetWord(0x20); } }
        public byte BlacksmithQuality { get { return this[0x22]; } }
        public byte MerchantQuality { get { return this[0x23]; } }
        public byte SwordsmithQuality { get { return this[0x24]; } }
        public byte ArmorerQuality { get { return this[0x25]; } }
        public byte UnknownQualityB26 { get { return this[0x26]; } }
        public byte BowyerQuality { get { return this[0x27]; } }
        public byte TinkerQuality { get { return this[0x28]; } }
        public byte UnknownQualityB29 { get { return this[0x29]; } }
        public byte ClothierQuality { get { return this[0x2a]; } }
        public byte UnknownB2b { get { return this[0x2b]; } }
        public byte UnknownB2c { get { return this[0x2c]; } }
        public byte UnknownB2d { get { return this[0x2d]; } }

        private Coordinate m_entryCoordinate;
        public Coordinate EntryCoordinate
        {
            get
            {
                if (m_entryCoordinate == null)
                {
                    m_entryCoordinate = new Coordinate(DataStream, BaseOffset + 0x02);
                }
                return m_entryCoordinate;
            }
        }

        private Coordinate m_exitCoordinate;
        public Coordinate ExitCoordinate
        {
            get
            {
                if (m_exitCoordinate == null)
                {
                    m_exitCoordinate = new Coordinate(DataStream, BaseOffset + 0x06);
                }
                return m_exitCoordinate;
            }
        }

        private IReadOnlyList<int> m_portDestinations;
        public IReadOnlyList<int> PortDestinations
        {
            get
            {
                if (m_portDestinations == null)
                {
                    var ports = new List<int>();
                    var index = 0x0a;
                    for (int i = 0; i < MAX_PORTS; i++)
                    {
                        var port = GetWord(index);
                        if (port != PORT_NO_DESTINATION)
                        {
                            ports.Add(port);
                        }

                        index += 2;
                    }
                    m_portDestinations = ports;
                }
                return m_portDestinations;

            }
        }

        public CityData(ByteStream dataStream, int offset, int id)
            : base(dataStream, offset, CITY_DATA_SIZE)
        {
            Id = id;
        }

        public override string ToString()
        {
            //return "'" + EntryCoordinate
            return "'" + Type
                + "' '" + ArmorerQuality
                + "'";
        }


        //https://web.archive.org/web/20091111161830/http://wallace.net/darklands/formats/darkland.cty.html#structdef-city_data
        //Structure: city_data

        //Size 0x2e.

        //0x00: city_size: word
        //Size of the city.
        //Ranges from 3 (small) to 8 (Koln).
        //0x02: entry_coords: struct coordinates
        //City location on the map.
        //0x06: exit_coords: struct coordinates
        //Party coordinates when leaving a city.
        //When you leave a city, you don't exit at the same point as you entered. The exit coordinates were (usually) selected so as not to place you in an untenable position (the ocean, trapped by a river loop, etc).
        //0x0a: dock_destinations: array[ 4 ] of word
        //Dock destination cities.
        //This contains the indices (in the cities array) of the destinations available via the city docks the docks.
        //0xffff is used for "no destination". Inland cities have all 0xffffs.
        //0x12: word
        //if coastal, side of the river ???[Hamburg] TODO.
        //Values are: 0xffff (inland), 0 (north of the river), 1 (south of the river)
        //0 and 1 cities are on or near tidal zones (swamps), and may be subject to flooding.
        //0x14: word [constant: 4]
        //0x16: word
        //Pseudo-ordinal.
        //At first glance, this looks like an ordinal offset running from 0 to 0x5b, but 0x18 is missing, and 0x3c repeats.
        //This value is probably not used.
        //0x18: city_type: word (enum ruler)
        //Type of city
        //0x1a: unknown word
        //0, 1, 2, or 3.
        //0x1c: word [constant: 0]
        //0x1e: city_contents: bitmask[16 bits]
        //Buildings and locations in the city.
        //Bits are on iff there is one of that type of building.
        //bit 1:	is_kloster:	Is there a kloster?
        //bit 2:	is_slums:	Is there a slums?
        //bit 3:	unknown	
        //bit 4:	is_cathedral:	Is there a cathedral?
        //bit 5:	unknown	
        //bit 6:	is_no_fortress:	Is there no city fortress?
        //A rare case of a reversed boolean. Possibly it's a "has something else" flag, and fortresses are added if it doesn't have the other?
        //bit 7:	is_town_hall:	Is there a town hall?
        //bit 8:	is_polit: [constant: 1]	Is there a political center?
        //bit 9:	[constant: 0]	
        //bit 10:	[constant: 0]	
        //bit 11:	[constant: 0]	
        //bit 12:	[constant: 0]	
        //bit 13:	docks:	Are there docks?
        //bit 14:	unknown	
        //bit 15:	is_pawnshop:	Is there a liefhaus (pawnshop)?
        //bit 16:	is_university:	Is there a university?
        //0x20: word [constant: 0]
        //0x22: qual_black: byte
        //Quality of the blacksmith.
        //This, and the other nine qualities, all seem to work in the same way.
        //A zero value indicates that the city does not have that particular shop.
        //Non-zero values do not exactly equal the quality of the items available, but merely indicate relative qualities! For example, Nurnberg has a 0x31 (49) listed for the armory, but offers q37 (0x25) armor. However, if one city has a higher value than another, then that city's items will be of equal or greater quality.
        //The quality of the healer is not stored here, but is apparently random. (TODO: verify?)
        //TODO: comments about Quality of the alchemist, university, pharmacist being the seed thing.
        //0x23: qual_merch: byte
        //Quality of the merchant.
        //0x24: qual_sword: byte
        //Quality of the swordsmith.
        //0x25: qual_armor: byte
        //Quality of the armorer.
        //0x26: qual_unk1: unknown byte
        //0x27: qual_bow: byte
        //Quality of the bowyer.
        //0x28: qual_tink: byte
        //Quality of the tinker.
        //0x29: qual_unk2: unknown byte
        //0x2a: qual_cloth: byte
        //Quality of the clothing merchant.
        //0x2b: byte [constant: 0]
        //0x2c: unknown byte
        //Since the following byte is 0 or 1, this and that might actually be a single word value.
        //0x2d: unknown byte
        //Either zero or one (only a couple of ones).

    }
}
