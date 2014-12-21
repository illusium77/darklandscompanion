using DarklandsServices.Services;
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

        public IEnumerable<string> MissingFiles
        {
            get
            {
                return StaticDataService.SetDarklandsPath(
                    ConfigurationManager.AppSettings["darklandsPath"])
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
            var path = ConfigurationManager.AppSettings["darklandsPath"];
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

            if (isConnected && !MessageVm.IsListening)
            {
                MessageVm.Start();
            }
        }


        public void SetDarklandPath(string path)
        {
            AddUpdateAppSettings("darklandsPath", path);
            Start();
        }

        private static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
