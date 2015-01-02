using DarklandsCompanion.ViewModels;
using DarklandsServices.Services;
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

                    if (!ConfigurationService.HasDarklandsPath(ConfigType.DarklandsCompanion)
                        && !StaticDataService.SetDarklandsPath(ConfigurationService.GetDarklandsPath(ConfigType.DarklandsCompanion)))
                    {
                        var dialog = new SelectFolderDialog
                        {
                            RequiredFiles = StaticDataService.RequiredFiles,
                            Owner = this
                        };

                        if (dialog.ShowDialog() == true)
                        {
                            ConfigurationService.SetDarklandsPath(dialog.SelectedPath);
                        }
                        else
                        {
                            Close();
                        }
                    }

                    if (!model.Start())
                    {
                        Close();
                    }
                };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Stop();
            base.OnClosing(e);
        }
    }
}
