using DarklandsBusinessObjects.Objects;
using DarklandsServices.Services;
using DarklandsUiCommon.ViewModels;

namespace DarklandsCompanion.ViewModels
{
    public class MainWindowViewModel : ModelBase
    {
        private bool _isConnected;
        private MessageViewModel _messageVm;
        private SaintSearchViewModel _saintSearchVm;
        private string _title;

        public MainWindowViewModel()
        {
            MessageVm = new MessageViewModel();
            SaintSearchVm = new SaintSearchViewModel();
        }

        public MessageViewModel MessageVm
        {
            get { return _messageVm; }
            set
            {
                _messageVm = value;
                NotifyPropertyChanged();
            }
        }

        public SaintSearchViewModel SaintSearchVm
        {
            get { return _saintSearchVm; }
            set
            {
                _saintSearchVm = value;
                NotifyPropertyChanged();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                NotifyPropertyChanged();
            }
        }

        public bool Start()
        {
            var path = ConfigurationService.GetDarklandsPath(ConfigType.DarklandsCompanion);
            if (StaticDataService.SetDarklandsPath(path))
            {
                LiveDataService.ConnectionMonitor += OnConnected;
                LiveDataService.Connect();

                return true;
            }
            MessageVm.Messages = "Error: Cannot find Darklands from the folder '" + path + "'";

            return false;
        }

        public void Stop()
        {
            LiveDataService.Stop();
        }

        private void OnConnected(bool isConnected)
        {
            Title = "Darklands Companion - " +
                    (isConnected ? "Connected" : "Looking for Darklands process...");
            IsConnected = isConnected;

            if (isConnected && !MessageVm.IsListening)
            {
                MessageVm.Start();

                LiveDataService.MonitorDate(OnDateChanged);
            }
        }

        private void OnDateChanged(Date date)
        {
            Title = string.Format("Darklands Companion - Connected - {0} {1:00}h", date, date.Hour);
        }
    }
}