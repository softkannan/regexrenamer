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
            int end = length - 1;
            for (int idx = 0; idx < length; idx++)
            {
                textElements[end - idx] = stringInfo.SubstringByTextElements(idx, 1);
            }

            return string.Concat(textElements);
        }
        
    }
}
