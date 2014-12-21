using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsServices.Memory
{
    class MemorySectionMonitor : MemoryWorker<byte[]>
    {
        private long m_address;
        private int m_dataSize;

        public MemorySectionMonitor(long address, int dataSize, Action<byte[]> callback)
            : base(callback, true)
        {
            m_address = address;
            m_dataSize = dataSize;
        }

        protected override void OnStart(object sender, DoWorkEventArgs e)
        {
            m_accessor.StartPolling(m_address, m_dataSize, 1000, bytes =>
            {
                m_worker.ReportProgress(0, bytes);
            });
        }
    }
}
