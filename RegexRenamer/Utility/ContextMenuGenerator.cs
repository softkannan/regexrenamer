using Config;
using ConfigFileParser;
using LogEx;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Utility
{
    public class ContextMenuItem
    {
        public string Name { get; set; }
        public string Command { get; set; }
    }

    public class ContextMenuGenerator
    {

        public List<ContextMenuItem> FileViewMenu { get; set; }
        public List<ContextMenuItem> FolderViewMenu { get; set; }
        

        private Dictionary<string, ContextMenuStrip> _contextMenus = new Dictionary<string, ContextMenuStrip>(StringComparer.OrdinalIgnoreCase);

        public ContextMenuStrip GetContextMenu(string menuName, List<ContextMenuItem> menuItems, Form form, EventHandler handler)
        {
            if (_contextMenus.ContainsKey(menuName))
            {
                return _contextMenus[menuName];
            }

            var result = new ContextMenuStrip();
            foreach (var item in menuItems)
            {
                var menuItem = new ToolStripMenuItem(item.Name);
                menuItem.Click += handler;  
                result.Items.Add(menuItem);
            }
            _contextMenus[menuName] = result;
            return result;
        }

        public ContextMenuGenerator() { }

        private static ContextMenuGenerator _inst = null;
        public static ContextMenuGenerator Inst 
        { 
            get 
            { 
                if (_inst == null)
                {
                    LoadConfigJson();
                }
                return _inst;
            } 
        }

        private static void LoadConfigJson()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "RegexRenamer.Assets.Config.ContextMenuConfig.json";

            // A FileStream is needed to read the XML document.
            using (var fs = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                try
                {
                    var json = fs.ReadToEnd();
                    var settings = new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    ContextMenuGenerator obj = JsonConvert.DeserializeObject<ContextMenuGenerator>(json, settings);
                    _inst = obj;
                }
                catch (Exception ex)
                {
                    ErrorLog.Inst.ShowError($"Unable to load Config Embedded resource : {resourceName} : {ex.Message}");
                }
            }
        }
    }
}
