using DarklandsUiCommon.DataValidation;
using System.Linq;
using System.Windows.Controls;

namespace DarklandsUiCommon.Views.Character
{
    /// <summary>
    /// Interaction logic for AttributeView.xaml
    /// </summary>
    public partial class AttributeView : Grid
    {
        public AttributeView()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                ErrorMonitor.Register(this);
            };
        }
    }
}
