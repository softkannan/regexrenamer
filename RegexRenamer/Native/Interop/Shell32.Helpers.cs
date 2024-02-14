using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PInvoke
{
    /// <content>
    /// Methods and nested types that are not strictly P/Invokes but provide
    /// a slightly higher level of functionality to ease calling into native code.
    /// </content>
    public static partial class NativeShell32
    {
        /// <summary>Converts an Unicode string to ANSI.</summary>
        /// <param name="value">The Unicode string value.</param>
        /// <returns>The ANSI string value.</returns>
        internal static byte[] UnicodeToAnsiBytes(string value)
        {
            if (string.IsNullOrEmpty(value)) return new byte[0];
            byte[] ret = null;
            var sz = Kernel32.WideCharToMultiByte(0, 0, value, value.Length, ret, 0);
            ret = new byte[sz == 0 ? 0 : sz + 1];
            Kernel32.WideCharToMultiByte(0, 0, value, value.Length, ret, sz);
            return ret;
        }

        /// <summary>Converts an ANSI string to Unicode.</summary>
        /// <param name="value">The ANSI string value.</param>
        /// <returns>The Unicode string value.</returns>
        internal static string AnsiToUnicode(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            byte[] ret = null;
            var sz = Kernel32.MultiByteToWideChar(0, 0, value, value.Length, ret, 0);
            ret = new byte[(int)sz];
            Kernel32.MultiByteToWideChar(0, 0, value, value.Length, ret, sz);
            return Encoding.Unicode.GetString(ret);
        }
    }
}