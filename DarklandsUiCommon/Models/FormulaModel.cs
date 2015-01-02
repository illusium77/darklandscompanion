using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsUiCommon.Models
{
    public class FormulaTypeModel : ModelBase
    {
        private Formula[] m_formulae;
        private FormulaeBitmask m_knownFormulae;

        public bool HasQuality25
        {
            get { return m_knownFormulae.HasFormula(m_formulae[0].Id); }
            set
            {
                if (value)
                {
                    m_knownFormulae.LearnFormula(m_formulae[0].Id);
                }
                else
                {
                    m_knownFormulae.ForgetFormula(m_formulae[0].Id);
                }
                NotifyPropertyChanged();
            }
        }

        public bool HasQuality35
        {
            get { return m_knownFormulae.HasFormula(m_formulae[1].Id); }
            set
            {
                if (value)
                {
                    m_knownFormulae.LearnFormula(m_formulae[1].Id);
                }
                else
                {
                    m_knownFormulae.ForgetFormula(m_formulae[1].Id);
                }
                NotifyPropertyChanged();
            }
        }

        public bool HasQuality45
        {
            get { return m_knownFormulae.HasFormula(m_formulae[2].Id); }
            set
            {
                if (value)
                {
                    m_knownFormulae.LearnFormula(m_formulae[2].Id);
                }
                else
                {
                    m_knownFormulae.ForgetFormula(m_formulae[2].Id);
                }
                NotifyPropertyChanged();
            }
        }

        public string Type { get; private set; }
        public string Description { get; private set; }

        public string Quality25Tip { get; private set; }
        public string Quality35Tip { get; private set; }
        public string Quality45Tip { get; private set; }

        public FormulaTypeModel(IEnumerable<Formula> formulae, FormulaeBitmask knownFormulae)
        {
            if (formulae.Count() != 3)
            {
                throw new ArgumentException("Model requires three formulas");
            }

            // order by ql just in case
            m_formulae = (from f in formulae
                         orderby f.Quality
                         select f).ToArray();

            m_knownFormulae = knownFormulae;

            Type = m_formulae[0].Type; // all should have same type
            Description = m_formulae[0].Description;

            Quality25Tip = CreateTooltip(m_formulae[0]);
            Quality35Tip = CreateTooltip(m_formulae[1]);
            Quality45Tip = CreateTooltip(m_formulae[2]);
        }

        private string CreateTooltip(Formula formula)
        {
            var sb = new StringBuilder();

            sb.AppendLine(formula.FullName);
            sb.Append(string.Format("Risk factor: {0}, Mystic number: {1}",
                formula.RiskFactor, formula.MysticNumber));

            return sb.ToString();
        }

    }
}
