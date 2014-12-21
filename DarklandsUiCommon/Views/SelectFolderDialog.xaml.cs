using DarklandsUiCommon.ViewModels;
using System.Windows;

namespace DarklandsUiCommon.Views
{
    /// <summary>
    /// Interaction logic for SelectDarklandFolderWindow.xaml
    /// </summary>
    public partial class SelectDarklandFolderWindow : Window
    {


        public SelectFolderDialogModel ViewModel
        {
            get
            {
                return (SelectFolderDialogModel)DataContext;
            }
        }

        public SelectDarklandFolderWindow()
        {
            InitializeComponent();
            DataContext = new SelectFolderDialogModel(OnButtonOkClick);
        }

        private void OnButtonOkClick()
        {
            DialogResult = true;
        }
    }
}
