using DarklandsBusinessObjects.Objects;
using DarklandsServices.Services;
using DarklandsUiCommon.ViewModels;

namespace DarklandsSaveGameEditor.ViewModels
{
    public class CharacterTabViewModel : ModelBase
    {
        private Character _character;
        private FormulaeViewModel _formulaeVm;
        private SaintViewModel _saintVm;
        private StatViewModel _statVm;

        public CharacterTabViewModel(Character character)
        {
            Character = character;

            StatVm = new StatViewModel(Character);
            SaintVm = new SaintViewModel(StaticDataService.Saints, Character.SaintBitmask);
            FormulaeVm = new FormulaeViewModel(StaticDataService.Formulae, Character.FormulaeBitmask);
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
    }
}