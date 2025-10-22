using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using RegexRenamer.Rename;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace RegexRenamer.Utility
{
    public class DataGridIconCache
    {
        private Dictionary<string, Icon> _extIcons { get; set; } = new Dictionary<string, Icon>(StringComparer.InvariantCultureIgnoreCase);
        private List<Icon> _dynamicIcons = new List<Icon>();

        public Icon GetIcon(FileInfo fileInfo)
        {
            return GetIcon(fileInfo.FullName, fileInfo.Extension, (fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory);
        }

        private Icon GetIcon(string fullpath, string fileExt, bool isFolder)
        {
            if (isFolder)
            {
                return FileIconAPI.GetDefaultFolderIcon(false);
            }

            // shortcut, don't key by extension as each may have different icon
            if (string.Compare(fileExt, ".lnk", StringComparison.InvariantCultureIgnoreCase) == 0) 
            {
                var lnkIcon = FileIconAPI.GetIcon(fullpath, false);
                _dynamicIcons.Add(lnkIcon);
                return lnkIcon;
            }

            if (_extIcons.TryGetValue(fileExt, out var icon))
            {
                return icon;
            }
            else
            {
                icon = FileIconAPI.GetIcon(fullpath, false);
                _extIcons[fileExt] = icon;
                return icon;
            }
        }

        public Icon GetIcon(RenameItemInfo fileInfo)
        {
            return GetIcon(fileInfo.Fullpath, fileInfo.Extension, fileInfo.IsFolder);
        }

        public void ClearDynamicIcons()
        {
            for(int idx = 0; idx < _dynamicIcons.Count; idx++)
            {
                FileIconAPI.DestroyIcon(_dynamicIcons[idx].Handle);
                _dynamicIcons[idx].Dispose();
            }

            _dynamicIcons.Clear();
        }

        public void ClearAll()
        {
            foreach (var icon in _extIcons.Values)
            {
                icon.Dispose();
            }
            _extIcons.Clear();
            foreach (var icon in _dynamicIcons)
            {
                icon.Dispose();
            }
            _dynamicIcons.Clear();
        }

    }

    public static class DataGridExtensions
    {
        public static void DoubleBuffered(this DataGridView pThis, bool setting)
        {
            Type dgvType = pThis.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(pThis, setting, null);
        }

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
