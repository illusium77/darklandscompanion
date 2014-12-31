using DarklandsServices.Services;
using DarklandsUiCommon.AppConfiguration;
using DarklandsUiCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsCompanion.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
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

        public IEnumerable<string> MissingFiles
        {
            get
            {
                return StaticDataService.SetDarklandsPath(
                    AppConfig.ReadSetting<string>(AppConfig.SETTING_DARKLANDS_PATH))
                    ? Enumerable.Empty<string>()
                    : StaticDataService.RequiredFiles;

            }
        }

        public MainWindowViewModel()
        {
            MessageVm = new MessageViewModel();
            SaintSearchVm = new SaintSearchViewModel();
        }

        public void Start()
        {
            var path = AppConfig.ReadSetting<string>(AppConfig.SETTING_DARKLANDS_PATH);
            if (StaticDataService.SetDarklandsPath(path))
            {
                LiveDataService.ConnectionMonitor += OnConnected;
                LiveDataService.Connect();
            }
            else
            {
                MessageVm.Messages = "Error: Cannot find Darklands from the folder '" + path + "'";
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


        public void SetDarklandPath(string path)
        {
            AppConfig.AddUpdateAppSettings(
                AppConfig.SETTING_DARKLANDS_PATH, path);
            Start();
        }
    }
}
