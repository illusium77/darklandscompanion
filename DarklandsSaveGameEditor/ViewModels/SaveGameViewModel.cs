using DarklandsBusinessObjects.Save;
using DarklandsUiCommon.ViewModels;
using System.Collections.Generic;
using System.Windows;

namespace DarklandsSaveGameEditor.ViewModels
{
    internal class SaveGameViewModel : ViewModelBase
    {
        private SaveGame m_saveGame;
        public SaveGame SaveGame
        {
            get { return m_saveGame; }
            set
            {
                m_saveGame = value;
                NotifyPropertyChanged();

                var tabs = new List<CharacterTabViewModel>();
                foreach (var character in SaveGame.Party.Characters)
                {
                    tabs.Add(new CharacterTabViewModel(character));
                }

                CharacterTabVms = tabs;

                NotifyPropertyChanged("Visibility");
            }
        }

        private IEnumerable<CharacterTabViewModel> m_characterTabVms;
        public IEnumerable<CharacterTabViewModel> CharacterTabVms
        {
            get { return m_characterTabVms; }
            set
            {
                m_characterTabVms = value;
                NotifyPropertyChanged();
            }
        }

        public Visibility Visibility
        {
            get
            {
                return SaveGame == null ? Visibility.Hidden : Visibility.Visible;
            }
        }
    }
}
