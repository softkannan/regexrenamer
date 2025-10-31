using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Config;

public class Tool
{
    public Tool()
    {
        ByPassRegistry = false;
        Arguments = new List<string>();
        Envs = new List<EnviromentVariable>();
        ToolDir = "";
        Path = "";
        Type = ToolType.RegularApp;
        Script = new List<string>();
        Style = new LaunchStyle();
        Warnings = new List<string>();
    }


    [XmlAttribute]
    public ToolType Type { get; set; }

    [XmlAttribute]
    public string Editor { get; set; }

    [XmlAttribute]
    public bool ByPassRegistry { get; set; }

    /// <summary>
    /// If the style is set then the tool will use this value, this takes precedence
    /// </summary>
    public LaunchStyle Style { get; set; }

    public string ToolDir { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }

    [XmlArray("Args")]
    [XmlArrayItem(ElementName = "Arg")]
    public List<string> Arguments { get; set; }

    [XmlArray("Script")]
    [XmlArrayItem(ElementName = "Cmd")]
    public List<string> Script { get; set; }

    [XmlArray("Envs")]
    [XmlArrayItem(ElementName = "Env")]
    public List<EnviromentVariable> Envs { get; set; }

    [XmlArray("Warnings")]
    [XmlArrayItem(ElementName = "Warning")]
    public List<string> Warnings { get; set; }
}
