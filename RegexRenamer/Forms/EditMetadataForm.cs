using Config;
using PInvoke;
using LogEx;
using RegexRenamer.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Formats.Tar;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel.VoiceCommands;
using RegexRenamer.Rename;
using RegexRenamer.Tools.EBookPDFTools;
using RegexRenamer.Utility.RegexMenu;
using Kavita;

namespace RegexRenamer.Forms;

public partial class EditMetadataForm : Form
{
    private List<Tuple<RenameItemInfo, ComicInfo,ComicInfo>> _activeFiles;
    private readonly string _action;
    private readonly string _activePath;
    private readonly string _searchPattern;
    private readonly bool _preserveExtension;

    private RegExContextMenuProvider regExCtxMenu;

    private void InitilaizeForm(string title)
    {
        InitializeComponent();
        this.Text = title;

        bttnClear.Click += async (s, e) => await PerformAction("Clear");
        bttnApply.Click += async (s, e) => await PerformAction("Apply");

        BuildList(cmbMatch, UserConfig.Inst.Meta);
        BuildList(cmbSeries, UserConfig.Inst.Meta);
        BuildList(cmbVolume, UserConfig.Inst.Meta);
        BuildList(cmbTitle, UserConfig.Inst.Meta);
        BuildList(cmbAuthor, UserConfig.Inst.Meta);
        BuildList(cmbLanguage, UserConfig.Inst.Meta);

        // Matching pattern combo box
        cmbMatch.MouseDown += cmbMatch_MouseDown;
        cmbMatch.MouseUp += cmbMatch_MouseUp;
        cmbMatch.LostFocus += (s, e) => UpdatePreview();

        //Auto Numbering
        txtNumberingStart.TextChanged += txtNumberingStart_TextChanged;
        txtNumberingPad.TextChanged += txtNumberingPad_TextChanged;
        txtNumberingInc.TextChanged += txtNumberingInc_TextChanged;
        txtNumberingReset.TextChanged += txtNumberingReset_TextChanged;
        mnuNumbering.MouseDown += mnuNumbering_MouseDown;

        // Replace pattern combo boxes
        cmbAuthor.LostFocus += CmbReplace_LostFocus;
        cmbAuthor.GotFocus += CmbReplace_GotFocus;
        cmbAuthor.MouseDown += cmbReplace_MouseDown;
        cmbAuthor.MouseUp += cmbReplace_MouseUp;

        cmbSeries.LostFocus += CmbReplace_LostFocus;
        cmbSeries.GotFocus += CmbReplace_GotFocus;
        cmbSeries.MouseDown += cmbReplace_MouseDown;
        cmbSeries.MouseUp += cmbReplace_MouseUp;

        cmbVolume.LostFocus += CmbReplace_LostFocus;
        cmbVolume.GotFocus += CmbReplace_GotFocus;
        cmbVolume.MouseDown += cmbReplace_MouseDown;
        cmbVolume.MouseUp += cmbReplace_MouseUp;

        cmbTitle.LostFocus += CmbReplace_LostFocus;
        cmbTitle.GotFocus += CmbReplace_GotFocus;
        cmbTitle.MouseDown += cmbReplace_MouseDown;
        cmbTitle.MouseUp += cmbReplace_MouseUp;

        cmbLanguage.LostFocus += CmbReplace_LostFocus;
        cmbLanguage.GotFocus += CmbReplace_GotFocus;
        cmbLanguage.MouseDown += cmbReplace_MouseDown;
        cmbLanguage.MouseUp += cmbReplace_MouseUp;

        // Changing case
        itmChangeCaseNoChange.Click += (sender, e) => ChangeCaseMenuItem(sender);
        itmChangeCaseUppercase.Click += (sender, e) => ChangeCaseMenuItem(sender);
        itmChangeCaseLowercase.Click += (sender, e) => ChangeCaseMenuItem(sender);
        itmChangeCaseTitlecase.Click += (sender, e) => ChangeCaseMenuItem(sender);
        itmChangeCaseCleanName.Click += (sender, e) => ChangeCaseMenuItem(sender);
        mnuChangeCase.MouseDown += mnuChangeCase_MouseDown;


        cmbViewMode.Items.AddRange(Enum.GetNames(typeof(MetadataViewMode)));
        cmbViewMode.SelectedItem = UserConfig.Inst.Meta.ViewMode;
        cmbViewMode.SelectedIndexChanged += (s, e) =>
        {
            UserConfig.Inst.Meta.ViewMode = cmbViewMode.SelectedItem as string;
            UpdatePreview();
        };

        cmbPDFToolsList.Items.AddRange(Enum.GetNames(typeof(PDFToolsList)));
        cmbPDFToolsList.SelectedItem = UserConfig.Inst.Meta.PreferredPDFTool;

        cmbEPUBToolsList.Items.AddRange(Enum.GetNames(typeof(EBookToolsList)));
        cmbEPUBToolsList.SelectedItem = UserConfig.Inst.Meta.PreferredEBookTool;

        this.FormClosing += Form_Closing;

        regExCtxMenu = new RegExContextMenuProvider(components);

    }

    private void BuildList(ComboBox cmbObj, MetadataFormConfig config)
    {
        cmbObj.Items.Clear();
        switch (cmbObj.Name)
        {
            case nameof(cmbMatch):
                cmbObj.Items.AddRange(config.PredefMatchPatterns.ToArray());
                break;
            case nameof(cmbSeries):
                cmbObj.Items.AddRange(config.PredefSeriesPatterns.ToArray());
                break;
            case nameof(cmbVolume):
                cmbObj.Items.AddRange(config.PredefVolumePatterns.ToArray());
                break;
            case nameof(cmbTitle):
                cmbObj.Items.AddRange(config.PredefTitlePatterns.ToArray());
                break;
            case nameof(cmbAuthor):
                cmbObj.Items.AddRange(config.PredefAuthorPatterns.ToArray());
                break;
            case nameof(cmbLanguage):
                cmbObj.Items.AddRange(config.PredefLanguagePatterns.ToArray());
                break;
        }
        cmbObj.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cmbObj.AutoCompleteSource = AutoCompleteSource.ListItems;
    }

    private void SetDefaultSuggestion(ComboBox cmbObj, MetadataFormConfig config)
    {
        var defaultPattern = string.Empty;
        switch (cmbObj.Name)
        {
            case nameof(cmbMatch):
                defaultPattern = config.DefaultMatchPattern;
                break;
            case nameof(cmbSeries):
                defaultPattern = config.DefaultSeriesPattern;
                break;
            case nameof(cmbVolume):
                defaultPattern = config.DefaultVolumePattern;
                break;
            case nameof(cmbTitle):
                defaultPattern = config.DefaultTitlePattern;
                break;
            case nameof(cmbAuthor):
                defaultPattern = config.DefaultAuthorPattern;
                break;
            case nameof(cmbLanguage):
                defaultPattern = config.DefaultLanguagePattern;
                break;
        }
        if (string.IsNullOrEmpty(cmbObj.Text) && !string.IsNullOrWhiteSpace(defaultPattern))
        {
            cmbObj.Text = defaultPattern;
            cmbObj.SelectAll();
        }
    }

    public EditMetadataForm(string activePath, string searchPattern, string title, string action, bool preservExt)
    {
        this._activeFiles = new List<Tuple<RenameItemInfo, ComicInfo, ComicInfo>>();
        this._action = action;
        this._activePath = activePath;
        this._searchPattern = searchPattern;
        this._preserveExtension = preservExt;
        InitilaizeForm(title);
        chkApplyRecursively.Checked = UserConfig.Inst.Meta.UpdateRecursively;
        chkApplyRecursively.CheckStateChanged += (sender, e) => UpdateFileList();
    }
    public EditMetadataForm(List<FileInfo> files, string title, string action, bool preservExt)
    {
        this._activeFiles = files.Select(t => new Tuple<RenameItemInfo, ComicInfo, ComicInfo>(new RenameItemInfo(t, false, preservExt), new ComicInfo(), t.GetMetadata() ?? new ComicInfo())).ToList();
        this._action = action;
        this._activePath = string.Empty;
        this._searchPattern = string.Empty;
        this._preserveExtension = preservExt;
        InitilaizeForm(title);
        chkApplyRecursively.Enabled = false;
    }

    private void Form_Closing(object sender, FormClosingEventArgs e)
    {
        if (!EnableUpdates && e.CloseReason == CloseReason.UserClosing)
        {
            MessageBox.Show("Operation in progress. Please wait until it completes.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            e.Cancel = true;
        }
    }

    // prevent setting to true if rename operation in progress
    private bool _enableUpdates = true;

    private bool EnableUpdates
    {
        get
        {
            return _enableUpdates;
        }
        set
        {
            _enableUpdates = value;
            if (_enableUpdates)
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.Cursor = Cursors.AppStarting;
            }
        }
    }

    private void EnableFormControls(bool enable)
    {
        cmbAuthor.Enabled = enable;
        cmbMatch.Enabled = enable;
        cmbSeries.Enabled = enable;
        cmbTitle.Enabled = enable;
        cmbVolume.Enabled = enable;
        cmbLanguage.Enabled = enable;
        mnuChangeCase.Enabled = enable;
        mnuNumbering.Enabled = enable;
        chkApplyRecursively.Enabled = enable;
        
        cmbEPUBToolsList.Enabled = enable;
        cmbPDFToolsList.Enabled = enable;

        chkIgnoreError.Enabled = enable;
        cmbViewMode.Enabled = enable;
        cbModifierG.Enabled = enable;
        cbModifierI.Enabled = enable;
        cbModifierX.Enabled = enable;
    }

    private void MetaDataForm_Load(object sender, EventArgs e)
    {
        // Implementation to set files and update UI accordingly
        cmbMatch.SetCueBanner("Shift+RightClick for a menu of regex elements");
        var bannerTxt = "Use $1, $2, ... to insert captured text, $` text before match, " +
            "$' text after match, $_ original filename";
        cmbSeries.SetCueBanner(bannerTxt);
        cmbTitle.SetCueBanner(bannerTxt);
        cmbAuthor.SetCueBanner(bannerTxt);
        cmbVolume.SetCueBanner(bannerTxt);
        cmbLanguage.SetCueBanner("Set language code, ta->tamil en->english etc");

        UpdateFileList();

        chkIgnoreError.Checked = UserConfig.Inst.Meta.IgnoreErrors;
        chkApplyRecursively.Checked = UserConfig.Inst.Meta.UpdateRecursively;
    }

    private async void MetaDataForm_Shown(object sender, EventArgs e)
    {
        await PerformAction(_action);
    }


    private async Task PerformAction(string action)
    {
        switch (action)
        {
            case "Ok":
                // Apply changes
                // (Implementation of applying changes goes here)
                this.DialogResult = DialogResult.OK;
                this.Close();
                break;
            case "Cancel":
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                break;
            case "Clear":
                EnableUpdates = false;
                EnableFormControls(false);
                try
                {
                    // Apply changes without closing
                    // (Implementation of applying changes goes here)
                    UpdatePreview();

                    if (MessageBox.Show("Do you want to Clear the metadata for all the files now?", "Clear Metadata", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;
                    }
                    pbMetadataWrite.Value = 0;
                    pbMetadataWrite.Maximum = _activeFiles.Count;
                    for (int index = 0; index < _activeFiles.Count; index++)
                    {
                        var file = _activeFiles[index];
                        pbMetadataWrite.Value = index + 1;
                        lblStatus.Text = $"Writing metadata for {file.Item1.Name} ({index + 1} of {_activeFiles.Count})";
                        fileListView.SelectedItems.Clear();
                        fileListView.Items[index].Selected = true;
                        UserConfig.Inst.Meta.UpdateRecursively = chkIgnoreError.Checked;
                        UserConfig.Inst.Meta.PreferredEBookTool = cmbEPUBToolsList.SelectedItem as string;
                        UserConfig.Inst.Meta.PreferredPDFTool = cmbPDFToolsList.SelectedItem as string;
                        UserConfig.Inst.Meta.UpdateRecursively = chkApplyRecursively.Checked;

                        var fileExt = file.Item1.Extension.ToLowerInvariant();
                        var toolName = fileExt == ".pdf" ? UserConfig.Inst.Meta.PreferredPDFTool : UserConfig.Inst.Meta.PreferredEBookTool;

                        if (fileExt != ".pdf" && fileExt != ".epub" && fileExt != ".kepub" && fileExt != ".azw")
                        {
                            // unsupported file type
                            continue;
                        }

                        Application.DoEvents();
                        try
                        {
                            //await Task.Delay(500); // to allow UI to update
                            await EBookHelper.ClearMetadata(file.Item1.Fullpath, toolName);
                        }
                        catch (Exception ex)
                        {
                            if (!UserConfig.Inst.Meta.IgnoreErrors)
                                ErrorLog.Inst.ShowError($"Failed to write metadata for {file.Item1.Name}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.Inst.ShowError($"An error occurred while applying changes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    EnableUpdates = true;
                    EnableFormControls(true);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
                break;
            case "Apply":
                EnableUpdates = false;
                EnableFormControls(false);
                try
                {
                    // Apply changes without closing
                    // (Implementation of applying changes goes here)
                    UpdatePreview();

                    if (MessageBox.Show("Do you want to apply the changes now?", "Apply Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        break;
                    }

                    pbMetadataWrite.Value = 0;
                    pbMetadataWrite.Maximum = _activeFiles.Count;

                    for (int index = 0; index < _activeFiles.Count; index++)
                    {
                        var file = _activeFiles[index];
                        pbMetadataWrite.Value = index + 1;
                        lblStatus.Text = $"Writing metadata for {file.Item1.Name} ({index + 1} of {_activeFiles.Count})";
                        fileListView.SelectedItems.Clear();
                        fileListView.Items[index].Selected = true;
                        UserConfig.Inst.Meta.IgnoreErrors = chkIgnoreError.Checked;
                        UserConfig.Inst.Meta.PreferredEBookTool = cmbEPUBToolsList.SelectedItem as string;
                        UserConfig.Inst.Meta.PreferredPDFTool = cmbPDFToolsList.SelectedItem as string;
                        UserConfig.Inst.Meta.UpdateRecursively = chkApplyRecursively.Checked;

                        var fileExt = file.Item1.Extension.ToLowerInvariant();
                        var toolName = fileExt == ".pdf" ? UserConfig.Inst.Meta.PreferredPDFTool : UserConfig.Inst.Meta.PreferredEBookTool;

                        if (fileExt != ".pdf" && fileExt != ".epub" && fileExt != ".kepub" && fileExt != ".azw")
                        {
                            // unsupported file type
                            continue;
                        }

                        Application.DoEvents();
                        try
                        {
                            //await Task.Delay(500); // to allow UI to update
                            await EBookHelper.WriteMetadata(file.Item1.Fullpath, file.Item2, toolName);
                        }
                        catch (Exception ex)
                        {
                            if (!UserConfig.Inst.Meta.IgnoreErrors)
                                ErrorLog.Inst.ShowError($"Failed to write metadata for {file.Item1.Name}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.Inst.ShowError($"An error occurred while applying changes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    EnableUpdates = true;
                    EnableFormControls(true);
                }

                if (MessageBox.Show("Changes applied. Do you want to close the dialog?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                break;
            default:
                break;
        }
    }

    private async void btnOK_Click(object sender, EventArgs e)
    {
        var tag = (string)((Button)sender).Tag;
        await PerformAction(tag);
    }
    

    #region Match pattern ComboBox methods

    private void cmbMatch_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
        {
            regExCtxMenu.ShowMatchMenu(cmbMatch, e.Location);
        }
    }
    private void cmbMatch_MouseUp(object sender, MouseEventArgs e)
    {
        if (cmbMatch.ContextMenuStrip != null)
            cmbMatch.ContextMenuStrip = null;  // restore default cms
    }

    #endregion

    #region Replace pattern combo boxes

    private void CmbReplace_GotFocus(object sender, EventArgs e)
    {
        ComboBox cmbTemp = (ComboBox)sender;

        if (string.IsNullOrWhiteSpace(cmbMatch.Text))
        {
            cmbMatch.Text = UserConfig.Inst.Meta.DefaultMatchPattern;
        }

        if (string.IsNullOrWhiteSpace(cmbTemp.Text))
        {
            SetDefaultSuggestion(cmbTemp, UserConfig.Inst.Meta);
        }
    }

    private void CmbReplace_LostFocus(object sender, EventArgs e)
    {
        ComboBox cmbTemp = (ComboBox)sender;
        if (!string.IsNullOrWhiteSpace(cmbTemp.Text) && string.IsNullOrWhiteSpace(cmbMatch.Text))
        {
            cmbMatch.Text = UserConfig.Inst.Meta.DefaultMatchPattern;
        }
        UpdatePreview();
    }

    private void cmbReplace_MouseDown(object sender, MouseEventArgs e)
    {
        ComboBox cmbReplace = (ComboBox)sender;
        if (e.Button == MouseButtons.Right && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
        {
            regExCtxMenu.ShowReplaceMenu(cmbReplace, e.Location);
        }
    }
    private void cmbReplace_MouseUp(object sender, MouseEventArgs e)
    {
        ComboBox cmbReplace = (ComboBox)sender;
        if (cmbReplace.ContextMenuStrip != null)
            cmbReplace.ContextMenuStrip = null;  // restore default cms
    }

    #endregion

    #region Change Case Methods

    //change case
    private void ChangeCaseMenuItem(object sender)
    {
        if (!EnableUpdates) return;

        ToolStripMenuItem checkedMenuItem = (ToolStripMenuItem)sender;
        if (checkedMenuItem.Checked) return;  // already checked


        // update checked marks
        for (int i = 0; i < mnuChangeCase.DropDownItems.Count; i++)
        {
            if (i == 1) continue;  // seperator

            if (mnuChangeCase.DropDownItems[i] == checkedMenuItem)
                ((ToolStripMenuItem)mnuChangeCase.DropDownItems[i]).Checked = true;
            else
                ((ToolStripMenuItem)mnuChangeCase.DropDownItems[i]).Checked = false;
        }


        // set default match/replace values (if empty)
        if (checkedMenuItem != itmChangeCaseNoChange)
        {
            if (cmbMatch.Text == "")
            {
                cmbMatch.Text = "(.*)";
            }
        }

        // set button text to bold if an option selected
        if (itmChangeCaseNoChange.Checked)
        {
            mnuChangeCase.Font = new Font("Tahoma", 8.25F);
            mnuChangeCase.Padding = new Padding(0, 0, 8, 0);
        }
        else
        {
            mnuChangeCase.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            mnuChangeCase.Padding = new Padding(0, 0, 0, 0);
        }

        // update preview
        this.Update();
        UpdatePreview();
    }

    private void mnuChangeCase_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right && !itmChangeCaseNoChange.Checked)  // set default
            itmChangeCaseNoChange.PerformClick();
    }

    private ChangeCaseOption GetChangeCaseInfo()
    {
        if (itmChangeCaseNoChange.Checked) return ChangeCaseOption.NoChange;
        else if (itmChangeCaseUppercase.Checked) return ChangeCaseOption.Uppercase;
        else if (itmChangeCaseLowercase.Checked) return ChangeCaseOption.Lowercase;
        else if (itmChangeCaseTitlecase.Checked) return ChangeCaseOption.Titlecase;
        else if (itmChangeCaseCleanName.Checked) return ChangeCaseOption.CleanName;
        else return ChangeCaseOption.NoChange;
    }
    #endregion

    #region Autonumbering Methods

    private bool _validNumber = true;      // numbering menu options are all valid
                                           // numbering
    private void NumberingMenuItem(object sender)
    {
        if (!EnableUpdates) return;

        ToolStripTextBox textBox = (ToolStripTextBox)sender;
        bool error = false;
        int num;

        if (textBox == txtNumberingPad && textBox.Text == "")
        {
            textBox.Text = "0";  // default: no padding
        }

        // parse int, check valid range
        if (!Int32.TryParse(textBox.Text, out num))
            error = true;
        else if (textBox == txtNumberingStart && num < 0)
            error = true;
        else if (textBox == txtNumberingPad && num != 0)
            error = true;
        else if (textBox == txtNumberingInc && num == 0)
            error = true;
        else if (textBox == txtNumberingReset && num < 0)
            error = true;


        // or, check for letter(s)
        if (textBox == txtNumberingStart)
        {
            if (Regex.IsMatch(textBox.Text, @"^([a-z]+|[A-Z]+)$"))
            {
                error = false;
                this.txtNumberingPad.Enabled = false;
            }
            else if (!this.txtNumberingPad.Enabled)
            {
                this.txtNumberingPad.Enabled = true;
            }
        }

        // set bg colour
        if (error)
            textBox.BackColor = Color.MistyRose;
        else
            textBox.BackColor = SystemColors.Window;


        // if all valid, update preview
        textBox.Tag = !error;
        _validNumber = (bool)mnuNumbering.DropDownItems[0].Tag
                    && (bool)mnuNumbering.DropDownItems[1].Tag
                    && (bool)mnuNumbering.DropDownItems[2].Tag
                    && (bool)mnuNumbering.DropDownItems[3].Tag;

        if (_validNumber)
            UpdatePreview();
    }
    private void txtNumberingStart_TextChanged(object sender, EventArgs e)
    {
        NumberingMenuItem(sender);
    }
    private void txtNumberingPad_TextChanged(object sender, EventArgs e)
    {
        NumberingMenuItem(sender);
    }
    private void txtNumberingInc_TextChanged(object sender, EventArgs e)
    {
        NumberingMenuItem(sender);
    }
    private void txtNumberingReset_TextChanged(object sender, EventArgs e)
    {
        NumberingMenuItem(sender);
    }

    private void mnuNumbering_MouseDown(object sender, MouseEventArgs e)
    {
        if(!EnableUpdates) return;

        if (e.Button == MouseButtons.Right)  // set defaults
        {
            if (txtNumberingStart.Text != "1") txtNumberingStart.Text = "1";
            if (txtNumberingPad.Text != "000") txtNumberingPad.Text = "000";
            if (txtNumberingInc.Text != "1") txtNumberingInc.Text = "1";
            if (txtNumberingReset.Text != "0") txtNumberingReset.Text = "0";

            UpdatePreview();
        }
    }
    #endregion

    #region Update Methods

    private void UpdatePreview()
    {
        if (!EnableUpdates) return;

        EnableUpdates = false;

        AutoNumberingInfo numInfo = new AutoNumberingInfo()
        {
            ValidNumber = _validNumber,
            NumberingStart = txtNumberingStart.Text,
            NumberingIncStep = txtNumberingInc.Text,
            NumberingReset = txtNumberingReset.Text,
            NumberingPad = txtNumberingPad.Text
        };

        ChangeCaseOption changeCaseOption = GetChangeCaseInfo();

        RegexModifierInfo modifierInfo = new RegexModifierInfo()
        {
            IgnoreCase = cbModifierI.Checked,
            ReplaceEveryMatch = cbModifierG.Checked,
            IgnorePatternWhitespace = cbModifierX.Checked
        };


        _activeFiles.BuildMetadataPreview(MetadataType.Series, cmbMatch.Text, cmbSeries.Text, numInfo, changeCaseOption, modifierInfo);
        _activeFiles.BuildMetadataPreview(MetadataType.Volume, cmbMatch.Text, cmbVolume.Text, numInfo, changeCaseOption, modifierInfo);
        _activeFiles.BuildMetadataPreview(MetadataType.Title, cmbMatch.Text, cmbTitle.Text, numInfo, changeCaseOption, modifierInfo);
        _activeFiles.BuildMetadataPreview(MetadataType.Writer, cmbMatch.Text, cmbAuthor.Text, numInfo, changeCaseOption, modifierInfo);
        _activeFiles.BuildMetadataPreview(MetadataType.Language, cmbMatch.Text, cmbLanguage.Text, numInfo, changeCaseOption, modifierInfo);
        UpdateDataToGrid();

        EnableUpdates = true;
    }


    private void UpdateDataToGrid()
    {
        Enum.TryParse<MetadataViewMode>(UserConfig.Inst.Meta.ViewMode,out MetadataViewMode viewMode);
        // update file list
        fileListView.BeginUpdate();
        for (int dfi = 0; dfi < fileListView.Items.Count; dfi++)
        {
            int afi = (int)fileListView.Items[dfi].Tag;
            
            var metaData = viewMode  == MetadataViewMode.ShowExitingMetadata ? _activeFiles[afi].Item3 : _activeFiles[afi].Item2;

            fileListView.Items[dfi].SubItems[1].Text = metaData.Title;
            fileListView.Items[dfi].SubItems[2].Text = metaData.Series;
            fileListView.Items[dfi].SubItems[3].Text = metaData.Volume;
            fileListView.Items[dfi].SubItems[4].Text = metaData.Writer;
            fileListView.Items[dfi].SubItems[5].Text = metaData.LanguageISO;
        }

        fileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        fileListView.EndUpdate();
    }

    private void UpdateFileList()
    {
        if (chkApplyRecursively.Enabled)
        {
            _activeFiles.Clear();
            _activeFiles = _activePath.GetFileInfo(_preserveExtension, _searchPattern, chkApplyRecursively.Checked);
        }

        // create datagridview items w/ filename
        fileListView.Items.Clear();
        for (int idx = 0; idx < _activeFiles.Count; idx++)
        {
            // add new item
            var row = fileListView.Items.Add(_activeFiles[idx].Item1.Name);
            row.SubItems.Add("               ");
            row.SubItems.Add("               ");
            row.SubItems.Add("               ");
            row.SubItems.Add("               ");
            row.SubItems.Add("               ");
            fileListView.Items[idx].Tag = idx;  // store activeFiles index so we can refer back when under different sorting
        }
        UpdatePreview();
    }
    #endregion
}
