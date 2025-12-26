using PinHoard.model.pins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PinHoard.util
{
    public static class PinHoardHelpers
    {
        public static string CutExtension(string original, int extensionLength = 4)
        {
            return original.Substring(0, original.Length - (extensionLength + 1));
        }
        public static string CutExtension(string original, string extensionLiteral)
        {
            return original.Substring(0, original.Length - extensionLiteral.Length);
        }
        public static bool IsValidFilename(string filename)
        {
            //Regex rg = new Regex(@"^[a-zA-Z0-9 . _ -]*$");
            Regex rg = new Regex(@"^[\w\-. ]*$");
            return (!string.IsNullOrEmpty(filename)
                && !string.IsNullOrWhiteSpace(filename)
                && rg.IsMatch(filename));
        }
        public static bool IsValidHexcode(string hexcode)
        {
            List<char> hexletters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F' };

            if (hexcode[0] != '#') return false;

            foreach (char c in hexcode[1..])
            {
                if (!System.Char.IsDigit(c) && !hexletters.Contains(c)) return false;
            }
            if (hexcode.Length > 7 || hexcode.Length < 7) return false;

            // all checks passed
            return true;
        }
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

        public static bool ValidateQuizResponse(string correct, string given)
        {
            return string.Equals(correct.Trim(), given.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
