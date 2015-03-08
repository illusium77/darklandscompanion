using DarklandsUiCommon.ViewServices;

namespace DarklandsUiCommon.Views.Character
{
    /// <summary>
    /// Interaction logic for RgbView.xaml
    /// </summary>
    public partial class RgbView
    {
        public RgbView()
        {
            InitializeComponent();
            Loaded += (s, e) => { ErrorMonitor.Register(this); };

            DataContextChanged += RgbView_DataContextChanged;
        }

        void RgbView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
