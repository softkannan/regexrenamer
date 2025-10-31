using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.Calbre;
public class TxtFileInputOptions : CalibreOptions
{
    //[ArgKey("--Test1-type=")]
    //public bool Test1 { get; set; }

    //[ArgKey("--Test2-type=")]
    //public string Test2 { get; set; }

    //[ArgKey("--Test3-type=")]
    //public int Test3 { get; set; }

    //[ArgKey("--Test4-type=")]
    //public float Test4 { get; set; }

    [Description("Formatting used within the document. *auto: Automatically decide which formatting processor to use *plain: No formatting *heuristic: Use heuristics to determine chapter headings, italics, etc. *textile: Use the Textile markup language *markdown: Use the Markdown markup language")]
    [ArgKey("--formatting-type=")]
    public TxtFileFormatType FileFormatingType { get; set; }

    [Description("Specify the character encoding of the input document. If set this option will override any encoding declared by the document itself. Particularly useful for documents that do not declare an encoding or that have erroneous encoding declarations.")]
    [ArgKey("--input-encoding=")]
    public TxtFileEncoding Encoding { get; set; }

    [Description("Paragraph structure to assume. The value of \"off\" is useful for formatted documents such as Markdown or Textile. Choices are: *auto: Try to auto detect paragraph type *block: Treat a blank line as a paragraph break *single: Assume every line is a paragraph *print: Assume every line starting with 2+ spaces or a tab starts a paragraph *unformatted: Most lines have hard line breaks, few/no blank lines or indents *off: Don't modify the paragraph structure")]
    [ArgKey("--paragraph-type=")]    
    public TxtParagraphType ParagraphType { get; set; }

    [Description("Normally extra spaces are condensed into a single space. With this option all spaces will be displayed.")]
    [ArgKey("--preserve-spaces")]
    public TxtPreserveSpaces PreserveSpaces { get; set; }
}


public enum TxtPreserveSpaces
{
    [ArgValue("")]
    none,
    [ArgValue("  ")]
    yes,
}

public enum TxtParagraphType
{
    [ArgValue("")]
    none,
    [ArgValue("auto")]
    auto,
    [ArgValue("block")]
    block,
    [ArgValue("single")]
    single,
    [ArgValue("print")]
    print,
    [ArgValue("unformatted")]
    unformatted,
    [ArgValue("off")]
    off
}


public enum TxtFileFormatType
{
    [ArgValue("")]
    none,
    [ArgValue("auto")]
    auto,
    [ArgValue("plain")]
    plain,
    [ArgValue("heuristic")]
    heuristic,
    [ArgValue("textile")]
    textile,
    [ArgValue("markdown")]
    markdown
}

public enum TxtFileEncoding
{
    [ArgValue("")]
    none,
    [ArgValue("auto")]
    auto,
    [ArgValue("utf-8")]
    utf_8,
    [ArgValue("utf-16")]
    utf_16,
    [ArgValue("")]
    ansi
}
