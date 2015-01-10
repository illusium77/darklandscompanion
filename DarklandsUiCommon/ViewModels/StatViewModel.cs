using DarklandsBusinessObjects.Objects;

namespace DarklandsUiCommon.ViewModels
{
    public class StatViewModel : ModelBase
    {
        private AttributeSet _attributes;
        private SkillSet _skills;

        public StatViewModel(Character character)
        {
            Attributes = character.MaxAttributes;
            Skills = character.Skills;
        }

        public AttributeSet Attributes
        {
            get { return _attributes; }
            set
            {
                _attributes = value;
                NotifyPropertyChanged();
            }
        }

        public SkillSet Skills
        {
            get { return _skills; }
            set
            {
                _skills = value;
                NotifyPropertyChanged();
            }
        }
    }
}