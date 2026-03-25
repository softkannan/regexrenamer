using Config;
using RegexRenamer.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer
{
    public partial class MainForm
    {
        private bool _validFilter = true;      // file filter is valid
        private bool _validMatch = true;      // regex match expression is valid
        private bool _validNumber = true;      // numbering menu options are all valid

        private SortMatchItem _activeSortMatch = null;         // regex for sorting (null if not sorting by regex)
        private string _activeSortMatchText = "";    // current sort regex text (for restoring on cancel)
        private bool _validSortMatch = true;         // sort regex is valid

        private void InitializeFilter()
        {
            cmbFilter.Items.AddRange(new string[] { "*.*", "*" });
            foreach (string hint in UserConfig.Inst.FilterHints)
            {
                if (!cmbFilter.Items.Contains(hint))
                    cmbFilter.Items.Add(hint);
            }

            foreach (var hint in UserConfig.Inst.SortHints)
            {
                if (!cmbSort.Items.Contains(hint.Name))
                {
                    cmbSort.Items.Add(hint.Name);
                }
            }
            cmbFilter.SelectedIndex = 0;
            cmbSort.SelectedIndex = 0;

            rbFilterRegex.CheckedChanged += rbFilterRegex_CheckedChanged;
            rbFilterRegex.Click += rbFilterRegex_Click;
            rbFilterGlob.Click += rbFilterGlob_Click;
            
            cmbFilter.TextChanged += cmbFilter_TextChanged;
            cmbFilter.KeyDown += cmbFilter_KeyDown;
            cmbFilter.Leave += cmbFilter_Leave;
            cmbFilter.MouseDown += cmbFilter_MouseDown;
            cmbFilter.MouseUp += cmbFilter_MouseUp;
            cmbFilter.SelectedValueChanged += cmbFilter_SelectedValueChanged;

            cmbSort.TextChanged += cmbSort_TextChanged;
            cmbSort.KeyDown += cmbSort_KeyDown;
            cmbSort.Leave += cmbSort_Leave;
            cmbSort.SelectedValueChanged += cmbSort_SelectedValueChanged;

            cbFilterExclude.CheckedChanged += cbFilterExclude_CheckedChanged;
            chkIncludeSubfolder.CheckedChanged += chkIncludeSubfolder_CheckedChanged;
        }


        // Sort
        #region Sorting handling
        private void cmbSort_SelectedValueChanged(object sender, EventArgs e)
        {
            ValidateSort();
            if (_validSortMatch)
                ApplySort();
        }
        private void cmbSort_TextChanged(object sender, EventArgs e)
        {
            ValidateSort();
        }
        private void cmbSort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)  // apply filter
            {
                if (_validSortMatch)
                    e.SuppressKeyPress = true;  // prevent beep on enter if valid filter

                ApplySort();
            }
            else if (e.KeyCode == Keys.Escape)  // revert
            {
                EnableUpdates = false;
                cmbSort.Text = _activeSortMatchText;
                EnableUpdates = true;

                _validSortMatch = true;
                cmbSort.BackColor = SystemColors.Window;
                toolTip.Hide(cmbSort);
                cmbSort.SelectionStart = _activeSortMatchText.Length;

                e.SuppressKeyPress = true;  // prevent beep
            }
        }
        private void cmbSort_Leave(object sender, EventArgs e)
        {
            EnableUpdates = false;
            cmbSort.Text = _activeSortMatchText;
            EnableUpdates = true;

            _validSortMatch = true;
            cmbSort.BackColor = SystemColors.Window;
            toolTip.Hide(cmbSort);
        }

        private void ApplySort()
        {
            if (!EnableUpdates || !_validSortMatch) return;

            _activeSortMatchText = cmbSort.Text;
            if (string.IsNullOrWhiteSpace(_activeSortMatchText))
            {
                _activeSortMatch = null;
            }
            else
            {
                try
                {
                    var foundConfig = UserConfig.Inst.SortHints.FirstOrDefault(hint => hint.Name == _activeSortMatchText);
                    if (foundConfig != null)
                    {
                        _activeSortMatch = new SortMatchItem(foundConfig);
                    }
                    else
                    {
                        _activeSortMatch = new SortMatchItem(_activeSortMatchText);
                    }
                    cmbSort.BackColor = SystemColors.Window;
                    toolTip.Hide(cmbSort);
                }
                catch (Exception ex)
                {
                    _activeSortMatch = null;
                    cmbSort.BackColor = Color.LightPink;
                    toolTip.Show("Invalid regex: " + ex.Message, cmbSort);
                }
            }

            if ((!string.IsNullOrWhiteSpace(cmbSort.Text)) && _activeSortMatch != null && cmbSort.Text != "Default")
            {
                UpdateDataGridColumnTextAlignment(DataGridViewContentAlignment.MiddleRight);
            }
            else
            {
                UpdateDataGridColumnTextAlignment(DataGridViewContentAlignment.MiddleLeft);
            }

            UpdatePreview();
        }
        #endregion

        // FILTER
        #region Filter handling
        private void ApplyFilter()
        {
            if (!EnableUpdates || !_validFilter) return;

            _activeFilter = cmbFilter.Text;
            UpdateFileList();
        }

        private void cmbFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            ValidateFilter();
            if (_validFilter)
                ApplyFilter();
        }

        private void chkIncludeSubfolder_CheckedChanged(object sender, EventArgs e)
        {
            if (!EnableUpdates) return;

            lblStats.Text = "Updating file list...";

            UpdateFileList();
        }
        private void rbFilterRegex_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFilterGlob.Checked && (cmbFilter.Text == ".*" || cmbFilter.Text == ".+"))
            {
                EnableUpdates = false;
                cmbFilter.Text = "*.*";
                _activeFilter = "*.*";
                EnableUpdates = true;
            }
            else if (rbFilterRegex.Checked && (cmbFilter.Text == "*.*" || cmbFilter.Text == "*"))
            {
                EnableUpdates = false;
                cmbFilter.Text = ".*";
                _activeFilter = ".*";
                EnableUpdates = true;
            }
            else
            {
                _activeFilter = "";
                ValidateFilter();
            }
        }
        private void rbFilterGlob_Click(object sender, EventArgs e)
        {
            cmbFilter.Focus();
        }
        private void rbFilterRegex_Click(object sender, EventArgs e)
        {
            cmbFilter.Focus();
        }
        private void cmbFilter_TextChanged(object sender, EventArgs e)
        {
            ValidateFilter();
        }
        private void cmbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)  // apply filter
            {
                if (_validFilter)
                    e.SuppressKeyPress = true;  // prevent beep on enter if valid filter

                ApplyFilter();
            }
            else if (e.KeyCode == Keys.Escape)  // revert
            {
                EnableUpdates = false;
                cmbFilter.Text = _activeFilter;
                EnableUpdates = true;

                _validFilter = true;
                cmbFilter.BackColor = SystemColors.Window;
                toolTip.Hide(cmbFilter);
                cmbFilter.SelectionStart = _activeFilter.Length;

                e.SuppressKeyPress = true;  // prevent beep
            }
        }
        private void cmbFilter_Leave(object sender, EventArgs e)
        {
            if (cbFilterExclude.Focused || rbFilterGlob.Focused || rbFilterRegex.Focused) return;

            EnableUpdates = false;
            cmbFilter.Text = _activeFilter;
            EnableUpdates = true;

            _validFilter = true;
            cmbFilter.BackColor = SystemColors.Window;
            toolTip.Hide(cmbFilter);
        }
        private void cbFilterExclude_CheckedChanged(object sender, EventArgs e)
        {
            if (_validFilter)
                ApplyFilter();
            else
                cmbFilter.Focus();
        }
        #endregion

        // Filter context menu
        private void cmbFilter_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                regExCtxMenu.ShowGlobMenu(cmbFilter, rbFilterGlob.Checked, e.Location);
            }
        }
        private void cmbFilter_MouseUp(object sender, MouseEventArgs e)
        {
            if (cmbFilter.ContextMenuStrip != null)
                cmbFilter.ContextMenuStrip = null;  // restore default cms
        }
    }
}
