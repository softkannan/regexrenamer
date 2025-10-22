using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Utility
{
    public static class StringHelperExtensions
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
    }
}
