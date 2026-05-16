using System;
using Kavita.Enum;
using RegexRenamer.Models;
using RegexRenamer.Rename;

namespace RegexRenamer;

public partial class MainForm
{
    UserInputModel _lastUserInputSnapshot;
    UserInputModel _currentInput;

    /// <summary>
    /// Captures the current form state into a <see cref="UserInputModel"/> snapshot.
    /// </summary>
    private UserInputModel GetUserInput()
    {
        return new UserInputModel
        {
            MatchPattern = cmbMatch.Text,
            ReplacePattern = cmbReplace.Text,

            Modifiers = new RegexModifierInfo
            {
                IgnoreCase = cbModifierI.Checked,
                ReplaceEveryMatch = cbModifierG.Checked,
                IgnorePatternWhitespace = cbModifierX.Checked
            },

            ShowInfo = chkShowInfo.Checked,
            FilterPattern = cmbFilter.Text,
            FilterExclude = cbFilterExclude.Checked,
            FilterIsGlob = rbFilterGlob.Checked,
            IncludeSubfolders = chkIncludeSubfolder.Checked,

            SortExpression = cmbSort.Text,

            Numbering = new AutoNumberingInfo
            {
                ValidNumber = _validNumber,
                NumberingStart = txtNumberingStart.Text,
                NumberingIncStep = txtNumberingInc.Text,
                NumberingReset = txtNumberingReset.Text,
                NumberingPad = txtNumberingPad.Text
            },

            ChangeCase = GetChangeCaseInfo(),

            Output = GetOutputMode(),
            MoveCopyPath = fbdMoveCopy.SelectedPath,

            RenameFolders = RenameFolders,
            RenameSelectionOnly = _renameSelectionOnly,

            ShowHiddenFiles = itmOptionsShowHidden.Checked,
            PreserveExtension = itmOptionsPreserveExt.Checked,
            RealtimePreview = itmOptionsRealtimePreview.Checked,
            AllowRenameIntoSubfolders = itmOptionsAllowRenSub.Checked,

            Kavita = new KavitaInfo
            {
                ShowPreview = !noneKavitaMenuItem.Checked,
                KavitaRoot = _kavitaLibRootpath,
                KavitaLibType = _kavitaPreviewLibType,
                UseMetadata = _kavitaUseMetadata
            },

            ActivePath = _activePath
        };
    }

    /// <summary>
    /// Applies a <see cref="UserInputModel"/> to the form controls, restoring user input state.
    /// </summary>
    private void SetUserInput(UserInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        EnableUpdates = false;

        // Regex match and replace
        cmbMatch.Text = model.MatchPattern;
        cmbReplace.Text = model.ReplacePattern;

        // Modifiers
        cbModifierI.Checked = model.Modifiers.IgnoreCase;
        cbModifierG.Checked = model.Modifiers.ReplaceEveryMatch;
        cbModifierX.Checked = model.Modifiers.IgnorePatternWhitespace;

        // Show info
        chkShowInfo.Checked = model.ShowInfo;

        // Filter
        cmbFilter.Text = model.FilterPattern;
        cbFilterExclude.Checked = model.FilterExclude;
        rbFilterGlob.Checked = model.FilterIsGlob;
        rbFilterRegex.Checked = !model.FilterIsGlob;
        chkIncludeSubfolder.Checked = model.IncludeSubfolders;

        // Sort
        cmbSort.Text = model.SortExpression;

        // Numbering
        txtNumberingStart.Text = model.Numbering.NumberingStart;
        txtNumberingInc.Text = model.Numbering.NumberingIncStep;
        txtNumberingReset.Text = model.Numbering.NumberingReset;
        txtNumberingPad.Text = model.Numbering.NumberingPad;

        // Change case
        SetChangeCaseOption(model.ChangeCase);

        // Output mode
        SetOutputMode(model.Output);
        fbdMoveCopy.SelectedPath = model.MoveCopyPath;

        // Rename target
        RenameFolders = model.RenameFolders;
        _renameSelectionOnly = model.RenameSelectionOnly;
        chkRenameSelectionOnly.Checked = model.RenameSelectionOnly;

        // Options
        itmOptionsShowHidden.Checked = model.ShowHiddenFiles;
        itmOptionsPreserveExt.Checked = model.PreserveExtension;
        itmOptionsRealtimePreview.Checked = model.RealtimePreview;
        itmOptionsAllowRenSub.Checked = model.AllowRenameIntoSubfolders;

        // Kavita
        _kavitaLibRootpath = model.Kavita.KavitaRoot;
        _kavitaPreviewLibType = model.Kavita.KavitaLibType;
        _kavitaUseMetadata = model.Kavita.UseMetadata;

        // Active path
        _activePath = model.ActivePath;

        EnableUpdates = true;
    }

    /// <summary>
    /// Returns the current output mode based on checked menu items.
    /// </summary>
    private OutputMode GetOutputMode()
    {
        if (itmOutputMoveTo.Checked) return OutputMode.MoveTo;
        if (itmOutputCopyTo.Checked) return OutputMode.CopyTo;
        if (itmOutputBackupTo.Checked) return OutputMode.BackupTo;
        return OutputMode.RenameInPlace;
    }

    /// <summary>
    /// Sets the output mode menu item check states.
    /// </summary>
    private void SetOutputMode(OutputMode mode)
    {
        itmOutputRenameInPlace.Checked = mode == OutputMode.RenameInPlace;
        itmOutputMoveTo.Checked = mode == OutputMode.MoveTo;
        itmOutputCopyTo.Checked = mode == OutputMode.CopyTo;
        itmOutputBackupTo.Checked = mode == OutputMode.BackupTo;
    }

    /// <summary>
    /// Sets the change case menu item check states.
    /// </summary>
    private void SetChangeCaseOption(ChangeCaseOption option)
    {
        itmChangeCaseNoChange.Checked = option == ChangeCaseOption.NoChange;
        itmChangeCaseUppercase.Checked = option == ChangeCaseOption.Uppercase;
        itmChangeCaseLowercase.Checked = option == ChangeCaseOption.Lowercase;
        itmChangeCaseTitlecase.Checked = option == ChangeCaseOption.Titlecase;
        itmChangeCaseCleanName.Checked = option == ChangeCaseOption.CleanName;
    }


    /// <summary>
    /// Compares the current form state against the last snapshot, determines
    /// which <see cref="UpdateStage"/> is required, performs the refresh, and
    /// saves the new snapshot.
    /// </summary>
    private void UpdateUserInputValues()
    {
        UserInputModel currentInput = GetUserInput();

        // Initialize snapshot on first call
        if (_lastUserInputSnapshot == null)
        {
            _lastUserInputSnapshot = currentInput;
            _currentInput = currentInput;
            return;
        }

        UpdateStage requiredStage = ComputeRequiredStage(currentInput, _lastUserInputSnapshot);

        // Update snapshots
        _lastUserInputSnapshot = currentInput;
        _currentInput = currentInput;

        if (requiredStage != UpdateStage.None)
            RefreshView(requiredStage);
    }

    /// <summary>
    /// Determines the minimum <see cref="UpdateStage"/> needed based on which
    /// input fields changed between the current and previous snapshots.
    /// Changes are evaluated from highest impact (FileList) to lowest (Validation).
    /// </summary>
    private static UpdateStage ComputeRequiredStage(UserInputModel current, UserInputModel previous)
    {
        // --- Changes that require a full file list rebuild ---

        if (!string.Equals(current.ActivePath, previous.ActivePath, StringComparison.OrdinalIgnoreCase))
            return UpdateStage.FileList;

        if (!string.Equals(current.FilterPattern, previous.FilterPattern, StringComparison.OrdinalIgnoreCase))
            return UpdateStage.FileList;

        if (current.FilterExclude != previous.FilterExclude)
            return UpdateStage.FileList;

        if (current.FilterIsGlob != previous.FilterIsGlob)
            return UpdateStage.FileList;

        if (current.IncludeSubfolders != previous.IncludeSubfolders)
            return UpdateStage.FileList;

        if (current.ShowHiddenFiles != previous.ShowHiddenFiles)
            return UpdateStage.FileList;

        if (current.RenameFolders != previous.RenameFolders)
            return UpdateStage.FileList;

        if (current.RenameSelectionOnly != previous.RenameSelectionOnly)
            return UpdateStage.FileList;

        if (current.PreserveExtension != previous.PreserveExtension)
            return UpdateStage.FileList;

        // --- Changes that require a preview regeneration ---

        if (current.ShowInfo != previous.ShowInfo)
            return UpdateStage.Preview;

        if (!string.Equals(current.MatchPattern, previous.MatchPattern, StringComparison.Ordinal))
            return UpdateStage.Preview;

        if (!string.Equals(current.ReplacePattern, previous.ReplacePattern, StringComparison.Ordinal))
            return UpdateStage.Preview;

        if (current.Modifiers.IgnoreCase != previous.Modifiers.IgnoreCase)
            return UpdateStage.Preview;

        if (current.Modifiers.ReplaceEveryMatch != previous.Modifiers.ReplaceEveryMatch)
            return UpdateStage.Preview;

        if (current.Modifiers.IgnorePatternWhitespace != previous.Modifiers.IgnorePatternWhitespace)
            return UpdateStage.Preview;

        if (!string.Equals(current.SortExpression, previous.SortExpression, StringComparison.OrdinalIgnoreCase))
            return UpdateStage.Preview;

        if (!string.Equals(current.Numbering.NumberingStart, previous.Numbering.NumberingStart, StringComparison.Ordinal))
            return UpdateStage.Preview;

        if (!string.Equals(current.Numbering.NumberingIncStep, previous.Numbering.NumberingIncStep, StringComparison.Ordinal))
            return UpdateStage.Preview;

        if (!string.Equals(current.Numbering.NumberingReset, previous.Numbering.NumberingReset, StringComparison.Ordinal))
            return UpdateStage.Preview;

        if (!string.Equals(current.Numbering.NumberingPad, previous.Numbering.NumberingPad, StringComparison.Ordinal))
            return UpdateStage.Preview;

        if (current.Numbering.ValidNumber != previous.Numbering.ValidNumber)
            return UpdateStage.Preview;

        if (current.ChangeCase != previous.ChangeCase)
            return UpdateStage.Preview;

        if (current.Kavita.ShowPreview != previous.Kavita.ShowPreview)
            return UpdateStage.Preview;

        if (!string.Equals(current.Kavita.KavitaRoot, previous.Kavita.KavitaRoot, StringComparison.OrdinalIgnoreCase))
            return UpdateStage.Preview;

        if (current.Kavita.KavitaLibType != previous.Kavita.KavitaLibType)
            return UpdateStage.Preview;

        if (current.Kavita.UseMetadata != previous.Kavita.UseMetadata)
            return UpdateStage.Preview;

        // --- Changes that require only validation ---

        if (current.Output != previous.Output)
            return UpdateStage.Validation;

        if (!string.Equals(current.MoveCopyPath, previous.MoveCopyPath, StringComparison.OrdinalIgnoreCase))
            return UpdateStage.Validation;

        if (current.AllowRenameIntoSubfolders != previous.AllowRenameIntoSubfolders)
            return UpdateStage.Validation;

        if (current.RealtimePreview != previous.RealtimePreview)
            return UpdateStage.Validation;

        return UpdateStage.None;
    }

}
