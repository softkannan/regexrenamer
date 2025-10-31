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
                explorerFileViewContextMenuToolStripMenuItem, copyFileViewToolStripMenuItem, 
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

            dgvFiles.SortCompare += DgvFiles_SortCompare;
            chkOrderByReverse.CheckedChanged += ChkOrderByReverse_CheckedChanged;

            colModified.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colFileSize.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        #endregion

        
        #region DataGrid and Sorting Handlers

        private void UpdateDataGridColumnTextAlignment(DataGridViewContentAlignment alignment)
        {
            if (_activePath == null) return;
            
            colFilename.DefaultCellStyle.Alignment = alignment;
           // colPreview.DefaultCellStyle.Alignment = alignment;
        }

        private void ChkOrderByReverse_CheckedChanged(object sender, EventArgs e)
        {
            if(!EnableUpdates) return;

            if(chkOrderByReverse.Checked)
            {
                UpdateDataGridColumnTextAlignment(DataGridViewContentAlignment.MiddleRight);
            }
            else
            {
                UpdateDataGridColumnTextAlignment(DataGridViewContentAlignment.MiddleLeft);
            }

            UpdatePreview();
        }

        private void DgvFiles_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == colFileSize)
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
            else if(chkOrderByReverse.Checked)
            {
                // reverse sort for other columns
                var val1 = e.CellValue1?.ToString() ?? "";
                var val2 = e.CellValue2?.ToString() ?? "";
                val1 = val1.ReverseTextElements();
                val2 = val2.ReverseTextElements();
                e.SortResult = string.Compare(val1, val2);
                e.Handled = true; // Indicate that sorting is handled
            }
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
                UpdateFileList();
            }
            if(e.KeyCode == Keys.F2)
            {
                dgvFiles.BeginEdit(false);
            }
            else if(e.KeyCode == Keys.C && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                List<RenameItemInfo> selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files);
                selectedFiles.CopyFilesToClipboad();
            }
            else if (e.KeyCode == Keys.X && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                List<RenameItemInfo> selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files);
                selectedFiles.CopyFilesToClipboad(true);
            }
            else if (e.KeyCode == Keys.V && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                _activePath.ClipboardPasteFiles();
                UpdateFileList();
            }
            else if(e.KeyCode == Keys.Delete)
            {
                var selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files).Select(item => item.Fullpath).ToList();
                var selectedIndex = dgvFiles.SelectedRows[0].Index;
                PInvoke.FileOperationAPI.SendToRecycleBin(selectedFiles);
                UpdateFileList();
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

            int afi = (int)dgvFiles.Rows[e.RowIndex].Tag;
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
            string newFullpath = Path.Combine(_activePath, newFilename);

            // validate
            string errorMessage = ValidateFilename(newFilename, false);
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
                    dgvFiles.Rows[e.RowIndex].Cells[0].Value = _fileViewIconCache.GetIcon(fi);
                }
                catch  // default = no image
                {
                    dgvFiles.Rows[e.RowIndex].Cells[0].Value = new Bitmap(1, 1);
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
            UpdatePreview();
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
                var filePath = _fileStore.Files[(int)dgvFiles.Rows[e.RowIndex].Tag].Fullpath;
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
            if (dgvFiles.Rows[e.RowIndex].Cells[2].Tag == null) return;  // no preview error

            ttPreviewError.SetToolTip(dgvFiles, WrapText((string)dgvFiles.Rows[e.RowIndex].Cells[2].Tag, 50));
        }
        private void dgvFiles_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            ttPreviewError.SetToolTip(dgvFiles, null);
        }

        // selected rows
        private void dgvFiles_SelectionChanged(object sender, EventArgs e)
        {
            UpdateSelection();
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

                var selectedItems = dgvFiles.GetSelectedFileItems(_fileStore.Files);
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
                UpdatePreview();
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

                var selectedItems = dgvFiles.GetSelectedFileItems(_fileStore.Files);
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

                UpdatePreview();
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
                var filePath = dgvFiles.GetSelectedFileItems(_fileStore.Files).Select(item => item.Fullpath).FirstOrDefault();
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

            var selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files).Select( item => item.Fullpath).ToList();
            var selectedIndex = dgvFiles.SelectedRows[0].Index;
            PInvoke.FileOperationAPI.SendToRecycleBin(selectedFiles);
            UpdateFileList();
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
            UpdateFileList();
        }

        private void cutFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<RenameItemInfo> selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files);
            selectedFiles.CopyFilesToClipboad(true);
        }

        private void copyFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<RenameItemInfo> selectedFiles = dgvFiles.GetSelectedFileItems(_fileStore.Files);
            selectedFiles.CopyFilesToClipboad();
        }

        private void editFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvFiles.BeginEdit(false);
        }
        private void explorerFileViewContextMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_fileStore.Files);
            //ctxMenu.ShowContextMenu(new []{ dirInfo }, this.PointToScreen(Cursor.Position));
            _shellCtxMenu.ShowContextMenu(selectedFiles.ToArray(), Cursor.Position);
            cmFolderView.Tag = null;
        }

        private void editMetadataFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_fileStore.Files);
            using (EditMetadataForm metaEditForm = new EditMetadataForm(selectedFiles,"Modify Metadata", "Edit",
                itmOptionsPreserveExt.Checked))
            {
                metaEditForm.ShowDialog();
            }
        }

        private void toolsFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_fileStore.Files);
            using (FileToolsForm convertForm = new FileToolsForm("",selectedFiles, "File Tools", ""))
            {
                convertForm.ShowDialog();
            }
        }

        #endregion
    }
}
