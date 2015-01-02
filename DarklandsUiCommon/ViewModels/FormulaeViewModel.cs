using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsUiCommon.ViewModels
{
    public class FormulaeViewModel : ModelBase
    {
        private IEnumerable<FormulaTypeModel> m_formulae;
        public IEnumerable<FormulaTypeModel> Formulae
        {
            get { return m_formulae; }
            set
            {
                m_formulae = value;
                NotifyPropertyChanged();
            }
        }

        public FormulaeViewModel(IEnumerable<Formula> formulae, FormulaeBitmask knownFormulae)
        {
            Formulae = from f in formulae
                       group f by f.Type into g
                       orderby g.Key
                       select new FormulaTypeModel(g.ToList(), knownFormulae);
        }
    }
}
