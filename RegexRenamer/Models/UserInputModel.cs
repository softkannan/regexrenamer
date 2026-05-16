using System;
using System.Collections.Generic;
using Kavita.Enum;
using RegexRenamer.Rename;

namespace RegexRenamer.Models;

/// <summary>
/// Captures all user input state from the MainForm for rename operations.
/// </summary>
public record UserInputModel
{
    // Regex match and replace
    public string MatchPattern { get; init; } = string.Empty;
    public string ReplacePattern { get; init; } = string.Empty;

    // Regex modifiers
    public RegexModifierInfo Modifiers { get; init; } = new();

    public bool ShowInfo { get; init; } = false;

    // Filter settings
    public string FilterPattern { get; init; } = "*.*";
    public bool FilterExclude { get; init; }
    public bool FilterIsGlob { get; init; } = true;
    public bool IncludeSubfolders { get; init; }

    // Sort
    public string SortExpression { get; init; } = string.Empty;

    // Numbering
    public AutoNumberingInfo Numbering { get; init; } = new()
    {
        NumberingStart = "1",
        NumberingIncStep = "1",
        NumberingReset = "0",
        NumberingPad = "0",
        ValidNumber = true
    };

    // Change case
    public ChangeCaseOption ChangeCase { get; init; } = ChangeCaseOption.NoChange;

    // Output mode
    public OutputMode Output { get; init; } = OutputMode.RenameInPlace;
    public string MoveCopyPath { get; init; } = string.Empty;

    // Rename target
    public bool RenameFolders { get; init; }
    public bool RenameSelectionOnly { get; init; }

    // Options
    public bool ShowHiddenFiles { get; init; }
    public bool PreserveExtension { get; init; }
    public bool RealtimePreview { get; init; } = true;
    public bool AllowRenameIntoSubfolders { get; init; }

    // Kavita
    public KavitaInfo Kavita { get; init; } = new()
    {
        ShowPreview = false,
        KavitaRoot = string.Empty,
        KavitaLibType = LibraryType.Comic,
        UseMetadata = false
    };

    // Active path
    public string ActivePath { get; init; } = string.Empty;
}

/// <summary>
/// Specifies the output mode for rename operations.
/// </summary>
public enum OutputMode
{
    RenameInPlace,
    MoveTo,
    CopyTo,
    BackupTo
}
