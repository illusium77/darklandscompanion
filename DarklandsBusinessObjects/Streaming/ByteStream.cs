using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
