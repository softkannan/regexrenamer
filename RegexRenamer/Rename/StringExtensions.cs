using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexRenamer.Rename;

/// <summary>
/// Layout size: 32 bytes total. 
/// Perfectly aligned, zero compiler padding.
/// </summary>
public readonly record struct SeparatedFileInfo(
    long Length,               // 8 bytes
    DateTime LastWriteTime,    // 8 bytes
    string NameWithoutExt,     // 8 bytes (Pointer to heap string)
    string Extension           // 8 bytes (Pointer to heap string)
);

public static partial class StringExtensions
{
    private const int MaxStackAllocSize = 256; // Max filename length we expect to handle without heap allocation

    public static string NormalizeToC(this ReadOnlySpan<char> pThis)
    {
        Span<char> cleanStrSpan = pThis.Length <= MaxStackAllocSize
                ? stackalloc char[pThis.Length]
                : new char[pThis.Length]; // Fallback to heap if filename is abnormally huge

        string retVal = string.Empty;
        if (pThis.TryNormalize(cleanStrSpan, out int charsWritten, NormalizationForm.FormC))
        {
            retVal = cleanStrSpan.ToString();
        }
        else
        {
            retVal = pThis.ToString().Normalize(NormalizationForm.FormC);
        }
        return retVal;
    }
    public static SeparatedFileInfo ContextualFactory(ref FileSystemEntry entry)
    {
        ReadOnlySpan<char> nameSpan = entry.FileName;
        int lastDot = nameSpan.LastIndexOf('.');

        string cleanName = string.Empty;
        string extension = string.Empty;

        // Check if an extension actually exists (guarding against '.gitignore' or no extension)
        if (lastDot > 0)
        {
            // Slice out both parts using zero-allocation spans
            ReadOnlySpan<char> namePart = nameSpan.Slice(0, lastDot);
            ReadOnlySpan<char> extPart = nameSpan.Slice(lastDot); // Includes the dot (e.g., ".txt")

            cleanName = NormalizeSpanToC(namePart);
            extension = NormalizeSpanToC(extPart);
        }
        else
        {
            cleanName = NormalizeToC(nameSpan);
        }

        return new SeparatedFileInfo(
            entry.Length,
            entry.LastWriteTimeUtc.UtcDateTime,
            cleanName,
            extension
        );
    }
    // A thread-safe pool holding our unique extension strings
    private static readonly ConcurrentDictionary<string, string> _pool = new(StringComparer.OrdinalIgnoreCase);

    public static string GetExtension(ReadOnlySpan<char> extSpan)
    {
        if (extSpan.IsEmpty) return string.Empty;

        // 1. Check if the extension is already in our pool safely
        // In modern .NET, we can use AlternateLookup to check a dictionary 
        // using a Span directly without allocating a string string key!
        var lookup = _pool.GetAlternateLookup<ReadOnlySpan<char>>();

        if (lookup.TryGetValue(extSpan, out string sharedExtension))
        {
            return sharedExtension; // Found! Returns the existing 8-byte pointer.
        }

        // 2. If it's a brand new extension, allocate it ONCE using string.Create
        string newExtension = string.Create(extSpan.Length, extSpan, (dest, src) =>
        {
            src.CopyTo(dest);
        });

        // 3. Store it in the pool for all future files to reuse
        _pool.TryAdd(newExtension, newExtension);
        return newExtension;
    }
    public static string NormalizeSpanToC(ReadOnlySpan<char> sourceSpan)
    {
        if (sourceSpan.IsEmpty) return string.Empty;

        // 1. Allocate a temporary buffer on the Stack. 
        // Tamil characters usually shrink or stay the same size during FormC normalization,
        // so making the buffer the same size as the source is perfectly safe.
        Span<char> buffer = sourceSpan.Length <= 256
            ? stackalloc char[sourceSpan.Length]
            : new char[sourceSpan.Length]; // Fallback to heap if filename is abnormally huge

        // 2. Perform the normalization directly into our temporary stack buffer
        if (sourceSpan.TryNormalize(buffer, out int charsWritten, NormalizationForm.FormC))
        {
            ReadOnlySpan<char> normalizedResult = buffer.Slice(0, charsWritten);

            // 3. Use string.Create to build the final string out of the normalized stack memory
            return string.Create(normalizedResult.Length, normalizedResult, (destWindow, srcSpan) =>
            {
                srcSpan.CopyTo(destWindow);
            });
        }

        // Fallback safety if the normalized text somehow expanded beyond the buffer size
        return sourceSpan.ToString().Normalize(NormalizationForm.FormC);
    }
    // The compiler writes the C# code for this regex at compile-time.
    // Zero runtime parsing, zero warmup penalty, maximum performance.
    [GeneratedRegex(@"^[\p{IsTamil}\s\d\-_]+$", RegexOptions.CultureInvariant)]
    private static partial Regex TamilNameRegex();
    public static bool FastPreFilterMatch(ReadOnlySpan<char> fileName, string extension)
    {
        // 1. Lightning fast O(1) trailing check (Ordinal)
        if (!fileName.EndsWith(extension, StringComparison.Ordinal))
            return false;

        // 2. Cheap character-level range sweep before committing to full Regex
        bool containsTamil = false;
        foreach (char c in fileName)
        {
            if (c >= '\u0B80' && c <= '\u0BFF') // Quick bounds check for the Tamil Unicode block
            {
                containsTamil = true;
                break;
            }
        }

        if (!containsTamil) return false;

        // 3. Only run the complex Regex if the fast checks pass
        return TamilNameRegex().IsMatch(fileName);
    }

    /// <summary>
    /// Determines a text file's encoding by analyzing its byte order mark (BOM).
    /// Defaults to ASCII when detection of the text file's endianness fails.
    /// </summary>
    /// <param name="filename">The text file to analyze.</param>
    /// <returns>The detected encoding.</returns>
    public static Tuple<string,Encoding> GetEncoding(this string filename)
    {
        // Read the BOM
        var bom = new byte[4];
        using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
        {
            file.ReadExactly(bom, 0, 4);
        }

        // Analyze the BOM
#pragma warning disable SYSLIB0001 // Type or member is obsolete
        if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return new Tuple<string, Encoding>("utf-7",Encoding.UTF7);
#pragma warning restore SYSLIB0001 // Type or member is obsolete
        if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return new Tuple<string, Encoding>("utf-8",Encoding.UTF8);
        if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return new Tuple<string, Encoding>("utf-16",Encoding.UTF32); //UTF-32LE
        if (bom[0] == 0xff && bom[1] == 0xfe) return new Tuple<string, Encoding>("utf-16", Encoding.Unicode); //UTF-16LE
        if (bom[0] == 0xfe && bom[1] == 0xff) return new Tuple<string, Encoding>("utf-16be", Encoding.BigEndianUnicode); //UTF-16BE
        if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new Tuple<string, Encoding>("utf-16be", new UTF32Encoding(true, true));  //UTF-32BE

        // We actually have no idea what the encoding is if we reach this point, so
        // you may wish to return null instead of defaulting to ASCII
        return new Tuple<string, Encoding>("ascii", Encoding.ASCII);
    }

    public static string FormatInvariant(this string format, params object[] args)
    {
        return string.Format(CultureInfo.InvariantCulture, format, args);
    }

    public static string FormatCurrentCulture(this string format, params object[] args)
    {
        return string.Format(CultureInfo.CurrentCulture, format, args);
    }

    public static string FilenameToUri(this string str)
    {
        return Uri.EscapeDataString(str);
    }

    public static string UriToFilename(this string str)
    {
        return Uri.UnescapeDataString(str);
    }

    public static bool IsValidFileName(this string str)
    {
        return !string.IsNullOrEmpty(str) && str.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
    }

    public static string ToCleanFileName(this string str)
    {
        str = str.Trim();
        str = Uri.UnescapeDataString(str);
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            str = str.Replace(c, ' ');
        }
        char[] repeatingChars = {'-',' ','_'};
        foreach (char c in repeatingChars)
        {
            str = str.RemoveRepeatingChars(c);
        }
        return str;
    }
    public static string RemoveRepeatingChars(this string str,char onlyChar)
    {
        var strResult = new StringBuilder();

        foreach (var element in str.ToCharArray())
        {
            if (strResult.Length == 0 || element != onlyChar || strResult[strResult.Length - 1] != element)
                strResult.Append(element);
        }

        return strResult.ToString();
    }
    public static string RemoveRepeatingChars(this string str)
    {
        var strResult = new StringBuilder();

        foreach (var element in str.ToCharArray())
        {
            if (strResult.Length == 0 || strResult[strResult.Length - 1] != element)
                strResult.Append(element);
        }

        return strResult.ToString();
    }

    // Returns a string you can safely use as valid Windows or Linux file name.
    // Does not check for reserved words.
    public static string ToSafeFileName(this string str)
    {
        str = str.Trim().Replace(' ', '-');
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            str = str.Replace(c, '-');
        }
        return str;
    }
    
    public static string Truncate(this string str, int maxLength)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }
        return str.Length <= maxLength ? str : str.Substring(0, maxLength);
    }
}
