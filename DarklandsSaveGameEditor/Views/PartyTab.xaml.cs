using DarklandsUiCommon.ViewServices;

namespace DarklandsSaveGameEditor.Views
{
    /// <summary>
    ///     Interaction logic for PartyTab.xaml
    /// </summary>
    public partial class PartyTab
    {
        public PartyTab()
        {
            InitializeComponent();

            Loaded += (s, e) => { ErrorMonitor.Register(PartyGrid); };
        }
    }
}