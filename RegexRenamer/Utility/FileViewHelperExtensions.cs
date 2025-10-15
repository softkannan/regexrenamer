using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Utility
{
    public static class FileViewHelperExtensions
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }


        public static List<RRItem> GetSelectedFileItems(this DataGridView pThis, List<RRItem> activeFiles)
        {
            List<RRItem> selectedFiles = new List<RRItem>();
            foreach (DataGridViewRow row in pThis.Rows)
            {
                if (row.Selected)
                {
                    var afi = (int)row.Tag;
                    selectedFiles.Add(activeFiles[afi]);
                }
            }

            return selectedFiles;
        }
        public static string GetHumanReadableBytes(this RRItem pThis)
        {
            if(pThis == null) return string.Empty;
            var fileInfo = new FileInfo(pThis.Fullpath);
            if ((fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory) return string.Empty;
            long bytes = fileInfo.Length;
            // Get absolute value
            var absoluteBytes = (bytes < 0 ? -bytes : bytes);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            switch (absoluteBytes)
            {
                // Exabyte
                case >= 0x1000000000000000:
                    suffix = "EB";
                    readable = (bytes >> 50);
                    break;
                // Petabyte
                case >= 0x4000000000000:
                    suffix = "PB";
                    readable = (bytes >> 40);
                    break;
                // Terabyte
                case >= 0x10000000000:
                    suffix = "TB";
                    readable = (bytes >> 30);
                    break;
                // Gigabyte
                case >= 0x40000000:
                    suffix = "GB";
                    readable = (bytes >> 20);
                    break;
                // Megabyte
                case >= 0x100000:
                    suffix = "MB";
                    readable = (bytes >> 10);
                    break;
                // Kilobyte
                case >= 0x400:
                    suffix = "KB";
                    readable = bytes;
                    break;
                default:
                    return bytes.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.## ") + suffix;
        }
        public static List<FileInfo> GetSelectedFileInfo(this DataGridView pThis, List<RRItem> activeFiles)
        {
            List<FileInfo> selectedFiles = new List<FileInfo>();
            foreach (DataGridViewRow row in pThis.Rows)
            {
                if (row.Selected)
                {
                    var afi = (int)row.Tag;
                    selectedFiles.Add(new FileInfo(activeFiles[afi].Fullpath));
                }
            }

            return selectedFiles;
        }

       

        public static List<FileInfo> GetAllFileInfo(this DataGridView pThis, List<RRItem> activeFiles)
        {
            List<FileInfo> selectedFiles = new List<FileInfo>();
            foreach (DataGridViewRow row in pThis.Rows)
            {
                var afi = (int)row.Tag;
                selectedFiles.Add(new FileInfo(activeFiles[afi].Fullpath));
            }

            return selectedFiles;
        }

    }
}
