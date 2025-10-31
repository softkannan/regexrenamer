using RegexRenamer.Controls;
using RegexRenamer.Controls.ListViewExCtrl;

namespace RegexRenamer.Forms;

partial class FileToolsForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        listViewFiles = new ListViewEx();
        columnFiles = new System.Windows.Forms.ColumnHeader();
        lblStatus = new System.Windows.Forms.Label();
        pbToolsForm = new System.Windows.Forms.ProgressBar();
        bttnConvert = new SplitButton();
        cmMenuConvert = new System.Windows.Forms.ContextMenuStrip(components);
        cmMenuConvertToEPUBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        cmMenuConvertToPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        bttnTools = new SplitButton();
        cmMenuTools = new System.Windows.Forms.ContextMenuStrip(components);
        cmMenuToolsPolishEPUBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        cmMenuToolsRemovePDFOwnerPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        cmMenuToolsReFormatTextFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        cmbLanguage = new MyComboBox();
        label6 = new System.Windows.Forms.Label();
        cmMenuToolsRemovePDFSignatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        cmMenuConvert.SuspendLayout();
        cmMenuTools.SuspendLayout();
        SuspendLayout();
        // 
        // listViewFiles
        // 
        listViewFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnFiles });
        listViewFiles.FullRowSelect = true;
        listViewFiles.Location = new System.Drawing.Point(15, 60);
        listViewFiles.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        listViewFiles.Name = "listViewFiles";
        listViewFiles.Size = new System.Drawing.Size(1100, 646);
        listViewFiles.TabIndex = 3;
        listViewFiles.UseCompatibleStateImageBehavior = false;
        listViewFiles.View = System.Windows.Forms.View.Details;
        // 
        // columnFiles
        // 
        columnFiles.Text = "Files";
        columnFiles.Width = 200;
        // 
        // lblStatus
        // 
        lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        lblStatus.Location = new System.Drawing.Point(292, 725);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new System.Drawing.Size(569, 22);
        lblStatus.TabIndex = 23;
        lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // pbToolsForm
        // 
        pbToolsForm.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        pbToolsForm.Location = new System.Drawing.Point(15, 725);
        pbToolsForm.Name = "pbToolsForm";
        pbToolsForm.Size = new System.Drawing.Size(271, 23);
        pbToolsForm.TabIndex = 22;
        // 
        // bttnConvert
        // 
        bttnConvert.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        bttnConvert.AutoSize = true;
        bttnConvert.ContextMenuStrip = cmMenuConvert;
        bttnConvert.Location = new System.Drawing.Point(994, 712);
        bttnConvert.Name = "bttnConvert";
        bttnConvert.Size = new System.Drawing.Size(121, 42);
        bttnConvert.TabIndex = 24;
        bttnConvert.Text = "Convert to EPUB";
        bttnConvert.UseVisualStyleBackColor = true;
        // 
        // cmMenuConvert
        // 
        cmMenuConvert.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { cmMenuConvertToEPUBToolStripMenuItem, cmMenuConvertToPDFToolStripMenuItem });
        cmMenuConvert.Name = "ccMenuConvert";
        cmMenuConvert.Size = new System.Drawing.Size(162, 48);
        // 
        // cmMenuConvertToEPUBToolStripMenuItem
        // 
        cmMenuConvertToEPUBToolStripMenuItem.Name = "cmMenuConvertToEPUBToolStripMenuItem";
        cmMenuConvertToEPUBToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
        cmMenuConvertToEPUBToolStripMenuItem.Text = "Convert to EPUB";
        // 
        // cmMenuConvertToPDFToolStripMenuItem
        // 
        cmMenuConvertToPDFToolStripMenuItem.Name = "cmMenuConvertToPDFToolStripMenuItem";
        cmMenuConvertToPDFToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
        cmMenuConvertToPDFToolStripMenuItem.Text = "Convert to PDF";
        // 
        // bttnTools
        // 
        bttnTools.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        bttnTools.AutoSize = true;
        bttnTools.ContextMenuStrip = cmMenuTools;
        bttnTools.Location = new System.Drawing.Point(867, 712);
        bttnTools.Name = "bttnTools";
        bttnTools.ShowMenuAlways = true;
        bttnTools.Size = new System.Drawing.Size(121, 42);
        bttnTools.TabIndex = 25;
        bttnTools.Text = "Tools";
        bttnTools.UseVisualStyleBackColor = true;
        // 
        // cmMenuTools
        // 
        cmMenuTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { cmMenuToolsPolishEPUBToolStripMenuItem, cmMenuToolsRemovePDFSignatureToolStripMenuItem, cmMenuToolsRemovePDFOwnerPassToolStripMenuItem, cmMenuToolsReFormatTextFilesToolStripMenuItem });
        cmMenuTools.Name = "contextMenuStrip1";
        cmMenuTools.Size = new System.Drawing.Size(206, 114);
        // 
        // cmMenuToolsPolishEPUBToolStripMenuItem
        // 
        cmMenuToolsPolishEPUBToolStripMenuItem.Name = "cmMenuToolsPolishEPUBToolStripMenuItem";
        cmMenuToolsPolishEPUBToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
        cmMenuToolsPolishEPUBToolStripMenuItem.Text = "Polish EPUB";
        // 
        // cmMenuToolsRemovePDFOwnerPassToolStripMenuItem
        // 
        cmMenuToolsRemovePDFOwnerPassToolStripMenuItem.Name = "cmMenuToolsRemovePDFOwnerPassToolStripMenuItem";
        cmMenuToolsRemovePDFOwnerPassToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
        cmMenuToolsRemovePDFOwnerPassToolStripMenuItem.Text = "Remove PDF Owner Pass";
        // 
        // cmMenuToolsReFormatTextFilesToolStripMenuItem
        // 
        cmMenuToolsReFormatTextFilesToolStripMenuItem.Name = "cmMenuToolsReFormatTextFilesToolStripMenuItem";
        cmMenuToolsReFormatTextFilesToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
        cmMenuToolsReFormatTextFilesToolStripMenuItem.Text = "Re-Format Text Files";
        // 
        // cmbLanguage
        // 
        cmbLanguage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        cmbLanguage.AutoCompleteCustomSource.AddRange(new string[] { "$1" });
        cmbLanguage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
        cmbLanguage.Items.AddRange(new object[] { "ta", "en" });
        cmbLanguage.Location = new System.Drawing.Point(85, 12);
        cmbLanguage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        cmbLanguage.Name = "cmbLanguage";
        cmbLanguage.Size = new System.Drawing.Size(170, 23);
        cmbLanguage.TabIndex = 111;
        // 
        // label6
        // 
        label6.Location = new System.Drawing.Point(13, 13);
        label6.Name = "label6";
        label6.Size = new System.Drawing.Size(65, 22);
        label6.TabIndex = 112;
        label6.Text = "Language:";
        label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // cmMenuToolsRemovePDFSignatureToolStripMenuItem
        // 
        cmMenuToolsRemovePDFSignatureToolStripMenuItem.Name = "cmMenuToolsRemovePDFSignatureToolStripMenuItem";
        cmMenuToolsRemovePDFSignatureToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
        cmMenuToolsRemovePDFSignatureToolStripMenuItem.Text = "Remove PDF Signature";
        // 
        // FileToolsForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1130, 769);
        Controls.Add(cmbLanguage);
        Controls.Add(label6);
        Controls.Add(bttnTools);
        Controls.Add(bttnConvert);
        Controls.Add(lblStatus);
        Controls.Add(pbToolsForm);
        Controls.Add(listViewFiles);
        Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        Name = "FileToolsForm";
        ShowInTaskbar = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Convert files";
        cmMenuConvert.ResumeLayout(false);
        cmMenuTools.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();

    }

    #endregion
    private ListViewEx listViewFiles;
    private System.Windows.Forms.ColumnHeader columnFiles;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.ProgressBar pbToolsForm;
    private SplitButton bttnConvert;
    private System.Windows.Forms.ContextMenuStrip cmMenuConvert;
    private System.Windows.Forms.ToolStripMenuItem cmMenuConvertToEPUBToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cmMenuConvertToPDFToolStripMenuItem;
    private SplitButton bttnTools;
    private System.Windows.Forms.ContextMenuStrip cmMenuTools;
    private System.Windows.Forms.ToolStripMenuItem cmMenuToolsPolishEPUBToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cmMenuToolsRemovePDFOwnerPassToolStripMenuItem;
    private MyComboBox cmbLanguage;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.ToolStripMenuItem cmMenuToolsReFormatTextFilesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cmMenuToolsRemovePDFSignatureToolStripMenuItem;
}