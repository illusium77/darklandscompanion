using System;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class Date : StreamObject
    {
        // https://web.archive.org/web/20091112194440/http://wallace.net/darklands/formats/structures.html#structdef-date

        public const int DateSize = 0x08;
        private readonly bool _isReversed;

        public Date(ByteStream dataStream, int offset, bool isReversed)
            : base(dataStream, offset, DateSize)
        {
            _isReversed = isReversed;
        }

        public int Year
        {
            get { return _isReversed ? GetWord(0x00) : GetWord(0x06); }
        }

        // Zero based
        public int Month
        {
            get { return _isReversed ? GetWord(0x02) + 1 : GetWord(0x04) + 1; }
        }

        public int Day
        {
            get { return _isReversed ? GetWord(0x04) : GetWord(0x02); }
        }

        public int Hour
        {
            get { return _isReversed ? GetWord(0x06) : GetWord(0x00); }
        }

        // quests etc. maybe have infinite date (1499/13/31) when there
        // is not time limit for delivery
        public bool IsInfinite
        {
            get { return Year == 1499 && Month == 13 && Day == 31; }
        }

        public bool IsValid
        {
            get
            {
                return
                    Year >= 1400 && Year <= 1499
                    && Month > 0 && Month < 14 // when date is set to infinite, month is 13
                    && Day > 0 && Day <= 31;
            }
        }

        public int DayDifference(Date other)
        {
            var dateA = new DateTime(Year, Month, Day);
            var dateB = new DateTime(other.Year, other.Month, other.Day);

            return dateA < dateB
                ? (dateB - dateA).Days
                : (dateA - dateB).Days;
        }

        public int MonthDifference(Date other)
        {
            var dateA = new DateTime(Year, Month, Day);
            var dateB = new DateTime(other.Year, other.Month, other.Day);

            return Math.Abs((dateA.Month - dateB.Month) + 12*(dateA.Year - dateB.Year));
        }

        public override string ToString()
        {
            return string.Format("{0}/{1:00}/{2:00}", Year, Month, Day);
        }

        #region Compare

        public override bool Equals(Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to type return false.
            var t = obj as Date;
            if ((Object) t == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Year == t.Year) && (Month == t.Month) && (Day == t.Day) && (Hour == t.Hour);
        }

        public static bool operator ==(Date lhs, Date rhs)
        {
            // Check for null on left side. 
            if (ReferenceEquals(lhs, null))
            {
                if (ReferenceEquals(rhs, null))
                {
                    // null == null = true. 
                    return true;
                }

                // Only the left side is null. 
                return false;
            }
            // Equals handles case of null on right side. 
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Date lhs, Date rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return Year ^ Month ^ Day ^ Hour;
        }

        #endregion
    }
}