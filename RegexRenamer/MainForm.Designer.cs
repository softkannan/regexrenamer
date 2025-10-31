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


using RegexRenamer.Controls.FolderTreeViewCtrl;
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
            tvwFolders = new FolderTreeView();
            btnRename = new RegexRenamer.Controls.SplitButton();
            cmsRename = new System.Windows.Forms.ContextMenuStrip(components);
            renameFilesCMSRenameItem = new System.Windows.Forms.ToolStripMenuItem();
            renameFoldersCMSRenameItem = new System.Windows.Forms.ToolStripMenuItem();
            cmbReplace = new RegexRenamer.Controls.MyComboBox();
            cmbMatch = new RegexRenamer.Controls.MyComboBox();
            gbFilter = new System.Windows.Forms.GroupBox();
            lblInfoFileSize = new System.Windows.Forms.Label();
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
            chkShowInfo = new System.Windows.Forms.CheckBox();
            chkOrderByReverse = new System.Windows.Forms.CheckBox();
            chkRenameSelectionOnly = new System.Windows.Forms.CheckBox();
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
            mnuMoveCopy = new System.Windows.Forms.ToolStripDropDownButton();
            itmOutputRenameInPlace = new System.Windows.Forms.ToolStripMenuItem();
            itmOutputSep = new System.Windows.Forms.ToolStripSeparator();
            itmOutputMoveTo = new System.Windows.Forms.ToolStripMenuItem();
            itmOutputCopyTo = new System.Windows.Forms.ToolStripMenuItem();
            itmOutputBackupTo = new System.Windows.Forms.ToolStripMenuItem();
            mnuKavitaCheck = new System.Windows.Forms.ToolStripDropDownButton();
            useMetadataKavitaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            noneKavitaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            previewComicsKavitaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            previewMangaKavitaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            previewBooksKavitaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ttPreviewError = new System.Windows.Forms.ToolTip(components);
            fbdNetwork = new System.Windows.Forms.FolderBrowserDialog();
            fbdMoveCopy = new System.Windows.Forms.FolderBrowserDialog();
            lblPath = new System.Windows.Forms.Label();
            dgvFiles = new System.Windows.Forms.DataGridView();
            colIcon = new System.Windows.Forms.DataGridViewImageColumn();
            colFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPreview = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colExt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colFileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSeries = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colChapter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEdition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSpecial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            tsOptions = new System.Windows.Forms.ToolStrip();
            mnuOptions = new System.Windows.Forms.ToolStripDropDownButton();
            itmOptionsShowHidden = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsPreserveExt = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsRealtimePreview = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsAllowRenSub = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsRememberWinPos = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsAddContextMenu = new System.Windows.Forms.ToolStripMenuItem();
            itmOptionsEditConfig = new System.Windows.Forms.ToolStripMenuItem();
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
            groupBoxFolderTree = new System.Windows.Forms.GroupBox();
            groupBoxFileView = new System.Windows.Forms.GroupBox();
            cmsRename.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlStats.SuspendLayout();
            tsMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFiles).BeginInit();
            tsOptions.SuspendLayout();
            groupBoxTop.SuspendLayout();
            groupBoxFolderTree.SuspendLayout();
            groupBoxFileView.SuspendLayout();
            SuspendLayout();
            // 
            // tvwFolders
            // 
            tvwFolders.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tvwFolders.HideSelection = false;
            tvwFolders.Location = new System.Drawing.Point(10, 16);
            tvwFolders.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tvwFolders.Name = "tvwFolders";
            tvwFolders.Size = new System.Drawing.Size(426, 671);
            tvwFolders.TabIndex = 1;
            // 
            // btnRename
            // 
            btnRename.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnRename.AutoSize = true;
            btnRename.ContextMenuStrip = cmsRename;
            btnRename.Location = new System.Drawing.Point(892, 692);
            btnRename.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnRename.Name = "btnRename";
            btnRename.Size = new System.Drawing.Size(148, 34);
            btnRename.TabIndex = 3;
            btnRename.Text = "&Rename";
            // 
            // cmsRename
            // 
            cmsRename.AutoSize = false;
            cmsRename.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { renameFilesCMSRenameItem, renameFoldersCMSRenameItem });
            cmsRename.Name = "cmsRename";
            cmsRename.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            cmsRename.Size = new System.Drawing.Size(200, 94);
            // 
            // renameFilesCMSRenameItem
            // 
            renameFilesCMSRenameItem.AutoSize = false;
            renameFilesCMSRenameItem.Checked = true;
            renameFilesCMSRenameItem.CheckState = System.Windows.Forms.CheckState.Checked;
            renameFilesCMSRenameItem.Name = "renameFilesCMSRenameItem";
            renameFilesCMSRenameItem.Size = new System.Drawing.Size(129, 22);
            renameFilesCMSRenameItem.Text = "Rename files";
            // 
            // renameFoldersCMSRenameItem
            // 
            renameFoldersCMSRenameItem.AutoSize = false;
            renameFoldersCMSRenameItem.Name = "renameFoldersCMSRenameItem";
            renameFoldersCMSRenameItem.Size = new System.Drawing.Size(129, 22);
            renameFoldersCMSRenameItem.Text = "Rename folders";
            // 
            // cmbReplace
            // 
            cmbReplace.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbReplace.AutoCompleteCustomSource.AddRange(new string[] { "$1" });
            cmbReplace.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbReplace.Location = new System.Drawing.Point(72, 56);
            cmbReplace.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbReplace.Name = "cmbReplace";
            cmbReplace.Size = new System.Drawing.Size(792, 23);
            cmbReplace.TabIndex = 1;
            toolTip.SetToolTip(cmbReplace, "Use $1, $2, ... to insert captured text");
            // 
            // cmbMatch
            // 
            cmbMatch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbMatch.AutoCompleteCustomSource.AddRange(new string[] { "(.+)", "(.+)(/d+)" });
            cmbMatch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbMatch.Location = new System.Drawing.Point(72, 26);
            cmbMatch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbMatch.Name = "cmbMatch";
            cmbMatch.Size = new System.Drawing.Size(792, 23);
            cmbMatch.TabIndex = 0;
            toolTip.SetToolTip(cmbMatch, "Shift+rightclick for a menu of regex elements");
            // 
            // gbFilter
            // 
            gbFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            gbFilter.Controls.Add(lblInfoFileSize);
            gbFilter.Controls.Add(cbFilterExclude);
            gbFilter.Controls.Add(txtFilter);
            gbFilter.Controls.Add(rbFilterGlob);
            gbFilter.Controls.Add(rbFilterRegex);
            gbFilter.Location = new System.Drawing.Point(1305, 8);
            gbFilter.Margin = new System.Windows.Forms.Padding(4);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new System.Windows.Forms.Padding(4);
            gbFilter.Size = new System.Drawing.Size(176, 83);
            gbFilter.TabIndex = 2;
            gbFilter.TabStop = false;
            gbFilter.Text = "Filter";
            // 
            // lblInfoFileSize
            // 
            lblInfoFileSize.AutoSize = true;
            lblInfoFileSize.Location = new System.Drawing.Point(8, 64);
            lblInfoFileSize.Name = "lblInfoFileSize";
            lblInfoFileSize.Size = new System.Drawing.Size(77, 15);
            lblInfoFileSize.TabIndex = 0;
            lblInfoFileSize.Text = "FileSize : 0 Kb";
            // 
            // cbFilterExclude
            // 
            cbFilterExclude.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbFilterExclude.Appearance = System.Windows.Forms.Appearance.Button;
            cbFilterExclude.ForeColor = System.Drawing.Color.Red;
            cbFilterExclude.Location = new System.Drawing.Point(154, 17);
            cbFilterExclude.Margin = new System.Windows.Forms.Padding(0);
            cbFilterExclude.Name = "cbFilterExclude";
            cbFilterExclude.Size = new System.Drawing.Size(14, 23);
            cbFilterExclude.TabIndex = 2;
            cbFilterExclude.Text = "!";
            cbFilterExclude.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(cbFilterExclude, "Exclude (invert filter)");
            // 
            // txtFilter
            // 
            txtFilter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            txtFilter.Location = new System.Drawing.Point(8, 19);
            txtFilter.Margin = new System.Windows.Forms.Padding(4);
            txtFilter.Name = "txtFilter";
            txtFilter.Size = new System.Drawing.Size(143, 23);
            txtFilter.TabIndex = 1;
            txtFilter.Text = "*.*";
            toolTip.SetToolTip(txtFilter, "Press ENTER to apply filter");
            // 
            // rbFilterGlob
            // 
            rbFilterGlob.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            rbFilterGlob.AutoSize = true;
            rbFilterGlob.Checked = true;
            rbFilterGlob.Location = new System.Drawing.Point(12, 43);
            rbFilterGlob.Margin = new System.Windows.Forms.Padding(4);
            rbFilterGlob.Name = "rbFilterGlob";
            rbFilterGlob.Size = new System.Drawing.Size(50, 19);
            rbFilterGlob.TabIndex = 3;
            rbFilterGlob.TabStop = true;
            rbFilterGlob.Text = "Glob";
            // 
            // rbFilterRegex
            // 
            rbFilterRegex.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            rbFilterRegex.AutoSize = true;
            rbFilterRegex.Location = new System.Drawing.Point(94, 43);
            rbFilterRegex.Margin = new System.Windows.Forms.Padding(4);
            rbFilterRegex.Name = "rbFilterRegex";
            rbFilterRegex.Size = new System.Drawing.Size(57, 19);
            rbFilterRegex.TabIndex = 3;
            rbFilterRegex.Text = "Regex";
            // 
            // pnlStats
            // 
            pnlStats.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            pnlStats.Controls.Add(lblStatsHidden);
            pnlStats.Controls.Add(lblStatsShown);
            pnlStats.Controls.Add(lblStatsFiltered);
            pnlStats.Controls.Add(lblStatsTotal);
            pnlStats.Location = new System.Drawing.Point(1190, 15);
            pnlStats.Margin = new System.Windows.Forms.Padding(4);
            pnlStats.Name = "pnlStats";
            pnlStats.Size = new System.Drawing.Size(108, 76);
            pnlStats.TabIndex = 6;
            // 
            // lblStatsHidden
            // 
            lblStatsHidden.AutoEllipsis = true;
            lblStatsHidden.Location = new System.Drawing.Point(4, 38);
            lblStatsHidden.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStatsHidden.Name = "lblStatsHidden";
            lblStatsHidden.Size = new System.Drawing.Size(99, 14);
            lblStatsHidden.TabIndex = 4;
            lblStatsHidden.Text = "0 hidden";
            // 
            // lblStatsShown
            // 
            lblStatsShown.AutoEllipsis = true;
            lblStatsShown.Location = new System.Drawing.Point(4, 20);
            lblStatsShown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStatsShown.Name = "lblStatsShown";
            lblStatsShown.Size = new System.Drawing.Size(99, 14);
            lblStatsShown.TabIndex = 3;
            lblStatsShown.Text = "0 shown";
            // 
            // lblStatsFiltered
            // 
            lblStatsFiltered.AutoEllipsis = true;
            lblStatsFiltered.Location = new System.Drawing.Point(4, 55);
            lblStatsFiltered.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStatsFiltered.Name = "lblStatsFiltered";
            lblStatsFiltered.Size = new System.Drawing.Size(99, 14);
            lblStatsFiltered.TabIndex = 2;
            lblStatsFiltered.Text = "0 filtered";
            // 
            // lblStatsTotal
            // 
            lblStatsTotal.AutoEllipsis = true;
            lblStatsTotal.Location = new System.Drawing.Point(5, 4);
            lblStatsTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStatsTotal.Name = "lblStatsTotal";
            lblStatsTotal.Size = new System.Drawing.Size(99, 14);
            lblStatsTotal.TabIndex = 1;
            lblStatsTotal.Text = "0 total";
            // 
            // lblStats
            // 
            lblStats.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblStats.Location = new System.Drawing.Point(1423, 16);
            lblStats.Margin = new System.Windows.Forms.Padding(0);
            lblStats.Name = "lblStats";
            lblStats.Size = new System.Drawing.Size(36, 15);
            lblStats.TabIndex = 4;
            lblStats.Text = "Stats";
            // 
            // lblMatch
            // 
            lblMatch.AutoSize = true;
            lblMatch.Location = new System.Drawing.Point(22, 28);
            lblMatch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMatch.Name = "lblMatch";
            lblMatch.Size = new System.Drawing.Size(44, 15);
            lblMatch.TabIndex = 0;
            lblMatch.Text = "Match:";
            // 
            // lblReplace
            // 
            lblReplace.AutoSize = true;
            lblReplace.Location = new System.Drawing.Point(10, 58);
            lblReplace.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblReplace.Name = "lblReplace";
            lblReplace.Size = new System.Drawing.Size(51, 15);
            lblReplace.TabIndex = 0;
            lblReplace.Text = "Replace:";
            // 
            // cbModifierI
            // 
            cbModifierI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbModifierI.AutoSize = true;
            cbModifierI.Location = new System.Drawing.Point(871, 21);
            cbModifierI.Margin = new System.Windows.Forms.Padding(4);
            cbModifierI.Name = "cbModifierI";
            cbModifierI.Size = new System.Drawing.Size(34, 19);
            cbModifierI.TabIndex = 3;
            cbModifierI.Tag = false;
            cbModifierI.Text = "/i";
            toolTip.SetToolTip(cbModifierI, "Ignore case");
            // 
            // cbModifierG
            // 
            cbModifierG.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbModifierG.AutoSize = true;
            cbModifierG.Location = new System.Drawing.Point(871, 41);
            cbModifierG.Margin = new System.Windows.Forms.Padding(4);
            cbModifierG.Name = "cbModifierG";
            cbModifierG.Size = new System.Drawing.Size(38, 19);
            cbModifierG.TabIndex = 4;
            cbModifierG.Tag = false;
            cbModifierG.Text = "/g";
            toolTip.SetToolTip(cbModifierG, "Global (match as many times as possible)");
            // 
            // cbModifierX
            // 
            cbModifierX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbModifierX.AutoSize = true;
            cbModifierX.Location = new System.Drawing.Point(871, 61);
            cbModifierX.Margin = new System.Windows.Forms.Padding(4);
            cbModifierX.Name = "cbModifierX";
            cbModifierX.Size = new System.Drawing.Size(37, 19);
            cbModifierX.TabIndex = 5;
            cbModifierX.Tag = false;
            cbModifierX.Text = "/x";
            toolTip.SetToolTip(cbModifierX, "Extended regex (ignore unescaped spaces)");
            // 
            // btnNetwork
            // 
            btnNetwork.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnNetwork.Image = (System.Drawing.Image)resources.GetObject("btnNetwork.Image");
            btnNetwork.Location = new System.Drawing.Point(386, 693);
            btnNetwork.Margin = new System.Windows.Forms.Padding(4);
            btnNetwork.Name = "btnNetwork";
            btnNetwork.Size = new System.Drawing.Size(42, 28);
            btnNetwork.TabIndex = 3;
            toolTip.SetToolTip(btnNetwork, "Browse network");
            // 
            // txtPath
            // 
            txtPath.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtPath.Location = new System.Drawing.Point(44, 698);
            txtPath.Margin = new System.Windows.Forms.Padding(4);
            txtPath.Name = "txtPath";
            txtPath.Size = new System.Drawing.Size(336, 23);
            txtPath.TabIndex = 2;
            toolTip.SetToolTip(txtPath, "Press ENTER to apply path");
            // 
            // lblNumMatched
            // 
            lblNumMatched.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblNumMatched.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lblNumMatched.ForeColor = System.Drawing.Color.Blue;
            lblNumMatched.Location = new System.Drawing.Point(795, 700);
            lblNumMatched.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblNumMatched.Name = "lblNumMatched";
            lblNumMatched.Size = new System.Drawing.Size(41, 19);
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
            lblNumConflict.Location = new System.Drawing.Point(843, 700);
            lblNumConflict.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblNumConflict.Name = "lblNumConflict";
            lblNumConflict.Size = new System.Drawing.Size(41, 19);
            lblNumConflict.TabIndex = 5;
            lblNumConflict.Text = "0";
            lblNumConflict.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            toolTip.SetToolTip(lblNumConflict, "Number of conflicts");
            // 
            // chkShowInfo
            // 
            chkShowInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            chkShowInfo.AutoSize = true;
            chkShowInfo.Location = new System.Drawing.Point(913, 21);
            chkShowInfo.Margin = new System.Windows.Forms.Padding(4);
            chkShowInfo.Name = "chkShowInfo";
            chkShowInfo.Size = new System.Drawing.Size(79, 19);
            chkShowInfo.TabIndex = 7;
            chkShowInfo.Tag = false;
            chkShowInfo.Text = "Show Info";
            toolTip.SetToolTip(chkShowInfo, "Extended regex (ignore unescaped spaces)");
            // 
            // chkOrderByReverse
            // 
            chkOrderByReverse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            chkOrderByReverse.AutoSize = true;
            chkOrderByReverse.Location = new System.Drawing.Point(913, 41);
            chkOrderByReverse.Margin = new System.Windows.Forms.Padding(4);
            chkOrderByReverse.Name = "chkOrderByReverse";
            chkOrderByReverse.Size = new System.Drawing.Size(115, 19);
            chkOrderByReverse.TabIndex = 8;
            chkOrderByReverse.Tag = false;
            chkOrderByReverse.Text = "Order By Reverse";
            toolTip.SetToolTip(chkOrderByReverse, "Extended regex (ignore unescaped spaces)");
            // 
            // chkRenameSelectionOnly
            // 
            chkRenameSelectionOnly.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            chkRenameSelectionOnly.AutoSize = true;
            chkRenameSelectionOnly.Location = new System.Drawing.Point(913, 60);
            chkRenameSelectionOnly.Margin = new System.Windows.Forms.Padding(4);
            chkRenameSelectionOnly.Name = "chkRenameSelectionOnly";
            chkRenameSelectionOnly.Size = new System.Drawing.Size(148, 19);
            chkRenameSelectionOnly.TabIndex = 9;
            chkRenameSelectionOnly.Tag = false;
            chkRenameSelectionOnly.Text = "Rename Selection Only";
            toolTip.SetToolTip(chkRenameSelectionOnly, "Extended regex (ignore unescaped spaces)");
            // 
            // tsMenu
            // 
            tsMenu.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tsMenu.CanOverflow = false;
            tsMenu.Dock = System.Windows.Forms.DockStyle.None;
            tsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            tsMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuChangeCase, mnuNumbering, mnuMoveCopy, mnuKavitaCheck });
            tsMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            tsMenu.Location = new System.Drawing.Point(1074, 14);
            tsMenu.Name = "tsMenu";
            tsMenu.Size = new System.Drawing.Size(113, 82);
            tsMenu.TabIndex = 1;
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
            // mnuMoveCopy
            // 
            mnuMoveCopy.AutoToolTip = false;
            mnuMoveCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuMoveCopy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { itmOutputRenameInPlace, itmOutputSep, itmOutputMoveTo, itmOutputCopyTo, itmOutputBackupTo });
            mnuMoveCopy.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            mnuMoveCopy.Name = "mnuMoveCopy";
            mnuMoveCopy.Padding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            mnuMoveCopy.Size = new System.Drawing.Size(111, 19);
            mnuMoveCopy.Text = "Move/Copy";
            mnuMoveCopy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // itmOutputRenameInPlace
            // 
            itmOutputRenameInPlace.Checked = true;
            itmOutputRenameInPlace.CheckState = System.Windows.Forms.CheckState.Checked;
            itmOutputRenameInPlace.Name = "itmOutputRenameInPlace";
            itmOutputRenameInPlace.Size = new System.Drawing.Size(161, 22);
            itmOutputRenameInPlace.Text = "Rename in place";
            // 
            // itmOutputSep
            // 
            itmOutputSep.Name = "itmOutputSep";
            itmOutputSep.Size = new System.Drawing.Size(158, 6);
            // 
            // itmOutputMoveTo
            // 
            itmOutputMoveTo.Name = "itmOutputMoveTo";
            itmOutputMoveTo.Size = new System.Drawing.Size(161, 22);
            itmOutputMoveTo.Text = "Move to...";
            itmOutputMoveTo.ToolTipText = "Files that match are moved and renamed";
            // 
            // itmOutputCopyTo
            // 
            itmOutputCopyTo.Name = "itmOutputCopyTo";
            itmOutputCopyTo.Size = new System.Drawing.Size(161, 22);
            itmOutputCopyTo.Text = "Copy to...";
            itmOutputCopyTo.ToolTipText = "Files that match are copied and the copies are renamed";
            // 
            // itmOutputBackupTo
            // 
            itmOutputBackupTo.Name = "itmOutputBackupTo";
            itmOutputBackupTo.Size = new System.Drawing.Size(161, 22);
            itmOutputBackupTo.Text = "Backup to...";
            itmOutputBackupTo.ToolTipText = "Files that match are copied and the originals are renamed";
            // 
            // mnuKavitaCheck
            // 
            mnuKavitaCheck.AutoToolTip = false;
            mnuKavitaCheck.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuKavitaCheck.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { useMetadataKavitaMenuItem, noneKavitaMenuItem, toolStripSeparator1, previewComicsKavitaMenuItem, previewMangaKavitaMenuItem, previewBooksKavitaMenuItem });
            mnuKavitaCheck.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            mnuKavitaCheck.Name = "mnuKavitaCheck";
            mnuKavitaCheck.Padding = new System.Windows.Forms.Padding(0, 0, 50, 0);
            mnuKavitaCheck.Size = new System.Drawing.Size(111, 19);
            mnuKavitaCheck.Text = "Kavita";
            mnuKavitaCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            mnuKavitaCheck.ToolTipText = "Preview Kavitha Parsed Values";
            // 
            // useMetadataKavitaMenuItem
            // 
            useMetadataKavitaMenuItem.Name = "useMetadataKavitaMenuItem";
            useMetadataKavitaMenuItem.Size = new System.Drawing.Size(158, 22);
            useMetadataKavitaMenuItem.Text = "Use Metadata";
            // 
            // noneKavitaMenuItem
            // 
            noneKavitaMenuItem.Checked = true;
            noneKavitaMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            noneKavitaMenuItem.Name = "noneKavitaMenuItem";
            noneKavitaMenuItem.Size = new System.Drawing.Size(158, 22);
            noneKavitaMenuItem.Text = "None";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(155, 6);
            // 
            // previewComicsKavitaMenuItem
            // 
            previewComicsKavitaMenuItem.Name = "previewComicsKavitaMenuItem";
            previewComicsKavitaMenuItem.Size = new System.Drawing.Size(158, 22);
            previewComicsKavitaMenuItem.Text = "Preview Comics";
            // 
            // previewMangaKavitaMenuItem
            // 
            previewMangaKavitaMenuItem.Name = "previewMangaKavitaMenuItem";
            previewMangaKavitaMenuItem.Size = new System.Drawing.Size(158, 22);
            previewMangaKavitaMenuItem.Text = "Preview Manga";
            // 
            // previewBooksKavitaMenuItem
            // 
            previewBooksKavitaMenuItem.Name = "previewBooksKavitaMenuItem";
            previewBooksKavitaMenuItem.Size = new System.Drawing.Size(158, 22);
            previewBooksKavitaMenuItem.Text = "Preview Books";
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
            // lblPath
            // 
            lblPath.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblPath.AutoSize = true;
            lblPath.Location = new System.Drawing.Point(4, 701);
            lblPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPath.Name = "lblPath";
            lblPath.Size = new System.Drawing.Size(34, 15);
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
            dgvFiles.ColumnHeadersHeight = 30;
            dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colIcon, colFilename, colPreview, colExt, colFileSize, colModified, colTitle, colSeries, colVolume, colChapter, colEdition, colSpecial });
            dgvFiles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            dgvFiles.GridColor = System.Drawing.SystemColors.Control;
            dgvFiles.Location = new System.Drawing.Point(6, 15);
            dgvFiles.Margin = new System.Windows.Forms.Padding(4);
            dgvFiles.Name = "dgvFiles";
            dgvFiles.RowHeadersVisible = false;
            dgvFiles.RowHeadersWidth = 50;
            dgvFiles.RowTemplate.Height = 22;
            dgvFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvFiles.ShowCellToolTips = false;
            dgvFiles.Size = new System.Drawing.Size(1034, 670);
            dgvFiles.StandardTab = true;
            dgvFiles.TabIndex = 6;
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
            // colExt
            // 
            colExt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            colExt.HeaderText = "Ext";
            colExt.Name = "colExt";
            colExt.ReadOnly = true;
            colExt.Visible = false;
            // 
            // colFileSize
            // 
            colFileSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colFileSize.HeaderText = "Size";
            colFileSize.Name = "colFileSize";
            colFileSize.ReadOnly = true;
            colFileSize.Visible = false;
            // 
            // colModified
            // 
            colModified.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colModified.HeaderText = "Modified";
            colModified.MinimumWidth = 10;
            colModified.Name = "colModified";
            colModified.ReadOnly = true;
            colModified.Visible = false;
            // 
            // colTitle
            // 
            colTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colTitle.HeaderText = "Title";
            colTitle.MinimumWidth = 6;
            colTitle.Name = "colTitle";
            colTitle.ReadOnly = true;
            colTitle.Visible = false;
            // 
            // colSeries
            // 
            colSeries.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            colSeries.HeaderText = "Series";
            colSeries.MinimumWidth = 6;
            colSeries.Name = "colSeries";
            colSeries.ReadOnly = true;
            colSeries.Visible = false;
            // 
            // colVolume
            // 
            colVolume.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            colVolume.HeaderText = "Volume";
            colVolume.MinimumWidth = 6;
            colVolume.Name = "colVolume";
            colVolume.ReadOnly = true;
            colVolume.Visible = false;
            // 
            // colChapter
            // 
            colChapter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            colChapter.HeaderText = "Chapters";
            colChapter.MinimumWidth = 6;
            colChapter.Name = "colChapter";
            colChapter.ReadOnly = true;
            colChapter.Visible = false;
            // 
            // colEdition
            // 
            colEdition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            colEdition.HeaderText = "Edition";
            colEdition.MinimumWidth = 6;
            colEdition.Name = "colEdition";
            colEdition.ReadOnly = true;
            colEdition.Visible = false;
            // 
            // colSpecial
            // 
            colSpecial.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            colSpecial.HeaderText = "Special";
            colSpecial.MinimumWidth = 6;
            colSpecial.Name = "colSpecial";
            colSpecial.ReadOnly = true;
            colSpecial.Visible = false;
            // 
            // tsOptions
            // 
            tsOptions.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            tsOptions.CanOverflow = false;
            tsOptions.Dock = System.Windows.Forms.DockStyle.None;
            tsOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            tsOptions.ImageScalingSize = new System.Drawing.Size(20, 20);
            tsOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuOptions, mnuHelp });
            tsOptions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            tsOptions.Location = new System.Drawing.Point(6, 700);
            tsOptions.Name = "tsOptions";
            tsOptions.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            tsOptions.Size = new System.Drawing.Size(149, 20);
            tsOptions.TabIndex = 2;
            tsOptions.TabStop = true;
            // 
            // mnuOptions
            // 
            mnuOptions.AutoToolTip = false;
            mnuOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { itmOptionsShowHidden, itmOptionsPreserveExt, itmOptionsRealtimePreview, itmOptionsAllowRenSub, itmOptionsRememberWinPos, itmOptionsAddContextMenu, itmOptionsEditConfig });
            mnuOptions.Margin = new System.Windows.Forms.Padding(0, 1, 10, 0);
            mnuOptions.Name = "mnuOptions";
            mnuOptions.Size = new System.Drawing.Size(62, 19);
            mnuOptions.Text = "Options";
            mnuOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // itmOptionsShowHidden
            // 
            itmOptionsShowHidden.CheckOnClick = true;
            itmOptionsShowHidden.Name = "itmOptionsShowHidden";
            itmOptionsShowHidden.Size = new System.Drawing.Size(223, 22);
            itmOptionsShowHidden.Text = "Show hidden files";
            // 
            // itmOptionsPreserveExt
            // 
            itmOptionsPreserveExt.CheckOnClick = true;
            itmOptionsPreserveExt.Name = "itmOptionsPreserveExt";
            itmOptionsPreserveExt.Size = new System.Drawing.Size(223, 22);
            itmOptionsPreserveExt.Text = "Preserve file extension";
            // 
            // itmOptionsRealtimePreview
            // 
            itmOptionsRealtimePreview.Checked = true;
            itmOptionsRealtimePreview.CheckOnClick = true;
            itmOptionsRealtimePreview.CheckState = System.Windows.Forms.CheckState.Checked;
            itmOptionsRealtimePreview.Name = "itmOptionsRealtimePreview";
            itmOptionsRealtimePreview.Size = new System.Drawing.Size(223, 22);
            itmOptionsRealtimePreview.Text = "Enable realtime preview";
            itmOptionsRealtimePreview.ToolTipText = "When unchecked, press ENTER in the regex fields to update the preview";
            // 
            // itmOptionsAllowRenSub
            // 
            itmOptionsAllowRenSub.CheckOnClick = true;
            itmOptionsAllowRenSub.Name = "itmOptionsAllowRenSub";
            itmOptionsAllowRenSub.Size = new System.Drawing.Size(223, 22);
            itmOptionsAllowRenSub.Text = "Allow rename to subfolders";
            // 
            // itmOptionsRememberWinPos
            // 
            itmOptionsRememberWinPos.Checked = true;
            itmOptionsRememberWinPos.CheckOnClick = true;
            itmOptionsRememberWinPos.CheckState = System.Windows.Forms.CheckState.Checked;
            itmOptionsRememberWinPos.Name = "itmOptionsRememberWinPos";
            itmOptionsRememberWinPos.Size = new System.Drawing.Size(223, 22);
            itmOptionsRememberWinPos.Text = "Remember window position";
            // 
            // itmOptionsAddContextMenu
            // 
            itmOptionsAddContextMenu.Name = "itmOptionsAddContextMenu";
            itmOptionsAddContextMenu.Size = new System.Drawing.Size(223, 22);
            itmOptionsAddContextMenu.Text = "Add explorer context menu";
            // 
            // itmOptionsEditConfig
            // 
            itmOptionsEditConfig.Name = "itmOptionsEditConfig";
            itmOptionsEditConfig.Size = new System.Drawing.Size(223, 22);
            itmOptionsEditConfig.Text = "Edit Config";
            // 
            // mnuHelp
            // 
            mnuHelp.AutoToolTip = false;
            mnuHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { itmHelpContents, itmHelpRegexReference, itmHelpSep1, itmHelpEmailAuthor, itmHelpReportBug, itmHelpHomepage, itmHelpSep2, itmHelpAbout });
            mnuHelp.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            mnuHelp.Name = "mnuHelp";
            mnuHelp.Size = new System.Drawing.Size(45, 19);
            mnuHelp.Text = "Help";
            mnuHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // itmHelpContents
            // 
            itmHelpContents.Name = "itmHelpContents";
            itmHelpContents.ShortcutKeys = System.Windows.Forms.Keys.F1;
            itmHelpContents.Size = new System.Drawing.Size(212, 22);
            itmHelpContents.Text = "Contents";
            // 
            // itmHelpRegexReference
            // 
            itmHelpRegexReference.Name = "itmHelpRegexReference";
            itmHelpRegexReference.ShortcutKeys = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F1;
            itmHelpRegexReference.Size = new System.Drawing.Size(212, 22);
            itmHelpRegexReference.Text = "Regex Reference";
            // 
            // itmHelpSep1
            // 
            itmHelpSep1.Name = "itmHelpSep1";
            itmHelpSep1.Size = new System.Drawing.Size(209, 6);
            // 
            // itmHelpEmailAuthor
            // 
            itmHelpEmailAuthor.Name = "itmHelpEmailAuthor";
            itmHelpEmailAuthor.Size = new System.Drawing.Size(212, 22);
            itmHelpEmailAuthor.Text = "Email the author";
            // 
            // itmHelpReportBug
            // 
            itmHelpReportBug.Name = "itmHelpReportBug";
            itmHelpReportBug.Size = new System.Drawing.Size(212, 22);
            itmHelpReportBug.Text = "Report a bug";
            // 
            // itmHelpHomepage
            // 
            itmHelpHomepage.Name = "itmHelpHomepage";
            itmHelpHomepage.Size = new System.Drawing.Size(212, 22);
            itmHelpHomepage.Text = "Homepage";
            // 
            // itmHelpSep2
            // 
            itmHelpSep2.Name = "itmHelpSep2";
            itmHelpSep2.Size = new System.Drawing.Size(209, 6);
            // 
            // itmHelpAbout
            // 
            itmHelpAbout.Name = "itmHelpAbout";
            itmHelpAbout.Size = new System.Drawing.Size(212, 22);
            itmHelpAbout.Text = "About RegexRenamer";
            // 
            // progressBar
            // 
            progressBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar.Location = new System.Drawing.Point(163, 698);
            progressBar.Margin = new System.Windows.Forms.Padding(4);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(619, 26);
            progressBar.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Enabled = false;
            btnCancel.Location = new System.Drawing.Point(892, 692);
            btnCancel.Margin = new System.Windows.Forms.Padding(4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(148, 34);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "&Cancel";
            btnCancel.Visible = false;
            // 
            // bgwRename
            // 
            bgwRename.WorkerReportsProgress = true;
            bgwRename.WorkerSupportsCancellation = true;
            // 
            // groupBoxTop
            // 
            groupBoxTop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBoxTop.Controls.Add(chkRenameSelectionOnly);
            groupBoxTop.Controls.Add(chkOrderByReverse);
            groupBoxTop.Controls.Add(chkShowInfo);
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
            groupBoxTop.Location = new System.Drawing.Point(10, 2);
            groupBoxTop.Name = "groupBoxTop";
            groupBoxTop.Size = new System.Drawing.Size(1487, 98);
            groupBoxTop.TabIndex = 7;
            groupBoxTop.TabStop = false;
            // 
            // groupBoxFolderTree
            // 
            groupBoxFolderTree.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            groupBoxFolderTree.Controls.Add(txtPath);
            groupBoxFolderTree.Controls.Add(lblPath);
            groupBoxFolderTree.Controls.Add(tvwFolders);
            groupBoxFolderTree.Controls.Add(btnNetwork);
            groupBoxFolderTree.Location = new System.Drawing.Point(10, 106);
            groupBoxFolderTree.Name = "groupBoxFolderTree";
            groupBoxFolderTree.Size = new System.Drawing.Size(441, 733);
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
            groupBoxFileView.Location = new System.Drawing.Point(457, 106);
            groupBoxFileView.Name = "groupBoxFileView";
            groupBoxFileView.Size = new System.Drawing.Size(1048, 732);
            groupBoxFileView.TabIndex = 9;
            groupBoxFileView.TabStop = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1516, 849);
            Controls.Add(groupBoxFileView);
            Controls.Add(groupBoxFolderTree);
            Controls.Add(groupBoxTop);
            Icon = Properties.Resources.icon;
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4);
            MinimumSize = new System.Drawing.Size(624, 337);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "RegexRenamer";
            cmsRename.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlStats.ResumeLayout(false);
            tsMenu.ResumeLayout(false);
            tsMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFiles).EndInit();
            tsOptions.ResumeLayout(false);
            tsOptions.PerformLayout();
            groupBoxTop.ResumeLayout(false);
            groupBoxTop.PerformLayout();
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
        private FolderTreeView tvwFolders;
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
        private System.Windows.Forms.Button btnCancel;
        private System.ComponentModel.BackgroundWorker bgwRename;
        private System.Windows.Forms.ToolStripMenuItem itmOptionsRememberWinPos;
        private Controls.SplitButton btnRename;
        private System.Windows.Forms.ContextMenuStrip cmsRename;
        private System.Windows.Forms.ToolStripMenuItem renameFilesCMSRenameItem;
        private System.Windows.Forms.ToolStripMenuItem renameFoldersCMSRenameItem;
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
        private System.Windows.Forms.GroupBox groupBoxTop;
        private System.Windows.Forms.GroupBox groupBoxFolderTree;
        private System.Windows.Forms.GroupBox groupBoxFileView;
        private System.Windows.Forms.ToolStripDropDownButton mnuKavitaCheck;
        private System.Windows.Forms.ToolStripMenuItem noneKavitaMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem previewComicsKavitaMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewMangaKavitaMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewBooksKavitaMenuItem;
        private System.Windows.Forms.Label lblInfoFileSize;
        private System.Windows.Forms.ToolStripMenuItem itmChangeCaseCleanName;
        private System.Windows.Forms.ToolStripMenuItem useMetadataKavitaMenuItem;
        private System.Windows.Forms.CheckBox chkShowInfo;
        private System.Windows.Forms.DataGridViewImageColumn colIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilename;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPreview;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModified;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeries;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEdition;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpecial;
        private System.Windows.Forms.CheckBox chkOrderByReverse;
        private System.Windows.Forms.ToolStripMenuItem itmOptionsEditConfig;
        private System.Windows.Forms.CheckBox chkRenameSelectionOnly;
    }
}

