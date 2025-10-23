using RegexRenamer.Rename;
using System;
using System.Collections.Generic;
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
        private void UpdateValidation()
        {
            bool[] hasError = new bool[dgvFiles.Rows.Count];
            Dictionary<string, List<int>> hashPreview = new Dictionary<string, List<int>>();

            // generate hash of preview filenames (value = list of dfi indexes)
            for (int dfi = 0; dfi < dgvFiles.Rows.Count; dfi++)  // dfi = dgvFiles index
            {
                int afi = (int)dgvFiles.Rows[dfi].Tag;              // afi = activeFiles index

                string preview = _fileStore.Files[afi].PreviewExt.ToLower();

                if (!hashPreview.ContainsKey(preview))
                    hashPreview.Add(preview, new List<int>());
                hashPreview[preview].Add(dfi);

                dgvFiles.Rows[dfi].Cells[2].Tag = null;  // clear all errors
            }


            // check for errors
            string outputPath = _activePath;
            if (itmOutputMoveTo.Checked || itmOutputCopyTo.Checked)
                outputPath = fbdMoveCopy.SelectedPath;

            for (int dfi = 0; dfi < dgvFiles.Rows.Count; dfi++)
            {
                int afi = (int)dgvFiles.Rows[dfi].Tag;
                string preview = _fileStore.Files[afi].PreviewExt.ToLower();

                // skip if already has an error, or an ignored file
                if (hasError[dfi]) continue;

                if (itmOutputRenameInPlace.Checked)
                {
                    if (_fileStore.Files[afi].Name == _fileStore.Files[afi].Preview) continue;
                }
                else
                {
                    if (!_fileStore.Files[afi].Matched) continue;
                }

                // check for valid filename
                string validFilenameErrmsg = ValidateFilename(_fileStore.Files[afi].PreviewExt, itmOptionsAllowRenSub.Checked);
                if (validFilenameErrmsg != null)
                {
                    dgvFiles.Rows[dfi].Cells[2].Tag = validFilenameErrmsg;
                    continue;
                }

                // check for dupe filename conflicts
                if (!_fileStore.Files[afi].Preview.Contains("\\")
                    && (itmOutputRenameInPlace.Checked || itmOutputBackupTo.Checked))  // destination is same directory
                {
                    // check against active files
                    if (hashPreview[preview].Count > 1)
                    {
                        foreach (int dfi2 in hashPreview[preview])
                        {
                            if (hasError[dfi2])
                                continue;
                            else
                                hasError[dfi2] = true;

                            int afi2 = (int)dgvFiles.Rows[dfi2].Tag;
                            dgvFiles.Rows[dfi2].Cells[2].Tag = "The " + strFilename + " '" + _fileStore.Files[afi2].PreviewExt
                                                             + "' conflicts with another " + strFile + " in the preview column.";
                        }
                        continue;
                    }

                    // check against inactive files
                    if (_fileStore.InactiveFiles.ContainsKey(preview))
                    {
                        switch (_fileStore.InactiveFiles[preview])
                        {
                            case InactiveReason.Filtered:
                                dgvFiles.Rows[dfi].Cells[2].Tag = "The " + strFilename + " '" + _fileStore.Files[afi].PreviewExt
                                                                + "' already exists in this directory but is currently filtered out.";
                                break;
                            case InactiveReason.Hidden:
                                dgvFiles.Rows[dfi].Cells[2].Tag = "The " + strFilename + " '" + _fileStore.Files[afi].PreviewExt
                                                                + "' already exists in this directory as a hidden " + strFile + ".";
                                break;
                        }
                        continue;
                    }

                    // check for file-folder conflicts
                    if (dgvFiles.Rows.Count < 2000)  // this check is expensive, only run if < 2000 items
                    {
                        string previewFullpath = Path.Combine(outputPath, _fileStore.Files[afi].PreviewExt);
                        if (RenameFolders ? File.Exists(previewFullpath) : Directory.Exists(previewFullpath))
                        {
                            dgvFiles.Rows[dfi].Cells[2].Tag = "The " + strFilename + " '" + _fileStore.Files[afi].PreviewExt
                                                            + "' conflicts with a " + (RenameFolders ? "file" : "folder")
                                                            + " in the current path.";
                            continue;
                        }
                    }
                }
                else  // destination is other directory, check against file system
                {
                    string previewFullpath = Path.Combine(outputPath, _fileStore.Files[afi].PreviewExt);
                    if (RenameFolders ? Directory.Exists(previewFullpath) : File.Exists(previewFullpath))
                    {
                        dgvFiles.Rows[dfi].Cells[2].Tag = "The " + strFilename + " '"
                                                        + Path.GetFileName(_fileStore.Files[afi].PreviewExt)
                                                        + "' already exists in the destination folder.";
                        continue;
                    }

                    if (RenameFolders ? File.Exists(previewFullpath) : Directory.Exists(previewFullpath))
                    {
                        dgvFiles.Rows[dfi].Cells[2].Tag = "The " + strFilename + " '"
                                                        + Path.GetFileName(_fileStore.Files[afi].PreviewExt) + "' conflicts with a "
                                                        + (RenameFolders ? "file" : "folder") + " in the destination path.";
                        continue;
                    }
                }


                // if doing 'Backup to' (files only), also check original names against backup path
                if (itmOutputBackupTo.Checked)
                {
                    string previewFullpath = Path.Combine(fbdMoveCopy.SelectedPath, _fileStore.Files[afi].Filename);
                    if (File.Exists(previewFullpath))
                    {
                        dgvFiles.Rows[dfi].Cells[2].Tag = "The original filename '" + _fileStore.Files[afi].Filename
                                                        + "' already exists in the selected backup folder.";
                        continue;
                    }

                    if (Directory.Exists(previewFullpath))
                    {
                        dgvFiles.Rows[dfi].Cells[2].Tag = "The original filename '" + _fileStore.Files[afi].Filename
                                                        + "' conflicts with a folder in the selected backup path.";
                        continue;
                    }
                }

            }  // end error loop


            // update filename/preview column colour
            for (int dfi = 0; dfi < dgvFiles.Rows.Count; dfi++)
            {
                int afi = (int)dgvFiles.Rows[dfi].Tag;

                if (_fileStore.Files[afi].Matched)
                    dgvFiles.Rows[dfi].Cells[1].Style.ForeColor = Color.Blue;
                else if (_fileStore.Files[afi].Hidden)
                    dgvFiles.Rows[dfi].Cells[1].Style.ForeColor = SystemColors.GrayText;
                else
                    dgvFiles.Rows[dfi].Cells[1].Style.ForeColor = SystemColors.WindowText;

                if (dgvFiles.Rows[dfi].Cells[2].Tag != null)
                    dgvFiles.Rows[dfi].Cells[2].Style.ForeColor = Color.Red;
                else if (itmOutputRenameInPlace.Checked && _fileStore.Files[afi].Name != _fileStore.Files[afi].Preview)
                    dgvFiles.Rows[dfi].Cells[2].Style.ForeColor = Color.Blue;
                else if (!itmOutputRenameInPlace.Checked && _fileStore.Files[afi].Matched)
                    dgvFiles.Rows[dfi].Cells[2].Style.ForeColor = Color.Blue;
                else if (_fileStore.Files[afi].Hidden)
                    dgvFiles.Rows[dfi].Cells[2].Style.ForeColor = SystemColors.GrayText;
                else
                    dgvFiles.Rows[dfi].Cells[2].Style.ForeColor = SystemColors.WindowText;
            }

            if (_dm != null)
            {
                dgvFiles.BackgroundColor = _dm.OScolors.Control;
                dgvFiles.GridColor = _dm.OScolors.SecondaryLight;
                dgvFiles.RowsDefaultCellStyle.BackColor = _dm.OScolors.Surface;
                dgvFiles.AlternatingRowsDefaultCellStyle.BackColor = _dm.OScolors.Control;
            }
            else
            {
                dgvFiles.BackgroundColor = System.Drawing.SystemColors.Window;
                dgvFiles.GridColor = System.Drawing.SystemColors.Control;
                dgvFiles.RowsDefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
                dgvFiles.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlLight;
            }

            // update matched/conflicts counters
            int matched = 0, conflict = 0;
            
            for (int idx = 0; idx < _fileStore.Files.Count; idx++)
                if (_fileStore.Files[idx].Matched) matched++;

            foreach (DataGridViewRow row in dgvFiles.Rows)
                if (row.Cells[2].Tag != null)
                    conflict++;

            lblNumMatched.Text = matched.ToString();
            lblNumConflict.Text = conflict.ToString();
        }

        // validate glob/regex/filename
        private string ValidateGlob(string testGlob)
        {
            Regex regex = new Regex("([\\\\/:\"<>|])");
            Match match = regex.Match(testGlob);
            if (match.Success)
                return "Invalid character: " + match.Groups[0].Value;
            else
                return null;
        }

        private string ValidateRegex(string testRegex)
        {
            try
            {
                Regex regex = new Regex(testRegex);
            }
            catch (Exception ex)
            {
                Regex regex = new Regex("^parsing \".+\" - ");
                return regex.Replace(ex.Message, "");
            }
            return null;
        }

        Regex regValidateInvalidChars = new Regex("([\\\\/:*?\"<>|])");
        Regex regValidateInvalidCharsAllowPath = new Regex("([/:*?\"<>|])");
        Regex regValidateOnlyDotSpace = new Regex("^[ .]+$");
        Regex regValidateEndsInDotSpace = new Regex("([ .]+)$");

        private string ValidateFilename(string testFilename, bool allowRenSub)
        {
            Match match;

            // invalid character
            string[] parts = allowRenSub ? testFilename.Split('\\') : new string[] { testFilename };
            for (int i = 0; i < parts.Length; i++)
            {
                if (allowRenSub)
                    match = regValidateInvalidCharsAllowPath.Match(parts[i]);  // ([/:*?\"<>|])
                else
                    match = regValidateInvalidChars.Match(parts[i]);           // ([\\\\/:*?\"<>|])

                if (match.Success)
                    if (parts.Length > 1 && i != parts.Length - 1)
                        return "The subfolder '" + parts[i] + "' contains an invalid character: '" + match.Groups[0].Value + "'.";
                    else
                        return "The " + strFilename + " '" + parts[i] + "' contains an invalid character: '" + match.Groups[0].Value + "'.";
            }

            // starts with "\"
            if (testFilename.StartsWith("\\"))
                if (parts.Length > 2)
                    return "The subfolder cannot begin with '\\'.";
                else
                    return "The " + strFilename + " cannot begin with a backslash.";

            // element is empty
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == "")
                    if (i != parts.Length - 1)
                        return "Duplicate path seperator";
                    else
                        return "The " + strFilename + " cannot end with a backslash.";
            }

            // element is [ .]+ (eg, "/../", "/ /", ...)
            for (int i = 0; i < parts.Length; i++)
            {
                match = regValidateOnlyDotSpace.Match(parts[i]);  // ^[ .]+$
                if (match.Success)
                    if (i != parts.Length - 1)
                        return "Invalid subfolder: '" + parts[i] + "'.";
                    else
                        return "Invalid " + strFilename + ": '" + parts[i] + "'.";
            }

            // element ends with [ .]+
            for (int i = 0; i < parts.Length; i++)
            {
                match = regValidateEndsInDotSpace.Match(parts[i]);  // ([ .]+)$
                if (match.Success)
                    if (i != parts.Length - 1)
                        return "The subfolder '" + parts[i] + "' ends with invalid character(s): '" + match.Groups[0].Value + "'.";
                    else
                        return "The " + strFilename + " '" + parts[i] + "' ends with invalid character(s): '" + match.Groups[0].Value + "'.";
            }

            return null;
        }

        // path field
        private string ValidatePath()
        {
            string errorMessage = null;
            this.Cursor = Cursors.AppStarting;
            try
            {
                string normPath = Path.GetFullPath(txtPath.Text);
                if (normPath.Length > 3)
                    normPath = normPath.TrimEnd('\\');

                if (!Directory.Exists(normPath))
                {
                    errorMessage = "Path does not exist.";
                }
                else if (normPath != txtPath.Text)
                {
                    int ss = txtPath.SelectionStart;
                    txtPath.Text = normPath;
                    txtPath.SelectionStart = ss;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            this.Cursor = Cursors.Default;
            return errorMessage;
        }

        // handle filter regex/glob validation
        private void ValidateFilter()
        {
            if (!EnableUpdates) return;

            string errorMessage;

            if (rbFilterGlob.Checked)
                errorMessage = ValidateGlob(txtFilter.Text);
            else  // regex
                errorMessage = ValidateRegex(txtFilter.Text);

            if (errorMessage == null)
            {
                txtFilter.BackColor = SystemColors.Window;
                toolTip.Hide(txtFilter);
                _validFilter = true;
            }
            else
            {
                txtFilter.BackColor = Color.MistyRose;
                toolTip.Show(errorMessage, txtFilter, 0, txtFilter.Height);
                _validFilter = false;
            }
        }

        // handle match regex validation
        private void ValidateMatch()
        {
            string errorMessage = ValidateRegex(cmbMatch.Text);

            if (errorMessage == null)
            {
                if (_dm != null)
                {
                    cmbMatch.BackColor = _dm.OScolors.Control;
                }
                else
                {
                    cmbMatch.BackColor = SystemColors.Window;
                }
                toolTip.Hide(cmbMatch);
                _validMatch = true;
            }
            else
            {
                cmbMatch.BackColor = Color.MistyRose;
                toolTip.Show(errorMessage, cmbMatch, 0, cmbMatch.Height);
                _validMatch = false;
            }
        }
    }
}
