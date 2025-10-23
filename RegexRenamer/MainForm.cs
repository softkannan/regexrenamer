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
using System.Reflection;
using System.Security;
using RegexRenamer.Utility;
using System.Threading;
using RegexRenamer.Native;
using RegexRenamer.Forms;
using RegexRenamer.Rename;
using Kavita;
using Config;        // FieldInfo


namespace RegexRenamer
{
    public partial class MainForm : Form
    {
        #region Consts

        private const int MAX_HISTORY = 20;      // number of regex history entries to keep

        #endregion

        #region Variables


        private string _activeFilter = "*.*";  // current filter
        private FilesStore _fileStore = new FilesStore();  // files in activePath displayed in filelist

        private int _countProgLaunches = 1;    // counters for
        private int _countFilesRenamed = 0;    // about-dialog stats

        private About _aboutForm;

        #endregion

        #region Properties

        private string _activePath =  string.Empty;

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

                if (renameFoldersCMSRenameItem.Checked != renameFolders)
                {
                    renameFilesCMSRenameItem.Checked = !renameFolders;
                    renameFoldersCMSRenameItem.Checked = renameFolders;
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
                regExCtxMenu.UpdateOrigFilename(oldFilename,strFilename);
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

        private DarkModeForms.DarkModeCS _dm = null;
        public MainForm(string initPath)
        {

            this._activePath = initPath;

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

            InitializeInsertRegExContextMenu();

            InitializeGUICore();
            InitializeChangeCase();
            InitializeFolderTreeView();
            InitializeFileListView();
            InitializeCore();
            InitializeFilter();
            InitializeOptionsHelp();
            InitializeKavita();

            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            Shown += MainForm_Shown;
            KeyDown += MainForm_KeyDown;
            cbModifierI.CheckedChanged += cbModifierI_CheckedChanged;
            cbModifierG.CheckedChanged += cbModifierG_CheckedChanged;
            cbModifierX.CheckedChanged += cbModifierX_CheckedChanged;
            lblStats.MouseEnter += lblStats_MouseEnter;
            lblStats.MouseLeave += lblStats_MouseLeave;

            dgvFiles.DoubleBuffered(true);


            //_dm = new DarkModeForms.DarkModeCS(this)
            //{
            //    // Choose your preferred mode here:
            //    ColorMode = DarkModeForms.DarkModeCS.DisplayMode.SystemDefault
            //};

        }

      

        #endregion

        public void UpdateFolderTree()
        {
            EnableUpdates = false;
            tvwFolders.UpdateFolderTree(_activePath);
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

            cmbMatch.SetCueBanner("Shift+RightClick for a menu of regex elements");
            cmbReplace.SetCueBanner("Use $1, $2, ... to insert captured text, $# auto number, $` text before match, $' text after match, $_ original filename");

            // load settings & regex history from registry
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
