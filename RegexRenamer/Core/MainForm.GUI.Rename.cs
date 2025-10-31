using RegexRenamer.Rename;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer
{
    public partial class MainForm
    {
        private bool _renameSelectionOnly = false;

        private void InitializeGUICore()
        {
            cmbMatch.SelectedIndexChanged += cmbMatch_SelectedIndexChanged;
            cmbMatch.TextChanged += cmbMatch_TextChanged;
            cmbMatch.Enter += cmbMatch_Enter;
            cmbMatch.KeyDown += cmbMatch_KeyDown;
            cmbMatch.Leave += cmbMatch_Leave;
            cmbMatch.MouseDown += cmbMatch_MouseDown;
            cmbMatch.MouseUp += cmbMatch_MouseUp;
            //cmbMatch.ContextMenuStrip = cmRegexMatch;

            cmbReplace.TextChanged += cmbReplace_TextChanged;
            cmbReplace.KeyDown += cmbReplace_KeyDown;
            cmbReplace.Leave += cmbReplace_Leave;
            cmbReplace.MouseDown += cmbReplace_MouseDown;
            cmbReplace.MouseUp += cmbReplace_MouseUp;
           // cmbReplace.ContextMenuStrip = cmRegexReplace;

            btnCancel.Click += btnCancel_Click;

            // rename button & menu
            btnRename.Click += btnRename_Click;
            cmsRename.Opening += cmsRename_Opening;
            //rename files split button menu
            renameFilesCMSRenameItem.Click += RenameFilesCMSRenameItem_Click;
            // rename folders split button menu
            renameFoldersCMSRenameItem.Click += RenameFoldersCMSRenameItem_Click;
            chkRenameSelectionOnly.CheckedChanged += ChkRenameSelectionOnly_Click;

            itmOutputRenameInPlace.Click += itmOutputRenameInPlace_Click;
            itmOutputMoveTo.Click += itmOutputMoveTo_Click;
            itmOutputCopyTo.Click += itmOutputCopyTo_Click;
            itmOutputBackupTo.Click += itmOutputBackupTo_Click;

            mnuMoveCopy.MouseDown += mnuMoveCopy_MouseDown;
        }

        


        // MATCH & REPLACE
        private void cmbMatch_TextChanged(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmbMatch.Text.StartsWith("/")) return;  // skip when selection from combobox

            ValidateMatch();
            cmbMatch.Update();

            if (itmOptionsRealtimePreview.Checked)
                UpdatePreview();
        }
        private void cmbMatch_Enter(object sender, EventArgs e)
        {
            ValidateMatch();  // show tooltip if left during error
        }
        private void cmbMatch_Leave(object sender, EventArgs e)
        {
            toolTip.Hide(cmbMatch);

            if (cmbReplace.Focused || cbModifierI.Focused || cbModifierG.Focused || cbModifierX.Focused) return;

            if (PreviewNeedsUpdate)
                UpdatePreview();
        }
        private void cmbReplace_TextChanged(object sender, EventArgs e)
        {
            if (!EnableUpdates || cmbMatch.Text.StartsWith("/")) return;  // skip when selection from combobox

            if (itmOptionsRealtimePreview.Checked)
                UpdatePreview();
        }
        private void cmbReplace_Leave(object sender, EventArgs e)
        {
            if (cmbMatch.Focused || cbModifierI.Focused || cbModifierG.Focused || cbModifierX.Focused) return;

            if (PreviewNeedsUpdate)
                UpdatePreview();
        }

        // parse combobox history string
        private void cmbMatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMatch.SelectedIndex == -1)
                return;  // can occur when alt-tabbing away and back while dropdown is open

            string regexString = (string)cmbMatch.Items[cmbMatch.SelectedIndex];

            Regex regex = new Regex("^/(?<match>[^/]+)/(?<replace>[^/]*)/(?<modifiers>[igx]*)$");
            Match match = regex.Match(regexString);

            if (match.Success)
            {
                EnableUpdates = false;

                cmbMatch.newText = match.Groups["match"].Value; cmbMatch.Invalidate();  // see MyComboBox
                cmbReplace.Text = match.Groups["replace"].Value;
                cbModifierI.Checked = match.Groups["modifiers"].Value.Contains("i");
                cbModifierG.Checked = match.Groups["modifiers"].Value.Contains("g");
                cbModifierX.Checked = match.Groups["modifiers"].Value.Contains("x");

                EnableUpdates = true;
            }
            else  // invalid regexString, remove
            {
                cmbMatch.Items.RemoveAt(cmbMatch.SelectedIndex);
                SaveRegexHistory();
                ResetFields();
            }
        }

        // custom key behaviour
        private void cmbMatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (cmbMatch.DroppedDown) return;

            if (e.KeyCode == Keys.Enter && !itmOptionsRealtimePreview.Checked)  // enter (when no realtime preview) = gen preview
            {
                if (_validMatch && PreviewNeedsUpdate)
                {
                    UpdatePreview();
                    e.SuppressKeyPress = true;
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && e.Control)  // ctrl+delete = delete history
            {
                ResetFields();
                cmbMatch.Items.Clear();
                SaveRegexHistory();
                UpdateFileList();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Back && e.Control)  // ctrl+backspace = clear fields
            {
                ResetFields();
                UpdateFileList();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Down && !e.Alt)  // down arrow = move to replace field (still allow alt+down to open menu)
            {
                cmbReplace.Select(cmbMatch.SelectionStart, 0);
                cmbReplace.Focus();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown)  // ignore default behaviour
            {
                e.Handled = true;
            }
        }
        private void cmbReplace_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !itmOptionsRealtimePreview.Checked)  // enter (when no realtime preview) = gen preview
            {
                if (_validMatch && PreviewNeedsUpdate)
                {
                    UpdatePreview();
                    e.SuppressKeyPress = true;
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Back && e.Control)  // ctrl+backspace = clear fields
            {
                ResetFields();
                UpdateFileList();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Up)  // up arrow = move to match field
            {
                cmbMatch.Focus();
                cmbMatch.Select(cmbReplace.SelectionStart, 0);
            }
            else if (e.KeyCode == Keys.Down)  // ignore default behaviour
            {
                e.Handled = true;
            }
        }
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
        private void cmbReplace_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                regExCtxMenu.ShowReplaceMenu(cmbReplace, e.Location);
            }
        }
        private void cmbReplace_MouseUp(object sender, MouseEventArgs e)
        {
            if (cmbReplace.ContextMenuStrip != null)
                cmbReplace.ContextMenuStrip = null;  // restore default cms
        }

        // click
        private void btnRename_Click(object sender, EventArgs e)
        {
            PerformRename();
        }

        private void ChkRenameSelectionOnly_Click(object sender, EventArgs e)
        {
            _renameSelectionOnly = chkRenameSelectionOnly.Checked;
            UpdateFileList();
        }

        private void RenameFilesCMSRenameItem_Click(object sender, EventArgs e)
        {
            RenameFolders = false;
            UpdateFileList();
        }
        private void RenameFoldersCMSRenameItem_Click(object sender, EventArgs e)
        {
            RenameFolders = true;
            UpdateFileList();
        }

        private void PerformRename()
        {
            // check for errors
            string errorMessage = null;

            // invalid match regex
            if (!_validMatch) errorMessage = "The match regular expression in invalid.";

            // preview errors exist
            if (errorMessage == null)
            {
                foreach (DataGridViewRow row in dgvFiles.Rows)
                {
                    int afi = (int)row.Tag;
                    if (_renameSelectionOnly && !_fileStore.Files[afi].Selected)
                        continue;  // ignore unselected rows

                    if (row.Cells[2].Tag != null)
                    {
                        errorMessage = "Can't rename while errors exist (highlighted in red).";
                        break;
                    }
                }
            }

            // no files need renaming
            int filesToRename = 0;
            if (errorMessage == null)
            {
                foreach (RenameItemInfo file in _fileStore.Files)
                {
                    if (_renameSelectionOnly && !file.Selected)
                        continue;  // ignore unselected rows

                    if ((itmOutputRenameInPlace.Checked && file.Name != file.Preview)
                     || (!itmOutputRenameInPlace.Checked && file.Matched))
                        filesToRename++;
                }

                if (filesToRename == 0) errorMessage = "There are no " + strFile + "s to rename.";
            }

            // move/copy path doesn't exist
            if (errorMessage == null && !itmOutputRenameInPlace.Checked && !Directory.Exists(fbdMoveCopy.SelectedPath))
            {
                if (itmOutputMoveTo.Checked) errorMessage = "'Move to' folder '" + fbdMoveCopy.SelectedPath + "' is not a valid path.";
                else if (itmOutputCopyTo.Checked) errorMessage = "'Copy to' folder '" + fbdMoveCopy.SelectedPath + "' is not a valid path.";
                else if (itmOutputBackupTo.Checked) errorMessage = "'Backup to' folder '" + fbdMoveCopy.SelectedPath + "' is not a valid path.";
            }

            // move/copy path same as activePath
            if (errorMessage == null && !itmOutputRenameInPlace.Checked && fbdMoveCopy.SelectedPath == _activePath)
            {
                if (itmOutputMoveTo.Checked) errorMessage = "'Move to' folder is the same as the currently selected folder.";
                else if (itmOutputCopyTo.Checked) errorMessage = "'Copy to' folder is the same as the currently selected folder.";
                else if (itmOutputBackupTo.Checked) errorMessage = "'Backup to' folder is the same as the currently selected folder.";
            }

            // if error found, display dialog & abort
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // warn if any files start with space or dot
            bool beginWithInvalidChars = false;
            Regex regexInvalidChars = new Regex("(^|\\\\)[ .]");

            foreach (RenameItemInfo file in _fileStore.Files)
            {
                if (itmOutputRenameInPlace.Checked)
                {
                    if (file.Name == file.Preview) continue;
                }
                else
                {
                    if (!file.Matched) continue;
                }

                if (regexInvalidChars.IsMatch(file.Preview))
                {
                    beginWithInvalidChars = true;
                    break;
                }
            }

            if (beginWithInvalidChars)
            {
                errorMessage = "One or more " + strFilename + "s begin with a space or a dot. While this is technically possible, Windows\n"
                             + "normally won't let you do this as it may cause problems with other programs.\n"
                             + "\n"
                             + "Are you sure you want to continue?";

                if (MessageBox.Show(errorMessage, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                    == DialogResult.Cancel)
                    return;
            }

            // remember regex history string to store later (in case the user changes the fields during rename)
            string regexString = "/" + cmbMatch.Text + "/" + cmbReplace.Text + "/";
            if (cbModifierI.Checked) regexString += "i";
            if (cbModifierG.Checked) regexString += "g";
            if (cbModifierX.Checked) regexString += "x";

            cmbMatch.Tag = regexString;

            // change button to cancel
            btnRename.Visible = false;
            btnCancel.Visible = true;
            btnRename.Enabled = false;
            btnCancel.Enabled = true;

            // init progressbar
            progressBar.Value = 0;
            tsOptions.Visible = lblNumMatched.Visible = lblNumConflict.Visible = false;
            progressBar.Visible = true;
            progressBar.Focus();


            // semi-disable form during rename
            SetFormActive(false);


            // perform rename operation in background thread
            bgwRename.RunWorkerAsync(filesToRename);
        }

        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnCancel.Text = "Cancelling...";
            bgwRename.CancelAsync();
        }

        // RENAME

        // drop-down menu
        private void cmsRename_Opening(object sender, CancelEventArgs e)
        {
            if (btnRename.State != System.Windows.Forms.VisualStyles.PushButtonState.Pressed)
                e.Cancel = true;  // prevent context menu on right-click
        }
        

        // move/copy
        private void OutputMenuItem(object sender)
        {
            if (!EnableUpdates) return;
            ToolStripMenuItem clickedMenuItem = (ToolStripMenuItem)sender;


            if (clickedMenuItem != itmOutputRenameInPlace)
            {
                // update dialog text
                if (clickedMenuItem == itmOutputMoveTo)
                    fbdMoveCopy.Description = "During the rename operation, " + strFile + "s that match the current regex will be "
                                            + "moved to the selected folder and renamed (if necessary).";
                else if (clickedMenuItem == itmOutputCopyTo)
                    fbdMoveCopy.Description = "During the rename operation, files that match the current regex will be "
                                            + "copied to the selected folder and the copies renamed (if necessary).";
                else if (clickedMenuItem == itmOutputBackupTo)
                    fbdMoveCopy.Description = "During the rename operation, files that match the current regex will be "
                                            + "copied to the selected folder and the originals renamed (if necessary).";


                // show dialog, ignore if cancelled
                if (fbdMoveCopy.ShowDialog() != DialogResult.OK) return;


                // update folder tree in case user created new folder within fbdNetwork
                if (!_activePath.StartsWith(fbdMoveCopy.SelectedPath))
                {
                    DirectoryInfo parent = Directory.GetParent(fbdMoveCopy.SelectedPath);
                    if (parent != null)
                    {
                        EnableUpdates = false;
                        tvwFolders.RefreshNode(parent.FullName);  // may DrillToFolder()
                        EnableUpdates = true;
                    }
                }


                // show warning if same as activePath
                if (fbdMoveCopy.SelectedPath == _activePath)
                {
                    string errorMessage = "This '";
                    if (clickedMenuItem == itmOutputMoveTo) errorMessage += "Move to";
                    else if (clickedMenuItem == itmOutputCopyTo) errorMessage += "Copy to";
                    else if (clickedMenuItem == itmOutputBackupTo) errorMessage += "Backup to";
                    errorMessage += "' folder is the same as the currently selected folder.\r\n";

                    MessageBox.Show(errorMessage, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }


            // update checked marks
            for (int i = 0; i < mnuMoveCopy.DropDownItems.Count; i++)
            {
                if (i == 1) continue;  // seperator

                if (mnuMoveCopy.DropDownItems[i] == clickedMenuItem)
                    ((ToolStripMenuItem)mnuMoveCopy.DropDownItems[i]).Checked = true;
                else
                    ((ToolStripMenuItem)mnuMoveCopy.DropDownItems[i]).Checked = false;
            }


            // set button text to bold if an option selected
            if (itmOutputRenameInPlace.Checked)
            {
                mnuMoveCopy.Font = new Font("Tahoma", 8.25F);
                mnuMoveCopy.Padding = new Padding(0, 0, 17, 0);
            }
            else
            {
                mnuMoveCopy.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                mnuMoveCopy.Padding = new Padding(0, 0, 7, 0);
            }


            // redo validation
            this.Update();
            UpdateValidation();
        }
        private void itmOutputRenameInPlace_Click(object sender, EventArgs e)
        {
            OutputMenuItem(sender);
        }
        private void itmOutputMoveTo_Click(object sender, EventArgs e)
        {
            OutputMenuItem(sender);
        }
        private void itmOutputCopyTo_Click(object sender, EventArgs e)
        {
            OutputMenuItem(sender);
        }
        private void itmOutputBackupTo_Click(object sender, EventArgs e)
        {
            OutputMenuItem(sender);
        }
        private void mnuMoveCopy_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && !itmOutputRenameInPlace.Checked)  // set default
                itmOutputRenameInPlace.PerformClick();
        }
    }
}
