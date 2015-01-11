using System.Windows.Controls;

namespace DarklandsUiCommon.Views.Quest
{
    /// <summary>
    ///     Interaction logic for QuestView.xaml
    /// </summary>
    public partial class QuestListView
    {
        public QuestListView()
        {
            InitializeComponent();
        }

        private void OnPreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var scroller = sender as ScrollViewer;
            if (scroller == null) return;

            scroller.ScrollToVerticalOffset(scroller.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}