using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Rename
{
    public static class StringExtensions
    {
        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public static Tuple<string,Encoding> GetEncoding(this string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.ReadExactly(bom, 0, 4);
            }

            // Analyze the BOM
#pragma warning disable SYSLIB0001 // Type or member is obsolete
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return new Tuple<string, Encoding>("utf-7",Encoding.UTF7);
#pragma warning restore SYSLIB0001 // Type or member is obsolete
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return new Tuple<string, Encoding>("utf-8",Encoding.UTF8);
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return new Tuple<string, Encoding>("utf-16",Encoding.UTF32); //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return new Tuple<string, Encoding>("utf-16", Encoding.Unicode); //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return new Tuple<string, Encoding>("utf-16be", Encoding.BigEndianUnicode); //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new Tuple<string, Encoding>("utf-16be", new UTF32Encoding(true, true));  //UTF-32BE

            // We actually have no idea what the encoding is if we reach this point, so
            // you may wish to return null instead of defaulting to ASCII
            return new Tuple<string, Encoding>("ascii", Encoding.ASCII);
        }

        public static string FormatInvariant(this string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        public static string FormatCurrentCulture(this string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
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
