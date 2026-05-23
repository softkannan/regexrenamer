using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using System.Windows.Forms;

namespace RegexRenamer.Controls.FolderTreeViewCtrl;

/// <summary>
/// Defines the contract for folder tree enumeration providers.
/// Implementations can use different strategies (fast file system, shell namespace, etc.).
/// </summary>
public interface IFolderTreeProvider
{
    /// <summary>
    /// Renames the folder represented by the TreeNode and returns the new path.
    /// Returns empty string if rename fails or if the node is not a folder.
    /// </summary>
    string RenameFolder(TreeNode node, string newLabel);

    /// <summary>
    /// Returns the file system path of the folder represented by the TreeNode.
    /// </summary>
    string TreeNodeToPath(TreeNode node);

    /// <summary>
    /// Returns the file system path of the folder represented by the TreeNode if it exists.
    /// </summary>
    string FolderTreeNodeToDirectory(TreeNode tn);

    /// <summary>
    /// Returns a ShellItemInfo corresponding to the specified file name within the folder at path.
    /// </summary>
    ShellItemInfo GetShellFolderItem(string path, string fileName);

    /// <summary>
    /// Populates the given TreeView with the folder structure.
    /// </summary>
    void PopulateFolders(TreeView tree, FolderTreeViewImageList imageList);

    /// <summary>
    /// Expands a folder node by replacing its dummy child with actual subfolder nodes.
    /// </summary>
    void ExpandFolder(TreeNode tn, FolderTreeViewImageList imageList);

    bool BringToView(TreeView pThis, string folderPath, bool recursive = false);
    void RefreshNode(TreeView pThis, string path);
    void RefreshNode(TreeView pThis, TreeNode tn, bool recursive = false);
}
