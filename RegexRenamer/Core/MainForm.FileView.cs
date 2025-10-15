using RegexRenamer.Controls.FolderTreeViewCtrl;
using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using RegexRenamer.Forms;
using RegexRenamer.Kavita;
using RegexRenamer.Native;
using RegexRenamer.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
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

        private void CreateFileViewContextMenu()
        {
            cmFileView = new System.Windows.Forms.ContextMenuStrip(components);
            var editFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var launchEditorFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var explorerFileViewContextMenuToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            var copyFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var cutFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var pasteFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var deleteFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var editMetadataFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var eBookOperationsFileViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            
            // 
            // cmFileView
            // 
            cmFileView.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmFileView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { editFileViewToolStripMenuItem, launchEditorFileViewToolStripMenuItem, explorerFileViewContextMenuToolStripMenuItem1, copyFileViewToolStripMenuItem, cutFileViewToolStripMenuItem, pasteFileViewToolStripMenuItem, deleteFileViewToolStripMenuItem, editMetadataFileViewToolStripMenuItem, eBookOperationsFileViewToolStripMenuItem
    });
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
            // explorerFileViewContextMenuToolStripMenuItem1
            // 
            explorerFileViewContextMenuToolStripMenuItem1.Name = "explorerFileViewContextMenuToolStripMenuItem1";
            explorerFileViewContextMenuToolStripMenuItem1.Size = new System.Drawing.Size(194, 22);
            explorerFileViewContextMenuToolStripMenuItem1.Text = "Explorer Context Menu";
            explorerFileViewContextMenuToolStripMenuItem1.Click += explorerFileViewContextMenuToolStripMenuItem_Click;
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
        }

        

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
                List<RRItem> selectedFiles = dgvFiles.GetSelectedFileItems(_activeFiles);
                selectedFiles.CopyFilesToClipboad();
            }
            else if (e.KeyCode == Keys.X && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                List<RRItem> selectedFiles = dgvFiles.GetSelectedFileItems(_activeFiles);
                selectedFiles.CopyFilesToClipboad(true);
            }
            else if (e.KeyCode == Keys.V && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                ActivePath.ClipboardPasteFiles();
                UpdateFileList();
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
            string prevFilename = _activeFiles[afi].Name;

            // cancel if new value empty or unchanged
            if (string.IsNullOrEmpty(newFilename) || newFilename == _activeFiles[afi].Name)
            {
                dgvFiles.CancelEdit();
                return;
            }

            // get new name/path
            if (itmOptionsPreserveExt.Checked)
            {
                newFilename += _activeFiles[afi].Extension;
            }
            string newFullpath = Path.Combine(ActivePath, newFilename);

            // validate
            string errorMessage = ValidateFilename(newFilename, false);
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvFiles.CancelEdit();
                return;
            }

            Regex regex = new Regex("^[ .]");
            if (regex.IsMatch(newFilename) && !regex.IsMatch(_activeFiles[afi].Filename))  // now starts with [ .]
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
                    Directory.Move(_activeFiles[afi].Fullpath, newFullpath);
                else
                    File.Move(_activeFiles[afi].Fullpath, newFullpath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvFiles.CancelEdit();
                return;
            }

            // update icon (if file)
            FileInfo fi = new FileInfo(newFullpath);
            if (!RenameFolders && fi.Extension != _activeFiles[afi].Extension)
            {
                try  // add image (keyed by extension)
                {
                    var icon = FileIconAPI.GetIcon(newFullpath, false);
                    dgvFiles.Rows[e.RowIndex].Cells[0].Value = icon;
                }
                catch  // default = no image
                {
                    dgvFiles.Rows[e.RowIndex].Cells[0].Value = new Bitmap(1, 1);
                }
            }

            // update RRItem
            if (RenameFolders)
            {
                _activeFiles[afi] = new RRItem(new DirectoryInfo(fi.FullName), _activeFiles[afi].Hidden, _activeFiles[afi].PreserveExt);
            }
            else
            {
                _activeFiles[afi] = new RRItem(fi, _activeFiles[afi].Hidden, _activeFiles[afi].PreserveExt);
            }

            // update folder tree (if folder)
            if (RenameFolders)
            {
                foreach (TreeNode node in tvwFolders.SelectedNode.Nodes)
                {
                    if (node.Text == prevFilename)
                    {
                        node.Text = _activeFiles[afi].Name;
                        node.Tag = ActivePath.GetShellFolderItem(_activeFiles[afi].Filename);
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
                var filePath = _activeFiles[(int)dgvFiles.Rows[e.RowIndex].Tag].Fullpath;
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
            if (!itmOptionsRenameSelectedRows.Checked)
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

        private void UpdateFileInfo(RRItem firstSelection)
        {
            var humanVal = firstSelection.GetHumanReadableBytes();
            lblInfoFileSize.Text = $"File Size: {humanVal}";
        }
        #endregion

        #region File View context menu
        private async void launchEditorFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var filePath = dgvFiles.GetSelectedFileItems(_activeFiles).Select(item => item.Fullpath).FirstOrDefault();
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
            var selectedFiles = dgvFiles.GetSelectedFileItems(_activeFiles).Select( item => item.Fullpath).ToList();
            PInvoke.FileOperationAPI.SendToRecycleBin(selectedFiles);
            UpdateFileList();
        }

        private void pasteFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActivePath.ClipboardPasteFiles();
            UpdateFileList();
        }

        private void cutFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<RRItem> selectedFiles = dgvFiles.GetSelectedFileItems(_activeFiles);
            selectedFiles.CopyFilesToClipboad(true);
        }

        private void copyFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<RRItem> selectedFiles = dgvFiles.GetSelectedFileItems(_activeFiles);
            selectedFiles.CopyFilesToClipboad();
        }

        private void editFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvFiles.BeginEdit(false);
        }
        private void explorerFileViewContextMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_activeFiles);
            //ctxMenu.ShowContextMenu(new []{ dirInfo }, this.PointToScreen(Cursor.Position));
            _shellCtxMenu.ShowContextMenu(selectedFiles.ToArray(), Cursor.Position);
            cmFolderView.Tag = null;
        }

        private void editMetadataFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_activeFiles);
            using (EditMetadataForm metaEditForm = new EditMetadataForm(selectedFiles,"Modify Metadata", "Edit" ))
            {
                metaEditForm.ShowDialog();
            }
        }

        private void toolsFileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(_activeFiles);
            using (FileToolsForm convertForm = new FileToolsForm("",selectedFiles, "File Tools", ""))
            {
                convertForm.ShowDialog();
            }
        }

        #endregion
    }
}
