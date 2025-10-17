using RegexRenamer.Rename;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Utility
{
    public static class DataGridExtensions
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }


        public static List<RenameItemInfo> GetSelectedFileItems(this DataGridView pThis, IReadOnlyList<RenameItemInfo> activeFiles)
        {
            List<RenameItemInfo> selectedFiles = new List<RenameItemInfo>();
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
        public static string GetHumanReadableBytes(this RenameItemInfo pThis)
        {
            if (pThis == null) return string.Empty;
            var fileInfo = new FileInfo(pThis.Fullpath);
            if ((fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory) return string.Empty;
            return fileInfo.GetHumanReadableBytes();
        }

        

        public static List<FileInfo> GetSelectedFileInfo(this DataGridView pThis, IReadOnlyList<RenameItemInfo> activeFiles)
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

       

        public static List<FileInfo> GetAllFileInfo(this DataGridView pThis, IReadOnlyList<RenameItemInfo> activeFiles)
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
