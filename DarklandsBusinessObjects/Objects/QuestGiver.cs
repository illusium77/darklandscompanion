using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public enum QuestGiver // pulled from exe file, 0x188876
    {
        NA = -2,
        GoodsMerchant = 0,
        Blacksmith, //not used?
        Artificier, //not used?
        Cathedral, //not used?
        ForeignTrader,
        Pharmacist,
        Medici,
        Hanse,
        Fugger,
        Village, //not used?
        CityLord,
        Castle //not used?
    }
}
