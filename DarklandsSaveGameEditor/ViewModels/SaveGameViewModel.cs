using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DarklandsBusinessObjects.Save;
using DarklandsUiCommon.ViewModels;

namespace DarklandsSaveGameEditor.ViewModels
{
    internal class SaveGameViewModel : ModelBase
    {
        private IEnumerable<CharacterTabViewModel> _characterTabVms;
        private QuestTabVm _questTabVm;
        private SaveGame _saveGame;
        private Visibility _visibility;

        public SaveGame SaveGame
        {
            get { return _saveGame; }
            private set
            {
                _saveGame = value;
                NotifyPropertyChanged();
            }
        }

        public IEnumerable<CharacterTabViewModel> CharacterTabVms
        {
            get { return _characterTabVms; }
            private set
            {
                _characterTabVms = value;
                NotifyPropertyChanged();
            }
        }

        public QuestTabVm QuestTabVm
        {
            get { return _questTabVm; }
            private set
            {
                _questTabVm = value;
                NotifyPropertyChanged();
            }
        }

        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                NotifyPropertyChanged();
            }
        }

        public void SetSave(SaveGame saveGame)
        {
            SaveGame = saveGame;
            CharacterTabVms = null;
            QuestTabVm = null;

            if (saveGame != null)
            {
                var tabs = new List<CharacterTabViewModel>(
                    from c in SaveGame.Party.Characters
                    select new CharacterTabViewModel(c));

                CharacterTabVms = tabs;

                QuestTabVm = new QuestTabVm(SaveGame.Events);
            }

            Visibility = SaveGame == null ? Visibility.Hidden : Visibility.Visible;
        }
    }
}