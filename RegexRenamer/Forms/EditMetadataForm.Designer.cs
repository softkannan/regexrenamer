using RegexRenamer.Controls.ListViewExCtrl;

namespace RegexRenamer.Forms
{
    partial class EditMetadataForm
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
            bttnClear = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            bttnApply = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            cmbMatch = new System.Windows.Forms.ComboBox();
            cmbSeries = new System.Windows.Forms.ComboBox();
            cmbTitle = new System.Windows.Forms.ComboBox();
            cmbAuthor = new System.Windows.Forms.ComboBox();
            cmsBlank = new System.Windows.Forms.ContextMenuStrip(components);
            cmRegexMatch = new System.Windows.Forms.ContextMenuStrip(components);
            cbModifierI = new System.Windows.Forms.CheckBox();
            cbModifierX = new System.Windows.Forms.CheckBox();
            tsMenu = new System.Windows.Forms.ToolStrip();
            mnuChangeCase = new System.Windows.Forms.ToolStripDropDownButton();
            itmChangeCaseNoChange = new System.Windows.Forms.ToolStripMenuItem();
            itmChangeCaseSep = new System.Windows.Forms.ToolStripSeparator();
            itmChangeCaseUppercase = new System.Windows.Forms.ToolStripMenuItem();
            itmChangeCaseLowercase = new System.Windows.Forms.ToolStripMenuItem();
            itmChangeCaseTitlecase = new System.Windows.Forms.ToolStripMenuItem();
            itmChangeCaseCleanName = new System.Windows.Forms.ToolStripMenuItem();
            mnuNumbering = new System.Windows.Forms.ToolStripDropDownButton();
            txtNumberingStart = new System.Windows.Forms.ToolStripTextBox();
            txtNumberingPad = new System.Windows.Forms.ToolStripTextBox();
            txtNumberingInc = new System.Windows.Forms.ToolStripTextBox();
            txtNumberingReset = new System.Windows.Forms.ToolStripTextBox();
            cmFileList = new System.Windows.Forms.ContextMenuStrip(components);
            readMetadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pbMetadataWrite = new System.Windows.Forms.ProgressBar();
            lblStatus = new System.Windows.Forms.Label();
            cmbVolume = new RegexRenamer.Controls.MyComboBox();
            label5 = new System.Windows.Forms.Label();
            chkIgnoreError = new System.Windows.Forms.CheckBox();
            chkApplyRecursively = new System.Windows.Forms.CheckBox();
            fileListView = new ListViewEx();
            colFileName = new System.Windows.Forms.ColumnHeader();
            colTitle = new System.Windows.Forms.ColumnHeader();
            colSeries = new System.Windows.Forms.ColumnHeader();
            colVolume = new System.Windows.Forms.ColumnHeader();
            colAuthor = new System.Windows.Forms.ColumnHeader();
            colLanguage = new System.Windows.Forms.ColumnHeader();
            cmbLanguage = new RegexRenamer.Controls.MyComboBox();
            label6 = new System.Windows.Forms.Label();
            cbModifierG = new System.Windows.Forms.CheckBox();
            cmbPDFToolsList = new System.Windows.Forms.ComboBox();
            cmbEPUBToolsList = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            cmbViewMode = new System.Windows.Forms.ComboBox();
            tsMenu.SuspendLayout();
            cmFileList.SuspendLayout();
            SuspendLayout();
            // 
            // bttnClear
            // 
            bttnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            bttnClear.Location = new System.Drawing.Point(1049, 695);
            bttnClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            bttnClear.Name = "bttnClear";
            bttnClear.Size = new System.Drawing.Size(107, 38);
            bttnClear.TabIndex = 15;
            bttnClear.Text = "Clear";
            bttnClear.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(15, 10);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(107, 22);
            label1.TabIndex = 16;
            label1.Text = "File Name:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(15, 39);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(107, 22);
            label2.TabIndex = 101;
            label2.Text = "Series:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(14, 95);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(107, 22);
            label3.TabIndex = 103;
            label3.Text = "Title:";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(14, 122);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(107, 22);
            label4.TabIndex = 104;
            label4.Text = "Author:";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bttnApply
            // 
            bttnApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            bttnApply.Location = new System.Drawing.Point(1162, 695);
            bttnApply.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            bttnApply.Name = "bttnApply";
            bttnApply.Size = new System.Drawing.Size(110, 38);
            bttnApply.TabIndex = 0;
            bttnApply.Text = "Apply";
            bttnApply.UseVisualStyleBackColor = true;
            // 
            // cmbMatch
            // 
            cmbMatch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbMatch.AutoCompleteCustomSource.AddRange(new string[] { "(.+)", "(.+)(/d+)" });
            cmbMatch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbMatch.Location = new System.Drawing.Point(129, 10);
            cmbMatch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbMatch.Name = "cmbMatch";
            cmbMatch.Size = new System.Drawing.Size(972, 23);
            cmbMatch.TabIndex = 1;
            // 
            // cmbSeries
            // 
            cmbSeries.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbSeries.AutoCompleteCustomSource.AddRange(new string[] { "$1" });
            cmbSeries.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbSeries.Location = new System.Drawing.Point(128, 37);
            cmbSeries.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbSeries.Name = "cmbSeries";
            cmbSeries.Size = new System.Drawing.Size(973, 23);
            cmbSeries.TabIndex = 2;
            // 
            // cmbTitle
            // 
            cmbTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbTitle.AutoCompleteCustomSource.AddRange(new string[] { "$1" });
            cmbTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbTitle.Location = new System.Drawing.Point(128, 95);
            cmbTitle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbTitle.Name = "cmbTitle";
            cmbTitle.Size = new System.Drawing.Size(973, 23);
            cmbTitle.TabIndex = 4;
            // 
            // cmbAuthor
            // 
            cmbAuthor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbAuthor.AutoCompleteCustomSource.AddRange(new string[] { "$1" });
            cmbAuthor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbAuthor.Location = new System.Drawing.Point(128, 122);
            cmbAuthor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbAuthor.Name = "cmbAuthor";
            cmbAuthor.Size = new System.Drawing.Size(973, 23);
            cmbAuthor.TabIndex = 5;
            // 
            // cmsBlank
            // 
            cmsBlank.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmsBlank.Name = "cmsBlank";
            cmsBlank.Size = new System.Drawing.Size(61, 4);
            // 
            // cmRegexMatch
            // 
            cmRegexMatch.Name = "cmRegexMatch";
            cmRegexMatch.Size = new System.Drawing.Size(61, 4);
            // 
            // cbModifierI
            // 
            cbModifierI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbModifierI.AutoSize = true;
            cbModifierI.Location = new System.Drawing.Point(1112, 14);
            cbModifierI.Margin = new System.Windows.Forms.Padding(4);
            cbModifierI.Name = "cbModifierI";
            cbModifierI.Size = new System.Drawing.Size(34, 19);
            cbModifierI.TabIndex = 7;
            cbModifierI.Tag = false;
            cbModifierI.Text = "/i";
            // 
            // cbModifierX
            // 
            cbModifierX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbModifierX.AutoSize = true;
            cbModifierX.Location = new System.Drawing.Point(1112, 60);
            cbModifierX.Margin = new System.Windows.Forms.Padding(4);
            cbModifierX.Name = "cbModifierX";
            cbModifierX.Size = new System.Drawing.Size(37, 19);
            cbModifierX.TabIndex = 9;
            cbModifierX.Tag = false;
            cbModifierX.Text = "/x";
            // 
            // tsMenu
            // 
            tsMenu.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tsMenu.CanOverflow = false;
            tsMenu.Dock = System.Windows.Forms.DockStyle.None;
            tsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            tsMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuChangeCase, mnuNumbering });
            tsMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            tsMenu.Location = new System.Drawing.Point(1162, 9);
            tsMenu.Name = "tsMenu";
            tsMenu.Size = new System.Drawing.Size(113, 42);
            tsMenu.TabIndex = 14;
            tsMenu.TabStop = true;
            // 
            // mnuChangeCase
            // 
            mnuChangeCase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuChangeCase.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { itmChangeCaseNoChange, itmChangeCaseSep, itmChangeCaseUppercase, itmChangeCaseLowercase, itmChangeCaseTitlecase, itmChangeCaseCleanName });
            mnuChangeCase.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            mnuChangeCase.Name = "mnuChangeCase";
            mnuChangeCase.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            mnuChangeCase.Size = new System.Drawing.Size(111, 19);
            mnuChangeCase.Text = "Change Case";
            mnuChangeCase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            mnuChangeCase.ToolTipText = "Only the matched portion of the filename will have its case changed";
            // 
            // itmChangeCaseNoChange
            // 
            itmChangeCaseNoChange.Checked = true;
            itmChangeCaseNoChange.CheckState = System.Windows.Forms.CheckState.Checked;
            itmChangeCaseNoChange.Name = "itmChangeCaseNoChange";
            itmChangeCaseNoChange.Size = new System.Drawing.Size(139, 22);
            itmChangeCaseNoChange.Text = "No change";
            // 
            // itmChangeCaseSep
            // 
            itmChangeCaseSep.Name = "itmChangeCaseSep";
            itmChangeCaseSep.Size = new System.Drawing.Size(136, 6);
            // 
            // itmChangeCaseUppercase
            // 
            itmChangeCaseUppercase.Name = "itmChangeCaseUppercase";
            itmChangeCaseUppercase.Size = new System.Drawing.Size(139, 22);
            itmChangeCaseUppercase.Text = "Uppercase";
            // 
            // itmChangeCaseLowercase
            // 
            itmChangeCaseLowercase.Name = "itmChangeCaseLowercase";
            itmChangeCaseLowercase.Size = new System.Drawing.Size(139, 22);
            itmChangeCaseLowercase.Text = "Lowercase";
            // 
            // itmChangeCaseTitlecase
            // 
            itmChangeCaseTitlecase.Name = "itmChangeCaseTitlecase";
            itmChangeCaseTitlecase.Size = new System.Drawing.Size(139, 22);
            itmChangeCaseTitlecase.Text = "Title case";
            // 
            // itmChangeCaseCleanName
            // 
            itmChangeCaseCleanName.Name = "itmChangeCaseCleanName";
            itmChangeCaseCleanName.Size = new System.Drawing.Size(139, 22);
            itmChangeCaseCleanName.Text = "Clean Name";
            // 
            // mnuNumbering
            // 
            mnuNumbering.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuNumbering.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { txtNumberingStart, txtNumberingPad, txtNumberingInc, txtNumberingReset });
            mnuNumbering.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            mnuNumbering.Name = "mnuNumbering";
            mnuNumbering.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            mnuNumbering.Size = new System.Drawing.Size(111, 19);
            mnuNumbering.Tag = "mnuNumbering";
            mnuNumbering.Text = "Auto Numbering";
            mnuNumbering.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            mnuNumbering.ToolTipText = "Enter \"$#\" in the replace field to insert a number sequence";
            // 
            // txtNumberingStart
            // 
            txtNumberingStart.MaxLength = 10;
            txtNumberingStart.Name = "txtNumberingStart";
            txtNumberingStart.Size = new System.Drawing.Size(75, 23);
            txtNumberingStart.Tag = true;
            txtNumberingStart.Text = "1";
            txtNumberingStart.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            txtNumberingStart.ToolTipText = "Starting number (or letter)";
            // 
            // txtNumberingPad
            // 
            txtNumberingPad.MaxLength = 10;
            txtNumberingPad.Name = "txtNumberingPad";
            txtNumberingPad.Size = new System.Drawing.Size(75, 23);
            txtNumberingPad.Tag = true;
            txtNumberingPad.Text = "000";
            txtNumberingPad.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            txtNumberingPad.ToolTipText = "Eg: \"0000\" means 14 => 0014";
            // 
            // txtNumberingInc
            // 
            txtNumberingInc.MaxLength = 10;
            txtNumberingInc.Name = "txtNumberingInc";
            txtNumberingInc.Size = new System.Drawing.Size(75, 23);
            txtNumberingInc.Tag = true;
            txtNumberingInc.Text = "1";
            txtNumberingInc.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            txtNumberingInc.ToolTipText = "Increment by x each file (may be negative)";
            // 
            // txtNumberingReset
            // 
            txtNumberingReset.MaxLength = 10;
            txtNumberingReset.Name = "txtNumberingReset";
            txtNumberingReset.Size = new System.Drawing.Size(75, 23);
            txtNumberingReset.Tag = true;
            txtNumberingReset.Text = "0";
            txtNumberingReset.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            txtNumberingReset.ToolTipText = "Reset to starting number every x files";
            // 
            // cmFileList
            // 
            cmFileList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { readMetadataToolStripMenuItem });
            cmFileList.Name = "cmFileList";
            cmFileList.Size = new System.Drawing.Size(154, 26);
            // 
            // readMetadataToolStripMenuItem
            // 
            readMetadataToolStripMenuItem.Name = "readMetadataToolStripMenuItem";
            readMetadataToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            readMetadataToolStripMenuItem.Text = "Read Metadata";
            // 
            // pbMetadataWrite
            // 
            pbMetadataWrite.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            pbMetadataWrite.Location = new System.Drawing.Point(12, 702);
            pbMetadataWrite.Name = "pbMetadataWrite";
            pbMetadataWrite.Size = new System.Drawing.Size(301, 23);
            pbMetadataWrite.TabIndex = 20;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblStatus.Location = new System.Drawing.Point(319, 702);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(724, 22);
            lblStatus.TabIndex = 21;
            lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbVolume
            // 
            cmbVolume.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbVolume.AutoCompleteCustomSource.AddRange(new string[] { "$1" });
            cmbVolume.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbVolume.Location = new System.Drawing.Point(128, 66);
            cmbVolume.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbVolume.Name = "cmbVolume";
            cmbVolume.Size = new System.Drawing.Size(973, 23);
            cmbVolume.TabIndex = 3;
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(15, 68);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(107, 22);
            label5.TabIndex = 102;
            label5.Text = "Volume:";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkIgnoreError
            // 
            chkIgnoreError.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            chkIgnoreError.AutoSize = true;
            chkIgnoreError.Location = new System.Drawing.Point(1162, 54);
            chkIgnoreError.Name = "chkIgnoreError";
            chkIgnoreError.Size = new System.Drawing.Size(88, 19);
            chkIgnoreError.TabIndex = 10;
            chkIgnoreError.Text = "Ignore Error";
            chkIgnoreError.UseVisualStyleBackColor = true;
            // 
            // chkApplyRecursively
            // 
            chkApplyRecursively.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            chkApplyRecursively.AutoSize = true;
            chkApplyRecursively.Location = new System.Drawing.Point(1112, 86);
            chkApplyRecursively.Name = "chkApplyRecursively";
            chkApplyRecursively.Size = new System.Drawing.Size(35, 19);
            chkApplyRecursively.TabIndex = 12;
            chkApplyRecursively.Text = "/r";
            chkApplyRecursively.UseVisualStyleBackColor = true;
            // 
            // fileListView
            // 
            fileListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            fileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { colFileName, colTitle, colSeries, colVolume, colAuthor, colLanguage });
            fileListView.FullRowSelect = true;
            fileListView.GridLines = true;
            fileListView.Location = new System.Drawing.Point(15, 192);
            fileListView.MultiSelect = false;
            fileListView.Name = "fileListView";
            fileListView.Size = new System.Drawing.Size(1257, 498);
            fileListView.TabIndex = 200;
            fileListView.TabStop = false;
            fileListView.UseCompatibleStateImageBehavior = false;
            fileListView.View = System.Windows.Forms.View.Details;
            // 
            // colFileName
            // 
            colFileName.Text = "FileName";
            // 
            // colTitle
            // 
            colTitle.DisplayIndex = 2;
            colTitle.Text = "Title";
            // 
            // colSeries
            // 
            colSeries.DisplayIndex = 1;
            colSeries.Text = "Series";
            // 
            // colVolume
            // 
            colVolume.Text = "Volume";
            // 
            // colAuthor
            // 
            colAuthor.Text = "Author";
            // 
            // colLanguage
            // 
            colLanguage.Text = "Language";
            colLanguage.Width = 80;
            // 
            // cmbLanguage
            // 
            cmbLanguage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbLanguage.AutoCompleteCustomSource.AddRange(new string[] { "$1" });
            cmbLanguage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbLanguage.Items.AddRange(new object[] { "ta", "en" });
            cmbLanguage.Location = new System.Drawing.Point(129, 150);
            cmbLanguage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new System.Drawing.Size(973, 23);
            cmbLanguage.TabIndex = 6;
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(15, 149);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(107, 22);
            label6.TabIndex = 110;
            label6.Text = "Language:";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbModifierG
            // 
            cbModifierG.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbModifierG.AutoSize = true;
            cbModifierG.Location = new System.Drawing.Point(1111, 37);
            cbModifierG.Margin = new System.Windows.Forms.Padding(4);
            cbModifierG.Name = "cbModifierG";
            cbModifierG.Size = new System.Drawing.Size(38, 19);
            cbModifierG.TabIndex = 8;
            cbModifierG.Tag = false;
            cbModifierG.Text = "/g";
            // 
            // cmbPDFToolsList
            // 
            cmbPDFToolsList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cmbPDFToolsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPDFToolsList.FormattingEnabled = true;
            cmbPDFToolsList.Location = new System.Drawing.Point(1151, 148);
            cmbPDFToolsList.Name = "cmbPDFToolsList";
            cmbPDFToolsList.Size = new System.Drawing.Size(121, 23);
            cmbPDFToolsList.TabIndex = 201;
            // 
            // cmbEPUBToolsList
            // 
            cmbEPUBToolsList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cmbEPUBToolsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbEPUBToolsList.FormattingEnabled = true;
            cmbEPUBToolsList.Location = new System.Drawing.Point(1151, 119);
            cmbEPUBToolsList.Name = "cmbEPUBToolsList";
            cmbEPUBToolsList.Size = new System.Drawing.Size(121, 23);
            cmbEPUBToolsList.TabIndex = 202;
            // 
            // label7
            // 
            label7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label7.Location = new System.Drawing.Point(1107, 120);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(38, 22);
            label7.TabIndex = 203;
            label7.Text = "EPUB:";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label8.Location = new System.Drawing.Point(1109, 149);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(38, 22);
            label8.TabIndex = 204;
            label8.Text = "PDF:";
            label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbViewMode
            // 
            cmbViewMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cmbViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbViewMode.FormattingEnabled = true;
            cmbViewMode.Location = new System.Drawing.Point(1151, 82);
            cmbViewMode.Name = "cmbViewMode";
            cmbViewMode.Size = new System.Drawing.Size(121, 23);
            cmbViewMode.TabIndex = 205;
            // 
            // EditMetadataForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1284, 742);
            Controls.Add(cmbViewMode);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(cmbEPUBToolsList);
            Controls.Add(cmbPDFToolsList);
            Controls.Add(cbModifierG);
            Controls.Add(cmbLanguage);
            Controls.Add(label6);
            Controls.Add(fileListView);
            Controls.Add(chkApplyRecursively);
            Controls.Add(chkIgnoreError);
            Controls.Add(cmbVolume);
            Controls.Add(label5);
            Controls.Add(lblStatus);
            Controls.Add(pbMetadataWrite);
            Controls.Add(tsMenu);
            Controls.Add(cbModifierI);
            Controls.Add(cbModifierX);
            Controls.Add(cmbAuthor);
            Controls.Add(cmbTitle);
            Controls.Add(cmbSeries);
            Controls.Add(cmbMatch);
            Controls.Add(bttnApply);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(bttnClear);
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "EditMetadataForm";
            ShowInTaskbar = false;
            Text = "Edit Metadata";
            Load += MetaDataForm_Load;
            Shown += MetaDataForm_Shown;
            tsMenu.ResumeLayout(false);
            tsMenu.PerformLayout();
            cmFileList.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button bttnClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bttnApply;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ComboBox cmbMatch;
        private System.Windows.Forms.ComboBox cmbSeries;
        private System.Windows.Forms.ComboBox cmbTitle;
        private System.Windows.Forms.ComboBox cmbAuthor;
        private System.Windows.Forms.ContextMenuStrip cmsBlank;
        private System.Windows.Forms.ContextMenuStrip cmRegexMatch;
        private System.Windows.Forms.CheckBox cbModifierI;
        private System.Windows.Forms.CheckBox cbModifierX;
        private System.Windows.Forms.ToolStrip tsMenu;
        private System.Windows.Forms.ToolStripDropDownButton mnuChangeCase;
        private System.Windows.Forms.ToolStripMenuItem itmChangeCaseNoChange;
        private System.Windows.Forms.ToolStripSeparator itmChangeCaseSep;
        private System.Windows.Forms.ToolStripMenuItem itmChangeCaseUppercase;
        private System.Windows.Forms.ToolStripMenuItem itmChangeCaseLowercase;
        private System.Windows.Forms.ToolStripMenuItem itmChangeCaseTitlecase;
        private System.Windows.Forms.ToolStripMenuItem itmChangeCaseCleanName;
        private System.Windows.Forms.ToolStripDropDownButton mnuNumbering;
        private System.Windows.Forms.ToolStripTextBox txtNumberingStart;
        private System.Windows.Forms.ToolStripTextBox txtNumberingPad;
        private System.Windows.Forms.ToolStripTextBox txtNumberingInc;
        private System.Windows.Forms.ToolStripTextBox txtNumberingReset;
        private System.Windows.Forms.ProgressBar pbMetadataWrite;
        private System.Windows.Forms.Label lblStatus;
        private Controls.MyComboBox cmbVolume;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ContextMenuStrip cmFileList;
        private System.Windows.Forms.ToolStripMenuItem readMetadataToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkIgnoreError;
        private System.Windows.Forms.CheckBox chkApplyRecursively;
        private ListViewEx fileListView;
        private System.Windows.Forms.ColumnHeader colFileName;
        private System.Windows.Forms.ColumnHeader colSeries;
        private System.Windows.Forms.ColumnHeader colTitle;
        private System.Windows.Forms.ColumnHeader colVolume;
        private System.Windows.Forms.ColumnHeader colAuthor;
        private Controls.MyComboBox cmbLanguage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColumnHeader colLanguage;
        private System.Windows.Forms.CheckBox cbModifierG;
        private System.Windows.Forms.ComboBox cmbPDFToolsList;
        private System.Windows.Forms.ComboBox cmbEPUBToolsList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbViewMode;
    }
}