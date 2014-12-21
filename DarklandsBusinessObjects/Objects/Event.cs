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
    //public enum QuestType
    //{
    //    NA,
    //    Raubritter,
    //    FindItem
    //}

    public class Event : StreamObject
    {
        // TODO fix this

        public const int EVENT_SIZE = 0x30;

        //public int Id { get; private set; }
        //public QuestType Type { get; set; }

        public int UnknownW00 { get { return GetWord(0x00); } }
        public QuestGiver QuestGiver { get { return (QuestGiver)GetWord(0x1a); } }
        public int DestinationId { get { return GetWord(0x1c); } }
        public int UnknownW1e { get { return GetWord(0x1e); } }
        public int UnknownW20 { get { return GetWord(0x20); } }
        public int UnknownW22 { get { return GetWord(0x22); } }
        public int UnknownW24 { get { return GetWord(0x24); } }
        public int UnknownW26 { get { return GetWord(0x26); } }
        public int UnknownW28 { get { return GetWord(0x28); } }
        public int UnknownW2a { get { return GetWord(0x2a); } }
        public int UnknownW2c { get { return GetWord(0x2c); } }
        public int ItemId { get { return GetWord(0x2e); } }

        private Date m_createDate;
        public Date CreateDate
        {
            get
            {
                if (m_createDate == null)
                {
                    m_createDate = new Date(DataStream, BaseOffset + 0x02, false);
                }

                return m_createDate;
            }
        }

        private Date m_unknownDate;
        public Date UnknownDate
        {
            get
            {
                if (m_unknownDate == null)
                {
                    m_unknownDate = new Date(DataStream, BaseOffset + 0x0a, false);
                }

                return m_unknownDate;
            }
        }

        private Date m_expireDate;
        public Date ExpireDate
        {
            get
            {
                if (m_expireDate == null)
                {
                    m_expireDate = new Date(DataStream, BaseOffset + 0x12, false);
                }

                return m_expireDate;
            }
        }

        public Event(ByteStream dataStream, int offset)
            : base(dataStream, offset, EVENT_SIZE)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (m_createDate != null)
                {
                    m_createDate.Dispose();
                }
                if (m_unknownDate != null)
                {
                    m_unknownDate.Dispose();
                }
                if (m_expireDate != null)
                {
                    m_expireDate.Dispose();
                }
            }
        }

        public override string ToString()
        {
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(CreateDate);
        //    if (UnknownDate != CreateDate)
        //    {
        //        sb.Append("(" + UnknownDate + ")");
        //    }
        //    sb.Append("-" + ExpireDate);

        //    sb.Append(string.Format("[{0:X2} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2} {7:X2} {8:X2}]", UnknownW00, UnknownW1e, UnknownW20, UnknownW22, UnknownW24, UnknownW26, UnknownW28, UnknownW2a, UnknownW2c));

        //    if (Type == QuestType.FindItem)
        //    {
        //        sb.Append(" Find " + Item.Name + " from " + Destination.Name);
        //        if (Destination.Icon != LocationIcon.City)
        //        {
        //            var nearestCity = (from l in DarklandsService.Locations
        //                              where l.Icon == LocationIcon.City
        //                              let dist = l.Coordinate.DistanceTo(Destination.Coordinate)
        //                              orderby dist
        //                              select l).First();

        //            sb.Append(" (" + nearestCity.Coordinate.BearingTo(Destination.Coordinate)
        //                + " of " + nearestCity.Name + ")" );
        //        }
        //        sb.Append(" for " + QuestGiver + " in " + DarklandsService.Locations[UnknownW1e].Name);
        //    }
        //    else
        //    {

        //        //sb.Append(string.Format(" {0,-10}", Type));

        //        sb.Append(string.Format(" {0,-15}", QuestGiver.ToString()));

        //        //if (Destination.Icon == LocationIcon.City)
        //        //{
        //        sb.Append(string.Format(" from {0}", (Destination != null ? (Destination.Name + "(" + Destination.Icon + ")" ) : "No destination")));
        //        if (Destination != null && Destination.Icon != LocationIcon.City)
        //        {
        //            var nearestCity = (from l in DarklandsService.Locations
        //                               where l.Icon == LocationIcon.City
        //                               let dist = l.Coordinate.DistanceTo(Destination.Coordinate)
        //                               orderby dist
        //                               select l).First();

        //            sb.Append(" (" + nearestCity.Coordinate.BearingTo(Destination.Coordinate)
        //                + " of " + nearestCity.Name + ")");
        //        }
        //        //}
        //        //else
        //        //{
        //        //    DarklandsService.
        //        //}

        //        sb.Append(string.Format(" item {0}", (Item != null ? Item.ToString() : "No item")));


        //        //if (Source == EventSource.NA)
        //        //{
        //        //    sb.Append(" Unknown event
        //        //}
        //        //else
        //        //{
        //        //}
        //    }

        //    return sb.ToString();

            return "['" + CreateDate
                + "' '" + UnknownW00
                //+ "' '" + UnknownDate
                + "' '" + ExpireDate
                + "' '" + QuestGiver
                //+ "' '" + (Item != null ? Item.Name : "No item")
                //+ "' '" + (Destination != null ? Destination.Name : "No destination")
                //+ "' '" + (Destination >= 0 && Destination < locs.Count ? locs[Destination].Name : Destination.ToString())
                + "']";
        }
    }

    // https://web.archive.org/web/20091112110231/http://wallace.net/darklands/formats/dksaveXX.sav.html#structdef-event
    //Structure: event

    //Size 0x30 (48).

    //An event, effect, or quest.

    //TODO: flesh this out!!!!!!!!!
    //TODO: describe the dynamics of this structure
    //0x00: unknown word
    //For RB quests, when first given this value is sort of the name of the person who gave it to you. After killing the RB the value moves to offset 0x1e, and this gets a copy of the value from 0x1a.
    //0x02: create_date: struct date
    //Date the event was created.
    //0x0a: unknown struct date
    //Always identical in value to create_date.
    //0x12: expire_date: struct date
    //Date the event will expire.
    //TODO: for effects, is this the date it happens instead??
    //0x1a: unknown word
    //For quests, this is the location where the quest was given, until the reward is taken; then it becomes -2.
    //For other things this is usually -2, although occasionally it is 0.
    //TODO: make this an enum.
    //0x1c: unknown word
    //For newly given RB quests, this is the location of the RB. Once the RB is slain it becomes the location of the city to return to.
    //0x1e: unknown word
    //For newly given RB quests, this is the location of the city that gave the quest.
    //Once the RB quest reward is taken, this becomes 0.
    //0x21: unknown word
    //Common values are 0x63, 0x5f, 0x32, and 0, although others appear sporadically.
    //0x23: unknown word
    //Maybe this is a sort of state variable?
    //For RB quests the sequence is: 0x8 (got quest), 0x24 (killed RB), 0x7 (got reward).
    //0x25: unknown word
    //This and the next five are usually, but not always, zero.
    //0x27: unknown word
    //0x29: unknown word
    //For RB quests this is often 0x03; it becomes 0 after the reward is taken, and no reward can be gotten if it is not 3.
    //Possibly (for quests, at least) this is the index of the option to 'enable' within the card? The 'ask X for your reward' option is the 3rd option on the screen...
    //TODO: this could maybe be tested by (carefully) altering the card and the index...? Unless the logic is in the executable.
    //0x2a: unknown word
    //0x2c: unknown word
    //0x2e: unknown word
    //This is usually the item code for an item required by a quest.
    //Codes are a reference to offsets into the item_definitions array (found in darkland.lst).
    //However, I've seen an event appear where this is non-zero and an item quest has not just been taken.
}
