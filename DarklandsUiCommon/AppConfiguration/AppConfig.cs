using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsUiCommon.AppConfiguration
{
    public static class AppConfig
    {
        public const string SETTING_DARKLANDS_PATH = "darklandsPath";
        public const string SETTING_BACKUP_SAVEGAME = "backupSaveGame";

        private static Configuration s_configFile;
        private static KeyValueConfigurationCollection s_settings;

        static AppConfig()
        {
            s_configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            s_settings = s_configFile.AppSettings.Settings;
        }

        public static bool HasSetting(string key)
        {
            return s_settings[key] != null && !string.IsNullOrWhiteSpace(s_settings[key].Value);
        }

        public static T ReadSetting<T>(string key)
        {
            var setting = s_settings[key];
            if (!HasSetting(key))
            {
                return default(T);
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromInvariantString(setting.Value);
        }

        public static void AddUpdateAppSettings(string key, string value)
        {
            if (s_settings[key] == null)
            {
                s_settings.Add(key, value);
            }
            else
            {
                s_settings[key].Value = value;
            }
            s_configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(s_configFile.AppSettings.SectionInformation.Name);
        }
    }
}
