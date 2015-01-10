using System.Collections.Generic;

namespace DarklandsServices.Saints
{
    public class SaintBuffFilter
    {
        public SaintBuffFilter(string name, string regexKey, params string[] additionalSearchWords)
        {
            Name = name;
            Regex = regexKey;

            var words = new List<string> {regexKey.Replace(@"/", string.Empty).ToLower()};

            if (additionalSearchWords.Length > 0)
            {
                words.AddRange(additionalSearchWords);
            }
            SearchWords = words;
        }

        public string Name { get; private set; }
        public string Regex { get; private set; }
        public IEnumerable<string> SearchWords { get; private set; }
    }
}