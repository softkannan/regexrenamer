using Config;
using System.ComponentModel;
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

namespace RegexRenamer
{
    public partial class MainForm
    {
        private ContextMenuStrip cmFileView;
        private DataGridIconCache _fileViewIconCache = new DataGridIconCache();


        #region Context Menu Builder and Initializer
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

        #endregion

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

        
        #region DataGrid and Sorting Handlers

        private void UpdateDataGridColumnTextAlignment(DataGridViewContentAlignment alignment)
        {
            if (_activePath == null) return;
            
            colFilename.DefaultCellStyle.Alignment = alignment;
           // colPreview.DefaultCellStyle.Alignment = alignment;
        }

        

        private void dgvFiles_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(cmbSort.Text)) && _activeSortMatch != null)
            {
                // reverse sort for other columns
                var val1 = e.CellValue1?.ToString() ?? "";
                var val2 = e.CellValue2?.ToString() ?? "";
                val1 = val1.ConvertToSortText(_activeSortMatch);
                val2 = val2.ConvertToSortText(_activeSortMatch);
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

        private void dgvFiles_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn clickedColumn = dgvFiles.Columns[e.ColumnIndex];
            ListSortDirection direction = ListSortDirection.Ascending;

            if (_currentSortColumn == clickedColumn && _currentSortOrder == SortOrder.Ascending)
                direction = ListSortDirection.Descending;

            SortFileViewRows(clickedColumn, direction);
        }
        #endregion

        #region VirtualMode Event Handlers

        private void dgvFiles_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _fileViewRows.Count) return;
            e.Value = _fileViewRows[e.RowIndex].CellValues[e.ColumnIndex];
        }

        private void dgvFiles_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _fileViewRows.Count) return;
            _fileViewRows[e.RowIndex].CellValues[e.ColumnIndex] = e.Value;
        }

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

        /// <summary>Gets the active file index for the given display row index.</summary>
        private int GetActiveFileIndex(int rowIndex) => _fileViewRows[rowIndex].ActiveFileIndex;

        private SortOrder _currentSortOrder = SortOrder.Ascending;
        private DataGridViewColumn _currentSortColumn;

        /// <summary>Sorts the backing list and refreshes the grid.</summary>
        private void SortFileViewRows(DataGridViewColumn column, ListSortDirection direction)
        {
            if (column == null || _fileViewRows.Count == 0) return;

            _currentSortColumn = column;
            _currentSortOrder = direction == ListSortDirection.Descending ? SortOrder.Descending : SortOrder.Ascending;

            int colIndex = column.Index;
            int sign = direction == ListSortDirection.Ascending ? 1 : -1;

            // Hoist invariant checks outside the per-comparison lambda
            bool useCustomSort = !string.IsNullOrWhiteSpace(cmbSort.Text) && _activeSortMatch != null;
            bool isFileSizeColumn = column == colFileSize;
            bool isModifiedColumn = column == colModified;
            var sortMatch = _activeSortMatch;

            _fileViewRows.Sort((a, b) =>
            {
                object val1 = a.CellValues[colIndex];
                object val2 = b.CellValues[colIndex];

                if (useCustomSort)
                {
                    string s1 = (val1?.ToString() ?? "").ConvertToSortText(sortMatch);
                    string s2 = (val2?.ToString() ?? "").ConvertToSortText(sortMatch);
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

        // FILE LIST
        // F5 = refresh
        #region Files List DataGridView Methods
        private void dgvFiles_KeyUp(object sender, KeyEventArgs e)
        {
            if (!EnableUpdates) return;

            if (e.KeyCode == Keys.F5)
            {
                RefreshView(UpdateStage.FileList);
            }
            if(e.KeyCode == Keys.F2)
            {
                dgvFiles.BeginEdit(false);
            }
            else if(e.KeyCode == Keys.C && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                List<RenameItemInfo> selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetActiveFileIndex);
                selectedFiles.CopyFilesToClipboad();
            }
            else if (e.KeyCode == Keys.X && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                List<RenameItemInfo> selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetActiveFileIndex);
                selectedFiles.CopyFilesToClipboad(true);
            }
            else if (e.KeyCode == Keys.V && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                _activePath.ClipboardPasteFiles();
                RefreshView(UpdateStage.FileList);
            }
            else if(e.KeyCode == Keys.Delete)
            {
                var selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetActiveFileIndex).Select(item => item.Fullpath).ToList();
                var selectedIndex = dgvFiles.SelectedRows[0].Index;
                PInvoke.FileOperationAPI.SendToRecycleBin(selectedFiles);
                RefreshView(UpdateStage.FileList);
                if (dgvFiles.Rows.Count > selectedIndex)
                {
                    selectedIndex = selectedIndex == 0 ? 0 : selectedIndex - 1;
                    dgvFiles.Rows[selectedIndex].Selected = true;
                    //scroll into view
                    dgvFiles.FirstDisplayedScrollingRowIndex = selectedIndex;
                }
            }
        }

        // rename single file
        bool single_file_rename_editing = false;
        private void dgvFiles_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!EnableUpdates) e.Cancel = true;

            single_file_rename_editing = true;
        }
        private void dgvFiles_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!single_file_rename_editing) return;

            int afi = GetActiveFileIndex(e.RowIndex);
            string newFilename = (string)e.FormattedValue;
            string prevFilename = _fileStore.Files[afi].Name;

            // cancel if new value empty or unchanged
            if (string.IsNullOrEmpty(newFilename) || newFilename == _fileStore.Files[afi].Name)
            {
                dgvFiles.CancelEdit();
                return;
            }

            // get new name/path
            if (itmOptionsPreserveExt.Checked)
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

            // update icon (if file)
            FileInfo fi = new FileInfo(newFullpath);
            if (!RenameFolders && fi.Extension != _fileStore.Files[afi].Extension)
            {
                // update datagrid icon
                try  // add image (keyed by extension)
                {
                    _fileViewRows[e.RowIndex].CellValues[0] = _fileViewIconCache.GetIcon(fi);
                }
                catch  // default = no image
                {
                    _fileViewRows[e.RowIndex].CellValues[0] = new Bitmap(1, 1);
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
        private void dgvFiles_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            single_file_rename_editing = false;
        }

        // double-click = open file
        private async void dgvFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!EnableUpdates) return;

            try
            {
                var filePath = _fileStore.Files[GetActiveFileIndex(e.RowIndex)].Fullpath;
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

        // hide selection when leaving
        private void dgvFiles_Leave(object sender, EventArgs e)
        {
            if (!_renameSelectionOnly)
            {
                dgvFiles.ClearSelection();
            }
        }

        // error tooltips for lvwFiles subitems
        private void dgvFiles_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != 2) return;  // not preview column
            if (_fileViewRows[e.RowIndex].PreviewErrorTag == null) return;  // no preview error

            ttPreviewError.SetToolTip(dgvFiles, WrapText(_fileViewRows[e.RowIndex].PreviewErrorTag, 50));
        }
        private void dgvFiles_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            ttPreviewError.SetToolTip(dgvFiles, null);
        }

        // selected rows
        private void dgvFiles_SelectionChanged(object sender, EventArgs e)
        {
            RefreshView(UpdateStage.Selection);
        }

        private void UpdateFileInfo(RenameItemInfo firstSelection)
        {
            var humanVal = firstSelection.GetHumanReadableBytes();
            lblInfoFileSize.Text = $"File Size: {humanVal}";
        }
        #endregion

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

                var selectedItems = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetActiveFileIndex);
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
                    selectedItems[idx].Preview = newNames[idx];
                    selectedItems[idx].Skip = true;
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

                var selectedItems = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetActiveFileIndex);
                if (selectedItems == null)
                    return;

                foreach (var fileItem in selectedItems)
                {
                    fileItem.Skip = false;
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
                        fileItem.Preview = result.MergedTranslation;
                        fileItem.Skip = true;
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
                var filePath = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetActiveFileIndex).Select(item => item.Fullpath).FirstOrDefault();
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

            var selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetActiveFileIndex).Select( item => item.Fullpath).ToList();
            var selectedIndex = dgvFiles.SelectedRows[0].Index;
            PInvoke.FileOperationAPI.SendToRecycleBin(selectedFiles);
            RefreshView(UpdateStage.FileList);
            if(dgvFiles.Rows.Count > selectedIndex)
            {
                selectedIndex = selectedIndex == 0 ? 0 : selectedIndex - 1;
                dgvFiles.Rows[selectedIndex].Selected = true;
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
            List<RenameItemInfo> selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetActiveFileIndex);
            selectedFiles.CopyFilesToClipboad(true);
        }

        private void copyFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<RenameItemInfo> selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetActiveFileIndex);
            selectedFiles.CopyFilesToClipboad();
        }

        private void copyPathFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<RenameItemInfo> selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files, GetActiveFileIndex);
            selectedFiles.CopyFilesPathToClipboad();
        }

        private void editFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvFiles.BeginEdit(false);
        }
        private void explorerFileViewContextMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_fileStore.Files, GetActiveFileIndex);
            //ctxMenu.ShowContextMenu(new []{ dirInfo }, this.PointToScreen(Cursor.Position));
            _shellCtxMenu.ShowContextMenu(selectedFiles.ToArray(), Cursor.Position);
            cmFolderView.Tag = null;
        }

        private void editMetadataFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_fileStore.Files, GetActiveFileIndex);
            using (EditMetadataForm metaEditForm = new EditMetadataForm(selectedFiles,"Modify Metadata", "Edit",
                itmOptionsPreserveExt.Checked))
            {
                metaEditForm.ShowDialog();
            }
        }

        private void toolsFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_fileStore.Files, GetActiveFileIndex);
            using (FileToolsForm convertForm = new FileToolsForm("",selectedFiles, "File Tools", ""))
            {
                convertForm.ShowDialog();
            }
        }

        #endregion
    }
}
