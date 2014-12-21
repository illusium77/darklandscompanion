using DarklandsBusinessObjects.Objects;
using DarklandsServices.Services;
using DarklandsUiCommon.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace DarklandsCompanion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScreenType m_currentScreen;
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
                {
                    var appSettings = ConfigurationManager.AppSettings;
                    if (!StaticDataService.SetDarklandsPath(appSettings["darklandsPath"]))
                    {
                        var dialog = new SelectDarklandFolderWindow();
                        dialog.Owner = this;
                        dialog.ViewModel.RequiredFiles = StaticDataService.RequiredFiles;

                        if (dialog.ShowDialog() == true)
                        {
                            appSettings["darklandsPath"] = dialog.ViewModel.SelectedPath;
                            AddUpdateAppSettings("darklandsPath", dialog.ViewModel.SelectedPath);

                            StaticDataService.SetDarklandsPath(dialog.ViewModel.SelectedPath);
                        }
                        else
                        {
                            Close();
                        }
                    }

                    LiveDataService.ConnectionMonitor += OnConnected;
                    LiveDataService.Initialize();
                };
        }

        private void OnConnected(bool isConnected)
        {
            Title = "Darklands Companion - " +
                (isConnected ? "Connected" : "Looking for Darklands process...");

            if (isConnected)
            {
                LiveDataService.MonitorCurrentScreen(OnScreenUpdated);
                LiveDataService.MonitorFormulae(UpdateFormulae);
                LiveDataService.MonitorSaints(UpdateSaints);

                //var party = LiveDataService.ReadParty();

                //var x  = "strength";
                //var sb = new StringBuilder();
                //sb.AppendLine("Characters with " + x + ": ");

                //foreach (var character in party)
                //{
                //    var saints = StaticDataService.FindSaints(character.SaintBitmask.SaintIds);

                //    var saintsWithX = from s in saints
                //                      where s.Description.ToLower().Contains(x)
                //                      select s.FullName;

                //    sb.AppendLine(character.ShortName + ": " + string.Join(", ", saintsWithX));
                //    sb.AppendLine();
                //}

                //Messages.Text = sb.ToString();
            }
        }

        private void OnScreenUpdated(string screenName)
        {
            m_currentScreen = LiveDataService.GetScreen(screenName);
        }

        private void UpdateFormulae(IEnumerable<Formula> formulae)
        {
            if (m_currentScreen == ScreenType.Alchemist
                && formulae.Count() == 4) // ackward hack #1
            {
                // #1 when entring the the alchemy screen memory section contains
                // 3 last formulae and text 'potions'. Skip that.
                var party = LiveDataService.ReadParty();


                var sb = new StringBuilder();
                foreach (var f in formulae)
                {
                    var ingredientsWithNames = from ing in f.Ingrediens
                                               let item = StaticDataService.ItemDefinitions.FirstOrDefault(i => i.Id == ing.ItemCode)
                                               select string.Format("{0} ({1})", (item != null ? item.ShortName : "???"), ing.Quantity);

                    var alreadyKnown = from c in party
                                       where c.HasFormula(f.Id)
                                       select c.ShortName;

                    sb.AppendLine(f.FullName + " (" + f.Quality + ") " + (alreadyKnown.Any() ? string.Join(", ", alreadyKnown) : string.Empty));
                    sb.AppendLine(string.Join(", ", ingredientsWithNames));

                    sb.AppendLine(f.Description);
                    sb.AppendLine();
                }

                Messages.Text = sb.ToString();
            }
        }

        private void UpdateSaints(IEnumerable<Saint> saints)
        {
            var party = LiveDataService.ReadParty();

            if (saints.Any())
            {
                var sb = new StringBuilder();
                foreach (var saint in saints)
                {
                    sb.AppendLine(saint.Clue);
                    var alreadyKnown = from c in party
                                       where c.HasSaint(saint.Id)
                                       select c.ShortName;

                    if (alreadyKnown.Any())
                    {
                        sb.AppendLine("Known by: " + string.Join(", ", alreadyKnown) + ".");
                    }

                    sb.AppendLine();
                }
                Messages.Text = sb.ToString();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            LiveDataService.Stop();

            base.OnClosing(e);
        }

        private void OnWindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ChangedButton == MouseButton.Left)
            //    this.DragMove();
        }

        private static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
