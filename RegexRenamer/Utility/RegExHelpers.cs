using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexRenamer.Utility;

public static class RegExHelpers
{
    public static Regex CreateGlobFilter(this string filterText, bool globFilter)
    {
        Regex filter = null;
        if (!string.IsNullOrWhiteSpace(filterText))
        {
            if (globFilter && filterText == "*.*")  // convert to "*" (include files with no extension)
                filterText = "*";

            if (globFilter)  // convert glob to regex
                filterText = "^" + Regex.Escape(filterText).Replace("\\*", ".*").Replace("\\?", ".") + "$";

            filter = new Regex(filterText, RegexOptions.IgnoreCase);
        }
        return filter;
    }
}
