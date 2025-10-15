using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Kavita;

public static class FilesHelper
{
    public static List<Tuple<FileInfo, ComicInfo>> GetFileInfo(this string pThis, string searchPattern = "*.*", bool recursiveLookup = false)
    {
        List<Tuple<FileInfo, ComicInfo>> selectedFiles = new List<Tuple<FileInfo, ComicInfo>>();
        foreach (var fileItem in Directory.GetFiles(pThis, searchPattern, recursiveLookup ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
        {
            var fileInfo = new FileInfo(fileItem);
            var comicInfo = new ComicInfo();
            selectedFiles.Add(new Tuple<FileInfo, ComicInfo>(fileInfo, comicInfo));
        }

        return selectedFiles;
    }

}
