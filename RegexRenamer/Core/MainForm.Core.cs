using RegexRenamer.Kavita;
using RegexRenamer.Native;
using RegexRenamer.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer
{
    public partial class MainForm
    {
        // update directory tree/filenames/previews/validation (each cascades into the one below)
        private void UpdateFolderTree()
        {
            // get selected path (regardless whether it exists)

            if (tvwFolders.SelectedNode != null && !Directory.Exists(activePath))
                activePath = tvwFolders.ForceGetSelectedNodePath();


            // save prev path to preserve node expansion
            string prevPathExpand = null;
            if (tvwFolders.SelectedNode != null && tvwFolders.SelectedNode.IsExpanded)
                prevPathExpand = activePath.ToLower();


            // init tvwFolders with directory tree
            tvwFolders.InitFolderTreeView();


            // get active path
            while (!Directory.Exists(activePath))  // if doesn't exist, walk tree backwards
            {
                DirectoryInfo di = null;
                try { di = Directory.GetParent(activePath); } catch { }
                if (di == null) break;

                activePath = di.FullName;
            }

            if (!Directory.Exists(activePath))  // still not found, default to system drive
                activePath = Environment.GetEnvironmentVariable("SystemDrive") + "\\";


            // drill to folder and expand
            EnableUpdates = false;
            if (activePath.StartsWith("\\\\"))
            {
                // select My Network Places
                tvwFolders.SelectedNode = (TreeNode)tvwFolders.Tag;
            }
            else if (Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).Equals(activePath, StringComparison.CurrentCultureIgnoreCase))
            {
                // select root node
                tvwFolders.SelectedNode = tvwFolders.Nodes[0];
            }
            else
            {
                // find folder in tree
                if (!tvwFolders.DrillToFolder(activePath))
                    activePath = tvwFolders.GetSelectedNodePath();
            }
            EnableUpdates = true;


            // re-expand
            if (tvwFolders.SelectedNode != null && prevPathExpand == activePath.ToLower())
                tvwFolders.SelectedNode.Expand();


            UpdateFileList();
        }
        private void UpdateFileList()
        {
            if (!EnableUpdates) return;
            dgvFiles.Tag = 0;  // reset files ignored
            fileCount.Reset();


            // update txtPath

            txtPath.BackColor = SystemColors.Window;
            txtPath.Text = activePath;
            txtPath.Update();


            // if invalid selection, clear all

            if (activePath == "")
            {
                activeFiles.Clear();
                inactiveFiles.Clear();
                icons.Clear();
                dgvFiles.Rows.Clear();
                lblNumMatched.Text = "0";
                lblNumConflict.Text = "0";
                UpdateFileStats();

                return;
            }

            this.Cursor = Cursors.AppStarting;


            // create filter regex, if necessary

            Regex filter = null;
            string filterText = activeFilter;

            if (filterText != "")
            {
                if (rbFilterGlob.Checked && filterText == "*.*")  // convert to "*" (include files with no extension)
                    filterText = "*";

                if (rbFilterGlob.Checked)  // convert glob to regex
                    filterText = "^" + Regex.Escape(filterText).Replace("\\*", ".*").Replace("\\?", ".") + "$";

                filter = new Regex(filterText, RegexOptions.IgnoreCase);
            }


            // loop through file list, build RRItem array

            activeFiles.Clear();
            inactiveFiles.Clear();
            icons.Clear();

            DirectoryInfo activeDir = new DirectoryInfo(activePath);

            if (RenameFolders)  // folders
            {
                DirectoryInfo[] dirs = new DirectoryInfo[0];
                try
                {
                    dirs = activeDir.GetDirectories();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                foreach (DirectoryInfo dir in dirs)
                {
                    fileCount.total++;

                    // ignore if filtered out

                    if (filter != null && filter.IsMatch(dir.Name) == cbFilterExclude.Checked)
                    {
                        if (!inactiveFiles.ContainsKey(dir.Name.ToLower()))
                            inactiveFiles.Add(dir.Name.ToLower(), InactiveReason.Filtered);
                        fileCount.filtered++;
                        continue;
                    }

                    // ignore if hidden and not showing hidden files

                    bool hidden = false;
                    try
                    {
                        hidden = (dir.Attributes & FileAttributes.Hidden) != 0;
                    }
                    catch { }  // reported System.UnauthorizedAccessException here under some versions of Samba when item is a link to /dev/null

                    if (hidden) fileCount.hidden++;
                    if (!itmOptionsShowHidden.Checked && hidden)
                    {
                        if (!inactiveFiles.ContainsKey(dir.Name.ToLower()))
                            inactiveFiles.Add(dir.Name.ToLower(), InactiveReason.Hidden);
                        continue;
                    }

                    activeFiles.Add(new RRItem(dir, hidden, itmOptionsPreserveExt.Checked));
                }
            }
            else // files
            {
                FileInfo[] files = new FileInfo[0];
                try
                {
                    files = activeDir.GetFiles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                foreach (FileInfo file in files)
                {
                    fileCount.total++;


                    // ignore if filtered out

                    if (filter != null && filter.IsMatch(file.Name) == cbFilterExclude.Checked)
                    {
                        if (!inactiveFiles.ContainsKey(file.Name.ToLower()))
                            inactiveFiles.Add(file.Name.ToLower(), InactiveReason.Filtered);
                        fileCount.filtered++;
                        continue;
                    }


                    // ignore if hidden and not showing hidden files

                    bool hidden = false;
                    try
                    {
                        hidden = (file.Attributes & FileAttributes.Hidden) != 0;
                    }
                    catch { }  // reported System.UnauthorizedAccessException here under some versions of Samba when item is a link to /dev/null

                    if (hidden) fileCount.hidden++;
                    if (!itmOptionsShowHidden.Checked && hidden)
                    {
                        if (!inactiveFiles.ContainsKey(file.Name.ToLower()))
                            inactiveFiles.Add(file.Name.ToLower(), InactiveReason.Hidden);
                        continue;
                    }

                    activeFiles.Add(new RRItem(file, hidden, itmOptionsPreserveExt.Checked));
                }
            }


            // create datagridview items w/ filename

            dgvFiles.Rows.Clear();

            for (int i = 0; i < activeFiles.Count; i++)
            {
                if (i >= MAX_FILES)  // reached limit
                {
                    dgvFiles.Tag = activeFiles.Count - i;  // num files ignored, will display warning dialog after preview is updated
                    activeFiles.RemoveRange(i, activeFiles.Count - MAX_FILES);
                    break;
                }


                // add new item

                dgvFiles.Rows.Add(null, activeFiles[i].Name, null);
                dgvFiles.Rows[i].Tag = i;  // store activeFiles index so we can refer back when under different sorting


                // add image (keyed by extension)

#if !DEBUG
        try  
        {
#endif
                if (RenameFolders)
                {
                    if (icons.Count == 0)
                        icons.Add("folder", TreeviewExtractIcons.GetFolderIcon());
                    dgvFiles.Rows[i].Cells[0].Value = icons["folder"];
                }
                else
                {
                    string ext = activeFiles[i].Extension.ToLower();
                    if (ext == ".lnk")  // shortcut, don't key by extension as each may have different icon
                    {
                        ext = ".lnk." + icons.Count;
                        icons.Add(ext, TreeviewExtractIcons.GetIcon(activeFiles[i].Fullpath, false));
                        dgvFiles.Rows[i].Cells[0].Value = icons[ext];
                    }
                    else  // non-shortcut
                    {
                        if (!icons.ContainsKey(ext))
                            icons.Add(ext, TreeviewExtractIcons.GetIcon(activeFiles[i].Fullpath, false));

                        dgvFiles.Rows[i].Cells[0].Value = icons[ext];
                    }
                }
#if !DEBUG
        }
        catch  // default: no image
        {
          dgvFiles.Rows[i].Cells[0].Value = new Bitmap( 1, 1 );
        }
#endif
            }


            fileCount.shown = dgvFiles.Rows.Count;
            UpdateFileStats();
            UpdateSelection();
            UpdatePreview();
        }
        private void UpdatePreview()
        {
            if (!EnableUpdates || !validMatch) return;

            this.Cursor = Cursors.AppStarting;

            const string rxDoller = @"(?<=(?:^|[^$])(?:\$\$)*)\$";  // regex for an actual (non-escaped) doller sign

            // generate preview

            if (cmbMatch.Text != "")
            {
                // compile regex
                RegexOptions options = RegexOptions.None | RegexOptions.Compiled;
                int count = cbModifierG.Checked ? -1 : 1;
                if (cbModifierI.Checked) { options |= RegexOptions.IgnoreCase; }
                if (cbModifierX.Checked) { options |= RegexOptions.IgnorePatternWhitespace; }
                //main regex
                Regex regex = new Regex(cmbMatch.Text, options);

                // auto numbering
                int numCurrent = 0, numInc = 0, numStart = 0, numReset = 0;
                string numFormatted = "";
                bool doingAutoNum = false;
                bool doingAutoNumLetter = false;  // number sequence is actually a-z letter sequence
                bool doingAutoNumLetterUpper = false;  // letter sequence is uppercase

                if (this.validNumber && Regex.IsMatch(this.cmbReplace.Text, rxDoller + "#"))
                    doingAutoNum = true;

                Match match = Regex.Match(this.txtNumberingStart.Text, @"^(([a-z]+)|([A-Z]+))$");
                doingAutoNumLetter = match.Success;
                doingAutoNumLetterUpper = match.Success && match.Groups[3].Length > 0;

                if (doingAutoNum)
                {
                    if (doingAutoNumLetter)
                        numStart = SequenceLetterToNumber(txtNumberingStart.Text.ToLower());
                    else
                        numStart = Int32.Parse(txtNumberingStart.Text);

                    numInc = Int32.Parse(txtNumberingInc.Text);
                    numReset = Int32.Parse(txtNumberingReset.Text);
                }
                numCurrent = numStart - numInc;  // back up one


                // regex each filename

                string userReplacePattern = cmbReplace.Text;
                if (doingAutoNum)
                    userReplacePattern = Regex.Replace(userReplacePattern, rxDoller + @"(\d+)" + rxDoller + "#", "$${$1}$$#");


                for (int afi = 0; afi < activeFiles.Count; afi++)
                {
                    // check if matches
                    activeFiles[afi].Info = null;
                    activeFiles[afi].Matched = regex.IsMatch(activeFiles[afi].Name);


                    // if not, bail early, don't incrememnt autonum

                    if (!activeFiles[afi].Matched)
                    {
                        activeFiles[afi].Preview = activeFiles[afi].Name;
                        continue;
                    }


                    // else, matched

                    string replacePattern;

                    if (doingAutoNum)
                    {
                        numCurrent += numInc;

                        if (numReset != 0 && (numCurrent - numStart) % numReset == 0)
                            numCurrent = numStart;

                        if (numFormatted != "$#")  // basic int overflow & negative number detection
                        {
                            if (!doingAutoNumLetter)  // number sequence
                            {
                                if (numCurrent < 0)
                                    numFormatted = "$#";
                                else
                                    numFormatted = numCurrent.ToString(txtNumberingPad.Text);
                            }
                            else  // letter sequence
                            {
                                if (numCurrent < 1)
                                    numFormatted = "$#";
                                else if (doingAutoNumLetterUpper)
                                    numFormatted = SequenceNumberToLetter(numCurrent).ToUpper();
                                else
                                    numFormatted = SequenceNumberToLetter(numCurrent);
                            }
                        }

                        replacePattern = Regex.Replace(userReplacePattern, rxDoller + "#", numFormatted);
                    }
                    else
                    {
                        replacePattern = userReplacePattern;
                    }

                    if (!itmChangeCaseNoChange.Checked)
                        replacePattern = "\n" + replacePattern + "\n";  // delimit change-case boundaries

                    activeFiles[afi].Preview = regex.Replace(activeFiles[afi].Name, replacePattern, count);

                    if (!itmChangeCaseNoChange.Checked)
                        activeFiles[afi].Preview = Regex.Replace(activeFiles[afi].Preview, @"\n([^\n]*)\n", new MatchEvaluator(MatchEvalChangeCase));

                    if (!noneToolStripMenuItem.Checked)
                    {
                        var curKavitaRoot = setAsKavitaLibraryRootToolStripMenuItem.Tag as string;
                        curKavitaRoot ??= Directory.GetDirectoryRoot(activeFiles[afi].Fullpath);

                        UpdateKavitaCheck(activeFiles[afi], curKavitaRoot, curLibType);
                    }

                    if (activeFiles[afi].Preview.Length == 0)
                        activeFiles[afi].Preview = activeFiles[afi].Name;
                }

            }
            else  // cmbMatch.Text == ""
            {
                foreach (RRItem file in activeFiles)
                {
                    file.Preview = file.Name;
                    file.Matched = false;
                }
            }


            // update file list

            for (int dfi = 0; dfi < dgvFiles.Rows.Count; dfi++)
            {
                int afi = (int)dgvFiles.Rows[dfi].Tag;
                dgvFiles.Rows[dfi].Cells[2].Value = activeFiles[afi].Preview;
                //showSizeToolStripMenuItem
                if (activeFiles[afi].Info != null)
                {
                    dgvFiles.Rows[dfi].Cells[3].Value = activeFiles[afi].Info.Series;
                    dgvFiles.Rows[dfi].Cells[4].Value = activeFiles[afi].Info.Volumes;
                    dgvFiles.Rows[dfi].Cells[5].Value = activeFiles[afi].Info.Chapters;
                    dgvFiles.Rows[dfi].Cells[6].Value = activeFiles[afi].Info.Title;
                    dgvFiles.Rows[dfi].Cells[7].Value = activeFiles[afi].Info.Edition;
                    dgvFiles.Rows[dfi].Cells[8].Value = activeFiles[afi].Info.IsSpecial ? "true" : "false";
                }
            }


            // do preview filename validation

            UpdateValidation();


            // redraw

            dgvFiles.Sort(this.dgvFiles.SortedColumn ?? this.colFilename,
                           dgvFiles.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending);  // resort
            this.Cursor = Cursors.Default;


            // show warning if any ignored files

            if ((int)dgvFiles.Tag > 0)
            {
                MessageBox.Show("For performance reasons, RegexRenamer will only display " + MAX_FILES
                               + " " + strFile + "s at once (" + (int)dgvFiles.Tag + " " + strFile + "s ignored).\r\n"
                               + "Use a filter to display only the " + strFile + "s you need to rename.",
                                 "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvFiles.Tag = 0;  // prevent re-display
            }


            // keep selection cleared

            if (!itmOptionsRenameSelectedRows.Checked)
                dgvFiles.ClearSelection();


            PreviewNeedsUpdate = false;
        }

        private void UpdateSelection()
        {
            if (dgvFiles.Rows.Count != this.activeFiles.Count)
                return;

            RRItem firstSelection = null;
            foreach (DataGridViewRow row in dgvFiles.Rows)
            {
                if (row.Tag == null) continue;

                int afi = (int)row.Tag;
                this.activeFiles[afi].Selected = row.Selected;
                if(firstSelection == null && this.activeFiles[afi].Selected)
                {
                    firstSelection = this.activeFiles[afi];
                }
            }

            UpdateFileInfo(firstSelection);
        }

        // background worker
        private void bgwRename_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            RenameResult result = new RenameResult();
            int filesToRename = (int)e.Argument;
            float filesRenamed = 0.5F;

            string outputPath = activePath;
            if (itmOutputMoveTo.Checked || itmOutputCopyTo.Checked)
                outputPath = fbdMoveCopy.SelectedPath;


            for (int afi = 0; afi < activeFiles.Count; afi++)
            {
                // abort if user cancelled

                if (bw.CancellationPending)
                {
                    // e.Cancel = true;       // don't use this as it prevents access to the result object
                    result.Cancelled = true;  // use our own instead
                    break;
                }


                // skip ignored/unselected files

                if (itmOutputRenameInPlace.Checked)
                {
                    if (activeFiles[afi].Name == activeFiles[afi].Preview) continue;
                }
                else
                {
                    if (!activeFiles[afi].Matched) continue;
                }

                if (itmOptionsRenameSelectedRows.Checked)
                {
                    if (!activeFiles[afi].Selected) continue;
                }


                // update progressbar

                bw.ReportProgress((int)((filesRenamed / filesToRename) * 100));
                filesRenamed++;


                // get new fullpath

                string newFullpath = Path.Combine(outputPath, activeFiles[afi].Preview);
                if (itmOptionsPreserveExt.Checked)
                    newFullpath += activeFiles[afi].Extension;


                // create subdirs (if any)

                if (activeFiles[afi].Preview.Contains("\\"))
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
                            result.ReportError(activeFiles[afi].Name,
                                                activeFiles[afi].Preview,
                                                "Create folder '" + newDirectory + "' failed: " + ex.Message);
                            continue;
                        }
                    }
                    result.RenameToSubfolders = true;
                }


                // rename/move/copy, catch any errors

                try
                {
                    if (RenameFolders)
                    {
                        Directory.Move(activeFiles[afi].Fullpath, newFullpath);
                    }
                    else
                    {
                        if (itmOutputRenameInPlace.Checked || itmOutputMoveTo.Checked)
                            File.Move(activeFiles[afi].Fullpath, newFullpath);
                        else if (itmOutputCopyTo.Checked)
                            File.Copy(activeFiles[afi].Fullpath, newFullpath);
                        else  // backup to
                        {
                            File.Copy(activeFiles[afi].Fullpath, Path.Combine(fbdMoveCopy.SelectedPath, Path.GetFileName(activeFiles[afi].Filename)));
                            File.Move(activeFiles[afi].Fullpath, newFullpath);
                        }
                    }

                    result.ReportSuccess();
                }
                catch (Exception ex)
                {
                    result.ReportError(activeFiles[afi].Name, activeFiles[afi].Preview, ex.Message);
                    continue;
                }

            }  // end rename loop


            bw.ReportProgress(100);
            e.Result = result;
        }
        private void bgwRename_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
        private void bgwRename_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // re-throw exception if one occured during rename

            if (e.Error != null)
                throw e.Error;


            // get result

            RenameResult result = (RenameResult)e.Result;


            // if necessary, refresh nodes in folder tree

            if (RenameFolders)
            {
                tvwFolders.RefreshNode(tvwFolders.SelectedNode);
                if (itmOutputMoveTo.Checked && !fbdMoveCopy.SelectedPath.StartsWith(activePath))
                    tvwFolders.RefreshNode(fbdMoveCopy.SelectedPath);
            }
            else if (result.RenameToSubfolders)
            {
                if (itmOutputRenameInPlace.Checked || itmOutputBackupTo.Checked)
                    tvwFolders.RefreshNode(tvwFolders.SelectedNode);
                else  // Move to, Copy to
                    tvwFolders.RefreshNode(fbdMoveCopy.SelectedPath);
            }


            // update stats

            countFilesRenamed += result.FilesRenamed;


            // swap rename/cancel buttons

            btnCancel.Visible = false;
            btnRename.Visible = true;
            btnCancel.Enabled = false;
            btnRename.Enabled = true;
            btnCancel.Text = "&Cancel";  // reset text


            // hide progress bar

            progressBar.Visible = false;
            tsOptions.Visible = lblNumMatched.Visible = lblNumConflict.Visible = true;
            UnFocusAll();


            // show error dialog if any errors occured

            if (result.AnyErrors)
                result.ShowErrorDialog(strFile);


            if (!result.AnyErrors && !result.Cancelled)
            {
                // save regex to history

                string regexString = (string)cmbMatch.Tag;

                if (cmbMatch.Items.Contains(regexString))
                    cmbMatch.Items.Remove(regexString);

                cmbMatch.Items.Insert(0, regexString);

                while (cmbMatch.Items.Count > MAX_HISTORY)
                    cmbMatch.Items.RemoveAt(cmbMatch.Items.Count - 1);

                SaveRegexHistory();


                // reset fields

                //ResetFields();
            }


            // reactivate form & refresh filelist

            SetFormActive(true);
            UpdateFileList();
            cmbMatch.Focus();
        }
    }
}
