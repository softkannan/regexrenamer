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
using iText.Signatures.Validation.Lotl;
using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using RegexRenamer.Native;
using RegexRenamer.Rename;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Enumeration;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using static RegexRenamer.Controls.FolderTreeViewCtrl.Native.KnownFolderAPI;
using static RegexRenamer.Native.ControlExtensions;

namespace RegexRenamer.Controls.FolderTreeViewCtrl;

#region FolderTreeView Class

public class FolderTreeView : TreeView
{
    private FolderTreeViewImageList iconImageList = new FolderTreeViewImageList();
    private System.Globalization.CultureInfo cultureInfo = System.Globalization.CultureInfo.CurrentCulture;

    private bool _isUpdating = false;

    #region Constructors

    public FolderTreeView()
    {

    }


    public void SetExplorerTheme(bool on = true)
    {
        if (Environment.OSVersion.Version.Major >= 6)
        {
		        const int TV_FIRST = 0x1100;

            const int TVS_NOHSCROLL = 0x8000;
            // Make sure the TVS_NOHSCROLL style is set
            this.SetStyle(TVS_NOHSCROLL);

            // Set explorer theme, set critical properties, and set extended styles
            this.SetWindowTheme(on ? "explorer" : null);
            if (!on) return;
            HotTracking = true;
            ShowLines = false;
            const int TVM_SETEXTENDEDSTYLE = TV_FIRST + 44;
            const int TVS_EX_FADEINOUTEXPANDOS = 0x0040;
            const int TVS_EX_AUTOHSCROLL = 0x0020;
            this.SendMessage(TVM_SETEXTENDEDSTYLE, TVS_EX_FADEINOUTEXPANDOS | TVS_EX_AUTOHSCROLL, TVS_EX_FADEINOUTEXPANDOS | TVS_EX_AUTOHSCROLL);
        }
    }
    
    public void InitFolderTreeView()
    {
        InitImageList();
        // Set default image list index to folder icon
        ImageIndex = 1;    
        SelectedImageIndex = 2;
        this.PopulateFolders(iconImageList);
        if (Nodes.Count > 0)
        {
            Nodes[0].Expand();
        }
    }

    private void InitFolders()
    {
        //get a list of the drives
        string[] drives = Environment.GetLogicalDrives();

        foreach (string drive in drives)
        {
            DriveInfo di = new DriveInfo(drive);
            int driveImage;

            switch (di.DriveType)    //set the drive's icon
            {
                case DriveType.CDRom:
                    driveImage = 3;
                    break;
                case DriveType.Network:
                    driveImage = 6;
                    break;
                case DriveType.NoRootDirectory:
                    driveImage = 8;
                    break;
                case DriveType.Unknown:
                    driveImage = 8;
                    break;
                default:
                    driveImage = 2;
                    break;
            }

            TreeNode node = new TreeNode(drive.Substring(0, 1), driveImage, driveImage);
            node.Tag = drive;

            if (di.IsReady == true)
                node.Nodes.Add("...");

            Nodes.Add(node);
        }
    }
    
    private void InitImageList()
    {
        // add the Desktop icon to the image list
        BeginUpdate();
        iconImageList.CleanUp();
        ImageList = iconImageList.Icons;
        EndUpdate();
    }

    #endregion

    #region Event Handlers

    protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
    {
        BeginUpdate();
        e.Node.ExpandFolder(iconImageList);
        EndUpdate();

        base.OnBeforeExpand(e);
    }

    #endregion

    #region FolderTreeView Properties & Methods

    public string GetSelectedNodePath()
    {
        return SelectedNode.FolderTreeNodeToDirectory();
    }

    // [xiperware]
    public string ForceGetSelectedNodePath()
    {
        return SelectedNode.TreeNodeToPath();
    }

    #endregion

    // Refreshes the entire tree and attempts to re-select the specified path.
    // If recursive is false, will not attempt to update if already in the process of updating, otherwise will force update.
    public void UpdateFolderTree(string activePath, bool recursive = false)
    {
        if (_isUpdating && recursive == false) return;

        _isUpdating = true;

        try
        {

            BeginUpdate();

            // get selected path (regardless whether it exists)
            if (SelectedNode != null && !FastPath.DirectoryExists(activePath))
            {
                activePath = ForceGetSelectedNodePath();
            }

            // save prev path to preserve node expansion
            string prevPathExpand = null;
            if (SelectedNode != null && SelectedNode.IsExpanded)
            {
                prevPathExpand = activePath;
            }

            // init tvwFolders with directory tree
            InitFolderTreeView();

            // get active path
            while (!FastPath.DirectoryExists(activePath))  // if doesn't exist, walk tree backwards
            {
                var parentDir = FastPath.GetParentDirectory(activePath);
                if (string.IsNullOrEmpty(parentDir))
                    break;
                activePath = parentDir;
            }

            if (!FastPath.DirectoryExists(activePath))  // still not found, default to system drive
            {
                activePath = Environment.SystemDirectory;
            }

            // drill to folder and expand
            if (activePath.StartsWith("\\\\"))
            {
                // select My Network Places
                SelectedNode = (TreeNode)Tag;
            }
            else if (string.Equals(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), activePath, StringComparison.OrdinalIgnoreCase))
            {
                // select root node
                SelectedNode = Nodes[0];
            }
            else
            {
                // find folder in tree
                if (!this.BringToView(activePath, true))
                {
                    activePath = GetSelectedNodePath();
                }
            }

            // re-expand
            if (SelectedNode != null && string.Equals(prevPathExpand, activePath, StringComparison.OrdinalIgnoreCase))
            {
                SelectedNode.Expand();
            }

            EndUpdate();
        }
        finally
        {
            _isUpdating = false;
        }
    }
}

#endregion

