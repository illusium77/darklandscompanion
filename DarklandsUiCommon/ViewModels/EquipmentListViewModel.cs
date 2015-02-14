using System.Collections.Generic;
using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.Models;

namespace DarklandsUiCommon.ViewModels
{
    public class EquipmentListViewModel : ModelBase
    {
        private Character _character;
        private EquipmentListModel _equipment;

        public EquipmentListViewModel(Character character, IReadOnlyList<ItemDefinition> itemDefinitions)
        {
            Character = character;
            _equipment = new EquipmentListModel(character, itemDefinitions);
        }

        public EquipmentListModel Equipment
        {
            get { return _equipment; }
            set
            {
                _equipment = value;
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