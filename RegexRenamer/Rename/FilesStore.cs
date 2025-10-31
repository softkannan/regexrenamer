using PdfSharp.Pdf.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Rename
{
    public enum InactiveReason
    {
        Hidden,
        Filtered
    }

    public class GlobInfo
    {
        public string RootPath { get; private set; }
        public string Filter { get; private set; }
        public bool IsExclude { get; private set; }
        public bool ShowHidden { get; private set; }
        public bool PreserveExt { get; private set; }
        public bool IsGlobFilter { get; private set; }  // true if filter is glob, false if regex

        public GlobInfo(string rootPath, string filter, bool isExclude, bool showHidden, bool preserveExt, bool isGlobFilter)
        {
            RootPath = rootPath;
            Filter = filter;
            IsExclude = isExclude;
            ShowHidden = showHidden;
            PreserveExt = preserveExt;
            IsGlobFilter = isGlobFilter;
        }

        public Regex CreateGlobFilter()
        {
            string filterText = Filter;
            Regex retVal = null;
            if (!string.IsNullOrWhiteSpace(filterText))
            {
                if (IsGlobFilter && filterText == "*.*")  // convert to "*" (include files with no extension)
                    filterText = "*";

                if (IsGlobFilter)  // convert glob to regex
                    filterText = "^" + Regex.Escape(filterText).Replace("\\*", ".*").Replace("\\?", ".") + "$";

                retVal = new Regex(filterText, RegexOptions.IgnoreCase);
            }
            return retVal;
        }
    }

    public class FilesStore
    {
        public static int MAX_FILES = 10000;   // file limit for filelist (was a const)
        public IReadOnlyList<RenameItemInfo> Files => _files;

        private readonly List<RenameItemInfo> _files = new List<RenameItemInfo>();

        private readonly Dictionary<string, InactiveReason> _inActiveFiles = new Dictionary<string, InactiveReason>();
        public IReadOnlyDictionary<string, InactiveReason> InactiveFiles => _inActiveFiles;

        public FileCount Stats { get; private set; } = new FileCount();

        private readonly GlobInfo _globInfo;

        public FilesStore()
        {
            _globInfo = null;
        }

        public FilesStore(GlobInfo globInfo, bool folderStore)
        {
            _globInfo = globInfo;
            if (folderStore)
                BuildFoldersStore();
            else
                BuildFilesStore();
        }

        public void Update(int idx, RenameItemInfo item)
        {
            if (idx < 0 || idx >= _files.Count) throw new ArgumentOutOfRangeException(nameof(idx));
            _files[idx] = item;
        }

        public void TrimFiles(int idx, int count)
        {
            if (idx < 0 || idx >= _files.Count) throw new ArgumentOutOfRangeException(nameof(idx));
            if (count < 0 || idx + count > _files.Count) throw new ArgumentOutOfRangeException(nameof(count));
            _files.RemoveRange(idx, count);
        }

        private void BuildFoldersStore()
        {
            var filter = _globInfo.CreateGlobFilter();
            DirectoryInfo activeDir = new DirectoryInfo(_globInfo.RootPath);
            DirectoryInfo[] dirs = new DirectoryInfo[0];
            try
            {
                dirs = activeDir.GetDirectories();
            }
            catch (Exception ex)
            {
            }

            //TODO: fill the paging
            int pageEnd = dirs.Length;
            int pageStart = 0;
            for (int idx = pageStart; idx < pageEnd; idx++)
            {
                DirectoryInfo dir = dirs[idx] as DirectoryInfo;
                Stats.IncrementTotal();

                // ignore if filtered out
                if (filter != null && filter.IsMatch(dir.Name) == _globInfo.IsExclude)
                {
                    if (!_inActiveFiles.ContainsKey(dir.Name.ToLower()))
                        _inActiveFiles.Add(dir.Name.ToLower(), InactiveReason.Filtered);
                    Stats.IncrementFiltered();
                    continue;
                }

                // ignore if hidden and not showing hidden files
                bool hidden = false;
                try
                {
                    hidden = (dir.Attributes & FileAttributes.Hidden) != 0;
                }
                catch { }  // reported System.UnauthorizedAccessException here under some versions of Samba when item is a link to /dev/null

                if (hidden) Stats.IncrementHidden();
                if (!_globInfo.ShowHidden && hidden)
                {
                    if (!_inActiveFiles.ContainsKey(dir.Name.ToLower()))
                        _inActiveFiles.Add(dir.Name.ToLower(), InactiveReason.Hidden);
                    continue;
                }

                _files.Add(new RenameItemInfo(dir, hidden, _globInfo.PreserveExt));
            }
        }

        private void BuildFilesStore()
        {
            var filter = _globInfo.CreateGlobFilter();
            DirectoryInfo activeDir = new DirectoryInfo(_globInfo.RootPath);
            FileInfo[] files = new FileInfo[0];
            
            try
            {
                files = activeDir.GetFiles();
            }
            catch (Exception)
            {
            }

            //TODO: fill the paging
            int pageEnd = files.Length;
            int pageStart = 0;
            for (int idx = pageStart; idx < pageEnd; idx++)
            {
                FileInfo file = files[idx] as FileInfo;
                Stats.IncrementTotal();

                // ignore if filtered out
                if (filter != null && filter.IsMatch(file.Name) == _globInfo.IsExclude)
                {
                    if (!_inActiveFiles.ContainsKey(file.Name.ToLower()))
                        _inActiveFiles.Add(file.Name.ToLower(), InactiveReason.Filtered);
                    Stats.IncrementFiltered();
                    continue;
                }

                // ignore if hidden and not showing hidden files
                bool hidden = false;
                try
                {
                    hidden = (file.Attributes & FileAttributes.Hidden) != 0;
                }
                catch { }  // reported System.UnauthorizedAccessException here under some versions of Samba when item is a link to /dev/null

                if (hidden) Stats.IncrementHidden();
                if (!_globInfo.ShowHidden && hidden)
                {
                    if (!_inActiveFiles.ContainsKey(file.Name.ToLower()))
                        _inActiveFiles.Add(file.Name.ToLower(), InactiveReason.Hidden);
                    continue;
                }

                _files.Add(new RenameItemInfo(file, hidden, _globInfo.PreserveExt));
            }
        }
    }
}
