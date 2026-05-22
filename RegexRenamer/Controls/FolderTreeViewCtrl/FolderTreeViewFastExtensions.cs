using PInvoke;
using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using RegexRenamer.Rename;
using System;
using System.IO;
using System.IO.Enumeration;
using System.Windows.Forms;

namespace RegexRenamer.Controls.FolderTreeViewCtrl;

// This class contains extension methods for TreeNode and TreeView to support folder tree view functionality using fast enumeration APIs.
// This will not enumurate the shell namespace or special folders, but will be much faster for regular file system folders.
// It also provides a method to rename folders represented by TreeNodes.
public static class FolderTreeViewExtensions
{
    private const string DUMMYNODE = "DUMMYNODE";

    #region Public methods

    /// <summary>
    /// Renames the folder represented by the TreeNode and returns the new path.
    /// Returns empty string if rename fails or if the node is not a folder.
    /// </summary>
    public static string RenameFolder(this TreeNode node, string newLabel)
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

    /// <summary>
    /// Returns the file system path of the folder represented by the TreeNode.
    /// </summary>
    public static string TreeNodeToPath(this TreeNode node)
    {
        return node.Tag is ShellItemInfo itemInfo ? itemInfo.Path : "";
    }

    /// <summary>
    /// Returns the file system path of the folder represented by the TreeNode if it exists.
    /// </summary>
    public static string FolderTreeNodeToDirectory(this TreeNode tn)
    {
        if (tn.Tag is not ShellItemInfo itemInfo)
            return "";

        return FastPath.DirectoryExists(itemInfo.Path) ? itemInfo.Path : "";
    }

    /// <summary>
    /// Returns a ShellItemInfo corresponding to the specified file name within the folder at path.
    /// </summary>
    public static ShellItemInfo GetShellFolderItem(this string path, string fileName)
    {
        string fullPath = Path.Combine(path, fileName);
        return Directory.Exists(fullPath) || File.Exists(fullPath)
            ? new ShellItemInfo(fullPath, fileName, fileName)
            : null;
    }

    /// <summary>
    /// Populates the given TreeView with the folder structure starting from the Desktop.
    /// </summary>
    public static void PopulateFolders(this TreeView tree, FolderTreeViewImageList imageList)
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

    /// <summary>
    /// Expands a folder node by replacing its dummy child with actual subfolder nodes.
    /// </summary>
    public static void ExpandFolder(this TreeNode tn, FolderTreeViewImageList imageList)
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

    #endregion

    #region Private Methods

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

