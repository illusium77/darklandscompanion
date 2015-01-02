using DarklandsBusinessObjects.Objects;
using DarklandsServices.Services;
using DarklandsUiCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsSaveGameEditor.ViewModels
{
    public class CharacterTabViewModel : ModelBase
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

        private StatViewModel m_statVm;
        public StatViewModel StatVm
        {
            get { return m_statVm; }
            set
            {
                m_statVm = value;
                NotifyPropertyChanged();
            }
        }

        private SaintViewModel m_saintVm;
        public SaintViewModel SaintVm
        {
            get { return m_saintVm; }
            set
            {
                m_saintVm = value;
                NotifyPropertyChanged();
            }
        }

        private FormulaeViewModel m_formulaeVm;
        public FormulaeViewModel FormulaeVm
        {
            get { return m_formulaeVm; }
            set
            {
                m_formulaeVm = value;
                NotifyPropertyChanged();
            }
        }

        public CharacterTabViewModel(Character character)
        {
            Character = character;

            StatVm = new StatViewModel(Character);
            SaintVm = new SaintViewModel(StaticDataService.Saints, Character.SaintBitmask);
            FormulaeVm = new FormulaeViewModel(StaticDataService.Formulae, Character.FormulaeBitmask);
        }

    }
}
