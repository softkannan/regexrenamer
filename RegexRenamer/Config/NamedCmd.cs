using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config;

public class NamedCmd
{
    public NamedCmd()
    {
        Args = new List<string>();
        Name = string.Empty;
    }
    public string Name { get; set; }
    public string ToolName { get; set; }
    public List<string> Args { get; set; }
}
