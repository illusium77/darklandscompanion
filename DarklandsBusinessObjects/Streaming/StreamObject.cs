using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarklandsBusinessObjects.Utils;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DarklandsBusinessObjects.Streaming
{

    public abstract class StreamObject : IDisposable, INotifyPropertyChanged
    {
        // When spawning sub streams for the contained object, remember to add base offset!
        protected ByteStream DataStream { get; private set; }
        protected int BaseOffset { get; private set; }
        protected int Length { get; private set; }

        private BinaryReader m_reader;
        private BinaryWriter m_writer;

        public StreamObject(ByteStream dataStream, int offset, int length = 0)
        {
            // zero length means that length is initially unknown
            if (length != 0 && dataStream.Length < offset + length)
            {
                throw new InvalidOperationException("Invalid stream length");
            }

            DataStream = dataStream;
            BaseOffset = offset;
            Length = length;

            m_reader = new BinaryReader(DataStream);
            m_writer = new BinaryWriter(DataStream);
        }

        public byte this[int index]
        {
            get
            {
                return GetByte(index);
            }
            set
            {
                SetByte(index, value);
            }
        }

        public byte GetByte(int startIndex)
        {
            DataStream.Seek(BaseOffset + startIndex);
            return m_reader.ReadByte();
        }

        public void SetByte(int startIndex, byte value)
        {
            DataStream.Seek(BaseOffset + startIndex);
            m_writer.Write(value);
        }

        public int GetWord(int startIndex)
        {
            DataStream.Seek(BaseOffset + startIndex);
            return m_reader.ReadInt16();
        }

        public void SetWord(int startIndex, int value)
        {
            DataStream.Seek(BaseOffset + startIndex);
            m_writer.Write((short)value);
        }

        public string GetString(int startIndex, int length)
        {
            DataStream.Seek(BaseOffset + startIndex);
            return StringHelper.ConvertToString(m_reader.ReadBytes(length));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_reader != null)
                {
                    m_reader.Dispose();
                    m_reader = null;
                }
                if (m_writer != null)
                {
                    m_writer.Dispose();
                    m_writer = null;
                }
                if (DataStream != null)
                {
                    DataStream.Dispose();
                    DataStream = null;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property. 
        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument. 
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
