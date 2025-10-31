using RegexRenamer.Rename;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kavita;

public static class FilesHelper
{
    public static List<Tuple<FileInfo, ComicInfo>> GetFileInfo(this string pThis, string searchPattern = "*.*", bool recursiveLookup = false)
    {
        List<Tuple<FileInfo, ComicInfo>> selectedFiles = new List<Tuple<FileInfo, ComicInfo>>();
        foreach (var fileItem in Directory.GetFiles(pThis, searchPattern, recursiveLookup ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
        {
            var fileInfo = new FileInfo(fileItem);
            var comicInfo = fileInfo.GetMetadata() ?? new ComicInfo();
            selectedFiles.Add(new Tuple<FileInfo, ComicInfo>(fileInfo, comicInfo));
        }

        return selectedFiles;
    }

    public static List<Tuple<RenameItemInfo,ComicInfo, ComicInfo>> GetFileInfo(this string pThis, bool preserveExt, string searchPattern = "*.*",  bool recursiveLookup = false)
    {
        List<Tuple<RenameItemInfo, ComicInfo, ComicInfo>> selectedFiles = new List<Tuple<RenameItemInfo, ComicInfo, ComicInfo>>();
        foreach (var fileItem in Directory.GetFiles(pThis, searchPattern, recursiveLookup ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
        {
            var fileInfo = new FileInfo(fileItem);
            var comicInfo = fileInfo.GetMetadata() ?? new ComicInfo();
            selectedFiles.Add(Tuple.Create(new RenameItemInfo(fileInfo,false,preserveExt), new ComicInfo(), comicInfo));
        }

        return selectedFiles;
    }

}
