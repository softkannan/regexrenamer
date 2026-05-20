using Config;
using RegexRenamer.Controls.FolderTreeViewCtrl;
using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using RegexRenamer.Forms;
using RegexRenamer.Native;
using RegexRenamer.Rename;
using RegexRenamer.Tools.EBookPDFTools;
using RegexRenamer.Tools.Translate;
using RegexRenamer.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Storage;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace RegexRenamer
{
    public partial class MainForm
    {
        private ContextMenuStrip cmFileView;
        private DataGridIconCache _fileViewIconCache = new DataGridIconCache();

        // Initialize file list view and context menu, and wire up event handlers for virtual mode, context menu, selection, sorting, and shortcuts.
        #region Context Menu Builder and Initializer
        private void InitializeFileListView()
        {
            CreateFileViewContextMenu();

            dgvFiles.VirtualMode = true;
            dgvFiles.CellValueNeeded += dgvFiles_CellValueNeeded;
            dgvFiles.CellValuePushed += dgvFiles_CellValuePushed;
            dgvFiles.CellFormatting += dgvFiles_CellFormatting;

            dgvFiles.ContextMenuStrip = cmFileView;
            dgvFiles.CellBeginEdit += dgvFiles_CellBeginEdit;
            dgvFiles.CellDoubleClick += dgvFiles_CellDoubleClick;
            dgvFiles.CellEndEdit += dgvFiles_CellEndEdit;
            dgvFiles.CellMouseEnter += dgvFiles_CellMouseEnter;
            dgvFiles.CellMouseLeave += dgvFiles_CellMouseLeave;
            dgvFiles.CellValidating += dgvFiles_CellValidating;
            dgvFiles.SelectionChanged += dgvFiles_SelectionChanged;
            dgvFiles.KeyUp += dgvFiles_KeyUp;
            dgvFiles.Leave += dgvFiles_Leave;

            dgvFiles.SortCompare += dgvFiles_SortCompare;
            dgvFiles.ColumnHeaderMouseClick += dgvFiles_ColumnHeaderMouseClick;

            colModified.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colFileSize.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Capture designer widths as minimum widths for every column
            foreach (DataGridViewColumn col in dgvFiles.Columns)
            {
                col.MinimumWidth = col.Width;
            }

            dgvFiles.ColumnWidthChanged += (_, args) =>
            {
                if (args.Column == colFilename || args.Column == colPreview)
                    _userResizedColumns = true;
            };
            dgvFiles.SizeChanged += (_, _) => DistributeColumnWidths();
            DistributeColumnWidths();
        }

        private void CreateFileViewContextMenu()
        {
            cmFileView = new System.Windows.Forms.ContextMenuStrip(components);
            var editFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var launchEditorFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var explorerFileViewContextMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var copyFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var copyPathFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var cutFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var pasteFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var deleteFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var editMetadataFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var eBookOperationsFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var translateFileNameFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var translateFileNameFileViewToolStripMenuItemGui = new System.Windows.Forms.ToolStripMenuItem();

            // 
            // cmFileView
            // 
            cmFileView.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmFileView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { editFileViewToolStripMenuItem, 
                launchEditorFileViewToolStripMenuItem, translateFileNameFileViewToolStripMenuItem, translateFileNameFileViewToolStripMenuItemGui,
                explorerFileViewContextMenuToolStripMenuItem, copyFileViewToolStripMenuItem, copyPathFileViewToolStripMenuItem,
                cutFileViewToolStripMenuItem, pasteFileViewToolStripMenuItem, deleteFileViewToolStripMenuItem, 
                editMetadataFileViewToolStripMenuItem, eBookOperationsFileViewToolStripMenuItem });
            cmFileView.Name = "contextMenuStripFileView";
            cmFileView.Size = new System.Drawing.Size(195, 224);
            // 
            // editFileViewToolStripMenuItem
            // 
            editFileViewToolStripMenuItem.Name = "editFileViewToolStripMenuItem";
            editFileViewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            editFileViewToolStripMenuItem.Text = "Edit";
            editFileViewToolStripMenuItem.Click += editFileViewToolStripMenuItem_Click;
            // 
            // launchEditorFileViewToolStripMenuItem
            // 
            launchEditorFileViewToolStripMenuItem.Name = "launchEditorFileViewToolStripMenuItem";
            launchEditorFileViewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            launchEditorFileViewToolStripMenuItem.Text = "Launch Editor";
            launchEditorFileViewToolStripMenuItem.Click += launchEditorFileViewToolStripMenuItem_Click;
            // 
            // translateFileNameFileViewToolStripMenuItem
            // 
            translateFileNameFileViewToolStripMenuItem.Name = "translateFileNameFileViewToolStripMenuItem";
            translateFileNameFileViewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            translateFileNameFileViewToolStripMenuItem.Text = "Translate";
            translateFileNameFileViewToolStripMenuItem.Click += TranslateFileNameFileViewToolStripMenuItem_Click;
            // 
            // translateFileNameFileViewToolStripMenuItemGui
            // 
            translateFileNameFileViewToolStripMenuItemGui.Name = "translateFileNameFileViewToolStripMenuItemGui";
            translateFileNameFileViewToolStripMenuItemGui.Size = new System.Drawing.Size(194, 22);
            translateFileNameFileViewToolStripMenuItemGui.Text = "Translate GUI";
            translateFileNameFileViewToolStripMenuItemGui.Click += TranslateFileNameFileViewToolStripMenuItemGui_Click;
            // 
            // explorerFileViewContextMenuToolStripMenuItem1
            // 
            explorerFileViewContextMenuToolStripMenuItem.Name = "explorerFileViewContextMenuToolStripMenuItem1";
            explorerFileViewContextMenuToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            explorerFileViewContextMenuToolStripMenuItem.Text = "Explorer Context Menu";
            explorerFileViewContextMenuToolStripMenuItem.Click += explorerFileViewContextMenuToolStripMenuItem_Click;
            // 
            // copyFileViewToolStripMenuItem
            // 
            copyFileViewToolStripMenuItem.Name = "copyFileViewToolStripMenuItem";
            copyFileViewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            copyFileViewToolStripMenuItem.Text = "Copy";
            copyFileViewToolStripMenuItem.Click += copyFileViewToolStripMenuItem_Click;
            // 
            // copyPathFileViewToolStripMenuItem
            // 
            copyPathFileViewToolStripMenuItem.Name = "copyPathFileViewToolStripMenuItem";
            copyPathFileViewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            copyPathFileViewToolStripMenuItem.Text = "Copy Path";
            copyPathFileViewToolStripMenuItem.Click += copyPathFileViewToolStripMenuItem_Click;
            // 
            // cutFileViewToolStripMenuItem
            // 
            cutFileViewToolStripMenuItem.Name = "cutFileViewToolStripMenuItem";
            cutFileViewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            cutFileViewToolStripMenuItem.Text = "Cut";
            cutFileViewToolStripMenuItem.Click += cutFileViewToolStripMenuItem_Click;
            // 
            // pasteFileViewToolStripMenuItem
            // 
            pasteFileViewToolStripMenuItem.Name = "pasteFileViewToolStripMenuItem";
            pasteFileViewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            pasteFileViewToolStripMenuItem.Text = "Paste";
            pasteFileViewToolStripMenuItem.Click += pasteFileViewToolStripMenuItem_Click;
            // 
            // deleteFileViewToolStripMenuItem
            // 
            deleteFileViewToolStripMenuItem.Name = "deleteFileViewToolStripMenuItem";
            deleteFileViewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            deleteFileViewToolStripMenuItem.Text = "Delete";
            deleteFileViewToolStripMenuItem.Click += deleteFileViewToolStripMenuItem_Click;
            // 
            // editMetadataFileViewToolStripMenuItem1
            // 
            editMetadataFileViewToolStripMenuItem.Name = "editMetadataFileViewToolStripMenuItem1";
            editMetadataFileViewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            editMetadataFileViewToolStripMenuItem.Text = "Clear/Edit Metadata";
            editMetadataFileViewToolStripMenuItem.Click += editMetadataFileViewToolStripMenuItem_Click;
            // 
            // eBookOperationsFileViewToolStripMenuItem
            // 
            eBookOperationsFileViewToolStripMenuItem.Name = "eBookOperationsFileViewToolStripMenuItem";
            eBookOperationsFileViewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            eBookOperationsFileViewToolStripMenuItem.Text = "Tools";
            eBookOperationsFileViewToolStripMenuItem.Click += toolsFileViewToolStripMenuItem_Click;
            
        }

        #endregion

        // dynamic column width management
        #region Dynamic Column Width Distribution

        private bool _userResizedColumns;

        /// <summary>
        /// Distributes available grid width among visible columns. Uses each column's
        /// <see cref="DataGridViewColumn.MinimumWidth"/> (captured from InitializeComponent widths)
        /// as the floor. Remaining space is split between Filename and Preview.
        /// </summary>
        private void DistributeColumnWidths()
        {
            if (dgvFiles.Columns.Count == 0) return;

            // Sum minimum widths of all visible non-stretch columns
            int fixedWidth = 0;
            foreach (DataGridViewColumn col in dgvFiles.Columns)
            {
                if (col == colFilename || col == colPreview) continue;
                if (!col.Visible) continue;
                fixedWidth += col.MinimumWidth;
            }

            int overhead = dgvFiles.RowHeadersVisible ? dgvFiles.RowHeadersWidth : 0;
            overhead += SystemInformation.VerticalScrollBarWidth + 2;

            int available = dgvFiles.ClientSize.Width - fixedWidth - overhead;

            colFilename.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colPreview.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            int minFilename = colFilename.MinimumWidth;
            int minPreview = colPreview.MinimumWidth;

            if (available < minFilename + minPreview)
            {
                colFilename.Width = minFilename;
                colPreview.Width = minPreview;
            }
            else if (_userResizedColumns)
            {
                // Preserve the user's ratio between Filename and Preview
                int currentTotal = colFilename.Width + colPreview.Width;
                if (currentTotal > 0)
                {
                    double ratio = (double)colFilename.Width / currentTotal;
                    int filenameWidth = Math.Max(minFilename, (int)(available * ratio));
                    int previewWidth = Math.Max(minPreview, available - filenameWidth);
                    colFilename.Width = filenameWidth;
                    colPreview.Width = previewWidth;
                }
            }
            else
            {
                int half = available / 2;
                colFilename.Width = Math.Max(minFilename, half);
                colPreview.Width = Math.Max(minPreview, available - colFilename.Width);
            }
        }

        /// <summary>Resets user column sizing so the next distribution splits evenly.</summary>
        private void ResetColumnDistribution()
        {
            _userResizedColumns = false;
            DistributeColumnWidths();
        }
        #endregion

        // Sorting handlers
        #region DataGrid and Sorting Handlers
        // called when changing active path to reset column text alignment based on new sort match (since sort matches can have different alignments)
        private void UpdateDataGridColumnTextAlignment(DataGridViewContentAlignment alignment)
        {
            if (_activePath == null) return;
            
            colFilename.DefaultCellStyle.Alignment = alignment;
           // colPreview.DefaultCellStyle.Alignment = alignment;
        }


        // called during sorting to compare two cells. We provide custom sort logic here for the Filename column (to convert to sort text based on the active sort match)
        // and for the File Size and Modified columns (to sort by actual file size and modified date instead of their string representations).
        // For other columns, we fall back to default string comparison.
        private void dgvFiles_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(cmbSortHint.Text)) && _activeSortStringProvider != null)
            {
                // reverse sort for other columns
                var val1 = e.CellValue1?.ToString() ?? "";
                var val2 = e.CellValue2?.ToString() ?? "";
                val1 = val1.ConvertToSortText(_activeSortStringProvider);
                val2 = val2.ConvertToSortText(_activeSortStringProvider);
                e.SortResult = string.Compare(val1, val2);
                e.Handled = true; // Indicate that sorting is handled
            }
            else if (e.Column == colFileSize)
            {
                var obj1 = e.CellValue1 as FileSizeInfo;
                var obj2 = e.CellValue2 as FileSizeInfo;

                if (obj1 != null && obj2 != null)
                {
                    e.SortResult = obj1.CompareTo(obj2);
                    e.Handled = true; // Indicate that sorting is handled
                }
            }
            else if (e.Column == colModified)
            {
                var obj1 = e.CellValue1 as FileModificationInfo;
                var obj2 = e.CellValue2 as FileModificationInfo;
                if (obj1 != null && obj2 != null)
                {
                    e.SortResult = obj1.CompareTo(obj2);
                    e.Handled = true; // Indicate that sorting is handled
                }
            }
        }
        // called when user clicks column header. We sort the backing list of FileViewRowData objects and refresh the grid (instead of using built-in sorting)
        // to maintain correct indexing to the active files list and allow for custom sort text conversion.
        private void dgvFiles_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn clickedColumn = dgvFiles.Columns[e.ColumnIndex];
            ListSortDirection direction = ListSortDirection.Ascending;

            if (_currentSortColumn == clickedColumn && _currentSortOrder == SortOrder.Ascending)
                direction = ListSortDirection.Descending;

            SortFileViewRows(clickedColumn, direction);
        }
        #endregion

        // virtual mode handlers
        #region VirtualMode Event Handlers

        // called when the grid needs a cell value.
        // We pull from the backing list of FileViewRowData objects, which in turn reference the active files list for dynamic values like filename and preview.
        private void dgvFiles_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _fileViewRows.Count) return;
            e.Value = GetCellValue(_fileViewRows[e.RowIndex], e.ColumnIndex);
        }
        // called when the user edits a cell and the grid needs to push the value back to the backing list.
        // We update the FileViewRowData, which will persist through sorting and is used for dynamic values like preview error tags and forecolors.
        private void dgvFiles_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            // Ignore edits since we handle single file renames separately in CellValidating to allow for cancellation and validation error messages.
            // This event would only be triggered for edits if the user presses F2 to edit a cell, which we disable when not in single file rename mode.
            //if (e.RowIndex < 0 || e.RowIndex >= _fileViewRows.Count) return;
            //_fileViewRows[e.RowIndex].CellValues[e.ColumnIndex] = e.Value;
        }
        // called when a cell is being rendered. We set dynamic forecolors here based on the backing FileViewRowData.
        private void dgvFiles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _fileViewRows.Count) return;

            var rowData = _fileViewRows[e.RowIndex];
            if (e.ColumnIndex == 1)
            {
                e.CellStyle.ForeColor = rowData.FilenameForeColor;
            }
            else if (e.ColumnIndex == 2)
            {
                e.CellStyle.ForeColor = rowData.PreviewForeColor;
            }
        }

        // helper to get active file index from row index (since the grid is virtual and doesn't directly bind to the active files list)
        private int GetFileStoreIndex(int rowIndex) => _fileViewRows[rowIndex].FileStoreIndex;
        // track current sort for toggling and visual indicators
        private SortOrder _currentSortOrder = SortOrder.Ascending;
        // Track the current sort column to allow toggling sort order and applying visual indicators in the UI (e.g. sort glyphs in the column header).
        private DataGridViewColumn _currentSortColumn;
        // helper to sort the backing list and refresh the grid
        private void SortFileViewRows(DataGridViewColumn column, ListSortDirection direction)
        {
            if (column == null || _fileViewRows.Count == 0) return;

            _currentSortColumn = column;
            _currentSortOrder = direction == ListSortDirection.Descending ? SortOrder.Descending : SortOrder.Ascending;

            int colIndex = column.Index;
            int sign = direction == ListSortDirection.Ascending ? 1 : -1;

            // Hoist invariant checks outside the per-comparison lambda
            bool useCustomSort = !string.IsNullOrWhiteSpace(_activeSortHintName) && _activeSortStringProvider != null;
            bool isFileSizeColumn = column == colFileSize;
            bool isModifiedColumn = column == colModified;
            var sortStringConverter = _activeSortStringProvider;

            _fileViewRows.Sort((a, b) =>
            {
                object val1 = GetCellValue(a, colIndex, false);
                object val2 = GetCellValue(b, colIndex, false);

                if (useCustomSort)
                {
                    string s1 = (val1?.ToString() ?? "").ConvertToSortText(sortStringConverter);
                    string s2 = (val2?.ToString() ?? "").ConvertToSortText(sortStringConverter);
                    return sign * string.Compare(s1, s2);
                }

                if (isFileSizeColumn && val1 is FileSizeInfo fs1 && val2 is FileSizeInfo fs2)
                    return sign * fs1.CompareTo(fs2);

                if (isModifiedColumn && val1 is FileModificationInfo fm1 && val2 is FileModificationInfo fm2)
                    return sign * fm1.CompareTo(fm2);

                string str1 = val1?.ToString() ?? "";
                string str2 = val2?.ToString() ?? "";
                return sign * string.Compare(str1, str2, StringComparison.CurrentCulture);
            });

            dgvFiles.Invalidate();
        }

        #endregion
        
        // Shortcut Keys for file operations
        #region Shortcut Keys for File Operations
        // F5 = refresh
        // called on key up in the files grid for shortcuts like refresh, rename, copy/paste, delete
        private void dgvFiles_KeyUp(object sender, KeyEventArgs e)
        {
            if (!EnableUpdates) return;

            if (e.KeyCode == Keys.F5)
            {
                RefreshView(UpdateStage.FileList);
            }
            if (e.KeyCode == Keys.F2)
            {
                dgvFiles.BeginEdit(false);
            }
            else if (e.KeyCode == Keys.C && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                var (selectedFiles, _) = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetFileStoreIndex);
                selectedFiles.CopyFilesToClipboad();
            }
            else if (e.KeyCode == Keys.X && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                var (selectedFiles, _) = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetFileStoreIndex);
                selectedFiles.CopyFilesToClipboad(true);
            }
            else if (e.KeyCode == Keys.V && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                _activePath.ClipboardPasteFiles();
                RefreshView(UpdateStage.FileList);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                var (selectedFiles, minIndex) = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetFileStoreIndex);
                var filePaths = selectedFiles.Select(item => item.Fullpath).ToList();
                PInvoke.FileOperationAPI.SendToRecycleBin(filePaths);
                RefreshView(UpdateStage.FileList);
                var selectedIndex = Math.Min(minIndex, dgvFiles.Rows.Count - 1);
                dgvFiles.Rows[selectedIndex].Selected = true;
                // if the selected row is now out of view after deletion, scroll into view
                if (dgvFiles.Rows[selectedIndex]?.Displayed == false)
                {
                    //scroll into view
                    dgvFiles.FirstDisplayedScrollingRowIndex = selectedIndex;
                }
            }
        }
        #endregion

        // single file rename handlers
        // this is only time we push value from the grid to the backing list, since for other edits (e.g. bulk rename, metadata edit)
        // we update the backing list directly and just refresh the grid to pull the new values
        #region Single File Rename Handlers
        // rename single file
        bool single_file_rename_editing = false;
        // called when user begins editing a cell. We set a flag to indicate that we're in the middle of a single file rename,
        // which will trigger our rename logic in CellValidating and prevent recursion and other unwanted behavior.
        private void dgvFiles_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!EnableUpdates) e.Cancel = true;

            single_file_rename_editing = true;
        }
        // called when user finishes editing a cell. We reset the single file rename flag here to allow for other types of edits and to prevent unwanted behavior in other event handlers.
        // perform single file rename on CellValidating (instead of CellEndEdit) to allow cancellation and prevent losing focus from validation error MessageBox
        private void dgvFiles_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!single_file_rename_editing) return;

            int afi = GetFileStoreIndex(e.RowIndex);
            string newFilename = (string)e.FormattedValue;
            string prevFilename = _fileStore.Files[afi].Name;

            // cancel if new value empty or unchanged
            if (string.IsNullOrEmpty(newFilename) || newFilename == _fileStore.Files[afi].Name)
            {
                dgvFiles.CancelEdit();
                return;
            }

            // get new name/path
            if (_currentInput.PreserveExtension)
            {
                newFilename += _fileStore.Files[afi].Extension;
            }

            //Remove invalid characters from filename with empty string to get valid filename
            if (!string.IsNullOrWhiteSpace(newFilename) && !newFilename.IsValidFileName())
            {
                newFilename = newFilename.ToCleanFileName();
            }

            string newFullpath = Path.Combine(_activePath, newFilename);

            // validate
            string errorMessage = _validationService.ValidateFilename(newFilename, false, strFilename);
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvFiles.CancelEdit();
                return;
            }

            // warn if filename now starts with space or dot (since that can cause issues with other programs, even though it's technically allowed)
            Regex regex = new Regex("^[ .]");
            if (regex.IsMatch(newFilename) && !regex.IsMatch(_fileStore.Files[afi].Filename))  // now starts with [ .]
            {
                errorMessage = "This " + strFilename + " begins with a space or a dot. While this is technically possible, Windows\n"
                             + "normally won't let you do this as it may cause problems with other programs.\n"
                             + "\n"
                             + "Are you sure you want to continue?";

                if (MessageBox.Show(errorMessage, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                    == DialogResult.Cancel)
                {
                    dgvFiles.CancelEdit();
                    return;
                }
            }

            // rename
            try
            {
                if (RenameFolders)
                    Directory.Move(_fileStore.Files[afi].Fullpath, newFullpath);
                else
                    File.Move(_fileStore.Files[afi].Fullpath, newFullpath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvFiles.CancelEdit();
                return;
            }

            // update icon if extension changed (folders won't have icon change)
            FileInfo fi = new FileInfo(newFullpath);
            if (!RenameFolders && fi.Extension != _fileStore.Files[afi].Extension)
            {
                // update datagrid icon
                try  
                {
                    // add image (keyed by extension)
                    _fileViewRows[e.RowIndex].FileIcon = _fileViewIconCache.GetIcon(fi);
                }
                catch  // default = no image
                {
                    _fileViewRows[e.RowIndex].FileIcon = null;
                }
            }

            // update RRItem
            if (RenameFolders)
            {
                _fileStore.Update(afi, new RenameItemInfo(new DirectoryInfo(fi.FullName), _fileStore.Files[afi].Hidden, _fileStore.Files[afi].PreserveExt));
            }
            else
            {
                _fileStore.Update(afi, new RenameItemInfo(fi, _fileStore.Files[afi].Hidden, _fileStore.Files[afi].PreserveExt));
            }

            // update folder tree (if folder)
            if (RenameFolders)
            {
                foreach (TreeNode node in tvwFolders.SelectedNode.Nodes)
                {
                    if (node.Text == prevFilename)
                    {
                        node.Text = _fileStore.Files[afi].Name;
                        node.Tag = _activePath.GetShellFolderItem(_fileStore.Files[afi].Filename);
                        break;
                    }
                }
            }

            // workaround for exception when ending edit by pressing ENTER in last cell
            dgvFiles.CommitEdit(DataGridViewDataErrorContexts.Commit);
            e.Cancel = true;

            // update preview
            single_file_rename_editing = false;  // prevent recursion: dgvFiles.Sort() in UpdatePreview() causes dgvFiles.CellValidating
            RefreshView(UpdateStage.Preview);
        }
        // called when user finishes editing a cell. We reset the single file rename flag here to allow for other types of edits and to prevent unwanted behavior in other event handlers.
        private void dgvFiles_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            single_file_rename_editing = false;
        }
        #endregion

        // double-click = open file
        #region Double click handlers
        // called when user double-clicks a cell. We attempt to launch the file with the default associated program. If that fails, we show an error message.
        private async void dgvFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!EnableUpdates) return;

            try
            {
                var filePath = _fileStore.Files[GetFileStoreIndex(e.RowIndex)].Fullpath;
                var result = await EBookHelper.LaunchEBookAsync(filePath);
                if(!result)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(filePath);
                    startInfo.UseShellExecute = true;
                    Process.Start(startInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        // selection handlers
        #region Selection and Info Update Handlers
        // called when the grid loses focus. We clear selection here to hide the blue highlight, which can be visually distracting when the user is focused on another part of the UI (e.g. folder tree). We only do this if we're not in the middle of a rename selection (since that would cause unwanted deselection while trying to rename multiple files).
        private void dgvFiles_Leave(object sender, EventArgs e)
        {
            if (!_renameSelectionOnly)
            {
                dgvFiles.ClearSelection();
            }
        }
        // called when the selection changes. We refresh the view to reflect the new selection.
        private void dgvFiles_SelectionChanged(object sender, EventArgs e)
        {
            RefreshView(UpdateStage.Selection);
        }
        // helper to update file info label based on the first selected file (since multiple selection may have different file sizes, we just show the first one's size)
        private void UpdateFileInfo(RenameItemInfo firstSelection)
        {
            var humanVal = firstSelection.GetHumanReadableBytes();
            lblInfoFileSize.Text = $"File Size: {humanVal}";
        }
        #endregion

        // tooltips management
        #region Tooltips management
        // error tooltips for lvwFiles subitems
        // called when the mouse enters a cell. We show a tooltip if there is a preview error for the cell.
        private void dgvFiles_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != 2) return;  // not preview column
            if (_fileViewRows[e.RowIndex].PreviewErrorTag == null) return;  // no preview error

            ttPreviewError.SetToolTip(dgvFiles, WrapText(_fileViewRows[e.RowIndex].PreviewErrorTag, 50));
        }
        // called when the mouse leaves a cell. We hide the tooltip.
        private void dgvFiles_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            ttPreviewError.SetToolTip(dgvFiles, null);
        }

        #endregion

        // file operations from context menu
        #region File View context menu handlers
        private async void TranslateFileNameFileViewToolStripMenuItemGui_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbMatch.Text) || !string.IsNullOrEmpty(cmbReplace.Text))
                {
                    MessageBox.Show("Please clear the Match/Replace fields before translating.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var (selectedItems, _) = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetFileStoreIndex);
                if (selectedItems == null)
                    return;

                selectedItems.CopyNamesToClipboad();
                if (UserConfig.Inst.Translator.WebEngine == WebEngineType.PuppeteerSharp)
                {
                    var puppeteerHandler = new PuppeteerHandler();
                    var provider = UserConfig.Inst.Translator.ServiceProviders[UserConfig.Inst.Translator.SelectedProviderIndex];
                    await puppeteerHandler.LaunchPuppeteer(provider.Url);
                }
                else
                {
                    using (GoogleTranslatorForm form = new GoogleTranslatorForm())
                    {
                        form.ShowDialog();
                    }
                }
                var newNames = ClipboardExtensions.GetNamesFromClipboard();
                if (newNames.Count != selectedItems.Count)
                {
                    MessageBox.Show("The number of translated names does not match the number of selected files.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for(int idx = 0; idx < selectedItems.Count; idx++)
                {
                    selectedItems[idx].Context.Preview = newNames[idx];
                    selectedItems[idx].Context.Skip = true;
                }
                RefreshView(UpdateStage.Preview);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void TranslateFileNameFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(cmbMatch.Text) || !string.IsNullOrEmpty(cmbReplace.Text))
                {
                    MessageBox.Show("Please clear the Match/Replace fields before translating.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var (selectedItems, _) = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetFileStoreIndex);
                if (selectedItems == null)
                    return;

                foreach (var fileItem in selectedItems)
                {
                    fileItem.Context.Skip = false;
                    if (fileItem != null)
                    {
                        var filePath = fileItem.Name;
                        var fromLang = UserConfig.Inst.Translator.From;
                        var toLang = UserConfig.Inst.Translator.To;

                        //var result = await filePath.TranslateWithTokenText(fromLang, toLang, true);
                        //var result = await filePath.TranslateText(fromLang, toLang);
                        var result = await filePath.TranslateTextScrape(fromLang, toLang);
                        if (result == null || string.IsNullOrEmpty(result.MergedTranslation))
                        {
                            MessageBox.Show("Translation failed or returned empty result.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        fileItem.Context.Preview = result.MergedTranslation;
                        fileItem.Context.Skip = true;
                    }
                }

                RefreshView(UpdateStage.Preview);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void launchEditorFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var (selectedFiles, _) = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetFileStoreIndex);
                var filePath = selectedFiles.Select(item => item.Fullpath).FirstOrDefault();
                var result = await EBookHelper.EditEBookAsync(filePath);
                if (!result)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(filePath);
                    startInfo.UseShellExecute = true;
                    Process.Start(startInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void deleteFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvFiles.SelectedRows.Count == 0)
                return;

            var (selectedFiles, minIndex) = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetFileStoreIndex);
            var filePaths = selectedFiles.Select(item => item.Fullpath).ToList();
            PInvoke.FileOperationAPI.SendToRecycleBin(filePaths);
            RefreshView(UpdateStage.FileList);
            var selectedIndex = Math.Min(minIndex, dgvFiles.Rows.Count - 1);
            dgvFiles.Rows[selectedIndex].Selected = true;
            // if the selected row is now out of view after deletion, scroll into view
            if (dgvFiles.Rows[selectedIndex]?.Displayed == false)
            {
                //scroll into view
                dgvFiles.FirstDisplayedScrollingRowIndex = selectedIndex;
            }
        }

        private void pasteFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _activePath.ClipboardPasteFiles();
            RefreshView(UpdateStage.FileList);
        }

        private void cutFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var (selectedFiles, _) = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetFileStoreIndex);
            selectedFiles.CopyFilesToClipboad(true);
        }

        private void copyFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var (selectedFiles, _) = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetFileStoreIndex);
            selectedFiles.CopyFilesToClipboad();
        }

        private void copyPathFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var (selectedFiles, _) = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetFileStoreIndex);
            selectedFiles.CopyFilesPathToClipboad();
        }

        private void editFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvFiles.BeginEdit(false);
        }
        private void explorerFileViewContextMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_fileStore.Files, GetFileStoreIndex);
            //ctxMenu.ShowContextMenu(new []{ dirInfo }, this.PointToScreen(Cursor.Position));
            _shellCtxMenu.ShowContextMenu(selectedFiles.ToArray(), Cursor.Position);
            cmFolderView.Tag = null;
        }

        private void editMetadataFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_fileStore.Files, GetFileStoreIndex);
            using (EditMetadataForm metaEditForm = new EditMetadataForm(selectedFiles,"Modify Metadata", "Edit",
                itmOptionsPreserveExt.Checked))
            {
                metaEditForm.ShowDialog();
            }
        }

        private void toolsFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_fileStore.Files, GetFileStoreIndex);
            using (FileToolsForm convertForm = new FileToolsForm("",selectedFiles, "File Tools", ""))
            {
                convertForm.ShowDialog();
            }
        }

        #endregion
    }
}
