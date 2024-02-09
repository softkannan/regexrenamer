using RegexRenamer.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        // FOLDER TREE

        // update file list on select different path
        private void tvwFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!EnableUpdates) return;

            if (tvwFolders.Tag != null && tvwFolders.SelectedNode == (TreeNode)tvwFolders.Tag)  // My Network Places
                toolTip.Show("Click to browse the network", btnNetwork, 0, btnNetwork.Height, 5000);

            activePath = tvwFolders.GetSelectedNodePath();
            UpdateFileList();
        }

        // F5 = refresh
        private void tvwFolders_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)  // refresh directory tree
            {
                UpdateFolderTree();
            }
        }

        private void txtPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)  // apply
            {
                if (!EnableUpdates) return;
                string errorMessage = ValidatePath();

                if (errorMessage == null)
                {
                    EnableUpdates = false;
                    if (txtPath.Text.StartsWith("\\\\"))
                    {
                        // select My Network Places
                        tvwFolders.SelectedNode = (TreeNode)tvwFolders.Tag;
                    }
                    else if (Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).Equals(txtPath.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        // select root node
                        tvwFolders.SelectedNode = tvwFolders.Nodes[0];
                    }
                    else
                    {
                        // find folder in tree
                        if (!tvwFolders.DrillToFolder(txtPath.Text))
                            tvwFolders.SelectedNode = null;
                    }
                    EnableUpdates = true;

                    e.SuppressKeyPress = true;  // prevent beep

                    activePath = txtPath.Text;
                    toolTip.Hide(txtPath);
                    this.Update();
                    UpdateFileList();
                }
                else
                {
                    tvwFolders.SelectedNode = null;
                    txtPath.BackColor = Color.MistyRose;
                    toolTip.Show(errorMessage, txtPath, 0, txtPath.Height);
                }
            }
            else if (e.KeyCode == Keys.Escape)  // revert
            {
                txtPath.BackColor = SystemColors.Window;
                txtPath.Text = activePath;
                txtPath.SelectionStart = activePath.Length;
                toolTip.Hide(txtPath);

                if (tvwFolders.SelectedNode == null)
                {
                    if (txtPath.Text.StartsWith("\\\\"))  // select My Network Places
                    {
                        EnableUpdates = false;
                        tvwFolders.SelectedNode = (TreeNode)tvwFolders.Tag;
                        EnableUpdates = true;
                    }
                    else  // find folder in tree
                    {
                        EnableUpdates = false;
                        if (!tvwFolders.DrillToFolder(txtPath.Text))
                            tvwFolders.SelectedNode = null;
                        EnableUpdates = true;
                    }
                }

                e.SuppressKeyPress = true;  // prevent beep
            }
            else  // other keypress
            {
                toolTip.Hide(txtPath);
            }
        }
        private void txtPath_Enter(object sender, EventArgs e)
        {
            ValidatePath();  // show tooltip if left during error
        }
        private void txtPath_Leave(object sender, EventArgs e)
        {
            toolTip.Hide(txtPath);
        }

        // browse network button
        private void btnNetwork_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates) return;


            // show dialog

            if (activePath.StartsWith("\\\\"))
                fbdNetwork.SelectedPath = activePath;

            if (fbdNetwork.ShowDialog() == DialogResult.Cancel) return;


            // get new path

            string newPath;
            try { newPath = fbdNetwork.SelectedPath; }
            catch { return; }  // invalid path


            // make sure exists

            if (!Directory.Exists(newPath)) return;


            // select My Network Places

            EnableUpdates = false;
            tvwFolders.SelectedNode = (TreeNode)tvwFolders.Tag;
            EnableUpdates = true;


            // update filelist

            activePath = newPath;
            UpdateFileList();
        }


        // FILE LIST

        // F5 = refresh
        private void dgvFiles_KeyUp(object sender, KeyEventArgs e)
        {
            if (!EnableUpdates) return;

            if (e.KeyCode == Keys.F5)
                UpdateFileList();
        }

        // rename single file
        bool editing = false;
        private void dgvFiles_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!EnableUpdates) e.Cancel = true;

            editing = true;
        }
        private void dgvFiles_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!editing) return;

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
                newFilename += activeFiles[afi].Extension;
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
                    string ext = fi.Extension.ToLower();
                    if (!icons.ContainsKey(ext))
                        icons.Add(ext, ExtractIcons.GetIcon(newFullpath, false));

                    dgvFiles.Rows[e.RowIndex].Cells[0].Value = icons[ext];
                }
                catch  // default = no image
                {
                    dgvFiles.Rows[e.RowIndex].Cells[0].Value = new Bitmap(1, 1);
                }
            }


            // update RRItem

            if (RenameFolders)
                activeFiles[afi] = new RRItem(new DirectoryInfo(fi.FullName), activeFiles[afi].Hidden, activeFiles[afi].PreserveExt);
            else
                activeFiles[afi] = new RRItem(fi, activeFiles[afi].Hidden, activeFiles[afi].PreserveExt);


            // update folder tree (if folder)

            if (RenameFolders)
            {
                foreach (TreeNode node in tvwFolders.SelectedNode.Nodes)
                {
                    if (node.Text == prevFilename)
                    {
                        node.Text = activeFiles[afi].Name;
                        Shell32.Folder folder = new Shell32.ShellClass().NameSpace(activePath);
                        node.Tag = folder.ParseName(activeFiles[afi].Filename);
                        break;
                    }
                }
            }


            // workaround for exception when ending edit by pressing ENTER in last cell

            dgvFiles.CommitEdit(DataGridViewDataErrorContexts.Commit);
            e.Cancel = true;


            // update preview

            editing = false;  // prevent recursion: dgvFiles.Sort() in UpdatePreview() causes dgvFiles.CellValidating
            UpdatePreview();

        }
        private void dgvFiles_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            editing = false;
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
                dgvFiles.ClearSelection();
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

    }
}
