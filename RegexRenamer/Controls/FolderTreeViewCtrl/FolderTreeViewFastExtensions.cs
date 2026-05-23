using iText.StyledXmlParser.Jsoup.Nodes;
using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using RegexRenamer.Rename;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace RegexRenamer.Controls.FolderTreeViewCtrl;

/// <summary>
/// Defines the available folder tree enumeration strategies.
/// </summary>
public enum FolderTreeProviderType
{
    /// <summary>
    /// Fast file system enumeration using FileSystemEnumerable. Does not support shell special folders.
    /// </summary>
    Fast,

    /// <summary>
    /// Shell namespace enumeration using COM Shell32 APIs. Supports special/virtual folders.
    /// </summary>
    Shell
}

/// <summary>
/// Facade that delegates folder tree operations to the active <see cref="IFolderTreeProvider"/>.
/// Allows switching between different enumeration strategies at runtime.
/// </summary>
public static class FolderTreeViewExtensions
{
    private static IFolderTreeProvider _provider = new FastFolderTreeProvider();

    /// <summary>
    /// Gets or sets the active folder tree provider.
    /// </summary>
    public static IFolderTreeProvider Provider
    {
        get => _provider;
        set => _provider = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Switches the active provider by type.
    /// </summary>
    public static void SetProvider(FolderTreeProviderType providerType)
    {
        _provider = providerType switch
        {
            FolderTreeProviderType.Fast => new FastFolderTreeProvider(),
            FolderTreeProviderType.Shell => new ShellFolderTreeProvider(),
            _ => throw new ArgumentOutOfRangeException(nameof(providerType))
        };
    }

    /// <summary>
    /// Renames the folder represented by the TreeNode and returns the new path.
    /// </summary>
    public static string RenameFolder(this TreeNode node, string newLabel)
    {
        return _provider.RenameFolder(node, newLabel);
    }

    /// <summary>
    /// Returns the file system path of the folder represented by the TreeNode.
    /// </summary>
    public static string TreeNodeToPath(this TreeNode node)
    {
        return _provider.TreeNodeToPath(node);
    }

    /// <summary>
    /// Returns the file system path of the folder represented by the TreeNode if it exists.
    /// </summary>
    public static string FolderTreeNodeToDirectory(this TreeNode tn)
    {
        return _provider.FolderTreeNodeToDirectory(tn);
    }

    /// <summary>
    /// Returns a ShellItemInfo corresponding to the specified file name within the folder at path.
    /// </summary>
    public static ShellItemInfo GetShellFolderItem(this string path, string fileName)
    {
        return _provider.GetShellFolderItem(path, fileName);
    }

    /// <summary>
    /// Populates the given TreeView with the folder structure.
    /// </summary>
    public static void PopulateFolders(this TreeView tree, FolderTreeViewImageList imageList)
    {
        _provider.PopulateFolders(tree, imageList);
    }

    /// <summary>
    /// Expands a folder node by replacing its dummy child with actual subfolder nodes.
    /// </summary>
    public static void ExpandFolder(this TreeNode tn, FolderTreeViewImageList imageList)
    {
        _provider.ExpandFolder(tn, imageList);
    }

    public static bool BringToView(this TreeView pThis, string folderPath, bool recursive = false)
    {
        return _provider.BringToView(pThis, folderPath, recursive);
    }

    public static void RefreshNode(this TreeView pThis, string path)
    {
        _provider.RefreshNode(pThis, path);
    }

    public static void RefreshNode(this TreeView pThis, TreeNode tn, bool recursive = false)
    {
        _provider.RefreshNode(pThis, tn, recursive);
    }

}

