using PInvoke;
using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using RegexRenamer.Rename;
using System;
using System.IO;
using System.IO.Enumeration;
using System.Windows.Forms;

namespace RegexRenamer.Controls.FolderTreeViewCtrl;

/// <summary>
/// Fast file system folder tree provider using FileSystemEnumerable.
/// Does not enumerate the shell namespace or special folders, but is much faster for regular file system folders.
/// </summary>
public sealed class FastFolderTreeProvider : IFolderTreeProvider
{
    private const string DUMMYNODE = "DUMMYNODE";
    private bool _isUpdating = false;

    /// <inheritdoc />
    public string RenameFolder(TreeNode node, string newLabel)
    {
        if (node.Tag is not ShellItemInfo itemInfo)
            return "";

        var parentDir = FastPath.GetParentDirectory(itemInfo.Path);
        if (string.IsNullOrEmpty(parentDir))
            return "";

        var newPath = Path.Combine(parentDir, newLabel);
        FileOperationAPI.RenameFolder(itemInfo.Path, newPath);

        return newPath;
    }

    /// <inheritdoc />
    public string TreeNodeToPath(TreeNode node)
    {
        return node.Tag is ShellItemInfo itemInfo ? itemInfo.Path : "";
    }

    /// <inheritdoc />
    public string FolderTreeNodeToDirectory(TreeNode tn)
    {
        if (tn.Tag is not ShellItemInfo itemInfo)
            return "";

        return FastPath.DirectoryExists(itemInfo.Path) ? itemInfo.Path : "";
    }

    /// <inheritdoc />
    public ShellItemInfo GetShellFolderItem(string path, string fileName)
    {
        string fullPath = Path.Combine(path, fileName);
        return Directory.Exists(fullPath) || File.Exists(fullPath)
            ? new ShellItemInfo(fullPath, fileName, fileName)
            : null;
    }

    /// <inheritdoc />
    public void PopulateFolders(TreeView tree, FolderTreeViewImageList imageList)
    {
        tree.BeginUpdate();
        try
        {
            tree.Nodes.Clear();
            AddDriveNodes(tree, imageList);
            if (tree.Nodes.Count > 0)
            {
                tree.SelectedNode = tree.Nodes[0];
                tree.Nodes[0].ExpandFolder(imageList);
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

        if (tn.Tag is not ShellItemInfo itemInfo)
            return;

        tn.TreeView?.BeginUpdate();
        try
        {
            EnumerateAndAddSubFolders(tn, itemInfo.Path, imageList);
        }
        finally
        {
            tn.TreeView?.EndUpdate();
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
                pThis.BeginUpdate();
                if (folderPath.Length > 3 && folderPath.LastIndexOf("\\") == folderPath.Length - 1)
                    folderPath = folderPath.Substring(0, folderPath.Length - 1);

                var trimedPath = Path.GetFullPath(folderPath + "\\");
                var parts = trimedPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
                int startIndex = 0;
                int endIndex = parts.Length - 1;
                foreach (TreeNode tn in pThis.Nodes)
                {
                    if (tn.Tag is ShellItemInfo itemInfo && string.Equals(itemInfo.Part, parts[startIndex], StringComparison.OrdinalIgnoreCase))
                    {
                        if (parts.Length > startIndex)
                        {
                            tn.Expand();
                            DrillTree(pThis, tn, parts, startIndex + 1, endIndex, ref folderFound);
                            if (folderFound) break;
                        }
                        else
                        {
                            pThis.SelectedNode = tn;
                            tn.EnsureVisible();
                            folderFound = true;
                            break;
                        }
                    }
                    else
                    {
                        tn.Collapse();
                    }
                }
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

    public void RefreshNode(TreeView pThis, string path) 
    {
        if (_isUpdating) return;

        try
        {
            _isUpdating = true;

            bool foundParent = false, foundChild = false;

            foreach (TreeNode rootNode in pThis.Nodes)
            {
                TreeNode currentNode = rootNode;
                while (true)
                {
                    foundParent = false;
                    foreach (TreeNode tn in currentNode.Nodes)
                    {
                        if (tn.Tag.ToString() == "DUMMYNODE")
                            return;
                        string nodePath = tn.TreeNodeToPath();
                        if (string.Compare(path, nodePath, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            currentNode = tn;
                            foundChild = true;
                            break;
                        }
                        else if (path.StartsWith(nodePath, StringComparison.OrdinalIgnoreCase))
                        {
                            currentNode = tn;
                            foundParent = true;
                            break;
                        }
                    }
                    if (foundChild || !foundParent) break;
                }

                RefreshNode(pThis, currentNode, true);
            }
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

    public void RefreshNode(TreeView pThis, TreeNode tn, bool recursive = false)
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


    private void DrillTree(TreeView pThis, TreeNode parentNode, string[] parts, int startIndex, int endIndex, ref bool folderFound)
    {
        foreach (TreeNode tn in parentNode.Nodes)
        {
            if (tn.Tag is ShellItemInfo itemInfo && string.Equals(itemInfo.Part, parts[startIndex], StringComparison.OrdinalIgnoreCase))
            {
                if (endIndex > startIndex)
                {
                    tn.Expand();
                    DrillTree(pThis, tn, parts, startIndex + 1, endIndex, ref folderFound);
                    if (folderFound) break;
                }
                else
                {
                    pThis.SelectedNode = tn;
                    tn.EnsureVisible();
                    folderFound = true;
                    break;
                }
            }
        }
    }

    private static void AddDriveNodes(TreeView tree, FolderTreeViewImageList imageList)
    {
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            if (!drive.IsReady)
                continue;

            string name = string.IsNullOrEmpty(drive.VolumeLabel)
                ? $"{drive.Name.TrimEnd('\\')}"
                : $"{drive.VolumeLabel} ({drive.Name.TrimEnd('\\')})";

            ShellItemInfo info = new(drive.RootDirectory.FullName, name, drive.Name.TrimEnd('\\'));
            TreeNode tn = AddTreeNode(info, imageList);
            tree.Nodes.Add(tn);
            AddSubFolderDummyNode(tn, info.Path);
        }
    }

    private static void EnumerateAndAddSubFolders(TreeNode parentNode, string parentPath, FolderTreeViewImageList imageList)
    {
        var enumerable = new FileSystemEnumerable<string>(
            parentPath,
            static (ref FileSystemEntry entry) => entry.ToFullPath(),
            new EnumerationOptions
            {
                IgnoreInaccessible = true,
                AttributesToSkip = FileAttributes.System | FileAttributes.Hidden,
                RecurseSubdirectories = false,
            })
        {
            ShouldIncludePredicate = static (ref FileSystemEntry entry) => entry.IsDirectory
        };

        foreach (string fullPath in enumerable)
        {
            string folderName = Path.GetFileName(fullPath);
            ShellItemInfo info = new(fullPath, folderName, folderName);
            TreeNode ntn = AddTreeNode(info, imageList);
            parentNode.Nodes.Add(ntn);
            AddSubFolderDummyNode(ntn, fullPath);
        }
    }

    private static void AddSubFolderDummyNode(TreeNode tn, string path)
    {
        if (tn.Nodes.Count != 0)
            return;

        if (HasSubDirectories(path))
        {
            tn.Nodes.Add(new TreeNode { Tag = DUMMYNODE });
        }
    }

    private static bool HasSubDirectories(string path)
    {
        var enumerable = new FileSystemEnumerable<bool>(
            path,
            static (ref FileSystemEntry _) => true,
            new EnumerationOptions
            {
                IgnoreInaccessible = true,
                AttributesToSkip = FileAttributes.System | FileAttributes.Hidden,
                RecurseSubdirectories = false,
            })
        {
            ShouldIncludePredicate = static (ref FileSystemEntry entry) => entry.IsDirectory
        };

        foreach (bool _ in enumerable)
            return true;

        return false;
    }

    private static TreeNode AddTreeNode(ShellItemInfo item, FolderTreeViewImageList imageList, bool getIcons = true)
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
            string path = item.Path;
            string iconName = path.EndsWith(":\\", StringComparison.Ordinal) ? path : "File Folder";

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

    #endregion
}
