using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexRenamer.Utility
{
    public static class StringSortHelperExtensions
    {
        public static string ReverseTextElements(this string input)
        {
            StringInfo stringInfo = new StringInfo(input);
            int length = stringInfo.LengthInTextElements;
            string[] textElements = new string[length];

            for (int i = 0; i < length; i++)
            {
                textElements[i] = stringInfo.SubstringByTextElements(i, 1);
            }

            Array.Reverse(textElements);
            return string.Concat(textElements);
        }

        public static string ConvertToSortText(this string input, SortMatchItem sortMatch)
        {
            SortType type = sortMatch == null ? SortType.Alphabetical : sortMatch.Type;

            switch(type)
            {
                case SortType.Alphabetical:
                    return input;
                case SortType.ReverseAlphabetical:
                    return input.ReverseTextElements();
                default:
                    {
                        var regex = sortMatch == null ? null : sortMatch.Match;
                        if (regex == null) return input;
                        var match = regex.Match(input);
                        if (match == null || !match.Success) return input;
                        if (match.Groups.Count < 2) return input;

                        if (string.IsNullOrEmpty(sortMatch.ReplacePattern))
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 1; i < match.Groups.Count; i++)
                            {
                                sb.Append(match.Groups[i].Value);
                            }
                            return sb.ToString();
                        }
                        else
                        {
                            string result = regex.Replace(input, sortMatch.ReplacePattern);
                            return result;
                        }
                    }
            }   
        }
    }
}
