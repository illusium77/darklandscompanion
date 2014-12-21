using DarklandsBusinessObjects.Streaming;
using DarklandsBusinessObjects.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class Money : StreamObject
    {
        public const int MONEY_SIZE = 0x06;

        public int Florings
        {
            get
            {
                return GetWord(0x00);
            }
            set
            {
                SetWord(0x00, value);
                NotifyPropertyChanged();
            }
        }


        public int Groschen
        {
            get
            {
                return GetWord(0x02);
            }
            set
            {
                SetWord(0x02, value);
                NotifyPropertyChanged();
            }
        }

        public int Pfenniges
        {
            get
            {
                return GetWord(0x04);
            }
            set
            {
                SetWord(0x04, value);
                NotifyPropertyChanged();
            }
        }

        public int TotalInPfenniges
        {
            get
            {
                return Florings * 240 + Groschen * 12 + Pfenniges;
            }
            set
            {
                var fl = value / 240;
                var gr = (value % 240) / 12;
                var pf = (value % 240) % 12;

                Florings = fl;
                Groschen = gr;
                Pfenniges = pf;
            }
        }

        public Money(ByteStream data, int offset)
            : base(data, offset, MONEY_SIZE)
        {
        }

        public override string ToString()
        {
            return "[" + Florings + "fl " + Groschen + "gr " + Pfenniges + "pf ("
                + (TotalInPfenniges) + "pf)]";
        }
    }

    //    https://web.archive.org/web/20091112194440/http://wallace.net/darklands/formats/structures.html#structdef-money
    //    Structure: money

    //Size 0x06.

    //An amount of money.

    //Money is stored with the monetary units separated, rather than simply storing the number of pfenniges.
    //Darklands does seem to recover if the number of pfenniges or groschen "overflow", and it makes change automatically.
    //0x00: florins: word
    //0x02: groschen: word
    //0x04: pfenniges: word
}
