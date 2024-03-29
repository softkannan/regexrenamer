﻿using System.IO;

namespace RegexRenamer.Kavita
{
    public static class PathExtensions
    {
        public static string GetFullPathWithoutExtension(this string filepath)
        {
            if (string.IsNullOrEmpty(filepath)) return filepath;
            var extension = Path.GetExtension(filepath);
            if (string.IsNullOrEmpty(extension)) return filepath;
            return Path.GetFullPath(filepath.Replace(extension, string.Empty));
        }

        public static string GetParentDirectory(string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }
    }
}