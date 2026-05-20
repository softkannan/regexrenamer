using RegexRenamer.Models;
using RegexRenamer.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace RegexRenamer.Rename;

/// <summary>
/// Executes the file/folder rename operation. Pure business logic — no UI dependencies.
/// Progress and cancellation are communicated via delegates.
/// </summary>
internal sealed class RenameService
{
    /// <summary>
    /// Executes the rename/move/copy operation for all applicable files.
    /// </summary>
    /// <param name="files">The list of rename items with preview data.</param>
    /// <param name="input">Current user input snapshot.</param>
    /// <param name="filesToRename">Expected number of files to rename (for progress calculation).</param>
    /// <param name="reportProgress">Callback to report progress percentage (0-100).</param>
    /// <param name="isCancelled">Function that returns true if the user requested cancellation.</param>
    /// <returns>The result of the rename operation.</returns>
    public RenameResult Execute(
        IReadOnlyList<RenameItemInfo> files,
        UserInputModel input,
        int filesToRename,
        Action<int> reportProgress,
        Func<bool> isCancelled)
    {
        RenameResult result = new();
        float filesRenamed = 0.5F;

        string outputPath = input.ActivePath;
        if (input.Output == OutputMode.MoveTo || input.Output == OutputMode.CopyTo)
            outputPath = input.MoveCopyPath;

        for (int afi = 0; afi < files.Count; afi++)
        {
            // abort if user cancelled
            if (isCancelled())
            {
                result.Cancelled = true;
                break;
            }

            // skip ignored/unselected files
            if (input.Output == OutputMode.RenameInPlace)
            {
                if (files[afi].Name == files[afi].Context.Preview) continue;
            }
            else
            {
                if (!files[afi].Context.Matched) continue;
            }

            if (input.RenameSelectionOnly)
            {
                if (!files[afi].Context.Selected) continue;
            }

            // update progress
            reportProgress((int)((filesRenamed / filesToRename) * 100));
            filesRenamed++;

            // get new fullpath
            string newFullpath = Path.Combine(outputPath, files[afi].Context.Preview);
            if (input.PreserveExtension)
                newFullpath += files[afi].Extension;

            // create subdirs (if any)
            if (files[afi].Context.Preview.Contains("\\"))
            {
                string newDirectory = Path.GetDirectoryName(newFullpath);
                if (!Directory.Exists(newDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(newDirectory);
                    }
                    catch (Exception ex)
                    {
                        result.ReportError(files[afi].Name, files[afi].Context.Preview,
                            "Create folder '" + newDirectory + "' failed: " + ex.Message);
                        continue;
                    }
                }
                result.RenameToSubfolders = true;
            }

            // rename/move/copy
            try
            {
                if (input.RenameFolders)
                {
                    Directory.Move(files[afi].Fullpath, newFullpath);
                }
                else
                {
                    if (input.Output == OutputMode.RenameInPlace || input.Output == OutputMode.MoveTo)
                        File.Move(files[afi].Fullpath, newFullpath);
                    else if (input.Output == OutputMode.CopyTo)
                        File.Copy(files[afi].Fullpath, newFullpath);
                    else // backup to
                    {
                        File.Copy(files[afi].Fullpath,
                            Path.Combine(input.MoveCopyPath, Path.GetFileName(files[afi].Filename)));
                        File.Move(files[afi].Fullpath, newFullpath);
                    }
                }

                result.ReportSuccess();
            }
            catch (Exception ex)
            {
                result.ReportError(files[afi].Name, files[afi].Context.Preview, ex.Message);
                continue;
            }
        }

        reportProgress(100);
        return result;
    }
}
