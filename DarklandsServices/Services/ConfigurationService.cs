using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsServices.Services
{
    public static class ConfigurationService
    {
        public const string SETTING_DARKLANDS_PATH = "darklandsPath";
        public const string SETTING_BACKUP_SAVEGAME = "backupSaveGame";

        private const string DarklandsCompanionFileName = "DarklandsCompanion.exe";
        private const string DarklandsSaveGameEditorFileName = "DarklandsSaveGameEditor.exe";

        private static Dictionary<ConfigType, Configuration> s_configFiles = new Dictionary<ConfigType, Configuration>();

        static ConfigurationService()
        {
            if (File.Exists(DarklandsCompanionFileName))
            {
                s_configFiles.Add(ConfigType.DarklandsCompanion, ConfigurationManager.OpenExeConfiguration(DarklandsCompanionFileName));               
            }
            if (File.Exists(DarklandsSaveGameEditorFileName))
            {
                s_configFiles.Add(ConfigType.DarklandsSaveGameEditor, ConfigurationManager.OpenExeConfiguration(DarklandsSaveGameEditorFileName));
            }
        }

        public static bool HasSetting(ConfigType type, string key)
        {
            if (type == ConfigType.Global)
            {
                throw new ArgumentException("Cannot use Global as a configuration type when reading settings!", "type");
            }

            if (!s_configFiles.ContainsKey(type))
            {
                return false;
            }

            return s_configFiles[type].AppSettings.Settings[key] != null && !string.IsNullOrWhiteSpace(s_configFiles[type].AppSettings.Settings[key].Value);
        }

        public static T ReadSetting<T>(ConfigType type, string key)
        {
            if (type == ConfigType.Global)
            {
                throw new ArgumentException("Cannot use Global as a configuration type when reading settings!", "type");
            }

            if (!s_configFiles.ContainsKey(type) || !HasSetting(type, key))
            {
                return default(T);
            }

            var setting = s_configFiles[type].AppSettings.Settings[key];
            var converter = TypeDescriptor.GetConverter(typeof(T));

            return (T)converter.ConvertFromInvariantString(setting.Value);
        }

        public static void AddUpdateAppSettings(ConfigType type, string key, string value)
        {
            if (type == ConfigType.Global)
            {
                foreach (var config in s_configFiles.Values)
                {
                    DoAddUpdateSettings(config, key, value);
                }
            }
            else if (s_configFiles.ContainsKey(type))
            {
                DoAddUpdateSettings(s_configFiles[type], key, value);
            }
            else
            {
                throw new ArgumentException("Could not find configuration for type " + type, "type");
            }
        }

        private static void DoAddUpdateSettings(Configuration config, string key, string value)
        {

            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }

        #region Helpers

        public static bool HasDarklandsPath(ConfigType type)
        {
            return ConfigurationService.HasSetting(
                type, ConfigurationService.SETTING_DARKLANDS_PATH);
        }

        public static string GetDarklandsPath(ConfigType type)
        {
            return ConfigurationService.ReadSetting<string>(
                type, ConfigurationService.SETTING_DARKLANDS_PATH);
        }

        public static void SetDarklandsPath(string path)
        {
            ConfigurationService.AddUpdateAppSettings(
                ConfigType.Global, ConfigurationService.SETTING_DARKLANDS_PATH, path);
        }

        #endregion
    }
}
