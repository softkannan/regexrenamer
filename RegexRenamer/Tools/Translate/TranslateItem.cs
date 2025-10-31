using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.Translate
{
    public struct TranslateItem
    {
        public string OriginalText { get; }
        public string FromLanguage { get; }
        public string ToLanguage { get; }

        // Some other user defined properties...

        public TranslateItem(string text, string fromLang, string toLang)
        {
            OriginalText = text;
            FromLanguage = fromLang;
            ToLanguage = toLang;
        }
    }
}
