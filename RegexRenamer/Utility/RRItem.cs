using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Utility
{
    public class RRItem
    {
        public string Filename;   // [subdir\]filename.txt
        public string Basename;   // [subdir\]filename
        public string Foldername; // c:\..\[subdir\]
        public string Extension;  // .txt
        public string Fullpath;   // c:\..\[subdir\]filename.txt
        public string Preview;    // [subdir\]newfilename[.txt]
        public bool Hidden;       // true if hidden file
        public bool Matched;      // true if matches current regex
        public bool PreserveExt;  // true if 'Preserve file extension' checked
        public bool Selected;     // true if row is currently selected

        public Kavita.ComicInfo ComicInfo;
        public Kavita.ParserInfo ParseInfo;

        public RRItem(FileInfo fi, bool hidden, bool preserveext)
        {
            this.Filename = fi.Name;
            this.Basename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
            this.Extension = fi.Extension;
            this.Fullpath = fi.FullName;
            this.Foldername = Path.GetDirectoryName(fi.FullName) + "\\";
            this.Preview = fi.Name;
            this.Hidden = hidden;
            this.Matched = false;
            this.PreserveExt = preserveext;
            this.Selected = false;
        }
        public RRItem(DirectoryInfo di, bool hidden, bool preserveext)
        {
            this.Filename = di.Name;
            this.Basename = di.Name;
            this.Extension = "";
            this.Fullpath = di.FullName;
            this.Foldername = "";
            this.Preview = di.Name;
            this.Hidden = hidden;
            this.Matched = false;
            this.PreserveExt = preserveext;
            this.Selected = false;
        }

        public string Name  // either filename or basename, depending on preserveext
        {
            get
            {
                if (this.PreserveExt)
                    return this.Basename;
                else
                    return this.Filename;
            }
        }
        public string PreviewExt  // if preserveext, append extension
        {
            get
            {
                if (this.PreserveExt)
                    return this.Preview + this.Extension;
                else
                    return this.Preview;
            }
        }

        public string PreviewFullPath  // if preserveext, append extension
        {
            get
            {
                if (this.PreserveExt)
                    return this.Foldername + this.Preview + this.Extension;
                else
                    return this.Foldername + this.Preview;
            }
        }
    }
}
