using RegexRenamer.Models;
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
        private ValidationResult _lastValidationResult;

        private void UpdateValidation()
        {
            // delegate to ValidationService for pure logic
            _lastValidationResult = _validationService.ValidatePreview(
                _fileViewRows,
                _fileStore.InactiveFiles,
                _currentInput,
                strFilename,
                strFile);

            // single pass: apply error tags and update column colours
            bool isRenameInPlace = _currentInput.Output == OutputMode.RenameInPlace;
            for (int dfi = 0; dfi < _fileViewRows.Count; dfi++)
            {
                var rowData = _fileViewRows[dfi];
                var file = rowData.FileInfo;

                // error tag
                rowData.PreviewErrorTag = _lastValidationResult.FileErrors.TryGetValue(dfi, out string error) ? error : null;

                // filename forecolor
                if (file.Context.Matched)
                    rowData.FilenameForeColor = Color.Blue;
                else if (file.Hidden)
                    rowData.FilenameForeColor = SystemColors.GrayText;
                else
                    rowData.FilenameForeColor = SystemColors.WindowText;

                // preview forecolor
                if (rowData.PreviewErrorTag != null)
                    rowData.PreviewForeColor = Color.Red;
                else if (isRenameInPlace && file.Name != file.Context.Preview)
                    rowData.PreviewForeColor = Color.Blue;
                else if (!isRenameInPlace && file.Context.Matched)
                    rowData.PreviewForeColor = Color.Blue;
                else if (file.Hidden)
                    rowData.PreviewForeColor = SystemColors.GrayText;
                else
                    rowData.PreviewForeColor = SystemColors.WindowText;
            }

            // update matched/conflicts counters
            lblNumMatched.Text = _lastValidationResult.MatchedCount.ToString();
            lblNumConflict.Text = _lastValidationResult.ConflictCount.ToString();
        }

        /// <summary>Applies theme colors to the grid. Call on theme change, not every preview update.</summary>
        private void ApplyGridThemeColors()
        {
            if (_dm != null)
            {
                dgvFiles.BackgroundColor = _dm.OScolors.Control;
                dgvFiles.GridColor = _dm.OScolors.SecondaryLight;
                dgvFiles.RowsDefaultCellStyle.BackColor = _dm.OScolors.Surface;
                dgvFiles.AlternatingRowsDefaultCellStyle.BackColor = _dm.OScolors.Control;
            }
            else
            {
                dgvFiles.BackgroundColor = SystemColors.Window;
                dgvFiles.GridColor = SystemColors.Control;
                dgvFiles.RowsDefaultCellStyle.BackColor = SystemColors.Window;
                dgvFiles.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;
            }
        }

        // path field — thin wrapper delegating to ValidationService
        private string ValidatePath()
        {
            this.Cursor = Cursors.AppStarting;

            string errorMessage = _validationService.ValidatePath(txtPath.Text, out string normalizedPath);

            if (errorMessage == null && normalizedPath != txtPath.Text)
            {
                int ss = txtPath.SelectionStart;
                txtPath.Text = normalizedPath;
                txtPath.SelectionStart = ss;
            }

            this.Cursor = Cursors.Default;
            return errorMessage;
        }

        // handle filter regex/glob validation
        private void ValidateFilter()
        {
            if (!EnableUpdates) return;

            string errorMessage = rbFilterGlob.Checked
                ? _validationService.ValidateGlob(cmbFilter.Text)
                : _validationService.ValidateRegex(cmbFilter.Text);

            if (errorMessage == null)
            {
                cmbFilter.BackColor = SystemColors.Window;
                toolTip.Hide(cmbFilter);
                _validFilter = true;
            }
            else
            {
                cmbFilter.BackColor = Color.MistyRose;
                toolTip.Show(errorMessage, cmbFilter, 0, cmbFilter.Height);
                _validFilter = false;
            }
        }

        private void ValidateSort()
        {
            if (!EnableUpdates) return;

            string errorMessage = _validationService.ValidateRegex(cmbSortHint.Text);

            if (errorMessage == null)
            {
                cmbSortHint.BackColor = SystemColors.Window;
                toolTip.Hide(cmbSortHint);
                _validSortMatch = true;
            }
            else
            {
                cmbSortHint.BackColor = Color.MistyRose;
                toolTip.Show(errorMessage, cmbSortHint, 0, cmbSortHint.Height);
                _validSortMatch = false;
            }
        }

        // handle match regex validation
        private void ValidateMatch()
        {
            string errorMessage = _validationService.ValidateRegex(cmbMatch.Text);

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
