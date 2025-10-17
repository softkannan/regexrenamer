using iText;
using iText.Forms.Fields;
using iText.Forms.Form.Element;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Kernel.XMP;
using RegexRenamer.Tools.Kavita;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace RegexRenamer.Tools.EBookPDFTools
{
    public static class ITextPDFHelper
    {
        public static void RemoveSign(string filePath, string password)
        {
            try
            {
                string destFilePath = filePath;
                if (string.IsNullOrWhiteSpace(password))
                {
                    using (var pdfDocument = new PdfDocument(new PdfReader(filePath), new PdfWriter(destFilePath)))
                    {
                        var form = PdfFormCreator.GetAcroForm(pdfDocument, true);

                        // If no fields have been explicitly included, then all fields are flattened.
                        // Otherwise only the included fields are flattened.
                        form.FlattenFields();
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

                    using (var pdfDocument = new PdfDocument(new PdfReader(filePath, readerProperties), new PdfWriter(destFilePath)))
                    {
                        var form = PdfFormCreator.GetAcroForm(pdfDocument, true);

                        // If no fields have been explicitly included, then all fields are flattened.
                        // Otherwise only the included fields are flattened.
                        form.FlattenFields();
                    }
                }
            }
            catch (Exception ex)
            {
                //item.Status = string.Format("Error: {0}", ex.Message);
            }
        }

        public static void RemovePassword(string filePath, string password)
        {
            try
            {
                string destFilePath = filePath;
                using (var pdfDocument = new PdfDocument(new PdfWriter(destFilePath)))
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
                }
            }
            catch (Exception ex)
            {
                ///item.Status = string.Format("Error: {0}", ex.Message);
            }
        }

        public static bool CleariTextMetadata(string filePath)
        {
            using (PdfReader reader = new PdfReader("HelloWorldNoMetadata.pdf"))
            using (PdfDocument stamper = new PdfDocument(reader, new PdfWriter("HelloWorldStampedMetadata.pdf")))
            {
                //var info = stamper.GetDocumentInfo();
                //info.SetAuthor("Bruno Lowagie");
                //info.SetTitle("Hello World stamped");
                //XMPMeta meta = XMPMetaFactory.Create();
                //stamper.SetXmpMetadata(meta);
            }
            return true;
        }

        public static bool WriteiTextMetadata(string filePath, ComicInfo metadata)
        {
            //String dest = "custom_xmp_metadata.pdf";
            //PdfWriter writer = new PdfWriter(dest);
            //PdfDocument pdfDoc = new PdfDocument(writer);

            //// 1. Create a new XMPMeta object
            //XMPMeta xmpMeta = XMPMetaFactory.create();

            //// 2. Register your custom namespace
            //String customNs = "http://www.yourdomain.com/custom-ns/1.0/";
            //String customPrefix = "yourprefix";
            //XMPMetaFactory.getQNameRegistry().registerNamespace(customNs, customPrefix);

            //try
            //{
            //    // 3. Add the custom property
            //    xmpMeta.setProperty(customNs, "myProperty", "myValue");

            //    // 4. Serialize the XMPMeta object to a byte array
            //    ByteArrayOutputStream baos = new ByteArrayOutputStream();
            //    XMPMetaFactory.serialize(xmpMeta, baos);
            //    byte[] xmpBytes = baos.toByteArray();

            //    // 5. Embed the XMP metadata in the PDF
            //    pdfDoc.setXmpMetadata(xmpBytes);

            //}
            //catch (XMPException e)
            //{
            //    e.printStackTrace();
            //}

            //// Close the document
            //pdfDoc.close();
            //System.out.println("PDF created successfully with custom XMP metadata.");

            //PdfReader reader = new PdfReader("HelloWorldNoMetadata.pdf");
            //PdfDocument stamper = new PdfDocument(reader,
            // new PdfWriter("HelloWorldStampedMetadata.pdf"));
            //var info = stamper.GetDocumentInfo();
            //info.SetAuthor("Bruno Lowagie");
            //info.SetTitle("Hello World stamped");
            //XMPMeta meta = XMPMetaFactory.Create();
            //stamper.SetXmpMetadata(meta);
            return true;
        }

        public static bool RemoveOwnerPassword(string filePath)
        {
            string folderPath = Path.GetDirectoryName(filePath);
            string fileExt = Path.GetExtension(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string srcPath = Path.Combine(folderPath, $"{fileName}.Backup{fileExt}");
            try
            {
                File.Move(filePath, srcPath, true);
            } catch { }


            using (PdfReader reader = new PdfReader(srcPath))
            {
                reader.SetUnethicalReading(true);
                using (PdfDocument pdfDoc = new PdfDocument(reader, new PdfWriter(filePath)))
                {
                    pdfDoc.Close();
                }
                reader.Close();
            }
            return true;
        }
        
    }
}
