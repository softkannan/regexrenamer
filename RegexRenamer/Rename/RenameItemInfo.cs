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

        public FileModificationInfo(DateTimeOffset lastWriteTimeUtc)
        {
            // Normalize to local time for display purposes 
            FileModified = lastWriteTimeUtc.ToLocalTime().DateTime;
            FileModifiedStr = FileModified.ToString("MM/dd/yyyy hh:mm:ss tt");
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

        public readonly string Filename;   // [subdir\]filename.txt
        public readonly string Basename;   // [subdir\]filename
        public readonly string Foldername; // c:\..\[subdir\]
        public readonly string Extension;  // .txt
        public readonly bool IsFolder;     // true if folder, false if file
        public readonly bool Hidden;       // true if hidden file
        public readonly bool PreserveExt;  // true if 'Preserve file extension' checked
        public readonly string Name;       // either filename or basename, depending on preserveext

        public readonly FileModificationInfo FileModified; // last modified date/time
        public readonly FileSizeInfo Size;                 // file size (0 for folders)

        public DynamicRenameItemInfo Context { get; init; }

        // On Demand Calculated Properties

        // c:\..\[subdir\]filename.txt
        private string _fullpath = null;
        public string Fullpath { 
            get
            {
                if (_fullpath == null)
                {
                    _fullpath = string.Concat(Foldername, Path.DirectorySeparatorChar, Filename);
                }
                return _fullpath;
            }
        }

        // if preserveext, append extension
        private string _previewExt = null;
        public string PreviewExt { 
            get
            {
                if (_previewExt == null)
                {
                    _previewExt = PreserveExt ? Context.Preview + Extension : Context.Preview;
                }
                return _previewExt;
            }
        }

        

        public RenameItemInfo(DirectoryInfo di, bool hidden, bool preserveext)
        {
            Filename = di.Name;
            Basename = di.Name;
            Extension = string.Empty;
            Foldername = string.Empty;
            Hidden = hidden;
            PreserveExt = preserveext;
            IsFolder = true;
            FileModified = new FileModificationInfo(di);
            Size = new FileSizeInfo(0);

            Context = new DynamicRenameItemInfo();
            Context.Preview = di.Name;
            Context.Matched = false;
            Context.Selected = false;

            Name = PreserveExt ? Basename : Filename;
        }

        public RenameItemInfo(FileInfo fi, bool hidden, bool preserveext)
        {
            Filename = fi.Name;
            Basename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
            Extension = fi.Extension;
            Foldername = fi.Directory.ToString();
            Hidden = hidden;
            PreserveExt = preserveext;
            IsFolder = false;
            FileModified = new FileModificationInfo(fi);
            Size = new FileSizeInfo(fi);

            Context = new DynamicRenameItemInfo();
            Context.Preview = fi.Name;
            Context.Matched = false;
            Context.Selected = false;

            Name = PreserveExt ? Basename : Filename;
        }

        public RenameItemInfo(ref FileSystemEntry entry, bool preserveext)
        {
            ReadOnlySpan<char> nameSpan = entry.FileName;
            string cleanName, extension;
            if (entry.IsDirectory)
            {
                cleanName = entry.FileName.ToString();
                extension = string.Empty;
            }
            else
            {
                GetCleanNameAndExt(nameSpan, out cleanName, out extension);
            }

            // For directories, Filename is just the clean name. For files, it's clean name + extension.
            Filename = entry.IsDirectory ? cleanName : string.Concat(cleanName, extension);
            // For files, Basename is the clean name without extension. For directories, it's the same as Filename.
            Basename = cleanName;
            // folders will have empty extension
            Extension = extension;
            // For directories, Foldername is empty. For files, it's the normalized parent directory path.
            Foldername = entry.IsDirectory ? string.Empty : entry.Directory.NormalizeToC();
            Hidden = entry.IsHidden;
            PreserveExt = preserveext;
            IsFolder = entry.IsDirectory;
            FileModified = new FileModificationInfo(entry.LastWriteTimeUtc);
            Size = new FileSizeInfo(entry.Length);

            Context = new DynamicRenameItemInfo();
            Context.Preview = Filename;
            Context.Matched = false;
            Context.Selected = false;

            Name = PreserveExt ? Basename : Filename;
        }

        private static void GetCleanNameAndExt(ReadOnlySpan<char> nameSpan, out string cleanName, out string extension)
        {
            int lastDot = nameSpan.LastIndexOf('.');
            cleanName = string.Empty;
            extension = string.Empty;
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
        }
    }
}
