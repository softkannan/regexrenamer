using Config;
using EpubSharp;
using PdfSharp.Pdf;
using Kavita;
using RegexRenamer.Tools.Calbre;
using RegexRenamer.Utility;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Kavita.ParserImpl;

namespace RegexRenamer.Tools.EBookPDFTools;


public enum PDFToolsList
{
    Calibre,
    SharpPDF,
    iTextSharp
}

public enum EBookToolsList
{
    Calibre,
    EpubMeta,
    // Epub sharp not fully working
    //EpubSharp
}

/// <summary>
/// Helper class for common ebook operations such as clearing/writing metadata,
/// polishing, launching, and editing ebooks. Supports EPUB and PDF formats,
/// delegating to Calibre or PDF-specific helpers as appropriate.
/// </summary>
public static class EBookHelper
{
    /// <summary>
    /// File extensions supported for reading/launching ebooks.
    /// </summary>
    private static string[] supportedEPubReadFileFormats = new string[] { "azw", "cbc", "cbr", "cbz", "epub", "fb2", "htm", "html", "lit", "lrf",
        "mobi", "odt", "opf", "rb", "pdb", "pdf", "pml", "pmlz", "prc", "recipe", "rtf", "shtm", "shtml", "tcr", "txt", "xhtm", "xhtml"
    };

    /// <summary>
    /// File extensions supported for editing/polishing ebooks.
    /// </summary>
    private static string[] supportedEpubEditFileFormats = new string[] { "azw", "epub", "kepub" };

    /// <summary>
    /// Clears all metadata from the specified ebook file.
    /// Uses Calibre for EPUBs and, depending on configuration, for PDFs.
    /// Otherwise, uses a PDF-specific helper for PDFs.
    /// </summary>
    /// <param name="filePath">Path to the ebook file.</param>
    /// <param name="toolName">If true, forces use of the PDF-specific method for PDFs.</param>
    /// <returns>True if operation succeeded.</returns>
    public static async Task<bool> ClearMetadata(string filePath, string toolNameStr)
    {
        if (Parser.IsEpub(filePath))
        {
            var toolName = Enum.Parse<EBookToolsList>(toolNameStr);
            switch(toolName)
            {
                case EBookToolsList.EpubMeta:
                    EpubMetaHelper.ClearEpubMetadata(filePath);
                    break;
                //case EBookToolsList.EpubSharp:
                //    EpubSharpHelper.ClearEpubMetadata(filePath);
                //    break;
                case EBookToolsList.Calibre:
                default:
                    await CalibreHelper.ClearMetadata(filePath);
                    break;
            }

        }
        else if (Parser.IsPdf(filePath))
        {
            var toolName = Enum.Parse<PDFToolsList>(toolNameStr);
            switch (toolName)
            {
                case PDFToolsList.SharpPDF:
                    SharpPDFHelper.ClearPDFMetadata(filePath);
                    break;
                case PDFToolsList.iTextSharp:
                    ITextPDFHelper.ClearPDFMetadata(filePath);
                    break;
                case PDFToolsList.Calibre:

                default:
                    await CalibreHelper.ClearMetadata(filePath);
                    break;
            }
        }
        return true;
    }
    
    /// <summary>
    /// Writes metadata to the specified ebook file.
    /// Uses Calibre for EPUBs and, depending on configuration, for PDFs.
    /// Otherwise, uses a PDF-specific helper for PDFs.
    /// </summary>
    /// <param name="filePath">Path to the ebook file.</param>
    /// <param name="metadata">ComicInfo metadata to write.</param>
    /// <returns>True if operation succeeded.</returns>
    public static async Task<bool> WriteMetadata(string filePath, ComicInfo metadata, string toolNameStr)
    {
        if (Parser.IsEpub(filePath))
        {
            var toolName = Enum.Parse<EBookToolsList>(toolNameStr);
            switch(toolName)
            {
                case EBookToolsList.EpubMeta:
                    EpubMetaHelper.WriteEpubMetadata(filePath, metadata);
                    break;
                //case EBookToolsList.EpubSharp:
                //    EpubSharpHelper.WriteEpubMetadata(filePath, metadata);
                //    break;
                case EBookToolsList.Calibre:
                default:
                    // Ensure volume is set if series is present (Calibre expects a value)
                    if (!string.IsNullOrEmpty(metadata.Series) && string.IsNullOrEmpty(metadata.Volume))
                    {
                        metadata.Volume = "0";
                    }
                    await CalibreHelper.WriteMetadata(filePath, metadata);
                    break;
            }
        }
        else if (Parser.IsPdf(filePath))
        {
            var toolName = Enum.Parse<PDFToolsList>(toolNameStr);
            switch(toolName)
            {
                case PDFToolsList.SharpPDF:
                    SharpPDFHelper.WritePDFMetadata(filePath, metadata);
                    break;
                case PDFToolsList.iTextSharp:
                    ITextPDFHelper.WritePDFMetadata(filePath, metadata);
                    break;
                case PDFToolsList.Calibre:
                default:
                    // Ensure volume is set if series is present (Calibre expects a value)
                    if (!string.IsNullOrEmpty(metadata.Series) && string.IsNullOrEmpty(metadata.Volume))
                    {
                        metadata.Volume = "0";
                    }
                    await CalibreHelper.WriteMetadata(filePath, metadata);
                    break;
            }
        }
        return true;
    }

    /// <summary>
    /// Polishes (optimizes and cleans) an ebook file using Calibre.
    /// Only supported for certain file types (e.g., EPUB, AZW, KEPUB).
    /// </summary>
    /// <param name="filePath">Path to the ebook file.</param>
    /// <returns>True if operation succeeded, false if file type is unsupported.</returns>
    public static async Task<bool> PolishEbook(string filePath)
    {
        var fileExt = Path.GetExtension(filePath).ToLowerInvariant().TrimStart('.');
        // check file is with ebook extension
        if (!supportedEpubEditFileFormats.Contains(fileExt))
        {
            return false;
        }
        if (Parser.IsEpub(filePath))
        {
            return await CalibreHelper.PolishEbook(filePath);
        }
        return false;
    }
    
    /// <summary>
    /// Launches the specified ebook file in the default reader.
    /// Supports a wide range of ebook file types.
    /// </summary>
    /// <param name="filePath">Path to the ebook file.</param>
    /// <returns>True if operation succeeded, false if file type is unsupported.</returns>
    public static async Task<bool> LaunchEBookAsync(string filePath)
    {
        var fileExt = Path.GetExtension(filePath).ToLowerInvariant().TrimStart('.');
        // check file is with ebook extension
        if (!supportedEPubReadFileFormats.Contains(fileExt))
        {
            return false;
        }
        var cmdName = "LaunchEbook";
        if (Parser.IsPdf(filePath))
        {
            cmdName = "LaunchPDF";
        }
        await cmdName.ExecNamedCmdAsync(Path.GetDirectoryName(filePath), new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("{filepath}", filePath)
                    });
        return true;
    }

    /// <summary>
    /// Opens the specified ebook file in an editor.
    /// Only supported for certain file types (e.g., EPUB, AZW, KEPUB).
    /// </summary>
    /// <param name="filePath">Path to the ebook file.</param>
    /// <returns>True if operation succeeded, false if file type is unsupported.</returns>
    public static async Task<bool> EditEBookAsync(string filePath)
    {
        var fileExt = Path.GetExtension(filePath).ToLowerInvariant().TrimStart('.');
        // check file is with ebook extension
        if (!supportedEpubEditFileFormats.Contains(fileExt))
        {
            return false;
        }
        var cmdName = "EditEbook";
        await cmdName.ExecNamedCmdAsync(Path.GetDirectoryName(filePath), new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("{filepath}", filePath)
                    });
        return true;
    }

    public static string GetSysTempFilePath(this string originalFilePath)
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "RegexRenamer");
        if (!Directory.Exists(tempDir))
        {
            Directory.CreateDirectory(tempDir);
        }
        var tempFilePath = Path.Combine(tempDir, Path.GetFileName(originalFilePath));
        return tempFilePath;
    }

    public static string GetInFolderTempFilePath(this string originalFilePath)
    {
        var tempFilePath = Path.Combine(Path.GetDirectoryName(originalFilePath), $"{Path.GetFileNameWithoutExtension(originalFilePath)}.Temp{Path.GetExtension(originalFilePath)}");

        return tempFilePath;
    }

    public static string MakeBackup(this string originalFilePath)
    {
        var backupFile = Path.Combine(Path.GetDirectoryName(originalFilePath), $"{Path.GetFileNameWithoutExtension(originalFilePath)}.Backup{Path.GetExtension(originalFilePath)}");
        
        File.Move(originalFilePath, backupFile, true);

        return backupFile;
    }
}
