using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.Calbre;

public class EpubInputFileOptions : CalibreOptions
{
    [Description("Specify the character encoding of the input document. If set this option will override any encoding declared by the document itself. Particularly useful for documents that do not declare an encoding or that have erroneous encoding declarations.")]
    [ArgKey("--input-encoding=")]
    public TxtFileEncoding Encoding { get; set; }
}


public class EpubOutputFileOptions : CalibreOptions
{
   
}