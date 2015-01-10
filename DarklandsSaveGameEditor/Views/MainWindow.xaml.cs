using DarklandsSaveGameEditor.ViewModels;
using DarklandsServices.Services;
using DarklandsUiCommon.DataValidation;
using DarklandsUiCommon.Views;

namespace DarklandsSaveGameEditor.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            Loaded += (s, e) =>
            {
                if (!ConfigurationService.HasDarklandsPath(ConfigType.DarklandsSaveGameEditor)
                    &&
                    !StaticDataService.SetDarklandsPath(
                        ConfigurationService.GetDarklandsPath(ConfigType.DarklandsSaveGameEditor)))
                {
                    var dialog = new SelectFolderDialog
                    {
                        RequiredFiles = StaticDataService.RequiredFiles,
                        Owner = this
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        ConfigurationService.SetDarklandsPath(dialog.SelectedPath);
                        if (!StaticDataService.SetDarklandsPath(dialog.SelectedPath))
                        {
                            Close();
                        }
                    }
                    else
                    {
                        Close();
                    }
                }

                StaticDataService.SetDarklandsPath(
                    ConfigurationService.GetDarklandsPath(ConfigType.DarklandsSaveGameEditor));

                var model = new MainWindowViewModel();
                DataContext = model;
            };

            Closing += (s, e) => { ErrorMonitor.ShutDown(); };
        }
    }
}