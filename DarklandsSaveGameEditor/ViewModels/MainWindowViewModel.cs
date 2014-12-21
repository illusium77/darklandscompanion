using DarklandsBusinessObjects.Save;
using DarklandsUiCommon.Commands;
using DarklandsUiCommon.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;

namespace DarklandsSaveGameEditor.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private const string DEFAULT_TITLE = "Darklands Save Game Editor";

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

        private SaveGameViewModel m_saveGameVm;
        public SaveGameViewModel SaveGameVm
        {
            get { return m_saveGameVm; }
            set
            {
                m_saveGameVm = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand LoadCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public MainWindowViewModel()
        {
            Title = DEFAULT_TITLE;

            SaveGameVm = new SaveGameViewModel();

            LoadCommand = new UiCommand(OnLoad);
            SaveCommand = new UiCommand(OnSave, CanSave);
        }

        private bool CanSave()
        {
            return SaveGameVm.SaveGame != null;
        }

        private void OnSave()
        {
            foreach (var character in SaveGameVm.SaveGame.Party.Characters)
            {
                // Copy modified max attributes to current so 
                // user does not need to rest to gain missing points
                // hopefully this does not mess things up if character has
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
            var openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".sav";
            openFileDialog.Filter = "Darkland save (.sav)|*.sav"; // Filter files by extension 

            if (openFileDialog.ShowDialog() == true)
            {
                SaveGameVm.SaveGame = new SaveGame(openFileDialog.FileName);

                Title = DEFAULT_TITLE + " - " + Path.GetFileName(openFileDialog.FileName);
            }
        }
    }
}
