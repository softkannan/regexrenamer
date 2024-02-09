using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Utility
{
    public class FileCount
    {
        public int total;     // num files in active path
        public int shown;     // num files in filelist
        public int filtered;  // num files filtered out
        public int hidden;    // num files hidden (may or may not be shown)

        public void Reset()
        {
            this.total = 0;
            this.shown = 0;
            this.filtered = 0;
            this.hidden = 0;
        }
    }
}
