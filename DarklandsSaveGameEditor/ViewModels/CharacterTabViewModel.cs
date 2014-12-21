using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsSaveGameEditor.ViewModels
{
    public class CharacterTabViewModel : ViewModelBase
    {
        private CharacterViewModel m_characterVm;
        public CharacterViewModel CharacterVm
        {
            get { return m_characterVm; }
            set
            {
                m_characterVm = value;
                NotifyPropertyChanged();
            }
        }

        public CharacterTabViewModel(Character character)
        {
            CharacterVm = new CharacterViewModel(character);
        }
    }
}
