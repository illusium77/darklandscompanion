using System.ComponentModel.DataAnnotations;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class Money : StreamObject
    {
        private const int MoneySize = 0x06;

        public Money(ByteStream data, int offset)
            : base(data, offset, MoneySize)
        {
        }

        public int Florings
        {
            get { return GetWord(0x00); }
            private set
            {
                SetWord(0x00, value);
                NotifyPropertyChanged();
            }
        }

        public int Groschen
        {
            get { return GetWord(0x02); }
            private set
            {
                SetWord(0x02, value);
                NotifyPropertyChanged();
            }
        }

        public int Pfenniges
        {
            get { return GetWord(0x04); }
            private set
            {
                SetWord(0x04, value);
                NotifyPropertyChanged();
            }
        }

        [Range(0, 7000000)]
        public int TotalInPfenniges
        {
            get { return Florings*240 + Groschen*12 + Pfenniges; }
            set
            {
                var fl = value/240;
                var gr = (value%240)/12;
                var pf = (value%240)%12;

                Florings = fl;
                Groschen = gr;
                Pfenniges = pf;
            }
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