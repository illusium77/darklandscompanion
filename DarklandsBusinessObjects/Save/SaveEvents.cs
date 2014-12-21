using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Streaming;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Save
{
    public class SaveEvents : StreamObject
    {
        // https://web.archive.org/web/20091112110231/http://wallace.net/darklands/formats/dksaveXX.sav.html#offset-0x189

        // TODO: add inn caches etc.

        public int NumberOfEvents
        {
            get
            {
                return GetWord(0x00);
            }
        }

        private IReadOnlyList<Event> m_events;
        public IReadOnlyList<Event> Events
        {
            get
            {
                if (m_events == null)
                {
                    var count = NumberOfEvents;
                    var events = new List<Event>(count);

                    for (int i = 0; i < count; i++)
                    {
                        var offset = 0x02 + i * Event.EVENT_SIZE;
                        events.Add(new Event(DataStream, BaseOffset + offset));
                    }

                    m_events = events;
                }

                return m_events;
            }
        }

        public int NumberOfLocations
        {
            get
            {
                // 0x02 is for size of number of events word
                return GetWord(0x02 + NumberOfEvents * Event.EVENT_SIZE);
            }
        }

        private IReadOnlyList<Location> m_locations;
        public IReadOnlyList<Location> Locations
        {
            get
            {
                if (m_locations == null)
                {
                    var count = NumberOfLocations;
                    // four bytes for sizes of event- and location numbers (two bytes each)
                    var startOffset = 0x04 + NumberOfEvents * Event.EVENT_SIZE;

                    var locations = new List<Location>(count);
                    for (int i = 0; i < count; i++)
                    {
                        locations.Add(new Location(DataStream, 
                            BaseOffset + startOffset + i * Location.LOCATION_SIZE, i));
                    }

                    m_locations = locations;
                }

                return m_locations;
            }
        }

        public SaveEvents(ByteStream data, int offset)
            : base(data, offset, 0) // size is unknown initially
        {
        }

        public override string ToString()
        {
            return "['" + NumberOfEvents
                + "']";
        }
    }
}
