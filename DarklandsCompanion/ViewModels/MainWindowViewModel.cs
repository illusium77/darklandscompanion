using DarklandsServices.Services;
using DarklandsUiCommon.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace DarklandsCompanion.ViewModels
{
    public class MainWindowViewModel : ModelBase
    {
        private MessageViewModel m_messageVm;
        public MessageViewModel MessageVm
        {
            get { return m_messageVm; }
            set
            {
                m_messageVm = value;
                NotifyPropertyChanged();
            }
        }

        private SaintSearchViewModel m_saintSearchVm;
        public SaintSearchViewModel SaintSearchVm
        {
            get { return m_saintSearchVm; }
            set
            {
                m_saintSearchVm = value;
                NotifyPropertyChanged();
            }
        }

        private string m_title;
        public string Title
        {
            get { return m_title; }
            set
            {
                m_title = value;
                NotifyPropertyChanged();
            }
        }

        private bool m_isConnected;
        public bool IsConnected
        {
            get { return m_isConnected; }
            set
            {
                m_isConnected = value;
                NotifyPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            MessageVm = new MessageViewModel();
            SaintSearchVm = new SaintSearchViewModel();
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
            else
            {
                MessageVm.Messages = "Error: Cannot find Darklands from the folder '" + path + "'";

                return false;
            }
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
            }
        }


        //public void SetDarklandPath(string path)
        //{
        //    ConfigurationService.AddUpdateAppSettings(
        //        ConfigType.Global, ConfigurationService.SETTING_DARKLANDS_PATH, path);
        //    Start();
        //}
    }
}
