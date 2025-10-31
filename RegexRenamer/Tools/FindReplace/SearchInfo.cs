using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.FindReplace
{
    public class SearchInfo
    {
        public Regex Regex { get; set; } = null;
        public string Pattern { get; set; } = string.Empty;
        public string Replace { get; set; } = string.Empty;
        public bool IsCaseSensitive { get; set; } = false;
        public FindReplaceType Type { get; set; } = FindReplaceType.Regex;
        public string ActionType { get; set; } = "Auto"; // or "Replace"


        private string _lastFound = string.Empty;
        private string _nextLookup = "1";
        private int _currentIndex = 0;
        private void SearchNumberInText(string sourceText)
        {
            if (string.IsNullOrEmpty(sourceText))
                return;
            int idx = sourceText.IndexOf(_nextLookup, _currentIndex);
            if (idx >= 0)
            {
                _lastFound = sourceText[idx].ToString();
            }
            else
            {
                _lastFound = string.Empty;
            }

            //bool areEqual = true;
            //TextElementEnumerator en1 = StringInfo.GetTextElementEnumerator(s1);
            //TextElementEnumerator en2 = StringInfo.GetTextElementEnumerator(s2);

            //while (en1.MoveNext() && en2.MoveNext())
            //{
            //    if (en1.GetTextElement() != en2.GetTextElement())
            //    {
            //        areEqual = false;
            //        break;
            //    }
            //}

            //if (en1.MoveNext() || en2.MoveNext()) // Check if one string is longer than the other
            //{
            //    areEqual = false;
            //}

        }

        public List<FoundInfo> FindAll(string inputText)
        {
            var retVal = new List<FoundInfo>();
            switch (Type)
            {
                case FindReplaceType.Regex:
                    {
                        if (string.IsNullOrEmpty(Pattern))
                        {
                            return retVal;
                        }
                        if (Regex == null)
                        {
                            // Precompiled regex available, use it
                            var options = IsCaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
                            Regex = new Regex(Pattern, options | RegexOptions.Compiled);
                        }
                        var matches = Regex.Matches(inputText);
                        foreach (Match match in matches)
                        {
                            retVal.Add(new FoundInfo()
                            {
                                Pattern = this,
                                StartIndex = match.Index,
                                Length = match.Length,
                                MatchValue = match.Value
                            });
                        }
                    }
                    break;
                case FindReplaceType.Text:
                    {
                        if (string.IsNullOrEmpty(Pattern))
                        {
                            return retVal;
                        }
                        var comparison = IsCaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
                        int index = 0;
                        while ((index = inputText.IndexOf(Pattern, index, comparison)) != -1)
                        {
                            retVal.Add(new FoundInfo()
                            {
                                Pattern = this,
                                StartIndex = index,
                                Length = Pattern.Length,
                                MatchValue = inputText.Substring(index, Pattern.Length)
                            });
                            index += Pattern.Length; // Move past the last found instance
                        }
                    }
                    break;
            }
            return retVal;
        }
    }
}
