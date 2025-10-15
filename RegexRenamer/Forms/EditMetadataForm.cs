using Config;
using PInvoke;
using RegexRenamer.Kavita;
using RegexRenamer.Native;
using RegexRenamer.Utility;
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

namespace RegexRenamer.Forms;

public partial class EditMetadataForm : Form
{
    private List<Tuple<FileInfo, ComicInfo>> _activeFiles;
    private readonly string _action;
    private readonly string _activePath;
    private readonly string _searchPattern;

    private void InitilaizeForm(string title)
    {
        InitializeComponent();
        this.Text = title;

        bttnClear.Click += async (s, e) => await PerformAction("Clear");
        bttnApply.Click += async (s, e) => await PerformAction("Apply");

        cmbMatch.MouseDown += cmbMatch_MouseDown;
        cmbMatch.MouseUp += cmbMatch_MouseUp;

        //Numbering
        txtNumberingStart.TextChanged += txtNumberingStart_TextChanged;
        txtNumberingPad.TextChanged += txtNumberingPad_TextChanged;
        txtNumberingInc.TextChanged += txtNumberingInc_TextChanged;
        txtNumberingReset.TextChanged += txtNumberingReset_TextChanged;
        mnuNumbering.MouseDown += mnuNumbering_MouseDown;

        cmbAuthor.LostFocus += CmbReplace_LostFocus;
        cmbSeries.LostFocus += CmbReplace_LostFocus;
        cmbTitle.LostFocus += CmbReplace_LostFocus;
        cmbVolume.LostFocus += CmbReplace_LostFocus;

        this.FormClosing += Form_Closing;
    }

    public EditMetadataForm(string activePath, string searchPattern, string title, string action)
    {
        this._activeFiles = new List<Tuple<FileInfo, ComicInfo>>();
        this._action = action;
        this._activePath = activePath;
        this._searchPattern = searchPattern;
        InitilaizeForm(title);
        chkApplyRecursively.Checked = UserConfig.Inst.ApplyRecursively;
        chkApplyRecursively.CheckStateChanged += (sender, e) => UpdateFileList();
    }
    public EditMetadataForm(List<FileInfo> files, string title, string action)
    {
        this._activeFiles = files.Select(t => new Tuple<FileInfo, ComicInfo>(t, new ComicInfo())).ToList();
        this._action = action;
        this._activePath = string.Empty;
        this._searchPattern = string.Empty;
        InitilaizeForm(title);
        chkApplyRecursively.Enabled = false;
    }

    private void Form_Closing(object sender, FormClosingEventArgs e)
    {
        if (_preventCancel && e.CloseReason == CloseReason.UserClosing)
        {
            MessageBox.Show("Operation in progress. Please wait until it completes.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            e.Cancel = true;
        }
    }

    private void CmbReplace_LostFocus(object sender, EventArgs e)
    {
        ComboBox cmbTemp = (ComboBox)sender;
        if (!string.IsNullOrWhiteSpace(cmbTemp.Text) && string.IsNullOrWhiteSpace(cmbMatch.Text))
        {
            cmbMatch.Text = "(.+)";
        }
        UpdatePreview();
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

        chkIgnoreError.Checked = UserConfig.Inst.IgnoreError;
        chkClearUseAltMethod.Checked = UserConfig.Inst.MetadatWriteUseAltMethodForPDF;
        chkApplyRecursively.Checked = UserConfig.Inst.ApplyRecursively;
    }

    private async void MetaDataForm_Shown(object sender, EventArgs e)
    {
        await PerformAction(_action);
    }

    private bool _preventCancel = false;

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
                _preventCancel = true;
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
                        UserConfig.Inst.IgnoreError = chkIgnoreError.Checked;
                        UserConfig.Inst.MetadatWriteUseAltMethodForPDF = chkClearUseAltMethod.Checked;
                        UserConfig.Inst.ApplyRecursively = chkApplyRecursively.Checked;
                        Application.DoEvents();
                        try
                        {
                            await EBookHelper.ClearMetadata(file.Item1.FullName, UserConfig.Inst.MetadatWriteUseAltMethodForPDF);
                        }
                        catch (Exception ex)
                        {
                            if(!UserConfig.Inst.IgnoreError)
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
                    _preventCancel = false;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
                break;
            case "Apply":
                _preventCancel = true;
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
                        UserConfig.Inst.IgnoreError = chkIgnoreError.Checked;
                        UserConfig.Inst.MetadatWriteUseAltMethodForPDF = chkClearUseAltMethod.Checked;
                        UserConfig.Inst.ApplyRecursively = chkApplyRecursively.Checked;
                        Application.DoEvents();
                        try
                        {
                            await EBookHelper.WriteMetadata(file.Item1.FullName, file.Item2);
                        }
                        catch (Exception ex)
                        {
                            if (!UserConfig.Inst.IgnoreError)
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
                    _preventCancel = false;
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

    #region DataGridView Events



    #endregion

    #region ComboBox Events

    private void cmbMatch_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
        {
            lastControlRightClicked = cmbMatch;
            cmbMatch.ContextMenuStrip = cmsBlank;  // prevent default cms from being displayed
            if (!cmbMatch.Focused)  // prevent combobox from selecting all if already focused
                cmbMatch.Focus();
            cmRegexMatch.Show(cmbMatch, e.Location);
        }
    }
    private void cmbMatch_MouseUp(object sender, MouseEventArgs e)
    {
        if (cmbMatch.ContextMenuStrip != null)
            cmbMatch.ContextMenuStrip = null;  // restore default cms
    }

    #endregion

    #region Autonumbering Methods

    private Control lastControlRightClicked;
    private void InsertRegexFragment(object sender, EventArgs e)
    {
        InsertArgs ia = (InsertArgs)((ToolStripMenuItem)sender).Tag;
        TextBox textBox = null;
        ComboBox comboBox = null;

        int selectionStart, selectionLength;
        string text;

        if (lastControlRightClicked.GetType().Name == "TextBox")
        {
            textBox = (TextBox)lastControlRightClicked;
            selectionStart = textBox.SelectionStart;
            selectionLength = textBox.SelectionLength;
            text = textBox.Text;
        }
        else
        {
            comboBox = (ComboBox)lastControlRightClicked;
            selectionStart = comboBox.SelectionStart;
            selectionLength = comboBox.SelectionLength;
            text = comboBox.Text;
        }

        if (ia.InsertBefore == "" && selectionLength == 0)
        {
            ia.InsertBefore = ia.InsertAfter;
            ia.InsertAfter = "";
        }

        if (ia.WrapIfSelection && selectionLength > 0)
            if (ia.InsertAfter == "")
                ia.InsertAfter = ia.InsertBefore;
            else
                ia.InsertBefore = ia.InsertAfter;

        int group = 0;
        if (ia.GroupSelection && selectionLength > 0)
        {
            text = text.Insert(selectionStart, "(");
            selectionStart += 1;
            text = text.Insert(selectionStart + selectionLength, ")");
            group = 1;
        }

        if (selectionLength > 0 && (ia.InsertBefore == "" || ia.InsertAfter == "") && !ia.GroupSelection)
        {
            text = text.Remove(selectionStart, selectionLength);
            selectionLength = 0;
        }

        if (ia.InsertBefore != "")
        {
            text = text.Insert(selectionStart - group, ia.InsertBefore);
            selectionStart += ia.InsertBefore.Length;
        }
        if (ia.InsertAfter != "")
        {
            text = text.Insert(selectionStart + selectionLength + group, ia.InsertAfter);
        }
        if (ia.SelectionStartOffset > 0)
        {
            selectionStart = selectionStart - group - ia.InsertBefore.Length + ia.SelectionStartOffset;
        }
        if (ia.SelectionStartOffset < 0)
        {
            selectionStart = selectionStart + selectionLength + group + ia.InsertAfter.Length + ia.SelectionStartOffset;
        }
        if (ia.SelectionLength != -1)
            selectionLength = ia.SelectionLength;

        if (textBox != null)
        {
            textBox.SelectAll(); textBox.Paste(text);  // allow undo
            textBox.SelectionStart = selectionStart;
            textBox.SelectionLength = selectionLength;
        }
        else
        {
            comboBox.SelectAll(); comboBox.SelectedText = text;  // allow undo
            comboBox.SelectionStart = selectionStart;
            comboBox.SelectionLength = selectionLength;
        }
    }

    private bool validNumber = true;      // numbering menu options are all valid
                                          // numbering
    private void NumberingMenuItem(object sender)
    {
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
        validNumber = (bool)mnuNumbering.DropDownItems[0].Tag
                    && (bool)mnuNumbering.DropDownItems[1].Tag
                    && (bool)mnuNumbering.DropDownItems[2].Tag
                    && (bool)mnuNumbering.DropDownItems[3].Tag;

        if (validNumber)
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

    private static string SequenceNumberToLetter(int i)
    {
        int dividend = i;
        string columnName = String.Empty;

        while (dividend > 0)
        {
            int modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(97 + modulo) + columnName;  // note: A-Z = 65-90, a-z = 97-122
            dividend = (dividend - modulo) / 26;
        }

        return columnName;
    }
    private static int SequenceLetterToNumber(string letter)
    {
        int number = 0;
        int pow = 1;
        for (int i = letter.Length - 1; i >= 0; i--)
        {
            number += (letter[i] - 'a' + 1) * pow;
            pow *= 26;
        }

        return number;
    }
    private void mnuNumbering_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)  // set defaults
        {
            if (txtNumberingStart.Text != "1") txtNumberingStart.Text = "1";
            if (txtNumberingPad.Text != "000") txtNumberingPad.Text = "000";
            if (txtNumberingInc.Text != "1") txtNumberingInc.Text = "1";
            if (txtNumberingReset.Text != "0") txtNumberingReset.Text = "0";

            UpdatePreview();
        }
    }

    private string MatchEvalChangeCase(Match match)
    {
        TextInfo ti = new CultureInfo("en").TextInfo;

        if (itmChangeCaseUppercase.Checked) return ti.ToUpper(match.Groups[1].Value);
        else if (itmChangeCaseLowercase.Checked) return ti.ToLower(match.Groups[1].Value);
        else if (itmChangeCaseTitlecase.Checked) return ti.ToTitleCase(match.Groups[1].Value.ToLower());
        else if (itmChangeCaseCleanName.Checked) return match.Groups[1].Value.ToCleanFileName();
        else return match.Groups[1].Value;
    }

    #endregion

    #region Update Methods

    private void UpdatePreview()
    {
        UpdatePreviewInternal(cmbSeries, 1);
        UpdatePreviewInternal(cmbVolume, 4);
        UpdatePreviewInternal(cmbTitle, 2);
        UpdatePreviewInternal(cmbAuthor, 3);
        UpdatePreviewInternal(cmbLanguage, 5);
        UpdateDataToGrid();
    }
    private int UpdatePreviewInternal(ComboBox cmbReplace, int colType)
    {
        int retVal = 0;
        this.Cursor = Cursors.AppStarting;

        const string rxDoller = @"(?<=(?:^|[^$])(?:\$\$)*)\$";  // regex for an actual (non-escaped) doller sign

        // generate preview
        if (!string.IsNullOrWhiteSpace(cmbMatch.Text) && !string.IsNullOrWhiteSpace(cmbReplace.Text))
        {
            // compile regex
            RegexOptions options = RegexOptions.None | RegexOptions.Compiled;
            //Global Modifier
            int count = -1;
            if (cbModifierI.Checked) { options |= RegexOptions.IgnoreCase; }
            if (cbModifierX.Checked) { options |= RegexOptions.IgnorePatternWhitespace; }
            //main regex
            Regex regex = new Regex(cmbMatch.Text, options);

            // auto numbering
            int numCurrent = 0, numInc = 0, numStart = 0, numReset = 0;
            string numFormatted = "";
            bool doingAutoNum = false;
            bool doingAutoNumLetter = false;  // number sequence is actually a-z letter sequence
            bool doingAutoNumLetterUpper = false;  // letter sequence is uppercase

            if (this.validNumber && Regex.IsMatch(cmbReplace.Text, rxDoller + "#"))
                doingAutoNum = true;

            Match match = Regex.Match(this.txtNumberingStart.Text, @"^(([a-z]+)|([A-Z]+))$");
            doingAutoNumLetter = match.Success;
            doingAutoNumLetterUpper = match.Success && match.Groups[3].Length > 0;

            if (doingAutoNum)
            {
                if (doingAutoNumLetter)
                {
                    numStart = SequenceLetterToNumber(txtNumberingStart.Text.ToLower());
                }
                else
                {
                    numStart = Int32.Parse(txtNumberingStart.Text);
                }

                numInc = Int32.Parse(txtNumberingInc.Text);
                numReset = Int32.Parse(txtNumberingReset.Text);
            }
            numCurrent = numStart - numInc;  // back up one

            // regex each filename
            string userReplacePattern = cmbReplace.Text;
            if (doingAutoNum)
            {
                userReplacePattern = Regex.Replace(userReplacePattern, rxDoller + @"(\d+)" + rxDoller + "#", "$${$1}$$#");
            }

            for (int afi = 0; afi < _activeFiles.Count; afi++)
            {
                string replacePattern;

                if (doingAutoNum)
                {
                    numCurrent += numInc;

                    if (numReset != 0 && (numCurrent - numStart) % numReset == 0)
                        numCurrent = numStart;

                    if (numFormatted != "$#")  // basic int overflow & negative number detection
                    {
                        if (!doingAutoNumLetter)  // number sequence
                        {
                            if (numCurrent < 0)
                                numFormatted = "$#";
                            else
                                numFormatted = numCurrent.ToString(txtNumberingPad.Text);
                        }
                        else  // letter sequence
                        {
                            if (numCurrent < 1)
                                numFormatted = "$#";
                            else if (doingAutoNumLetterUpper)
                                numFormatted = SequenceNumberToLetter(numCurrent).ToUpper();
                            else
                                numFormatted = SequenceNumberToLetter(numCurrent);
                        }
                    }

                    replacePattern = Regex.Replace(userReplacePattern, rxDoller + "#", numFormatted);
                }
                else
                {
                    replacePattern = userReplacePattern;
                }

                if (!itmChangeCaseNoChange.Checked)
                    replacePattern = "\n" + replacePattern + "\n";  // delimit change-case boundaries

                switch (colType)
                {
                    case 1:
                        _activeFiles[afi].Item2.Series = regex.Replace(_activeFiles[afi].Item1.Name, replacePattern, count);
                        if (!itmChangeCaseNoChange.Checked)
                            _activeFiles[afi].Item2.Series = Regex.Replace(_activeFiles[afi].Item2.Series, @"\n([^\n]*)\n", new MatchEvaluator(MatchEvalChangeCase));
                        if (_activeFiles[afi].Item2.Series.Length == 0)
                            _activeFiles[afi].Item2.Series = _activeFiles[afi].Item1.Name;
                        retVal = string.IsNullOrWhiteSpace(_activeFiles[afi].Item2.Series) ? 0 : 1;
                        break;
                    case 2:
                        _activeFiles[afi].Item2.Title = regex.Replace(_activeFiles[afi].Item1.Name, replacePattern, count);
                        if (!itmChangeCaseNoChange.Checked)
                            _activeFiles[afi].Item2.Title = Regex.Replace(_activeFiles[afi].Item2.Title, @"\n([^\n]*)\n", new MatchEvaluator(MatchEvalChangeCase));
                        if (_activeFiles[afi].Item2.Title.Length == 0)
                            _activeFiles[afi].Item2.Title = _activeFiles[afi].Item1.Name;
                        retVal = string.IsNullOrWhiteSpace(_activeFiles[afi].Item2.Title) ? 0 : 1;
                        break;
                    case 3:
                        _activeFiles[afi].Item2.Writer = regex.Replace(_activeFiles[afi].Item1.Name, replacePattern, count);
                        if (!itmChangeCaseNoChange.Checked)
                            _activeFiles[afi].Item2.Writer = Regex.Replace(_activeFiles[afi].Item2.Writer, @"\n([^\n]*)\n", new MatchEvaluator(MatchEvalChangeCase));
                        if (_activeFiles[afi].Item2.Writer.Length == 0)
                            _activeFiles[afi].Item2.Writer = _activeFiles[afi].Item1.Name;
                        retVal = string.IsNullOrWhiteSpace(_activeFiles[afi].Item2.Writer) ? 0 : 1;
                        break;
                    case 4:
                        _activeFiles[afi].Item2.Volume = regex.Replace(_activeFiles[afi].Item1.Name, replacePattern, count);
                        if (!itmChangeCaseNoChange.Checked)
                            _activeFiles[afi].Item2.Volume = Regex.Replace(_activeFiles[afi].Item2.Volume, @"\n([^\n]*)\n", new MatchEvaluator(MatchEvalChangeCase));
                        if (_activeFiles[afi].Item2.Volume.Length == 0)
                            _activeFiles[afi].Item2.Volume = _activeFiles[afi].Item1.Name;
                        retVal = string.IsNullOrWhiteSpace(_activeFiles[afi].Item2.Volume) ? 0 : 1;
                        break;
                    case 5:
                        _activeFiles[afi].Item2.LanguageISO = regex.Replace(_activeFiles[afi].Item1.Name, replacePattern, count);
                        if (!itmChangeCaseNoChange.Checked)
                            _activeFiles[afi].Item2.LanguageISO = Regex.Replace(_activeFiles[afi].Item2.LanguageISO, @"\n([^\n]*)\n", new MatchEvaluator(MatchEvalChangeCase));
                        if (_activeFiles[afi].Item2.LanguageISO.Length == 0)
                            _activeFiles[afi].Item2.LanguageISO = _activeFiles[afi].Item1.Name;
                        retVal = string.IsNullOrWhiteSpace(_activeFiles[afi].Item2.LanguageISO) ? 0 : 1;
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            retVal = 1;
            // clear preview
            for (int afi = 0; afi < _activeFiles.Count; afi++)
            {
                switch (colType)
                {
                    case 1:
                        _activeFiles[afi].Item2.Series = "";
                        break;
                    case 2:
                        _activeFiles[afi].Item2.Title  = "";
                        break;
                    case 3:
                        _activeFiles[afi].Item2.Writer = "";
                        break;
                    case 4:
                        _activeFiles[afi].Item2.Volume = "";
                        break;
                     case 5:
                        _activeFiles[afi].Item2.LanguageISO = "";
                        break;
                    default:
                        break;
                }
            }
        }

        return retVal;
    }

    private void UpdateDataToGrid()
    {
        var colStyle = new ColumnHeaderAutoResizeStyle[]
        {
            ColumnHeaderAutoResizeStyle.ColumnContent,
            ColumnHeaderAutoResizeStyle.HeaderSize,
            ColumnHeaderAutoResizeStyle.HeaderSize,
            ColumnHeaderAutoResizeStyle.HeaderSize,
            ColumnHeaderAutoResizeStyle.HeaderSize,
            ColumnHeaderAutoResizeStyle.HeaderSize
        };

        // update file list
        fileListView.BeginUpdate();
        //fileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        for (int dfi = 0; dfi < fileListView.Items.Count; dfi++)
        {
            int afi = (int)fileListView.Items[dfi].Tag;
            if(!string.IsNullOrEmpty(_activeFiles[afi].Item2.Title))
                fileListView.Items[dfi].SubItems[1].Text = _activeFiles[afi].Item2.Title;
            //colStyle[1] = string.IsNullOrWhiteSpace(_activeFiles[afi].Item2.Title) ? ColumnHeaderAutoResizeStyle.HeaderSize : ColumnHeaderAutoResizeStyle.ColumnContent;
            if (!string.IsNullOrEmpty(_activeFiles[afi].Item2.Series))
                fileListView.Items[dfi].SubItems[2].Text = _activeFiles[afi].Item2.Series;
            //colStyle[2] = string.IsNullOrWhiteSpace(_activeFiles[afi].Item2.Series) ? ColumnHeaderAutoResizeStyle.HeaderSize : ColumnHeaderAutoResizeStyle.ColumnContent;
            if (!string.IsNullOrEmpty(_activeFiles[afi].Item2.Volume))
                fileListView.Items[dfi].SubItems[3].Text = _activeFiles[afi].Item2.Volume;
            //colStyle[3] = string.IsNullOrWhiteSpace(_activeFiles[afi].Item2.Volume) ? ColumnHeaderAutoResizeStyle.HeaderSize : ColumnHeaderAutoResizeStyle.ColumnContent;
            if (!string.IsNullOrEmpty(_activeFiles[afi].Item2.Writer))
                fileListView.Items[dfi].SubItems[4].Text = _activeFiles[afi].Item2.Writer;
            //colStyle[4] = string.IsNullOrWhiteSpace(_activeFiles[afi].Item2.Writer) ? ColumnHeaderAutoResizeStyle.HeaderSize : ColumnHeaderAutoResizeStyle.ColumnContent;
            if (!string.IsNullOrEmpty(_activeFiles[afi].Item2.LanguageISO))
                fileListView.Items[dfi].SubItems[5].Text = _activeFiles[afi].Item2.LanguageISO;
            //colStyle[5] = string.IsNullOrWhiteSpace(_activeFiles[afi].Item2.LanguageISO) ? ColumnHeaderAutoResizeStyle.HeaderSize : ColumnHeaderAutoResizeStyle.ColumnContent;

        }

        fileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);


        //for (int idx = 0; idx < colStyle.Length; idx++)
        //{
        //    fileListView.AutoResizeColumn(idx, colStyle[idx]);
        //}
        fileListView.EndUpdate();
        this.Cursor = Cursors.Default;
    }

    private void UpdateFileList()
    {
        this.Cursor = Cursors.AppStarting;

        if (chkApplyRecursively.Enabled)
        {
            _activeFiles.Clear();
            _activeFiles = _activePath.GetFileInfo(_searchPattern,chkApplyRecursively.Checked);
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

    
}
#endregion
