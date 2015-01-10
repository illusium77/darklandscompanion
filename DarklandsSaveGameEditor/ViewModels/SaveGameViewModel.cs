using DarklandsBusinessObjects.Save;
using DarklandsUiCommon.ViewModels;
using System.Collections.Generic;
using System.Windows;

namespace DarklandsSaveGameEditor.ViewModels
{
    internal class SaveGameViewModel : ModelBase
    {
        private SaveGame m_saveGame;
        public SaveGame SaveGame
        {
            get { return m_saveGame; }
            private set
            {
                m_saveGame = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<CharacterTabViewModel> m_characterTabVms;
        public IEnumerable<CharacterTabViewModel> CharacterTabVms
        {
            get { return m_characterTabVms; }
            private set
            {
                m_characterTabVms = value;
                NotifyPropertyChanged();
            }
        }

        private QuestTabVm m_questTabVm;
        public QuestTabVm QuestTabVm
        {
            get { return m_questTabVm; }
            private set
            {
                m_questTabVm = value;
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

        public void SetSave(SaveGame saveGame)
        {
            SaveGame = saveGame;
            CharacterTabVms = null;
            QuestTabVm = null;

            if (saveGame != null)
            {
                var tabs = new List<CharacterTabViewModel>();
                foreach (var character in SaveGame.Party.Characters)
                {
                    tabs.Add(new CharacterTabViewModel(character));
                }
                CharacterTabVms = tabs;

                QuestTabVm = new QuestTabVm(SaveGame.Events);
            }

            NotifyPropertyChanged("Visibility");
        }
    }
}
