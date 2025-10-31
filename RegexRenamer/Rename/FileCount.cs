using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Rename
{
    public class FileCount
    {
        public int Total { get; private set; }     // num files in active path
        public int Shown { get; private set; }     // num files in filelist
        public int Filtered { get; private set; }  // num files filtered out
        public int Hidden { get; private set; }    // num files hidden (may or may not be shown)

        public FileCount()
        {
            Total = 0;
            Shown = 0;
            Filtered = 0;
            Hidden = 0;
        }

        public void SetShown(int shown) { Shown = shown; }
        public void IncrementTotal() { Total++; }
        public void IncrementFiltered() { Filtered++; }
        public void IncrementHidden() { Hidden++; }
    }
}
