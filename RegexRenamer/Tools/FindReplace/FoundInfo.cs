using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.FindReplace
{
    public class FoundInfo
    {
        public SearchInfo Pattern { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public string MatchValue { get; set; }


        //public string GetReplacementPreview(string sourceText)
        //{
        //    string retVal = string.Empty;
        //    if (this.Type == FindReplaceType.Regex)
        //    {
        //        var tempVal = this.Regex.Replace(sourceText, this.Replace);
        //        retVal = tempVal.Replace("\r\n", "[CR][LF]");
        //    }
        //    else
        //    {
        //        var comparison = this.IsCaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
        //        var tempVal = sourceText.Replace(this.Pattern, this.Replace, comparison);
        //        retVal = tempVal.Replace("\r\n", "[CR][LF]");
        //    }
        //    return retVal;
        //}

        public string GetReplacement()
        {
            string retVal = MatchValue;
            switch(Pattern.Type)
            {
                case FindReplaceType.Regex:
                    retVal = Pattern.Regex.Replace(MatchValue, Pattern.Replace);
                    break;
                case FindReplaceType.Text:
                    var comparison = Pattern.IsCaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
                    retVal = MatchValue.Replace(Pattern.Pattern, Pattern.Replace, comparison);
                    break;
            }
            return retVal;
        }

        public string GetReplacementPreview()
        {
            string retVal = string.Empty;
            switch(Pattern.Type)
            {
                case FindReplaceType.Regex:
                    var tempVal = Pattern.Regex.Replace(MatchValue, Pattern.Replace);
                    retVal = tempVal.Replace("\r\n", "[CR][LF]");
                    break;
                case FindReplaceType.Text:
                    var comparison = Pattern.IsCaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
                    var tempVal2 = MatchValue.Replace(Pattern.Pattern, Pattern.Replace, comparison);
                    retVal = tempVal2.Replace("\r\n", "[CR][LF]");
                    break;
            }
            return retVal;
        }
    }
}
