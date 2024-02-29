/* =============================================================================
 * RegexRenamer                                     Copyright (c) 2011 Xiperware
 * http://regexrenamer.sourceforge.net/                      xiperware@gmail.com
 * 
 * This program is free software; you can redistribute it and/or modify it under
 * the terms of the GNU General Public License v2, as published by the Free
 * Software Foundation.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * =============================================================================
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using RegexRenamer.Controls;      // ExtractIcons
using Microsoft.Win32;          // Registry
using System.Reflection;
using System.Security;
using RegexRenamer.Kavita;
using RegexRenamer.Utility;
using System.Threading;
using RegexRenamer.Native;        // FieldInfo


namespace RegexRenamer
{
    public partial class MainForm : Form
    {
        #region Consts

        private int MAX_FILES = 10000;   // file limit for filelist (was a const)
        private const int MAX_HISTORY = 20;      // number of regex history entries to keep

        #endregion

        #region Variables

        private LibraryType curLibType = LibraryType.Comic;
        private readonly Kavita.DefaultParser parser = new Kavita.DefaultParser();

        private string activePath = null;     // current path
        private string activeFilter = "*.*";  // current filter

        private List<RRItem> activeFiles = new List<RRItem>();  // files in activePath displayed in filelist
        private Dictionary<string, InactiveReason> inactiveFiles = new Dictionary<string, InactiveReason>();  // files in activePath but not displayed

        private bool validFilter = true;      // file filter is valid
        private bool validMatch = true;      // regex match expression is valid
        private bool validNumber = true;      // numbering menu options are all valid

        private int countProgLaunches = 1;    // counters for
        private int countFilesRenamed = 0;    // about-dialog stats

        private FileCount fileCount = new FileCount();  // holds file counts (total/shown/filtered/hidden)

        private About aboutForm;

        private enum InactiveReason
        {
            Hidden,
            Filtered
        }

        #endregion

        #region Properties

        // prevent setting to true if rename operation in progress

        private bool enableUpdates = true;
        private bool EnableUpdates
        {
            get
            {
                return enableUpdates;
            }
            set
            {
                if (value && bgwRename.IsBusy) return;
                enableUpdates = value;
            }
        }


        // when toggling rename folders, update menus, change strings, etc.

        private bool renameFolders = false;
        private bool RenameFolders
        {
            get
            {
                return renameFolders;
            }
            set
            {
                renameFolders = value;


                // update menu

                if (itmRenameFolders.Checked != renameFolders)
                {
                    itmRenameFiles.Checked = !renameFolders;
                    itmRenameFolders.Checked = renameFolders;
                }


                // preserve file extensions disabled when renaming folders

                itmOptionsPreserveExt.Enabled = !renameFolders;


                // can't use Copy To or Backup To while renaming folders

                itmOutputCopyTo.Enabled = itmOutputBackupTo.Enabled = !renameFolders;

                if (renameFolders && (itmOutputCopyTo.Checked || itmOutputBackupTo.Checked))
                    itmOutputRenameInPlace.PerformClick();


                // change text "file" <=> "folder"

                string oldFile = renameFolders ? "file" : "folder";
                string oldFilename = renameFolders ? "filename" : "folder name";
                string oldCapFile = renameFolders ? "File" : "Folder";

                mnuChangeCase.ToolTipText = mnuChangeCase.ToolTipText.Replace(oldFilename, strFilename);
                txtNumberingInc.ToolTipText = txtNumberingInc.ToolTipText.Replace(oldFile, strFile);
                txtNumberingReset.ToolTipText = txtNumberingReset.ToolTipText.Replace(oldFile, strFile);
                itmOutputMoveTo.ToolTipText = itmOutputMoveTo.ToolTipText.Replace(oldCapFile, strCapFile);
                itmOutputCopyTo.ToolTipText = renameFolders ? "Unavailable during folder rename" : "Files that match are copied and the copies are renamed";
                itmOutputBackupTo.ToolTipText = renameFolders ? "Unavailable during folder rename" : "Files that match are copied and the originals are renamed";
                miRegexReplaceOrigAll.Text = miRegexReplaceOrigAll.Text.Replace(oldFilename, strFilename);
                itmOptionsShowHidden.Text = itmOptionsShowHidden.Text.Replace(oldFile, strFile);
                colFilename.HeaderText = strCapFilename;
            }
        }


        // "file"/"folder" strings used throughout the program

        private string strFile
        {
            get
            {
                return RenameFolders ? "folder" : "file";
            }
        }
        private string strFilename
        {
            get
            {
                return RenameFolders ? "folder name" : "filename";
            }
        }
        private string strCapFile
        {
            get
            {
                return RenameFolders ? "Folder" : "File";
            }
        }
        private string strCapFilename
        {
            get
            {
                return RenameFolders ? "Folder name" : "Filename";
            }
        }


        // used when realtime update is disabled

        private string prevMatch;
        private string prevReplace;
        private bool PreviewNeedsUpdate
        {
            set
            {
                if (!value)  // reset
                {
                    prevMatch = cmbMatch.Text;
                    prevReplace = cmbReplace.Text;
                }
            }
            get
            {
                return cmbMatch.Text != prevMatch || cmbReplace.Text != prevReplace;
            }
        }

        #endregion

        #region Constructor

        private DarkModeCS DM =null;
        public MainForm(string initPath)
        {

            this.activePath = initPath;

            // draw form
            InitializeComponent();

            //DM = new DarkModeCS(this);
            this.ApplyTheme();

            // Render draws label and prevent borders on toolstrip
            tsMenu.Renderer = new Controls.MyToolStripSystemRenderer();
            tsOptions.Renderer = new Controls.MyToolStripSystemRenderer();


            // manually set network folder browser root to My Network Places

            //FieldInfo fieldInfo = fbdNetwork.GetType().GetField("rootFolder", BindingFlags.NonPublic | BindingFlags.Instance);
            //fieldInfo.SetValue(fbdNetwork, (Environment.SpecialFolder)0x0012);  // My Network Places

            // add insert args to regex context menu items

            miRegexMatchMatchSingleChar.Tag = new InsertArgs(".");
            miRegexMatchMatchDigit.Tag = new InsertArgs("\\d");
            miRegexMatchMatchAlpha.Tag = new InsertArgs("\\w");
            miRegexMatchMatchSpace.Tag = new InsertArgs("\\s");
            miRegexMatchMatchMultiChar.Tag = new InsertArgs(".*");
            miRegexMatchMatchNonDigit.Tag = new InsertArgs("\\D");
            miRegexMatchMatchNonAlpha.Tag = new InsertArgs("\\W");
            miRegexMatchMatchNonSpace.Tag = new InsertArgs("\\S");

            miRegexMatchAnchorStart.Tag = new InsertArgs("^", "", "group");
            miRegexMatchAnchorEnd.Tag = new InsertArgs("", "$", "group");
            miRegexMatchAnchorStartEnd.Tag = new InsertArgs("^", "$", "group");
            miRegexMatchAnchorBound.Tag = new InsertArgs("\\b", "", "wrap");
            miRegexMatchAnchorNonBound.Tag = new InsertArgs("\\B", "", "wrap");

            miRegexMatchGroupCapt.Tag = new InsertArgs("(", ")");
            miRegexMatchGroupNonCapt.Tag = new InsertArgs("(?:", ")");
            miRegexMatchGroupAlt.Tag = new InsertArgs("(", "|)", -1, 0);

            miRegexMatchQuantZeroOneG.Tag = new InsertArgs("", "?", "group");
            miRegexMatchQuantOneMoreG.Tag = new InsertArgs("", "+", "group");
            miRegexMatchQuantZeroMoreG.Tag = new InsertArgs("", "*", "group");
            miRegexMatchQuantExactG.Tag = new InsertArgs("", "{n}", -2, 1, "group");
            miRegexMatchQuantAtLeastG.Tag = new InsertArgs("", "{n,}", -3, 1, "group");
            miRegexMatchQuantBetweenG.Tag = new InsertArgs("", "{n,m}", -4, 3, "group");
            miRegexMatchQuantZeroOneL.Tag = new InsertArgs("", "??", "group");
            miRegexMatchQuantOneMoreL.Tag = new InsertArgs("", "+?", "group");
            miRegexMatchQuantZeroMoreL.Tag = new InsertArgs("", "*?", "group");
            miRegexMatchQuantExactL.Tag = new InsertArgs("", "{n}?", -3, 1, "group");
            miRegexMatchQuantAtLeastL.Tag = new InsertArgs("", "{n,}?", -4, 1, "group");
            miRegexMatchQuantBetweenL.Tag = new InsertArgs("", "{n,m}?", -5, 3, "group");

            miRegexMatchClassPos.Tag = new InsertArgs("[", "]");
            miRegexMatchClassNeg.Tag = new InsertArgs("[^", "]");
            miRegexMatchClassLower.Tag = new InsertArgs("[a-z]");
            miRegexMatchClassUpper.Tag = new InsertArgs("[A-Z]");

            miRegexMatchCaptCreateUnnamed.Tag = new InsertArgs("(", ")");
            miRegexMatchCaptMatchUnnamed.Tag = new InsertArgs("\\n", "", 1, 1);
            miRegexMatchCaptCreateNamed.Tag = new InsertArgs("(?<name>", ")", 3, 4);
            miRegexMatchCaptMatchNamed.Tag = new InsertArgs("\\<name>", "", 2, 4);

            miRegexMatchLookPosAhead.Tag = new InsertArgs("(?=", ")");
            miRegexMatchLookNegAhead.Tag = new InsertArgs("(?!", ")");
            miRegexMatchLookPosBehind.Tag = new InsertArgs("(?<=", ")");
            miRegexMatchLookNegBehind.Tag = new InsertArgs("(?<!", ")");

            miRegexMatchLiteralDot.Tag = new InsertArgs("\\.");
            miRegexMatchLiteralQuestion.Tag = new InsertArgs("\\?");
            miRegexMatchLiteralPlus.Tag = new InsertArgs("\\+");
            miRegexMatchLiteralStar.Tag = new InsertArgs("\\*");
            miRegexMatchLiteralCaret.Tag = new InsertArgs("\\^");
            miRegexMatchLiteralDollar.Tag = new InsertArgs("\\$");
            miRegexMatchLiteralBackslash.Tag = new InsertArgs("\\\\");
            miRegexMatchLiteralOpenRound.Tag = new InsertArgs("\\(");
            miRegexMatchLiteralCloseRound.Tag = new InsertArgs("\\)");
            miRegexMatchLiteralOpenSquare.Tag = new InsertArgs("\\[");
            miRegexMatchLiteralCloseSquare.Tag = new InsertArgs("\\]");
            miRegexMatchLiteralOpenCurly.Tag = new InsertArgs("\\{");
            miRegexMatchLiteralCloseCurly.Tag = new InsertArgs("\\}");
            miRegexMatchLiteralPipe.Tag = new InsertArgs("\\|");

            miRegexReplaceCaptureUnnamed.Tag = new InsertArgs("$n", "", 1, 1);
            miRegexReplaceCaptureNamed.Tag = new InsertArgs("${name}", "", 2, 4);

            miRegexReplaceOrigMatched.Tag = new InsertArgs("$0");
            miRegexReplaceOrigBefore.Tag = new InsertArgs("$`");
            miRegexReplaceOrigAfter.Tag = new InsertArgs("$'");
            miRegexReplaceOrigAll.Tag = new InsertArgs("$_");

            miRegexReplaceSpecialNumSeq.Tag = new InsertArgs("$#");
            miRegexReplaceLiteralDollar.Tag = new InsertArgs("$$");

            miGlobMatchSingle.Tag = new InsertArgs("?");
            miGlobMatchMultiple.Tag = new InsertArgs("*");
        }

        #endregion

        public void UpdateFolderTree()
        {
            EnableUpdates = false;
            tvwFolders.UpdateFolderTree(activePath);
            EnableUpdates = true;
            UpdateFileList();
        }

        #region Event Handlers

        // MAINFORM

        // load settings, update folder tree view

        private void MainForm_Load(object sender, EventArgs e)
        {
            // disable help menuitems if files are missing
            if (!File.Exists(Path.Combine(Application.StartupPath, "RegexRenamer.chm")))
            {
                itmHelpContents.Enabled = false;
                itmHelpContents.ToolTipText = "Not installed";
            }
            if (!File.Exists(Path.Combine(Application.StartupPath, "Regex Quick Reference.chm")))
            {
                itmHelpRegexReference.Enabled = false;
                itmHelpRegexReference.ToolTipText = "Not installed";
            }

            // load settings & regex history from registry

            cmbMatch.SetCueBanner("Shift+RightClick for a menu of regex elements");
            cmbReplace.SetCueBanner("Use $1, $2, ... to insert captured text, $# auto number, $` text before match, $' text after match, $_ original filename");

            LoadSettings();
            LoadRegexHistory();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // popluate folder tree and file list
            UpdateFolderTree();
            dgvFiles.ClearSelection();

            // focus folder list
            tvwFolders.Focus();
        }

        // save current settings
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        // global shortcuts
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (btnRename.Enabled && e.Control)
            {
                if (e.KeyCode == Keys.R)  // Ctrl+R = start rename
                {
                    e.Handled = true;
                    btnRename.PerformClick();
                }
                else if (e.KeyCode == Keys.M)  // Ctrl+M = focus match textbox
                {
                    e.Handled = true;
                    cmbMatch.Focus();
                }
            }
            else if (btnCancel.Enabled && e.KeyCode == Keys.Escape && e.Modifiers == Keys.None)  // Esc = cancel rename
            {
                e.Handled = true;
                btnCancel.PerformClick();
            }

            if (e.Handled)
                e.SuppressKeyPress = true;
        }

        // MODIFIERS

        // update preview on change
        private void cbModifierI_CheckedChanged(object sender, EventArgs e)
        {
            cbModifierI.Refresh();
            UpdatePreview();
        }
        private void cbModifierG_CheckedChanged(object sender, EventArgs e)
        {
            cbModifierG.Refresh();
            UpdatePreview();
        }
        private void cbModifierX_CheckedChanged(object sender, EventArgs e)
        {
            cbModifierX.Refresh();
            UpdatePreview();
        }


        // stats mouseover
        private void lblStats_MouseEnter(object sender, EventArgs e)
        {
            if (this.ActiveControl == txtFilter || this.ActiveControl == cbFilterExclude)  // store state
            {
                txtFilter.Tag = new object[] { this.ActiveControl, txtFilter.Text, txtFilter.SelectionStart, txtFilter.SelectionLength };
                UnFocusAll();
            }
            else
                txtFilter.Tag = null;

            gbFilter.Enabled = false;
            lblStats.ForeColor = Color.FromArgb(0, 70, 213);  // default winxp groupbox header colour
            pnlStats.Visible = true;
        }
        private void lblStats_MouseLeave(object sender, EventArgs e)
        {
            gbFilter.Enabled = true;
            lblStats.ForeColor = SystemColors.ControlDark;
            pnlStats.Visible = false;

            if (txtFilter.Tag != null)  // restore state
            {
                object[] state = (object[])txtFilter.Tag;
                ((Control)state[0]).Focus();
                txtFilter.Text = (string)state[1];
                txtFilter.SelectionStart = (int)state[2];
                txtFilter.SelectionLength = (int)state[3];
            }
        }

        #endregion

    }
}


/* ==============================================================================
 * Tag usage
 * ------------------------------------------------------------------------------
 * Variable                    Type        Usage                     Can be null?
 * ------------------------------------------------------------------------------
 * cmbMatch                    string      Regex string for history entry     N
 * tvwFolders                  TreeNode    The 'My Network Places' node       Y
 * tvwFolders.Nodes            FolderItem  Folder information for node        N
 * dgvFiles                    int         Number of files ignored            N
 * dgvFiles.Rows[i]            int         Index for entry in activeFiles     N
 * dgvFiles.Rows[i].Cells[2]   string      Preview validation error message   Y
 * mnuNumbering.DropDownItems  bool        True if validation error           N
 * cmRegexMatch.MenuItems      InsertArgs  The text to be inserted and how    N
 * txtFilter                   object[]    Store state during stat mouseover  Y
 * =============================================================================
 */
