using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Utility
{
    public static class StringExtensions
    {
        public static string FormatInvariant(this string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }

        public static string FormatCurrentCulture(this string format, params object[] args)
        {
            return String.Format(CultureInfo.CurrentCulture, format, args);
        }

        public static string FilenameToUri(this string str)
        {
            return Uri.EscapeDataString(str);
        }

        public static string UriToFilename(this string str)
        {
            return Uri.UnescapeDataString(str);
        }

        public static string ToCleanFileName(this string str)
        {
            str = str.Trim();
            str = Uri.UnescapeDataString(str);
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '-');
            }
            char[] repeatingChars = {'-',' ','_'};
            foreach (char c in repeatingChars)
            {
                str = str.RemoveRepeatingChars(c);
            }
            return str;
        }
        public static string RemoveRepeatingChars(this string str,char onlyChar)
        {
            var strResult = new StringBuilder();

            foreach (var element in str.ToCharArray())
            {
                if (strResult.Length == 0 || element != onlyChar || strResult[strResult.Length - 1] != element)
                    strResult.Append(element);
            }

            return strResult.ToString();
        }
        public static string RemoveRepeatingChars(this string str)
        {
            var strResult = new StringBuilder();

            foreach (var element in str.ToCharArray())
            {
                if (strResult.Length == 0 || strResult[strResult.Length - 1] != element)
                    strResult.Append(element);
            }

            return strResult.ToString();
        }

        // Returns a string you can safely use as valid Windows or Linux file name.
        // Does not check for reserved words.
        public static string ToSafeFileName(this string str)
        {
            str = str.Trim().Replace(' ', '-');
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '-');
            }
            return str;
        }
        
        public static string Truncate(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return str.Length <= maxLength ? str : str.Substring(0, maxLength);
        }
    }
}
