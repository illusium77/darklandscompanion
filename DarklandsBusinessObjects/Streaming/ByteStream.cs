using System.IO;

namespace DarklandsBusinessObjects.Streaming
{
    public class ByteStream : MemoryStream
    {
        public ByteStream(byte[] bytes)
            : base(bytes, 0, bytes.Length, true, true)
        {
        }

        public void Seek(int startIndex)
        {
            Seek(startIndex, SeekOrigin.Begin);
        }
    }
}