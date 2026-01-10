using PinHoard.model.pins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PinHoard.util
{
    public static class PinHoardHelpers
    {
        /// <summary>
        /// Removes the extension from a filename
        /// </summary>
        /// <param name="original">The filename string that is being processed</param>
        /// <param name="extensionLength">The length of the extension portion of the string exclusive of the ellipse</param>
        /// <returns>String containing only the name portion of a filename</returns>
        public static string CutExtension(string original, int extensionLength = 4)
        {
            return original.Substring(0, original.Length - (extensionLength + 1));
        }

        /// <summary>
        /// Removes the extension from a filename
        /// </summary>
        /// <param name="original">The filename string that is being processed</param>
        /// <param name="extensionLiteral">The literal extension string i.e. ".json"</param>
        /// <returns>String containing only the name portion of a filename</returns>
        public static string CutExtension(string original, string extensionLiteral)
        {
            return original.Substring(0, original.Length - extensionLiteral.Length);
        }

        /// <summary>
        /// Returns true if the given filename is valid
        /// </summary>
        /// <param name="filename">Filename input by the user</param>
        /// <returns></returns>
        public static bool IsValidFilename(string filename)
        {
            //Regex rg = new Regex(@"^[a-zA-Z0-9 . _ -]*$");
            Regex rg = new Regex(@"^[\w\-. ]*$");
            return (!string.IsNullOrEmpty(filename)
                && !string.IsNullOrWhiteSpace(filename)
                && rg.IsMatch(filename));
        }

        /// <summary>
        /// Returns true if the given hexcode is valid
        /// </summary>
        /// <param name="hexcode">Hexcode input by the user</param>
        /// <returns></returns>
        public static bool IsValidHexcode(string hexcode)
        {
            List<char> hexletters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F' };

            //must start with #
            if (hexcode[0] != '#') return false;

            foreach (char c in hexcode[1..])
            {
                // character must be either a digit or A-F
                if (!System.Char.IsDigit(c) && !hexletters.Contains(c)) return false;
            }
            if (hexcode.Length > 7 || hexcode.Length < 7) return false;

            // all checks passed
            return true;
        }

        /// <summary>
        /// Unused method
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static (string formattedString, int newLineCount) FitText(string original)
        {
            StringBuilder newText = new StringBuilder();
            int charCount = 0;
            int lastWordIndex = 0;
            int newLineCount = 1;

            foreach (char c in original)
            {
                newText.Append(c);
                if (c != '.' || c != ',') charCount++;
                if (c == ' ') lastWordIndex = newText.Length;

                if (charCount % 16 == 0)
                {
                    newText.Insert(lastWordIndex, "\n");
                    newLineCount++;
                }
            }

            return (newText.ToString(), newLineCount);
        }

        /// <summary>
        /// Returns a list of ListPoint components for each input string
        /// </summary>
        /// <param name="strings">A list of raw components as strings</param>
        /// <returns></returns>
        public static List<ListPoint> ConvertStringListToComponents(List<string> strings)
        {
            List<ListPoint> result = new List<ListPoint>();
            int i = 0;
            foreach (string s in strings)
            {
                result.Add(new ListPoint(i, 120, s));
                i += 1;
            }
            return result;
        }

        /// <summary>
        /// Determines whether the provided quiz response matches the correct answer, ignoring leading and trailing
        /// whitespace and case differences.
        /// </summary>
        /// <param name="correct">The correct answer to compare against. Cannot be <see langword="null"/>.</param>
        /// <param name="given">The response provided by the user. Cannot be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the trimmed, case-insensitive values of <paramref name="correct"/> and <paramref
        /// name="given"/> are equal; otherwise, <see langword="false"/>.</returns>
        public static bool ValidateQuizResponse(string correct, string given)
        {
            return string.Equals(correct.Trim(), given.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
