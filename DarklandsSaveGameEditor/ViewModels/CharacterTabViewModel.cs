using DarklandsBusinessObjects.Objects;
using DarklandsBusinessObjects.Save;
using DarklandsServices.Services;
using DarklandsUiCommon.Models;
using DarklandsUiCommon.ViewModels;

namespace DarklandsSaveGameEditor.ViewModels
{
    public class CharacterTabViewModel : ModelBase
    {
        private Character _character;
        private CharacterDetailsViewModel _detailsVm;
        private EquipmentListViewModel _equipmentListVm;
        private FormulaeViewModel _formulaeVm;
        private SaintViewModel _saintVm;
        private StatViewModel _statVm;

        public CharacterTabViewModel(Character character, SaveParty party)
        {
            Character = character;

            DetailsVm = new CharacterDetailsViewModel(Character, party);
            StatVm = new StatViewModel(Character);
            SaintVm = new SaintViewModel(StaticDataService.Saints, Character.SaintBitmask);
            FormulaeVm = new FormulaeViewModel(StaticDataService.Formulae, Character.FormulaeBitmask);
            EquipmentListVm = new EquipmentListViewModel(Character, StaticDataService.ItemDefinitions);
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

        public CharacterDetailsViewModel DetailsVm
        {
            get { return _detailsVm; }
            set
            {
                _detailsVm = value;
                NotifyPropertyChanged();
            }
        }

        public StatViewModel StatVm
        {
            get { return _statVm; }
            set
            {
                _statVm = value;
                NotifyPropertyChanged();
            }
        }

        public SaintViewModel SaintVm
        {
            get { return _saintVm; }
            set
            {
                _saintVm = value;
                NotifyPropertyChanged();
            }
        }

        public FormulaeViewModel FormulaeVm
        {
            get { return _formulaeVm; }
            set
            {
                _formulaeVm = value;
                NotifyPropertyChanged();
            }
        }

        public EquipmentListViewModel EquipmentListVm
        {
            get { return _equipmentListVm; }
            set
            {
                _equipmentListVm = value;
                NotifyPropertyChanged();
            }
        }
    }
}