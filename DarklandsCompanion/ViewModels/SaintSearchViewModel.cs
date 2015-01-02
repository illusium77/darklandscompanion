using DarklandsServices.Services;
using DarklandsBusinessObjects.Objects;
using DarklandsUiCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DarklandsCompanion.ViewModels
{
    public class SaintSearchViewModel : ModelBase
    {
        private string DEFAULT_TEXT = "Enter the search term to the field above. Type 'help' to see known search terms.";

        private static IReadOnlyDictionary<string, string[]> s_searchTerms = new Dictionary<string, string[]>
        {
        { "End", new string[] {"end"}},
        { "Str", new string[] {"str"}},
        { "Agl", new string [] {"agi"}},
        { "Per", new string [] {"per"}},
        { "Int", new string [] {"int"}},
        { "Chr", new string [] {"cha"}},
		{ "wEdg", new string [] {"edg"}},
        { "wImp", new string [] {"imp"}},
        { "wFll", new string [] {"fla"}},
        { "wPol", new string [] {"pol"}},
        { "wThr", new string [] {"thr"}},
        { "wBow", new string [] {"bow"}},
        { "wMsD", new string [] {"mis"}},
        { "Alch", new string [] {"alc"}},
        { "Relg", new string [] {"rel"}},
        { "Virt", new string [] {"vir"}},
        { "SpkC", new string [] {"speak c", "com"}},
        { "SpkL", new string [] {"speak l", "lat"}},
        { "R&W", new string [] {"rea"}},
        { "Heal", new string [] {"hea"}},
        { "Artf", new string [] {"art"}},
        { "Stlh", new string [] {"ste"}},
        { "StrW", new string [] {"stre"}},
        { "Ride", new string [] {"rid"}},
        { "WdWs", new string [] {"woo"}}
        };

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

        public SaintSearchViewModel()
        {
            Result = DEFAULT_TEXT;
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
                sb.AppendLine("Enter one of the following search terms:");
                sb.AppendLine(string.Join(", ", s_searchTerms.Keys));
            }
            else
            {
                var filter = Filter.ToLower();

                // maybe the user used correct word directly
                var searchWord = s_searchTerms.Keys.FirstOrDefault(k => k.ToLower() == filter);
                if (searchWord == null)
                {
                    searchWord = (from term in s_searchTerms
                                 where term.Value.Any(word => filter.StartsWith(word))
                                 select term.Key).FirstOrDefault();
                }
                if (searchWord != null)
                {
                    var party = LiveDataService.ReadParty();

                    sb.AppendLine("Known saints with bonus to '" + searchWord + "': ");
                    sb.AppendLine();

                    var groupName = "range";
                    var statRegex = new Regex(@"((" + searchWord + @")\s*\+(?<" + groupName + @">\(\d{1,2}-\d{1,2}\)))",
                        RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

                    foreach (var character in party)
                    {
                        var saints = StaticDataService.FindSaints(character.SaintBitmask.SaintIds);

                        var regexMatches = from s in saints
                                           let matches = statRegex.Matches(s.Clue)
                                           where matches.Count > 0
                                           select new
                                           {
                                               Name = s.ShortName,
                                               Ranges = string.Join(", ", from match in matches.Cast<Match>()
                                                                          select match.Groups[groupName])
                                           };

                        if (regexMatches.Any())
                        {

                            sb.AppendLine(character.ShortName + ": " + string.Join(", ", regexMatches.Select(s => s.Name + ": " + s.Ranges)));
                            sb.AppendLine();
                        }
                    }
                }
            }

            Result = sb.ToString();
        }
    }
}
