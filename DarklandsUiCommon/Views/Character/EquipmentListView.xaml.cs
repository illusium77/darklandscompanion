using System.Windows.Controls;
using System.Windows.Input;
using DarklandsUiCommon.ViewServices;

namespace DarklandsUiCommon.Views.Character
{
    /// <summary>
    /// Interaction logic for EquipmentListView.xaml
    /// </summary>
    public partial class EquipmentListView
    {
        public EquipmentListView()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                ErrorMonitor.Register(EquipmentGrid);
            };
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scroller = sender as ScrollViewer;
            if (scroller == null) return;

            scroller.ScrollToVerticalOffset(scroller.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
