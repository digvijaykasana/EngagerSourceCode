using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EngagerMark4.Common.Utility
{
    public static class TextManager
    {
        public static string ReplaceCommaWithSpace(string aStr)
        {
            return aStr.Replace(',', ' ');
        }

        public static string FilterTextandNumber(string aPara)
        {
            if (string.IsNullOrEmpty(aPara))
                return string.Empty;
            string aResult = string.Empty;
            foreach (var ch in aPara.ToCharArray())
            {
                if (char.IsLetterOrDigit(ch) || ch == '-')
                    aResult += ch;
            }
            return aResult;
        }

        public static string FilterSpeicalCharacter(string aPara)
        {
            if (String.IsNullOrEmpty(aPara))
                return string.Empty;

            string aResult = string.Empty;

            foreach (var ch in aPara.ToCharArray())
            {
                if (char.IsLetterOrDigit(ch)
                    || ch == '~'
                    || ch == '!'
                    || ch == '@'
                    || ch == '#'
                    || ch == ' '
                    || ch == '$'
                    || ch == '%'
                    || ch == '&'
                    || ch == '*'
                    || ch == '('
                    || ch == ')'
                    || ch == '_'
                    || ch == '-'
                    || ch == '+'
                    || ch == '-'
                    || ch == '='
                    || ch == '{'
                    || ch == '}'
                    || ch == '['
                    || ch == ']'
                    || ch== '|'
                    || ch == '\\'
                    || ch== ':'
                    || ch== ';'
                    || ch == '"'
                    || ch == '\''
                    || ch == '?'
                    || ch == ','
                    || ch == '.'
                    || ch == '/')
                {
                    aResult += ch;
                }
            }
            return aResult;
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
    }
}
