using System.Collections.Generic;
using System.Linq;
using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Save;
using DarklandsUiCommon.Models;

namespace DarklandsUiCommon.ViewModels
{
    public class CharacterDetailsViewModel : ModelBase
    {
        private readonly SaveParty _party;

        private Dictionary<int, string> _shields = new Dictionary<int, string>
        {
            {65, "Grey with red stripe"},
            {66, "Nut"},
            {67, "Blue Grey"},
            {68, "Lion"},
            {69, "Red yellow"},
            {70, "Sword"},
            {71, "Heart"},
            {72, "Rose"},
            {73, "Red with grey stripe"},
            {74, "Black cross"},
            {75, "Grey cross -thingy"},
            {76, "Red yellow checkers"},
            {77, "Eagle"},
            {78, "Red yellow stripes"},
            {79, "Black grey cross"}
        };

        public Dictionary<int, string> Shields
        {
            get { return _shields; }
            set { _shields = value; }
        }

        public KeyValuePair<int, string> SelectedShield
        {
            get { return _shields.FirstOrDefault(k => k.Key == Character.Shield); }
            set
            {
                Character.Shield = value.Key;
                NotifyPropertyChanged();
            }
        }

        private Dictionary<string, string> _playerImages = new Dictionary<string, string>
        {
            {"A00", "Alchemist"},
            {"C00", "Cleric"},
            {"F01", "Male Fighter"},
            {"F60", "Female Fighter"}
        };

        public Dictionary<string, string> PlayerImages
        {
            get { return _playerImages; }
            set { _playerImages = value; }
        }
        
        public KeyValuePair<string, string> SelectedPlayerImage
        {
            get
            {
                var key = _party.GetCharacterImage(Character.Id);
                return _playerImages.FirstOrDefault(k => k.Key == key);
            }
            set
            {
                _party.SetCharacterImage(Character.Id, value.Key);
                NotifyPropertyChanged();
            }
        }


        private Character _character;
        private CharacterColors _colors;

        public bool IsFemale
        {
            get
            {
                return _character.Gender == 1;
            }
            set
            {
                _character.Gender = value ? 1 : 0;
                NotifyPropertyChanged();
            }
        }

        public CharacterDetailsViewModel(Character character, SaveParty party)
        {
            _party = party;

            Character = character;
            Colors = party.GetCharacterColors(Character.Id);
        }

        public CharacterColors Colors
        {
            get { return _colors; }
            set
            {
                _colors = value;
                NotifyPropertyChanged();
            }
        }

        public Character Character
        {
            get { return _character; }
            set
            {
                _character = value;
                NotifyPropertyChanged();
            }
        }
    }
}
