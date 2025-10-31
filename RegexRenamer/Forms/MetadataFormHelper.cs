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

        public string DefaultMatchPattern { get; set; } = "(.+)";
        public string DefaultSeriesPattern { get; set; } = @"$2";
        public string DefaultVolumePattern { get; set; } = "$#";
        public string DefaultTitlePattern { get; set; } = "$1";
        public string DefaultAuthorPattern { get; set; } = "$2";
        public string DefaultLanguagePattern { get; set; } = "";
        public List<string> PredefMatchPatterns { get; set; } = new List<string>();
        public List<string> PredefSeriesPatterns { get; set; } = new List<string>();
        public List<string> PredefVolumePatterns { get; set; } = new List<string>();
        public List<string> PredefAuthorPatterns { get; set; } = new List<string>();
        public List<string> PredefTitlePatterns { get; set; } = new List<string>();
        public List<string> PredefLanguagePatterns { get; set; } = new List<string>();
    }

    public static class MetadataFormHelper
    {
        

    }
}
