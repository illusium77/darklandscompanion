using DarklandsBusinessObjects.Save;
using DarklandsUiCommon.ViewModels;
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

                CharacterAVm = new CharacterTabViewModel(SaveGame.Party.Characters[0]);
                CharacterBVm = new CharacterTabViewModel(SaveGame.Party.Characters[1]);
                CharacterCVm = new CharacterTabViewModel(SaveGame.Party.Characters[2]);
                CharacterDVm = new CharacterTabViewModel(SaveGame.Party.Characters[3]);

                NotifyPropertyChanged("Visibility");
            }
        }

        private CharacterTabViewModel m_characterAVm;
        public CharacterTabViewModel CharacterAVm
        {
            get { return m_characterAVm; }
            set
            {
                m_characterAVm = value;
                NotifyPropertyChanged();
            }
        }

        private CharacterTabViewModel m_characterBVm;
        public CharacterTabViewModel CharacterBVm
        {
            get { return m_characterBVm; }
            set
            {
                m_characterBVm = value;
                NotifyPropertyChanged();
            }
        }

        private CharacterTabViewModel m_characterCVm;
        public CharacterTabViewModel CharacterCVm
        {
            get { return m_characterCVm; }
            set
            {
                m_characterCVm = value;
                NotifyPropertyChanged();
            }
        }

        private CharacterTabViewModel m_characterDVm;
        public CharacterTabViewModel CharacterDVm
        {
            get { return m_characterDVm; }
            set
            {
                m_characterDVm = value;
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
