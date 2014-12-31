using DarklandsUiCommon.DataValidation;
using System.Linq;
using System.Windows.Controls;

namespace DarklandsUiCommon.Views.Character
{
    /// <summary>
    /// Interaction logic for SkillView.xaml
    /// </summary>
    public partial class SkillView : Grid
    {
        public SkillView()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                ErrorMonitor.Register(this);
            };
        }
    }
}
