using System.Collections.Generic;
using System.Linq;
using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.Models;

namespace DarklandsUiCommon.ViewModels
{
    public class CharacterDetailsViewModel :  ModelBase
    {
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


        public KeyValuePair<int, string> SelectedShield
        {
            get { return _shields.FirstOrDefault(k => k.Key == Character.Shield); }
            set
            {
                Character.Shield = value.Key;
                NotifyPropertyChanged();
            }
        }



        private Character _character;

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

        public CharacterDetailsViewModel(Character character)
        {
            Character = character;
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

        public Dictionary<int, string> Shields
        {
            get { return _shields; }
            set { _shields = value; }
        }
    }
}
