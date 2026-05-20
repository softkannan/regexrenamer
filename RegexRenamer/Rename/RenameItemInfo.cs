using Kavita;
using Kavita.ParserImpl;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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

    public class DynamicRenameItemInfo
    {
        public string Preview;    // [subdir\]newfilename[.txt]
        public bool Matched;      // true if matches current regex
        public bool Skip;         // true if marked to skip (not renamed)
        public bool Selected;     // true if row is currently selected

        public ComicInfo ComicInfo;
        public ParserInfo ParseInfo;
    }

    public class RenameItemInfo
    {
        private const int MaxStackAllocSize = 256; // Max filename length we expect to handle without heap allocation

        public string Filename { get; init; }   // [subdir\]filename.txt
        public string Basename { get; init; }   // [subdir\]filename
        public string Foldername { get; init; } // c:\..\[subdir\]
        public string Extension { get; init; }  // .txt
        public string Fullpath { get; init; }   // c:\..\[subdir\]filename.txt
        public bool IsFolder { get; init; }
        public bool Hidden { get; init; }       // true if hidden file
        public bool PreserveExt { get; init; }  // true if 'Preserve file extension' checked

        public FileModificationInfo FileModified { get; init; }
        public FileSizeInfo Size { get; init; }

        // either filename or basename, depending on preserveext
        public string Name { get; init; }
        // if preserveext, append extension
        public string PreviewExt { get; init; }
        // if preserveext, append extension
        public string PreviewFullPath { get; init; }

        public DynamicRenameItemInfo Context { get; init; }

        public RenameItemInfo(FileInfo fi, bool hidden, bool preserveext)
        {
            Filename = fi.Name;
            Basename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
            Extension = fi.Extension;
            Fullpath = fi.FullName;
            Foldername = Path.GetDirectoryName(fi.FullName) + Path.DirectorySeparatorChar;
            Hidden = hidden;
            PreserveExt = preserveext;
            IsFolder = false;
            FileModified = new FileModificationInfo(fi);
            Size = new FileSizeInfo(fi);

            Context = new DynamicRenameItemInfo();
            Context.Preview = fi.Name;
            Context.Matched = false;
            Context.Selected = false;

            if (PreserveExt)
            {
                Name = Basename;
                PreviewExt = Context.Preview + Extension;
                PreviewFullPath = Foldername + Context.Preview + Extension;
            }
            else
            {
                Name = Filename;
                PreviewExt = Context.Preview;
                PreviewFullPath = Foldername + Context.Preview;
            }
        }

        public RenameItemInfo(DirectoryInfo di, bool hidden, bool preserveext)
        {
            Filename = di.Name;
            Basename = di.Name;
            Extension = "";
            Fullpath = di.FullName;
            Foldername = "";
            Hidden = hidden;
            PreserveExt = preserveext;
            IsFolder = true;
            FileModified = new FileModificationInfo(di);
            Size = new FileSizeInfo(0);

            Context = new DynamicRenameItemInfo();
            Context.Preview = di.Name;
            Context.Matched = false;
            Context.Selected = false;

            if (PreserveExt)
            {
                PreviewExt = Context.Preview + Extension;
                Name = Basename;
                PreviewFullPath = Foldername + Context.Preview + Extension;
            }
            else
            {
                Name = Filename;
                PreviewExt = Context.Preview;
                PreviewFullPath = Foldername + Context.Preview;
            }
        }

        public RenameItemInfo(ref FileSystemEntry entry, bool preserveext)
        {
            ReadOnlySpan<char> nameSpan = entry.FileName;
            int lastDot = nameSpan.LastIndexOf('.');

            string cleanName = string.Empty;
            string extension = string.Empty;

            // Check if an extension actually exists (guarding against '.gitignore' or no extension)
            if (lastDot > 0)
            {
                // Slice out both parts using zero-allocation spans
                ReadOnlySpan<char> namePart = nameSpan.Slice(0, lastDot);
                ReadOnlySpan<char> extPart = nameSpan.Slice(lastDot); // Includes the dot (e.g., ".txt")

                Span<char> cleanNameSpan = namePart.Length <= MaxStackAllocSize
                    ? stackalloc char[namePart.Length]
                    : new char[namePart.Length]; // Fallback to heap if filename is abnormally huge
                Span<char> extensionSpan = extPart.Length <= MaxStackAllocSize
                    ? stackalloc char[extPart.Length]
                    : new char[extPart.Length]; // Fallback to heap if extension is abnormally huge

                // Materialize and normalize strings
                if (namePart.TryNormalize(cleanNameSpan, out int charsWritten, NormalizationForm.FormC))
                {
                    cleanName = cleanNameSpan.ToString();
                }
                else
                {
                    cleanName = namePart.ToString().Normalize(NormalizationForm.FormC);
                }
                if (extPart.TryNormalize(extensionSpan, out int extCharsWritten, NormalizationForm.FormC))
                {
                    extension = extensionSpan.ToString();
                }
                else
                {
                    extension = extPart.ToString().Normalize(NormalizationForm.FormC);
                }
            }
            else
            {
                Span<char> cleanNameSpan = nameSpan.Length <= MaxStackAllocSize
                    ? stackalloc char[nameSpan.Length]
                    : new char[nameSpan.Length]; // Fallback to heap if filename is abnormally huge

                if (nameSpan.TryNormalize(cleanNameSpan, out int charsWritten, NormalizationForm.FormC))
                {
                    cleanName = cleanNameSpan.ToString();
                }
                else
                {
                    cleanName = nameSpan.ToString().Normalize(NormalizationForm.FormC);
                }
                extension = string.Empty;
            }

            Filename = cleanName + extension;
            Basename = cleanName;
            Extension = extension;
            Fullpath = entry.ToSpecifiedFullPath().ToString();
            Foldername = Path.GetDirectoryName(Fullpath) + Path.DirectorySeparatorChar;
            Hidden = entry.IsHidden;
            PreserveExt = preserveext;
            IsFolder = entry.IsDirectory;
            FileModified = new FileModificationInfo(new FileInfo(Fullpath));
            Size = new FileSizeInfo(entry.Length);

            Context = new DynamicRenameItemInfo();
            Context.Preview = Filename;
            Context.Matched = false;
            Context.Selected = false;

            if (PreserveExt)
            {
                Name = Basename;
                PreviewExt = Context.Preview + Extension;
                PreviewFullPath = Foldername + Context.Preview + Extension;
            }
            else
            {
                Name = Filename;
                PreviewExt = Context.Preview;
                PreviewFullPath = Foldername + Context.Preview;
            }
        }
    }
}
