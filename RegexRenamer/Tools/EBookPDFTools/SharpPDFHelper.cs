using Config;
using PdfSharp.Pdf;
using RegexRenamer.Tools.Kavita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.EBookPDFTools;

public static class SharpPDFHelper
{
    public static bool ClearShapPDFMetadata(string filePath)
    {
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

    public static bool WriteSharpPDFMetadata(string filePath, ComicInfo metadata)
    {
        using (var book = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Modify))
        {
            SetPDFMetadata(book, "Title", metadata.Title);
            SetPDFMetadata(book, "TitleSort", string.Empty);
            SetPDFMetadata(book, "Author", metadata.Writer);
            SetPDFMetadata(book, "Series", metadata.Series);
            SetPDFMetadata(book, "Volume", string.Empty);
            book.Save(filePath);
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
