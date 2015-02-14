using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using DarklandsBusinessObjects.Utils;

namespace DarklandsBusinessObjects.Streaming
{
    public abstract class StreamObject : IDisposable, INotifyPropertyChanged, IDataErrorInfo
    {
        private List<string> _errors = new List<string>();
        private BinaryReader _reader;
        private BinaryWriter _writer;

        protected StreamObject(ByteStream dataStream, int offset, int length = 0)
        {
            // zero length means that length is initially unknown
            if (length != 0 && dataStream.Length < offset + length)
            {
                throw new InvalidOperationException("Invalid stream length");
            }

            DataStream = dataStream;
            BaseOffset = offset;
            Length = length;

            _reader = new BinaryReader(DataStream);
            _writer = new BinaryWriter(DataStream);
        }

        // When spawning sub streams for the contained object, remember to add base offset!
        protected ByteStream DataStream { get; private set; }
        protected int BaseOffset { get; private set; }
        protected int Length { get; private set; }

        public int this[int index]
        {
            get { return GetByte(index); }
            set { SetByte(index, value); }
        }

        private int GetByte(int startIndex)
        {
            DataStream.Seek(BaseOffset + startIndex);
            return _reader.ReadByte();
        }

        private void SetByte(int startIndex, int value)
        {
            DataStream.Seek(BaseOffset + startIndex);

            if (value > byte.MaxValue)
            {
                _writer.Write(byte.MaxValue);
            }
            else if (value < byte.MinValue)
            {
                _writer.Write(byte.MinValue);
            }
            else
            {
                _writer.Write((byte) value);
            }
        }

        public int GetWord(int startIndex)
        {
            DataStream.Seek(BaseOffset + startIndex);
            return _reader.ReadInt16();
        }

        public void SetWord(int startIndex, int value)
        {
            DataStream.Seek(BaseOffset + startIndex);

            if (value > short.MaxValue)
            {
                _writer.Write(short.MaxValue);
            }
            else if (value < short.MinValue)
            {
                _writer.Write(short.MinValue);
            }
            else
            {
                _writer.Write((short) value);
            }
        }

        public string GetString(int startIndex, int length)
        {
            DataStream.Seek(BaseOffset + startIndex);
            return StringHelper.ConvertToString(_reader.ReadBytes(length));
        }

        public override string ToString()
        {
            return BitConverter.ToString(DataStream.GetBuffer(), BaseOffset, Length);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }
                if (_writer != null)
                {
                    _writer.Dispose();
                    _writer = null;
                }
                if (DataStream != null)
                {
                    DataStream.Dispose();
                    DataStream = null;
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

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

        #endregion

        #region IDataErrorInfo

        public string Error
        {
            get { return _errors.Any() ? string.Join(", ", _errors) : null; }
        }

        public string this[string propertyName]
        {
            get { return ValidateProperty(propertyName); }
        }

        private string ValidateProperty(string propertyName)
        {
            var info = GetType().GetProperty(propertyName);
            var validators = info.GetCustomAttributes(true).OfType<ValidationAttribute>().ToList();

            if (!validators.Any())
            {
                // property has no data annotation attibutes -> no validation required
                return string.Empty;
            }

            var value = info.GetValue(this);
            _errors = (from va in validators
                where !va.IsValid(value)
                select va.FormatErrorMessage(propertyName)).ToList();

            return Error;
        }

        #endregion
    }
}