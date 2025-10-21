using RegexRenamer.Tools.EBookPDFTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Forms
{
    public enum MetadataViewMode
    {
        EditMetadata,
        ShowExitingMetadata
    }

    public class MetadataFormConfig
    {
        public bool IgnoreErrors { get; set; } = false;

        public bool UpdateRecursively { get; set; } = false;

        public string ViewMode { get; set; } = MetadataViewMode.EditMetadata.ToString();

        public string PreferredPDFTool { get; set; } = PDFToolsList.Calibre.ToString();
        public string PreferredEBookTool { get; set; } = EBookToolsList.Calibre.ToString();

        public static MetadataFormConfig Inst { get; private set; } = new MetadataFormConfig();
    }

    public static class MetadataFormHelper
    {

    }
}
