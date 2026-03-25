using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexRenamer.Utility
{
    public enum SortType
    {
        None,
        Alphabetical,
        ReverseAlphabetical,
        ByRegex
    }

    public class SortItem
    {
        public string Name { get; set; }
        public string Pattern { get; set; }
        public string ReplacePattern { get; set; }
    }

    public class SortMatchItem
    {
        public string MatchText { get; private set; }
        public string ReplacePattern { get; private set; }

        public SortType Type { get; private set; } = SortType.None;

        private Regex _match = null;
        public Regex Match
        {
            get
            {
                if (_match == null)
                {
                    try
                    {
                        _match = new Regex(MatchText, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
                    }
                    catch (ArgumentException)
                    {
                        _match = null;
                    }
                }
                return _match;

            }
        }

        public SortMatchItem(SortItem config)
        {
            MatchText = config.Pattern.Trim();
            ReplacePattern = config.ReplacePattern?.Trim();
            UpdateType(config.Name);
        }

        private void UpdateType(string name)
        {
            switch (name)
            {
                case "Default":
                    Type = SortType.Alphabetical;
                    break;
                case "Reverse":
                    Type = SortType.ReverseAlphabetical;
                    break;
                default:
                    Type = SortType.ByRegex;
                    break;
            }
        }

        public SortMatchItem(string matchText)
        {
            MatchText = matchText.Trim();
            ReplacePattern = "";
            UpdateType("");
        }
    }
}
