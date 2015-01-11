using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using DarklandsBusinessObjects.Save;
using DarklandsServices.Services;
using DarklandsUiCommon.Commands;
using DarklandsUiCommon.DataValidation;
using DarklandsUiCommon.ViewModels;
using Microsoft.Win32;

namespace DarklandsSaveGameEditor.ViewModels
{
    internal class MainWindowViewModel : ModelBase
    {
        private const string DefaultTitle = "Darklands Save Game Editor";
        private SaveGameViewModel _saveGameVm;
        private string _title;

        public MainWindowViewModel()
        {
            Title = DefaultTitle;

            SaveGameVm = new SaveGameViewModel();

            LoadCommand = new UiCommand(OnLoad);
            LoadLastestCommand = new UiCommand(OnLoadLatest, CanLoadLatest);
            SaveCommand = new UiCommand(OnSave, CanSave);

            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                LoadSave(args[1]);
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

        public SaveGameViewModel SaveGameVm
        {
            get { return _saveGameVm; }
            set
            {
                _saveGameVm = value;
                NotifyPropertyChanged();
            }
        }

        public bool MakeBackup
        {
            get
            {
                return !ConfigurationService.HasSetting(ConfigType.DarklandsSaveGameEditor, ConfigurationService.SettingBackupSavegame)
                    || ConfigurationService.ReadSetting<bool>(ConfigType.DarklandsSaveGameEditor, ConfigurationService.SettingBackupSavegame);
            }
            set
            {
                ConfigurationService.AddUpdateAppSettings(
                    ConfigType.DarklandsSaveGameEditor, ConfigurationService.SettingBackupSavegame, value.ToString());

                NotifyPropertyChanged();
            }
        }

        public ICommand LoadCommand { get; private set; }
        public ICommand LoadLastestCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        private bool CanSave()
        {
            return SaveGameVm.SaveGame != null && !ErrorMonitor.HasErrors;
        }

        private void OnSave()
        {
            foreach (var character in SaveGameVm.SaveGame.Party.Characters)
            {
                // Copy modified max attributes to current so 
                // user does not need to rest to gain missing points.
                // Hopefully this does not mess things up if character has
                // active temporary buffs
                var current = character.CurrentAttributes;
                var max = character.MaxAttributes;

                if (current.Agility < max.Agility)
                {
                    current.Agility = max.Agility;
                }
                if (current.Charisma < max.Charisma)
                {
                    current.Charisma = max.Charisma;
                }
                if (current.DivineFavor < max.DivineFavor)
                {
                    current.DivineFavor = max.DivineFavor;
                }
                if (current.Endurance < max.Endurance)
                {
                    current.Endurance = max.Endurance;
                }
                if (current.Intelligence < max.Intelligence)
                {
                    current.Intelligence = max.Intelligence;
                }
                if (current.Perception < max.Perception)
                {
                    current.Perception = max.Perception;
                }
                if (current.Strength < max.Strength)
                {
                    current.Strength = max.Strength;
                }
            }

            SaveGameVm.SaveGame.Save();
        }

        private void OnLoad()
        {
            var openFileDialog = new OpenFileDialog
            {
                DefaultExt = ".sav",
                Filter = "Darklands save (.sav)|*.sav"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadSave(openFileDialog.FileName);
            }
        }

        private bool CanLoadLatest()
        {
            return ConfigurationService.HasDarklandsPath(ConfigType.DarklandsSaveGameEditor);
        }

        private void OnLoadLatest()
        {
            var path = Path.Combine(ConfigurationService.GetDarklandsPath(ConfigType.DarklandsSaveGameEditor), "saves");

            if (!Directory.Exists(path))
            {
                return;
            }

            var latestSave = (
                from f in new DirectoryInfo(path).EnumerateFiles("*.sav")
                orderby f.LastWriteTime descending
                select f.FullName).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(latestSave))
            {
                LoadSave(latestSave);
            }

        }

        private void LoadSave(string fileName)
        {
            if (File.Exists(fileName))
            {
                var save = new SaveGame(fileName);
                SaveGameVm.SetSave(save);
                Title = DefaultTitle + " - " + Path.GetFileName(fileName) + " " + save.Header.Date + " " + save.Header.Label;

                var backup = (fileName + ".backup").ToUpper();
                if (MakeBackup && !File.Exists(backup))
                {
                    SaveGameVm.SaveGame.Save(backup);
                }
            }
        }
    }
}