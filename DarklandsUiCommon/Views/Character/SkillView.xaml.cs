using DarklandsUiCommon.ViewServices;

namespace DarklandsUiCommon.Views.Character
{
    /// <summary>
    ///     Interaction logic for SkillView.xaml
    /// </summary>
    public partial class SkillView
    {
        public SkillView()
        {
            InitializeComponent();

            Loaded += (s, e) => { ErrorMonitor.Register(this); };
        }
    }
}