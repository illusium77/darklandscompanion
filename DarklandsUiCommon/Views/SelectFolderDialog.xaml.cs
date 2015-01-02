using DarklandsUiCommon.ViewModels;
using System.Windows;
using System.Collections.Generic;

namespace DarklandsUiCommon.Views
{
    /// <summary>
    /// Interaction logic for SelectDarklandFolderWindow.xaml
    /// </summary>
    public partial class SelectFolderDialog : Window
    {
        private SelectFolderDialogModel ViewModel
        {
            get
            {
                return (SelectFolderDialogModel)DataContext;
            }
        }

        public IEnumerable<string> RequiredFiles
        {
            get { return ViewModel.RequiredFiles; }
            set { ViewModel.RequiredFiles = value; }
        }

        public string SelectedPath
        {
            get { return ViewModel.SelectedPath; }
        }

        public SelectFolderDialog()
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
