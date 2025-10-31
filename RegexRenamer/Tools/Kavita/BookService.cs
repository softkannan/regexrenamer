using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using Kavita.Extensions;
using Kavita.Helpers;
using VersOne.Epub;
using VersOne.Epub.Options;
using VersOne.Epub.Schema;
using Kavita.Enum;
using Kavita.ParserImpl;

namespace Kavita;

#nullable enable

public partial class BookService 
{
    private readonly IDirectoryService _directoryService;
    private const string CssScopeClass = ".book-content";
    private const string BookApiUrl = "book-resources?file=";
    private readonly PdfComicInfoExtractor _pdfComicInfoExtractor;

    /// <summary>
    /// Setup the most lenient book parsing options possible as people have some really bad epubs
    /// </summary>
    public static readonly EpubReaderOptions BookReaderOptions = new()
    {
        PackageReaderOptions = new PackageReaderOptions
        {
            IgnoreMissingToc = true,
            SkipInvalidManifestItems = true,
        },
        Epub2NcxReaderOptions = new Epub2NcxReaderOptions
        {
            IgnoreMissingContentForNavigationPoints = false
        },
        SpineReaderOptions = new SpineReaderOptions
        {
            IgnoreMissingManifestItems = false
        },
        BookCoverReaderOptions =  new BookCoverReaderOptions
        {
            Epub2MetadataIgnoreMissingManifestItem = false
        }
    };

    public static readonly EpubReaderOptions LenientBookReaderOptions = new()
    {
        PackageReaderOptions = new PackageReaderOptions
        {
            IgnoreMissingToc = true,
            SkipInvalidManifestItems = true,
        },
        Epub2NcxReaderOptions = new Epub2NcxReaderOptions
        {
            IgnoreMissingContentForNavigationPoints = false
        },
        SpineReaderOptions = new SpineReaderOptions
        {
            IgnoreMissingManifestItems = false
        },
        BookCoverReaderOptions =  new BookCoverReaderOptions
        {
            Epub2MetadataIgnoreMissingManifestItem = true
        }
    };

    public BookService(IDirectoryService directoryService)
    {
        _directoryService = directoryService;
        _pdfComicInfoExtractor = new PdfComicInfoExtractor();
    }
    
    private ComicInfo? GetEpubComicInfo(string filePath)
    {
        EpubBookRef? epubBook = null;

        try
        {
            epubBook = OpenEpubWithFallback(filePath, epubBook);

            var publicationDate =
                epubBook?.Schema.Package.Metadata.Dates.Find(pDate => pDate.Event == "publication")?.Date ?? string.Empty;

            if (string.IsNullOrEmpty(publicationDate))
            {
                publicationDate = epubBook?.Schema.Package.Metadata.Dates.FirstOrDefault()?.Date;
            }

            var (year, month, day) = GetPublicationDate(publicationDate);

            var summary = epubBook?.Schema.Package.Metadata.Descriptions.FirstOrDefault();
            var info = new ComicInfo
            {
                Summary = string.IsNullOrEmpty(summary?.Description) ? string.Empty : summary.Description,
                Publisher = string.Join(",", epubBook?.Schema.Package.Metadata.Publishers.Select(p => p.Publisher) ?? new string[] { }),
                Month = month,
                Day = day,
                Year = year,
                Title = epubBook?.Title ?? string.Empty,
                Genre = string.Join(",",
                    epubBook?.Schema.Package.Metadata.Subjects.Select(s => s.Subject.ToLower().Trim()) ?? new string[] { }),
                LanguageISO = ValidateLanguage(epubBook?.Schema.Package.Metadata.Languages
                    .Select(l => l.Language)
                    .FirstOrDefault())
            };
            ComicInfo.CleanComicInfo(info);

            var weblinks = new List<string>();
            foreach (var identifier in epubBook?.Schema.Package.Metadata.Identifiers ?? new List<EpubMetadataIdentifier>())
            {
                if (string.IsNullOrEmpty(identifier.Identifier)) continue;
                if (!string.IsNullOrEmpty(identifier.Scheme) &&
                    identifier.Scheme.Equals("ISBN", StringComparison.InvariantCultureIgnoreCase))
                {
                    var isbn = identifier.Identifier.Replace("urn:isbn:", string.Empty).Replace("isbn:", string.Empty);

                    info.Isbn = isbn;
                }

                if ((!string.IsNullOrEmpty(identifier.Scheme) &&
                     identifier.Scheme.Equals("URL", StringComparison.InvariantCultureIgnoreCase)) ||
                    identifier.Identifier.StartsWith("url:"))
                {
                    var url = identifier.Identifier.Replace("url:", string.Empty);
                    weblinks.Add(url.Trim());
                }
            }

            if (weblinks.Count > 0)
            {
                info.Web = string.Join(',', weblinks.Distinct());
            }

            // Parse tags not exposed via Library
            foreach (var metadataItem in epubBook?.Schema.Package.Metadata.MetaItems ?? new List<EpubMetadataMeta>())
            {
                // EPUB 2 and 3
                switch (metadataItem.Name)
                {
                    case "calibre:rating":
                        info.UserRating = metadataItem.Content.AsFloat();
                        break;
                    case "calibre:title_sort":
                        info.TitleSort = metadataItem.Content;
                        break;
                    case "calibre:series":
                        info.Series = metadataItem.Content;
                        if (string.IsNullOrEmpty(info.SeriesSort))
                        {
                            info.SeriesSort = metadataItem.Content;
                        }

                        break;
                    case "calibre:series_index":
                        info.Volume = metadataItem.Content;
                        break;
                }


                // EPUB 3.2+ only
                switch (metadataItem.Property)
                {
                    case "group-position":
                        info.Volume = metadataItem.Content;
                        break;
                    case "belongs-to-collection":
                        info.Series = metadataItem.Content;
                        if (string.IsNullOrEmpty(info.SeriesSort))
                        {
                            info.SeriesSort = metadataItem.Content;
                        }

                        break;
                    case "collection-type":
                        // These look to be genres from https://manual.calibre-ebook.com/sub_groups.html or can be "series"
                        break;
                    case "role":
                        if (metadataItem.Scheme != null && !metadataItem.Scheme.Equals("marc:relators")) break;

                        var creatorId = metadataItem.Refines?.Replace("#", string.Empty);
                        var person = epubBook?.Schema.Package.Metadata.Creators
                            .SingleOrDefault(c => c.Id == creatorId);
                        if (person == null) break;

                        PopulatePerson(metadataItem, info, person);
                        break;
                    case "title-type":
                        if (metadataItem.Content.Equals("collection"))
                        {
                            ExtractCollectionOrReadingList(metadataItem, epubBook, info);
                        }

                        if (metadataItem.Content.Equals("main"))
                        {
                            ExtractSortTitle(metadataItem, epubBook, info);
                        }

                        break;
                }
            }

            // If this is a single book and not a collection, set publication status to Completed
            if (string.IsNullOrEmpty(info.Volume) &&
                Parser.ParseVolume(filePath, LibraryType.Manga).Equals(Parser.LooseLeafVolume))
            {
                info.Count = 1;
            }

            // Include regular Writer as well, for cases where there is no special tag
            info.Writer = string.Join(",",
                epubBook?.Schema.Package.Metadata.Creators.Select(c => Parser.CleanAuthor(c.Creator)) ?? new string[] { });

            var hasVolumeInSeries = !Parser.ParseVolume(info.Title, LibraryType.Manga)
                .Equals(Parser.LooseLeafVolume);

            if (string.IsNullOrEmpty(info.Volume) && hasVolumeInSeries &&
                (!info.Series.Equals(info.Title) || string.IsNullOrEmpty(info.Series)))
            {
                // This is likely a light novel for which we can set series from parsed title
                info.Series = Parser.ParseSeries(info.Title, LibraryType.Manga);
                info.Volume = Parser.ParseVolume(info.Title, LibraryType.Manga);
            }

            return info;
        }
        catch (Exception)
        {
            
        }
        finally
        {
            epubBook?.Dispose();
        }

        return null;
    }

    private EpubBookRef? OpenEpubWithFallback(string filePath, EpubBookRef? epubBook)
    {
        // TODO: Refactor this to use the Async version
        try
        {
            // Read file into memory stream to avoid locking file on disk
            MemoryStream ms;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ms = new MemoryStream();
                fs.CopyTo(ms);
                ms.Position = 0;
            }
            epubBook = EpubReader.OpenBook(ms, BookReaderOptions);
        }
        catch (Exception)
        {
           
        }
        finally
        {
            if (epubBook == null)
            {
                MemoryStream ms;
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    ms = new MemoryStream();
                    fs.CopyTo(ms);
                    ms.Position = 0;
                }
                epubBook = EpubReader.OpenBook(ms, LenientBookReaderOptions);
            }
        }
        return epubBook;
    }

    public ComicInfo? GetComicInfo(string filePath)
    {
        if (!IsValidFile(filePath)) return null;

        if (Parser.IsPdf(filePath))
        {
            return _pdfComicInfoExtractor.GetComicInfo(filePath);
        }

        return GetEpubComicInfo(filePath);
    }

    private static void ExtractSortTitle(EpubMetadataMeta metadataItem, EpubBookRef? epubBook, ComicInfo info)
    {
        var titleId = metadataItem.Refines?.Replace("#", string.Empty);
        var titleElem = epubBook?.Schema.Package.Metadata.Titles
            .Find(item => item.Id == titleId);
        if (titleElem == null) return;

        var sortTitleElem = epubBook?.Schema.Package.Metadata.MetaItems
            .Find(item =>
                item.Property == "file-as" && item.Refines == metadataItem.Refines);
        if (sortTitleElem == null || string.IsNullOrWhiteSpace(sortTitleElem.Content)) return;
        info.SeriesSort = sortTitleElem.Content;
    }

    private static void ExtractCollectionOrReadingList(EpubMetadataMeta metadataItem, EpubBookRef? epubBook, ComicInfo info)
    {
        var titleId = metadataItem.Refines?.Replace("#", string.Empty);
        var readingListElem = epubBook?.Schema.Package.Metadata.Titles
            .Find(item => item.Id == titleId);
        if (readingListElem == null) return;

        var count = epubBook?.Schema.Package.Metadata.MetaItems
            .Find(item =>
                item.Property == "display-seq" && item.Refines == metadataItem.Refines);
        if (count == null || count.Content == "0")
        {
            // Treat this as a Collection
            info.SeriesGroup += (string.IsNullOrEmpty(info.StoryArc) ? string.Empty : ",") +
                                readingListElem.Title.Replace(',', '_');
        }
        else
        {
            // Treat as a reading list
            info.AlternateSeries += (string.IsNullOrEmpty(info.AlternateSeries) ? string.Empty : ",") +
                                    readingListElem.Title.Replace(',', '_');
            info.AlternateNumber += (string.IsNullOrEmpty(info.AlternateNumber) ? string.Empty : ",") + count.Content;
        }
    }

    private static void PopulatePerson(EpubMetadataMeta metadataItem, ComicInfo info, EpubMetadataCreator person)
    {
        switch (metadataItem.Content)
        {
            case "art":
            case "artist":
                info.CoverArtist += AppendAuthor(person);
                return;
            case "aut":
            case "author":
            case "creator":
            case "cre":
                info.Writer += AppendAuthor(person);
                return;
            case "pbl":
            case "publisher":
                info.Publisher += AppendAuthor(person);
                return;
            case "trl":
            case "translator":
                info.Translator += AppendAuthor(person);
                return;
            case "edt":
            case "editor":
                info.Editor += AppendAuthor(person);
                return;
            case "ill":
            case "illustrator":
                info.Inker += AppendAuthor(person);
                return;
            case "clr":
            case "colorist":
                info.Colorist += AppendAuthor(person);
                return;
        }
    }

    private static string AppendAuthor(EpubMetadataCreator person)
    {
        return Parser.CleanAuthor(person.Creator) + ",";
    }

    private static (int year, int month, int day) GetPublicationDate(string? publicationDate)
    {
        var year = 0;
        var month = 0;
        var day = 0;
        if (string.IsNullOrEmpty(publicationDate)) return (year, month, day);
        switch (DateTime.TryParse(publicationDate, CultureInfo.InvariantCulture, out var date))
        {
            case true:
                year = date.Year;
                month = date.Month;
                day = date.Day;
                break;
            case false when !string.IsNullOrEmpty(publicationDate) && publicationDate.Length == 4:
                int.TryParse(publicationDate, out year);
                break;
        }

        return (year, month, day);
    }

    public static string ValidateLanguage(string? language)
    {
        if (string.IsNullOrEmpty(language)) return string.Empty;

        try
        {
            return CultureInfo.GetCultureInfo(language).ToString();
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    private bool IsValidFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return false;
        }

        if (Parser.IsBook(filePath)) return true;

        return false;
    }

    /// <summary>
    /// Parses out Title from book. Chapters and Volumes will always be "0". If there is any exception reading book (malformed books)
    /// then null is returned. This expects only an epub file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public ParserInfo? ParseInfo(string filePath)
    {
        if (!Parser.IsEpub(filePath) || !File.Exists(filePath)) return null;

        try
        {
            using var epubBook = EpubReader.OpenBook(filePath, LenientBookReaderOptions);

            // <meta content="The Dark Tower" name="calibre:series"/>
            // <meta content="Wolves of the Calla" name="calibre:title_sort"/>
            // If all three are present, we can take that over dc:title and format as:
            // Series = The Dark Tower, Volume = 5, Filename as "Wolves of the Calla"
            // In addition, the following can exist and should parse as a series (EPUB 3.2 spec)
            // <meta property="belongs-to-collection" id="c01">
            //   The Lord of the Rings
            // </meta>
            // <meta refines="#c01" property="collection-type">set</meta>
            // <meta refines="#c01" property="group-position">2</meta>
            try
            {
                var seriesIndex = string.Empty;
                var series = string.Empty;
                var specialName = string.Empty;


                foreach (var metadataItem in epubBook.Schema.Package.Metadata.MetaItems)
                {
                    // EPUB 2 and 3
                    switch (metadataItem.Name)
                    {
                        case "calibre:series_index":
                            seriesIndex = metadataItem.Content;
                            break;
                        case "calibre:series":
                            series = metadataItem.Content;
                            break;
                        case "calibre:title_sort":
                            specialName = metadataItem.Content;
                            break;
                    }

                    // EPUB 3.2+ only
                    switch (metadataItem.Property)
                    {
                        case "group-position":
                            seriesIndex = metadataItem.Content;
                            break;
                        case "belongs-to-collection":
                            series = metadataItem.Content;
                            break;
                        case "collection-type":
                            // These look to be genres from https://manual.calibre-ebook.com/sub_groups.html or can be "series"
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(series) && !string.IsNullOrEmpty(seriesIndex))
                {
                    if (string.IsNullOrEmpty(specialName))
                    {
                        specialName = epubBook.Title;
                    }
                    var info = new ParserInfo
                    {
                        Chapters = Parser.DefaultChapter,
                        Edition = string.Empty,
                        Format = MangaFormat.Epub,
                        Filename = Path.GetFileName(filePath),
                        Title = specialName?.Trim() ?? string.Empty,
                        FullFilePath = Parser.NormalizePath(filePath),
                        IsSpecial = Parser.HasSpecialMarker(filePath),
                        Series = series.Trim(),
                        SeriesSort = series.Trim(),
                        Volumes = seriesIndex
                    };

                    return info;
                }
            }
            catch (Exception)
            {
                // Swallow exception
            }

            return new ParserInfo
            {
                Chapters = Parser.DefaultChapter,
                Edition = string.Empty,
                Format = MangaFormat.Epub,
                Filename = Path.GetFileName(filePath),
                Title = epubBook.Title.Trim(),
                FullFilePath = Parser.NormalizePath(filePath),
                IsSpecial = Parser.HasSpecialMarker(filePath),
                Series = epubBook.Title.Trim(),
                Volumes = Parser.LooseLeafVolume,
            };
        }
        catch (Exception)
        {
           
        }

        return null;
    }


    [GeneratedRegex(@"\.\./")]
    private static partial Regex ParentDirectoryRegex();
}
