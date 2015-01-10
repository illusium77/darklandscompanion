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
    public class SaveGame : IDisposable
    {
        // https://web.archive.org/web/20091112110231/http://wallace.net/darklands/formats/dksaveXX.sav.html

        public string FileName { get; private set; }

        public SaveHeader Header { get; private set; }
        public SaveParty Party { get; private set; }
        public SaveEvents Events { get; private set; }

        private ByteStream m_saveDataStream;

        public SaveGame(string fileName)
        {
            FileName = fileName;

            using (var file = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                m_saveDataStream = new ByteStream(file.ReadBytes((int)file.BaseStream.Length));

                Header = new SaveHeader(m_saveDataStream, 0x00);
                Party = new SaveParty(m_saveDataStream, 0xef);
                Events = new SaveEvents(m_saveDataStream,
                    0x189 + Party.NumberOfCharacters * SaveParty.SAVE_CHARACTER_SIZE);
            }
        }

        public void Save(string fileName = null)
        {
            using (var file = File.OpenWrite(fileName ?? FileName))
            {
                m_saveDataStream.WriteTo(file);
                file.Flush();
            }
        }

        public override string ToString()
        {
            return "[' " + Header.ToString()
                + "'"+ Environment.NewLine + "'" + Party.ToString()
                //+ "' '" + Events.ToString()
                + "']";
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (Header != null)
            {
                Header.Dispose();
            }
            if (Party != null)
            {
                Party.Dispose();
            }
            if (Events != null)
            {
                Events.Dispose();
            }
            if (m_saveDataStream != null)
            {
                m_saveDataStream.Dispose();
            }
        }
    }
}
