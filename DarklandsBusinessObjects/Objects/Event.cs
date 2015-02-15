using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Objects
{
    public class Event : StreamObject
    {
        public const int EventSize = 0x30;
        private Date _createDate;
        private Date _expireDate;
        private Date _unknownDate;

        public Event(ByteStream dataStream, int offset)
            : base(dataStream, offset, EventSize)
        {
        }

        public int UnknownW00
        {
            get { return GetWord(0x00); }
        }

        public QuestGiver QuestGiver
        {
            get { return (QuestGiver) GetWord(0x1a); }
        }

        public int DestinationLocationId
        {
            get { return GetWord(0x1c); }
        }

        public int SourceLocationId
        {
            get { return GetWord(0x1e); }
        } // invalid after RB has been killed, value copied to destinatioId

        public int UnknownW20
        {
            get { return GetWord(0x20); }
        }

        public int UnknownW22
        {
            get { return GetWord(0x22); }
        }

        public int UnknownW24
        {
            get { return GetWord(0x24); }
        }

        public int UnknownW26
        {
            get { return GetWord(0x26); }
        }

        public int UnknownW28
        {
            get { return GetWord(0x28); }
        }

        public int UnknownW2A
        {
            get { return GetWord(0x2a); }
        }

        public int UnknownW2C
        {
            get { return GetWord(0x2c); }
        }

        public int ItemId
        {
            get { return GetWord(0x2e); }
        }

        public Date CreateDate
        {
            get { return _createDate ?? (_createDate = new Date(DataStream, BaseOffset + 0x02, false)); }
        }

        public Date UnknownDate
        {
            get { return _unknownDate ?? (_unknownDate = new Date(DataStream, BaseOffset + 0x0a, false)); }
        }

        public Date ExpireDate
        {
            get { return _expireDate ?? (_expireDate = new Date(DataStream, BaseOffset + 0x12, false)); }
        }

        public bool IsQuest
        {
            get { return QuestGiver != QuestGiver.Na; }
        }

        public bool IsActiveQuest
        {
            get
            {
                // logic from dkedit dos savegame editor
                if (!IsQuest || !ExpireDate.IsValid || !CreateDate.IsValid || ExpireDate.Year == CreateDate.Year)
                {
                    return false;
                }

                //if (UnknownW22 != 8 || UnknownW2a == 0 || UnknownW2c != 2)
                //{
                //    if (ItemId == 0)
                //    {
                //        return false;
                //    }
                //}

                return true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (_createDate != null)
                {
                    _createDate.Dispose();
                }
                if (_unknownDate != null)
                {
                    _unknownDate.Dispose();
                }
                if (_expireDate != null)
                {
                    _expireDate.Dispose();
                }
            }
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