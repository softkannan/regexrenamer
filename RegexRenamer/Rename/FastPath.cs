using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

// disable nullable warnings for this file since it's a low-level utility and we want to avoid the overhead of nullability annotations and checks here.
// The public API can be designed to be non-nullable if desired, but internally we want to keep it simple and efficient.
#pragma warning disable CS8632

namespace RegexRenamer.Rename;

public static class FastPath
{
    private const uint INVALID_FILE_ATTRIBUTES = 0xFFFFFFFF;
    private const uint FILE_ATTRIBUTE_DIRECTORY = 0x10;

    // Win32 error codes (GetLastError)
    private const int ERROR_ACCESS_DENIED = 5;
    private const int ERROR_SHARING_VIOLATION = 32;
    private const int ERROR_LOCK_VIOLATION = 33;

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern uint GetFileAttributesW(string lpFileName);

    /// <summary>
    /// Fast directory exists check using GetFileAttributesW (no exceptions on miss).
    /// Returns true if path exists AND is a directory.
    /// </summary>
    public static bool DirectoryExists(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        // Optional: trim trailing spaces. (Windows paths generally shouldn't have them.)
        path = path.Trim();

        // Avoid the "C:\" vs "C:" oddity (C: refers to current directory on drive C).
        // If you pass "C:" you likely don't mean "exists directory".
        if (path.Length == 2 && path[1] == ':')
            return false;

        uint attrs = GetFileAttributesW(path);
        if (attrs == INVALID_FILE_ATTRIBUTES)
            return false;

        return (attrs & FILE_ATTRIBUTE_DIRECTORY) != 0;
    }

    /// <summary>
    /// Variant: if access is denied (or locked), treat as exists.
    /// Useful when you only need to know "is there something there" even without permission.
    /// </summary>
    public static bool DirectoryExistsTreatAccessDeniedAsExists(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        path = path.Trim();
        if (path.Length == 2 && path[1] == ':')
            return false;

        uint attrs = GetFileAttributesW(path);
        if (attrs == INVALID_FILE_ATTRIBUTES)
        {
            int err = Marshal.GetLastWin32Error();
            // Decide policy: access denied or locked can still mean the directory exists.
            if (err == ERROR_ACCESS_DENIED || err == ERROR_SHARING_VIOLATION || err == ERROR_LOCK_VIOLATION)
                return true;

            return false;
        }

        return (attrs & FILE_ATTRIBUTE_DIRECTORY) != 0;
    }

    public static bool FileExists(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;
        path = path.Trim();
        uint attrs = GetFileAttributesW(path);
        if (attrs == INVALID_FILE_ATTRIBUTES)
            return false;
        return (attrs & FILE_ATTRIBUTE_DIRECTORY) == 0;
    }

    public static bool FileExistsTreatAccessDeniedAsExists(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;
        path = path.Trim();
        uint attrs = GetFileAttributesW(path);
        if (attrs == INVALID_FILE_ATTRIBUTES)
        {
            int err = Marshal.GetLastWin32Error();
            if (err == ERROR_ACCESS_DENIED || err == ERROR_SHARING_VIOLATION || err == ERROR_LOCK_VIOLATION)
                return true;
            return false;
        }
        return (attrs & FILE_ATTRIBUTE_DIRECTORY) == 0;
    }

    public static string GetParentDirectory(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return string.Empty;
        path = path.Trim();
        // Handle root paths (e.g. "C:\") - they have no parent.
        if (path.Length == 3 && path[1] == ':' && path[2] == '\\')
            return string.Empty;
        int lastSeparator = path.LastIndexOfAny(new char[] { '\\', '/' });
        if (lastSeparator <= 0) // No separator or separator is at the start (e.g. "\folder")
            return string.Empty;
        return path.Substring(0, lastSeparator);
    }

}
