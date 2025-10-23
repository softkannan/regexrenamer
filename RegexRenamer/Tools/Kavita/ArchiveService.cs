using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Kavita.Enum;
using SharpCompress.Archives;
using Kavita.ParserImpl;

namespace Kavita;


#nullable enable

/// <summary>
/// Responsible for manipulating Archive files. Used by <see cref="CacheService"/> and <see cref="ScannerService"/>
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class ArchiveService
{
    private readonly IDirectoryService _directoryService;
    private const string ComicInfoFilename = "ComicInfo.xml";

    public ArchiveService(IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }

    /// <summary>
    /// Checks if a File can be opened. Requires up to 2 opens of the filestream.
    /// </summary>
    /// <param name="archivePath"></param>
    /// <returns></returns>
    public virtual ArchiveLibrary CanOpen(string archivePath)
    {
        if (string.IsNullOrEmpty(archivePath) || !(File.Exists(archivePath) && Parser.IsArchive(archivePath) || Parser.IsEpub(archivePath))) return ArchiveLibrary.NotSupported;

        var ext = Path.GetExtension(archivePath).ToUpper();
        if (ext.Equals(".CBR") || ext.Equals(".RAR")) return ArchiveLibrary.SharpCompress;

        try
        {
            using var a2 = ZipFile.OpenRead(archivePath);
            return ArchiveLibrary.Default;
        }
        catch (Exception)
        {
            try
            {
                using var a1 = ArchiveFactory.Open(archivePath);
                return ArchiveLibrary.SharpCompress;
            }
            catch (Exception)
            {
                return ArchiveLibrary.NotSupported;
            }
        }
    }

    /// <summary>
    /// Test if the archive path exists and an archive
    /// </summary>
    /// <param name="archivePath"></param>
    /// <returns></returns>
    public bool IsValidArchive(string archivePath)
    {
        if (!File.Exists(archivePath))
        {
            return false;
        }

        if (Parser.IsArchive(archivePath)) return true;

        return false;
    }

    private static bool IsComicInfoArchiveEntry(string? fullName, string name)
    {
        if (fullName == null) return false;
        return !Parser.HasBlacklistedFolderInPath(fullName)
               && name.EndsWith(ComicInfoFilename, StringComparison.OrdinalIgnoreCase)
               && !name.StartsWith(Parser.MacOsMetadataFileStartsWith);
    }

    /// <summary>
    /// This can be null if nothing is found or any errors occur during access
    /// </summary>
    /// <param name="archivePath"></param>
    /// <returns></returns>
    public ComicInfo? GetComicInfo(string archivePath)
    {
        if (!IsValidArchive(archivePath)) return null;

        try
        {
            if (!File.Exists(archivePath)) return null;

            var libraryHandler = CanOpen(archivePath);
            switch (libraryHandler)
            {
                case ArchiveLibrary.Default:
                {
                    using var archive = ZipFile.OpenRead(archivePath);

                    var entry = archive.Entries.FirstOrDefault(x => (x.FullName ?? x.Name) == ComicInfoFilename) ??
                        archive.Entries.FirstOrDefault(x => IsComicInfoArchiveEntry(x.FullName, x.Name));
                    if (entry != null)
                    {
                        using var stream = entry.Open();
                        return Deserialize(stream);
                    }

                    break;
                }
                case ArchiveLibrary.SharpCompress:
                {
                    using var archive = ArchiveFactory.Open(archivePath);
                    var entry = archive.Entries.FirstOrDefault(entry => entry.Key == ComicInfoFilename) ??
                        archive.Entries.FirstOrDefault(entry =>
                        IsComicInfoArchiveEntry(Path.GetDirectoryName(entry.Key) ?? string.Empty, entry?.Key ?? string.Empty));

                    if (entry != null)
                    {
                        using var stream = entry.OpenEntryStream();
                        var info = Deserialize(stream);
                        return info;
                    }

                    break;
                }
                case ArchiveLibrary.NotSupported:
                    return null;
                default:
                    return null;
            }
        }
        catch (Exception)
        {
        }

        return null;
    }

    /// <summary>
    /// Strips out empty tags before deserializing
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    private static ComicInfo? Deserialize(Stream stream)
    {
        var comicInfoXml = XDocument.Load(stream);
        comicInfoXml.Descendants()
            .Where(e => e.IsEmpty || string.IsNullOrWhiteSpace(e.Value))
            .Remove();

        var serializer = new XmlSerializer(typeof(ComicInfo));
        using var reader = comicInfoXml.Root?.CreateReader();
        if (reader == null) return null;

        var info  = (ComicInfo?) serializer.Deserialize(reader);
        ComicInfo.CleanComicInfo(info);
        return info;

    }
}
