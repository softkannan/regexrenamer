using System.Drawing;

namespace RegexRenamer.Models;

/// <summary>
/// Backing data for a single virtual row in the dgvFiles DataGridView.
/// </summary>
internal sealed class FileViewRowData
{
    private const int ColumnCount = 12;

    /// <summary>Index into the active files list (_fileStore.Files).</summary>
    public int ActiveFileIndex { get; set; }

    /// <summary>Cell values indexed by column ordinal.</summary>
    public object[] CellValues { get; } = new object[ColumnCount];

    /// <summary>Error tooltip text for the preview column (col 2).</summary>
    public string PreviewErrorTag { get; set; }

    /// <summary>ForeColor for the filename column (col 1).</summary>
    public Color FilenameForeColor { get; set; } = SystemColors.WindowText;

    /// <summary>ForeColor for the preview column (col 2).</summary>
    public Color PreviewForeColor { get; set; } = SystemColors.WindowText;
}
