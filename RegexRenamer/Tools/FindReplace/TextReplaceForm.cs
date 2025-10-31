using Microsoft.Extensions.DependencyModel.Resolution;
using Microsoft.Win32;
using LogEx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static iText.Kernel.Pdf.Colorspace.PdfSpecialCs;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using RegexRenamer.Tools.FindReplace;

namespace RegexRenamer.Forms;

public partial class TextReplaceForm : Form
{
    public TextReplaceForm()
    {
        InitializeComponent();
    }

    private string _originalText = string.Empty;
    private string _originalTextCache = string.Empty;
    private string _replaceText = string.Empty;

    public string ResultText { get { return txtPreview.Text; } }

    public TextReplaceForm(string inputText) : this() {

        var template = ProcessTemplateFile.GetAllTemplateFiles();
        if(template != null && template.Count > 0)
        {
            cbTemplateNames.Items.AddRange(template.ToArray());
        }
        else
        {
            cbTemplateNames.Items.Add("ProcessCrimeNovel");
        }
        cbTemplateNames.SelectedIndex = 0;
        cbTemplateNames.SelectedIndexChanged += (s,e) => BttnHandle_Click("SelectTemplate");
        _originalText = inputText;

        Load += TextReplaceForm_Load;
        Shown += TextReplaceForm_Shown;
        FormClosing += TextReplaceForm_FormClosing;

        bttnOk.Click += BttnClose_Click;
        bttnCancel.Click += BttnClose_Click;

        this.Text = "Find and Replace Text";

        bttnAutoProcess.Text = "Action";
        bttnAutoProcess.ShowMenuAlways = true;
        bttnAutoProcess.Tag = null;
        bttnAutoProcess.Click += (s, e) => BttnHandle_Click(((Control)s).Tag as string);
        processCMProcessToolStripMenuItem.Click += (s,e) => BttnHandle_Click("Process");
        enterInteractiveModeCMProcessToolStripMenuItem.Click += (s, e) => BttnHandle_Click("EnterInteractive");
        exitInteractiveModeToolStripMenuItem.Click += (s, e) => BttnHandle_Click("ExitInteractive");
        resetCMProcessToolStripMenuItem.Click += (s, e) => BttnHandle_Click("Reset");
        skipProcessingCMProcessToolStripMenuItem.Click += (s, e) => BttnHandle_Click("SkipProcessing");
        updatePreviewCMProcessToolStripMenuItem.Click += (s, e) => BttnHandle_Click("UpdatePreview");

        txtPreview.TextChanged += TxtPreview_TextChanged;

        lstFoundItems.SelectedIndexChanged += LstFoundItems_SelectedIndexChanged;
        this.Resize += TextReplaceForm_Resize;
    }

    private void TextReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        SaveRegexHistory();
    }

    private void TextReplaceForm_Shown(object sender, EventArgs e)
    {
    }

    private bool _isProcessing = false;
    private void TextReplaceForm_Load(object sender, EventArgs e)
    {
        try
        {
            _isProcessing = true;
           
            txtPreview.Enabled = true;
            txtPreview.ReadOnly = false;

            //CultureInfo tamilCulture = new CultureInfo("ta-IN");
            //// Set the current thread's culture. This affects all subsequent
            //// formatting operations on this thread.
            //Thread.CurrentThread.CurrentCulture = tamilCulture;
            //Thread.CurrentThread.CurrentUICulture = tamilCulture;

            LoadRegexHistory();
            UpdateTemplateSelection();
            ProcessTemplate(false);

            lstFoundItems.Columns[0].Width = lstFoundItems.Width / 2;
            lstFoundItems.Columns[1].Width = lstFoundItems.Width / 2 - 20;
        }
        finally
        {
            _isProcessing = false;
        }
    }


    private void TxtPreview_TextChanged(object sender, EventArgs e)
    {
        if (_ignoreTextChangeEvent)
            return;

        _replaceText = txtPreview.Text;
        if(_interactivePattern != null)
        {
            var allFound = _interactivePattern.FindAll(_replaceText);
            lstFoundItems.UpdateListView(allFound);
        }
        if(_currentPattern != null)
        {
            var allFound = _currentPattern.FindAll(_replaceText);
            lstFoundItems.UpdateListView(allFound);
        }
    }

    private bool _ignoreTextChangeEvent = false;
    private void UpdatePreviewText(string text)
    {
        _ignoreTextChangeEvent = true;
        txtPreview.Text = text;
        _ignoreTextChangeEvent = false;
    }

    private bool _isInteractiveMode = false;
    private SearchInfo _interactivePattern = null;

    private void EnterInteractiveMode()
    {
        _isInteractiveMode = true;
        bttnAutoProcess.ShowMenuAlways = false;
        bttnAutoProcess.Tag = "UpdatePreview";
        bttnAutoProcess.Text = "Update Preview";
        cbTemplateNames.Enabled = false;
        this.Text = "Find and Replace Text - Interactive Mode";
    }

    private void ExitInteractiveMode()
    {
        _isInteractiveMode = false;
        _interactivePattern = null;

        bttnAutoProcess.Tag = null;
        bttnAutoProcess.Text = "Action";
        bttnAutoProcess.ShowMenuAlways = true;

        cbTemplateNames.Enabled = true;
        this.Text = "Find and Replace Text";
    }

    private void BttnHandle_Click(string actionName)
    {
        if (_isProcessing || string.IsNullOrEmpty(actionName))
            return;

        try
        {             
            _isProcessing = true;
            switch(actionName)
            {
                case "Process":
                    {
                        // if it is running on interactive mode, then process the interactive pattern
                        // instead of current review pattern
                        if (_isInteractiveMode)
                        {
                            ProcessTemplate(true);

                        }
                        else
                        {
                            ProcessTemplate(false);
                        }
                        cbSearchPattern.AddUniqueItem(cbSearchPattern.Text);
                        cbReplacePattern.AddUniqueItem(cbReplacePattern.Text);
                        SaveRegexHistory();
                    }
                    break;
                case "SkipProcessing":
                    {
                        // not in interactive mode then skip current pattern else skip interactive pattern
                        if (!_isInteractiveMode)
                        {
                            _currentPatternIndex++;
                        }
                        ExitInteractiveMode() ;
                        _currentPattern = null;
                        ProcessTemplate(false);
                    }
                    break;
                case "UpdatePreview":
                    {
                        if (_isInteractiveMode)
                        {
                            // update the interactive pattern preview
                            _interactivePattern = new Tools.FindReplace.SearchInfo
                            {
                                Pattern = cbSearchPattern.Text,
                                Replace = cbReplacePattern.Text,
                                Type = FindReplaceType.Regex,
                                IsCaseSensitive = chkIgnoreCase.Checked,
                                ActionType = "Review"
                            };
                            var allFound = _interactivePattern.FindAll(_replaceText);
                            lstFoundItems.UpdateListView(allFound);
                        }
                        else if(_currentPattern != null)
                        {
                            var allFound = _currentPattern.FindAll(_replaceText);
                            lstFoundItems.UpdateListView(allFound);
                        }
                    }
                    break;
                case "EnterInteractive":
                    {
                        // Enter into interactive mode with current review pattern
                        EnterInteractiveMode();
                        _interactivePattern = new Tools.FindReplace.SearchInfo
                        {
                            Pattern = cbSearchPattern.Text,
                            Replace = cbReplacePattern.Text,
                            Type =  FindReplaceType.Regex,
                            IsCaseSensitive = chkIgnoreCase.Checked,
                            ActionType = "Review"
                        };
                        var allFound = _interactivePattern.FindAll(_replaceText);
                        lstFoundItems.UpdateListView(allFound);
                    }
                    break;
                case "ExitInteractive":
                    {
                        ExitInteractiveMode();
                        ProcessTemplate(false);
                    }
                    break;
                case "SelectTemplate":
                case "Reset":
                    {
                        //reset all mode
                        ExitInteractiveMode();
                        _currentPatternIndex = 0;
                        _replaceText = _originalText;
                        _currentPattern = null;
                        ProcessTemplate(false);
                    }
                    break;
                default:
                    break;
            }
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private void BttnClose_Click(object sender, EventArgs e)
    {
        this.DialogResult = ((Button)sender).DialogResult;
        this.Close();
    }

    private void TextReplaceForm_Resize(object sender, EventArgs e)
    {
        lstFoundItems.Columns[0].Width = lstFoundItems.Width / 2;
        lstFoundItems.Columns[1].Width = lstFoundItems.Width / 2 - 20;
    }

    private void LstFoundItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(_isProcessing)
            return;

        if (lstFoundItems.SelectedItems.Count != 1)
            return;
        var selectedItem = lstFoundItems.SelectedItems[0];
        if (selectedItem.Tag is FoundInfo foundItem)
        {
            txtPreview.HideSelection = false;
            txtPreview.SelectionStart = foundItem.StartIndex;
            txtPreview.SelectionLength = foundItem.MatchValue.Length;
            txtPreview.ScrollToCaret(); // brings the selection into view
        }
    }

    private ProcessTemplateFile _currentTemplate = null;
    private int _currentPatternIndex = -1;
    
    private void UpdateTemplateSelection()
    {
        _currentPatternIndex = 0;
        if (_currentTemplate == null)
        {
            var templateName = cbTemplateNames.SelectedItem as string;
            _currentTemplate = ProcessTemplateFile.GetTemplateByName(templateName);
        }
    }

    private SearchInfo _currentPattern = null;
    private void ProcessTemplate(bool processInteractive)
    {
        if (_currentTemplate == null || 
            _currentTemplate.TextFileProcessing == null || _currentTemplate.TextFileProcessing.Count == 0)
        {
            return;
        }

        if(string.IsNullOrEmpty(_replaceText))
        {
            if (string.IsNullOrEmpty(_originalTextCache))
            {
                UpdatePreviewText(_originalText);
                _originalTextCache = txtPreview.Text;
            }
            _replaceText = _originalTextCache;
        }

        if (processInteractive)
        {
            var checkedItems = lstFoundItems.GetAllCheckedItems();
            _replaceText = _replaceText.ReplaceAll(checkedItems);
        }
        else
        {
            do
            {
                if (_currentPatternIndex >= _currentTemplate.TextFileProcessing.Count)
                {
                    // All done
                    break;
                }

                var nextPattern = _currentTemplate.TextFileProcessing[_currentPatternIndex];
                if (string.Compare(nextPattern.ActionType, "Review", StringComparison.OrdinalIgnoreCase) == 0 &&
                    _currentPattern == null)
                {
                    // Only next action is allowed 
                    var allFound = nextPattern.FindAll(_replaceText);
                    lstFoundItems.UpdateListView(allFound);
                    _currentPattern = nextPattern;
                    cbSearchPattern.Text = nextPattern.Pattern;
                    cbReplacePattern.Text = nextPattern.Replace;
                    chkIgnoreCase.Checked = !nextPattern.IsCaseSensitive;
                    break;
                }
                else if (string.Compare(nextPattern.ActionType, "Review", StringComparison.OrdinalIgnoreCase) == 0 &&
                    _currentPattern != null)
                {
                    var checkedItems = lstFoundItems.GetAllCheckedItems();
                    _replaceText = _replaceText.ReplaceAll(checkedItems);
                    _currentPattern = null;
                    _currentPatternIndex++;
                }
                else if (string.Compare(nextPattern.ActionType, "Auto", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _currentPattern = null;
                    var allFound = nextPattern.FindAll(_replaceText);
                    _replaceText = _replaceText.ReplaceAll(allFound);
                    _currentPatternIndex++;
                }
                else
                {
                    ErrorLog.Inst.ShowError("ProcessTeamplate Unknown ActionType: " + nextPattern.ActionType);
                }
            } while (true);
        }

        UpdatePreviewText(_replaceText);
        _replaceText = txtPreview.Text;
        //// After setting the selection in the TextBox or RichTextBox
        txtPreview.SelectionStart = 0; // or your desired index
        txtPreview.SelectionLength = 0; // optional, for selecting text
        txtPreview.ScrollToCaret(); // brings the selection into view
    }


    private void LoadRegexHistory()
    {

        this.cbSearchPattern.LoadFromConfigFile();
        this.cbReplacePattern.LoadFromConfigFile();
    }

    
    private void SaveRegexHistory()
    {
        this.cbSearchPattern.SaveToConfigFile();
        this.cbReplacePattern.SaveToConfigFile();
    }
}
