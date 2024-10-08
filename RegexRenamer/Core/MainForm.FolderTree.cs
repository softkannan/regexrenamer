﻿using RegexRenamer.Native;
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
        PInvoke.ShellContextMenu ctxMenu = new PInvoke.ShellContextMenu();

        private void explorerContextMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode , Point location) = ((TreeNode , Point)) cmFolderView.Tag;
            var destPath = clickNode.TreeNodeToPath();

            DirectoryInfo dirInfo  = new DirectoryInfo(destPath);
            //ctxMenu.ShowContextMenu(new []{ dirInfo }, this.PointToScreen(Cursor.Position));
            ctxMenu.ShowContextMenu(new []{ dirInfo }, location);
            cmFolderView.Tag = null;
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode , Point location) = ((TreeNode , Point)) cmFolderView.Tag;
            tvwFolders.LabelEdit = true;
            clickNode.BeginEdit();
            cmFolderView.Tag = null;
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void copyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            Clipboard.SetText(ActivePath);
            cmFolderView.Tag = null;
        }

        private void openInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(ActivePath);
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmFolderView.Tag = null;
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode , Point location) = ((TreeNode , Point)) cmFolderView.Tag;
            var destPath = clickNode.TreeNodeToPath();
            destPath.ClipboardPasteFiles();
            UpdateFolderTree();
            cmFolderView.Tag = null;
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode, Point location) = ((TreeNode, Point))cmFolderView.Tag;
            var destPath = clickNode.TreeNodeToPath();
            PInvoke.FileOperationAPI.SendToRecycleBin(destPath);
            UpdateFolderTree();
            cmFolderView.Tag = null;
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode, Point location) = ((TreeNode, Point))cmFolderView.Tag;
            var srcPath = clickNode.TreeNodeToPath();
            srcPath.CopyFilesToClipboad(false);
            cmFolderView.Tag = null;
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmFolderView.Tag == null) return;

            (TreeNode clickNode, Point location) = ((TreeNode, Point))cmFolderView.Tag;
            var srcPath = clickNode.TreeNodeToPath();
            srcPath.CopyFilesToClipboad(true);
            cmFolderView.Tag = null;
        }
        // FOLDER TREE

        // browse network button
        private void btnNetwork_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates) return;


            // show dialog

            if (ActivePath.StartsWith("\\\\"))
                fbdNetwork.SelectedPath = ActivePath;

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

            ActivePath = newPath;
            UpdateFileList();
        }

        // update file list on select different path
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

            var newPath = e.Node.RenameFolder(e.Label);
            tvwFolders.LabelEdit = false;
            if (!string.IsNullOrEmpty(newPath))
            {
                ActivePath = newPath;   
            }
            UpdateFolderTree();
        }
        private void tvwFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!EnableUpdates) return;

            if (tvwFolders.Tag != null && tvwFolders.SelectedNode == (TreeNode)tvwFolders.Tag)  // My Network Places
                toolTip.Show("Click to browse the network", btnNetwork, 0, btnNetwork.Height, 5000);

            ActivePath = tvwFolders.GetSelectedNodePath();
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
                        if (!tvwFolders.BringToView(txtPath.Text))
                            tvwFolders.SelectedNode = null;
                    }
                    EnableUpdates = true;

                    e.SuppressKeyPress = true;  // prevent beep

                    ActivePath = txtPath.Text;
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
                txtPath.Text = ActivePath;
                txtPath.SelectionStart = ActivePath.Length;
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

    }
}
