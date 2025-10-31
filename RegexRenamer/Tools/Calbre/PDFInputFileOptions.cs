using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.Calbre;

public class PDFInputFileOptions : CalibreOptions
{
    [Description("Specify the character encoding of the input document. If set this option will override any encoding declared by the document itself. Particularly useful for documents that do not declare an encoding or that have erroneous encoding declarations.")]
    [ArgKey("--input-encoding=")]
    public TxtFileEncoding Encoding { get; set; }

    //
    [Description("Do not extract images from the document")]
    [ArgKey("--no-images")]
    public PDFFileNoImages NoImages { get; set; }

    [Description("The PDF engine to use, the \"calibre\" engine is recommended as it has automatic header and footer removal. Choices: calibre, pdftohtml")]
    [ArgKey("--pdf-engine=")]
    public PDFFilePdfEngines PdfEngine { get; set; }
   
}

public enum PDFFileNoImages
{
    [ArgValue("")]
    none,
    [ArgValue("  ")]
    yes
}

public enum PDFFilePdfEngines
{
    [ArgValue("")]
    none,
    [ArgValue("calibre")]
    calibre,
    [ArgValue("pdftohtml")]
    pdftohtml
}


public class PDFOutputFileOptions : CalibreOptions
{
    [Description("The size of the paper. This size will be overridden when a non default output profile is used. Default is letter. Choices are a0, a1, a2, a3, a4, a5, a6, b0, b1, b2, b3, b4, b5, b6, legal, letter")]
    [ArgKey("--paper-size=")]
    public TxtFileEncoding PaperSize { get; set; }

    [Description("The size of the bottom page margin, in pts. Default is *72pt. Overrides the common bottom page margin setting, unless set to zero.")]
    [ArgKey("--pdf-page-margin-bottom=")]
    public string PdfPageMarginBottom { get; set; } 

    [Description("The size of the left page margin, in pts. Default is *72pt. Overrides the common left page margin setting.")]
    [ArgKey("--pdf-page-margin-left=")]
    public string PdfPageMarginLeft { get; set; }

    [Description("The size of the right page margin, in pts. Default is *72pt. Overrides the common right page margin setting, unless set to zero.")]
    [ArgKey("--pdf-page-margin-right=")]
    public string PdfPageMarginRight { get; set; }

    [Description("The size of the top page margin, in pts. Default is *72pt. Overrides the common top page margin setting, unless set to zero.")]
    [ArgKey("--pdf-page-margin-top=")]
    public string PdfPageMarginTop { get; set; }

}