using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace RegexRenamer.Utility
{
    public static class FileSystemExtensions
    {
        public static string GetNextAvailableFileName(this string fileName, string folderPath, string suffixFormat = "-({0})")
        {
            string baseFileName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            int counter = 1;
            string newFileName = fileName;
            while (File.Exists(Path.Combine(folderPath, newFileName)))
            {
                newFileName = $"{baseFileName}{string.Format(suffixFormat, counter)}{extension}";
                counter++;
            }
            return newFileName;
        }
    }
}
