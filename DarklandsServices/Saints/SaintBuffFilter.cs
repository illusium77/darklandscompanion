using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsServices.Saints
{
    public class SaintBuffFilter
    {
        public string Name { get; private set; }
        public string Regex { get; private set; }
        public IEnumerable<string> SearchWords { get; private set; }

        public SaintBuffFilter(string name, string regexKey, params string[] additionalSearchWords)
        {
            Name = name;
            Regex = regexKey;

            var words = new List<string>();
            words.Add(regexKey.Replace(@"/", string.Empty).ToLower());

            if (additionalSearchWords.Length > 0)
            {
                words.AddRange(additionalSearchWords);
            }
            SearchWords = words;
        }
    }
}
