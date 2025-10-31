using Microsoft.VisualBasic.CompilerServices;
using PdfSharp.Pdf.Filters;
using RegexRenamer.Controls;
using LogEx;
using RegexRenamer.Native;
using RegexRenamer.Tools.TxtFiles;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegexRenamer.Tools.Calbre;
using RegexRenamer.Tools.EBookPDFTools;
using DarkModeForms;

namespace RegexRenamer.Forms;

public partial class FileToolsForm : Form
{

    private readonly List<FileInfo> _activeFiles;
    private readonly string _action;
    private readonly string _rootFolder;
    

    public FileToolsForm(string rootFolder, List<FileInfo> files, string title, string action) 
    {
        this._activeFiles = files;
        this._rootFolder = rootFolder;
        this._action = action;
        InitializeComponent();
        listViewFiles.Items.Clear();
        this.Text = title;
       

        cmMenuConvertToEPUBToolStripMenuItem.Click += async (s, e) => await PerformConversionAction("ConvertToEPUB");
        cmMenuConvertToPDFToolStripMenuItem.Click += async (s, e) => await PerformConversionAction("ConvertToPDF");

        cmMenuToolsPolishEPUBToolStripMenuItem.Click += async (s, e) => await PerformConversionAction("PolishEPUB");
        cmMenuToolsRemovePDFOwnerPassToolStripMenuItem.Click += async (s, e) => await PerformConversionAction("RemoveOwnerPassPDF");
        cmMenuToolsRemovePDFSignatureToolStripMenuItem.Click += async (s, e) => await PerformConversionAction("RemoveSignaturePDF");
        cmMenuToolsReFormatTextFilesToolStripMenuItem.Click += async (s, e) => await PerformConversionAction("ReFormatTextFiles");

        bttnConvert.Click += async (s, e) => await PerformConversionAction("ConvertToEPUB");
        //bttnTools.Click += async (s, e) => cm ;

        this.Load += ConvertSelectionForm_Load;
        this.Shown += ConvertSelectionForm_Shown;
        this.FormClosing += Form_Closing;
    }

    private void Form_Closing(object sender, FormClosingEventArgs e)
    {
        if (_preventCancel && e.CloseReason == CloseReason.UserClosing)
        {
            MessageBox.Show("Operation in progress. Please wait until it completes.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            e.Cancel = true;
        }
    }

    private async void ConvertSelectionForm_Shown(object sender, EventArgs e)
    {
        await PerformConversionAction(_action);
    }

    private void ConvertSelectionForm_Load(object sender, EventArgs e)
    {
        cmbLanguage.SetCueBanner("Set language code, ta->tamil en->english etc");

        RefreshListView();
    }

    private void PerformAction(string action)
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
            default:
                break;
        }
    }

    private bool _preventCancel = false;

    private async Task PerformConversionAction(string action)
    {
        switch (action)
        {
            case "ReFormatTextFiles":
                {
                    _preventCancel = true;
                    try
                    {
                        pbToolsForm.Value = 0;
                        pbToolsForm.Maximum = _activeFiles.Count;
                        FormWindowState windowState = FormWindowState.Normal;
                        for (int index = 0; index < _activeFiles.Count; index++)
                        {
                            var file = _activeFiles[index];
                            pbToolsForm.Value = index + 1;
                            lblStatus.Text = $"Re-formating the text file {file.Name} ({index + 1} of {_activeFiles.Count})";
                            listViewFiles.SelectedItems.Clear();
                            listViewFiles.Items[index].Selected = true;
                            Application.DoEvents();
                            try
                            {
                                var fileExt = Path.GetExtension(file.FullName).ToLowerInvariant();
                                if(fileExt == ".txt")
                                {
                                    var processor = new ReFormatTextFiles(file.FullName);
                                    var (windowStateRet, dresult) = processor.Process(windowState);
                                    if(dresult == DialogResult.Cancel && (index + 1) < _activeFiles.Count)
                                    {
                                        if(MessageBox.Show("Do you want to cancel the re-formatting operation?", "Cancel Operation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.Inst.ShowError($"Failed to Re-Format the text file {file.Name}:\r\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        ErrorLog.Inst.ShowError($"An error occurred while ReFormatTextFiles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        _preventCancel = false;
                    }
                }
                break;
            case "RemoveSignaturePDF":
                {
                    _preventCancel = true;
                    try
                    {
                        // Apply changes without closing
                        // (Implementation of applying changes goes here)
                        if (MessageBox.Show("Do you want to remove pdf signature for all the files now?", "PDF Remove signature", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            break;
                        }
                        pbToolsForm.Value = 0;
                        pbToolsForm.Maximum = _activeFiles.Count;

                        List<KeyValue> fields = new List<KeyValue> {
                           new KeyValue("Password","Password", KeyValue.ValueTypes.Password)
                           };

                        string password = string.Empty;
                        if (Messenger.InputBox("Remove PDF Signature", "Enter pdf password if any:", ref fields,
                            MsgIcon.Edit, MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            password = fields[0].Value;
                        }
                        for (int index = 0; index < _activeFiles.Count; index++)
                        {
                            var file = _activeFiles[index];
                            pbToolsForm.Value = index + 1;
                            lblStatus.Text = $"Removing signature {file.Name} ({index + 1} of {_activeFiles.Count})";
                            listViewFiles.SelectedItems.Clear();
                            listViewFiles.Items[index].Selected = true;
                            Application.DoEvents();
                            try
                            {
                                ITextPDFHelper.RemoveSign(file.FullName, password);
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.Inst.ShowError($"Failed to remove signature {file.Name}:\r\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Inst.ShowError($"An error occurred while removing signature: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        _preventCancel = false;
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                break;
            case "RemoveOwnerPassPDF":
                {
                    _preventCancel = true;
                    try
                    {
                        // Apply changes without closing
                        // (Implementation of applying changes goes here)
                        if (MessageBox.Show("Do you want to remove owner password for all the files now?", "PDF Remove owner password", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            break;
                        }
                        pbToolsForm.Value = 0;
                        pbToolsForm.Maximum = _activeFiles.Count;
                        for (int index = 0; index < _activeFiles.Count; index++)
                        {
                            var file = _activeFiles[index];
                            pbToolsForm.Value = index + 1;
                            lblStatus.Text = $"Removing password {file.Name} ({index + 1} of {_activeFiles.Count})";
                            listViewFiles.SelectedItems.Clear();
                            listViewFiles.Items[index].Selected = true;
                            Application.DoEvents();
                            try
                            {
                                ITextPDFHelper.RemoveOwnerPassword(file.FullName);
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.Inst.ShowError($"Failed to remove owner password {file.Name}:\r\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Inst.ShowError($"An error occurred while removing owner password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        _preventCancel = false;
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                break;
            case "PolishEPUB":
                {
                    _preventCancel = true;
                    try
                    {
                        // Apply changes without closing
                        // (Implementation of applying changes goes here)
                        if (MessageBox.Show("Do you want to Polish epub for all the files now?", "Polish Epub", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            break;
                        }
                        pbToolsForm.Value = 0;
                        pbToolsForm.Maximum = _activeFiles.Count;
                        for (int index = 0; index < _activeFiles.Count; index++)
                        {
                            var file = _activeFiles[index];
                            pbToolsForm.Value = index + 1;
                            lblStatus.Text = $"Polishing {file.Name} ({index + 1} of {_activeFiles.Count})";
                            listViewFiles.SelectedItems.Clear();
                            listViewFiles.Items[index].Selected = true;
                            Application.DoEvents();
                            try
                            {
                                string destFile = Path.ChangeExtension(file.FullName, ".epub");
                                await EBookHelper.PolishEbook(file.FullName);
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.Inst.ShowError($"Failed to polish {file.Name}:\r\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Inst.ShowError($"An error occurred while polishing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        _preventCancel = false;
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                break;
            case "ConvertToEPUB":
                {
                    _preventCancel = true;
                    try
                    {
                        // Apply changes without closing
                        // (Implementation of applying changes goes here)
                        if (MessageBox.Show("Do you want to convert all the files now?", "Convert to Epub file format", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            break;
                        }
                        pbToolsForm.Value = 0;
                        pbToolsForm.Maximum = _activeFiles.Count;
                        Dictionary<string,CalibreOptions> inputOptions = new Dictionary<string,CalibreOptions>();
                        for (int index = 0; index < _activeFiles.Count; index++)
                        {
                            var file = _activeFiles[index];
                            pbToolsForm.Value = index + 1;
                            lblStatus.Text = $"Converting {file.Name} ({index + 1} of {_activeFiles.Count})";
                            listViewFiles.SelectedItems.Clear();
                            listViewFiles.Items[index].Selected = true;
                            var filePath = file.FullName;
                            var fileExt = Path.GetExtension(filePath).ToLowerInvariant();
                            var extraArgs = string.Empty;
                            if(!inputOptions.ContainsKey(fileExt))
                            {
                                var calibreOptionsUser = CalibreOptionsFactory.CreateInputFileOptions(fileExt);
                                var userOption = GetConversionOptions.GetInputeOption(calibreOptionsUser);
                                var calibreOptions = userOption == DialogResult.OK ? calibreOptionsUser : CalibreOptionsFactory.CreateInputFileOptions(string.Empty);
                                inputOptions[fileExt] = calibreOptions;
                            }
                            extraArgs = inputOptions[fileExt].ToString();
                            Application.DoEvents();
                            try
                            {
                                string destFile = Path.ChangeExtension(filePath, ".epub");
                                await CalibreHelper.ConvertCalibre(filePath, destFile);
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.Inst.ShowError($"Failed to convert {file.Name}:\r\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Inst.ShowError($"An error occurred while converting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        _preventCancel = false;
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                break;
            case "ConvertToPDF":
                {
                    _preventCancel = false;
                    try
                    {
                        // Apply changes without closing
                        // (Implementation of applying changes goes here)
                        if (MessageBox.Show("Do you want to apply the changes now?", "Apply Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            break;
                        }

                        pbToolsForm.Value = 0;
                        pbToolsForm.Maximum = _activeFiles.Count;

                        for (int index = 0; index < _activeFiles.Count; index++)
                        {
                            var file = _activeFiles[index];
                            pbToolsForm.Value = index + 1;
                            lblStatus.Text = $"Converting {file.Name} ({index + 1} of {_activeFiles.Count})";
                            listViewFiles.SelectedItems.Clear();
                            listViewFiles.Items[index].Selected = true;
                            Application.DoEvents();
                            try
                            {
                                string destFile = Path.ChangeExtension(file.FullName, ".pdf");
                                await CalibreHelper.ConvertCalibre(file.FullName, destFile);
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.Inst.ShowError($"Failed to convert {file.Name}:\r\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Inst.ShowError($"An error occurred while converting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                }
                break;
            default:
                break;
        }
    }

    private ListViewItem[] GetProcessedList(string realativeTo, List<FileInfo> inFiles)
    {
        if (string.IsNullOrEmpty(realativeTo))
        {
            return inFiles.Select((fileInfo) => new ListViewItem(fileInfo.FullName) { Tag = fileInfo }).ToArray();
        }
        return inFiles.Select((fileInfo) => new ListViewItem(Path.GetRelativePath(realativeTo,fileInfo.FullName)) { Tag = fileInfo }).ToArray();
    }

    private void RefreshListView()
    {
        var searchResults = GetProcessedList(_rootFolder, _activeFiles);

        listViewFiles.BeginUpdate();
        listViewFiles.Items.Clear();
        listViewFiles.Items.AddRange(searchResults.ToArray());
        listViewFiles.EndUpdate();
        listViewFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
    }

    
}
