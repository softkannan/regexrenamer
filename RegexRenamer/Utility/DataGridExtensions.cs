using Interop.Shell32;
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
        private HashSet<string> _dynamicIconFileTypes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { ".exe", ".lnk", ".url", ".ico", ".cpl", ".msc" };

        public Icon GetIcon(FileInfo fileInfo)
        {
            var isFolder = (fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
            if (isFolder)
            {
                return FileIconAPI.GetDefaultFolderIcon(false);
            }
            var fileExt = fileInfo.Extension;
            // shortcut, don't key by extension as each may have different icon
            if (string.Compare(fileExt, ".lnk", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                var lnkIcon = FileIconAPI.GetIcon(fileInfo.FullName, false);
                return lnkIcon;
            }

            if (_extIcons.TryGetValue(fileExt, out var icon))
            {
                return icon;
            }
            else
            {
                icon = FileIconAPI.GetIcon(fileInfo.FullName, false);
                _extIcons[fileExt] = icon;
                return icon;
            }
        }


        public Icon GetIcon(RenameItemInfo fileInfo)
        {
            if (fileInfo.IsFolder   )
            {
                return FileIconAPI.GetDefaultFolderIcon(false);
            }

            var fileExt = fileInfo.Extension;

            // shortcut, don't key .lnk files by extension as each may have different icon
            if (_dynamicIconFileTypes.Contains(fileExt))
            {
                var lnkIcon = FileIconAPI.GetIcon(fileInfo.Fullpath, false);
                return lnkIcon;
            }

            if (_extIcons.TryGetValue(fileExt, out var icon))
            {
                return icon;
            }
            else
            {
                icon = FileIconAPI.GetIcon(fileInfo.Fullpath, false);
                _extIcons[fileExt] = icon;
                return icon;
            }
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

        
        public static string GetHumanReadableBytes(this RenameItemInfo pThis)
        {
            if (pThis == null) return string.Empty;
            var fileInfo = new FileInfo(pThis.Fullpath);
            if ((fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory) return string.Empty;
            return fileInfo.GetHumanReadableBytes();
        }

        

        public static List<FileInfo> GetSelectedFileInfo(this DataGridView pThis, IReadOnlyList<RenameItemInfo> activeFiles, Func<int, int> activeFileIndexMapper = null)
        {
            List<FileInfo> selectedFiles = new List<FileInfo>(pThis.SelectedRows.Count);
            foreach (DataGridViewRow row in pThis.SelectedRows)
            {
                int afi = activeFileIndexMapper != null ? activeFileIndexMapper(row.Index) : (int)row.Tag;
                selectedFiles.Add(new FileInfo(activeFiles[afi].Fullpath));
            }

            return selectedFiles;
        }

       

        public static List<FileInfo> GetAllFileInfo(this DataGridView pThis, IReadOnlyList<RenameItemInfo> activeFiles, Func<int, int> activeFileIndexMapper = null)
        {
            List<FileInfo> selectedFiles = new List<FileInfo>();
            foreach (DataGridViewRow row in pThis.Rows)
            {
                int afi = activeFileIndexMapper != null ? activeFileIndexMapper(row.Index) : (int)row.Tag;
                selectedFiles.Add(new FileInfo(activeFiles[afi].Fullpath));
            }

            return selectedFiles;
        }

    }
}
