using Kavita;
using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using RegexRenamer.Models;
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
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace RegexRenamer;

/// <summary>Specifies which UI refresh stages to execute.</summary>
[Flags]
public enum UpdateStage
{
    None = 0,
    FileList = 1 << 0,
    Preview = 1 << 1,
    Validation = 1 << 2,
    Selection = 1 << 3,

    /// <summary>Full cascade: FileList → Preview → Validation → Selection.</summary>
    All = FileList | Preview | Validation | Selection,
}

public partial class MainForm
{
    private int MAX_VIEW_PAGE_SIZE = 200;

    public void InitializeCore()
    {
        bgwRename.DoWork += bgwRename_DoWork;
        bgwRename.ProgressChanged += bgwRename_ProgressChanged;
        bgwRename.RunWorkerCompleted += bgwRename_RunWorkerCompleted;
    }

    /// <summary>
    /// Unified refresh entry point. Executes the requested stages in cascade order.
    /// Stages that depend on earlier ones (e.g., Preview depends on FileList) are
    /// automatically included when a higher-level stage is requested.
    /// </summary>
    private void RefreshView(UpdateStage stages)
    {
        if (!EnableUpdates) return;

        if (stages.HasFlag(UpdateStage.FileList))
        {
            UpdateFileList();
            return; // UpdateFileList already cascades into Preview → Validation → Selection
        }

        if (stages.HasFlag(UpdateStage.Preview))
        {
            UpdatePreview();
            return; // UpdatePreview already cascades into Validation
        }

        if (stages.HasFlag(UpdateStage.Validation))
            UpdateValidation();

        if (stages.HasFlag(UpdateStage.Selection))
            UpdateSelection();
    }

    // update directory tree/filenames/previews/validation (each cascades into the one below)
    private void UpdateFileList()
    {
        if (!EnableUpdates) return;

        // Snapshot current user input for business logic
        _currentInput = GetUserInput();

        dgvFiles.Tag = 0;  // reset files ignored
        dgvFiles.RowCount = 0;
        _fileViewRows.Clear();
        _fileViewIconCache.ClearDynamicIcons();

        // update txtPath
        txtPath.Text = _currentInput.ActivePath;
        txtPath.Update();

        // if invalid selection, clear all
        if (string.IsNullOrEmpty(_currentInput.ActivePath))
        {
            _fileStore = new FilesStore();
            lblNumMatched.Text = "0";
            lblNumConflict.Text = "0";
            UpdateFileStats();
            return;
        }

        this.Cursor = Cursors.AppStarting;

        // build file list based on current path and filter
        GlobInfo globInfo = new GlobInfo(_currentInput.ActivePath, _activeFilter, _currentInput.FilterExclude, _currentInput.ShowHiddenFiles, _currentInput.PreserveExtension, _currentInput.FilterIsGlob);
        _fileStore = new FilesStore(globInfo, _currentInput.RenameFolders, _currentInput.IncludeSubfolders);

        // create datagridview items w/ filename
        int expectedCount = Math.Min(_fileStore.Files.Count, FilesStore.MAX_FILES);
        if (_fileStore.Files.Count > FilesStore.MAX_FILES)
        {
            dgvFiles.Tag = _fileStore.Files.Count - FilesStore.MAX_FILES;  // num files ignored, will display warning dialog after preview is updated)
            _fileStore.TrimFiles(FilesStore.MAX_FILES, _fileStore.Files.Count - FilesStore.MAX_FILES);
        }
        var list = new List<Models.FileViewRowData>(expectedCount);
        for (int idx = 0; idx < expectedCount; idx++)
        {
            list.Add(new Models.FileViewRowData(idx));
        }

        _fileViewRows = list;
        dgvFiles.RowCount = _fileViewRows.Count;

        _fileStore.Stats.SetShown(_fileViewRows.Count);
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

        // Snapshot current user input for business logic
        _currentInput = GetUserInput();

        this.Cursor = Cursors.AppStarting;

        _fileStore.BuildPreview(_currentInput.MatchPattern, _currentInput.ReplacePattern, _currentInput.Numbering, _currentInput.ChangeCase, _currentInput.Kavita, _currentInput.Modifiers);

        // write the preview data to the datagridview
        //WriteToDataGrid(_fileStore.Files);

        // do preview filename validation
        UpdateValidation();

        // redraw
        SortFileViewRows(dgvFiles.SortedColumn ?? colFilename,
                         dgvFiles.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending);

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
        if (!_currentInput.RenameSelectionOnly)
            dgvFiles.ClearSelection();

        PreviewNeedsUpdate = false;
    }
    
    private object GetCellValue(FileViewRowData rowData, int columnIndex, bool forDisplay = true)
    {
        if (rowData == null)
            return null;
        int afi = rowData.FileStoreIndex;
        switch (columnIndex)
        {
            case 0:
                {
                    if (rowData.FileIcon == null)
                    {
#if !DEBUG
                        try
                        {
#endif
                            // add image (keyed by extension)
                            rowData.FileIcon = _fileViewIconCache.GetIcon(_fileStore.Files[afi]);
#if !DEBUG
                        }
                        catch  // default: no image
                        {
                            rowData.FileIcon = null;
                        }
#endif
                    }
                    return rowData.FileIcon;
                }
            case 1:
                return _fileStore.Files[afi].Name;
            case 2:
                return _fileStore.Files[afi].Context.Preview;
            case 3:
                return chkShowInfo.Checked ? _fileStore.Files[afi].Extension : null;
            case 4:
                return chkShowInfo.Checked ? _fileStore.Files[afi].Size : null;
            case 5:
                return chkShowInfo.Checked ? _fileStore.Files[afi].FileModified : null;
            case 6:
                return _fileStore.Files[afi].Context.ParseInfo != null ? _fileStore.Files[afi].Context.ParseInfo.Title : null;
            case 7:
                return _fileStore.Files[afi].Context.ParseInfo != null ? _fileStore.Files[afi].Context.ParseInfo.Series : null;
            case 8:
                return _fileStore.Files[afi].Context.ParseInfo != null ? _fileStore.Files[afi].Context.ParseInfo.Volumes : null;
            case 9:
                return _fileStore.Files[afi].Context.ParseInfo != null ? _fileStore.Files[afi].Context.ParseInfo.Chapters : null;
            case 10:
                return _fileStore.Files[afi].Context.ParseInfo != null ? _fileStore.Files[afi].Context.ParseInfo.Edition : null;
            case 11:
                return _fileStore.Files[afi].Context.ParseInfo != null ? (_fileStore.Files[afi].Context.ParseInfo.IsSpecial ? "true" : "false") : null;
            default:
                return null;
        }
    }

    private void UpdateSelection()
    {
        if (_fileViewRows.Count != this._fileStore.Files.Count)
            return;

        // Bulk-clear all selected flags first
        for (int i = 0; i < _fileStore.Files.Count; i++)
            _fileStore.Files[i].Context.Selected = false;

        // Only iterate actually-selected rows (typically very few)
        RenameItemInfo firstSelection = null;
        foreach (DataGridViewRow row in dgvFiles.SelectedRows)
        {
            if (row.Index < 0 || row.Index >= _fileViewRows.Count) continue;

            int afi = _fileViewRows[row.Index].FileStoreIndex;
            _fileStore.Files[afi].Context.Selected = true;
            firstSelection ??= _fileStore.Files[afi];
        }

        if (firstSelection != null)
            UpdateFileInfo(firstSelection);
    }
    

    // background worker
    private void bgwRename_DoWork(object sender, DoWorkEventArgs e)
    {
        BackgroundWorker bw = sender as BackgroundWorker;
        int filesToRename = (int)e.Argument;

        e.Result = _renameService.Execute(
            _fileStore.Files,
            _currentInput,
            filesToRename,
            progress => bw.ReportProgress(progress),
            () => bw.CancellationPending);
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
        if (_currentInput.RenameFolders)
        {
            tvwFolders.RefreshNode(tvwFolders.SelectedNode);
            if (_currentInput.Output == OutputMode.MoveTo && !_currentInput.MoveCopyPath.StartsWith(_currentInput.ActivePath))
                tvwFolders.RefreshNode(_currentInput.MoveCopyPath);
        }
        else if (result.RenameToSubfolders)
        {
            if (_currentInput.Output == OutputMode.RenameInPlace || _currentInput.Output == OutputMode.BackupTo)
                tvwFolders.RefreshNode(tvwFolders.SelectedNode);
            else  // Move to, Copy to
                tvwFolders.RefreshNode(_currentInput.MoveCopyPath);
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
        RefreshView(UpdateStage.FileList);
        cmbMatch.Focus();
    }
}
