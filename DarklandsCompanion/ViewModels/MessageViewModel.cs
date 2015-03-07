using System.Collections.Generic;
using System.Linq;
using System.Text;
using DarklandsBusinessObjects.Objects;
using DarklandsServices.Services;
using DarklandsUiCommon.Models;
using DarklandsUiCommon.ViewModels;

namespace DarklandsCompanion.ViewModels
{
    public class MessageViewModel : ModelBase
    {
        private ScreenType _currentScreen;
        private string _messages;

        public MessageViewModel()
        {
            IsListening = false;
        }

        public string Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsListening { get; set; }

        public void Start()
        {
            if (!IsListening)
            {
                IsListening = true;

                LiveDataService.MonitorCurrentScreen(OnScreenUpdated);
                LiveDataService.MonitorFormulae(UpdateFormulae);
                LiveDataService.MonitorSaints(UpdateSaints);
            }
        }

        private void OnScreenUpdated(string screenName)
        {
            _currentScreen = LiveDataService.GetScreen(screenName);
        }

        private void UpdateFormulae(IEnumerable<Formula> formulae)
        {
            var formulaList = formulae.ToList();

            if (_currentScreen == ScreenType.Alchemist
                && formulaList.Count == 4) // ackward hack #1
            {
                // #1 when entring the the alchemy screen memory section contains
                // 3 last formulaList and text 'potions'. Skip that.
                var party = LiveDataService.ReadParty().ToList();


                var sb = new StringBuilder();
                foreach (var f in formulaList)
                {
                    var formula = f;

                    var ingredientsWithNames =
                        from ing in formula.Ingrediens
                        let item = StaticDataService.ItemDefinitions.FirstOrDefault(i => i.Id == ing.ItemCode)
                        select string.Format("{0} ({1})", (item != null ? item.ShortName : "???"), ing.Quantity);

                    var alreadyKnown = (from c in party
                                        where c.HasFormula(formula.Id)
                                        select c.ShortName).ToList();

                    sb.AppendLine(f.FullName + " (" + f.Quality + ") " +
                                  (alreadyKnown.Any() ? string.Join(", ", alreadyKnown) : string.Empty));
                    sb.AppendLine(string.Join(", ", ingredientsWithNames));

                    sb.AppendLine(f.Description);
                    sb.AppendLine();
                }

                Messages = sb.ToString();
            }
        }

        private void UpdateSaints(IEnumerable<Saint> saints)
        {
            var party = LiveDataService.ReadParty().ToList();
            var saintList = saints.ToList();

            if (saintList.Any())
            {
                var sb = new StringBuilder();
                foreach (var saint in saintList)
                {
                    sb.AppendLine(saint.Clue);
                    var alreadyKnown = (from c in party
                                        where c.HasSaint(saint.Id)
                                        select c.ShortName).ToList();

                    if (alreadyKnown.Any())
                    {
                        sb.AppendLine("Known by: " + string.Join(", ", alreadyKnown) + ".");
                    }

                    sb.AppendLine();
                }
                Messages = sb.ToString();
            }
        }
    }
}