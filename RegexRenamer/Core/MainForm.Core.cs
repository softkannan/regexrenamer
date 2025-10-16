using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using RegexRenamer.FindReplace;
using RegexRenamer.Kavita;
using RegexRenamer.Native;
using RegexRenamer.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer;

public partial class MainForm
{
    private int MAX_VIEW_PAGE_SIZE = 200;
    private int MAX_FILES = 10000;   // file limit for filelist (was a const)

    public void InitializeCore()
    {
        cbFilePaging.SelectedIndexChanged += cbFilePaging_SelectedIndexChanged;

        bgwRename.DoWork += bgwRename_DoWork;
        bgwRename.ProgressChanged += bgwRename_ProgressChanged;
        bgwRename.RunWorkerCompleted += bgwRename_RunWorkerCompleted;
    }

    // update directory tree/filenames/previews/validation (each cascades into the one below)
    private void cbFilePaging_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void UpdateFileList()
    {
        if (!EnableUpdates) return;

        dgvFiles.Tag = 0;  // reset files ignored
        _fileCount.Reset();

        // update txtPath
        txtPath.Text = ActivePath;
        txtPath.Update();

        // if invalid selection, clear all
        if (string.IsNullOrEmpty(ActivePath))
        {
            _activeFiles.Clear();
            _inactiveFiles.Clear();
            dgvFiles.Rows.Clear();
            lblNumMatched.Text = "0";
            lblNumConflict.Text = "0";
            UpdateFileStats();

            return;
        }

        this.Cursor = Cursors.AppStarting;
        // create filter regex, if necessary
        Regex filter = _activeFilter.CreateGlobFilter(rbFilterGlob.Checked);

        // loop through file list, build RRItem array
        _activeFiles.Clear();
        _inactiveFiles.Clear();
        if (RenameFolders)  // folders
        {
            BuildFoldersList(filter);
        }
        else // files
        {
            BuildFileList(filter);
        }

        // create datagridview items w/ filename
        dgvFiles.Rows.Clear();

        for (int i = 0; i < _activeFiles.Count; i++)
        {
            if (i >= MAX_FILES)  // reached limit
            {
                dgvFiles.Tag = _activeFiles.Count - i;  // num files ignored, will display warning dialog after preview is updated
                _activeFiles.RemoveRange(i, _activeFiles.Count - MAX_FILES);
                break;
            }

            // add new item
            dgvFiles.Rows.Add(null, _activeFiles[i].Name, null);
            dgvFiles.Rows[i].Tag = i;  // store activeFiles index so we can refer back when under different sorting

            // add image (keyed by extension)
#if !DEBUG
    try  
    {
#endif
            if (RenameFolders)
            {
                dgvFiles.Rows[i].Cells[0].Value = FileIconAPI.GetDefaultFolderIcon(false);
            }
            else
            {
                string ext = _activeFiles[i].Extension.ToLower();
                if (ext == ".lnk")  // shortcut, don't key by extension as each may have different icon
                {
                    var icon = FileIconAPI.GetIcon(_activeFiles[i].Fullpath, false);
                    dgvFiles.Rows[i].Cells[0].Value = icon;
                }
                else  // non-shortcut
                {
                    var icon = FileIconAPI.GetIcon(_activeFiles[i].Fullpath, false);
                    dgvFiles.Rows[i].Cells[0].Value = icon;
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

        _fileCount.shown = dgvFiles.Rows.Count;
        UpdateFileStats();
        UpdateSelection();
        UpdatePreview();
    }

    private void BuildFileList(Regex filter)
    {
        DirectoryInfo activeDir = new DirectoryInfo(ActivePath);
        FileInfo[] files = new FileInfo[0];
        try
        {
            files = activeDir.GetFiles();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //TODO: fill the paging
        int pageEnd = files.Length;
        int pageStart = 0;
        for (int idx = pageStart; idx < pageEnd; idx++)
        {
            FileInfo file = files[idx] as FileInfo;
            _fileCount.total++;

            // ignore if filtered out
            if (filter != null && filter.IsMatch(file.Name) == cbFilterExclude.Checked)
            {
                if (!_inactiveFiles.ContainsKey(file.Name.ToLower()))
                    _inactiveFiles.Add(file.Name.ToLower(), InactiveReason.Filtered);
                _fileCount.filtered++;
                continue;
            }

            // ignore if hidden and not showing hidden files
            bool hidden = false;
            try
            {
                hidden = (file.Attributes & FileAttributes.Hidden) != 0;
            }
            catch { }  // reported System.UnauthorizedAccessException here under some versions of Samba when item is a link to /dev/null

            if (hidden) _fileCount.hidden++;
            if (!itmOptionsShowHidden.Checked && hidden)
            {
                if (!_inactiveFiles.ContainsKey(file.Name.ToLower()))
                    _inactiveFiles.Add(file.Name.ToLower(), InactiveReason.Hidden);
                continue;
            }

            _activeFiles.Add(new RRItem(file, hidden, itmOptionsPreserveExt.Checked));
        }
    }

    private void BuildFoldersList(Regex filter)
    {
        DirectoryInfo activeDir = new DirectoryInfo(ActivePath);
        DirectoryInfo[] dirs = new DirectoryInfo[0];
        try
        {
            dirs = activeDir.GetDirectories();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //TODO: fill the paging
        int pageEnd = dirs.Length;
        int pageStart = 0;
        for (int idx = pageStart; idx < pageEnd; idx++)
        {
            DirectoryInfo dir = dirs[idx] as DirectoryInfo;
            _fileCount.total++;

            // ignore if filtered out
            if (filter != null && filter.IsMatch(dir.Name) == cbFilterExclude.Checked)
            {
                if (!_inactiveFiles.ContainsKey(dir.Name.ToLower()))
                    _inactiveFiles.Add(dir.Name.ToLower(), InactiveReason.Filtered);
                _fileCount.filtered++;
                continue;
            }

            // ignore if hidden and not showing hidden files
            bool hidden = false;
            try
            {
                hidden = (dir.Attributes & FileAttributes.Hidden) != 0;
            }
            catch { }  // reported System.UnauthorizedAccessException here under some versions of Samba when item is a link to /dev/null

            if (hidden) _fileCount.hidden++;
            if (!itmOptionsShowHidden.Checked && hidden)
            {
                if (!_inactiveFiles.ContainsKey(dir.Name.ToLower()))
                    _inactiveFiles.Add(dir.Name.ToLower(), InactiveReason.Hidden);
                continue;
            }

            _activeFiles.Add(new RRItem(dir, hidden, itmOptionsPreserveExt.Checked));
        }
    }

    private string MatchEvalChangeCase(Match match)
    {
        TextInfo ti = new CultureInfo("ta").TextInfo;

        if (itmChangeCaseUppercase.Checked) return ti.ToUpper(match.Groups[1].Value);
        else if (itmChangeCaseLowercase.Checked) return ti.ToLower(match.Groups[1].Value);
        else if (itmChangeCaseTitlecase.Checked) return ti.ToTitleCase(match.Groups[1].Value.ToLower());
        else if (itmChangeCaseCleanName.Checked) return match.Groups[1].Value.ToCleanFileName();
        else return match.Groups[1].Value;
    }

    private void UpdatePreview()
    {
        if (!EnableUpdates || !_validMatch) return;

        this.Cursor = Cursors.AppStarting;

        string matchingPattern = cmbMatch.Text;
        string userReplacePattern = cmbReplace.Text;

        // Starting number (or letter)
        string numberingStart = txtNumberingStart.Text;
        // Increment by x each file (may be negative)
        string numberingIncStep = txtNumberingInc.Text;
        // Reset to starting number every x files
        string numberingReset = txtNumberingReset.Text;
        //Eg: \"0000\" means 14 => 0014
        string numberingPad = txtNumberingPad.Text;

        var noChangeCase = itmChangeCaseNoChange.Checked;
        var showKavita = noneToolStripMenuItem.Checked == false;

        const string rxDoller = @"(?<=(?:^|[^$])(?:\$\$)*)\$";  // regex for an actual (non-escaped) doller sign

        // generate preview
        if (!string.IsNullOrWhiteSpace(matchingPattern))
        {
            var (options, count) = GetRegexOptions();
            
            //main regex
            Regex regex = new Regex(matchingPattern, options);

            // auto numbering
            int numCurrent = 0, numIncStep = 0, numStart = 0, numReset = 0;
            string numFormatted = "";
            bool doingAutoNum = false;
            bool doingAutoNumLetter = false;  // number sequence is actually a-z letter sequence
            bool doingAutoNumLetterUpper = false;  // letter sequence is uppercase

            if (this._validNumber && Regex.IsMatch(userReplacePattern, rxDoller + "#"))
                doingAutoNum = true;

            Match match = Regex.Match(numberingStart, @"^(([a-z]+)|([A-Z]+))$");
            doingAutoNumLetter = match.Success;
            doingAutoNumLetterUpper = match.Success && match.Groups[3].Length > 0;

            // validate numbering inputs and parse to int
            if (doingAutoNum)
            {
                // if starting is letter sequence, then increment step and reset must be 1
                if (doingAutoNumLetter)
                    numStart = SequenceLetterToNumber(numberingStart.ToLower());
                else
                    numStart = Int32.Parse(numberingStart);

                numIncStep = Int32.Parse(numberingIncStep);
                // Reset to starting number every x files
                numReset = Int32.Parse(numberingReset);
            }
            // backup one step so first file is correct after initial increment
            numCurrent = numStart - numIncStep;  // back up one

            // 
            if (doingAutoNum)
            {
                userReplacePattern = Regex.Replace(userReplacePattern, rxDoller + @"(\d+)" + rxDoller + "#", "$${$1}$$#");
            }

            for (int afi = 0; afi < _activeFiles.Count; afi++)
            {
                // check if matches
                _activeFiles[afi].ComicInfo = null;
                _activeFiles[afi].ParseInfo = null;
                _activeFiles[afi].Matched = regex.IsMatch(_activeFiles[afi].Name);

                // if not, bail early, don't incrememnt autonum
                if (!_activeFiles[afi].Matched)
                {
                    _activeFiles[afi].Preview = _activeFiles[afi].Name;
                    continue;
                }

                // increment autonum and replace numbering pattern with current number
                string replacePattern;
                if (doingAutoNum)
                {
                    // increment number with current step
                    numCurrent += numIncStep;

                    // Apply number reset logic
                    if (numReset != 0 && (numCurrent - numStart) % numReset == 0)
                        numCurrent = numStart;

                    if (numFormatted != "$#")  // basic int overflow & negative number detection
                    {
                        if (!doingAutoNumLetter)  // number sequence
                        {
                            if (numCurrent < 0)
                                numFormatted = "$#";
                            else
                                numFormatted = numCurrent.ToString(numberingPad);
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

                // enclose the replace pattern with \n so we can do Title-case 
                if (!noChangeCase)
                    replacePattern = "\n" + replacePattern + "\n";  // delimit change-case boundaries

                // do replace and store preview
                _activeFiles[afi].Preview = regex.Replace(_activeFiles[afi].Name, replacePattern, count);

                // if change case selected, then do it now, looks for all \n delimited sections,
                if (!noChangeCase)
                    _activeFiles[afi].Preview = Regex.Replace(_activeFiles[afi].Preview, @"\n([^\n]*)\n", new MatchEvaluator(MatchEvalChangeCase));

                // if kavita preview is selected, then get the kavita parse info and attach to the item
                if (showKavita)
                {
                    var curKavitaRoot = setAsKavitaLibraryRootFolderViewToolStripMenuItem.Tag as string;
                    curKavitaRoot ??= Directory.GetDirectoryRoot(_activeFiles[afi].Fullpath);

                    UpdateKavitaCheck(_activeFiles[afi], curKavitaRoot, _curLibType);
                }

                // if preview is empty, use original name
                if (_activeFiles[afi].Preview.Length == 0)
                    _activeFiles[afi].Preview = _activeFiles[afi].Name;
            }

        }
        else  // cmbMatch.Text == ""
        {
            foreach (RRItem file in _activeFiles)
            {
                file.Preview = file.Name;
                file.Matched = false;
            }
        }

        // update file list
        for (int dfi = 0; dfi < dgvFiles.Rows.Count; dfi++)
        {
            int afi = (int)dgvFiles.Rows[dfi].Tag;
            dgvFiles.Rows[dfi].Cells[2].Value = _activeFiles[afi].Preview;
            //showSizeToolStripMenuItem
            if (_activeFiles[afi].ParseInfo != null)
            {
                dgvFiles.Rows[dfi].Cells[3].Value = _activeFiles[afi].ParseInfo.Title;
                dgvFiles.Rows[dfi].Cells[4].Value = _activeFiles[afi].ParseInfo.Series;
                dgvFiles.Rows[dfi].Cells[5].Value = _activeFiles[afi].ParseInfo.Volumes;
                dgvFiles.Rows[dfi].Cells[6].Value = _activeFiles[afi].ParseInfo.Chapters;
                dgvFiles.Rows[dfi].Cells[7].Value = _activeFiles[afi].ParseInfo.Edition;
                dgvFiles.Rows[dfi].Cells[8].Value = _activeFiles[afi].ParseInfo.IsSpecial ? "true" : "false";
            }
        }

        // do preview filename validation
        UpdateValidation();

        // redraw
        dgvFiles.Sort(this.dgvFiles.SortedColumn ?? this.colFilename,
                       dgvFiles.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending);  // resort
        
        this.Cursor = Cursors.Default;

        // show warning if any ignored files
        int fileNum = 0;
        if(dgvFiles.Tag != null) { fileNum = (int)dgvFiles.Tag; }
        if (fileNum > 0)
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

    private Tuple<RegexOptions,int> GetRegexOptions()
    {
        RegexOptions options;
        int count;
        // compile regex
        options = RegexOptions.None | RegexOptions.Compiled;
        count = cbModifierG.Checked ? -1 : 1;
        if (cbModifierI.Checked) { options |= RegexOptions.IgnoreCase; }
        if (cbModifierX.Checked) { options |= RegexOptions.IgnorePatternWhitespace; }

        return Tuple.Create(options, count);
    }

    private void UpdateSelection()
    {
        if (dgvFiles.Rows.Count != this._activeFiles.Count)
            return;

        RRItem firstSelection = null;
        foreach (DataGridViewRow row in dgvFiles.Rows)
        {
            if (row.Tag == null) continue;

            int afi = (int)row.Tag;
            this._activeFiles[afi].Selected = row.Selected;
            if(firstSelection == null && this._activeFiles[afi].Selected)
            {
                firstSelection = this._activeFiles[afi];
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

        string outputPath = ActivePath;
        if (itmOutputMoveTo.Checked || itmOutputCopyTo.Checked)
            outputPath = fbdMoveCopy.SelectedPath;


        for (int afi = 0; afi < _activeFiles.Count; afi++)
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
                if (_activeFiles[afi].Name == _activeFiles[afi].Preview) continue;
            }
            else
            {
                if (!_activeFiles[afi].Matched) continue;
            }

            if (itmOptionsRenameSelectedRows.Checked)
            {
                if (!_activeFiles[afi].Selected) continue;
            }


            // update progressbar
            bw.ReportProgress((int)((filesRenamed / filesToRename) * 100));
            filesRenamed++;


            // get new fullpath
            string newFullpath = Path.Combine(outputPath, _activeFiles[afi].Preview);
            if (itmOptionsPreserveExt.Checked)
                newFullpath += _activeFiles[afi].Extension;


            // create subdirs (if any)
            if (_activeFiles[afi].Preview.Contains("\\"))
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
                        result.ReportError(_activeFiles[afi].Name,
                                            _activeFiles[afi].Preview,
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
                    Directory.Move(_activeFiles[afi].Fullpath, newFullpath);
                }
                else
                {
                    if (itmOutputRenameInPlace.Checked || itmOutputMoveTo.Checked)
                        File.Move(_activeFiles[afi].Fullpath, newFullpath);
                    else if (itmOutputCopyTo.Checked)
                        File.Copy(_activeFiles[afi].Fullpath, newFullpath);
                    else  // backup to
                    {
                        File.Copy(_activeFiles[afi].Fullpath, Path.Combine(fbdMoveCopy.SelectedPath, Path.GetFileName(_activeFiles[afi].Filename)));
                        File.Move(_activeFiles[afi].Fullpath, newFullpath);
                    }
                }

                result.ReportSuccess();
            }
            catch (Exception ex)
            {
                result.ReportError(_activeFiles[afi].Name, _activeFiles[afi].Preview, ex.Message);
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
            if (itmOutputMoveTo.Checked && !fbdMoveCopy.SelectedPath.StartsWith(ActivePath))
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
        _countFilesRenamed += result.FilesRenamed;


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
            string regexMatch = (string)cmbMatch.Tag;
            cmbMatch.AddUniqueItem(regexMatch);
            string regexReplace = (string)cmbReplace.Tag;
            cmbReplace.AddUniqueItem(regexReplace);
            SaveRegexHistory();
        }


        // reactivate form & refresh filelist
        SetFormActive(true);
        UpdateFileList();
        cmbMatch.Focus();
    }
}
