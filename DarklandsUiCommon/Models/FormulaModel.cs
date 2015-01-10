using System;
using System.Linq;
using System.Text;
using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.ViewModels;

namespace DarklandsUiCommon.Models
{
    public class FormulaModel : ModelBase
    {
        private readonly Formula[] _formulae;
        private readonly FormulaeBitmask _knownFormulae;

        public FormulaModel(Formula[] formulae, FormulaeBitmask knownFormulae)
        {
            if (formulae.Length != 3)
            {
                throw new ArgumentException("Model requires three formulas");
            }

            // order by ql just in case
            _formulae = (from f in formulae
                orderby f.Quality
                select f).ToArray();

            _knownFormulae = knownFormulae;

            Type = _formulae[0].Type; // all should have same type
            Description = _formulae[0].Description;

            Quality25Tip = CreateTooltip(_formulae[0]);
            Quality35Tip = CreateTooltip(_formulae[1]);
            Quality45Tip = CreateTooltip(_formulae[2]);
        }

        public bool HasQuality25
        {
            get { return _knownFormulae.HasFormula(_formulae[0].Id); }
            set
            {
                if (value)
                {
                    _knownFormulae.LearnFormula(_formulae[0].Id);
                }
                else
                {
                    _knownFormulae.ForgetFormula(_formulae[0].Id);
                }
                NotifyPropertyChanged();
            }
        }

        public bool HasQuality35
        {
            get { return _knownFormulae.HasFormula(_formulae[1].Id); }
            set
            {
                if (value)
                {
                    _knownFormulae.LearnFormula(_formulae[1].Id);
                }
                else
                {
                    _knownFormulae.ForgetFormula(_formulae[1].Id);
                }
                NotifyPropertyChanged();
            }
        }

        public bool HasQuality45
        {
            get { return _knownFormulae.HasFormula(_formulae[2].Id); }
            set
            {
                if (value)
                {
                    _knownFormulae.LearnFormula(_formulae[2].Id);
                }
                else
                {
                    _knownFormulae.ForgetFormula(_formulae[2].Id);
                }
                NotifyPropertyChanged();
            }
        }

        public string Type { get; private set; }
        public string Description { get; private set; }
        public string Quality25Tip { get; private set; }
        public string Quality35Tip { get; private set; }
        public string Quality45Tip { get; private set; }

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