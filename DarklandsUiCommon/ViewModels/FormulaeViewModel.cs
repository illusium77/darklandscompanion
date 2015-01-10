using System.Collections.Generic;
using System.Linq;
using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.Models;

namespace DarklandsUiCommon.ViewModels
{
    public class FormulaeViewModel : ModelBase
    {
        private IEnumerable<FormulaModel> _formulae;

        public FormulaeViewModel(IEnumerable<Formula> formulae, FormulaeBitmask knownFormulae)
        {
            Formulae = from f in formulae
                group f by f.Type
                into g
                orderby g.Key
                select new FormulaModel(g.ToArray(), knownFormulae);
        }

        public IEnumerable<FormulaModel> Formulae
        {
            get { return _formulae; }
            set
            {
                _formulae = value;
                NotifyPropertyChanged();
            }
        }
    }
}