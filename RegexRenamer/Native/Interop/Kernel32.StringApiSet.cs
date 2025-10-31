using System;
using System.Runtime.InteropServices;
using System.Text;

using HWND = System.IntPtr;
using HANDLE = System.IntPtr;

namespace PInvoke;

internal static class StringAPI
{
    internal const int MAX_PATH           = 260;
    internal const uint CP_ACP            = 0;
    internal const uint CP_MACCP          = 2;
    internal const uint CP_OEMCP          = 1;
    internal const uint CP_SYMBOL         = 42;
    internal const uint CP_THREAD_ACP     = 3;
    internal const uint CP_UTF7           = 65000;
    internal const uint CP_UTF8           = 65001;
    internal const uint CSTR_EQUAL        = 2;
    internal const uint CSTR_GREATER_THAN = 3;
    internal const uint CSTR_LESS_THAN    = 1;

    /// <summary>Flags used by CompareString and CompareStringEx</summary>
    internal enum COMPARE_STRING
    {
        NORM_IGNORECASE            = 1,
        NORM_IGNORENONSPACE        = 2,
        NORM_IGNORESYMBOLS         = 4,
        NORM_IGNOREKANATYPE        = 65536,
        NORM_IGNOREWIDTH           = 131072,
        LINGUISTIC_IGNORECASE      = 16,
        LINGUISTIC_IGNOREDIACRITIC = 32,
        NORM_LINGUISTIC_CASING     = 134217728,
        SORT_STRINGSORT            = 0x00001000,
        SORT_DIGITSASNUMBERS       = 8
    }

    /// <summary>Flags indicating the conversion type.</summary>
    [Flags]
    internal enum MBCONV
    {
        MB_PRECOMPOSED       = 0x00000001,
        MB_COMPOSITE         = 0x00000002,
        MB_USEGLYPHCHARS     = 0x00000004,
        MB_ERR_INVALID_CHARS = 0x00000008,
    }

    /// <summary>Flags specifying the type of transformation to use during string mapping.</summary>
    [Flags]
    internal enum STRING_MAPPING
    {
        MAP_FOLDCZONE        = 16,
        MAP_PRECOMPOSED      = 32,
        MAP_COMPOSITE        = 64,
        MAP_FOLDDIGITS       = 128,
        MAP_EXPAND_LIGATURES = 8192,
    }

    /// <summary>Flags indicating the conversion type.</summary>
    [Flags]
    internal enum WCCONV
    {
        WC_COMPOSITECHECK    = 0x00000200,
        WC_DISCARDNS         = 0x00000010,
        WC_SEPCHARS          = 0x00000020,
        WC_DEFAULTCHAR       = 0x00000040,
        WC_ERR_INVALID_CHARS = 0x00000080,
        WC_NO_BEST_FIT_CHARS = 0x00000400,
    }
    
    [DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
    internal static extern int CompareStringEx(string lpLocaleName, COMPARE_STRING dwCmpFlags, string lpString1, int cchCount1, string lpString2, int cchCount2, [Optional] nint lpVersionInformation, [Optional] nint lpReserved, [Optional] nint lParam);
    
    [DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
    internal static extern int CompareStringOrdinal(string lpString1, int cchCount1, string lpString2, int cchCount2, [MarshalAs(UnmanagedType.Bool)] bool bIgnoreCase);
    
    [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern int FoldString(STRING_MAPPING dwMapFlags, string lpSrcStr, int cchSrc, StringBuilder lpDestStr, int cchDest);

    [DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true)]
    internal static extern int MultiByteToWideChar(uint CodePage, MBCONV dwFlags, [In][MarshalAs(UnmanagedType.LPStr)] string lpMultiByteStr, int cbMultiByte,
        [Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpWideCharStr, int cchWideChar);

    [DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true)]
    internal static extern int MultiByteToWideChar(uint CodePage, MBCONV dwFlags, [In][MarshalAs(UnmanagedType.LPStr)] string lpMultiByteStr, int cbMultiByte,
        [Out] byte[] lpWideCharStr, int cchWideChar);

    [DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true)]
    internal static extern int WideCharToMultiByte(uint CodePage, WCCONV dwFlags, [In][MarshalAs(UnmanagedType.LPWStr)] string lpWideCharStr, int cchWideChar,
        [Out, Optional, MarshalAs(UnmanagedType.LPStr)] StringBuilder lpMultiByteStr, int cbMultiByte, [In][MarshalAs(UnmanagedType.LPStr)] string lpDefaultChar, [MarshalAs(UnmanagedType.Bool)] out bool lpUsedDefaultChar);

    [DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true)]
    internal static extern int WideCharToMultiByte(uint CodePage, WCCONV dwFlags, [In][MarshalAs(UnmanagedType.LPWStr)] string lpWideCharStr, int cchWideChar,
        [Out, Optional, MarshalAs(UnmanagedType.LPStr)] StringBuilder lpMultiByteStr, int cbMultiByte, nint lpDefaultChar = default, nint lpUsedDefaultChar = default);

    [DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true)]
    internal static extern int WideCharToMultiByte(uint CodePage, WCCONV dwFlags, [In][MarshalAs(UnmanagedType.LPWStr)] string lpWideCharStr, int cchWideChar,
        [Out] byte[] lpMultiByteStr, int cbMultiByte, nint lpDefaultChar = default, nint lpUsedDefaultChar = default);

    /// <summary>Converts an Unicode string to ANSI.</summary>
    /// <param name="value">The Unicode string value.</param>
    /// <returns>The ANSI string value.</returns>
    internal static byte[] UnicodeToAnsiBytes(string value)
    {
        if (string.IsNullOrEmpty(value)) return new byte[0];
        byte[] ret = null;
        var sz = WideCharToMultiByte(0, 0, value, value.Length, ret, 0);
        ret = new byte[sz == 0 ? 0 : sz + 1];
        WideCharToMultiByte(0, 0, value, value.Length, ret, sz);
        return ret;
    }

    /// <summary>Converts an ANSI string to Unicode.</summary>
    /// <param name="value">The ANSI string value.</param>
    /// <returns>The Unicode string value.</returns>
    internal static string AnsiToUnicode(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        byte[] ret = null;
        var sz = MultiByteToWideChar(0, 0, value, value.Length, ret, 0);
        ret = new byte[(int)sz];
        MultiByteToWideChar(0, 0, value, value.Length, ret, sz);
        return Encoding.Unicode.GetString(ret);
    }
}