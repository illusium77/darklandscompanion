using System.Collections.Generic;
using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Save
{
    public class SaveEvents : StreamObject
    {
        private IReadOnlyList<Event> _events;
        private IReadOnlyList<Location> _locations;

        public SaveEvents(ByteStream data, int offset)
            : base(data, offset, 0) // size is unknown initially
        {
        }

        // https://web.archive.org/web/20091112110231/http://wallace.net/darklands/formats/dksaveXX.sav.html#offset-0x189

        // TODO: add inn caches etc.

        public int NumberOfEvents
        {
            get { return GetWord(0x00); }
        }

        public IReadOnlyList<Event> Events
        {
            get
            {
                if (_events == null)
                {
                    var count = NumberOfEvents;
                    var events = new List<Event>(count);

                    for (var i = 0; i < count; i++)
                    {
                        var offset = 0x02 + i*Event.EventSize;
                        events.Add(new Event(DataStream, BaseOffset + offset));
                    }

                    _events = events;
                }

                return _events;
            }
        }

        public int NumberOfLocations
        {
            get
            {
                // 0x02 is for size of number of events word
                return GetWord(0x02 + NumberOfEvents*Event.EventSize);
            }
        }

        public IReadOnlyList<Location> Locations
        {
            get
            {
                if (_locations == null)
                {
                    var count = NumberOfLocations;
                    // four bytes for sizes of event- and location numbers (two bytes each)
                    var startOffset = 0x04 + NumberOfEvents*Event.EventSize;

                    var locations = new List<Location>(count);
                    for (var i = 0; i < count; i++)
                    {
                        locations.Add(new Location(DataStream,
                            BaseOffset + startOffset + i*Location.LocationSize, i));
                    }

                    _locations = locations;
                }

                return _locations;
            }
        }

        public override string ToString()
        {
            return "['" + NumberOfEvents
                   + "']";
        }
    }
}