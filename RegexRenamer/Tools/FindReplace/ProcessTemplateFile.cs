using Newtonsoft.Json;
using LogEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.FindReplace
{
    public class ProcessTemplateFile
    {
        public string Name { get; set; }
        public List<SearchInfo> TextFileProcessing { get; set; }

        public static List<string> GetAllTemplateFiles()
        {
            var retVal = new List<string>();
            string fileLocation = Assembly.GetExecutingAssembly().Location;
            string templateFolder = Path.Combine(fileLocation, "FindReplaceTemplate");
            if (Directory.Exists(templateFolder))
            {
                retVal = Directory.GetFiles(templateFolder, "*.json",SearchOption.TopDirectoryOnly).Select( (f) => Path.GetFileNameWithoutExtension(f)).ToList();
            }
            return retVal;
        }

        private static Dictionary<string, ProcessTemplateFile> _templateCache = new Dictionary<string, ProcessTemplateFile>(StringComparer.OrdinalIgnoreCase);

        public static ProcessTemplateFile GetTemplateByName(string configName)
        {
            if (_templateCache.ContainsKey(configName))
            {
                return _templateCache[configName];
            }

            ProcessTemplateFile retVal = null;
            var assembly = Assembly.GetExecutingAssembly();
            string fileLocation = Assembly.GetExecutingAssembly().Location;
            string filePath = Path.Combine(fileLocation, "FindReplaceTemplate",$"{configName}.json");
            var resourceName = $"RegexRenamer.Assets.FindReplaceTemplate.{configName}.json";

            // A FileStream is needed to read the XML document.
            using (var fs = File.Exists(filePath) ? new StreamReader(filePath) : new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                try
                {
                    var json = fs.ReadToEnd();
                    var settings = new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    retVal = JsonConvert.DeserializeObject<ProcessTemplateFile>(json, settings);
                    _templateCache[configName] = retVal;
                }
                catch (Exception ex)
                {
                    ErrorLog.Inst.ShowError($"Unable to load Config file : {filePath} : {ex.Message}");
                }
            }
            return retVal;
        }
    }
}
