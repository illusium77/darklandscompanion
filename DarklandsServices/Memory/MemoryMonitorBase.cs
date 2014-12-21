using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsServices.Memory
{
    public interface IMemoryWorker : IDisposable
    {
        void Start();
        void Stop();
    }

    abstract class MemoryWorker<T> : IMemoryWorker
    {
        private const string PROCESS_NAME = "dosbox";
        
        protected MemoryAccessor m_accessor;
        protected BackgroundWorker m_worker;

        private Action<T> m_dataReadCb;
        
        private bool m_isContinous;

        public MemoryWorker(Action<T> dataReadCb, bool isContinous)
        {
            m_isContinous = isContinous;

            m_worker = new BackgroundWorker();
            m_worker.WorkerSupportsCancellation = true;
            if (isContinous)
            {
                m_worker.WorkerReportsProgress = true;
            }

            m_dataReadCb = dataReadCb;
        }

        public void Start()
        {
            if (!m_worker.IsBusy)
            {
                m_accessor = new MemoryAccessor(PROCESS_NAME);

                m_worker.DoWork += OnStart;
                m_worker.RunWorkerCompleted += OnDone;

                if (m_isContinous)
                {
                    m_worker.ProgressChanged += OnProgressChanged;
                }

                m_worker.RunWorkerAsync();
            }
        }

        public void Stop()
        {
            if (m_worker.IsBusy)
            {
                m_worker.CancelAsync();

                m_worker.DoWork -= OnStart;
                m_worker.RunWorkerCompleted -= OnDone;

                if (m_isContinous)
                {
                    m_worker.ProgressChanged -= OnProgressChanged;
                }

                m_accessor.Stop();
            }
        }

        protected abstract void OnStart(object sender, DoWorkEventArgs e);

        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SendResult(e.UserState);
        }

        private void OnDone(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!m_isContinous && !e.Cancelled)
            {
                SendResult(e.Result);
            }

            if (m_accessor != null)
            {
                m_accessor.Stop();
            }
        }

        private void SendResult(object result)
        {
            var data = (T)result;
            if (data != null && m_dataReadCb != null)
            {
                m_dataReadCb(data);
            }
        }

        public void Dispose()
        {
            if (m_worker.IsBusy)
            {
                Stop();
            }
            else
            {
                if (m_accessor != null)
                {
                    m_accessor.Dispose();
                }
                if (m_worker != null)
                {
                    m_worker.Dispose();
                }
            }
        }
    }
}

