using PInvoke;
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
        // FILE LIST

        // F5 = refresh
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
                List<RRItem> selectedFiles = dgvFiles.GetSelectedFileItems(activeFiles);
                selectedFiles.CopyFilesToClipboad();
            }
            else if (e.KeyCode == Keys.X && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                List<RRItem> selectedFiles = dgvFiles.GetSelectedFileItems(activeFiles);
                selectedFiles.CopyFilesToClipboad(true);
            }
            else if (e.KeyCode == Keys.V && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                activePath.ClipboardPasteFiles();
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
            string prevFilename = activeFiles[afi].Name;

            // cancel if new value empty or unchanged
            if (string.IsNullOrEmpty(newFilename) || newFilename == activeFiles[afi].Name)
            {
                dgvFiles.CancelEdit();
                return;
            }

            // get new name/path
            if (itmOptionsPreserveExt.Checked)
            {
                newFilename += activeFiles[afi].Extension;
            }
            string newFullpath = Path.Combine(activePath, newFilename);

            // validate
            string errorMessage = ValidateFilename(newFilename, false);
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvFiles.CancelEdit();
                return;
            }

            Regex regex = new Regex("^[ .]");
            if (regex.IsMatch(newFilename) && !regex.IsMatch(activeFiles[afi].Filename))  // now starts with [ .]
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
                    Directory.Move(activeFiles[afi].Fullpath, newFullpath);
                else
                    File.Move(activeFiles[afi].Fullpath, newFullpath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvFiles.CancelEdit();
                return;
            }

            // update icon (if file)
            FileInfo fi = new FileInfo(newFullpath);
            if (!RenameFolders && fi.Extension != activeFiles[afi].Extension)
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
                activeFiles[afi] = new RRItem(new DirectoryInfo(fi.FullName), activeFiles[afi].Hidden, activeFiles[afi].PreserveExt);
            }
            else
            {
                activeFiles[afi] = new RRItem(fi, activeFiles[afi].Hidden, activeFiles[afi].PreserveExt);
            }

            // update folder tree (if folder)
            if (RenameFolders)
            {
                foreach (TreeNode node in tvwFolders.SelectedNode.Nodes)
                {
                    if (node.Text == prevFilename)
                    {
                        node.Text = activeFiles[afi].Name;
                        node.Tag = activePath.GetShellFolderItem(activeFiles[afi].Filename);
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
        private void dgvFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!EnableUpdates) return;

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(activeFiles[(int)dgvFiles.Rows[e.RowIndex].Tag].Fullpath);
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
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

        #region File View context menu
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileItems(activeFiles).Select( item => item.Fullpath).ToList();
            PInvoke.FileOperationAPI.SendToRecycleBin(selectedFiles);
            UpdateFileList();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            activePath.ClipboardPasteFiles();
            UpdateFileList();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<RRItem> selectedFiles = dgvFiles.GetSelectedFileItems(activeFiles);
            selectedFiles.CopyFilesToClipboad(true);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<RRItem> selectedFiles = dgvFiles.GetSelectedFileItems(activeFiles);
            selectedFiles.CopyFilesToClipboad();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvFiles.BeginEdit(false);
        }
        private void explorerContextMenuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetSelectedFileInfo(activeFiles);
            //ctxMenu.ShowContextMenu(new []{ dirInfo }, this.PointToScreen(Cursor.Position));
            ctxMenu.ShowContextMenu(selectedFiles.ToArray(), Cursor.Position);
            cmFolderView.Tag = null;
        }
        #endregion
    }
}
