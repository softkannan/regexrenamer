using RegexRenamer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace RegexRenamer.Rename;

/// <summary>
/// Pure validation logic for filenames, regex patterns, globs, paths, and preview conflict detection.
/// No UI dependencies — operates on data models and returns results.
/// </summary>
internal sealed class ValidationService
{
    private static readonly Regex s_regInvalidChars = new("([\\\\/:*?\"<>|])");
    private static readonly Regex s_regInvalidCharsAllowPath = new("([/:*?\"<>|])");
    private static readonly Regex s_regOnlyDotSpace = new("^[ .]+$");
    private static readonly Regex s_regEndsInDotSpace = new("([ .]+)$");

    /// <summary>
    /// Validates a glob filter pattern for invalid characters.
    /// </summary>
    /// <returns>Error message, or null if valid.</returns>
    public string ValidateGlob(string testGlob)
    {
        Regex regex = new("([\\\\/:\"<>|])");
        Match match = regex.Match(testGlob);
        return match.Success
            ? "Invalid character: " + match.Groups[0].Value
            : null;
    }

    /// <summary>
    /// Validates a regex pattern for syntax errors.
    /// </summary>
    /// <returns>Error message, or null if valid.</returns>
    public string ValidateRegex(string testRegex)
    {
        try
        {
            _ = new Regex(testRegex);
        }
        catch (Exception ex)
        {
            Regex regex = new("^parsing \".+\" - ");
            return regex.Replace(ex.Message, "");
        }
        return null;
    }

    /// <summary>
    /// Validates a filename (or path with subfolders) for invalid characters and patterns.
    /// </summary>
    /// <param name="testFilename">The filename or relative path to validate.</param>
    /// <param name="allowRenSub">Whether subfolder paths (backslashes) are allowed.</param>
    /// <param name="strFilename">Display string for "filename" or "folder name".</param>
    /// <returns>Error message, or null if valid.</returns>
    public string ValidateFilename(string testFilename, bool allowRenSub, string strFilename)
    {
        Match match;

        // invalid character
        string[] parts = allowRenSub ? testFilename.Split('\\') : [testFilename];
        for (int i = 0; i < parts.Length; i++)
        {
            match = allowRenSub
                ? s_regInvalidCharsAllowPath.Match(parts[i])
                : s_regInvalidChars.Match(parts[i]);

            if (match.Success)
                if (parts.Length > 1 && i != parts.Length - 1)
                    return "The subfolder '" + parts[i] + "' contains an invalid character: '" + match.Groups[0].Value + "'.";
                else
                    return "The " + strFilename + " '" + parts[i] + "' contains an invalid character: '" + match.Groups[0].Value + "'.";
        }

        // starts with "\"
        if (testFilename.StartsWith("\\"))
            if (parts.Length > 2)
                return "The subfolder cannot begin with '\\'.";
            else
                return "The " + strFilename + " cannot begin with a backslash.";

        // element is empty
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] == "")
                if (i != parts.Length - 1)
                    return "Duplicate path seperator";
                else
                    return "The " + strFilename + " cannot end with a backslash.";
        }

        // element is [ .]+ (eg, "/../", "/ /", ...)
        for (int i = 0; i < parts.Length; i++)
        {
            match = s_regOnlyDotSpace.Match(parts[i]);
            if (match.Success)
                if (i != parts.Length - 1)
                    return "Invalid subfolder: '" + parts[i] + "'.";
                else
                    return "Invalid " + strFilename + ": '" + parts[i] + "'.";
        }

        // element ends with [ .]+
        for (int i = 0; i < parts.Length; i++)
        {
            match = s_regEndsInDotSpace.Match(parts[i]);
            if (match.Success)
                if (i != parts.Length - 1)
                    return "The subfolder '" + parts[i] + "' ends with invalid character(s): '" + match.Groups[0].Value + "'.";
                else
                    return "The " + strFilename + " '" + parts[i] + "' ends with invalid character(s): '" + match.Groups[0].Value + "'.";
        }

        return null;
    }

    /// <summary>
    /// Validates a path string, normalizes it, and checks existence.
    /// </summary>
    /// <param name="pathText">The raw path text to validate.</param>
    /// <param name="normalizedPath">The normalized path if valid, or null.</param>
    /// <returns>Error message, or null if valid.</returns>
    public string ValidatePath(string pathText, out string normalizedPath)
    {
        normalizedPath = null;
        try
        {
            string normPath = Path.GetFullPath(pathText);
            if (normPath.Length > 3)
                normPath = normPath.TrimEnd('\\');

            if (!Directory.Exists(normPath))
            {
                return "Path does not exist.";
            }

            normalizedPath = normPath;
            return null;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    /// Runs preview validation across all files, detecting naming conflicts and errors.
    /// </summary>
    /// <param name="files">The list of rename items with preview data.</param>
    /// <param name="inactiveFiles">Map of inactive file names to their reason.</param>
    /// <param name="input">Current user input snapshot.</param>
    /// <param name="strFilename">Display string for "filename" or "folder name".</param>
    /// <param name="strFile">Display string for "file" or "folder".</param>
    /// <param name="fileCount">Number of rows in the data grid (may differ from files.Count).</param>
    /// <param name="getActiveFileIndex">Function mapping display-row index to active-file index.</param>
    /// <returns>Validation result with per-file errors and aggregate counts.</returns>
    public ValidationResult ValidatePreview(
        IReadOnlyList<RenameItemInfo> files,
        IReadOnlyDictionary<string, InactiveReason> inactiveFiles,
        UserInputModel input,
        string strFilename,
        string strFile,
        int fileCount,
        Func<int, int> getActiveFileIndex)
    {
        ValidationResult result = new();
        bool[] hasError = new bool[fileCount];
        Dictionary<string, List<int>> hashPreview = [];

        // generate hash of preview filenames
        for (int dfi = 0; dfi < fileCount; dfi++)
        {
            int afi = getActiveFileIndex(dfi);
            string preview = files[afi].PreviewExt.ToLower();

            if (!hashPreview.ContainsKey(preview))
                hashPreview.Add(preview, []);
            hashPreview[preview].Add(dfi);
        }

        // determine output path
        string outputPath = input.ActivePath;
        if (input.Output == OutputMode.MoveTo || input.Output == OutputMode.CopyTo)
            outputPath = input.MoveCopyPath;

        // check for errors
        for (int dfi = 0; dfi < fileCount; dfi++)
        {
            int afi = getActiveFileIndex(dfi);
            string preview = files[afi].PreviewExt.ToLower();

            if (hasError[dfi]) continue;

            if (input.Output == OutputMode.RenameInPlace)
            {
                if (files[afi].Name == files[afi].Preview) continue;
            }
            else
            {
                if (!files[afi].Matched) continue;
            }

            // valid filename check
            string validFilenameErrmsg = ValidateFilename(files[afi].PreviewExt, false, strFilename);
            if (validFilenameErrmsg != null)
            {
                result.FileErrors[afi] = validFilenameErrmsg;
                continue;
            }

            // dupe filename conflicts
            if (!files[afi].Preview.Contains("\\")
                && (input.Output == OutputMode.RenameInPlace || input.Output == OutputMode.BackupTo))
            {
                // check against active files
                if (hashPreview[preview].Count > 1)
                {
                    foreach (int dfi2 in hashPreview[preview])
                    {
                        if (hasError[dfi2])
                            continue;
                        else
                            hasError[dfi2] = true;

                        int afi2 = getActiveFileIndex(dfi2);
                        result.FileErrors[afi2] = "The " + strFilename + " '" + files[afi2].PreviewExt
                                                + "' conflicts with another " + strFile + " in the preview column.";
                    }
                    continue;
                }

                // check against inactive files
                if (inactiveFiles.ContainsKey(preview))
                {
                    switch (inactiveFiles[preview])
                    {
                        case InactiveReason.Filtered:
                            result.FileErrors[afi] = "The " + strFilename + " '" + files[afi].PreviewExt
                                                   + "' already exists in this directory but is currently filtered out.";
                            break;
                        case InactiveReason.Hidden:
                            result.FileErrors[afi] = "The " + strFilename + " '" + files[afi].PreviewExt
                                                   + "' already exists in this directory as a hidden " + strFile + ".";
                            break;
                    }
                    continue;
                }

                // file-folder conflicts
                if (fileCount < 2000)
                {
                    string previewFullpath = Path.Combine(outputPath, files[afi].PreviewExt);
                    if (input.RenameFolders ? File.Exists(previewFullpath) : Directory.Exists(previewFullpath))
                    {
                        result.FileErrors[afi] = "The " + strFilename + " '" + files[afi].PreviewExt
                                               + "' conflicts with a " + (input.RenameFolders ? "file" : "folder")
                                               + " in the current path.";
                        continue;
                    }
                }
            }
            else // destination is other directory
            {
                string previewFullpath = Path.Combine(outputPath, files[afi].PreviewExt);
                if (input.RenameFolders ? Directory.Exists(previewFullpath) : File.Exists(previewFullpath))
                {
                    result.FileErrors[afi] = "The " + strFilename + " '"
                                           + Path.GetFileName(files[afi].PreviewExt)
                                           + "' already exists in the destination folder.";
                    continue;
                }

                if (input.RenameFolders ? File.Exists(previewFullpath) : Directory.Exists(previewFullpath))
                {
                    result.FileErrors[afi] = "The " + strFilename + " '"
                                           + Path.GetFileName(files[afi].PreviewExt) + "' conflicts with a "
                                           + (input.RenameFolders ? "file" : "folder") + " in the destination path.";
                    continue;
                }
            }

            // backup path original name conflict
            if (input.Output == OutputMode.BackupTo)
            {
                string previewFullpath = Path.Combine(input.MoveCopyPath, files[afi].Filename);
                if (File.Exists(previewFullpath))
                {
                    result.FileErrors[afi] = "The original filename '" + files[afi].Filename
                                           + "' already exists in the selected backup folder.";
                    continue;
                }

                if (Directory.Exists(previewFullpath))
                {
                    result.FileErrors[afi] = "The original filename '" + files[afi].Filename
                                           + "' conflicts with a folder in the selected backup path.";
                    continue;
                }
            }
        }

        // compute counts
        for (int idx = 0; idx < files.Count; idx++)
            if (files[idx].Matched) result.MatchedCount++;

        result.ConflictCount = result.FileErrors.Count;

        return result;
    }

    /// <summary>
    /// Performs pre-rename validation checks (error conditions, file counts, path validity).
    /// </summary>
    /// <param name="files">The list of rename items.</param>
    /// <param name="input">Current user input snapshot.</param>
    /// <param name="validMatch">Whether the current match regex is valid.</param>
    /// <param name="fileErrors">Per-file error map from preview validation.</param>
    /// <param name="strFile">Display string for "file" or "folder".</param>
    /// <param name="strFilename">Display string for "filename" or "folder name".</param>
    /// <returns>Check result indicating whether rename can proceed.</returns>
    public PreRenameCheckResult CheckBeforeRename(
        IReadOnlyList<RenameItemInfo> files,
        UserInputModel input,
        bool validMatch,
        Dictionary<int, string> fileErrors,
        string strFile,
        string strFilename)
    {
        // invalid match regex
        if (!validMatch)
            return new PreRenameCheckResult { ErrorMessage = "The match regular expression in invalid." };

        // preview errors exist
        foreach (var kvp in fileErrors)
        {
            int afi = kvp.Key;
            if (input.RenameSelectionOnly && !files[afi].Selected)
                continue;

            return new PreRenameCheckResult { ErrorMessage = "Can't rename while errors exist (highlighted in red)." };
        }

        // count files to rename
        int filesToRename = 0;
        foreach (RenameItemInfo file in files)
        {
            if (input.RenameSelectionOnly && !file.Selected)
                continue;

            if ((input.Output == OutputMode.RenameInPlace && file.Name != file.Preview)
             || (input.Output != OutputMode.RenameInPlace && file.Matched))
                filesToRename++;
        }

        if (filesToRename == 0)
            return new PreRenameCheckResult { ErrorMessage = "There are no " + strFile + "s to rename." };

        // move/copy path doesn't exist
        if (input.Output != OutputMode.RenameInPlace && !Directory.Exists(input.MoveCopyPath))
        {
            string label = input.Output switch
            {
                OutputMode.MoveTo => "Move to",
                OutputMode.CopyTo => "Copy to",
                OutputMode.BackupTo => "Backup to",
                _ => "Output"
            };
            return new PreRenameCheckResult { ErrorMessage = "'" + label + "' folder '" + input.MoveCopyPath + "' is not a valid path." };
        }

        // move/copy path same as activePath
        if (input.Output != OutputMode.RenameInPlace && input.MoveCopyPath == input.ActivePath)
        {
            string label = input.Output switch
            {
                OutputMode.MoveTo => "Move to",
                OutputMode.CopyTo => "Copy to",
                OutputMode.BackupTo => "Backup to",
                _ => "Output"
            };
            return new PreRenameCheckResult { ErrorMessage = "'" + label + "' folder is the same as the currently selected folder." };
        }

        // check for filenames starting with space or dot
        bool hasInvalidStartChars = false;
        Regex regexInvalidChars = new("(^|\\\\)[ .]");

        foreach (RenameItemInfo file in files)
        {
            if (input.Output == OutputMode.RenameInPlace)
            {
                if (file.Name == file.Preview) continue;
            }
            else
            {
                if (!file.Matched) continue;
            }

            if (regexInvalidChars.IsMatch(file.Preview))
            {
                hasInvalidStartChars = true;
                break;
            }
        }

        return new PreRenameCheckResult
        {
            FilesToRename = filesToRename,
            HasInvalidStartChars = hasInvalidStartChars
        };
    }
}
