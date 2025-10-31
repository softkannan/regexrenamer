using Interop.Shell32;
using PInvoke;
using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RegexRenamer.Controls.FolderTreeViewCtrl;

public static class FolderTreeViewExtensions
{
    private const string      DUMMYNODE = "DUMMYNODE";
    private static ShellClass shell32   = new ShellClass();
    #region Public methods

    public static string RenameFolder(this TreeNode node, string newLabel)
    {
        var ret = "";
        var folderItem = node.Tag as FolderItem;
        if (folderItem != null)
        {
            var parentDir = Directory.GetParent(folderItem.Path);
            var newPath = $"{parentDir}\\{newLabel}";
            FileOperationAPI.RenameFolder(folderItem.Path, newPath);
            ret = newPath;
        }
        return ret;
    }

    public static string TreeNodeToPath(this TreeNode node)
    {
        var folderItem = node.Tag as FolderItem;
        var ret = folderItem?.Path;
        ret ??= "";
        return ret;
    }

    public static string FolderTreeNodeToDirectory(this TreeNode tn)
    {
        FolderItem folderItem = tn.Tag as FolderItem;
        var folderPath = folderItem?.Path;
        return Directory.Exists(folderPath) ? folderPath : "";
    }

    public static FolderItem GetShellFolderItem(this string path, string fileName)
    {
        Folder folder = GetFolder(path);
        var ret = folder.ParseName(fileName);
        return ret;
    }

    public static void PopulateFolders(this TreeView tree, FolderTreeViewImageList imageList)
    {
        tree.Nodes.Clear();
        AddRootNode(tree, imageList, KnownFolderAPI.SHShellFolder.DESKTOP, true);
        if (tree.Nodes.Count > 1)
        {
            tree.SelectedNode = tree.Nodes[1];
            tree.Nodes[1].ExpandFolder(imageList);
        }
    }

    public static void ExpandFolder(this TreeNode tn, FolderTreeViewImageList imageList)
    {
        // if there's a dummy node present, clear it and replace with actual contents
        if (tn.Nodes.Count == 1 && tn.Nodes[0].Tag.ToString() == DUMMYNODE)
        {
            tn.Nodes.Clear();
            FolderItem folderItem = (FolderItem)tn.Tag;
            Folder folder = (Folder)folderItem.GetFolder;
            foreach (FolderItem item in folder.Items())
            {
                //if (item.IsFileSystem && item.IsFolder && !item.IsBrowsable)
                if (item.IsFolder && !item.IsBrowsable)
                {
                    TreeNode ntn = AddTreeNode(item, imageList);
                    tn.Nodes.Add(ntn);
                    AddSubFolderDummyNodes(ntn);
                }
            }
        }
    }
    #endregion

    #region Private Methods
    private static Folder GetFolder(KnownFolderAPI.SHShellFolder topFolder)
    {
        return  shell32.NameSpace(topFolder);
    }
    private static Folder GetFolder(string path)
    {
        return  shell32.NameSpace(path);
    }
    private static void AddRootNode(TreeView tree, FolderTreeViewImageList imageList, KnownFolderAPI.SHShellFolder rootFolder, bool getIcons)
    {
        Folder shell32RootFolder = GetFolder(rootFolder);
        FolderItems rootItems = shell32RootFolder.Items();

        TreeNode rootNode = new TreeNode(rootFolder.ToString(), 0, 0);

        // Added in version 1.11
        // add a FolderItem object to the root (Desktop) node tag that corresponds to the DesktopDirectory namespace
        // This ensures that the GetSelectedNodePath will return the actual Desktop folder path when queried.
        // There's possibly a better way to create a Shell32.FolderItem instance for this purpose, 
        // but I surely don't know it

        Folder dfolder = GetFolder(KnownFolderAPI.SHShellFolder.DESKTOPDIRECTORY);
        foreach (FolderItem fi in dfolder.ParentFolder.Items())
        {
            if (fi.Name == dfolder.Title)
            {
                rootNode.Tag = fi;
                break;
            }
        }

        // Add the Desktop root node to the tree
        tree.Nodes.Add(rootNode);

        // Get FolderItem that represents Recycle Bin
        Folder recFolder = GetFolder(KnownFolderAPI.SHShellFolder.BITBUCKET);
        FolderItem recycle = null;
        foreach (FolderItem fi in recFolder.ParentFolder.Items())
        {
            if (fi.Name == recFolder.Title)
            {
                recycle = fi;
                break;
            }
        }

        // Get FolderItem that represents My Network Places
        Folder netFolder = GetFolder(KnownFolderAPI.SHShellFolder.NETWORK);
        FolderItem mynetwork = null;
        foreach (FolderItem fi in netFolder.ParentFolder.Items())
        {
            if (fi.Name == netFolder.Title)
            {
                mynetwork = fi;
                break;
            }
        }

        // iterate through the Desktop namespace and populate the first level nodes
        foreach (FolderItem item in rootItems)
        {
            if (!item.IsFolder) continue;  // this ensures that desktop shortcuts etc are not displayed
            if (item.IsBrowsable) continue;  // exclude zip files
            if (recycle != null && item.Path == recycle.Path) continue;  //  skip recycle bin

            TreeNode tn = AddTreeNode(item, imageList, getIcons);
            rootNode.Nodes.Add(tn);

            Debug.WriteLine(item.Path);

            if (mynetwork != null && item.Path == mynetwork.Path)  // skip my network places subdirs
            {
                tree.Tag = tn;  // store node in tag and skip sub folder evaluation
                continue;
            }

            AddSubFolderDummyNodes(tn);
        }
    }

    private static void FillSubDirectories(TreeNode tn, FolderTreeViewImageList imageList, bool getIcons)
    {
        FolderItem folderItem = (FolderItem)tn.Tag;
        Folder folder = (Folder)folderItem.GetFolder;

        foreach (FolderItem item in folder.Items())
        {
            //if (item.IsFileSystem && item.IsFolder && !item.IsBrowsable)
            if (item.IsFolder && !item.IsBrowsable)
            {
                TreeNode ntn = AddTreeNode(item, imageList, getIcons);
                tn.Nodes.Add(ntn);
                AddSubFolderDummyNodes(ntn);
            }
        }
    }

    private static void AddSubFolderDummyNodes(TreeNode tn)
    {
        if (tn.Nodes.Count == 0)
        {
            // create dummy nodes for any subfolders that have further subfolders
            FolderItem folderItem = tn.Tag as FolderItem;
            Folder folder = folderItem?.GetFolder as Folder;
            if(folder == null) { return; }
            bool hasFolders = false;
            foreach (FolderItem item in folder.Items())
            {
                if (item.IsFolder && !item.IsBrowsable)
                {
                    hasFolders = true;
                    break;
                }
            }
            if (hasFolders)
            {
                TreeNode ntn = new TreeNode();
                ntn.Tag = DUMMYNODE;
                tn.Nodes.Add(ntn);
            }
        }
    }

    private static TreeNode AddTreeNode(FolderItem item, FolderTreeViewImageList imageList, bool getIcons = true)
    {
        TreeNode tn = new TreeNode();
        tn.Text = item.Name;
        tn.Tag = item;
        var imageIndex = 1;
        var selectedImageIndex = 2;
        var name = item.Name;
        var type = item.Type;
        var path = item.Path;
        var isFolder = item.IsFolder;
        var isFileSystem = item.IsFileSystem;
        var isBrowsable = item.IsBrowsable;

        if (getIcons)
        {
            string iconName = path;
            if (string.Compare("System Folder", type, StringComparison.OrdinalIgnoreCase) == 0)
                iconName = name;
            else if (string.Compare("File Folder", type, StringComparison.OrdinalIgnoreCase) == 0)
            {
                iconName = type;
                if (path.EndsWith(":\\"))
                    iconName = path;
            }

            imageIndex = imageList.GetIcon(iconName, true);
            selectedImageIndex = imageList.GetIcon(iconName, false);

            if(imageIndex != -1 && selectedImageIndex != -1)
            {
                tn.ImageIndex = imageIndex;
                tn.SelectedImageIndex = selectedImageIndex;
                return tn;
            }

            //item.Type
            var normalIcon = FileIconAPI.GetIcon(path, false);
            var selectedIcon = FileIconAPI.GetIcon(path, true);
            if (normalIcon != null && selectedIcon != null)
            {
                imageIndex = imageList.AddIcon(iconName, normalIcon, false); // normal icon
                selectedImageIndex = imageIndex = imageList.AddIcon(iconName, selectedIcon,true); // selected icon
            }
        }
        tn.ImageIndex = imageIndex;
        tn.SelectedImageIndex = selectedImageIndex;
        return tn;
    }
    #endregion

}
