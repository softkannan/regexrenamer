using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Config;

public static class ConfigExtensions
{
    public static string GetNextValidFolderNameFromFolder(this string pThis)
    {
        var retVal = pThis;
        while(!string.IsNullOrEmpty(retVal) && !Directory.Exists(retVal))
        {
            retVal = Path.GetDirectoryName(retVal);
        }
        return retVal;
    }

    public static string GetNextValidFolderNameFromFile(this string pThis)
    {
        if (string.IsNullOrEmpty(pThis))
            return string.Empty;
        var retVal = pThis;
        do
        {
            retVal = Path.GetDirectoryName(retVal);
        } while (!string.IsNullOrEmpty(retVal) && !Directory.Exists(retVal));
        return retVal;
    }

    // convert string to filename
    public static string ToFileName(this string pThis)
    {
        if (string.IsNullOrEmpty(pThis))
            return string.Empty;
        var sb = new StringBuilder();
        foreach (var c in pThis)
        {
            if (char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.')
                sb.Append(c);
            else
                sb.Append('_');
        }
        return sb.ToString();
    }
    public static string BuildCommandline(this List<string> pThis)
    {
        if (pThis == null || pThis.Count == 0)
            return string.Empty;
        var sb = new StringBuilder();
        foreach (var item in pThis)
        {
            if (sb.Length > 0)
                sb.Append(" ");
            var expandedItem = ResolveValue.Inst.ResolveFullPath(item);
            sb.Append(expandedItem);
        }
        return sb.ToString();
    }

    public static string Replace(this string str, string oldValue, string newValue, StringComparison comparisonType)
    {

        // Check inputs.
        if (str == null)
        {
            // Same as original .NET C# string.Replace behavior.
            throw new ArgumentNullException(nameof(str));
        }
        if (oldValue == null)
        {
            // Same as original .NET C# string.Replace behavior.
            throw new ArgumentNullException(nameof(oldValue));
        }
        if (oldValue.Length == 0)
        {
            // Same as original .NET C# string.Replace behavior.
            throw new ArgumentException("String cannot be of zero length.");
        }
        if (str.Length == 0)
        {
            // Same as original .NET C# string.Replace behavior.
            return str;
        }

        //if (oldValue.Equals(newValue, comparisonType))
        //{
        //This condition has no sense
        //It will prevent method from replacesing: "Example", "ExAmPlE", "EXAMPLE" to "example"
        //return str;
        //}

        // Prepare string builder for storing the processed string.
        // Note: StringBuilder has a better performance than String by 30-40%.
        StringBuilder resultStringBuilder = new StringBuilder(str.Length);
        // Analyze the replacement: replace or remove.
        bool isReplacementNullOrEmpty = string.IsNullOrEmpty(newValue);
        // Replace all values.
        const int valueNotFound = -1;
        int foundAt;
        int startSearchFromIndex = 0;
        while ((foundAt = str.IndexOf(oldValue, startSearchFromIndex, comparisonType)) != valueNotFound)
        {
            // Append all characters until the found replacement.
            int charsUntilReplacment = foundAt - startSearchFromIndex;
            bool isNothingToAppend = charsUntilReplacment == 0;
            if (!isNothingToAppend)
            {
                resultStringBuilder.Append(str, startSearchFromIndex, charsUntilReplacment);
            }
            // Process the replacement.
            if (!isReplacementNullOrEmpty)
            {
                resultStringBuilder.Append(newValue);
            }
            // Prepare start index for the next search.
            // This needed to prevent infinite loop, otherwise method always start search 
            // from the start of the string. For example: if an oldValue == "EXAMPLE", newValue == "example"
            // and comparisonType == "any ignore case" will conquer to replacing:
            // "EXAMPLE" to "example" to "example" to "example" … infinite loop.
            startSearchFromIndex = foundAt + oldValue.Length;
            if (startSearchFromIndex == str.Length)
            {
                // It is end of the input string: no more space for the next search.
                // The input string ends with a value that has already been replaced. 
                // Therefore, the string builder with the result is complete and no further action is required.
                return resultStringBuilder.ToString();
            }
        }
        // Append the last part to the result.
        int charsUntilStringEnd = str.Length - startSearchFromIndex;
        resultStringBuilder.Append(str, startSearchFromIndex, charsUntilStringEnd);
        return resultStringBuilder.ToString();
    }

    public static string ExpandTemplate(this string pThis, List<Tuple<string, string>> templatePairs)
    {
        if (string.IsNullOrEmpty(pThis) || templatePairs == null || templatePairs.Count == 0)
            return pThis;
        var result = pThis;
        foreach (var item in templatePairs)
        {
            // Replace item1 with item2 and ignore case
            result = result.Replace(item.Item1, item.Item2,StringComparison.OrdinalIgnoreCase);
        }
        return result;
    }

    // Get Cleartool entry for given name
    public static NamedCmd GetToolCmd(this UserConfig config, string name)
    {
        if (config.KnownCmds == null )
            return null;
        return config.KnownCmds.FirstOrDefault(tool => string.Compare(tool.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
    }

    public static Tool GetTool(this UserConfig config, string toolName)
    {
        Tool retVal = null;
        var foundTool = config.AvailableTools.FirstOrDefault((item) => string.Compare(item.Name, toolName, StringComparison.OrdinalIgnoreCase) == 0);
        if (foundTool != null)
        {
            retVal = foundTool;
        }
        return retVal;
    }

    public static string GetToolPath(this UserConfig config, string toolName)
    {
        string retVal = "";
        var foundTool = config.AvailableTools.FirstOrDefault((item) => string.Compare(item.Name, toolName, StringComparison.OrdinalIgnoreCase) == 0);
        if (foundTool != null)
        {
            retVal = foundTool.Path;
        }
        return retVal;
    }

    public static bool IsToolAvailable(this UserConfig config, string toolName)
    {
        var foundTool = config.AvailableTools.FirstOrDefault((item) => string.Compare(item.Name, toolName, StringComparison.OrdinalIgnoreCase) == 0);
        return foundTool != null;
    }
}
