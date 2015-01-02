using DarklandsBusinessObjects.Objects;
using DarklandsServices.Services;
using DarklandsUiCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsCompanion.ViewModels
{
    public class MessageViewModel : ModelBase
    {
        private ScreenType m_currentScreen;

        private string m_messages;
        public string Messages
        {
            get { return m_messages; }
            set
            {
                m_messages = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsListening { get; set; }

        public MessageViewModel()
        {
            IsListening = false;
        }

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

                Messages = sb.ToString();
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
                Messages = sb.ToString();
            }
        }
    }
}
