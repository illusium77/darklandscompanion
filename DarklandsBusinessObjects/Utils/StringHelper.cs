using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Utils
{
    public static class StringHelper
    {
        public static string ConvertToString(IEnumerable<byte> bytes)
        {
            var trimmed = bytes.TakeWhile(b => b != '\0').ToArray();

            for (int i = 0; i < trimmed.Length; i++)
            {
                if (trimmed[i] == 0x7b)
                {
                    trimmed[i] = 0xf6; // ö
                }
                else if (trimmed[i] == 0x7c)
                {
                    trimmed[i] = 0xfc; // ü
                }
                else if (trimmed[i] == 0x1f)
                {
                    trimmed[i] = 0xe4; // ä
                }
            }

            return Encoding.UTF7.GetString(trimmed).Trim();
        }

        public static string[] GetNullDelimitedStrings(IEnumerable<byte> bytes, int numberOfStrings, ref int startIndex)
        {
            var strings = new List<string>(numberOfStrings);

            for (int i = 0; i < numberOfStrings; i++)
            {
                var str = ConvertToString(bytes.Skip(startIndex));
                strings.Add(str);

                startIndex += str.Length + 1;
            }

            return strings.ToArray();
        }
    }
}
