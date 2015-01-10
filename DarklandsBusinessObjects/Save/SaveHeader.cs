using System.ComponentModel.DataAnnotations;
using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Save
{
    public class SaveHeader : StreamObject
    {
        // https://web.archive.org/web/20091112110231/http://wallace.net/darklands/formats/dksaveXX.sav.html#offset-0x00

        private const int SaveHeaderSize = 0x188;
        private Coordinate _coordinate;
        private Date _date;
        private Money _money;

        public SaveHeader(ByteStream data, int offset)
            : base(data, offset, SaveHeaderSize)
        {
        }

        public string CurrentLocationName
        {
            get { return GetString(0, 12); }
        }

        public string Label
        {
            get { return GetString(0x15, 23); }
        }

        public Date Date
        {
            get { return _date ?? (_date = new Date(DataStream, BaseOffset + 0x68, true)); }
        }

        public Money Money
        {
            get { return _money ?? (_money = new Money(DataStream, BaseOffset + 0x70)); }
        }

        // I dont think negative numbers should be possible but lets allow it any away to some extent
        [Range(-1000, 30000)]
        public int Reputation
        {
            get { return GetWord(0x7a); }
            set
            {
                SetWord(0x7a, value);
                NotifyPropertyChanged();
            }
        }

        public int LocationId
        {
            get { return GetWord(0x7c); }
        }

        public Coordinate Coordinate
        {
            get { return _coordinate ?? (_coordinate = new Coordinate(DataStream, BaseOffset + 0x7e)); }
        }

        [Range(0, 30000)]
        public int PhilosopherStone
        {
            get { return GetWord(0x92); }
            set
            {
                SetWord(0x92, value);
                NotifyPropertyChanged();
            }
        }

        [Range(0, 30000)]
        public int BankNotes
        {
            get { return GetWord(0x8c); }
            set
            {
                SetWord(0x8c, value);
                NotifyPropertyChanged();
            }
        }

        public override string ToString()
        {
            return "['" + Date
                   + "' '" + Label
                   + "' '" + CurrentLocationName
                   + "' '" + Coordinate
                   + "' '" + Money
                   + "' Notes: '" + BankNotes
                   + "' PhStone: '" + PhilosopherStone
                   + "' Reputation: '" + Reputation
                   + "']";
        }
    }
}