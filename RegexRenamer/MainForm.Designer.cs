/* =============================================================================
 * RegexRenamer                                     Copyright (c) 2011 Xiperware
 * http://regexrenamer.sourceforge.net/                      xiperware@gmail.com
 * 
 * program is free software; you can redistribute it and/or modify it under
 * the terms of the GNU General Public License v2, as published by the Free
 * Software Foundation.
 * 
 * program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * =============================================================================
 */


using System.Security;

namespace RegexRenamer
{
    partial class MainForm
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
        /// the contents of method with the code editor.
        /// </summary>

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tvwFolders = new Controls.FolderTreeView();
            cmFolderView = new System.Windows.Forms.ContextMenuStrip(components);
            deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            pasteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            cutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            copyPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            explorerContextMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            setAsKavitaLibraryRootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            btnRename = new Controls.SplitButton();
            cmsRename = new System.Windows.Forms.ContextMenuStrip(components);
            itmRenameFiles = new System.Windows.Forms.ToolStripMenuItem();
            itmRenameFolders = new System.Windows.Forms.ToolStripMenuItem();
            cmbReplace = new Controls.MyComboBox();
            cmbMatch = new Controls.MyComboBox();
            miRegexMatchMatch = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchMatchSingleChar = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchMatchDigit = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchMatchAlpha = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchMatchSpace = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchMatchMultiChar = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchMatchNonDigit = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchMatchNonAlpha = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchMatchNonSpace = new System.Windows.Forms.ToolStripMenuItem();
            gbFilter = new System.Windows.Forms.GroupBox();
            cbFilterExclude = new System.Windows.Forms.CheckBox();
            txtFilter = new System.Windows.Forms.TextBox();
            rbFilterGlob = new System.Windows.Forms.RadioButton();
            rbFilterRegex = new System.Windows.Forms.RadioButton();
            pnlStats = new System.Windows.Forms.Panel();
            lblStatsHidden = new System.Windows.Forms.Label();
            lblStatsShown = new System.Windows.Forms.Label();
            lblStatsFiltered = new System.Windows.Forms.Label();
            lblStatsTotal = new System.Windows.Forms.Label();
            lblStats = new System.Windows.Forms.Label();
            lblMatch = new System.Windows.Forms.Label();
            lblReplace = new System.Windows.Forms.Label();
            cbModifierI = new System.Windows.Forms.CheckBox();
            cbModifierG = new System.Windows.Forms.CheckBox();
            cbModifierX = new System.Windows.Forms.CheckBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            btnNetwork = new System.Windows.Forms.Button();
            txtPath = new System.Windows.Forms.TextBox();
            lblNumMatched = new System.Windows.Forms.Label();
            lblNumConflict = new System.Windows.Forms.Label();
            tsMenu = new System.Windows.Forms.ToolStrip();
            mnuChangeCase = new System.Windows.Forms.ToolStripDropDownButton();
            itmChangeCaseNoChange = new System.Windows.Forms.ToolStripMenuItem();
            itmChangeCaseSep = new System.Windows.Forms.ToolStripSeparator();
            itmChangeCaseUppercase = new System.Windows.Forms.ToolStripMenuItem();
            itmChangeCaseLowercase = new System.Windows.Forms.ToolStripMenuItem();
            itmChangeCaseTitlecase = new System.Windows.Forms.ToolStripMenuItem();
            mnuNumbering = new System.Windows.Forms.ToolStripDropDownButton();
            txtNumberingStart = new System.Windows.Forms.ToolStripTextBox();
            txtNumberingPad = new System.Windows.Forms.ToolStripTextBox();
            txtNumberingInc = new System.Windows.Forms.ToolStripTextBox();
            txtNumberingReset = new System.Windows.Forms.ToolStripTextBox();
            mnuMoveCopy = new System.Windows.Forms.ToolStripDropDownButton();
            itmOutputRenameInPlace = new System.Windows.Forms.ToolStripMenuItem();
            itmOutputSep = new System.Windows.Forms.ToolStripSeparator();
            itmOutputMoveTo = new System.Windows.Forms.ToolStripMenuItem();
            itmOutputCopyTo = new System.Windows.Forms.ToolStripMenuItem();
            itmOutputBackupTo = new System.Windows.Forms.ToolStripMenuItem();
            mnuKavitaCheck = new System.Windows.Forms.ToolStripDropDownButton();
            noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            previewComicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            previewMangaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            previewBooksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ttPreviewError = new System.Windows.Forms.ToolTip(components);
            fbdNetwork = new System.Windows.Forms.FolderBrowserDialog();
            fbdMoveCopy = new System.Windows.Forms.FolderBrowserDialog();
            cmRegexMatch = new System.Windows.Forms.ContextMenuStrip(components);
            miRegexMatchAnchor = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchAnchorStart = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchAnchorEnd = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchAnchorStartEnd = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchAnchorBound = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchAnchorNonBound = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchGroup = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchGroupCapt = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchGroupNonCapt = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchGroupAlt = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuant = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantGreedy = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantZeroOneG = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantOneMoreG = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantZeroMoreG = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantExactG = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantAtLeastG = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantBetweenG = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantLazy = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantZeroOneL = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantOneMoreL = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantZeroMoreL = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantExactL = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantAtLeastL = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchQuantBetweenL = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchClass = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchClassPos = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchClassNeg = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchClassLower = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchClassUpper = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchCapt = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchCaptCreateUnnamed = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchCaptMatchUnnamed = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchCaptCreateNamed = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchCaptMatchNamed = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLook = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLookPosAhead = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLookNegAhead = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLookPosBehind = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLookNegBehind = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchSep1 = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteral = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralDot = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralQuestion = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralPlus = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralStar = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralCaret = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralDollar = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralBackslash = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralOpenRound = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralCloseRound = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralOpenSquare = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralCloseSquare = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralOpenCurly = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralCloseCurly = new System.Windows.Forms.ToolStripMenuItem();
            miRegexMatchLiteralPipe = new System.Windows.Forms.ToolStripMenuItem();
            cmRegexReplace = new System.Windows.Forms.ContextMenuStrip(components);
            miRegexReplaceCapture = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceCaptureUnnamed = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceCaptureNamed = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceOrig = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceOrigMatched = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceOrigBefore = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceOrigAfter = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceOrigAll = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceSpecial = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceSpecialNumSeq = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceSep1 = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceLiteral = new System.Windows.Forms.ToolStripMenuItem();
            miRegexReplaceLiteralDollar = new System.Windows.Forms.ToolStripMenuItem();
            cmGlobMatch = new System.Windows.Forms.ContextMenuStrip(components);
            miGlobMatchSingle = new System.Windows.Forms.ToolStripMenuItem();
            miGlobMatchMultiple = new System.Windows.Forms.ToolStripMenuItem();
            cmsBlank = new System.Windows.Forms.ContextMenuStrip(components);
            lblPath = new System.Windows.Forms.Label();
            dgvFiles = new System.Windows.Forms.DataGridView();
            colIcon = new System.Windows.Forms.DataGridViewImageColumn();
            colFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPreview = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSeries = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colChapter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEdition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSpecial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            cmFileView = new System.Windows.Forms.ContextMenuStrip(components);
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            explorerContextMenuToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tsOptions = new System.Windows.Forms.ToolStrip();
            mnuOptions = new System.Windows.Forms.ToolStripDropDownButton();
            itmOptionsShowHidden = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsPreserveExt = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsRealtimePreview = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsAllowRenSub = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsRenameSelectedRows = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsRememberWinPos = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsAddContextMenu = new System.Windows.Forms.ToolStripMenuItem();
            mnuHelp = new System.Windows.Forms.ToolStripDropDownButton();
            itmHelpContents = new System.Windows.Forms.ToolStripMenuItem();
            itmHelpRegexReference = new System.Windows.Forms.ToolStripMenuItem();
            itmHelpSep1 = new System.Windows.Forms.ToolStripSeparator();
            itmHelpEmailAuthor = new System.Windows.Forms.ToolStripMenuItem();
            itmHelpReportBug = new System.Windows.Forms.ToolStripMenuItem();
            itmHelpHomepage = new System.Windows.Forms.ToolStripMenuItem();
            itmHelpSep2 = new System.Windows.Forms.ToolStripSeparator();
            itmHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            progressBar = new System.Windows.Forms.ProgressBar();
            btnCancel = new System.Windows.Forms.Button();
            bgwRename = new System.ComponentModel.BackgroundWorker();
            groupBoxTop = new System.Windows.Forms.GroupBox();
            pnlInfo = new System.Windows.Forms.Panel();
            lblInfoFileSize = new System.Windows.Forms.Label();
            groupBoxFolderTree = new System.Windows.Forms.GroupBox();
            groupBoxFileView = new System.Windows.Forms.GroupBox();
            cmFolderView.SuspendLayout();
            cmsRename.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlStats.SuspendLayout();
            tsMenu.SuspendLayout();
            cmRegexMatch.SuspendLayout();
            cmRegexReplace.SuspendLayout();
            cmGlobMatch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFiles).BeginInit();
            cmFileView.SuspendLayout();
            tsOptions.SuspendLayout();
            groupBoxTop.SuspendLayout();
            pnlInfo.SuspendLayout();
            groupBoxFolderTree.SuspendLayout();
            groupBoxFileView.SuspendLayout();
            SuspendLayout();
            // 
            // tvwFolders
            // 
            tvwFolders.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tvwFolders.HideSelection = false;
            tvwFolders.Location = new System.Drawing.Point(11, 22);
            tvwFolders.Margin = new System.Windows.Forms.Padding(4);
            tvwFolders.Name = "tvwFolders";
            tvwFolders.Size = new System.Drawing.Size(486, 903);
            tvwFolders.TabIndex = 1;
            tvwFolders.AfterLabelEdit += tvwFolders_AfterLabelEdit;
            tvwFolders.AfterSelect += tvwFolders_AfterSelect;
            tvwFolders.NodeMouseClick += tvwFolders_NodeMouseClick;
            tvwFolders.KeyUp += tvwFolders_KeyUp;
            // 
            // cmFolderView
            // 
            cmFolderView.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmFolderView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { deleteToolStripMenuItem1, pasteToolStripMenuItem1, cutToolStripMenuItem1, copyToolStripMenuItem1, copyPathToolStripMenuItem, renameToolStripMenuItem, openInExplorerToolStripMenuItem, explorerContextMenuToolStripMenuItem, newFolderToolStripMenuItem, setAsKavitaLibraryRootToolStripMenuItem });
            cmFolderView.Name = "cmFileView";
            cmFolderView.Size = new System.Drawing.Size(250, 244);
            // 
            // deleteToolStripMenuItem1
            // 
            deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            deleteToolStripMenuItem1.Size = new System.Drawing.Size(249, 24);
            deleteToolStripMenuItem1.Text = "Delete";
            deleteToolStripMenuItem1.Click += deleteToolStripMenuItem1_Click;
            // 
            // pasteToolStripMenuItem1
            // 
            pasteToolStripMenuItem1.Name = "pasteToolStripMenuItem1";
            pasteToolStripMenuItem1.Size = new System.Drawing.Size(249, 24);
            pasteToolStripMenuItem1.Text = "Paste";
            pasteToolStripMenuItem1.Click += pasteToolStripMenuItem1_Click;
            // 
            // cutToolStripMenuItem1
            // 
            cutToolStripMenuItem1.Name = "cutToolStripMenuItem1";
            cutToolStripMenuItem1.Size = new System.Drawing.Size(249, 24);
            cutToolStripMenuItem1.Text = "Cut";
            cutToolStripMenuItem1.Click += cutToolStripMenuItem1_Click;
            // 
            // copyToolStripMenuItem1
            // 
            copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            copyToolStripMenuItem1.Size = new System.Drawing.Size(249, 24);
            copyToolStripMenuItem1.Text = "Copy";
            copyToolStripMenuItem1.Click += copyToolStripMenuItem1_Click;
            // 
            // copyPathToolStripMenuItem
            // 
            copyPathToolStripMenuItem.Name = "copyPathToolStripMenuItem";
            copyPathToolStripMenuItem.Size = new System.Drawing.Size(249, 24);
            copyPathToolStripMenuItem.Text = "Copy Path";
            copyPathToolStripMenuItem.Click += copyPathToolStripMenuItem_Click;
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.Size = new System.Drawing.Size(249, 24);
            renameToolStripMenuItem.Text = "Rename";
            renameToolStripMenuItem.Click += renameToolStripMenuItem_Click;
            // 
            // openInExplorerToolStripMenuItem
            // 
            openInExplorerToolStripMenuItem.Name = "openInExplorerToolStripMenuItem";
            openInExplorerToolStripMenuItem.Size = new System.Drawing.Size(249, 24);
            openInExplorerToolStripMenuItem.Text = "Open In Explorer";
            openInExplorerToolStripMenuItem.Click += openInExplorerToolStripMenuItem_Click;
            // 
            // explorerContextMenuToolStripMenuItem
            // 
            explorerContextMenuToolStripMenuItem.Name = "explorerContextMenuToolStripMenuItem";
            explorerContextMenuToolStripMenuItem.Size = new System.Drawing.Size(249, 24);
            explorerContextMenuToolStripMenuItem.Text = "Explorer Context Menu";
            explorerContextMenuToolStripMenuItem.Click += explorerContextMenuToolStripMenuItem_Click;
            // 
            // newFolderToolStripMenuItem
            // 
            newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            newFolderToolStripMenuItem.Size = new System.Drawing.Size(249, 24);
            newFolderToolStripMenuItem.Text = "New Folder";
            newFolderToolStripMenuItem.Click += newFolderToolStripMenuItem_Click;
            // 
            // setAsKavitaLibraryRootToolStripMenuItem
            // 
            setAsKavitaLibraryRootToolStripMenuItem.Name = "setAsKavitaLibraryRootToolStripMenuItem";
            setAsKavitaLibraryRootToolStripMenuItem.Size = new System.Drawing.Size(249, 24);
            setAsKavitaLibraryRootToolStripMenuItem.Text = "Set As Kavita Library Root";
            setAsKavitaLibraryRootToolStripMenuItem.Click += setAsKavitaLibraryRootToolStripMenuItem_Click;
            // 
            // btnRename
            // 
            btnRename.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnRename.AutoSize = true;
            btnRename.ContextMenuStrip = cmsRename;
            btnRename.Location = new System.Drawing.Point(1076, 940);
            btnRename.Margin = new System.Windows.Forms.Padding(4);
            btnRename.Name = "btnRename";
            btnRename.Size = new System.Drawing.Size(113, 32);
            btnRename.State = System.Windows.Forms.VisualStyles.PushButtonState.Normal;
            btnRename.TabIndex = 3;
            btnRename.Text = "&Rename";
            btnRename.UseVisualStyleBackColor = true;
            btnRename.Click += btnRename_Click;
            // 
            // cmsRename
            // 
            cmsRename.AutoSize = false;
            cmsRename.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmsRename.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { itmRenameFiles, itmRenameFolders });
            cmsRename.Name = "cmsRename";
            cmsRename.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            cmsRename.Size = new System.Drawing.Size(130, 48);
            cmsRename.Opening += cmsRename_Opening;
            // 
            // itmRenameFiles
            // 
            itmRenameFiles.AutoSize = false;
            itmRenameFiles.Checked = true;
            itmRenameFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            itmRenameFiles.Name = "itmRenameFiles";
            itmRenameFiles.Size = new System.Drawing.Size(129, 22);
            itmRenameFiles.Text = "Rename files";
            itmRenameFiles.Click += itmRenameFiles_Click;
            // 
            // itmRenameFolders
            // 
            itmRenameFolders.AutoSize = false;
            itmRenameFolders.Name = "itmRenameFolders";
            itmRenameFolders.Size = new System.Drawing.Size(129, 22);
            itmRenameFolders.Text = "Rename folders";
            itmRenameFolders.Click += itmRenameFolders_Click;
            // 
            // cmbReplace
            // 
            cmbReplace.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbReplace.AutoCompleteCustomSource.AddRange(new string[] { "$1" });
            cmbReplace.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbReplace.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            cmbReplace.Location = new System.Drawing.Point(82, 74);
            cmbReplace.Margin = new System.Windows.Forms.Padding(4);
            cmbReplace.Name = "cmbReplace";
            cmbReplace.Size = new System.Drawing.Size(904, 25);
            cmbReplace.TabIndex = 2;
            toolTip.SetToolTip(cmbReplace, "Use $1, $2, ... to insert captured text");
            cmbReplace.TextChanged += cmbReplace_TextChanged;
            cmbReplace.KeyDown += cmbReplace_KeyDown;
            cmbReplace.Leave += cmbReplace_Leave;
            cmbReplace.MouseDown += cmbReplace_MouseDown;
            cmbReplace.MouseUp += cmbReplace_MouseUp;
            // 
            // cmbMatch
            // 
            cmbMatch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbMatch.AutoCompleteCustomSource.AddRange(new string[] { "(.+)", "(.+)(/d+)" });
            cmbMatch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbMatch.Font = new System.Drawing.Font("Courier New", 8.25F);
            cmbMatch.Location = new System.Drawing.Point(82, 34);
            cmbMatch.Margin = new System.Windows.Forms.Padding(4);
            cmbMatch.Name = "cmbMatch";
            cmbMatch.Size = new System.Drawing.Size(904, 25);
            cmbMatch.TabIndex = 1;
            toolTip.SetToolTip(cmbMatch, "Shift+rightclick for a menu of regex elements");
            cmbMatch.SelectedIndexChanged += cmbMatch_SelectedIndexChanged;
            cmbMatch.TextChanged += cmbMatch_TextChanged;
            cmbMatch.Enter += cmbMatch_Enter;
            cmbMatch.KeyDown += cmbMatch_KeyDown;
            cmbMatch.Leave += cmbMatch_Leave;
            cmbMatch.MouseDown += cmbMatch_MouseDown;
            cmbMatch.MouseUp += cmbMatch_MouseUp;
            // 
            // miRegexMatchMatch
            // 
            miRegexMatchMatch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchMatchSingleChar, miRegexMatchMatchDigit, miRegexMatchMatchAlpha, miRegexMatchMatchSpace, miRegexMatchMatchMultiChar, miRegexMatchMatchNonDigit, miRegexMatchMatchNonAlpha, miRegexMatchMatchNonSpace });
            miRegexMatchMatch.Name = "miRegexMatchMatch";
            miRegexMatchMatch.Size = new System.Drawing.Size(176, 24);
            miRegexMatchMatch.Text = "Match";
            // 
            // miRegexMatchMatchSingleChar
            // 
            miRegexMatchMatchSingleChar.Name = "miRegexMatchMatchSingleChar";
            miRegexMatchMatchSingleChar.Size = new System.Drawing.Size(236, 26);
            miRegexMatchMatchSingleChar.Text = "Single character\t.";
            miRegexMatchMatchSingleChar.Click += InsertRegexFragment;
            // 
            // miRegexMatchMatchDigit
            // 
            miRegexMatchMatchDigit.Name = "miRegexMatchMatchDigit";
            miRegexMatchMatchDigit.Size = new System.Drawing.Size(236, 26);
            miRegexMatchMatchDigit.Text = "Digit\t\\d";
            miRegexMatchMatchDigit.Click += InsertRegexFragment;
            // 
            // miRegexMatchMatchAlpha
            // 
            miRegexMatchMatchAlpha.Name = "miRegexMatchMatchAlpha";
            miRegexMatchMatchAlpha.Size = new System.Drawing.Size(236, 26);
            miRegexMatchMatchAlpha.Text = "Alphanumeric\t\\w";
            miRegexMatchMatchAlpha.Click += InsertRegexFragment;
            // 
            // miRegexMatchMatchSpace
            // 
            miRegexMatchMatchSpace.Name = "miRegexMatchMatchSpace";
            miRegexMatchMatchSpace.Size = new System.Drawing.Size(236, 26);
            miRegexMatchMatchSpace.Text = "Space\t\\s";
            miRegexMatchMatchSpace.Click += InsertRegexFragment;
            // 
            // miRegexMatchMatchMultiChar
            // 
            miRegexMatchMatchMultiChar.Name = "miRegexMatchMatchMultiChar";
            miRegexMatchMatchMultiChar.Size = new System.Drawing.Size(236, 26);
            miRegexMatchMatchMultiChar.Text = "Multiple characters\t.*";
            miRegexMatchMatchMultiChar.Click += InsertRegexFragment;
            // 
            // miRegexMatchMatchNonDigit
            // 
            miRegexMatchMatchNonDigit.Name = "miRegexMatchMatchNonDigit";
            miRegexMatchMatchNonDigit.Size = new System.Drawing.Size(236, 26);
            miRegexMatchMatchNonDigit.Text = "Non-digit\t\\D";
            miRegexMatchMatchNonDigit.Click += InsertRegexFragment;
            // 
            // miRegexMatchMatchNonAlpha
            // 
            miRegexMatchMatchNonAlpha.Name = "miRegexMatchMatchNonAlpha";
            miRegexMatchMatchNonAlpha.Size = new System.Drawing.Size(236, 26);
            miRegexMatchMatchNonAlpha.Text = "Non-alphanumeric\t\\W";
            miRegexMatchMatchNonAlpha.Click += InsertRegexFragment;
            // 
            // miRegexMatchMatchNonSpace
            // 
            miRegexMatchMatchNonSpace.Name = "miRegexMatchMatchNonSpace";
            miRegexMatchMatchNonSpace.Size = new System.Drawing.Size(236, 26);
            miRegexMatchMatchNonSpace.Text = "Non-space\t\\S";
            miRegexMatchMatchNonSpace.Click += InsertRegexFragment;
            // 
            // gbFilter
            // 
            gbFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            gbFilter.Controls.Add(cbFilterExclude);
            gbFilter.Controls.Add(txtFilter);
            gbFilter.Controls.Add(rbFilterGlob);
            gbFilter.Controls.Add(rbFilterRegex);
            gbFilter.Location = new System.Drawing.Point(1491, 20);
            gbFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            gbFilter.Size = new System.Drawing.Size(201, 100);
            gbFilter.TabIndex = 2;
            gbFilter.TabStop = false;
            gbFilter.Text = "Filter";
            // 
            // cbFilterExclude
            // 
            cbFilterExclude.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbFilterExclude.Appearance = System.Windows.Forms.Appearance.Button;
            cbFilterExclude.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            cbFilterExclude.Image = Properties.Resources.x;
            cbFilterExclude.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cbFilterExclude.Location = new System.Drawing.Point(92, 39);
            cbFilterExclude.Margin = new System.Windows.Forms.Padding(0);
            cbFilterExclude.Name = "cbFilterExclude";
            cbFilterExclude.Size = new System.Drawing.Size(16, 31);
            cbFilterExclude.TabIndex = 2;
            toolTip.SetToolTip(cbFilterExclude, "Exclude (invert filter)");
            cbFilterExclude.UseVisualStyleBackColor = true;
            cbFilterExclude.CheckedChanged += cbFilterExclude_CheckedChanged;
            // 
            // txtFilter
            // 
            txtFilter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            txtFilter.Font = new System.Drawing.Font("Courier New", 8.25F);
            txtFilter.Location = new System.Drawing.Point(9, 44);
            txtFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtFilter.Name = "txtFilter";
            txtFilter.Size = new System.Drawing.Size(79, 23);
            txtFilter.TabIndex = 1;
            txtFilter.Text = "*.*";
            toolTip.SetToolTip(txtFilter, "Press ENTER to apply filter");
            txtFilter.TextChanged += txtFilter_TextChanged;
            txtFilter.KeyDown += txtFilter_KeyDown;
            txtFilter.Leave += txtFilter_Leave;
            txtFilter.MouseDown += txtFilter_MouseDown;
            txtFilter.MouseUp += txtFilter_MouseUp;
            // 
            // rbFilterGlob
            // 
            rbFilterGlob.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            rbFilterGlob.AutoSize = true;
            rbFilterGlob.Checked = true;
            rbFilterGlob.Location = new System.Drawing.Point(124, 18);
            rbFilterGlob.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            rbFilterGlob.Name = "rbFilterGlob";
            rbFilterGlob.Size = new System.Drawing.Size(62, 24);
            rbFilterGlob.TabIndex = 3;
            rbFilterGlob.TabStop = true;
            rbFilterGlob.Text = "Glob";
            rbFilterGlob.UseVisualStyleBackColor = true;
            rbFilterGlob.Click += rbFilterGlob_Click;
            // 
            // rbFilterRegex
            // 
            rbFilterRegex.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            rbFilterRegex.AutoSize = true;
            rbFilterRegex.Location = new System.Drawing.Point(124, 46);
            rbFilterRegex.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            rbFilterRegex.Name = "rbFilterRegex";
            rbFilterRegex.Size = new System.Drawing.Size(71, 24);
            rbFilterRegex.TabIndex = 3;
            rbFilterRegex.Text = "Regex";
            rbFilterRegex.UseVisualStyleBackColor = true;
            rbFilterRegex.CheckedChanged += rbFilterRegex_CheckedChanged;
            rbFilterRegex.Click += rbFilterRegex_Click;
            // 
            // pnlStats
            // 
            pnlStats.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            pnlStats.Controls.Add(lblStatsHidden);
            pnlStats.Controls.Add(lblStatsShown);
            pnlStats.Controls.Add(lblStatsFiltered);
            pnlStats.Controls.Add(lblStatsTotal);
            pnlStats.Location = new System.Drawing.Point(1045, 20);
            pnlStats.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pnlStats.Name = "pnlStats";
            pnlStats.Size = new System.Drawing.Size(123, 98);
            pnlStats.TabIndex = 6;
            // 
            // lblStatsHidden
            // 
            lblStatsHidden.AutoEllipsis = true;
            lblStatsHidden.Location = new System.Drawing.Point(5, 50);
            lblStatsHidden.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStatsHidden.Name = "lblStatsHidden";
            lblStatsHidden.Size = new System.Drawing.Size(113, 19);
            lblStatsHidden.TabIndex = 4;
            lblStatsHidden.Text = "0 hidden";
            // 
            // lblStatsShown
            // 
            lblStatsShown.AutoEllipsis = true;
            lblStatsShown.Location = new System.Drawing.Point(5, 27);
            lblStatsShown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStatsShown.Name = "lblStatsShown";
            lblStatsShown.Size = new System.Drawing.Size(113, 19);
            lblStatsShown.TabIndex = 3;
            lblStatsShown.Text = "0 shown";
            // 
            // lblStatsFiltered
            // 
            lblStatsFiltered.AutoEllipsis = true;
            lblStatsFiltered.Location = new System.Drawing.Point(5, 73);
            lblStatsFiltered.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStatsFiltered.Name = "lblStatsFiltered";
            lblStatsFiltered.Size = new System.Drawing.Size(113, 19);
            lblStatsFiltered.TabIndex = 2;
            lblStatsFiltered.Text = "0 filtered";
            // 
            // lblStatsTotal
            // 
            lblStatsTotal.AutoEllipsis = true;
            lblStatsTotal.Location = new System.Drawing.Point(6, 5);
            lblStatsTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStatsTotal.Name = "lblStatsTotal";
            lblStatsTotal.Size = new System.Drawing.Size(113, 19);
            lblStatsTotal.TabIndex = 1;
            lblStatsTotal.Text = "0 total";
            // 
            // lblStats
            // 
            lblStats.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblStats.ForeColor = System.Drawing.SystemColors.ControlDark;
            lblStats.Location = new System.Drawing.Point(1626, 22);
            lblStats.Margin = new System.Windows.Forms.Padding(0);
            lblStats.Name = "lblStats";
            lblStats.Size = new System.Drawing.Size(41, 20);
            lblStats.TabIndex = 4;
            lblStats.Text = "Stats";
            lblStats.MouseEnter += lblStats_MouseEnter;
            lblStats.MouseLeave += lblStats_MouseLeave;
            // 
            // lblMatch
            // 
            lblMatch.AutoSize = true;
            lblMatch.Location = new System.Drawing.Point(25, 34);
            lblMatch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMatch.Name = "lblMatch";
            lblMatch.Size = new System.Drawing.Size(53, 20);
            lblMatch.TabIndex = 0;
            lblMatch.Text = "Match:";
            // 
            // lblReplace
            // 
            lblReplace.AutoSize = true;
            lblReplace.Location = new System.Drawing.Point(9, 75);
            lblReplace.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblReplace.Name = "lblReplace";
            lblReplace.Size = new System.Drawing.Size(65, 20);
            lblReplace.TabIndex = 0;
            lblReplace.Text = "Replace:";
            // 
            // cbModifierI
            // 
            cbModifierI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbModifierI.AutoSize = true;
            cbModifierI.Location = new System.Drawing.Point(993, 28);
            cbModifierI.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cbModifierI.Name = "cbModifierI";
            cbModifierI.Size = new System.Drawing.Size(41, 24);
            cbModifierI.TabIndex = 3;
            cbModifierI.Tag = false;
            cbModifierI.Text = "/i";
            toolTip.SetToolTip(cbModifierI, "Ignore case");
            cbModifierI.UseVisualStyleBackColor = true;
            cbModifierI.CheckedChanged += cbModifierI_CheckedChanged;
            // 
            // cbModifierG
            // 
            cbModifierG.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbModifierG.AutoSize = true;
            cbModifierG.Location = new System.Drawing.Point(993, 55);
            cbModifierG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cbModifierG.Name = "cbModifierG";
            cbModifierG.Size = new System.Drawing.Size(46, 24);
            cbModifierG.TabIndex = 4;
            cbModifierG.Tag = false;
            cbModifierG.Text = "/g";
            toolTip.SetToolTip(cbModifierG, "Global (match as many times as possible)");
            cbModifierG.UseVisualStyleBackColor = true;
            cbModifierG.CheckedChanged += cbModifierG_CheckedChanged;
            // 
            // cbModifierX
            // 
            cbModifierX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbModifierX.AutoSize = true;
            cbModifierX.Location = new System.Drawing.Point(993, 81);
            cbModifierX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cbModifierX.Name = "cbModifierX";
            cbModifierX.Size = new System.Drawing.Size(44, 24);
            cbModifierX.TabIndex = 5;
            cbModifierX.Tag = false;
            cbModifierX.Text = "/x";
            toolTip.SetToolTip(cbModifierX, "Extended regex (ignore unescaped spaces)");
            cbModifierX.UseVisualStyleBackColor = true;
            cbModifierX.CheckedChanged += cbModifierX_CheckedChanged;
            // 
            // btnNetwork
            // 
            btnNetwork.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnNetwork.Image = (System.Drawing.Image)resources.GetObject("btnNetwork.Image");
            btnNetwork.Location = new System.Drawing.Point(441, 934);
            btnNetwork.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnNetwork.Name = "btnNetwork";
            btnNetwork.Size = new System.Drawing.Size(48, 38);
            btnNetwork.TabIndex = 3;
            toolTip.SetToolTip(btnNetwork, "Browse network");
            btnNetwork.UseVisualStyleBackColor = true;
            btnNetwork.Click += btnNetwork_Click;
            // 
            // txtPath
            // 
            txtPath.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtPath.Location = new System.Drawing.Point(50, 941);
            txtPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtPath.Name = "txtPath";
            txtPath.Size = new System.Drawing.Size(383, 27);
            txtPath.TabIndex = 2;
            toolTip.SetToolTip(txtPath, "Press ENTER to apply path");
            txtPath.Enter += txtPath_Enter;
            txtPath.KeyDown += txtPath_KeyDown;
            txtPath.Leave += txtPath_Leave;
            // 
            // lblNumMatched
            // 
            lblNumMatched.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblNumMatched.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lblNumMatched.ForeColor = System.Drawing.Color.Blue;
            lblNumMatched.Location = new System.Drawing.Point(967, 943);
            lblNumMatched.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblNumMatched.Name = "lblNumMatched";
            lblNumMatched.Size = new System.Drawing.Size(47, 25);
            lblNumMatched.TabIndex = 4;
            lblNumMatched.Text = "0";
            lblNumMatched.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            toolTip.SetToolTip(lblNumMatched, "Number of matches");
            // 
            // lblNumConflict
            // 
            lblNumConflict.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblNumConflict.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lblNumConflict.ForeColor = System.Drawing.Color.Red;
            lblNumConflict.Location = new System.Drawing.Point(1022, 943);
            lblNumConflict.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblNumConflict.Name = "lblNumConflict";
            lblNumConflict.Size = new System.Drawing.Size(47, 25);
            lblNumConflict.TabIndex = 5;
            lblNumConflict.Text = "0";
            lblNumConflict.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            toolTip.SetToolTip(lblNumConflict, "Number of conflicts");
            // 
            // tsMenu
            // 
            tsMenu.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tsMenu.AutoSize = false;
            tsMenu.BackColor = System.Drawing.SystemColors.ButtonFace;
            tsMenu.CanOverflow = false;
            tsMenu.Dock = System.Windows.Forms.DockStyle.None;
            tsMenu.Font = new System.Drawing.Font("Tahoma", 8.25F);
            tsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            tsMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuChangeCase, mnuNumbering, mnuMoveCopy, mnuKavitaCheck });
            tsMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            tsMenu.Location = new System.Drawing.Point(1375, 20);
            tsMenu.Name = "tsMenu";
            tsMenu.Size = new System.Drawing.Size(112, 103);
            tsMenu.TabIndex = 1;
            tsMenu.TabStop = true;
            // 
            // mnuChangeCase
            // 
            mnuChangeCase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuChangeCase.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { itmChangeCaseNoChange, itmChangeCaseSep, itmChangeCaseUppercase, itmChangeCaseLowercase, itmChangeCaseTitlecase });
            mnuChangeCase.Font = new System.Drawing.Font("Tahoma", 8.25F);
            mnuChangeCase.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            mnuChangeCase.Name = "mnuChangeCase";
            mnuChangeCase.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            mnuChangeCase.Size = new System.Drawing.Size(110, 21);
            mnuChangeCase.Text = "Change Case";
            mnuChangeCase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            mnuChangeCase.ToolTipText = "Only the matched portion of the filename will have its case changed";
            mnuChangeCase.MouseDown += mnuChangeCase_MouseDown;
            // 
            // itmChangeCaseNoChange
            // 
            itmChangeCaseNoChange.Checked = true;
            itmChangeCaseNoChange.CheckState = System.Windows.Forms.CheckState.Checked;
            itmChangeCaseNoChange.Font = new System.Drawing.Font("Tahoma", 8.25F);
            itmChangeCaseNoChange.Name = "itmChangeCaseNoChange";
            itmChangeCaseNoChange.Size = new System.Drawing.Size(156, 26);
            itmChangeCaseNoChange.Text = "No change";
            itmChangeCaseNoChange.Click += itmChangeCaseNoChange_Click;
            // 
            // itmChangeCaseSep
            // 
            itmChangeCaseSep.Name = "itmChangeCaseSep";
            itmChangeCaseSep.Size = new System.Drawing.Size(153, 6);
            // 
            // itmChangeCaseUppercase
            // 
            itmChangeCaseUppercase.Font = new System.Drawing.Font("Tahoma", 8.25F);
            itmChangeCaseUppercase.Name = "itmChangeCaseUppercase";
            itmChangeCaseUppercase.Size = new System.Drawing.Size(156, 26);
            itmChangeCaseUppercase.Text = "Uppercase";
            itmChangeCaseUppercase.Click += itmChangeCaseUppercase_Click;
            // 
            // itmChangeCaseLowercase
            // 
            itmChangeCaseLowercase.Font = new System.Drawing.Font("Tahoma", 8.25F);
            itmChangeCaseLowercase.Name = "itmChangeCaseLowercase";
            itmChangeCaseLowercase.Size = new System.Drawing.Size(156, 26);
            itmChangeCaseLowercase.Text = "Lowercase";
            itmChangeCaseLowercase.Click += itmChangeCaseLowercase_Click;
            // 
            // itmChangeCaseTitlecase
            // 
            itmChangeCaseTitlecase.Font = new System.Drawing.Font("Tahoma", 8.25F);
            itmChangeCaseTitlecase.Name = "itmChangeCaseTitlecase";
            itmChangeCaseTitlecase.Size = new System.Drawing.Size(156, 26);
            itmChangeCaseTitlecase.Text = "Title case";
            itmChangeCaseTitlecase.Click += itmChangeCaseTitlecase_Click;
            // 
            // mnuNumbering
            // 
            mnuNumbering.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuNumbering.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { txtNumberingStart, txtNumberingPad, txtNumberingInc, txtNumberingReset });
            mnuNumbering.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            mnuNumbering.Name = "mnuNumbering";
            mnuNumbering.Padding = new System.Windows.Forms.Padding(0, 0, 21, 0);
            mnuNumbering.Size = new System.Drawing.Size(110, 21);
            mnuNumbering.Tag = "mnuNumbering";
            mnuNumbering.Text = "Numbering";
            mnuNumbering.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            mnuNumbering.ToolTipText = "Enter \"$#\" in the replace field to insert a number sequence";
            mnuNumbering.MouseDown += mnuNumbering_MouseDown;
            // 
            // txtNumberingStart
            // 
            txtNumberingStart.MaxLength = 10;
            txtNumberingStart.Name = "txtNumberingStart";
            txtNumberingStart.Size = new System.Drawing.Size(75, 27);
            txtNumberingStart.Tag = true;
            txtNumberingStart.Text = "1";
            txtNumberingStart.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            txtNumberingStart.ToolTipText = "Starting number (or letter)";
            txtNumberingStart.TextChanged += txtNumberingStart_TextChanged;
            // 
            // txtNumberingPad
            // 
            txtNumberingPad.MaxLength = 10;
            txtNumberingPad.Name = "txtNumberingPad";
            txtNumberingPad.Size = new System.Drawing.Size(75, 27);
            txtNumberingPad.Tag = true;
            txtNumberingPad.Text = "000";
            txtNumberingPad.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            txtNumberingPad.ToolTipText = "Eg: \"0000\" means 14 => 0014";
            txtNumberingPad.TextChanged += txtNumberingPad_TextChanged;
            // 
            // txtNumberingInc
            // 
            txtNumberingInc.MaxLength = 10;
            txtNumberingInc.Name = "txtNumberingInc";
            txtNumberingInc.Size = new System.Drawing.Size(75, 27);
            txtNumberingInc.Tag = true;
            txtNumberingInc.Text = "1";
            txtNumberingInc.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            txtNumberingInc.ToolTipText = "Increment by x each file (may be negative)";
            txtNumberingInc.TextChanged += txtNumberingInc_TextChanged;
            // 
            // txtNumberingReset
            // 
            txtNumberingReset.MaxLength = 10;
            txtNumberingReset.Name = "txtNumberingReset";
            txtNumberingReset.Size = new System.Drawing.Size(75, 27);
            txtNumberingReset.Tag = true;
            txtNumberingReset.Text = "0";
            txtNumberingReset.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            txtNumberingReset.ToolTipText = "Reset to starting number every x files";
            txtNumberingReset.TextChanged += txtNumberingReset_TextChanged;
            // 
            // mnuMoveCopy
            // 
            mnuMoveCopy.AutoToolTip = false;
            mnuMoveCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuMoveCopy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { itmOutputRenameInPlace, itmOutputSep, itmOutputMoveTo, itmOutputCopyTo, itmOutputBackupTo });
            mnuMoveCopy.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            mnuMoveCopy.Name = "mnuMoveCopy";
            mnuMoveCopy.Padding = new System.Windows.Forms.Padding(0, 0, 17, 0);
            mnuMoveCopy.Size = new System.Drawing.Size(110, 21);
            mnuMoveCopy.Text = "Move/Copy";
            mnuMoveCopy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            mnuMoveCopy.MouseDown += mnuMoveCopy_MouseDown;
            // 
            // itmOutputRenameInPlace
            // 
            itmOutputRenameInPlace.Checked = true;
            itmOutputRenameInPlace.CheckState = System.Windows.Forms.CheckState.Checked;
            itmOutputRenameInPlace.Font = new System.Drawing.Font("Tahoma", 8.25F);
            itmOutputRenameInPlace.Name = "itmOutputRenameInPlace";
            itmOutputRenameInPlace.Size = new System.Drawing.Size(189, 26);
            itmOutputRenameInPlace.Text = "Rename in place";
            itmOutputRenameInPlace.Click += itmOutputRenameInPlace_Click;
            // 
            // itmOutputSep
            // 
            itmOutputSep.Name = "itmOutputSep";
            itmOutputSep.Size = new System.Drawing.Size(186, 6);
            // 
            // itmOutputMoveTo
            // 
            itmOutputMoveTo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            itmOutputMoveTo.Name = "itmOutputMoveTo";
            itmOutputMoveTo.Size = new System.Drawing.Size(189, 26);
            itmOutputMoveTo.Text = "Move to...";
            itmOutputMoveTo.ToolTipText = "Files that match are moved and renamed";
            itmOutputMoveTo.Click += itmOutputMoveTo_Click;
            // 
            // itmOutputCopyTo
            // 
            itmOutputCopyTo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            itmOutputCopyTo.Name = "itmOutputCopyTo";
            itmOutputCopyTo.Size = new System.Drawing.Size(189, 26);
            itmOutputCopyTo.Text = "Copy to...";
            itmOutputCopyTo.ToolTipText = "Files that match are copied and the copies are renamed";
            itmOutputCopyTo.Click += itmOutputCopyTo_Click;
            // 
            // itmOutputBackupTo
            // 
            itmOutputBackupTo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            itmOutputBackupTo.Name = "itmOutputBackupTo";
            itmOutputBackupTo.Size = new System.Drawing.Size(189, 26);
            itmOutputBackupTo.Text = "Backup to...";
            itmOutputBackupTo.ToolTipText = "Files that match are copied and the originals are renamed";
            itmOutputBackupTo.Click += itmOutputBackupTo_Click;
            // 
            // mnuKavitaCheck
            // 
            mnuKavitaCheck.AutoToolTip = false;
            mnuKavitaCheck.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuKavitaCheck.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { noneToolStripMenuItem, toolStripSeparator1, previewComicsToolStripMenuItem, previewMangaToolStripMenuItem, previewBooksToolStripMenuItem });
            mnuKavitaCheck.ImageTransparentColor = System.Drawing.Color.Magenta;
            mnuKavitaCheck.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            mnuKavitaCheck.Name = "mnuKavitaCheck";
            mnuKavitaCheck.Padding = new System.Windows.Forms.Padding(0, 0, 43, 0);
            mnuKavitaCheck.Size = new System.Drawing.Size(102, 21);
            mnuKavitaCheck.Text = "Kavita";
            mnuKavitaCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            mnuKavitaCheck.ToolTipText = "Preview Kavitha Parsed Values";
            // 
            // noneToolStripMenuItem
            // 
            noneToolStripMenuItem.Checked = true;
            noneToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            noneToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            noneToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            noneToolStripMenuItem.Text = "None";
            noneToolStripMenuItem.Click += this.noneToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(221, 6);
            // 
            // previewComicsToolStripMenuItem
            // 
            previewComicsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            previewComicsToolStripMenuItem.Name = "previewComicsToolStripMenuItem";
            previewComicsToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            previewComicsToolStripMenuItem.Text = "Preview Comics";
            previewComicsToolStripMenuItem.Click += this.previewComicsToolStripMenuItem_Click;
            // 
            // previewMangaToolStripMenuItem
            // 
            previewMangaToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            previewMangaToolStripMenuItem.Name = "previewMangaToolStripMenuItem";
            previewMangaToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            previewMangaToolStripMenuItem.Text = "Preview Manga";
            previewMangaToolStripMenuItem.Click += this.previewMangaToolStripMenuItem_Click;
            // 
            // previewBooksToolStripMenuItem
            // 
            previewBooksToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            previewBooksToolStripMenuItem.Name = "previewBooksToolStripMenuItem";
            previewBooksToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            previewBooksToolStripMenuItem.Text = "Preview Books";
            previewBooksToolStripMenuItem.Click += this.previewBooksToolStripMenuItem_Click;
            // 
            // ttPreviewError
            // 
            ttPreviewError.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            ttPreviewError.ToolTipTitle = "Preview validation error";
            // 
            // fbdNetwork
            // 
            fbdNetwork.Description = "\r\n Select a network share or subfolder:";
            fbdNetwork.ShowNewFolderButton = false;
            // 
            // cmRegexMatch
            // 
            cmRegexMatch.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmRegexMatch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchMatch, miRegexMatchAnchor, miRegexMatchGroup, miRegexMatchQuant, miRegexMatchClass, miRegexMatchCapt, miRegexMatchLook, miRegexMatchSep1, miRegexMatchLiteral });
            cmRegexMatch.Name = "cmRegexMatch";
            cmRegexMatch.Size = new System.Drawing.Size(177, 220);
            // 
            // miRegexMatchAnchor
            // 
            miRegexMatchAnchor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchAnchorStart, miRegexMatchAnchorEnd, miRegexMatchAnchorStartEnd, miRegexMatchAnchorBound, miRegexMatchAnchorNonBound });
            miRegexMatchAnchor.Name = "miRegexMatchAnchor";
            miRegexMatchAnchor.Size = new System.Drawing.Size(176, 24);
            miRegexMatchAnchor.Text = "Anchor";
            // 
            // miRegexMatchAnchorStart
            // 
            miRegexMatchAnchorStart.Name = "miRegexMatchAnchorStart";
            miRegexMatchAnchorStart.Size = new System.Drawing.Size(242, 26);
            miRegexMatchAnchorStart.Text = "Start\t^";
            miRegexMatchAnchorStart.Click += InsertRegexFragment;
            // 
            // miRegexMatchAnchorEnd
            // 
            miRegexMatchAnchorEnd.Name = "miRegexMatchAnchorEnd";
            miRegexMatchAnchorEnd.Size = new System.Drawing.Size(242, 26);
            miRegexMatchAnchorEnd.Text = "End\t$";
            miRegexMatchAnchorEnd.Click += InsertRegexFragment;
            // 
            // miRegexMatchAnchorStartEnd
            // 
            miRegexMatchAnchorStartEnd.Name = "miRegexMatchAnchorStartEnd";
            miRegexMatchAnchorStartEnd.Size = new System.Drawing.Size(242, 26);
            miRegexMatchAnchorStartEnd.Text = "Start and End\t^(...)$";
            miRegexMatchAnchorStartEnd.Click += InsertRegexFragment;
            // 
            // miRegexMatchAnchorBound
            // 
            miRegexMatchAnchorBound.Name = "miRegexMatchAnchorBound";
            miRegexMatchAnchorBound.Size = new System.Drawing.Size(242, 26);
            miRegexMatchAnchorBound.Text = "Word boundary\t\\b";
            miRegexMatchAnchorBound.Click += InsertRegexFragment;
            // 
            // miRegexMatchAnchorNonBound
            // 
            miRegexMatchAnchorNonBound.Name = "miRegexMatchAnchorNonBound";
            miRegexMatchAnchorNonBound.Size = new System.Drawing.Size(242, 26);
            miRegexMatchAnchorNonBound.Text = "Non-word boundary\t\\B";
            miRegexMatchAnchorNonBound.Click += InsertRegexFragment;
            // 
            // miRegexMatchGroup
            // 
            miRegexMatchGroup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchGroupCapt, miRegexMatchGroupNonCapt, miRegexMatchGroupAlt });
            miRegexMatchGroup.Name = "miRegexMatchGroup";
            miRegexMatchGroup.Size = new System.Drawing.Size(176, 24);
            miRegexMatchGroup.Text = "Group";
            // 
            // miRegexMatchGroupCapt
            // 
            miRegexMatchGroupCapt.Name = "miRegexMatchGroupCapt";
            miRegexMatchGroupCapt.Size = new System.Drawing.Size(228, 26);
            miRegexMatchGroupCapt.Text = "With capture\t(...)";
            miRegexMatchGroupCapt.Click += InsertRegexFragment;
            // 
            // miRegexMatchGroupNonCapt
            // 
            miRegexMatchGroupNonCapt.Name = "miRegexMatchGroupNonCapt";
            miRegexMatchGroupNonCapt.Size = new System.Drawing.Size(228, 26);
            miRegexMatchGroupNonCapt.Text = "Without capture\t(?:...)";
            miRegexMatchGroupNonCapt.Click += InsertRegexFragment;
            // 
            // miRegexMatchGroupAlt
            // 
            miRegexMatchGroupAlt.Name = "miRegexMatchGroupAlt";
            miRegexMatchGroupAlt.Size = new System.Drawing.Size(228, 26);
            miRegexMatchGroupAlt.Text = "Alternative\t(...|...)";
            miRegexMatchGroupAlt.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuant
            // 
            miRegexMatchQuant.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchQuantGreedy, miRegexMatchQuantZeroOneG, miRegexMatchQuantOneMoreG, miRegexMatchQuantZeroMoreG, miRegexMatchQuantExactG, miRegexMatchQuantAtLeastG, miRegexMatchQuantBetweenG, miRegexMatchQuantLazy, miRegexMatchQuantZeroOneL, miRegexMatchQuantOneMoreL, miRegexMatchQuantZeroMoreL, miRegexMatchQuantExactL, miRegexMatchQuantAtLeastL, miRegexMatchQuantBetweenL });
            miRegexMatchQuant.Name = "miRegexMatchQuant";
            miRegexMatchQuant.Size = new System.Drawing.Size(176, 24);
            miRegexMatchQuant.Text = "Quantifiers";
            // 
            // miRegexMatchQuantGreedy
            // 
            miRegexMatchQuantGreedy.Enabled = false;
            miRegexMatchQuantGreedy.Name = "miRegexMatchQuantGreedy";
            miRegexMatchQuantGreedy.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantGreedy.Text = "Match as much as possible";
            // 
            // miRegexMatchQuantZeroOneG
            // 
            miRegexMatchQuantZeroOneG.Name = "miRegexMatchQuantZeroOneG";
            miRegexMatchQuantZeroOneG.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantZeroOneG.Text = "Zero or one times\t?";
            miRegexMatchQuantZeroOneG.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuantOneMoreG
            // 
            miRegexMatchQuantOneMoreG.Name = "miRegexMatchQuantOneMoreG";
            miRegexMatchQuantOneMoreG.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantOneMoreG.Text = "One or more times\t+";
            miRegexMatchQuantOneMoreG.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuantZeroMoreG
            // 
            miRegexMatchQuantZeroMoreG.Name = "miRegexMatchQuantZeroMoreG";
            miRegexMatchQuantZeroMoreG.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantZeroMoreG.Text = "Zero or more times\t*";
            miRegexMatchQuantZeroMoreG.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuantExactG
            // 
            miRegexMatchQuantExactG.Name = "miRegexMatchQuantExactG";
            miRegexMatchQuantExactG.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantExactG.Text = "Exactly n times\t{n}";
            miRegexMatchQuantExactG.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuantAtLeastG
            // 
            miRegexMatchQuantAtLeastG.Name = "miRegexMatchQuantAtLeastG";
            miRegexMatchQuantAtLeastG.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantAtLeastG.Text = "At least n times\t{n,}";
            miRegexMatchQuantAtLeastG.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuantBetweenG
            // 
            miRegexMatchQuantBetweenG.Name = "miRegexMatchQuantBetweenG";
            miRegexMatchQuantBetweenG.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantBetweenG.Text = "Between n to m times\t{n,m}";
            miRegexMatchQuantBetweenG.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuantLazy
            // 
            miRegexMatchQuantLazy.Enabled = false;
            miRegexMatchQuantLazy.Name = "miRegexMatchQuantLazy";
            miRegexMatchQuantLazy.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantLazy.Text = "Match as little as possible";
            // 
            // miRegexMatchQuantZeroOneL
            // 
            miRegexMatchQuantZeroOneL.Name = "miRegexMatchQuantZeroOneL";
            miRegexMatchQuantZeroOneL.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantZeroOneL.Text = "Zero or one times\t??";
            miRegexMatchQuantZeroOneL.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuantOneMoreL
            // 
            miRegexMatchQuantOneMoreL.Name = "miRegexMatchQuantOneMoreL";
            miRegexMatchQuantOneMoreL.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantOneMoreL.Text = "One or more times\t+?";
            miRegexMatchQuantOneMoreL.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuantZeroMoreL
            // 
            miRegexMatchQuantZeroMoreL.Name = "miRegexMatchQuantZeroMoreL";
            miRegexMatchQuantZeroMoreL.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantZeroMoreL.Text = "Zero or more times\t*?";
            miRegexMatchQuantZeroMoreL.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuantExactL
            // 
            miRegexMatchQuantExactL.Name = "miRegexMatchQuantExactL";
            miRegexMatchQuantExactL.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantExactL.Text = "Exactly n times\t{n}?";
            miRegexMatchQuantExactL.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuantAtLeastL
            // 
            miRegexMatchQuantAtLeastL.Name = "miRegexMatchQuantAtLeastL";
            miRegexMatchQuantAtLeastL.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantAtLeastL.Text = "At least n times\t{n,}?";
            miRegexMatchQuantAtLeastL.Click += InsertRegexFragment;
            // 
            // miRegexMatchQuantBetweenL
            // 
            miRegexMatchQuantBetweenL.Name = "miRegexMatchQuantBetweenL";
            miRegexMatchQuantBetweenL.Size = new System.Drawing.Size(277, 26);
            miRegexMatchQuantBetweenL.Text = "Between n to m times\t{n,m}?";
            miRegexMatchQuantBetweenL.Click += InsertRegexFragment;
            // 
            // miRegexMatchClass
            // 
            miRegexMatchClass.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchClassPos, miRegexMatchClassNeg, miRegexMatchClassLower, miRegexMatchClassUpper });
            miRegexMatchClass.Name = "miRegexMatchClass";
            miRegexMatchClass.Size = new System.Drawing.Size(176, 24);
            miRegexMatchClass.Text = "Character class";
            // 
            // miRegexMatchClassPos
            // 
            miRegexMatchClassPos.Name = "miRegexMatchClassPos";
            miRegexMatchClassPos.Size = new System.Drawing.Size(216, 26);
            miRegexMatchClassPos.Text = "Positive class\t[...]";
            miRegexMatchClassPos.Click += InsertRegexFragment;
            // 
            // miRegexMatchClassNeg
            // 
            miRegexMatchClassNeg.Name = "miRegexMatchClassNeg";
            miRegexMatchClassNeg.Size = new System.Drawing.Size(216, 26);
            miRegexMatchClassNeg.Text = "Negative class\t[^...]";
            miRegexMatchClassNeg.Click += InsertRegexFragment;
            // 
            // miRegexMatchClassLower
            // 
            miRegexMatchClassLower.Name = "miRegexMatchClassLower";
            miRegexMatchClassLower.Size = new System.Drawing.Size(216, 26);
            miRegexMatchClassLower.Text = "Lowercase\t[a-z]";
            miRegexMatchClassLower.Click += InsertRegexFragment;
            // 
            // miRegexMatchClassUpper
            // 
            miRegexMatchClassUpper.Name = "miRegexMatchClassUpper";
            miRegexMatchClassUpper.Size = new System.Drawing.Size(216, 26);
            miRegexMatchClassUpper.Text = "Uppercase\t[A-Z]";
            miRegexMatchClassUpper.Click += InsertRegexFragment;
            // 
            // miRegexMatchCapt
            // 
            miRegexMatchCapt.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchCaptCreateUnnamed, miRegexMatchCaptMatchUnnamed, miRegexMatchCaptCreateNamed, miRegexMatchCaptMatchNamed });
            miRegexMatchCapt.Name = "miRegexMatchCapt";
            miRegexMatchCapt.Size = new System.Drawing.Size(176, 24);
            miRegexMatchCapt.Text = "Captures";
            // 
            // miRegexMatchCaptCreateUnnamed
            // 
            miRegexMatchCaptCreateUnnamed.Name = "miRegexMatchCaptCreateUnnamed";
            miRegexMatchCaptCreateUnnamed.Size = new System.Drawing.Size(322, 26);
            miRegexMatchCaptCreateUnnamed.Text = "Create unnamed capture\t(...)";
            miRegexMatchCaptCreateUnnamed.Click += InsertRegexFragment;
            // 
            // miRegexMatchCaptMatchUnnamed
            // 
            miRegexMatchCaptMatchUnnamed.Name = "miRegexMatchCaptMatchUnnamed";
            miRegexMatchCaptMatchUnnamed.Size = new System.Drawing.Size(322, 26);
            miRegexMatchCaptMatchUnnamed.Text = "Match unnamed capture\t\\n";
            miRegexMatchCaptMatchUnnamed.Click += InsertRegexFragment;
            // 
            // miRegexMatchCaptCreateNamed
            // 
            miRegexMatchCaptCreateNamed.Name = "miRegexMatchCaptCreateNamed";
            miRegexMatchCaptCreateNamed.Size = new System.Drawing.Size(322, 26);
            miRegexMatchCaptCreateNamed.Text = "Create named capture\t(?<name>...)";
            miRegexMatchCaptCreateNamed.Click += InsertRegexFragment;
            // 
            // miRegexMatchCaptMatchNamed
            // 
            miRegexMatchCaptMatchNamed.Name = "miRegexMatchCaptMatchNamed";
            miRegexMatchCaptMatchNamed.Size = new System.Drawing.Size(322, 26);
            miRegexMatchCaptMatchNamed.Text = "Match named capture\t\\<name>";
            miRegexMatchCaptMatchNamed.Click += InsertRegexFragment;
            // 
            // miRegexMatchLook
            // 
            miRegexMatchLook.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchLookPosAhead, miRegexMatchLookNegAhead, miRegexMatchLookPosBehind, miRegexMatchLookNegBehind });
            miRegexMatchLook.Name = "miRegexMatchLook";
            miRegexMatchLook.Size = new System.Drawing.Size(176, 24);
            miRegexMatchLook.Text = "Lookaround";
            // 
            // miRegexMatchLookPosAhead
            // 
            miRegexMatchLookPosAhead.Name = "miRegexMatchLookPosAhead";
            miRegexMatchLookPosAhead.Size = new System.Drawing.Size(271, 26);
            miRegexMatchLookPosAhead.Text = "Positive lookahead\t(?=...)";
            miRegexMatchLookPosAhead.Click += InsertRegexFragment;
            // 
            // miRegexMatchLookNegAhead
            // 
            miRegexMatchLookNegAhead.Name = "miRegexMatchLookNegAhead";
            miRegexMatchLookNegAhead.Size = new System.Drawing.Size(271, 26);
            miRegexMatchLookNegAhead.Text = "Negative lookahead\t(?!...)";
            miRegexMatchLookNegAhead.Click += InsertRegexFragment;
            // 
            // miRegexMatchLookPosBehind
            // 
            miRegexMatchLookPosBehind.Name = "miRegexMatchLookPosBehind";
            miRegexMatchLookPosBehind.Size = new System.Drawing.Size(271, 26);
            miRegexMatchLookPosBehind.Text = "Positive lookbehind\t(?<=...)";
            miRegexMatchLookPosBehind.Click += InsertRegexFragment;
            // 
            // miRegexMatchLookNegBehind
            // 
            miRegexMatchLookNegBehind.Name = "miRegexMatchLookNegBehind";
            miRegexMatchLookNegBehind.Size = new System.Drawing.Size(271, 26);
            miRegexMatchLookNegBehind.Text = "Negative lookbehind\t(?<!...)";
            miRegexMatchLookNegBehind.Click += InsertRegexFragment;
            // 
            // miRegexMatchSep1
            // 
            miRegexMatchSep1.Name = "miRegexMatchSep1";
            miRegexMatchSep1.Size = new System.Drawing.Size(176, 24);
            miRegexMatchSep1.Text = "-";
            // 
            // miRegexMatchLiteral
            // 
            miRegexMatchLiteral.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchLiteralDot, miRegexMatchLiteralQuestion, miRegexMatchLiteralPlus, miRegexMatchLiteralStar, miRegexMatchLiteralCaret, miRegexMatchLiteralDollar, miRegexMatchLiteralBackslash, miRegexMatchLiteralOpenRound, miRegexMatchLiteralCloseRound, miRegexMatchLiteralOpenSquare, miRegexMatchLiteralCloseSquare, miRegexMatchLiteralOpenCurly, miRegexMatchLiteralCloseCurly, miRegexMatchLiteralPipe });
            miRegexMatchLiteral.Name = "miRegexMatchLiteral";
            miRegexMatchLiteral.Size = new System.Drawing.Size(176, 24);
            miRegexMatchLiteral.Text = "Literals";
            // 
            // miRegexMatchLiteralDot
            // 
            miRegexMatchLiteralDot.Name = "miRegexMatchLiteralDot";
            miRegexMatchLiteralDot.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralDot.Text = "Dot\t\\.";
            miRegexMatchLiteralDot.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralQuestion
            // 
            miRegexMatchLiteralQuestion.Name = "miRegexMatchLiteralQuestion";
            miRegexMatchLiteralQuestion.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralQuestion.Text = "Question mark\t\\?";
            miRegexMatchLiteralQuestion.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralPlus
            // 
            miRegexMatchLiteralPlus.Name = "miRegexMatchLiteralPlus";
            miRegexMatchLiteralPlus.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralPlus.Text = "Plus sign\t\\+";
            miRegexMatchLiteralPlus.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralStar
            // 
            miRegexMatchLiteralStar.Name = "miRegexMatchLiteralStar";
            miRegexMatchLiteralStar.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralStar.Text = "Star\t\\*";
            miRegexMatchLiteralStar.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralCaret
            // 
            miRegexMatchLiteralCaret.Name = "miRegexMatchLiteralCaret";
            miRegexMatchLiteralCaret.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralCaret.Text = "Caret\t\\^";
            miRegexMatchLiteralCaret.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralDollar
            // 
            miRegexMatchLiteralDollar.Name = "miRegexMatchLiteralDollar";
            miRegexMatchLiteralDollar.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralDollar.Text = "Dollar sign\t\\$";
            miRegexMatchLiteralDollar.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralBackslash
            // 
            miRegexMatchLiteralBackslash.Name = "miRegexMatchLiteralBackslash";
            miRegexMatchLiteralBackslash.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralBackslash.Text = "Backslash\t\\\\";
            miRegexMatchLiteralBackslash.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralOpenRound
            // 
            miRegexMatchLiteralOpenRound.Name = "miRegexMatchLiteralOpenRound";
            miRegexMatchLiteralOpenRound.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralOpenRound.Text = "Open round bracket\t\\(";
            miRegexMatchLiteralOpenRound.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralCloseRound
            // 
            miRegexMatchLiteralCloseRound.Name = "miRegexMatchLiteralCloseRound";
            miRegexMatchLiteralCloseRound.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralCloseRound.Text = "Close round bracket\t\\)";
            miRegexMatchLiteralCloseRound.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralOpenSquare
            // 
            miRegexMatchLiteralOpenSquare.Name = "miRegexMatchLiteralOpenSquare";
            miRegexMatchLiteralOpenSquare.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralOpenSquare.Text = "Open square bracket\t\\[";
            miRegexMatchLiteralOpenSquare.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralCloseSquare
            // 
            miRegexMatchLiteralCloseSquare.Name = "miRegexMatchLiteralCloseSquare";
            miRegexMatchLiteralCloseSquare.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralCloseSquare.Text = "Close square bracket\t\\]";
            miRegexMatchLiteralCloseSquare.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralOpenCurly
            // 
            miRegexMatchLiteralOpenCurly.Name = "miRegexMatchLiteralOpenCurly";
            miRegexMatchLiteralOpenCurly.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralOpenCurly.Text = "Open curly bracket\t\\{";
            miRegexMatchLiteralOpenCurly.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralCloseCurly
            // 
            miRegexMatchLiteralCloseCurly.Name = "miRegexMatchLiteralCloseCurly";
            miRegexMatchLiteralCloseCurly.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralCloseCurly.Text = "Close curly bracket\t\\}";
            miRegexMatchLiteralCloseCurly.Click += InsertRegexFragment;
            // 
            // miRegexMatchLiteralPipe
            // 
            miRegexMatchLiteralPipe.Name = "miRegexMatchLiteralPipe";
            miRegexMatchLiteralPipe.Size = new System.Drawing.Size(240, 26);
            miRegexMatchLiteralPipe.Text = "Pipe\t\\|";
            miRegexMatchLiteralPipe.Click += InsertRegexFragment;
            // 
            // cmRegexReplace
            // 
            cmRegexReplace.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmRegexReplace.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexReplaceCapture, miRegexReplaceOrig, miRegexReplaceSpecial, miRegexReplaceSep1, miRegexReplaceLiteral });
            cmRegexReplace.Name = "cmRegexReplace";
            cmRegexReplace.Size = new System.Drawing.Size(161, 124);
            // 
            // miRegexReplaceCapture
            // 
            miRegexReplaceCapture.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexReplaceCaptureUnnamed, miRegexReplaceCaptureNamed });
            miRegexReplaceCapture.Name = "miRegexReplaceCapture";
            miRegexReplaceCapture.Size = new System.Drawing.Size(160, 24);
            miRegexReplaceCapture.Text = "Capture";
            // 
            // miRegexReplaceCaptureUnnamed
            // 
            miRegexReplaceCaptureUnnamed.Name = "miRegexReplaceCaptureUnnamed";
            miRegexReplaceCaptureUnnamed.Size = new System.Drawing.Size(196, 26);
            miRegexReplaceCaptureUnnamed.Text = "Unnamed\t$n";
            miRegexReplaceCaptureUnnamed.Click += InsertRegexFragment;
            // 
            // miRegexReplaceCaptureNamed
            // 
            miRegexReplaceCaptureNamed.Name = "miRegexReplaceCaptureNamed";
            miRegexReplaceCaptureNamed.Size = new System.Drawing.Size(196, 26);
            miRegexReplaceCaptureNamed.Text = "Named\t${name}";
            miRegexReplaceCaptureNamed.Click += InsertRegexFragment;
            // 
            // miRegexReplaceOrig
            // 
            miRegexReplaceOrig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexReplaceOrigMatched, miRegexReplaceOrigBefore, miRegexReplaceOrigAfter, miRegexReplaceOrigAll });
            miRegexReplaceOrig.Name = "miRegexReplaceOrig";
            miRegexReplaceOrig.Size = new System.Drawing.Size(160, 24);
            miRegexReplaceOrig.Text = "Original text";
            // 
            // miRegexReplaceOrigMatched
            // 
            miRegexReplaceOrigMatched.Name = "miRegexReplaceOrigMatched";
            miRegexReplaceOrigMatched.Size = new System.Drawing.Size(224, 26);
            miRegexReplaceOrigMatched.Text = "Matched text\t$0";
            miRegexReplaceOrigMatched.Click += InsertRegexFragment;
            // 
            // miRegexReplaceOrigBefore
            // 
            miRegexReplaceOrigBefore.Name = "miRegexReplaceOrigBefore";
            miRegexReplaceOrigBefore.Size = new System.Drawing.Size(224, 26);
            miRegexReplaceOrigBefore.Text = "Text before match\t$`";
            miRegexReplaceOrigBefore.Click += InsertRegexFragment;
            // 
            // miRegexReplaceOrigAfter
            // 
            miRegexReplaceOrigAfter.Name = "miRegexReplaceOrigAfter";
            miRegexReplaceOrigAfter.Size = new System.Drawing.Size(224, 26);
            miRegexReplaceOrigAfter.Text = "Text after match\t$'";
            miRegexReplaceOrigAfter.Click += InsertRegexFragment;
            // 
            // miRegexReplaceOrigAll
            // 
            miRegexReplaceOrigAll.Name = "miRegexReplaceOrigAll";
            miRegexReplaceOrigAll.Size = new System.Drawing.Size(224, 26);
            miRegexReplaceOrigAll.Text = "Original filename\t$_";
            miRegexReplaceOrigAll.Click += InsertRegexFragment;
            // 
            // miRegexReplaceSpecial
            // 
            miRegexReplaceSpecial.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexReplaceSpecialNumSeq });
            miRegexReplaceSpecial.Name = "miRegexReplaceSpecial";
            miRegexReplaceSpecial.Size = new System.Drawing.Size(160, 24);
            miRegexReplaceSpecial.Text = "Special";
            // 
            // miRegexReplaceSpecialNumSeq
            // 
            miRegexReplaceSpecialNumSeq.Name = "miRegexReplaceSpecialNumSeq";
            miRegexReplaceSpecialNumSeq.Size = new System.Drawing.Size(229, 26);
            miRegexReplaceSpecialNumSeq.Text = "Number sequence\t$#";
            miRegexReplaceSpecialNumSeq.Click += InsertRegexFragment;
            // 
            // miRegexReplaceSep1
            // 
            miRegexReplaceSep1.Name = "miRegexReplaceSep1";
            miRegexReplaceSep1.Size = new System.Drawing.Size(160, 24);
            miRegexReplaceSep1.Text = "-";
            // 
            // miRegexReplaceLiteral
            // 
            miRegexReplaceLiteral.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexReplaceLiteralDollar });
            miRegexReplaceLiteral.Name = "miRegexReplaceLiteral";
            miRegexReplaceLiteral.Size = new System.Drawing.Size(160, 24);
            miRegexReplaceLiteral.Text = "Literals";
            // 
            // miRegexReplaceLiteralDollar
            // 
            miRegexReplaceLiteralDollar.Name = "miRegexReplaceLiteralDollar";
            miRegexReplaceLiteralDollar.Size = new System.Drawing.Size(180, 26);
            miRegexReplaceLiteralDollar.Text = "Dollar sign\t$$";
            miRegexReplaceLiteralDollar.Click += InsertRegexFragment;
            // 
            // cmGlobMatch
            // 
            cmGlobMatch.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmGlobMatch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miGlobMatchSingle, miGlobMatchMultiple });
            cmGlobMatch.Name = "cmGlobMatch";
            cmGlobMatch.Size = new System.Drawing.Size(211, 52);
            // 
            // miGlobMatchSingle
            // 
            miGlobMatchSingle.Name = "miGlobMatchSingle";
            miGlobMatchSingle.Size = new System.Drawing.Size(210, 24);
            miGlobMatchSingle.Text = "Single character\t?";
            miGlobMatchSingle.Click += InsertRegexFragment;
            // 
            // miGlobMatchMultiple
            // 
            miGlobMatchMultiple.Name = "miGlobMatchMultiple";
            miGlobMatchMultiple.Size = new System.Drawing.Size(210, 24);
            miGlobMatchMultiple.Text = "Multiple characters\t*";
            miGlobMatchMultiple.Click += InsertRegexFragment;
            // 
            // cmsBlank
            // 
            cmsBlank.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmsBlank.Name = "cmsBlank";
            cmsBlank.Size = new System.Drawing.Size(61, 4);
            // 
            // lblPath
            // 
            lblPath.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblPath.AutoSize = true;
            lblPath.Location = new System.Drawing.Point(5, 945);
            lblPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPath.Name = "lblPath";
            lblPath.Size = new System.Drawing.Size(40, 20);
            lblPath.TabIndex = 0;
            lblPath.Text = "Path:";
            // 
            // dgvFiles
            // 
            dgvFiles.AllowUserToAddRows = false;
            dgvFiles.AllowUserToDeleteRows = false;
            dgvFiles.AllowUserToResizeRows = false;
            dgvFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvFiles.BackgroundColor = System.Drawing.SystemColors.Window;
            dgvFiles.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dgvFiles.ColumnHeadersHeight = 29;
            dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colIcon, colFilename, colPreview, colSeries, colVolume, colChapter, colTitle, colEdition, colSpecial });
            dgvFiles.ContextMenuStrip = cmFileView;
            dgvFiles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            dgvFiles.GridColor = System.Drawing.SystemColors.Control;
            dgvFiles.Location = new System.Drawing.Point(7, 20);
            dgvFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            dgvFiles.Name = "dgvFiles";
            dgvFiles.RowHeadersVisible = false;
            dgvFiles.RowHeadersWidth = 51;
            dgvFiles.RowTemplate.Height = 17;
            dgvFiles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dgvFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvFiles.ShowCellToolTips = false;
            dgvFiles.Size = new System.Drawing.Size(1182, 911);
            dgvFiles.StandardTab = true;
            dgvFiles.TabIndex = 6;
            dgvFiles.CellBeginEdit += dgvFiles_CellBeginEdit;
            dgvFiles.CellDoubleClick += dgvFiles_CellDoubleClick;
            dgvFiles.CellEndEdit += dgvFiles_CellEndEdit;
            dgvFiles.CellMouseEnter += dgvFiles_CellMouseEnter;
            dgvFiles.CellMouseLeave += dgvFiles_CellMouseLeave;
            dgvFiles.CellValidating += dgvFiles_CellValidating;
            dgvFiles.SelectionChanged += dgvFiles_SelectionChanged;
            dgvFiles.KeyUp += dgvFiles_KeyUp;
            dgvFiles.Leave += dgvFiles_Leave;
            // 
            // colIcon
            // 
            colIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            colIcon.HeaderText = "";
            colIcon.MinimumWidth = 6;
            colIcon.Name = "colIcon";
            colIcon.ReadOnly = true;
            colIcon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            colIcon.Width = 20;
            // 
            // colFilename
            // 
            colFilename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colFilename.HeaderText = "Filename";
            colFilename.MinimumWidth = 6;
            colFilename.Name = "colFilename";
            // 
            // colPreview
            // 
            colPreview.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colPreview.HeaderText = "Preview";
            colPreview.MinimumWidth = 6;
            colPreview.Name = "colPreview";
            colPreview.ReadOnly = true;
            // 
            // colSeries
            // 
            colSeries.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colSeries.HeaderText = "Series";
            colSeries.MinimumWidth = 6;
            colSeries.Name = "colSeries";
            colSeries.ReadOnly = true;
            colSeries.Visible = false;
            colSeries.Width = 125;
            // 
            // colVolume
            // 
            colVolume.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            colVolume.HeaderText = "Volume";
            colVolume.MinimumWidth = 6;
            colVolume.Name = "colVolume";
            colVolume.ReadOnly = true;
            colVolume.Visible = false;
            colVolume.Width = 125;
            // 
            // colChapter
            // 
            colChapter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            colChapter.HeaderText = "Chapter";
            colChapter.MinimumWidth = 6;
            colChapter.Name = "colChapter";
            colChapter.ReadOnly = true;
            colChapter.Visible = false;
            colChapter.Width = 125;
            // 
            // colTitle
            // 
            colTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colTitle.HeaderText = "Title";
            colTitle.MinimumWidth = 6;
            colTitle.Name = "colTitle";
            colTitle.ReadOnly = true;
            colTitle.Visible = false;
            colTitle.Width = 125;
            // 
            // colEdition
            // 
            colEdition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            colEdition.HeaderText = "Edition";
            colEdition.MinimumWidth = 6;
            colEdition.Name = "colEdition";
            colEdition.ReadOnly = true;
            colEdition.Visible = false;
            colEdition.Width = 125;
            // 
            // colSpecial
            // 
            colSpecial.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            colSpecial.HeaderText = "Special";
            colSpecial.MinimumWidth = 6;
            colSpecial.Name = "colSpecial";
            colSpecial.ReadOnly = true;
            colSpecial.Visible = false;
            colSpecial.Width = 125;
            // 
            // cmFileView
            // 
            cmFileView.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmFileView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { editToolStripMenuItem, explorerContextMenuToolStripMenuItem1, copyToolStripMenuItem, cutToolStripMenuItem, pasteToolStripMenuItem, deleteToolStripMenuItem });
            cmFileView.Name = "contextMenuStripFileView";
            cmFileView.Size = new System.Drawing.Size(230, 148);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(229, 24);
            editToolStripMenuItem.Text = "Edit";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // explorerContextMenuToolStripMenuItem1
            // 
            explorerContextMenuToolStripMenuItem1.Name = "explorerContextMenuToolStripMenuItem1";
            explorerContextMenuToolStripMenuItem1.Size = new System.Drawing.Size(229, 24);
            explorerContextMenuToolStripMenuItem1.Text = "Explorer Context Menu";
            explorerContextMenuToolStripMenuItem1.Click += explorerContextMenuToolStripMenuItem1_Click;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(229, 24);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.Size = new System.Drawing.Size(229, 24);
            cutToolStripMenuItem.Text = "Cut";
            cutToolStripMenuItem.Click += cutToolStripMenuItem_Click;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new System.Drawing.Size(229, 24);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += pasteToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new System.Drawing.Size(229, 24);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // tsOptions
            // 
            tsOptions.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            tsOptions.BackColor = System.Drawing.SystemColors.ButtonFace;
            tsOptions.CanOverflow = false;
            tsOptions.Dock = System.Windows.Forms.DockStyle.None;
            tsOptions.Font = new System.Drawing.Font("Tahoma", 8.25F);
            tsOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            tsOptions.ImageScalingSize = new System.Drawing.Size(20, 20);
            tsOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuOptions, mnuHelp });
            tsOptions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            tsOptions.Location = new System.Drawing.Point(7, 948);
            tsOptions.Name = "tsOptions";
            tsOptions.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            tsOptions.Size = new System.Drawing.Size(128, 22);
            tsOptions.TabIndex = 2;
            tsOptions.TabStop = true;
            // 
            // mnuOptions
            // 
            mnuOptions.AutoToolTip = false;
            mnuOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { itmOptionsShowHidden, itmOptionsPreserveExt, itmOptionsRealtimePreview, itmOptionsAllowRenSub, itmOptionsRenameSelectedRows, itmOptionsRememberWinPos, itmOptionsAddContextMenu });
            mnuOptions.Margin = new System.Windows.Forms.Padding(0, 1, 10, 0);
            mnuOptions.Name = "mnuOptions";
            mnuOptions.Size = new System.Drawing.Size(69, 21);
            mnuOptions.Text = "Options";
            mnuOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // itmOptionsShowHidden
            // 
            itmOptionsShowHidden.CheckOnClick = true;
            itmOptionsShowHidden.Name = "itmOptionsShowHidden";
            itmOptionsShowHidden.Size = new System.Drawing.Size(259, 26);
            itmOptionsShowHidden.Text = "Show hidden files";
            itmOptionsShowHidden.Click += itmOptionsShowHidden_Click;
            // 
            // itmOptionsPreserveExt
            // 
            itmOptionsPreserveExt.CheckOnClick = true;
            itmOptionsPreserveExt.Name = "itmOptionsPreserveExt";
            itmOptionsPreserveExt.Size = new System.Drawing.Size(259, 26);
            itmOptionsPreserveExt.Text = "Preserve file extension";
            itmOptionsPreserveExt.Click += itmOptionsPreserveExt_Click;
            // 
            // itmOptionsRealtimePreview
            // 
            itmOptionsRealtimePreview.Checked = true;
            itmOptionsRealtimePreview.CheckOnClick = true;
            itmOptionsRealtimePreview.CheckState = System.Windows.Forms.CheckState.Checked;
            itmOptionsRealtimePreview.Name = "itmOptionsRealtimePreview";
            itmOptionsRealtimePreview.Size = new System.Drawing.Size(259, 26);
            itmOptionsRealtimePreview.Text = "Enable realtime preview";
            itmOptionsRealtimePreview.ToolTipText = "When unchecked, press ENTER in the regex fields to update the preview";
            // 
            // itmOptionsAllowRenSub
            // 
            itmOptionsAllowRenSub.CheckOnClick = true;
            itmOptionsAllowRenSub.Name = "itmOptionsAllowRenSub";
            itmOptionsAllowRenSub.Size = new System.Drawing.Size(259, 26);
            itmOptionsAllowRenSub.Text = "Allow rename to subfolders";
            itmOptionsAllowRenSub.Click += itmOptionsAllowRenSub_Click;
            // 
            // itmOptionsRenameSelectedRows
            // 
            itmOptionsRenameSelectedRows.CheckOnClick = true;
            itmOptionsRenameSelectedRows.Name = "itmOptionsRenameSelectedRows";
            itmOptionsRenameSelectedRows.Size = new System.Drawing.Size(259, 26);
            itmOptionsRenameSelectedRows.Text = "Only rename selected rows";
            // 
            // itmOptionsRememberWinPos
            // 
            itmOptionsRememberWinPos.Checked = true;
            itmOptionsRememberWinPos.CheckOnClick = true;
            itmOptionsRememberWinPos.CheckState = System.Windows.Forms.CheckState.Checked;
            itmOptionsRememberWinPos.Name = "itmOptionsRememberWinPos";
            itmOptionsRememberWinPos.Size = new System.Drawing.Size(259, 26);
            itmOptionsRememberWinPos.Text = "Remember window position";
            // 
            // itmOptionsAddContextMenu
            // 
            itmOptionsAddContextMenu.Name = "itmOptionsAddContextMenu";
            itmOptionsAddContextMenu.Size = new System.Drawing.Size(259, 26);
            itmOptionsAddContextMenu.Text = "Add explorer context menu";
            itmOptionsAddContextMenu.Click += itmOptionsAddContextMenu_Click;
            // 
            // mnuHelp
            // 
            mnuHelp.AutoToolTip = false;
            mnuHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { itmHelpContents, itmHelpRegexReference, itmHelpSep1, itmHelpEmailAuthor, itmHelpReportBug, itmHelpHomepage, itmHelpSep2, itmHelpAbout });
            mnuHelp.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            mnuHelp.Name = "mnuHelp";
            mnuHelp.Size = new System.Drawing.Size(48, 21);
            mnuHelp.Text = "Help";
            mnuHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // itmHelpContents
            // 
            itmHelpContents.Name = "itmHelpContents";
            itmHelpContents.ShortcutKeys = System.Windows.Forms.Keys.F1;
            itmHelpContents.Size = new System.Drawing.Size(254, 26);
            itmHelpContents.Text = "Contents";
            itmHelpContents.Click += itmHelpContents_Click;
            // 
            // itmHelpRegexReference
            // 
            itmHelpRegexReference.Name = "itmHelpRegexReference";
            itmHelpRegexReference.ShortcutKeys = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F1;
            itmHelpRegexReference.Size = new System.Drawing.Size(254, 26);
            itmHelpRegexReference.Text = "Regex Reference";
            itmHelpRegexReference.Click += itmHelpRegexReference_Click;
            // 
            // itmHelpSep1
            // 
            itmHelpSep1.Name = "itmHelpSep1";
            itmHelpSep1.Size = new System.Drawing.Size(251, 6);
            // 
            // itmHelpEmailAuthor
            // 
            itmHelpEmailAuthor.Name = "itmHelpEmailAuthor";
            itmHelpEmailAuthor.Size = new System.Drawing.Size(254, 26);
            itmHelpEmailAuthor.Text = "Email the author";
            itmHelpEmailAuthor.Click += itmHelpEmailAuthor_Click;
            // 
            // itmHelpReportBug
            // 
            itmHelpReportBug.Name = "itmHelpReportBug";
            itmHelpReportBug.Size = new System.Drawing.Size(254, 26);
            itmHelpReportBug.Text = "Report a bug";
            itmHelpReportBug.Click += itmHelpReportBug_Click;
            // 
            // itmHelpHomepage
            // 
            itmHelpHomepage.Name = "itmHelpHomepage";
            itmHelpHomepage.Size = new System.Drawing.Size(254, 26);
            itmHelpHomepage.Text = "Homepage";
            itmHelpHomepage.Click += itmHelpHomepage_Click;
            // 
            // itmHelpSep2
            // 
            itmHelpSep2.Name = "itmHelpSep2";
            itmHelpSep2.Size = new System.Drawing.Size(251, 6);
            // 
            // itmHelpAbout
            // 
            itmHelpAbout.Name = "itmHelpAbout";
            itmHelpAbout.Size = new System.Drawing.Size(254, 26);
            itmHelpAbout.Text = "About RegexRenamer";
            itmHelpAbout.Click += itmHelpAbout_Click;
            // 
            // progressBar
            // 
            progressBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar.Location = new System.Drawing.Point(154, 940);
            progressBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(805, 34);
            progressBar.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Enabled = false;
            btnCancel.Location = new System.Drawing.Point(1076, 937);
            btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(113, 38);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Visible = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // bgwRename
            // 
            bgwRename.WorkerReportsProgress = true;
            bgwRename.WorkerSupportsCancellation = true;
            bgwRename.DoWork += bgwRename_DoWork;
            bgwRename.ProgressChanged += bgwRename_ProgressChanged;
            bgwRename.RunWorkerCompleted += bgwRename_RunWorkerCompleted;
            // 
            // groupBoxTop
            // 
            groupBoxTop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBoxTop.Controls.Add(pnlInfo);
            groupBoxTop.Controls.Add(lblMatch);
            groupBoxTop.Controls.Add(cmbReplace);
            groupBoxTop.Controls.Add(cmbMatch);
            groupBoxTop.Controls.Add(cbModifierI);
            groupBoxTop.Controls.Add(cbModifierG);
            groupBoxTop.Controls.Add(cbModifierX);
            groupBoxTop.Controls.Add(lblReplace);
            groupBoxTop.Controls.Add(tsMenu);
            groupBoxTop.Controls.Add(gbFilter);
            groupBoxTop.Controls.Add(lblStats);
            groupBoxTop.Controls.Add(pnlStats);
            groupBoxTop.Location = new System.Drawing.Point(12, 3);
            groupBoxTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBoxTop.Name = "groupBoxTop";
            groupBoxTop.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBoxTop.Size = new System.Drawing.Size(1699, 127);
            groupBoxTop.TabIndex = 7;
            groupBoxTop.TabStop = false;
            // 
            // pnlInfo
            // 
            pnlInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            pnlInfo.Controls.Add(lblInfoFileSize);
            pnlInfo.Location = new System.Drawing.Point(1175, 19);
            pnlInfo.Name = "pnlInfo";
            pnlInfo.Size = new System.Drawing.Size(199, 99);
            pnlInfo.TabIndex = 7;
            // 
            // lblInfoFileSize
            // 
            lblInfoFileSize.AutoSize = true;
            lblInfoFileSize.Location = new System.Drawing.Point(13, 5);
            lblInfoFileSize.Name = "lblInfoFileSize";
            lblInfoFileSize.Size = new System.Drawing.Size(39, 20);
            lblInfoFileSize.TabIndex = 0;
            lblInfoFileSize.Text = "0 Kb";
            // 
            // groupBoxFolderTree
            // 
            groupBoxFolderTree.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            groupBoxFolderTree.Controls.Add(txtPath);
            groupBoxFolderTree.Controls.Add(lblPath);
            groupBoxFolderTree.Controls.Add(tvwFolders);
            groupBoxFolderTree.Controls.Add(btnNetwork);
            groupBoxFolderTree.Location = new System.Drawing.Point(12, 132);
            groupBoxFolderTree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBoxFolderTree.Name = "groupBoxFolderTree";
            groupBoxFolderTree.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBoxFolderTree.Size = new System.Drawing.Size(504, 987);
            groupBoxFolderTree.TabIndex = 8;
            groupBoxFolderTree.TabStop = false;
            // 
            // groupBoxFileView
            // 
            groupBoxFileView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBoxFileView.Controls.Add(dgvFiles);
            groupBoxFileView.Controls.Add(progressBar);
            groupBoxFileView.Controls.Add(tsOptions);
            groupBoxFileView.Controls.Add(btnRename);
            groupBoxFileView.Controls.Add(btnCancel);
            groupBoxFileView.Controls.Add(lblNumConflict);
            groupBoxFileView.Controls.Add(lblNumMatched);
            groupBoxFileView.Location = new System.Drawing.Point(522, 132);
            groupBoxFileView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBoxFileView.Name = "groupBoxFileView";
            groupBoxFileView.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBoxFileView.Size = new System.Drawing.Size(1198, 986);
            groupBoxFileView.TabIndex = 9;
            groupBoxFileView.TabStop = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1732, 1132);
            Controls.Add(groupBoxFileView);
            Controls.Add(groupBoxFolderTree);
            Controls.Add(groupBoxTop);
            Icon = Properties.Resources.icon;
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MinimumSize = new System.Drawing.Size(711, 436);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "RegexRenamer";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            KeyDown += MainForm_KeyDown;
            cmFolderView.ResumeLayout(false);
            cmsRename.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlStats.ResumeLayout(false);
            tsMenu.ResumeLayout(false);
            tsMenu.PerformLayout();
            cmRegexMatch.ResumeLayout(false);
            cmRegexReplace.ResumeLayout(false);
            cmGlobMatch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvFiles).EndInit();
            cmFileView.ResumeLayout(false);
            tsOptions.ResumeLayout(false);
            tsOptions.PerformLayout();
            groupBoxTop.ResumeLayout(false);
            groupBoxTop.PerformLayout();
            pnlInfo.ResumeLayout(false);
            pnlInfo.PerformLayout();
            groupBoxFolderTree.ResumeLayout(false);
            groupBoxFolderTree.PerformLayout();
            groupBoxFileView.ResumeLayout(false);
            groupBoxFileView.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.RadioButton rbFilterRegex;
        private System.Windows.Forms.RadioButton rbFilterGlob;
        private System.Windows.Forms.TextBox txtFilter;
        private Controls.MyComboBox cmbMatch;
        private System.Windows.Forms.Label lblMatch;
        private System.Windows.Forms.Label lblReplace;
        private System.Windows.Forms.CheckBox cbModifierI;
        private System.Windows.Forms.CheckBox cbModifierG;
        private System.Windows.Forms.CheckBox cbModifierX;
        private Controls.MyComboBox cmbReplace;
        private System.Windows.Forms.ToolTip toolTip;
        private Controls.FolderTreeView tvwFolders;
        private System.Windows.Forms.ToolStrip tsMenu;
        private System.Windows.Forms.ToolStripDropDownButton mnuNumbering;
        private System.Windows.Forms.ToolStripTextBox txtNumberingStart;
        private System.Windows.Forms.ToolStripTextBox txtNumberingPad;
        private System.Windows.Forms.ToolStripTextBox txtNumberingInc;
        private System.Windows.Forms.ToolStripDropDownButton mnuChangeCase;
        private System.Windows.Forms.ToolStripMenuItem itmChangeCaseNoChange;
        private System.Windows.Forms.ToolStripMenuItem itmChangeCaseUppercase;
        private System.Windows.Forms.ToolStripMenuItem itmChangeCaseLowercase;
        private System.Windows.Forms.ToolStripMenuItem itmChangeCaseTitlecase;
        private System.Windows.Forms.ToolStripDropDownButton mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem itmHelpContents;
        private System.Windows.Forms.ToolStripMenuItem itmHelpRegexReference;
        private System.Windows.Forms.ToolStripMenuItem itmHelpEmailAuthor;
        private System.Windows.Forms.ToolStripMenuItem itmHelpReportBug;
        private System.Windows.Forms.ToolStripMenuItem itmHelpHomepage;
        private System.Windows.Forms.ToolStripMenuItem itmHelpAbout;
        private System.Windows.Forms.ToolStripSeparator itmHelpSep1;
        private System.Windows.Forms.ToolStripSeparator itmHelpSep2;
        private System.Windows.Forms.ToolTip ttPreviewError;
        private System.Windows.Forms.ToolStripSeparator itmChangeCaseSep;
        private System.Windows.Forms.FolderBrowserDialog fbdNetwork;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnNetwork;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.CheckBox cbFilterExclude;
        private System.Windows.Forms.ToolStrip tsOptions;
        private System.Windows.Forms.ToolStripDropDownButton mnuOptions;
        private System.Windows.Forms.ToolStripMenuItem itmOptionsShowHidden;
        private System.Windows.Forms.ToolStripMenuItem itmOptionsPreserveExt;
        private System.Windows.Forms.ToolStripMenuItem itmOptionsAllowRenSub;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ToolStripTextBox txtNumberingReset;
        private System.Windows.Forms.ToolStripDropDownButton mnuMoveCopy;
        private System.Windows.Forms.ToolStripMenuItem itmOutputRenameInPlace;
        private System.Windows.Forms.ToolStripSeparator itmOutputSep;
        private System.Windows.Forms.ToolStripMenuItem itmOutputMoveTo;
        private System.Windows.Forms.ToolStripMenuItem itmOutputCopyTo;
        private System.Windows.Forms.ToolStripMenuItem itmOutputBackupTo;
        private System.Windows.Forms.FolderBrowserDialog fbdMoveCopy;

        private System.Windows.Forms.ContextMenuStrip cmRegexMatch;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchMatch;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchMatchSingleChar;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchMatchDigit;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchMatchAlpha;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchMatchSpace;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchMatchMultiChar;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchMatchNonDigit;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchMatchNonAlpha;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchMatchNonSpace;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchAnchor;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchAnchorStart;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchAnchorBound;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchAnchorEnd;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchAnchorNonBound;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchGroup;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuant;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchClass;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchCapt;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLook;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchGroupCapt;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchGroupNonCapt;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchGroupAlt;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantGreedy;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantZeroOneG;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantOneMoreG;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantZeroMoreG;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantExactG;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantAtLeastG;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantBetweenG;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantLazy;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantZeroOneL;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantOneMoreL;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantZeroMoreL;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantExactL;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantAtLeastL;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchQuantBetweenL;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchClassPos;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchClassNeg;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchClassLower;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchClassUpper;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchCaptCreateUnnamed;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchCaptMatchUnnamed;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchCaptCreateNamed;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchCaptMatchNamed;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLookPosAhead;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLookNegAhead;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLookPosBehind;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLookNegBehind;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchAnchorStartEnd;
        private System.Windows.Forms.ContextMenuStrip cmRegexReplace;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceCapture;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceCaptureUnnamed;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceCaptureNamed;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceOrig;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceOrigMatched;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceOrigBefore;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceOrigAfter;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceOrigAll;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceSpecialNumSeq;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceSpecial;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceLiteral;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceLiteralDollar;
        private System.Windows.Forms.ToolStripMenuItem miRegexReplaceSep1;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchSep1;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteral;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralDot;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralQuestion;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralPlus;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralStar;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralCaret;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralDollar;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralBackslash;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralOpenRound;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralCloseRound;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralOpenSquare;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralCloseSquare;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralOpenCurly;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralCloseCurly;
        private System.Windows.Forms.ToolStripMenuItem miRegexMatchLiteralPipe;
        private System.Windows.Forms.ContextMenuStrip cmGlobMatch;
        private System.Windows.Forms.ToolStripMenuItem miGlobMatchSingle;
        private System.Windows.Forms.ToolStripMenuItem miGlobMatchMultiple;
        private System.Windows.Forms.ContextMenuStrip cmsBlank;
        private System.Windows.Forms.Button btnCancel;
        private System.ComponentModel.BackgroundWorker bgwRename;
        private System.Windows.Forms.ToolStripMenuItem itmOptionsRememberWinPos;
        private Controls.SplitButton btnRename;
        private System.Windows.Forms.ContextMenuStrip cmsRename;
        private System.Windows.Forms.ToolStripMenuItem itmRenameFiles;
        private System.Windows.Forms.ToolStripMenuItem itmRenameFolders;
        private System.Windows.Forms.Label lblNumMatched;
        private System.Windows.Forms.Label lblNumConflict;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.Label lblStatsHidden;
        private System.Windows.Forms.Label lblStatsShown;
        private System.Windows.Forms.Label lblStatsFiltered;
        private System.Windows.Forms.Label lblStatsTotal;
        private System.Windows.Forms.ToolStripMenuItem itmOptionsRealtimePreview;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.ToolStripMenuItem itmOptionsAddContextMenu;
        private System.Windows.Forms.ToolStripMenuItem itmOptionsRenameSelectedRows;
        private System.Windows.Forms.GroupBox groupBoxTop;
        private System.Windows.Forms.GroupBox groupBoxFolderTree;
        private System.Windows.Forms.GroupBox groupBoxFileView;
        private System.Windows.Forms.ContextMenuStrip cmFileView;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmFolderView;
        private System.Windows.Forms.ToolStripMenuItem setAsKavitaLibraryRootToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openInExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyPathToolStripMenuItem;
        private System.Windows.Forms.DataGridViewImageColumn colIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilename;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPreview;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeries;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEdition;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpecial;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem explorerContextMenuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem explorerContextMenuToolStripMenuItem1;
        private System.Windows.Forms.ToolStripDropDownButton mnuKavitaCheck;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem previewComicsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewMangaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewBooksToolStripMenuItem;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblInfoFileSize;
    }
}

