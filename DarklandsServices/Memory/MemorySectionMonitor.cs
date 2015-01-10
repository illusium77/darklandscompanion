using System;
using System.ComponentModel;

namespace DarklandsServices.Memory
{
    internal class MemorySectionMonitor : MemoryWorker<byte[]>
    {
        private readonly long _address;
        private readonly int _dataSize;

        public MemorySectionMonitor(long address, int dataSize, Action<byte[]> callback)
            : base(callback, true)
        {
            _address = address;
            _dataSize = dataSize;
        }

        protected override void OnStart(object sender, DoWorkEventArgs e)
        {
            Accessor.StartPolling(_address, _dataSize, 1000, bytes => { Worker.ReportProgress(0, bytes); });
        }
    }
}