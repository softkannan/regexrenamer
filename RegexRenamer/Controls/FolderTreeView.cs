/*

  Windows Forms Folder Tree View control for .Net
  Version 1.1, posted 20-Oct-2002
  (c)Copyright 2002 Furty (furty74@yahoo.com). All rights reserved.
  Free for any use, so long as copyright is acknowledged.
  
  This is an all-new version of the FolderTreeView control I posted here at CP some weeks ago.
  The control now starts in the Desktop namespace, and a new DrillToFolder method has been added
  so the startup folder can be specified. Please note that this control is not intended to have 
  all of the functionality of the actual Windows Explorer TreeView - it is a light-weight control 
  designed for use in projects where you want to supply a treeview for folder navigation, without supporting
  windows shell extensions. If you are looking for a control that supports shell extensions
  you should be looking at the excellent ËxplorerTreeControl submitted by Carlos H Perez at the CP website.
  
  The 3 classes that make up the control have been merged into the one file here for ease of
  integration into your own projects. The reason for separate classes is that this code has been
  extracted from a much larger project I'm working on, and the code that is not required for this
  control has been removed.  
  
  Acknowledgments:
  Substantial portions of the ShellOperations and ExtractIcons classes were borrowed from the 
  FTPCom article written by Jerome Lacaille, available on the www.codeproject.com website.
  
  If you improve this control, please email me the updated source, and if you have any 
  comments or suggestions, please post your thoughts in the feedback section on the 
  codeproject.com page for this control.
  
  Version 1.11 Changes:
  Updated the GetDesktopIcon method so that the small (16x16) desktop icon is returned instead of the large version
  Added code to give the Desktop root node a FolderItem object tag equal to the DesktopDirectory SpecialFolder,
  this ensures that the desktop node returns a file path.
 
 */
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

using System.Diagnostics;
using System.Security;
using RegexRenamer.Native;
using PInvoke;

namespace RegexRenamer.Controls
{
    #region FolderTreeView Class

    public class FolderTreeView : System.Windows.Forms.TreeView
    {
        private System.Windows.Forms.ImageList iconImageList;
        private System.Globalization.CultureInfo cultureInfo = System.Globalization.CultureInfo.CurrentCulture;

        #region Constructors
        
        public FolderTreeView()
        {
            this.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewBeforeExpand);
        }
        
        public void InitFolderTreeView()
        {
            InitImageList();
            // Set default image list index to folder icon
            this.ImageIndex = 1;    
            this.SelectedImageIndex = 2;
            this.PopulateTree(base.ImageList);
            if (this.Nodes.Count > 0)
            {
                this.Nodes[0].Expand();
            }
        }
        
        private void InitImageList()
        {
            // setup the image list to hold the folder icons
            iconImageList = new ImageList();
            iconImageList.ColorDepth = ColorDepth.Depth32Bit;
            iconImageList.ImageSize = new Size(16, 16);
            iconImageList.TransparentColor = Color.Transparent;

            // add the Desktop icon to the image list
            try
            {
                iconImageList.Images.Add(ExtractIconsAPI.GetDesktopIcon());
                iconImageList.Images.Add(FileIconAPI.GetDefaultFolderIcon(false));
                iconImageList.Images.Add(FileIconAPI.GetDefaultFolderIcon(true));
            }
            catch
            {
                // Create a blank icon if the desktop icon fails for some reason
                Bitmap bmp = new Bitmap(16, 16);
                Image img = (Image)bmp;
                iconImageList.Images.Add((Image)img.Clone());
                bmp.Dispose();
            }
            this.ImageList = iconImageList;
        }

        #endregion

        #region Event Handlers

        
        private void TreeViewBeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            this.BeginUpdate();
            e.Node.ExpandTreeNode(this.ImageList);
            this.EndUpdate();
        }

        #endregion

        #region FolderTreeView Properties & Methods

        public string GetSelectedNodePath()
        {
            return SelectedNode.GetOnlyDirectory();
        }

        // [xiperware]
        public string ForceGetSelectedNodePath()
        {
            return SelectedNode.TreeNodeToPath();
        }

        public bool BringToView(string folderPath)
        {
            bool folderFound = false;
            if (Directory.Exists(folderPath)) // don't bother drilling unless the directory exists
            {
                this.BeginUpdate();
                // if there's a trailing \ on the folderPath, remove it unless it's a drive letter
                if (folderPath.Length > 3 && folderPath.LastIndexOf("\\") == folderPath.Length - 1)
                    folderPath = folderPath.Substring(0, folderPath.Length - 1);
                //Start drilling the tree
                DrillTree(this.Nodes[0].Nodes, folderPath.ToUpper(cultureInfo), ref folderFound);
                this.EndUpdate();
            }
            if (!folderFound)
                this.SelectedNode = this.Nodes[0];
            return folderFound;
        }

        private void DrillTree(TreeNodeCollection tnc, string path, ref bool folderFound)
        {
            foreach (TreeNode tn in tnc)
            {
                if (!folderFound)
                {
                    this.SelectedNode = tn;
                    string tnPath = tn.GetOnlyDirectory().ToUpper(cultureInfo);
                    if (path == tnPath && !folderFound)
                    {
                        this.SelectedNode = tn;
                        tn.EnsureVisible();
                        folderFound = true;
                        break;
                    }
                    else if (path.IndexOf(tnPath) > -1 && !folderFound)
                    {
                        //TODO: fix this
                        tn.Expand();
                        DrillTree(tn.Nodes, path, ref folderFound);
                    }
                }
            }
        }

        public void RefreshNode(string path)  // [xiperware]
        {
            //if (path.StartsWith("\\\\")) return;
            path = path.ToLower();

            TreeNode currentNode = this.Nodes[0].Nodes[0];  // assume My Computer is first node under Desktop
            bool foundParent = false, foundChild = false;

            while (true)
            {
                foundParent = false;
                foreach (TreeNode node in currentNode.Nodes)
                {
                    if (node.Tag.ToString() == "DUMMYNODE")
                        return;
                    string nodePath = node.TreeNodeToPath().ToLower();
                    if (path == nodePath)
                    {
                        currentNode = node;
                        foundChild = true;
                        break;
                    }
                    else if (path.StartsWith(nodePath))
                    {
                        currentNode = node;
                        foundParent = true;
                        break;
                    }
                }
                if (foundChild || !foundParent) break;
            }

            RefreshNode(currentNode);
        }

        public void RefreshNode(TreeNode tn)  // [xiperware]
        {
            if (this.Tag != null && tn == (TreeNode)this.Tag)  // My Network Places
                return;

            if (tn.Nodes.Count == 1 && tn.Nodes[0].Tag.ToString() == "DUMMYNODE")
                return;

            this.BeginUpdate();

            TreeNode selectedNode = this.SelectedNode;

            bool nodeWasExpanded = tn.IsExpanded;
            if (nodeWasExpanded)
                tn.Collapse();

            tn.Nodes.Clear();

            TreeNode dummyNode = new TreeNode();
            dummyNode.Tag = "DUMMYNODE";
            tn.Nodes.Add(dummyNode);

            tn.Expand();  // populates sub-nodes
            if (!nodeWasExpanded)
                tn.Collapse();

            if (this.SelectedNode != selectedNode)
                this.BringToView(selectedNode.TreeNodeToPath());

            this.EndUpdate();
        }

        #endregion

        public void UpdateFolderTree(string activePath)
        {
            this.BeginUpdate();

            // get selected path (regardless whether it exists)
            if (SelectedNode != null && !Directory.Exists(activePath))
            {
                activePath = this.ForceGetSelectedNodePath();
            }

            // save prev path to preserve node expansion
            string prevPathExpand = null;
            if (this.SelectedNode != null && this.SelectedNode.IsExpanded)
            {
                prevPathExpand = activePath.ToLower();
            }

            // init tvwFolders with directory tree
            this.InitFolderTreeView();

            // get active path
            while (!Directory.Exists(activePath))  // if doesn't exist, walk tree backwards
            {
                DirectoryInfo di = null;
                try { di = Directory.GetParent(activePath); } catch { }
                if (di == null) break;

                activePath = di.FullName;
            }

            if (!Directory.Exists(activePath))  // still not found, default to system drive
            {
                activePath = Environment.GetEnvironmentVariable("SystemDrive") + "\\";
            }

            // drill to folder and expand
            if (activePath.StartsWith("\\\\"))
            {
                // select My Network Places
                this.SelectedNode = (TreeNode)this.Tag;
            }
            else if (Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).Equals(activePath, StringComparison.CurrentCultureIgnoreCase))
            {
                // select root node
                this.SelectedNode = this.Nodes[0];
            }
            else
            {
                // find folder in tree
                //if (!this.BringToView(activePath))
                //{
                //    activePath = this.GetSelectedNodePath();
                //}
            }

            // re-expand
            if (this.SelectedNode != null && prevPathExpand == activePath.ToLower())
            {
                this.SelectedNode.Expand();
            }

            this.EndUpdate(); 
        }
    }

    #endregion
}

