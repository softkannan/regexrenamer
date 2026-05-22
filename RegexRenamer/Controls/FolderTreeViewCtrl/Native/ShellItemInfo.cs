namespace RegexRenamer.Controls.FolderTreeViewCtrl.Native;

/// <summary>
/// Lightweight data holder for folder tree nodes. Stores only the filesystem path.
/// </summary>
public sealed record ShellItemInfo(string Path, string Name);
