using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Utility.RegexMenu
{
    public struct InsertArgs
    {
        public string InsertBefore;       // text to insert before selection/cursor
        public string InsertAfter;        // text to insert after selection/cursor
        public int SelectionStartOffset;  // adjust cursor position (0 = no change, >0 adjust from front, <0 adjust from end)
        public int SelectionLength;       // set selection length (-1 = leave as is)
        public bool GroupSelection;       // [group] if selection, group selection first
        public bool WrapIfSelection;      // [wrap]  if selection, insert InsertBefore before AND after

        public InsertArgs(string ib)
        {
            InsertBefore = ib;
            InsertAfter = "";
            SelectionStartOffset = 0;
            SelectionLength = -1;
            GroupSelection = false;
            WrapIfSelection = false;
        }
        public InsertArgs(string ib, string ia)
        {
            InsertBefore = ib;
            InsertAfter = ia;
            SelectionStartOffset = 0;
            SelectionLength = -1;
            GroupSelection = false;
            WrapIfSelection = false;
        }
        public InsertArgs(string ib, string ia, string flags)
        {
            InsertBefore = ib;
            InsertAfter = ia;
            SelectionStartOffset = 0;
            SelectionLength = -1;
            GroupSelection = flags.Contains("group");
            WrapIfSelection = flags.Contains("wrap");
        }
        public InsertArgs(string ib, string ia, int sso, int sl)
        {
            InsertBefore = ib;
            InsertAfter = ia;
            SelectionStartOffset = sso;
            SelectionLength = sl;
            GroupSelection = false;
            WrapIfSelection = false;
        }
        public InsertArgs(string ib, string ia, int sso, int sl, string flags)
        {
            InsertBefore = ib;
            InsertAfter = ia;
            SelectionStartOffset = sso;
            SelectionLength = sl;
            GroupSelection = flags.Contains("group");
            WrapIfSelection = flags.Contains("wrap");
        }

    }
}
