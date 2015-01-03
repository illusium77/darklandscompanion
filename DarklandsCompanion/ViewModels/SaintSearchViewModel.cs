using DarklandsServices.Services;
using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DarklandsServices.Saints;

namespace DarklandsCompanion.ViewModels
{
    public class SaintSearchViewModel : ModelBase
    {
        private string DEFAULT_TEXT = "Enter the search term to the field above. Type 'help' to see known search terms.";

        private string m_helpText;

        private string m_filter;
        public string Filter
        {
            get { return m_filter; }
            set
            {
                m_filter = value;

                UpdateResults();
                NotifyPropertyChanged();
            }
        }

        private string m_result;
        public string Result
        {
            get { return m_result; }
            set
            {
                m_result = value;
                NotifyPropertyChanged();
            }
        }

        private bool m_knownOnly;
        public bool KnownOnly
        {
            get { return m_knownOnly; }
            set
            {
                m_knownOnly = value;
                UpdateResults();
                NotifyPropertyChanged();
            }
        }

        public SaintSearchViewModel()
        {
            Result = DEFAULT_TEXT;
            KnownOnly = true;

            var helpBuilder = new StringBuilder();
            helpBuilder.AppendLine("Enter one of the following search terms:");

            foreach (var type in SaintBuffManager.SaintBuffTypes)
            {
                helpBuilder.AppendLine(type.SearchWords.Last() + " (" + type.Name + ")" );
            }

            m_helpText = helpBuilder.ToString();
        }

        private void UpdateResults()
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(Filter))
            {
                sb.AppendLine(DEFAULT_TEXT);
            }
            else if (Filter.ToLower().StartsWith("help"))
            {
                sb.AppendLine(m_helpText);
            }
            else
            {
                var buffFilter = SaintBuffManager.FindFilter(Filter);
                if (buffFilter == null)
                {
                    return;
                }

                var party = LiveDataService.ReadParty();

                IEnumerable<int> knownSaintIds = null;
                if (KnownOnly)
                {
                    knownSaintIds = (from c in party
                                     select c.SaintBitmask.SaintIds).SelectMany(s => s).Distinct();
                }

                var matchingSaints = StaticDataService.FilterSaints(buffFilter, knownSaintIds);
                if (matchingSaints.Any())
                {
                    sb.AppendLine("Known saints with bonus to '" + buffFilter.Name + "': ");
                    sb.AppendLine();

                    foreach (var saint in matchingSaints)
                    {
                        sb.AppendFormat("{0}: {1}", saint.ShortName, saint.GetBuff(buffFilter.Name));

                        var knownBy = from c in party
                                      where c.SaintBitmask.HasSaint(saint.Id)
                                      select c.ShortName;
                        if (knownBy.Any())
                        {
                            sb.AppendFormat(" ({0})", string.Join(", ", knownBy));
                        }

                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.AppendLine("No one in the party knows saints with buff to '" + buffFilter.Name + "'.");
                }
            }

            if (sb.Length > 0)
            {
                Result = sb.ToString();
            }
        }
    }
}
