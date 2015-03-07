using System.Collections.Generic;
using System.Linq;
using System.Text;
using DarklandsServices.Saints;
using DarklandsServices.Services;
using DarklandsUiCommon.Models;
using DarklandsUiCommon.ViewModels;

namespace DarklandsCompanion.ViewModels
{
    public class SaintSearchViewModel : ModelBase
    {
        private readonly string _helpText;

        private const string DefaultText = "Enter the search term to the field above. Type 'help' to see known search terms.";

        private string _filter;
        private bool _knownOnly;
        private string _result;

        public SaintSearchViewModel()
        {
            Result = DefaultText;
            KnownOnly = true;

            var helpBuilder = new StringBuilder();
            helpBuilder.AppendLine("Enter one of the following search terms:");

            foreach (var type in SaintBuffManager.SaintBuffTypes)
            {
                helpBuilder.AppendLine(type.SearchWords.Last() + " (" + type.Name + ")");
            }

            _helpText = helpBuilder.ToString();
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;

                UpdateResults();
                NotifyPropertyChanged();
            }
        }

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                NotifyPropertyChanged();
            }
        }

        public bool KnownOnly
        {
            get { return _knownOnly; }
            set
            {
                _knownOnly = value;
                UpdateResults();
                NotifyPropertyChanged();
            }
        }

        private void UpdateResults()
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(Filter))
            {
                sb.AppendLine(DefaultText);
            }
            else if (Filter.ToLower().StartsWith("help"))
            {
                sb.AppendLine(_helpText);
            }
            else
            {
                var buffFilter = SaintBuffManager.FindFilter(Filter);
                if (buffFilter == null)
                {
                    return;
                }

                var party = LiveDataService.ReadParty().ToList();

                IEnumerable<int> knownSaintIds = null;
                if (KnownOnly)
                {
                    knownSaintIds = (from c in party
                        select c.SaintBitmask.SaintIds).SelectMany(s => s).Distinct();
                }

                var matchingSaints = StaticDataService.FilterSaints(buffFilter, knownSaintIds).ToList();
                if (matchingSaints.Any())
                {
                    sb.AppendLine("Known saints with bonus to '" + buffFilter.Name + "': ");
                    sb.AppendLine();

                    foreach (var s in matchingSaints)
                    {
                        var saint = s;

                        sb.AppendFormat("{0}: {1}", saint.ShortName, saint.GetBuff(buffFilter.Name));

                        var knownBy = (from c in party
                                       where c.SaintBitmask.HasSaint(saint.Id)
                                       select c.ShortName).ToList();

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