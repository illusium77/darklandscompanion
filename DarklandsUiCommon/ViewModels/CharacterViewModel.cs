using DarklandsBusinessObjects.Objects;

namespace DarklandsUiCommon.ViewModels
{
    public class CharacterViewModel : ViewModelBase
    {
        private Character m_character;
        public Character Character
        {
            get { return m_character; }
            set
            {
                m_character = value;
                NotifyPropertyChanged();
            }
        }

        public CharacterViewModel(Character character)
        {
            Character = character;
        }
    }
}
