using DarklandsUiCommon.ViewServices;

namespace DarklandsUiCommon.Views.Character
{
    /// <summary>
    ///     Interaction logic for AttributeView.xaml
    /// </summary>
    public partial class AttributeView
    {
        public AttributeView()
        {
            InitializeComponent();

            Loaded += (s, e) => { ErrorMonitor.Register(this); };
        }
    }
}