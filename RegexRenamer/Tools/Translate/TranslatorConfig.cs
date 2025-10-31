using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.Translate
{
    public class TranslatorProvider
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public enum WebEngineType
    {
        WebView2,
        PuppeteerSharp
    }

    public class TranslatorConfig
    {
        public string From { get; set; } = "en";
        public string To { get; set; } = "ta";

        public WebEngineType WebEngine { get; set; } = WebEngineType.WebView2;

        public List<TranslatorProvider> ServiceProviders { get; set; } = new List<TranslatorProvider>();


        [JsonIgnore]
        public int SelectedProviderIndex { get; set; } = 0;
    }
}
