using System;
using System.ComponentModel;

namespace DarklandsServices.Memory
{
    public interface IMemoryWorker : IDisposable
    {
        void Start();
        void Stop();
    }

    internal abstract class MemoryWorker<T> : IMemoryWorker
    {
        private const string ProcessName = "dosbox";
        private readonly Action<T> _dataReadCb;
        private readonly bool _isContinous;
        protected MemoryAccessor Accessor;
        protected readonly BackgroundWorker Worker;

        public MemoryWorker(Action<T> dataReadCb, bool isContinous)
        {
            _isContinous = isContinous;

            Worker = new BackgroundWorker {WorkerSupportsCancellation = true};
            if (isContinous)
            {
                Worker.WorkerReportsProgress = true;
            }

            _dataReadCb = dataReadCb;
        }

        public void Start()
        {
            if (!Worker.IsBusy)
            {
                Accessor = new MemoryAccessor(ProcessName);

                Worker.DoWork += OnStart;
                Worker.RunWorkerCompleted += OnDone;

                if (_isContinous)
                {
                    Worker.ProgressChanged += OnProgressChanged;
                }

                Worker.RunWorkerAsync();
            }
        }

        public void Stop()
        {
            if (Worker.IsBusy)
            {
                Worker.CancelAsync();

                Worker.DoWork -= OnStart;
                Worker.RunWorkerCompleted -= OnDone;

                if (_isContinous)
                {
                    Worker.ProgressChanged -= OnProgressChanged;
                }

                Accessor.Stop();
            }
        }

        public void Dispose()
        {
            if (Worker.IsBusy)
            {
                Stop();
            }
            else
            {
                if (Accessor != null)
                {
                    Accessor.Dispose();
                }
                if (Worker != null)
                {
                    Worker.Dispose();
                }
            }
        }

        protected abstract void OnStart(object sender, DoWorkEventArgs e);

        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SendResult(e.UserState);
        }

        private void OnDone(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!_isContinous && !e.Cancelled)
            {
                SendResult(e.Result);
            }

            if (Accessor != null)
            {
                Accessor.Stop();
            }
        }

        private void SendResult(object result)
        {
            var data = (T) result;
            if (data != null && _dataReadCb != null)
            {
                _dataReadCb(data);
            }
        }
    }
}