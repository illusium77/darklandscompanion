using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarklandsBusinessObjects.Utils
{
    public static class StringHelper
    {
        public static string ConvertToString(byte[] bytes)
        {
            var trimmed = bytes.TakeWhile(b => b != '\0').ToArray();

            for (var i = 0; i < trimmed.Length; i++)
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

        public static string[] GetNullDelimitedStrings(byte[] bytes, int numberOfStrings, ref int startIndex)
        {
            var strings = new List<string>(numberOfStrings);

            for (var i = 0; i < numberOfStrings; i++)
            {
                var str = ConvertToString(bytes.Skip(startIndex).ToArray());
                strings.Add(str);

                startIndex += str.Length + 1;
            }

            return strings.ToArray();
        }

        /// <summary>
        ///     Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="width">
        ///     Width, in characters, to which the text
        ///     should be word wrapped
        /// </param>
        /// <returns>The modified text</returns>
        public static string WordWrap(string text, int width)
        {
            // Credit http://www.codeproject.com/Articles/51488/Implementing-Word-Wrap-in-C
            int pos, next;
            var sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                var eol = text.IndexOf(Environment.NewLine, pos, StringComparison.Ordinal);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        var len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);
                        sb.Append(text, pos, len);
                        sb.Append(Environment.NewLine);

                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                }
                else sb.Append(Environment.NewLine); // Empty line
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        ///     Locates position to break the given line so as to avoid
        ///     breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        private static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            var i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }
    }
}