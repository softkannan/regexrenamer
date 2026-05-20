using System.Drawing;

namespace RegexRenamer.Models;

/// <summary>
/// Backing data for a single virtual row in the dgvFiles DataGridView.
/// </summary>
internal class FileViewRowData
{
    // The number of columns in the dgvFiles DataGridView. This is used to size the CellValues array.
    private const int ColumnCount = 12;
    // Index into the files list (_fileStore.Files)
    public readonly int FileStoreIndex;

    public FileViewRowData(int fileIndex)
    {
        FileStoreIndex = fileIndex;
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
