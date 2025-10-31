using Config;
using PdfSharp.Pdf;
using Kavita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.EBookPDFTools;

public static class SharpPDFHelper
{
    public static bool ClearPDFMetadata(string filePath)
    {
        // Open file for modification
        using (var book = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Modify))
        {
            SetPDFMetadata(book, "Summary", string.Empty);
            SetPDFMetadata(book, "Publisher", string.Empty);
            SetPDFMetadata(book, "Author", string.Empty);
            SetPDFMetadata(book, "Title", string.Empty);
            SetPDFMetadata(book, "TitleSort", string.Empty);
            SetPDFMetadata(book, "Subject", string.Empty);
            SetPDFMetadata(book, "Language", string.Empty);
            SetPDFMetadata(book, "UserRating", string.Empty);
            SetPDFMetadata(book, "Series", string.Empty);
            SetPDFMetadata(book, "Volume", string.Empty);
            book.Save(filePath);
            book.Close();
        }
        return true;
    }

    public static bool WritePDFMetadata(string filePath, ComicInfo metadata)
    {
        int writCount = 0;
        using (var book = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Modify))
        {
            if (!string.IsNullOrWhiteSpace(metadata.Title))
            {
                writCount++;
                SetPDFMetadata(book, "Title", metadata.Title);
                SetPDFMetadata(book, "TitleSort", string.Empty);
            }
            if (!string.IsNullOrWhiteSpace(metadata.Writer))
            {
                writCount++;
                SetPDFMetadata(book, "Author", metadata.Writer);
            }
            if (!string.IsNullOrWhiteSpace(metadata.Series))
            {
                writCount++;
                SetPDFMetadata(book, "Series", metadata.Series);
            }
            if (!string.IsNullOrWhiteSpace(metadata.Volume))
            {
                writCount++;
                SetPDFMetadata(book, "Volume", metadata.Volume);
            }
            if(!string.IsNullOrWhiteSpace(metadata.LanguageISO))
            {
                writCount++;
                SetPDFMetadata(book, "Language", metadata.LanguageISO);
            }
            if(writCount > 0)
            {
                book.Save(filePath);
            }
            book.Close();
        }
        return true;
    }
    private static void SetPDFMetadata(PdfDocument pThis, string propertyName, string propertyValue)
    {
        var propertyElementName = $"/{propertyName}";
        if (pThis.Info.Elements.ContainsKey(propertyElementName))
        {
            pThis.Info.Elements.SetValue(propertyElementName, new PdfString(propertyValue));
        }
        else
        {
            pThis.Info.Elements.Add(new KeyValuePair<string, PdfItem>(propertyElementName, new PdfString(propertyValue)));
        }
    }
}
