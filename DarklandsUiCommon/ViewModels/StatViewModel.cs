using DarklandsBusinessObjects.Objects;

namespace DarklandsUiCommon.ViewModels
{
    public class StatViewModel : ModelBase
    {
        private AttributeSet m_attributes;
        public AttributeSet Attributes
        {
            get { return m_attributes; }
            set
            {
                m_attributes = value;
                NotifyPropertyChanged();
            }
        }

        private SkillSet m_skills;
        public SkillSet Skills
        {
            get { return m_skills; }
            set
            {
                m_skills = value;
                NotifyPropertyChanged();
            }
        }

        public StatViewModel(Character character)
        {
            Attributes = character.MaxAttributes;
            Skills = character.Skills;
        }
    }
}
