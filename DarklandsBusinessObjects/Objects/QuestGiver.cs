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
        Merchant = 0,
        Blacksmith,     // 1, not used?
        Artificier,     // 2, not used?
        Cathedral,      // 3, not used?
        ForeignTrader,  // 4
        Pharmacist,     // 5
        Medici,         // 6
        Hanse,          // 7
        Fugger,         // 8
        Schulz,        // 9
        Mayor,       // 10
        Castle          // 11 not used?
    }
}
