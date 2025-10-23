using ConfigFileParser;
using Microsoft.Win32;
using RegexRenamer.Controls.ListViewExCtrl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Tools.FindReplace
{
    public static class FindReplaceHelpers
    {
        private const int MAX_HISTORY = 20;
        public static void SaveToConfigFile(this ComboBox pThis, string appName = "", string keyName = "")
        {
#if !DEBUG
      try
      {
#endif
            if (string.IsNullOrEmpty(appName))
            {
                appName = Assembly.GetEntryAssembly().GetName().Name;
            }
            if (string.IsNullOrEmpty(keyName))
            {
                keyName = pThis.Name;
            }
            var configFile = new ConfigFile();
            var key = configFile.Data[keyName];
            {
                if (key == null) return;

                key.RemoveAllKeys();

                for (int i = 0; i < pThis.Items.Count; i++)
                {
                    if(i < MAX_HISTORY)
                        key.SetValue(i.ToString("00"), pThis.Items[i]);  // update padding if changing MAX_HISTORY
                }
            }
            configFile.Save();
#if !DEBUG
      }
      catch {}
#endif
        }
        public static void LoadFromConfigFile(this ComboBox pThis, string appName = "", string keyName = "")
        {
#if !DEBUG
      try
      {
#endif
            if (string.IsNullOrEmpty(appName))
            {
                appName = Assembly.GetEntryAssembly().GetName().Name;
            }
            if (string.IsNullOrEmpty(keyName))
            {
                keyName = pThis.Name;
            }

            var configFile = new ConfigFile();

            var key = configFile.Data[keyName];
            {
                if (key == null) return;
                pThis.Items.Clear();
                foreach (var item in key)
                    pThis.Items.Add(item.Value);
            }
#if !DEBUG
      }
      catch {}
#endif
        }
        public static void AddUniqueItem(this ComboBox pThis, string itemName)
        {
            if(string.IsNullOrWhiteSpace(itemName))
            {
                return;
            }

            bool foundItem = false;
            for (int idx = 0; idx < pThis.Items.Count; idx++)
            {
                string curItem = pThis.Items[idx] as string;
                if (string.Compare(curItem, itemName, true) == 0)
                {
                    foundItem = true;
                    break;
                }
            }
            if(foundItem == false)
                pThis.Items.Insert(0, itemName);
        }

        public static void UpdateListView(this ListViewEx pThis, List<FoundInfo> foundItems)
        {
            pThis.BeginUpdate();
            pThis.Items.Clear();
            foreach (var found in foundItems)
            {
                var lstItem = pThis.Items.Add(found.MatchValue);
                lstItem.Tag = found;
                lstItem.SubItems.Add(found.GetReplacementPreview());
                lstItem.Checked = true;
            }
            pThis.EndUpdate();
        }
        public static string ReplaceAll(this string sourceText, List<FoundInfo> foundItems)
        {
            if (foundItems == null || foundItems.Count == 0)
            {
                return sourceText;
            }
            var sb = new StringBuilder();
            var sortedItems = foundItems.OrderBy(fi => fi.StartIndex).ToList();
            int lastIndex = 0;
            foreach (var item in sortedItems)
            {
                string replacement = item.GetReplacement();
                sb.Append(sourceText.Substring(lastIndex, item.StartIndex - lastIndex));
                sb.Append(replacement);
                lastIndex = item.StartIndex + item.Length;
            }
            sb.Append(sourceText.Substring(lastIndex));
            return sb.ToString();
        }

        public static List<FoundInfo> GetAllCheckedItems(this ListViewEx pThis)
        {
            var retVal = new List<FoundInfo>();
            for(int idx=0; idx < pThis.CheckedItems.Count;idx++)
            {
                var item = pThis.CheckedItems[idx];
                if (item.Tag is FoundInfo foundItem)
                {
                    retVal.Add(foundItem);
                }
            }
            return retVal;
        }
    }
}
