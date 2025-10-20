using RegexRenamer.Controls.FolderTreeViewCtrl;
using RegexRenamer.Forms;
using RegexRenamer.Native;
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
        PInvoke.ShellContextMenu _shellCtxMenu = new PInvoke.ShellContextMenu();
        private ContextMenuStrip cmFolderView;
        private ToolStripMenuItem setAsKavitaLibraryRootFolderViewToolStripMenuItem;
        private void CreateFolderViewMenu()
        {
            cmFolderView = new System.Windows.Forms.ContextMenuStrip(components);
            setAsKavitaLibraryRootFolderViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var explorerContextMenuFolderViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var renameFolderViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var deleteFolderViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var pasteFolderViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var cutFolderViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var copyFolderViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var copyPathFolderViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var openInExplorerFolderViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            var editMetadataFolderViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();


            // 
            // cmFolderView
            // 
            cmFolderView.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmFolderView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { setAsKavitaLibraryRootFolderViewToolStripMenuItem, explorerContextMenuFolderViewToolStripMenuItem, renameFolderViewToolStripMenuItem, deleteFolderViewToolStripMenuItem, pasteFolderViewToolStripMenuItem, cutFolderViewToolStripMenuItem, copyFolderViewToolStripMenuItem, copyPathFolderViewToolStripMenuItem, openInExplorerFolderViewToolStripMenuItem, editMetadataFolderViewToolStripMenuItem });
            cmFolderView.Name = "cmFileView";
            cmFolderView.Size = new System.Drawing.Size(209, 246);
            // 
            // setAsKavitaLibraryRootFolderViewToolStripMenuItem
            // 
            setAsKavitaLibraryRootFolderViewToolStripMenuItem.Name = "setAsKavitaLibraryRootFolderViewToolStripMenuItem";
            setAsKavitaLibraryRootFolderViewToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            setAsKavitaLibraryRootFolderViewToolStripMenuItem.Text = "Set As Kavita Library Root";
            // 
            // explorerContextMenuFolderViewToolStripMenuItem
            // 
            explorerContextMenuFolderViewToolStripMenuItem.Name = "explorerContextMenuFolderViewToolStripMenuItem";
            explorerContextMenuFolderViewToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            explorerContextMenuFolderViewToolStripMenuItem.Text = "Explorer Context Menu";
            // 
            // renameFolderViewToolStripMenuItem
            // 
            renameFolderViewToolStripMenuItem.Name = "renameFolderViewToolStripMenuItem";
            renameFolderViewToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            renameFolderViewToolStripMenuItem.Text = "Rename";
            // 
            // deleteFolderViewToolStripMenuItem
            // 
            deleteFolderViewToolStripMenuItem.Name = "deleteFolderViewToolStripMenuItem";
            deleteFolderViewToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            deleteFolderViewToolStripMenuItem.Text = "Delete";
            // 
            // pasteFolderViewToolStripMenuItem
            // 
            pasteFolderViewToolStripMenuItem.Name = "pasteFolderViewToolStripMenuItem";
            pasteFolderViewToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            pasteFolderViewToolStripMenuItem.Text = "Paste";
            // 
            // cutFolderViewToolStripMenuItem
            // 
            cutFolderViewToolStripMenuItem.Name = "cutFolderViewToolStripMenuItem";
            cutFolderViewToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            cutFolderViewToolStripMenuItem.Text = "Cut";
            // 
            // copyFolderViewToolStripMenuItem
            // 
            copyFolderViewToolStripMenuItem.Name = "copyFolderViewToolStripMenuItem";
            copyFolderViewToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            copyFolderViewToolStripMenuItem.Text = "Copy";
            // 
            // copyPathFolderViewToolStripMenuItem
            // 
            copyPathFolderViewToolStripMenuItem.Name = "copyPathFolderViewToolStripMenuItem";
            copyPathFolderViewToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            copyPathFolderViewToolStripMenuItem.Text = "Copy Path";
            // 
            // openInExplorerFolderViewToolStripMenuItem
            // 
            openInExplorerFolderViewToolStripMenuItem.Name = "openInExplorerFolderViewToolStripMenuItem";
            openInExplorerFolderViewToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            openInExplorerFolderViewToolStripMenuItem.Text = "Open In Explorer";
            // 
            // editMetadataFolderViewToolStripMenuItem
            // 
            editMetadataFolderViewToolStripMenuItem.Name = "editMetadataFolderViewToolStripMenuItem";
            editMetadataFolderViewToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            editMetadataFolderViewToolStripMenuItem.Text = "Clear / Edit Metadata";

            setAsKavitaLibraryRootFolderViewToolStripMenuItem.Click += setAsKavitaLibraryRootFolderViewToolStripMenuItem_Click;
            explorerContextMenuFolderViewToolStripMenuItem.Click += explorerContextMenuFolderViewToolStripMenuItem_Click;
            renameFolderViewToolStripMenuItem.Click += renameFolderViewToolStripMenuItem_Click;
            deleteFolderViewToolStripMenuItem.Click += deleteFolderViewToolStripMenuItem_Click;
            pasteFolderViewToolStripMenuItem.Click += pasteFolderViewToolStripMenuItem_Click;
            cutFolderViewToolStripMenuItem.Click += cutFolderViewToolStripMenuItem_Click;
            copyFolderViewToolStripMenuItem.Click += copyFolderViewToolStripMenuItem_Click;
            copyPathFolderViewToolStripMenuItem.Click += copyPathFolderViewToolStripMenuItem_Click;
            openInExplorerFolderViewToolStripMenuItem.Click += openInExplorerFolderViewToolStripMenuItem_Click;
            editMetadataFolderViewToolStripMenuItem.Click += editFolderViewMetadataFolderToolStripMenuItem_Click;
        }
        private void InitializeFolderTreeView()
        {
            CreateFolderViewMenu();

            tvwFolders.AfterLabelEdit += tvwFolders_AfterLabelEdit;
            tvwFolders.AfterSelect += tvwFolders_AfterSelect;
            tvwFolders.NodeMouseClick += tvwFolders_NodeMouseClick;
            tvwFolders.KeyUp += tvwFolders_KeyUp;

            btnNetwork.Click += bttnNetwork_Click;
            txtPath.Enter += txtPath_Enter;
            txtPath.KeyDown += txtPath_KeyDown;
            txtPath.Leave += txtPath_Leave;
        }

        #region Folder View Context Menu

        private void setAsKavitaLibraryRootFolderViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _kavitaLibRootpath = _activePath;
        }
        private void explorerContextMenuFolderViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode , Point location) = ((TreeNode , Point)) cmFolderView.Tag;
            var destPath = clickNode.TreeNodeToPath();

            DirectoryInfo dirInfo  = new DirectoryInfo(destPath);
            //ctxMenu.ShowContextMenu(new []{ dirInfo }, this.PointToScreen(Cursor.Position));
            _shellCtxMenu.ShowContextMenu(new []{ dirInfo }, location);
            cmFolderView.Tag = null;
        }

        private void renameFolderViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode , Point location) = ((TreeNode , Point)) cmFolderView.Tag;
            tvwFolders.LabelEdit = true;
            clickNode.BeginEdit();
            cmFolderView.Tag = null;
        }

        private void newFolderFolderViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode , Point location) = ((TreeNode , Point)) cmFolderView.Tag;
            var destPath = clickNode.TreeNodeToPath();

            if (Directory.Exists(destPath))
            {
                var newFolderPath = $"{destPath}\\New Folder";
                Directory.CreateDirectory(newFolderPath);
                UpdateFolderTree();
            }
            cmFolderView.Tag = null;
        }

        private void copyPathFolderViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            Clipboard.SetText(_activePath);
            cmFolderView.Tag = null;
        }

        private void openInExplorerFolderViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(_activePath);
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmFolderView.Tag = null;
        }

        private void pasteFolderViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode , Point location) = ((TreeNode , Point)) cmFolderView.Tag;
            var destPath = clickNode.TreeNodeToPath();
            destPath.ClipboardPasteFiles();
            UpdateFolderTree();
            cmFolderView.Tag = null;
        }

        private void deleteFolderViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode, Point location) = ((TreeNode, Point))cmFolderView.Tag;
            var destPath = clickNode.TreeNodeToPath();
            PInvoke.FileOperationAPI.SendToRecycleBin(destPath);
            UpdateFolderTree();
            cmFolderView.Tag = null;
        }

        private void copyFolderViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode, Point location) = ((TreeNode, Point))cmFolderView.Tag;
            var srcPath = clickNode.TreeNodeToPath();
            srcPath.CopyFilesToClipboad(false);
            cmFolderView.Tag = null;
        }

        private void cutFolderViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode, Point location) = ((TreeNode, Point))cmFolderView.Tag;
            var srcPath = clickNode.TreeNodeToPath();
            srcPath.CopyFilesToClipboad(true);
            cmFolderView.Tag = null;
        }
        private void editFolderViewMetadataFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFiles = dgvFiles.GetAllFileInfo(_fileStore.Files);
            using (EditMetadataForm editMetaForm = new EditMetadataForm(_activePath, 
                _activeFilter, "Modify Metadata", "Edit", itmOptionsPreserveExt.Checked))
            {
                editMetaForm.ShowDialog();
            }
        }
       
        // FOLDER TREE
        #endregion

        // browse network button
        private void bttnNetwork_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates) return;


            // show dialog

            if (_activePath.StartsWith("\\\\"))
                fbdNetwork.SelectedPath = _activePath;

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

            _activePath = newPath;
            UpdateFileList();
        }

        // update file list on select different path

        #region TreeView Events

        private void tvwFolders_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!EnableUpdates) return;

            if (e.Button == MouseButtons.Right )
            {
                cmFolderView.Tag = (e.Node, e.Location);
                cmFolderView.Show(e.Location);
            }
        }

        private void tvwFolders_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!EnableUpdates)
            {
                e.CancelEdit = true;
                return;
            }

            if (string.IsNullOrEmpty(e.Label))
                return;

            if(e.Node.Text == e.Label) 
                return;
            
            var newPath = e.Node.RenameFolder(e.Label);
            tvwFolders.LabelEdit = false;
            if (!string.IsNullOrEmpty(newPath))
            {
                _activePath = newPath;   
            }
            UpdateFolderTree();
        }
        private void tvwFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!EnableUpdates) return;

            if (tvwFolders.Tag != null && tvwFolders.SelectedNode == (TreeNode)tvwFolders.Tag)  // My Network Places
                toolTip.Show("Click to browse the network", btnNetwork, 0, btnNetwork.Height, 5000);

            _activePath = tvwFolders.GetSelectedNodePath();
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
        #endregion


        #region Network path txtbox events

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
                        if (!tvwFolders.BringToView(txtPath.Text))
                            tvwFolders.SelectedNode = null;
                    }
                    EnableUpdates = true;

                    e.SuppressKeyPress = true;  // prevent beep

                    _activePath = txtPath.Text;
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
                txtPath.Text = _activePath;
                txtPath.SelectionStart = _activePath.Length;
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
                        if (!tvwFolders.BringToView(txtPath.Text))
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
        #endregion

    }
}
