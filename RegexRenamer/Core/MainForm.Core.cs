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
    None,
    FileList = 1 << 0,
    Preview = 1 << 1,
    Validation = 1 << 2,
    Selection = 1 << 3,
    FullRefresh = 1 << 4,
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

    private void BuildFileViewRows()
    {
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
            list.Add(new Models.FileViewRowData(_fileStore.Files[idx], idx, idx));
        }
        _fileViewRows = list;
        dgvFiles.RowCount = _fileViewRows.Count;
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

    /// <summary>
    /// Unified refresh entry point. Executes the requested stages in cascade order.
    /// Stages that depend on earlier ones (e.g., Preview depends on FileList) are
    /// automatically included when a higher-level stage is requested.
    /// </summary>
    private void RefreshFileListView(UpdateStage stages)
    {
        if (!EnableUpdates) return;

        if(stages.HasFlag(UpdateStage.FullRefresh))
        {
            FullRefresh();
            return; // FullRefresh already cascades into FileList → Preview → Validation → Selection
        }

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

    private void FullRefresh()
    {
        if (!EnableUpdates) return;

        _currentSortColumn = null;
        _currentSortOrder = SortOrder.None;

        // update directory tree/filenames/previews/validation/selection
        UpdateFileList();
    }

    // update directory tree/filenames/previews/validation (each cascades into the one below)
    private void UpdateFileList()
    {
        if (!EnableUpdates) return;

        // Snapshot current user input for business logic
        //_currentInput = GetUserInput();

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

        BuildFileViewRows();
        _fileStore.Stats.SetShown(_fileViewRows.Count);
        UpdateFileStats();
        UpdateSelection();
        UpdatePreview();
    }

    
    private void UpdatePreview()
    {
        if (!EnableUpdates || !_validMatch) return;

        this.Cursor = Cursors.AppStarting;

        _fileStore.BuildPreview(_currentInput.MatchPattern, _currentInput.ReplacePattern, _currentInput.Numbering, _currentInput.ChangeCase, _currentInput.Kavita, _currentInput.Modifiers);

        // do preview filename validation
        UpdateValidation();

        // redraw
        if (_currentSortOrder != SortOrder.None)
        {
            SortFileViewRows(_currentSortColumn ?? colFilename,
                             _currentSortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending);
        }

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

        dgvFiles.Invalidate();
    }

    private enum ColumnIndex
    {
        Icon,
        Sno,
        Filename,
        Preview,
        Extension,
        Size,
        ModifiedDate,
        Title,
        Series,
        Volumes,
        Chapters,
        Edition,
        IsSpecial
    }

    private object GetCellValue(FileViewRowData rowData, int columnIndex, bool forDisplay = true)
    {
        if (rowData == null)
            return null;
        var fileInfo = rowData.FileInfo;
        if (fileInfo == null)
            return null;
        switch (columnIndex)
        {
            case (int)ColumnIndex.Sno:
                return forDisplay ? (rowData.FileStoreIndex + 1).ToString() : rowData.FileStoreIndex;
            case (int)ColumnIndex.Icon:
                {
                    if (rowData.FileIcon == null)
                    {
#if !DEBUG
                        try
                        {
#endif
                            // add image (keyed by extension)
                            rowData.FileIcon = _fileViewIconCache.GetIcon(fileInfo);
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
            case (int)ColumnIndex.Filename:
                return fileInfo.Name;
            case (int)ColumnIndex.Preview:
                return fileInfo.Context.Preview;
            case (int)ColumnIndex.Extension:
                return chkShowInfo.Checked ? fileInfo.Extension : null;
            case (int)ColumnIndex.Size:
                return chkShowInfo.Checked ? fileInfo.Size : null;
            case (int)ColumnIndex.ModifiedDate:
                return chkShowInfo.Checked ? fileInfo.FileModified : null;
            case (int)ColumnIndex.Title:
                return fileInfo.Context.ParseInfo != null ? fileInfo.Context.ParseInfo.Title : null;
            case (int)ColumnIndex.Series:
                return fileInfo.Context.ParseInfo != null ? fileInfo.Context.ParseInfo.Series : null;
            case (int)ColumnIndex.Volumes:
                return fileInfo.Context.ParseInfo != null ? fileInfo.Context.ParseInfo.Volumes : null;
            case (int)ColumnIndex.Chapters:
                return fileInfo.Context.ParseInfo != null ? fileInfo.Context.ParseInfo.Chapters : null;
            case (int)ColumnIndex.Edition:
                return fileInfo.Context.ParseInfo != null ? fileInfo.Context.ParseInfo.Edition : null;
            case (int)ColumnIndex.IsSpecial:
                return fileInfo.Context.ParseInfo != null ? (fileInfo.Context.ParseInfo.IsSpecial ? "true" : "false") : null;
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

            var fileInfo = _fileViewRows[row.Index].FileInfo;
            fileInfo.Context.Selected = true;
            firstSelection ??= fileInfo;
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
        RefreshFileListView(UpdateStage.FileList);
        cmbMatch.Focus();
    }
}
