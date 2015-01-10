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
    public class Date : StreamObject
    {
        // https://web.archive.org/web/20091112194440/http://wallace.net/darklands/formats/structures.html#structdef-date

        public const int DATE_SIZE = 0x08;

        private bool m_isReversed;

        public int Year
        {
            get { return m_isReversed ? GetWord(0x00) : GetWord(0x06); }
        }

        // Zero based
        public int Month
        {
            get { return m_isReversed ? GetWord(0x02) + 1 : GetWord(0x04) + 1; }
        }

        public int Day
        {
            get { return m_isReversed ? GetWord(0x04) : GetWord(0x02); }
        }

        public int Hour
        {
            get { return m_isReversed ? GetWord(0x06) : GetWord(0x00); }
        }

        // quests etc. maybe have infinite date (1499/13/31) when there
        // is not time limit for delivery
        public bool IsInfinite { get { return Year == 1499 && Month == 13 && Day == 31; } }

        public Date(ByteStream dataStream, int offset, bool isReversed)
            : base(dataStream, offset, DATE_SIZE)
        {
            m_isReversed = isReversed;
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
            if ((Object)t == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Year == t.Year) && (Month == t.Month) && (Day == t.Day) && (Hour == t.Hour);
        }

        public static bool operator ==(Date lhs, Date rhs)
        {
            // Check for null on left side. 
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
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
