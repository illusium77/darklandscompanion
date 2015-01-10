using System.Collections.Generic;
using DarklandsUiCommon.ViewModels;

namespace DarklandsUiCommon.Views
{
    /// <summary>
    ///     Interaction logic for SelectDarklandFolderWindow.xaml
    /// </summary>
    public partial class SelectFolderDialog
    {
        public SelectFolderDialog()
        {
            InitializeComponent();
            DataContext = new SelectFolderDialogModel(OnButtonOkClick);
        }

        private SelectFolderDialogModel ViewModel
        {
            get { return (SelectFolderDialogModel) DataContext; }
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

        private void OnButtonOkClick()
        {
            DialogResult = true;
        }
    }
}