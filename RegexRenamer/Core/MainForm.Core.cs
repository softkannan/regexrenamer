using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using Kavita;
using RegexRenamer.Native;
using RegexRenamer.Rename;
using RegexRenamer.Tools.FindReplace;
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

    public void InitializeCore()
    {
        bgwRename.DoWork += bgwRename_DoWork;
        bgwRename.ProgressChanged += bgwRename_ProgressChanged;
        bgwRename.RunWorkerCompleted += bgwRename_RunWorkerCompleted;
    }

    // update directory tree/filenames/previews/validation (each cascades into the one below)
    private void UpdateFileList()
    {
        if (!EnableUpdates) return;

        dgvFiles.Tag = 0;  // reset files ignored
        dgvFiles.Rows.Clear();
        _fileViewIconCache.ClearDynamicIcons();

        // update txtPath
        txtPath.Text = _activePath;
        txtPath.Update();

        // if invalid selection, clear all
        if (string.IsNullOrEmpty(_activePath))
        {
            _fileStore = new FilesStore();
            lblNumMatched.Text = "0";
            lblNumConflict.Text = "0";
            UpdateFileStats();
            return;
        }

        this.Cursor = Cursors.AppStarting;

        GlobInfo globInfo = new GlobInfo(_activePath, _activeFilter, cbFilterExclude.Checked, itmOptionsShowHidden.Checked, itmOptionsPreserveExt.Checked, rbFilterGlob.Checked);
        _fileStore = new FilesStore(globInfo, RenameFolders);

        // create datagridview items w/ filename
        for (int idx = 0; idx < _fileStore.Files.Count; idx++)
        {
            if (idx >= FilesStore.MAX_FILES)  // reached limit
            {
                dgvFiles.Tag = _fileStore.Files.Count - idx;  // num files ignored, will display warning dialog after preview is updated
                _fileStore.TrimFiles(idx, _fileStore.Files.Count - FilesStore.MAX_FILES);
                break;
            }

            // add new item
            dgvFiles.Rows.Add(null, _fileStore.Files[idx].Name, null);
            dgvFiles.Rows[idx].Tag = idx;  // store activeFiles index so we can refer back when under different sorting

            // add image (keyed by extension)
#if !DEBUG
    try  
    {
#endif
            // Update the icons, for folders, always use folder icon
            dgvFiles.Rows[idx].Cells[0].Value = _fileViewIconCache.GetIcon(_fileStore.Files[idx]);

#if !DEBUG
    }
    catch  // default: no image
    {
      dgvFiles.Rows[idx].Cells[0].Value = new Bitmap( 1, 1 );
    }
#endif
        }

        _fileStore.Stats.SetShown(dgvFiles.Rows.Count);
        UpdateFileStats();
        UpdateSelection();
        UpdatePreview();
    }

    private ChangeCaseOption GetChangeCaseInfo()
    {
        if (itmChangeCaseNoChange.Checked) return ChangeCaseOption.NoChange;
        else if (itmChangeCaseUppercase.Checked) return ChangeCaseOption.Uppercase;
        else if (itmChangeCaseLowercase.Checked) return ChangeCaseOption.Lowercase;
        else if (itmChangeCaseTitlecase.Checked) return ChangeCaseOption.Titlecase;
        else if (itmChangeCaseCleanName.Checked) return ChangeCaseOption.CleanName;
        else return ChangeCaseOption.NoChange;
    }

    private void UpdatePreview()
    {
        if (!EnableUpdates || !_validMatch) return;

        this.Cursor = Cursors.AppStarting;

        // run the match and build the preview using replace pattern
        string matchingPattern = cmbMatch.Text;
        string userReplacePattern = cmbReplace.Text;

        AutoNumberingInfo numInfo = new AutoNumberingInfo()
        {
            ValidNumber = _validNumber,
            NumberingStart = txtNumberingStart.Text,
            NumberingIncStep = txtNumberingInc.Text,
            NumberingReset = txtNumberingReset.Text,
            NumberingPad = txtNumberingPad.Text
        };

        KavitaInfo kavitaInfo = new KavitaInfo()
        {
            ShowPreview = noneKavitaMenuItem.Checked == false,
            KavitaRoot = _kavitaLibRootpath,
            KavitaLibType = _kavitaPreviewLibType,
            UseMetadata = _kavitaUseMetadata
        };

        ChangeCaseOption changeCaseOption = GetChangeCaseInfo();

        RegexModifierInfo modifierInfo = new RegexModifierInfo()
        {
            IgnoreCase = cbModifierI.Checked,
            ReplaceEveryMatch = cbModifierG.Checked,
            IgnorePatternWhitespace = cbModifierX.Checked
        };

        _fileStore.BuildPreview(cmbMatch.Text, cmbReplace.Text, numInfo,changeCaseOption, kavitaInfo,modifierInfo);

        // write the preview data to the datagridview
        WriteToDataGrid(_fileStore.Files);

        // do preview filename validation
        UpdateValidation();

        // redraw
        dgvFiles.Sort(this.dgvFiles.SortedColumn ?? this.colFilename,
                       dgvFiles.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending);  // resort

        this.Cursor = Cursors.Default;

        // show warning if any ignored files
        int fileNum = 0;
        if (dgvFiles.Tag != null) { fileNum = (int)dgvFiles.Tag; }
        if (fileNum > 0)
        {
            MessageBox.Show("For performance reasons, RegexRenamer will only display " + FilesStore.MAX_FILES
                           + " " + strFile + "s at once (" + (int)dgvFiles.Tag + " " + strFile + "s ignored).\r\n"
                           + "Use a filter to display only the " + strFile + "s you need to rename.",
                             "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            dgvFiles.Tag = 0;  // prevent re-display
        }

        // keep selection cleared
        if (!_renameSelectionOnly)
            dgvFiles.ClearSelection();

        PreviewNeedsUpdate = false;
    }
    

    private void WriteToDataGrid(IReadOnlyList<RenameItemInfo> listOfItems)
    {
        // update file list
        for (int dfi = 0; dfi < dgvFiles.Rows.Count; dfi++)
        {
            // column 0 is file icon, column 1 is filename and column 2 is preview filename
            int colStart = 2;  
            int afi = (int)dgvFiles.Rows[dfi].Tag;
            // column 0 is file icon, column 1 is filename and column 2 is preview filename
            dgvFiles.Rows[dfi].Cells[colStart++].Value = listOfItems[afi].Preview;

            colStart = 3;  // reset to column 3 for extra info
            if (chkShowInfo.Checked)
            {
                dgvFiles.Rows[dfi].Cells[colStart++].Value = listOfItems[afi].Extension;
                dgvFiles.Rows[dfi].Cells[colStart++].Value = listOfItems[afi].Size;
                dgvFiles.Rows[dfi].Cells[colStart++].Value = listOfItems[afi].FileModified;
            }

            colStart = 6; //reset to column 6 for kavita info
            if (listOfItems[afi].ParseInfo != null)
            {
                dgvFiles.Rows[dfi].Cells[colStart++].Value = listOfItems[afi].ParseInfo.Title;
                dgvFiles.Rows[dfi].Cells[colStart++].Value = listOfItems[afi].ParseInfo.Series;
                dgvFiles.Rows[dfi].Cells[colStart++].Value = listOfItems[afi].ParseInfo.Volumes;
                dgvFiles.Rows[dfi].Cells[colStart++].Value = listOfItems[afi].ParseInfo.Chapters;
                dgvFiles.Rows[dfi].Cells[colStart++].Value = listOfItems[afi].ParseInfo.Edition;
                dgvFiles.Rows[dfi].Cells[colStart++].Value = listOfItems[afi].ParseInfo.IsSpecial ? "true" : "false";
            }
        }
    }

    private void UpdateSelection()
    {
        if (dgvFiles.Rows.Count != this._fileStore.Files.Count)
            return;

        RenameItemInfo firstSelection = null;
        foreach (DataGridViewRow row in dgvFiles.Rows)
        {
            if (row.Tag == null) continue;

            int afi = (int)row.Tag;
            this._fileStore.Files[afi].Selected = row.Selected;
            if(firstSelection == null && this._fileStore.Files[afi].Selected)
            {
                firstSelection = this._fileStore.Files[afi];
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

        string outputPath = _activePath;
        if (itmOutputMoveTo.Checked || itmOutputCopyTo.Checked)
            outputPath = fbdMoveCopy.SelectedPath;


        for (int afi = 0; afi < _fileStore.Files.Count; afi++)
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
                if (_fileStore.Files[afi].Name == _fileStore.Files[afi].Preview) continue;
            }
            else
            {
                if (!_fileStore.Files[afi].Matched) continue;
            }

            if (_renameSelectionOnly)
            {
                if (!_fileStore.Files[afi].Selected) continue;
            }


            // update progressbar
            bw.ReportProgress((int)((filesRenamed / filesToRename) * 100));
            filesRenamed++;


            // get new fullpath
            string newFullpath = Path.Combine(outputPath, _fileStore.Files[afi].Preview);
            if (itmOptionsPreserveExt.Checked)
                newFullpath += _fileStore.Files[afi].Extension;


            // create subdirs (if any)
            if (_fileStore.Files[afi].Preview.Contains("\\"))
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
                        result.ReportError(_fileStore.Files[afi].Name,
                                            _fileStore.Files[afi].Preview,
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
                    Directory.Move(_fileStore.Files[afi].Fullpath, newFullpath);
                }
                else
                {
                    if (itmOutputRenameInPlace.Checked || itmOutputMoveTo.Checked)
                        File.Move(_fileStore.Files[afi].Fullpath, newFullpath);
                    else if (itmOutputCopyTo.Checked)
                        File.Copy(_fileStore.Files[afi].Fullpath, newFullpath);
                    else  // backup to
                    {
                        File.Copy(_fileStore.Files[afi].Fullpath, Path.Combine(fbdMoveCopy.SelectedPath, Path.GetFileName(_fileStore.Files[afi].Filename)));
                        File.Move(_fileStore.Files[afi].Fullpath, newFullpath);
                    }
                }

                result.ReportSuccess();
            }
            catch (Exception ex)
            {
                result.ReportError(_fileStore.Files[afi].Name, _fileStore.Files[afi].Preview, ex.Message);
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
            if (itmOutputMoveTo.Checked && !fbdMoveCopy.SelectedPath.StartsWith(_activePath))
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
