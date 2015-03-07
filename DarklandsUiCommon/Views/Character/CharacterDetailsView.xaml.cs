using DarklandsUiCommon.ViewServices;

namespace DarklandsUiCommon.Views.Character
{
    /// <summary>
    /// Interaction logic for CharacterDetailsView.xaml
    /// </summary>
    public partial class CharacterDetailsView
    {
        public CharacterDetailsView()
        {
            InitializeComponent();
            Loaded += (s, e) => { ErrorMonitor.Register(this); };
        }
    }
}
