using Interop.Shell32;
using PInvoke;
using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using RegexRenamer.Rename;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RegexRenamer.Controls.FolderTreeViewCtrl;

/// <summary>
/// Shell namespace folder tree provider.
/// Uses COM Shell32 APIs to enumerate shell folders including special/virtual folders.
/// </summary>
public sealed class ShellFolderTreeProvider : IFolderTreeProvider
{
    private const string DUMMYNODE = "DUMMYNODE";
    private static readonly ShellClass shell32 = new ShellClass();
    private bool _isUpdating = false;

    /// <inheritdoc />
    public string RenameFolder(TreeNode node, string newLabel)
    {
        if (node.Tag is not FolderItem folderItem)
            return "";

        var parentDir = FastPath.GetParentDirectory(folderItem.Path);
        if (string.IsNullOrEmpty(parentDir))
            return "";

        var newPath = $"{parentDir}\\{newLabel}";
        FileOperationAPI.RenameFolder(folderItem.Path, newPath);

        return newPath;
    }

    /// <inheritdoc />
    public string TreeNodeToPath(TreeNode node)
    {
        return node.Tag is FolderItem folderItem ? folderItem.Path : "";
    }

    /// <inheritdoc />
    public string FolderTreeNodeToDirectory(TreeNode tn)
    {
        if (tn.Tag is not FolderItem folderItem)
            return "";

        return FastPath.DirectoryExists(folderItem.Path) ? folderItem.Path : "";
    }

    /// <inheritdoc />
    public ShellItemInfo GetShellFolderItem(string path, string fileName)
    {
        Folder folder = GetFolder(path);
        FolderItem item = folder.ParseName(fileName);
        if (item is null)
            return null;

        return new ShellItemInfo(item.Path, item.Name, item.Name);
    }

    /// <inheritdoc />
    public void PopulateFolders(TreeView tree, FolderTreeViewImageList imageList)
    {
        tree.BeginUpdate();
        try
        {
            ReleaseTree(tree.Nodes);
            tree.Nodes.Clear();
            AddRootNode(tree, imageList, KnownFolderAPI.SHShellFolder.DESKTOP, true);
            if (tree.Nodes.Count > 1)
            {
                tree.SelectedNode = tree.Nodes[1];
                ExpandFolder(tree.Nodes[1], imageList);
            }
        }
        finally
        {
            tree.EndUpdate();
        }
    }

    /// <inheritdoc />
    public void ExpandFolder(TreeNode tn, FolderTreeViewImageList imageList)
    {
        if (tn.Nodes.Count != 1 || !ReferenceEquals(tn.Nodes[0].Tag, DUMMYNODE))
            return;

        tn.Nodes.Clear();

        if (tn.Tag is not FolderItem folderItem || folderItem.GetFolder is not Folder folder)
            return;

        FolderItems items = folder.Items();
        tn.TreeView?.BeginUpdate();
        try
        {
            foreach (FolderItem item in items)
            {
                if (item.IsFolder && !item.IsBrowsable)
                {
                    TreeNode ntn = AddTreeNode(item, imageList);
                    tn.Nodes.Add(ntn);
                    AddSubFolderDummyNodes(ntn);
                }
            }
        }
        finally
        {
            tn.TreeView?.EndUpdate();
            ReleaseCom(items);
            ReleaseCom(folder);
        }
    }

    public bool BringToView(TreeView pThis, string folderPath, bool recursive = false)
    {
        if (_isUpdating && recursive == false) return false;
        bool folderFound = false;
        try
        {
            _isUpdating = true;
            if (FastPath.DirectoryExists(folderPath))
            {
                // Assume My Computer is first node under Desktop and start drilling from there
                TreeNode topNode = pThis.Nodes[0];
                pThis.BeginUpdate();
                if (folderPath.Length > 3 && folderPath.LastIndexOf("\\") == folderPath.Length - 1)
                    folderPath = folderPath.Substring(0, folderPath.Length - 1);

                DrillTree(pThis, topNode.Nodes, folderPath, ref folderFound);
                pThis.EndUpdate();
            }
            if (!folderFound)
            {
                pThis.SelectedNode = pThis.Nodes[0];
            }
        }
        finally
        {
            _isUpdating = false;
        }
        return folderFound;
    }

    public void RefreshNode(TreeView pThis, string path)  // [xiperware]
    {
        if (_isUpdating) return;

        try
        {
            _isUpdating = true;

            TreeNode currentNode = pThis.Nodes[0].Nodes[0];  // assume My Computer is first node under Desktop
            bool foundParent = false, foundChild = false;

            while (true)
            {
                foundParent = false;
                foreach (TreeNode node in currentNode.Nodes)
                {
                    if (node.Tag.ToString() == "DUMMYNODE")
                        return;
                    string nodePath = node.TreeNodeToPath();
                    if (string.Compare(path, nodePath, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        currentNode = node;
                        foundChild = true;
                        break;
                    }
                    else if (path.StartsWith(nodePath, StringComparison.OrdinalIgnoreCase))
                    {
                        currentNode = node;
                        foundParent = true;
                        break;
                    }
                }
                if (foundChild || !foundParent) break;
            }

            RefreshNode(pThis, currentNode, true);
        }
        catch
        {
            return;
        }
        finally
        {
            _isUpdating = false;
        }
    }

    public void RefreshNode(TreeView pThis, TreeNode tn, bool recursive = false)  // [xiperware]
    {
        if (_isUpdating && recursive == false) return;

        try
        {
            _isUpdating = true;

            if (pThis.Tag != null && tn == (TreeNode)pThis.Tag)  // My Network Places
                return;

            if (tn.Nodes.Count == 1 && tn.Nodes[0].Tag.ToString() == "DUMMYNODE")
                return;

            pThis.BeginUpdate();

            TreeNode selectedNode = pThis.SelectedNode;

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

            if (pThis.SelectedNode != selectedNode)
                BringToView(pThis, selectedNode.TreeNodeToPath(), true);

            pThis.EndUpdate();
        }
        finally
        {
            _isUpdating = false;
        }
    }

    #region Private Methods

    private void DrillTree(TreeView pThis, TreeNodeCollection tnc, string path, ref bool folderFound)
    {
        foreach (TreeNode tn in tnc)
        {
            if (!folderFound)
            {
                pThis.SelectedNode = tn;
                string tnPath = tn.FolderTreeNodeToDirectory();
                if (path.Equals(tnPath, StringComparison.OrdinalIgnoreCase) && !folderFound)
                {
                    pThis.SelectedNode = tn;
                    tn.EnsureVisible();
                    folderFound = true;
                    break;
                }
                else if (path.IndexOf(tnPath) > -1 && !folderFound)
                {
                    tn.Expand();
                    DrillTree(pThis, tn.Nodes, path, ref folderFound);
                }
            }
        }
    }

    private static Folder GetFolder(KnownFolderAPI.SHShellFolder topFolder)
    {
        return shell32.NameSpace(topFolder);
    }

    private static Folder GetFolder(string path)
    {
        return shell32.NameSpace(path);
    }

    private static void ReleaseTree(TreeNodeCollection topLevelNodes)
    {
        foreach (TreeNode node in topLevelNodes)
        {
            if (node.Tag is FolderItem)
            {
                Marshal.FinalReleaseComObject(node.Tag);
            }
            if (node.Nodes.Count > 0)
            {
                ReleaseTree(node.Nodes);
            }
        }
    }

    private static void AddRootNode(TreeView tree, FolderTreeViewImageList imageList, KnownFolderAPI.SHShellFolder rootFolder, bool getIcons)
    {
        Folder shell32RootFolder = GetFolder(rootFolder);
        FolderItems rootItems = shell32RootFolder.Items();

        TreeNode rootNode = new TreeNode(rootFolder.ToString(), 0, 0);

        // Add a FolderItem object to the root (Desktop) node tag that corresponds to the DesktopDirectory namespace
        Folder dfolder = GetFolder(KnownFolderAPI.SHShellFolder.DESKTOPDIRECTORY);
        FolderItems dfolderItems = dfolder.ParentFolder.Items();
        try
        {
            foreach (FolderItem fi in dfolderItems)
            {
                if (fi.Name == dfolder.Title)
                {
                    rootNode.Tag = fi;
                    break;
                }
            }
        }
        finally
        {
            ReleaseCom(dfolderItems);
            ReleaseCom(dfolder);
        }

        tree.Nodes.Add(rootNode);

        // Get FolderItem that represents Recycle Bin
        Folder recFolder = GetFolder(KnownFolderAPI.SHShellFolder.BITBUCKET);
        FolderItems recFolderItems = recFolder.ParentFolder.Items();
        FolderItem recycle = null;
        try
        {
            foreach (FolderItem fi in recFolderItems)
            {
                if (fi.Name == recFolder.Title)
                {
                    recycle = fi;
                    break;
                }
            }
        }
        finally
        {
            ReleaseCom(recFolderItems);
            ReleaseCom(recFolder);
        }

        // Get FolderItem that represents My Network Places
        Folder netFolder = GetFolder(KnownFolderAPI.SHShellFolder.NETWORK);
        FolderItems netFolderItems = netFolder.ParentFolder.Items();
        FolderItem mynetwork = null;
        try
        {
            foreach (FolderItem fi in netFolderItems)
            {
                if (fi.Name == netFolder.Title)
                {
                    mynetwork = fi;
                    break;
                }
            }
        }
        finally
        {
            ReleaseCom(netFolderItems);
            ReleaseCom(netFolder);
        }

        // Iterate through the Desktop namespace and populate the first level nodes
        try
        {
            foreach (FolderItem item in rootItems)
            {
                if (!item.IsFolder) continue;
                if (item.IsBrowsable) continue;
                if (recycle != null && item.Path == recycle.Path) continue;

                TreeNode tn = AddTreeNode(item, imageList, getIcons);
                rootNode.Nodes.Add(tn);

                Debug.WriteLine(item.Path);

                if (mynetwork != null && item.Path == mynetwork.Path)
                {
                    tree.Tag = tn;
                    continue;
                }

                AddSubFolderDummyNodes(tn);
            }
        }
        finally
        {
            ReleaseCom(rootItems);
            ReleaseCom(shell32RootFolder);
            ReleaseCom(recycle);
            ReleaseCom(mynetwork);
        }
    }

    private static void AddSubFolderDummyNodes(TreeNode tn)
    {
        if (tn.Nodes.Count != 0)
            return;

        if (tn.Tag is not FolderItem folderItem || folderItem.GetFolder is not Folder folder)
            return;

        FolderItems items = folder.Items();
        try
        {
            foreach (FolderItem item in items)
            {
                if (item.IsFolder && !item.IsBrowsable)
                {
                    tn.Nodes.Add(new TreeNode { Tag = DUMMYNODE });
                    break;
                }
            }
        }
        finally
        {
            ReleaseCom(items);
            ReleaseCom(folder);
        }
    }

    private static TreeNode AddTreeNode(FolderItem item, FolderTreeViewImageList imageList, bool getIcons = true)
    {
        TreeNode tn = new TreeNode
        {
            Text = item.Name,
            Tag = item
        };

        int imageIndex = 1;
        int selectedImageIndex = 2;

        if (getIcons)
        {
            string name = item.Name;
            string type = item.Type;
            string path = item.Path;

            string iconName = path;
            if (string.Equals("System Folder", type, StringComparison.OrdinalIgnoreCase))
                iconName = name;
            else if (string.Equals("File Folder", type, StringComparison.OrdinalIgnoreCase))
            {
                iconName = type;
                if (path.EndsWith(":\\", StringComparison.Ordinal))
                    iconName = path;
            }

            imageIndex = imageList.GetIcon(iconName, true);
            selectedImageIndex = imageList.GetIcon(iconName, false);

            if (imageIndex != -1 && selectedImageIndex != -1)
            {
                tn.ImageIndex = imageIndex;
                tn.SelectedImageIndex = selectedImageIndex;
                return tn;
            }

            var normalIcon = FileIconAPI.GetIcon(path, false);
            var selectedIcon = FileIconAPI.GetIcon(path, true);
            if (normalIcon != null && selectedIcon != null)
            {
                imageIndex = imageList.AddIcon(iconName, normalIcon, false);
                selectedImageIndex = imageList.AddIcon(iconName, selectedIcon, true);
            }
        }

        tn.ImageIndex = imageIndex;
        tn.SelectedImageIndex = selectedImageIndex;
        return tn;
    }

    /// <summary>
    /// Safely releases a COM object using Marshal.FinalReleaseComObject.
    /// </summary>
    private static void ReleaseCom(object comObject)
    {
        if (comObject is not null && Marshal.IsComObject(comObject))
            Marshal.FinalReleaseComObject(comObject);
    }

    #endregion
}
