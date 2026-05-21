using PdfSharp.Pdf.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
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
                    return null;

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

        private readonly Dictionary<string, InactiveReason> _inActiveFiles = new Dictionary<string, InactiveReason>(StringComparer.OrdinalIgnoreCase);
        public IReadOnlyDictionary<string, InactiveReason> InactiveFiles => _inActiveFiles;

        public FileCount Stats { get; private set; } = new FileCount();

        private readonly GlobInfo _globInfo;
        private readonly bool _searchInFolders;
        private readonly bool _searchForFolders;

        public FilesStore()
        {
            _globInfo = null;
        }

        public FilesStore(GlobInfo globInfo, bool searchForFolders, bool searchInFolders)
        {
            _globInfo = globInfo;
            _searchInFolders = searchInFolders;
            _searchForFolders = searchForFolders;
            if (searchForFolders) {
                //BuildFoldersStore();
                BuildFoldersStoreFast();
            }
            else {
                //BuildFilesStore();
                BuildFilesStoreFast();
            }
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

        public void RemoveAt(int idx)
        {
            if (idx < 0 || idx >= _files.Count) throw new ArgumentOutOfRangeException(nameof(idx));
            _files.RemoveAt(idx);
        }
        private void BuildFoldersStoreFast()
        {
            var filter = _globInfo.CreateGlobFilter();
            DirectoryInfo activeDir = new DirectoryInfo(_globInfo.RootPath);
            var options = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = _searchInFolders,
            };
            var enumerator = new FileSystemEnumerable<RenameItemInfo>(
                activeDir.FullName,
                (ref FileSystemEntry entry) =>
                {
                    if ((entry.Attributes & FileAttributes.Directory) == 0)
                        return null;

                    Stats.IncrementTotal();

                    // ignore if filtered out
                    if (filter != null && filter.IsMatch(entry.FileName) == _globInfo.IsExclude)
                    {
                        var fileName = entry.FileName.NormalizeToC();
                        if (!_inActiveFiles.ContainsKey(fileName))
                            _inActiveFiles.Add(fileName, InactiveReason.Filtered);
                        Stats.IncrementFiltered();
                        return null;
                    }

                    // ignore if hidden and not showing hidden files
                    bool hidden = entry.IsHidden;
                    if (hidden) Stats.IncrementHidden();
                    if (!_globInfo.ShowHidden && hidden)
                    {
                        var fileName = entry.FileName.NormalizeToC();
                        if (!_inActiveFiles.ContainsKey(fileName))
                            _inActiveFiles.Add(fileName, InactiveReason.Hidden);
                        return null;
                    }

                    return new RenameItemInfo(ref entry, _globInfo.PreserveExt);
                },
                options
            );

            // 2. Perform exactly ONE I/O pass
            foreach (var file in enumerator)
            {

                if (file == null)
                    continue;

                _files.Add(file);
            }
        }
        private void BuildFoldersStore()
        {
            var filter = _globInfo.CreateGlobFilter();
            DirectoryInfo activeDir = new DirectoryInfo(_globInfo.RootPath);
            IEnumerable<DirectoryInfo> dirs;
            try
            {
                dirs = activeDir.EnumerateDirectories();
            }
            catch
            {
                return;
            }

            foreach (DirectoryInfo dir in dirs)
            {
                Stats.IncrementTotal();

                var name = dir.Name;

                // ignore if filtered out
                if (filter != null && filter.IsMatch(name) == _globInfo.IsExclude)
                {
                    if (!_inActiveFiles.ContainsKey(name))
                        _inActiveFiles.Add(name, InactiveReason.Filtered);
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
                    if (!_inActiveFiles.ContainsKey(name))
                        _inActiveFiles.Add(name, InactiveReason.Hidden);
                    continue;
                }

                _files.Add(new RenameItemInfo(dir, hidden, _globInfo.PreserveExt));
            }
        }
        private void BuildFilesStoreFast()
        {
            var filter = _globInfo.CreateGlobFilter();
            DirectoryInfo activeDir = new DirectoryInfo(_globInfo.RootPath);
            var options = new EnumerationOptions { 
                IgnoreInaccessible = true,
                RecurseSubdirectories = _searchInFolders,
            };
            var enumerator = new FileSystemEnumerable<RenameItemInfo>(
                activeDir.FullName,
                (ref FileSystemEntry entry) =>
                    {
                        if((entry.Attributes & FileAttributes.Directory) != 0)
                            return null;

                        Stats.IncrementTotal();

                        // ignore if filtered out
                        if (filter != null && filter.IsMatch(entry.FileName) == _globInfo.IsExclude)
                        {
                            var fileName = entry.FileName.NormalizeToC();
                            if (!_inActiveFiles.ContainsKey(fileName))
                                _inActiveFiles.Add(fileName, InactiveReason.Filtered);
                            Stats.IncrementFiltered();
                            return null;
                        }

                        // ignore if hidden and not showing hidden files
                        bool hidden = entry.IsHidden;
                        if (hidden) Stats.IncrementHidden();
                        if (!_globInfo.ShowHidden && hidden)
                        {
                            var fileName = entry.FileName.NormalizeToC();
                            if (!_inActiveFiles.ContainsKey(fileName))
                                _inActiveFiles.Add(fileName, InactiveReason.Hidden);
                            return null;
                        }

                        return new RenameItemInfo(ref entry, _globInfo.PreserveExt);
                    },
                options
            );

            // 2. Perform exactly ONE I/O pass
            foreach (var file in enumerator)
            {

                if(file == null)
                    continue;

                _files.Add(file);
            }
        }
        private void BuildFilesStore()
        {
            var filter = _globInfo.CreateGlobFilter();
            DirectoryInfo activeDir = new DirectoryInfo(_globInfo.RootPath);
            IEnumerable<FileInfo> files;
            try
            {
                if (_searchInFolders)
                {
                    files = activeDir.EnumerateFiles("*", SearchOption.AllDirectories);
                }
                else
                {
                    files = activeDir.EnumerateFiles();
                }
            }
            catch (Exception)
            {
                return;
            }

            foreach (FileInfo file in files)
            {
                Stats.IncrementTotal();

                var name = file.Name;

                // ignore if filtered out
                if (filter != null && filter.IsMatch(name) == _globInfo.IsExclude)
                {
                    if (!_inActiveFiles.ContainsKey(name))
                        _inActiveFiles.Add(name, InactiveReason.Filtered);
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
                    if (!_inActiveFiles.ContainsKey(name))
                        _inActiveFiles.Add(name, InactiveReason.Hidden);
                    continue;
                }

                _files.Add(new RenameItemInfo(file, hidden, _globInfo.PreserveExt));
            }
        }
    }
}
