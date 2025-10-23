using VersOne.Epub;
using VersOne.Epub.Options;
using VersOne.Epub.Schema;
using System;
using System.IO;
using Kavita.Enum;

namespace Kavita.ParserImpl;
#nullable enable
public class BookParser(IDirectoryService directoryService, BasicParser basicParser) : DefaultParser(directoryService)
{
    public override ParserInfo? Parse(string filePath, string rootPath, string libraryRoot, LibraryType type, bool enableMetadata = true, ComicInfo? comicInfo = null)
    {
        var info = ParseInfo(filePath);
        if (info == null) return null;

        info.ComicInfo = comicInfo;

        // We need a special piece of code to override the Series IF there is a special marker in the filename for epub files
        if (info.IsSpecial && info.Volumes is "0" or "0.0" && info.ComicInfo?.Series != info.Series)
        {
            info.Series = info.ComicInfo?.Series ?? "";
        }

        // This catches when original library type is Manga/Comic and when parsing with non
        if (Parser.ParseVolume(info.Series, type) != Parser.LooseLeafVolume)
        {
            var hasVolumeInTitle = !Parser.ParseVolume(info.Title, type)
                .Equals(Parser.LooseLeafVolume);
            var hasVolumeInSeries = !Parser.ParseVolume(info.Series, type)
                .Equals(Parser.LooseLeafVolume);

            if (string.IsNullOrEmpty(info.ComicInfo?.Volume) && hasVolumeInTitle && (hasVolumeInSeries || string.IsNullOrEmpty(info.Series)))
            {
                // NOTE: I'm not sure the comment is true. I've never seen this triggered
                // This is likely a light novel for which we can set series from parsed title
                info.Series = Parser.ParseSeries(info.Title, type);
                info.Volumes = Parser.ParseVolume(info.Title, type);
            }
            else
            {
                var info2 = basicParser.Parse(filePath, rootPath, libraryRoot, LibraryType.Book, enableMetadata, comicInfo);
                info.Merge(info2);
                if (hasVolumeInSeries && info2 != null && Parser.ParseVolume(info2.Series, type)
                        .Equals(Parser.LooseLeafVolume))
                {
                    // Override the Series name so it groups appropriately
                    info.Series = info2.Series;
                }
            }
        }

        return string.IsNullOrEmpty(info.Series) ? null : info;
    }

    /// <summary>
    /// Only applicable for Epub files
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public override bool IsApplicable(string filePath, LibraryType type)
    {
        return Parser.IsEpub(filePath);
    }

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
        BookCoverReaderOptions = new BookCoverReaderOptions
        {
            Epub2MetadataIgnoreMissingManifestItem = true
        }
    };

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
}
