using Kavita;
using Kavita.ParserImpl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Rename
{
    public class FileSizeInfo : IComparable<FileSizeInfo>
    {
        public long Size;
        public string SizeStr;
        public FileSizeInfo(FileInfo fi)
        {
            Size = fi.Length;
            SizeStr = fi.GetHumanReadableBytes();
        }
        public FileSizeInfo(long length)
        {
            Size = length;
            SizeStr = Size.GetHumanReadableBytes();
        }

        public int CompareTo(FileSizeInfo other)
        {
            if (other == null) return 1;
            return Size.CompareTo(other.Size);
        }

        override public string ToString()
        {
            return SizeStr;
        }
    }

    public class FileModificationInfo : IComparable<FileModificationInfo>
    {
        public DateTime FileModified;
        public string FileModifiedStr;

        public FileModificationInfo(FileInfo fi)
        {
            FileModified = fi.LastWriteTime;
            FileModifiedStr = fi.LastWriteTime.ToString("MM/dd/yyyy hh:mm:ss tt");
        }

        public FileModificationInfo(DirectoryInfo di)
        {
            FileModified = di.LastWriteTime;
            FileModifiedStr = di.LastWriteTime.ToString("MM/dd/yyyy hh:mm:ss tt");
        }

        public int CompareTo(FileModificationInfo other)
        {
            if (other == null) return 1;
            return FileModified.CompareTo(other.FileModified);
        }

        override public string ToString()
        {
            return FileModifiedStr;
        }
    }

    public class RenameItemInfo
    {
        public readonly string Filename;   // [subdir\]filename.txt
        public readonly string Basename;   // [subdir\]filename
        public readonly string Foldername; // c:\..\[subdir\]
        public readonly string Extension;  // .txt
        public readonly string Fullpath;   // c:\..\[subdir\]filename.txt
        public readonly bool IsFolder;
        public readonly bool Hidden;       // true if hidden file

        public string Preview;    // [subdir\]newfilename[.txt]
        public bool Matched;      // true if matches current regex
        public bool Skip;         // true if marked to skip (not renamed)
        public bool PreserveExt;  // true if 'Preserve file extension' checked
        public bool Selected;     // true if row is currently selected

        public readonly FileModificationInfo FileModified;
        public readonly FileSizeInfo Size;

        public ComicInfo ComicInfo;
        public ParserInfo ParseInfo;

        public RenameItemInfo(FileInfo fi, bool hidden, bool preserveext)
        {
            Filename = fi.Name;
            Basename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
            Extension = fi.Extension;
            Fullpath = fi.FullName;
            Foldername = Path.GetDirectoryName(fi.FullName) + Path.DirectorySeparatorChar;
            Preview = fi.Name;
            Hidden = hidden;
            Matched = false;
            PreserveExt = preserveext;
            Selected = false;
            IsFolder = false;
            var humanVal = fi.GetHumanReadableBytes();
            FileModified = new FileModificationInfo(fi);
            Size = new FileSizeInfo(fi);
        }
        public RenameItemInfo(DirectoryInfo di, bool hidden, bool preserveext)
        {
            Filename = di.Name;
            Basename = di.Name;
            Extension = "";
            Fullpath = di.FullName;
            Foldername = "";
            Preview = di.Name;
            Hidden = hidden;
            Matched = false;
            PreserveExt = preserveext;
            Selected = false;
            IsFolder = true;
            FileModified = new FileModificationInfo(di);
            Size = new FileSizeInfo(0);
        }

       

        public string Name  // either filename or basename, depending on preserveext
        {
            get
            {
                if (PreserveExt)
                    return Basename;
                else
                    return Filename;
            }
        }
        public string PreviewExt  // if preserveext, append extension
        {
            get
            {
                if (PreserveExt)
                    return Preview + Extension;
                else
                    return Preview;
            }
        }

        public string PreviewFullPath  // if preserveext, append extension
        {
            get
            {
                if (PreserveExt)
                    return Foldername + Preview + Extension;
                else
                    return Foldername + Preview;
            }
        }
    }
}
