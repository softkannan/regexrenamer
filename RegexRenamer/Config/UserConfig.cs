using Newtonsoft.Json;
using RegexRenamer.Tools.FindReplace;
using LogEx;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using RegexRenamer.Tools.EBookPDFTools;
using RegexRenamer.Forms;
using RegexRenamer.Tools.Translate;

namespace Config;

public class UserConfig
{
    public UserConfig()
    {
        string exePath = Assembly.GetExecutingAssembly().Location;
        ExeDir = Path.GetDirectoryName(exePath);
        Environment.SetEnvironmentVariable("APP_DIR", ExeDir, EnvironmentVariableTarget.Process);
        Environment.CurrentDirectory = ExeDir;

        KnownCmds = new List<NamedCmd>();
        Tools = new List<Tool>();
        Envs = new List<EnviromentVariable>();
    }

    #region Config File Public Entires

    [XmlArray("Envs")]
    [XmlArrayItem(ElementName = "Env")]
    public List<EnviromentVariable> Envs { get; set; }
    public List<Tool> Tools { get; set; }

    public List<NamedCmd> KnownCmds { get; set; }

    public string DefaultWorkspace { get; set; }
    public string DefaultOutputPath { get; set; }

    public MetadataFormConfig Meta { get; set; } = new MetadataFormConfig();
   
    public TranslatorConfig Translator { get; set; } = new TranslatorConfig();
    #endregion


    #region Config readonly properties

    [XmlIgnore]
    public List<Regex> IgnoreFilesFilterRegEx { get; private set; }

    [XmlIgnore]
    public List<string> IgnoreFilesFilterEx { get; private set; }

    [XmlIgnore]
    public string ExeDir { get; private set; }


    [XmlIgnore]
    public List<Tool> AvailableTools { get; private set; }
   

    [XmlIgnore]
    public bool IsElevated
    {
        get
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

    [XmlIgnore]
    public bool Is64Bit
    {
        get
        {
            return IntPtr.Size == 8;
        }
    }

    [XmlIgnore]
    public string CurrentTimeStamp
    {
        get
        {
            var currTime = DateTime.Now;
            return string.Format("{0:00}_{1:00}_{2:0000}_{3:00}_{4:00}_{5:00}_{6:00}", currTime.Day, currTime.Month, currTime.Year, currTime.Hour, currTime.Minute, currTime.Second, currTime.Millisecond);
        }
    }

    [XmlIgnore]
    public static string ConfigFile
    {
        get
        {
            string fileLocation = Assembly.GetExecutingAssembly().Location;
            string retVal = string.Format("{0}\\Config\\UserConfig.json", Path.GetDirectoryName(fileLocation));
            return retVal;
        }
    }

    #endregion


    #region Methods
    public void Process()
    {
        IgnoreFilesFilterRegEx = new List<Regex>();
        IgnoreFilesFilterEx = new List<string>();

        //AutoDetectToolPath();
        if (Envs != null)
        {
            foreach (var item in Envs)
            {
                string currentValue = Environment.GetEnvironmentVariable(item.Name);
                if (!string.IsNullOrWhiteSpace(currentValue))
                {
                    switch (item.Action)
                    {
                        case EnvironmentAction.Overwrite:
                            {
                                string newValue = ResolveValue.Inst.ResolveEnvironmentValue(item.Type, item.Value);
                                Environment.SetEnvironmentVariable(item.Name, newValue, EnvironmentVariableTarget.Process);
                            }
                            break;
                        case EnvironmentAction.Prefix:
                            {
                                string newValue = ResolveValue.Inst.ResolveEnvironmentValue(item.Type, item.Value) + ";" + currentValue;
                                Environment.SetEnvironmentVariable(item.Name, newValue, EnvironmentVariableTarget.Process);
                            }
                            break;
                        case EnvironmentAction.Append:
                            {
                                string newValue = currentValue + ";" + ResolveValue.Inst.ResolveEnvironmentValue(item.Type, item.Value);
                                Environment.SetEnvironmentVariable(item.Name, newValue, EnvironmentVariableTarget.Process);
                            }
                            break;
                    }
                }
                else
                {
                    string newValue = ResolveValue.Inst.ResolveEnvironmentValue(item.Type, item.Value);
                    Environment.SetEnvironmentVariable(item.Name, newValue, EnvironmentVariableTarget.Process);
                }
            }
        }

        if (AvailableTools == null)
        {
            AvailableTools = new List<Tool>();
        }

        foreach (var item in Tools)
        {
            string newValue = ResolveValue.Inst.ResolveFullPath(item.Path);
            item.Path = newValue;

            newValue = ResolveValue.Inst.ResolveFullPath(item.ToolDir);
            item.ToolDir = newValue;

            Tool addTool = null;

            if (item.Type == ToolType.RegularApp)
            {
                if (File.Exists(item.Path))
                {
                    addTool = item;
                }
            }
            else
            {
                addTool = item;
            }

            if (addTool != null)
            {
                //if editor is not explicitly defined then assume tool name and editor name is same
                if (string.IsNullOrWhiteSpace(addTool.Editor))
                {
                    addTool.Editor = addTool.Name;
                }
                AvailableTools.Add(addTool);
            }
        }

        DefaultWorkspace = ResolveValue.Inst.ResolveFullPath(DefaultWorkspace);
        DefaultOutputPath = ResolveValue.Inst.ResolveFullPath(DefaultOutputPath);
    }

    public static void LoadConfig()
    {
        LoadConfigJson();
    }

    private static void LoadConfigJson()
    {
        string filePath = ConfigFile;

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "RegexRenamer.Assets.Config.UserConfig.json";

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

                UserConfig obj = JsonConvert.DeserializeObject<UserConfig>(json, settings);
                _inst = obj;
            }
            catch (Exception ex)
            {
                ErrorLog.Inst.ShowError($"Unable to load Config file : {filePath} : {ex.Message}");
            }
        }
#if !DEBUG
        if (!File.Exists(filePath))
        {
            ErrorLog.Inst.ShowInfo($"Unable to find `{Path.GetFileName(filePath)}` Config file : {filePath}");
        }
#endif
    }
    private static void LoadConfigXml()
    {
        string filePath = ConfigFile;

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "ClearcaseActivityManager.Assets.Config.UserConfig.xml";

        XmlSerializer serializer = new XmlSerializer(typeof(UserConfig));
        // A FileStream is needed to read the XML document.
        using (var fs = File.Exists(filePath) ? new StreamReader(filePath) : new StreamReader(assembly.GetManifestResourceStream(resourceName)))
        {
            try
            {
                _inst = (UserConfig)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                ErrorLog.Inst.ShowError($"Unable to load Config file : {filePath} : {ex.Message}");
            }
        }
#if !DEBUG
        if (!File.Exists(filePath))
        {
            ErrorLog.Inst.ShowInfo($"Unable to find `{Path.GetFileName(filePath)}` Config file : {filePath}");
        }
#endif
        //if (_inst != null)
        //{
        //    _inst.ProcessRuntimeInfo();
        //}
    }

    public static void SaveConfig()
    {
        string filePath = ConfigFile;
        if (File.Exists(filePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserConfig));
            // A FileStream is needed to read the XML document.
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                serializer.Serialize(fs, _inst);
            }
        }
    }
    static UserConfig _inst = new UserConfig();
    public static UserConfig Inst
    {
        get
        {
            return _inst;
        }
    }

    #endregion
}
