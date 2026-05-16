using System.Collections.Generic;

namespace RegexRenamer.Models;

/// <summary>
/// Holds per-file validation error messages and aggregate counts produced by <see cref="Rename.ValidationService"/>.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Per-file error messages keyed by active-file index. Null value means no error.
    /// </summary>
    public Dictionary<int, string> FileErrors { get; } = [];

    /// <summary>
    /// Number of files that matched the regex.
    /// </summary>
    public int MatchedCount { get; set; }

    /// <summary>
    /// Number of files with validation conflicts/errors.
    /// </summary>
    public int ConflictCount { get; set; }
}

/// <summary>
/// Result of pre-rename checks performed before launching the background rename operation.
/// </summary>
public class PreRenameCheckResult
{
    /// <summary>
    /// Error message that should block the rename, or null if checks passed.
    /// </summary>
    public string ErrorMessage { get; init; }

    /// <summary>
    /// Number of files that will be renamed.
    /// </summary>
    public int FilesToRename { get; init; }

    /// <summary>
    /// Whether any preview filenames begin with a space or dot.
    /// </summary>
    public bool HasInvalidStartChars { get; init; }

    /// <summary>
    /// True if all checks passed and rename can proceed.
    /// </summary>
    public bool CanProceed => ErrorMessage == null;
}
