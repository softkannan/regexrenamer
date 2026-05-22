//using Interop.Shell32;
//using PInvoke;
//using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
//using RegexRenamer.Rename;
//using System;
//using System.Diagnostics;
//using System.Runtime.InteropServices;
//using System.Runtime.InteropServices.Marshalling;
//using System.Windows.Forms;

//namespace RegexRenamer.Controls.FolderTreeViewCtrl;

//public static class FolderTreeViewExtensions
//{
//    private const string      DUMMYNODE = "DUMMYNODE";
//    private static readonly ShellClass shell32 = new ShellClass();
//    #region Public methods
//    // Renames the folder represented by the TreeNode and returns the new path. Returns empty string if rename fails or if the node is not a folder.
//    public static string RenameFolder(this TreeNode node, string newLabel)
//    {
//        if (node.Tag is not FolderItem folderItem)
//            return "";

//        var parentDir = FastPath.GetParentDirectory(folderItem.Path);
//        if (string.IsNullOrEmpty(parentDir))
//            return ""; // can't rename root items like "This PC", "Network", etc.

//        var newPath = $"{parentDir}\\{newLabel}";
//        FileOperationAPI.RenameFolder(folderItem.Path, newPath);

//        return newPath;
//    }
//    // Returns the file system path of the folder represented by the TreeNode. Returns empty string if the node does not represent a folder or if the path cannot be determined.
//    public static string TreeNodeToPath(this TreeNode node)
//    {
//        return node.Tag is FolderItem folderItem ? folderItem.Path : "";
//    }
//    // Returns the file system path of the folder represented by the TreeNode if it exists, otherwise returns empty string. This is a safer alternative to TreeNodeToPath for cases where the folder may have been moved or deleted.
//    public static string FolderTreeNodeToDirectory(this TreeNode tn)
//    {
//        if (tn.Tag is not FolderItem folderItem)
//            return "";

//        return FastPath.DirectoryExists(folderItem.Path) ? folderItem.Path : "";
//    }
//    // Returns the Shell32.FolderItem corresponding to the specified file name within the folder represented by the given path. Returns null if the folder or file cannot be found.
//    public static FolderItem GetShellFolderItem(this string path, string fileName)
//    {
//        Folder folder = GetFolder(path);
//        var ret = folder.ParseName(fileName);
//        return ret;
//    }
//    // Populates the given TreeView with the folder structure starting from the Desktop. The imageList parameter is used to assign icons to the nodes. This method should be called once during initialization.
//    public static void PopulateFolders(this TreeView tree, FolderTreeViewImageList imageList)
//    {
//        tree.BeginUpdate();
//        try
//        {
//            ReleaseTree(tree.Nodes);
//            tree.Nodes.Clear();
//            AddRootNode(tree, imageList, KnownFolderAPI.SHShellFolder.DESKTOP, true);
//            if (tree.Nodes.Count > 1)
//            {
//                tree.SelectedNode = tree.Nodes[1];
//                tree.Nodes[1].ExpandFolder(imageList);
//            }
//        }
//        finally
//        {
//            tree.EndUpdate();
//        }
//    }

//    private static void ReleaseTree(TreeNodeCollection topLevelNodes)
//    {
//        foreach (TreeNode node in topLevelNodes)
//        {
//            if (node.Tag is FolderItem folderItem)
//            {
//                Marshal.FinalReleaseComObject(node.Tag);
//            }
//            if (node.Nodes.Count > 0)
//            {
//                ReleaseTree(node.Nodes);
//            }
//        }
//    }

//    public static void ExpandFolder(this TreeNode tn, FolderTreeViewImageList imageList)
//    {
//        // if there's a dummy node present, clear it and replace with actual contents
//        if (tn.Nodes.Count != 1 || !ReferenceEquals(tn.Nodes[0].Tag, DUMMYNODE))
//            return;

//        tn.Nodes.Clear();

//        if (tn.Tag is not FolderItem folderItem || folderItem.GetFolder is not Folder folder)
//            return;

//        FolderItems items = folder.Items();
//        tn.TreeView?.BeginUpdate();
//        try
//        {
//            foreach (FolderItem item in items)
//            {
//                if (item.IsFolder && !item.IsBrowsable)
//                {
//                    TreeNode ntn = AddTreeNode(item, imageList);
//                    tn.Nodes.Add(ntn);
//                    AddSubFolderDummyNodes(ntn);
//                }
//            }
//        }
//        finally
//        {
//            tn.TreeView?.EndUpdate();
//            ReleaseCom(items);
//            ReleaseCom(folder);
//        }
//    }
//    #endregion

//    #region Private Methods
//    private static Folder GetFolder(KnownFolderAPI.SHShellFolder topFolder)
//    {
//        return  shell32.NameSpace(topFolder);
//    }
//    private static Folder GetFolder(string path)
//    {
//        return  shell32.NameSpace(path);
//    }
//    private static void AddRootNode(TreeView tree, FolderTreeViewImageList imageList, KnownFolderAPI.SHShellFolder rootFolder, bool getIcons)
//    {
//        Folder shell32RootFolder = GetFolder(rootFolder);
//        FolderItems rootItems = shell32RootFolder.Items();

//        TreeNode rootNode = new TreeNode(rootFolder.ToString(), 0, 0);

//        // Added in version 1.11
//        // add a FolderItem object to the root (Desktop) node tag that corresponds to the DesktopDirectory namespace
//        // This ensures that the GetSelectedNodePath will return the actual Desktop folder path when queried.
//        // There's possibly a better way to create a Shell32.FolderItem instance for this purpose, 
//        // but I surely don't know it

//        Folder dfolder = GetFolder(KnownFolderAPI.SHShellFolder.DESKTOPDIRECTORY);
//        FolderItems dfolderItems = dfolder.ParentFolder.Items();
//        try
//        {
//            foreach (FolderItem fi in dfolderItems)
//            {
//                if (fi.Name == dfolder.Title)
//                {
//                    rootNode.Tag = fi;
//                    break;
//                }
//            }
//        }
//        finally
//        {
//            ReleaseCom(dfolderItems);
//            ReleaseCom(dfolder);
//        }

//        // Add the Desktop root node to the tree
//        tree.Nodes.Add(rootNode);

//        // Get FolderItem that represents Recycle Bin
//        Folder recFolder = GetFolder(KnownFolderAPI.SHShellFolder.BITBUCKET);
//        FolderItems recFolderItems = recFolder.ParentFolder.Items();
//        FolderItem recycle = null;
//        try
//        {
//            foreach (FolderItem fi in recFolderItems)
//            {
//                if (fi.Name == recFolder.Title)
//                {
//                    recycle = fi;
//                    break;
//                }
//            }
//        }
//        finally
//        {
//            ReleaseCom(recFolderItems);
//            ReleaseCom(recFolder);
//        }

//        // Get FolderItem that represents My Network Places
//        Folder netFolder = GetFolder(KnownFolderAPI.SHShellFolder.NETWORK);
//        FolderItems netFolderItems = netFolder.ParentFolder.Items();
//        FolderItem mynetwork = null;
//        try
//        {
//            foreach (FolderItem fi in netFolderItems)
//            {
//                if (fi.Name == netFolder.Title)
//                {
//                    mynetwork = fi;
//                    break;
//                }
//            }
//        }
//        finally
//        {
//            ReleaseCom(netFolderItems);
//            ReleaseCom(netFolder);
//        }

//        // iterate through the Desktop namespace and populate the first level nodes
//        try
//        {
//            foreach (FolderItem item in rootItems)
//            {
//                if (!item.IsFolder) continue;  // this ensures that desktop shortcuts etc are not displayed
//                if (item.IsBrowsable) continue;  // exclude zip files
//                if (recycle != null && item.Path == recycle.Path) continue;  //  skip recycle bin

//                TreeNode tn = AddTreeNode(item, imageList, getIcons);
//                rootNode.Nodes.Add(tn);

//                Debug.WriteLine(item.Path);

//                if (mynetwork != null && item.Path == mynetwork.Path)  // skip my network places subdirs
//                {
//                    tree.Tag = tn;  // store node in tag and skip sub folder evaluation
//                    continue;
//                }

//                AddSubFolderDummyNodes(tn);
//            }
//        }
//        finally
//        {
//            ReleaseCom(rootItems);
//            ReleaseCom(shell32RootFolder);
//            ReleaseCom(recycle);
//            ReleaseCom(mynetwork);
//        }
//    }

//    private static void FillSubDirectories(TreeNode tn, FolderTreeViewImageList imageList, bool getIcons)
//    {
//        if (tn.Tag is not FolderItem folderItem || folderItem.GetFolder is not Folder folder)
//            return;

//        FolderItems items = folder.Items();
//        try
//        {
//            foreach (FolderItem item in items)
//            {
//                if (item.IsFolder && !item.IsBrowsable)
//                {
//                    TreeNode ntn = AddTreeNode(item, imageList, getIcons);
//                    tn.Nodes.Add(ntn);
//                    AddSubFolderDummyNodes(ntn);
//                }
//            }
//        }
//        finally
//        {
//            ReleaseCom(items);
//            ReleaseCom(folder);
//        }
//    }

//    private static void AddSubFolderDummyNodes(TreeNode tn)
//    {
//        if (tn.Nodes.Count != 0)
//            return;

//        // create dummy nodes for any subfolders that have further subfolders
//        if (tn.Tag is not FolderItem folderItem || folderItem.GetFolder is not Folder folder)
//            return;

//        FolderItems items = folder.Items();
//        try
//        {
//            foreach (FolderItem item in items)
//            {
//                if (item.IsFolder && !item.IsBrowsable)
//                {
//                    tn.Nodes.Add(new TreeNode { Tag = DUMMYNODE });
//                    break;
//                }
//            }
//        }
//        finally
//        {
//            ReleaseCom(items);
//            ReleaseCom(folder);
//        }
//    }

//    private static TreeNode AddTreeNode(FolderItem item, FolderTreeViewImageList imageList, bool getIcons = true)
//    {
//        TreeNode tn = new TreeNode
//        {
//            Text = item.Name,
//            Tag = item
//        };

//        int imageIndex = 1;
//        int selectedImageIndex = 2;

//        if (getIcons)
//        {
//            string name = item.Name;
//            string type = item.Type;
//            string path = item.Path;

//            string iconName = path;
//            if (string.Equals("System Folder", type, StringComparison.OrdinalIgnoreCase))
//                iconName = name;
//            else if (string.Equals("File Folder", type, StringComparison.OrdinalIgnoreCase))
//            {
//                iconName = type;
//                if (path.EndsWith(":\\", StringComparison.Ordinal))
//                    iconName = path;
//            }

//            imageIndex = imageList.GetIcon(iconName, true);
//            selectedImageIndex = imageList.GetIcon(iconName, false);

//            if (imageIndex != -1 && selectedImageIndex != -1)
//            {
//                tn.ImageIndex = imageIndex;
//                tn.SelectedImageIndex = selectedImageIndex;
//                return tn;
//            }

//            var normalIcon = FileIconAPI.GetIcon(path, false);
//            var selectedIcon = FileIconAPI.GetIcon(path, true);
//            if (normalIcon != null && selectedIcon != null)
//            {
//                imageIndex = imageList.AddIcon(iconName, normalIcon, false);
//                selectedImageIndex = imageList.AddIcon(iconName, selectedIcon, true);
//            }
//        }

//        tn.ImageIndex = imageIndex;
//        tn.SelectedImageIndex = selectedImageIndex;
//        return tn;
//    }
//    /// <summary>
//    /// Safely releases a COM object using Marshal.FinalReleaseComObject.
//    /// </summary>
//    private static void ReleaseCom(object comObject)
//    {
//        if (comObject is not null && Marshal.IsComObject(comObject))
//            Marshal.FinalReleaseComObject(comObject);
//    }
//    #endregion

//}
