using RegexRenamer.Rename;
using System;
using System.Drawing;

namespace RegexRenamer.Models;

/// <summary>
/// Backing data for a single virtual row in the dgvFiles DataGridView.
/// </summary>
internal class FileViewRowData
{
    public readonly int RowIndex;
    public RenameItemInfo FileInfo {get; private set;}

    public void UpdateFileInfo(RenameItemInfo newInfo)
    {
        ArgumentNullException.ThrowIfNull(newInfo);
        FileInfo = newInfo;
    }
    // Used only by single rename operations.
    public readonly int FileStoreIndex;
    public FileViewRowData(RenameItemInfo renameInfo, int fileStoreIndex, int rowIndex)
    {
        ArgumentNullException.ThrowIfNull(renameInfo);
        FileInfo = renameInfo;
        FileStoreIndex = fileStoreIndex;
        RowIndex = rowIndex;
    }
    // Icon for the file in the filename column (col 0).
    public Icon FileIcon { get; set; }
    // Error tooltip text for the preview column (col 2).
    public string PreviewErrorTag { get; set; }

    // ForeColor for the filename column (col 1).
    public Color FilenameForeColor { get; set; } = SystemColors.WindowText;

    // ForeColor for the preview column (col 2).
    public Color PreviewForeColor { get; set; } = SystemColors.WindowText;
}
