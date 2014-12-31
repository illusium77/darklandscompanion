using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Streaming;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Save
{
    public class SaveHeader : StreamObject
    {
        // https://web.archive.org/web/20091112110231/http://wallace.net/darklands/formats/dksaveXX.sav.html#offset-0x00

        const int SAVE_HEADER_SIZE = 0x188;

        public string CurrentLocationName
        {
            get
            {
                return GetString(0, 12);
            }
        }

        public string Label
        {
            get
            {
                return GetString(0x15, 23);
            }
        }

        private Date m_date;
        public Date Date
        {
            get
            {
                if (m_date == null)
                {
                    m_date = new Date(DataStream, BaseOffset + 0x68, true);
                }

                return m_date;
            }
        }

        private Money m_money;
        public Money Money
        {
            get
            {
                if (m_money == null)
                {
                    m_money = new Money(DataStream, BaseOffset + 0x70);
                }

                return m_money;
            }
        }

        // I dont think negative numbers should be possible but lets allow it any away to some extent
        [Range(-1000, 30000)]
        public int Reputation
        {
            get
            {
                return GetWord(0x7a);
            }
            set
            {
                SetWord(0x7a, value);
                NotifyPropertyChanged();
            }
        }

        public int LocationId
        {
            get
            {
                return GetWord(0x7c);
            }
        }

        private Coordinate m_coordinate;
        public Coordinate Coordinate
        {
            get
            {
                if (m_coordinate == null)
                {
                    m_coordinate = new Coordinate(DataStream, BaseOffset + 0x7e);
                }

                return m_coordinate;
            }
        }

        [Range(0, 30000)]
        public int PhilosopherStone
        {
            get
            {
                return GetWord(0x92);
            }
            set
            {
                SetWord(0x92, value);
                NotifyPropertyChanged();
            }
        }

        [Range(0, 30000)]
        public int BankNotes
        {
            get
            {
                return GetWord(0x8c);
            }
            set
            {
                SetWord(0x8c, value);
                NotifyPropertyChanged();
            }
        }

        public SaveHeader(ByteStream data, int offset)
            : base(data, offset, SAVE_HEADER_SIZE)
        {
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
