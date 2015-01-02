using DarklandsSaveGameEditor.ViewModels;
using DarklandsServices.Services;
using DarklandsUiCommon.DataValidation;
using DarklandsUiCommon.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DarklandsSaveGameEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Loaded += (s, e) =>
            {
                if (!ConfigurationService.HasDarklandsPath(ConfigType.DarklandsSaveGameEditor)
                    && !StaticDataService.SetDarklandsPath(ConfigurationService.GetDarklandsPath(ConfigType.DarklandsSaveGameEditor)))
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

                StaticDataService.SetDarklandsPath(ConfigurationService.GetDarklandsPath(ConfigType.DarklandsSaveGameEditor));

                var model = new MainWindowViewModel();
                DataContext = model;

            };

            Closing += (s, e) =>
                {
                    ErrorMonitor.ShutDown();
                };
        }
    }
}
