/**
 * Contributed by https://github.com/microtherion
 *
 * All references to the "PDF Spec" (section numbers, etc) refer to the
 * PDF 1.7 Specification a.k.a. PDF32000-1:2008
 * https://opensource.adobe.com/dc-acrobat-sdk-docs/pdfstandards/PDF32000_2008.pdf
 */
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using Kavita.Enum;
using Kavita;
using Kavita.ParserImpl;


namespace Kavita.Helpers;
#nullable enable

public interface IPdfComicInfoExtractor
{
    ComicInfo? GetComicInfo(string filePath);
}

/// <summary>
/// Translate PDF metadata (See PdfMetadataExtractor.cs) into ComicInfo structure.
/// </summary>
public class PdfComicInfoExtractor : IPdfComicInfoExtractor
{
    private readonly string[] _pdfDateFormats = [ // PDF Spec 7.9.4
            "D:yyyyMMddHHmmsszzz:", "D:yyyyMMddHHmmss+", "D:yyyyMMddHHmmss",
            "D:yyyyMMddHHmmzzz:",  "D:yyyyMMddHHmm+",   "D:yyyyMMddHHmm",
            "D:yyyyMMddHHzzz:", "D:yyyyMMddHH+", "D:yyyyMMddHH",
            "D:yyyyMMdd", "D:yyyyMM", "D:yyyy"
        ];

    public PdfComicInfoExtractor()
    {
    }

    private static float? GetFloatFromText(string? text)
    {
        if (string.IsNullOrEmpty(text)) return null;

        if (float.TryParse(text, CultureInfo.InvariantCulture, out var value)) return value;

        return null;
    }

    private DateTime? GetDateTimeFromText(string? text)
    {
        if (string.IsNullOrEmpty(text)) return null;

        // Dates stored in the XMP metadata stream (PDF Spec 14.3.2)
        // are stored in ISO 8601 format, which is handled by C# out of the box
        if (DateTime.TryParse(text, CultureInfo.InvariantCulture, out var date)) return date;

        // Dates stored in the document information directory (PDF Spec 14.3.3)
        // are stored in a proprietary format (PDF Spec 7.9.4) that needs to be
        // massaged slightly to be expressible by a DateTime format.
        if (text[0] != 'D') {
            text = "D:" + text;
        }
        text = text.Replace("'", ":");
        text = text.Replace("Z", "+");

        foreach(var format in _pdfDateFormats)
        {
            if (DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var pdfDate)) return pdfDate;
        }

        return null;
    }

    private static string? MaybeGetMetadata(Dictionary<string, string> metadata, string key)
    {
        return metadata.TryGetValue(key, out var value) ? value : null;
    }

    private ComicInfo? GetComicInfoFromMetadata(Dictionary<string, string> metadata, string filePath)
    {
        var info = new ComicInfo();

        var publicationDate = GetDateTimeFromText(MaybeGetMetadata(metadata, "CreationDate"));

        if (publicationDate != null)
        {
            info.Year  = publicationDate.Value.Year;
            info.Month = publicationDate.Value.Month;
            info.Day   = publicationDate.Value.Day;
        }

        info.Summary   = MaybeGetMetadata(metadata, "Summary") ?? string.Empty;
        info.Publisher = MaybeGetMetadata(metadata, "Publisher") ?? string.Empty;
        info.Writer    = MaybeGetMetadata(metadata, "Author") ?? string.Empty;
        info.Title     = MaybeGetMetadata(metadata, "Title") ?? string.Empty;
        info.TitleSort  = MaybeGetMetadata(metadata, "TitleSort") ?? string.Empty;
        info.Genre     = MaybeGetMetadata(metadata, "Subject") ?? string.Empty;
        info.LanguageISO = MaybeGetMetadata(metadata, "Language") ?? string.Empty;
        info.Isbn      = MaybeGetMetadata(metadata, "ISBN") ?? string.Empty;

        info.UserRating = GetFloatFromText(MaybeGetMetadata(metadata, "UserRating")) ?? 0.0f;
        info.Series     = MaybeGetMetadata(metadata, "Series") ?? info.Title;
        info.SeriesSort = info.Series;
        info.Volume     = MaybeGetMetadata(metadata, "Volume") ?? string.Empty;

        // If this is a single book and not a collection, set publication status to Completed
        if (string.IsNullOrEmpty(info.Volume) && Parser.ParseVolume(filePath, LibraryType.Manga).Equals(Parser.LooseLeafVolume))
        {
            info.Count = 1;
        }

        ComicInfo.CleanComicInfo(info);

        return info;
    }

    public ComicInfo? GetComicInfo(string filePath)
    {
        try
        {
            using var extractor = new PdfMetadataExtractor(filePath);

            return GetComicInfoFromMetadata(extractor.GetMetadata(), filePath);
        }
        catch (Exception)
        {
           
        }
        return null;
    }
}
