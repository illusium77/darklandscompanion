using System;
using System.IO;
using DarklandsBusinessObjects.Streaming;

namespace DarklandsBusinessObjects.Save
{
    public class SaveGame : IDisposable
    {
        private readonly ByteStream _saveDataStream;

        public SaveGame(string fileName)
        {
            FileName = fileName;

            using (var file = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                _saveDataStream = new ByteStream(file.ReadBytes((int) file.BaseStream.Length));

                Header = new SaveHeader(_saveDataStream, 0x00);
                Party = new SaveParty(_saveDataStream, 0xef);
                Events = new SaveEvents(_saveDataStream,
                    0x189 + Party.NumberOfDefinedCharacters*SaveParty.SaveCharacterSize);
            }
        }

        // https://web.archive.org/web/20091112110231/http://wallace.net/darklands/formats/dksaveXX.sav.html

        public string FileName { get; private set; }
        public SaveHeader Header { get; private set; }
        public SaveParty Party { get; private set; }
        public SaveEvents Events { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save(string fileName = null)
        {
            _saveDataStream.Flush();
            using (var file = File.OpenWrite(fileName ?? FileName))
            {
                _saveDataStream.WriteTo(file);
                file.Flush();
            }
        }

        public override string ToString()
        {
            return "[' " + Header
                   + "'" + Environment.NewLine + "'" + Party
                //+ "' '" + Events.ToString()
                   + "']";
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
            if (_saveDataStream != null)
            {
                _saveDataStream.Dispose();
            }
        }
    }
}