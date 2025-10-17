using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Rename
{
    public static class OtherExtensions
    {
        public static string GetHumanReadableBytes(this FileInfo pThis)
        {
            long bytes = pThis.Length;
            return bytes.GetHumanReadableBytes();
        }
        public static string GetHumanReadableBytes(this long pThis)
        {
            // Get absolute value
            var absoluteBytes = (pThis < 0 ? -pThis : pThis);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            switch (absoluteBytes)
            {
                // Exabyte
                case >= 0x1000000000000000:
                    suffix = "EB";
                    readable = (pThis >> 50);
                    break;
                // Petabyte
                case >= 0x4000000000000:
                    suffix = "PB";
                    readable = (pThis >> 40);
                    break;
                // Terabyte
                case >= 0x10000000000:
                    suffix = "TB";
                    readable = (pThis >> 30);
                    break;
                // Gigabyte
                case >= 0x40000000:
                    suffix = "GB";
                    readable = (pThis >> 20);
                    break;
                // Megabyte
                case >= 0x100000:
                    suffix = "MB";
                    readable = (pThis >> 10);
                    break;
                // Kilobyte
                case >= 0x400:
                    suffix = "KB";
                    readable = pThis;
                    break;
                default:
                    return pThis.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.## ") + suffix;
        }
    }
}
