using iText;
using iText.Forms.Fields;
using iText.Forms.Form.Element;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Kernel.XMP;
using iText.Kernel.XMP.Options;
using Kavita;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace RegexRenamer.Tools.EBookPDFTools
{
    /*
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

     */
    public static class ITextPDFHelper
    {
        public static bool ClearPDFMetadata(string filePath)
        {
            string tempFilePath = filePath.GetInFolderTempFilePath(); 
            using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(filePath), new PdfWriter(tempFilePath)))
            {
                var info = pdfDoc.GetDocumentInfo();
                if (info != null)
                {
                    info.SetAuthor("");
                    info.SetTitle("");
                    info.SetMoreInfo("TitleSort", "");
                    info.SetMoreInfo("Series", "");
                    info.SetMoreInfo("Volume", "");
                }

                // cleat series metadata if exists
                var xmpMeta = pdfDoc.GetXmpMetadata();
                if (xmpMeta != null)
                {
                    var dcNS = "http://purl.org/dc/elements/1.1/";
                    var calibreNS = "http://calibre-ebook.com/xmp-namespace";
                    var calibreSINS = "http://calibre-ebook.com/xmp-namespace-series-index";
                    XMPMetaFactory.GetSchemaRegistry().RegisterNamespace(dcNS,"dc");
                    XMPMetaFactory.GetSchemaRegistry().RegisterNamespace(calibreNS, "calibre");
                    XMPMetaFactory.GetSchemaRegistry().RegisterNamespace(calibreSINS, "calibreSI");

                    xmpMeta.SetProperty(dcNS, "dc:title", "");
                    xmpMeta.SetProperty(calibreNS, "calibre:series", "");
                    xmpMeta.SetProperty(calibreSINS, "calibreSI:series_index", "");
                    pdfDoc.SetXmpMetadata(xmpMeta);
                }
            }

            // if success move temp file to original file
            File.Move(tempFilePath, filePath, true);

            return true;
        }

        public static bool WritePDFMetadata(string filePath, ComicInfo metadata)
        {
            string tempFilePath = filePath.GetInFolderTempFilePath();
            int writeCount = 0;
            int xmpWriteCount = 0;

            using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(filePath), new PdfWriter(tempFilePath)))
            {
                var dcNS = "http://purl.org/dc/elements/1.1/";
                var calibreNS = "http://calibre-ebook.com/xmp-namespace";
                var calibreSINS = "http://calibre-ebook.com/xmp-namespace-series-index";
                var dcNSPrefix = XMPMetaFactory.GetSchemaRegistry().RegisterNamespace(dcNS, "dc");
                var calibreNSPrefix = XMPMetaFactory.GetSchemaRegistry().RegisterNamespace(calibreNS, "calibre");
                var calibreSINSPrefix = XMPMetaFactory.GetSchemaRegistry().RegisterNamespace(calibreSINS, "calibreSI");

                // cleat series metadata if exists
                var xmpMeta = pdfDoc.GetXmpMetadata(true);
                if (xmpMeta != null)
                {
                    if (!string.IsNullOrWhiteSpace(metadata.Series))
                    {
                        xmpWriteCount++;
                        xmpMeta.SetProperty(calibreNS, $"{calibreNSPrefix}series", metadata.Series);
                    }

                    if (!string.IsNullOrWhiteSpace(metadata.Volume))
                    {
                        xmpWriteCount++;
                        xmpMeta.SetProperty(calibreSINS, $"{calibreSINSPrefix}series_index", metadata.Volume);
                    }
                }

                var info = pdfDoc.GetDocumentInfo();
                if (info != null)
                {
                    if (!string.IsNullOrWhiteSpace(metadata.Writer))
                    {
                        writeCount++;
                        info.SetAuthor(metadata.Writer);
                        info.SetCreator(metadata.Writer);
                        info.SetProducer(metadata.Writer);
                    }

                    if (!string.IsNullOrWhiteSpace(metadata.Title))
                    {
                        writeCount++;
                        info.SetTitle(metadata.Title);
                        info.SetMoreInfo("TitleSort", metadata.Title);
                    }

                    if (!string.IsNullOrWhiteSpace(metadata.Series))
                    {
                        writeCount++;
                        info.SetMoreInfo("Series", metadata.Series);
                    }

                    if (!string.IsNullOrWhiteSpace(metadata.Volume))
                    {
                        writeCount++;
                        info.SetMoreInfo("Volume", metadata.Volume);
                    }

                }

                if(!string.IsNullOrWhiteSpace(metadata.LanguageISO))
                {
                    writeCount++;
                    pdfDoc.GetCatalog().SetLang(new PdfString(metadata.LanguageISO.ToUpper()));
                }

                //if (xmpWriteCount > 0)
                //{
                //    pdfDoc.SetXmpMetadata(xmpMeta);
                //}
            }

            writeCount += xmpWriteCount;

            // if success move temp file to original file
            if (writeCount > 0)
            {
                File.Move(tempFilePath, filePath, true);
            }
            else
            {
                // delete the temp file as no changes were made
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            return true;
        }

        public static bool RemoveOwnerPassword(string filePath)
        {
            string tempFilePath = filePath.GetInFolderTempFilePath();

            using (PdfReader reader = new PdfReader(filePath))
            {
                reader.SetUnethicalReading(true);
                using (PdfDocument pdfDoc = new PdfDocument(reader, new PdfWriter(tempFilePath)))
                {
                    pdfDoc.Close();
                }
                reader.Close();
            }

            // if success then make backup of original file
            filePath.MakeBackup();

            // if success move temp file to original file
            File.Move(tempFilePath, filePath, true);

            return true;
        }

        public static void RemoveSign(string filePath, string password)
        {
            string tempFilePath = filePath.GetInFolderTempFilePath();

            if (string.IsNullOrWhiteSpace(password))
            {
                using (PdfReader reader = new PdfReader(filePath))
                {
                    reader.SetUnethicalReading(true);
                    using (var pdfDoc = new PdfDocument(reader, new PdfWriter(tempFilePath)))
                    {
                        var form = PdfFormCreator.GetAcroForm(pdfDoc, true);
                        // If no fields have been explicitly included, then all fields are flattened.
                        // Otherwise only the included fields are flattened.
                        form.FlattenFields();
                        pdfDoc.Close();
                    }
                    reader.Close();
                }
            }
            else
            {
                // Convert the string into a byte array.
                byte[] unicodeBytes = Encoding.Unicode.GetBytes(password);

                // Perform the conversion from one encoding to the other.
                byte[] asciiBytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, unicodeBytes);
                var readerProperties = new ReaderProperties();
                readerProperties.SetPassword(asciiBytes);

                using (var pdfDocument = new PdfDocument(new PdfReader(filePath, readerProperties), new PdfWriter(tempFilePath)))
                {
                    var form = PdfFormCreator.GetAcroForm(pdfDocument, true);

                    // If no fields have been explicitly included, then all fields are flattened.
                    // Otherwise only the included fields are flattened.
                    form.FlattenFields();
                }
            }

            // if success then make backup of original file
            filePath.MakeBackup();

            // if success move temp file to original file
            File.Move(tempFilePath, filePath, true);
        }

        public static bool ExtractText(string filePath, string extractedFilePath)
        {
            string PageBreakChar = "\f";
            string extractedText = string.Empty;
            using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(filePath)))
            {
                StringBuilder text = new StringBuilder();
                int noOfPages = pdfDoc.GetNumberOfPages();
                for (int pageNum = 1; pageNum <= noOfPages; pageNum++)
                {
                    var page = pdfDoc.GetPage(pageNum);
                    var strategy = new iText.Kernel.Pdf.Canvas.Parser.Listener.SimpleTextExtractionStrategy();
                    var pageContent = iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(page, strategy);
                    text.AppendLine(pageContent);
                    text.AppendLine();
                    //Append a page break character
                    text.AppendLine(PageBreakChar);
                }
                extractedText = text.ToString();
            }

            // Delete existing target file if any
            if (File.Exists(extractedFilePath))
            {
                try
                {
                    File.Delete(extractedFilePath);
                }
                catch
                {
                }
            }

            // Write extracted text to file
            File.WriteAllText(extractedFilePath, extractedText, Encoding.UTF8);

            return true;
        }

        public static void RemovePassword(string filePath, string password)
        {
            string tempFilePath = filePath.GetInFolderTempFilePath();

            using (var pdfDocument = new PdfDocument(new PdfWriter(tempFilePath)))
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    using (var inputPdf = new PdfDocument(new PdfReader(filePath)))
                    {

                        int noOfPages = inputPdf.GetNumberOfPages();
                        for (int pageNum = 1; pageNum <= noOfPages; pageNum++)
                        {
                            //var pageSize = inputPdf.GetPageSize(pageNum);
                            //pdfDocument.SetPageSize(pageSize);
                            //var page = pdfDocument.GetImportedPage(inputPdf, pageNum);
                            //pdfDocument.SetPageSize(new iTextSharp.text.Rectangle(page.Width, page.Height));
                            //pdfDocument.NewPage();
                            //pdfContentBytes.AddTemplate(page, 0, 0);
                        }
                        pdfDocument.Close();
                    }
                }
                else
                {
                    // Convert the string into a byte array.
                    byte[] unicodeBytes = Encoding.Unicode.GetBytes(password);

                    // Perform the conversion from one encoding to the other.
                    byte[] asciiBytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, unicodeBytes);
                    var readerProperties = new ReaderProperties();
                    readerProperties.SetPassword(asciiBytes);
                    using (var inputPdf = new PdfDocument(new PdfReader(filePath, readerProperties)))
                    {

                        int noOfPages = inputPdf.GetNumberOfPages();
                        for (int pageNum = 1; pageNum <= noOfPages; pageNum++)
                        {
                            //var pageSize = inputPdf.GetPageSize(pageNum);
                            //pdfDocument.SetPageSize(pageSize);
                            //var page = writer.GetImportedPage(inputPdf, pageNum);
                            //pdfDocument.SetPageSize(new iTextSharp.text.Rectangle(page.Width, page.Height));
                            //pdfDocument.NewPage();
                            //pdfContentBytes.AddTemplate(page, 0, 0);
                        }
                        pdfDocument.Close();
                    }
                }

                // if success then make backup of original file
                filePath.MakeBackup();

                // if success move temp file to original file
                File.Move(tempFilePath, filePath, true);
            }
        }

    }
}
