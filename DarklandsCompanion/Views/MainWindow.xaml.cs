using DarklandsCompanion.ViewModels;
using DarklandsUiCommon.Views;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace DarklandsCompanion.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
                {
                    var model = new MainWindowViewModel();
                    DataContext = model;

                    var missingFiles = model.MissingFiles;
                    if (missingFiles.Any())
                    {
                        var dialog = new SelectDarklandFolderWindow();
                        dialog.Owner = this;
                        dialog.ViewModel.RequiredFiles = missingFiles;

                        if (dialog.ShowDialog() == true)
                        {
                            model.SetDarklandPath(dialog.ViewModel.SelectedPath);
                        }
                        else
                        {
                            Close();
                        }
                    }

                    model.Start();
                };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Stop();
            base.OnClosing(e);
        }
    }
}
