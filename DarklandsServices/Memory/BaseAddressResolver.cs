using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DarklandsServices.Memory
{
    class BaseAddressResolver : MemoryWorker<long>
    {
        private static byte[] DARKLANDS_EXE_SIGNATURE = new byte[] { 
            0xC8, 0x10, 0x00, 0x00, 0x57, 0x56, 0x8B, 0x1E,
            0x7C, 0x01, 0x2B, 0xFF, 0x8B, 0x47, 0x0C, 0x89 };

        public BaseAddressResolver(Action<long> callback)
            : base(callback, false)
        {
        }

        protected override void OnStart(object sender, DoWorkEventArgs e)
        {
            var addresses = m_accessor.SearchMemory(DARKLANDS_EXE_SIGNATURE);
            if (addresses.Count() != 1)
            {
                e.Cancel = true;
            }
            else
            {
                e.Result = addresses.First();
            }
        }
    }
}
