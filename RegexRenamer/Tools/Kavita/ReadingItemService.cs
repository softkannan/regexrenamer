using System;
using Microsoft.Extensions.Logging;
using Kavita.Enum;
using Kavita.ParserImpl;

namespace Kavita;
#nullable enable

public class ReadingItemService 
{
    private readonly IDirectoryService _directoryService;
    private readonly BasicParser _basicParser;
    private readonly ComicVineParser _comicVineParser;
    private readonly ImageParser _imageParser;
    private readonly BookParser _bookParser;
    private readonly PdfParser _pdfParser;
    private readonly ArchiveService _archiveService;
    private readonly BookService _bookService;
    public ReadingItemService(IDirectoryService directoryService)
    {
        _directoryService = directoryService;

        _archiveService = new ArchiveService(directoryService);
        _bookService = new BookService(directoryService);
        _imageParser = new ImageParser(directoryService);
        _basicParser = new BasicParser(directoryService, _imageParser);
        _bookParser = new BookParser(directoryService, _basicParser);
        _comicVineParser = new ComicVineParser(directoryService);
        _pdfParser = new PdfParser(directoryService);

    }

    /// <summary>
    /// Gets the ComicInfo for the file if it exists. Null otherwise.
    /// </summary>
    /// <param name="filePath">Fully qualified path of file</param>
    /// <param name="enableMetadata">If false, returns null</param>
    /// <returns></returns>
    public ComicInfo? GetComicInfo(string filePath, bool enableMetadata)
    {
        if (!enableMetadata) return null;

        if (Parser.IsEpub(filePath) || Parser.IsPdf(filePath))
        {
            return _bookService.GetComicInfo(filePath);
        }
        else if (Parser.IsComicInfoExtension(filePath))
        {
            return _archiveService.GetComicInfo(filePath);
        }

        return null;
    }

    /// <summary>
    /// Processes files found during a library scan.
    /// </summary>
    /// <param name="path">Path of a file</param>
    /// <param name="rootPath"></param>
    /// <param name="type">Library type to determine parsing to perform</param>
    /// <param name="enableMetadata">Enable Metadata parsing overriding filename parsing</param>
    public ParserInfo? ParseFile(string path, string rootPath, string libraryRoot, LibraryType type, bool enableMetadata)
    {
        try
        {
            var info = Parse(path, rootPath, libraryRoot, type, enableMetadata);
            if (info == null)
            {
                return null;
            }

            return info;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Parses information out of a file. If file is a book (epub), it will use book metadata regardless of LibraryType
    /// </summary>
    /// <param name="path"></param>
    /// <param name="rootPath"></param>
    /// <param name="type"></param>
    /// <param name="enableMetadata"></param>
    /// <returns></returns>
    private ParserInfo? Parse(string path, string rootPath, string libraryRoot, LibraryType type, bool enableMetadata)
    {
        if (_comicVineParser.IsApplicable(path, type))
        {
            return _comicVineParser.Parse(path, rootPath, libraryRoot, type, enableMetadata, GetComicInfo(path, enableMetadata));
        }
        if (_imageParser.IsApplicable(path, type))
        {
            return _imageParser.Parse(path, rootPath, libraryRoot, type, enableMetadata, GetComicInfo(path, enableMetadata));
        }
        if (_bookParser.IsApplicable(path, type))
        {
            return _bookParser.Parse(path, rootPath, libraryRoot, type, enableMetadata, GetComicInfo(path, enableMetadata));
        }
        if (_pdfParser.IsApplicable(path, type))
        {
            return _pdfParser.Parse(path, rootPath, libraryRoot, type, enableMetadata, GetComicInfo(path, enableMetadata));
        }
        if (_basicParser.IsApplicable(path, type))
        {
            return _basicParser.Parse(path, rootPath, libraryRoot, type, enableMetadata, GetComicInfo(path, enableMetadata));
        }

        return null;
    }
}
