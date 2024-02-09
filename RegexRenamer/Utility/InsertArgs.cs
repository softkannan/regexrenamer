using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Utility
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
            this.InsertBefore = ib;
            this.InsertAfter = "";
            this.SelectionStartOffset = 0;
            this.SelectionLength = -1;
            this.GroupSelection = false;
            this.WrapIfSelection = false;
        }
        public InsertArgs(string ib, string ia)
        {
            this.InsertBefore = ib;
            this.InsertAfter = ia;
            this.SelectionStartOffset = 0;
            this.SelectionLength = -1;
            this.GroupSelection = false;
            this.WrapIfSelection = false;
        }
        public InsertArgs(string ib, string ia, string flags)
        {
            this.InsertBefore = ib;
            this.InsertAfter = ia;
            this.SelectionStartOffset = 0;
            this.SelectionLength = -1;
            this.GroupSelection = flags.Contains("group");
            this.WrapIfSelection = flags.Contains("wrap");
        }
        public InsertArgs(string ib, string ia, int sso, int sl)
        {
            this.InsertBefore = ib;
            this.InsertAfter = ia;
            this.SelectionStartOffset = sso;
            this.SelectionLength = sl;
            this.GroupSelection = false;
            this.WrapIfSelection = false;
        }
        public InsertArgs(string ib, string ia, int sso, int sl, string flags)
        {
            this.InsertBefore = ib;
            this.InsertAfter = ia;
            this.SelectionStartOffset = sso;
            this.SelectionLength = sl;
            this.GroupSelection = flags.Contains("group");
            this.WrapIfSelection = flags.Contains("wrap");
        }

    }
}
