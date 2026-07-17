using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace RegexRenamer.Utility
{
    public static class FileSystemExtensions
    {
        public static string GetNextAvailableFileName(this string filePath, string folderPath, string suffixFormat = "-({0})")
        {
            var (newFilename, newFilePath) = GetNextAvailableFilePathInternal(filePath, folderPath, suffixFormat);
            return newFilePath;
        }

        public static string GetNextAvailableFilePath(this string filePath, string folderPath, string suffixFormat = "-({0})")
        {
            var (newFilename, newFilePath) = GetNextAvailableFilePathInternal(filePath, folderPath, suffixFormat);
            return newFilePath;
        }

        private static (string filename, string filepath) GetNextAvailableFilePathInternal(this string filePath, string folderPath, string suffixFormat = "-({0})")
        {
            string baseFileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            int counter = 1;
            string newFileName = $"{baseFileName}{extension}";
            var newFilePath = Path.Combine(folderPath, newFileName);
            while (File.Exists(newFilePath))
            {
                newFileName = $"{baseFileName}{string.Format(suffixFormat, counter)}{extension}";
                newFilePath = Path.Combine(folderPath, newFileName);
                counter++;
            }
            return (newFileName, newFilePath);
        }
    }
}
