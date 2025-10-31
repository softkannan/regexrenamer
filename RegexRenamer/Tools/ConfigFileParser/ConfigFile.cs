using ConfigFileParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConfigFileParser
{
    public class ConfigFile
    {
        public string FilePath { get; private set; }
        public string Folder { get; private set; }

        public ConfigData Data { get; private set; }


        private void Init(string filePath)
        {
            FilePath = filePath;
            Folder = System.IO.Path.GetDirectoryName(filePath) ?? string.Empty;
            if (File.Exists(FilePath))
            {
                var parser = new ConfigFileParser.FileIniDataParser();
                Data = parser.ReadFile(filePath);
            }
            else
            {
                Data = new ConfigData();
            }
        }

        private bool CanWriteToFolder(string folderPath)
        {
            try
            {
                string testFilePath = Path.Combine(folderPath, "test.tmp");
                using (var fs = File.Create(testFilePath))
                {
                }
                File.Delete(testFilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string _configFolder = null;


        public ConfigFile()
        {
            var assembly = Assembly.GetEntryAssembly();
            var appName = assembly.GetName().Name;
            
            do
            {
                if (_configFolder == null)
                {

#if !DEBUG
                    var appConfigFolder = Path.Combine(Path.GetDirectoryName(assembly.Location), "Config");
                    if (CanWriteToFolder(appConfigFolder))
                    {
                        _configFolder = appConfigFolder;
                        break;
                    }
#endif
                    var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    if (CanWriteToFolder(appDataFolder))
                    {
                        _configFolder = appDataFolder;
                        break;
                    }
                    throw new InvalidOperationException("Cannot write to application config folder or user AppData folder.");
                }
            } while (false);

            string appConfigFilePath = Path.Combine(_configFolder, appName, $"{appName}.config");
            Init(appConfigFilePath);
        }


        private ConfigFile(string filePath)
        {
            Init(filePath);
        }

        public void Save()
        {
            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }
            var parser = new ConfigFileParser.FileIniDataParser();
            parser.WriteFile(FilePath, Data);
        }
    }
}
