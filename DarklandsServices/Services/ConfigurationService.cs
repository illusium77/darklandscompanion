using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;

namespace DarklandsServices.Services
{
    public static class ConfigurationService
    {
        private const string SettingDarklandsPath = "darklandsPath";
        public const string SettingBackupSavegame = "backupSaveGame";
        private const string DarklandsCompanionFileName = "DarklandsCompanion.exe";
        private const string DarklandsSaveGameEditorFileName = "DarklandsSaveGameEditor.exe";

        private static readonly Dictionary<ConfigType, Configuration> ConfigFiles =
            new Dictionary<ConfigType, Configuration>();

        static ConfigurationService()
        {
            if (File.Exists(DarklandsCompanionFileName))
            {
                ConfigFiles.Add(ConfigType.DarklandsCompanion,
                    ConfigurationManager.OpenExeConfiguration(DarklandsCompanionFileName));
            }
            if (File.Exists(DarklandsSaveGameEditorFileName))
            {
                ConfigFiles.Add(ConfigType.DarklandsSaveGameEditor,
                    ConfigurationManager.OpenExeConfiguration(DarklandsSaveGameEditorFileName));
            }
        }

        public static bool HasSetting(ConfigType type, string key)
        {
            if (type == ConfigType.Global)
            {
                throw new ArgumentException("Cannot use Global as a configuration type when reading settings!", "type");
            }

            if (!ConfigFiles.ContainsKey(type))
            {
                return false;
            }

            return ConfigFiles[type].AppSettings.Settings[key] != null &&
                   !string.IsNullOrWhiteSpace(ConfigFiles[type].AppSettings.Settings[key].Value);
        }

        public static T ReadSetting<T>(ConfigType type, string key)
        {
            if (type == ConfigType.Global)
            {
                throw new ArgumentException("Cannot use Global as a configuration type when reading settings!", "type");
            }

            if (!ConfigFiles.ContainsKey(type) || !HasSetting(type, key))
            {
                return default(T);
            }

            var setting = ConfigFiles[type].AppSettings.Settings[key];
            var converter = TypeDescriptor.GetConverter(typeof (T));

            return (T) converter.ConvertFromInvariantString(setting.Value);
        }

        public static void AddUpdateAppSettings(ConfigType type, string key, string value)
        {
            if (type == ConfigType.Global)
            {
                foreach (var config in ConfigFiles.Values)
                {
                    DoAddUpdateSettings(config, key, value);
                }
            }
            else if (ConfigFiles.ContainsKey(type))
            {
                DoAddUpdateSettings(ConfigFiles[type], key, value);
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
            return HasSetting(
                type, SettingDarklandsPath);
        }

        public static string GetDarklandsPath(ConfigType type)
        {
            return ReadSetting<string>(
                type, SettingDarklandsPath);
        }

        public static void SetDarklandsPath(string path)
        {
            AddUpdateAppSettings(
                ConfigType.Global, SettingDarklandsPath, path);
        }

        #endregion
    }
}