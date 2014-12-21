using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    [Flags]
    public enum CityContent
    {
        IsKloster = 1 << 0,
        IsSlums = 1 << 1,
        Unknown1 = 1 << 2,
        IsCathedral = 1 << 3,
        Unknown2 = 1 << 4,
        IsNoFortress = 1 << 5,
        IsTownhall = 1 << 6,
        IsPolit = 1 << 7,
        Constant1 = 1 << 8,
        Constant2 = 1 << 9,
        Constant3 = 1 << 10,
        Constant4 = 1 << 11,
        IsDocks = 1 << 12,
        Unknown3 = 1 << 13,
        IsPawnShop = 1 << 14,
        IsUniversity = 1 << 15,
    }

    //https://web.archive.org/web/20091111161830/http://wallace.net/darklands/formats/darkland.cty.html#structdef-city_data
    //city_contents: bitmask[16 bits]
    //Buildings and locations in the city.
    //Bits are on iff there is one of that type of building.
    //bit 1:	is_kloster:	Is there a kloster?
    //bit 2:	is_slums:	Is there a slums?
    //bit 3:	unknown	
    //bit 4:	is_cathedral:	Is there a cathedral?
    //bit 5:	unknown	
    //bit 6:	is_no_fortress:	Is there no city fortress?
    //  A rare case of a reversed boolean. Possibly it's a "has something else" flag,
    //  and fortresses are added if it doesn't have the other?
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
}
