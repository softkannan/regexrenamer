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
        private SortMatchItem _activeSortMatch = null;         // regex for sorting (null if not sorting by regex)
        private string _activeSortMatchText = "";    // current sort regex text (for restoring on cancel)
        private bool _validSortMatch = true;         // sort regex is valid

        private void InitializeSort()
        {
            cmbSort.SelectedIndex = 0;

            cmbSort.TextChanged += cmbSort_TextChanged;
            cmbSort.KeyDown += cmbSort_KeyDown;
            cmbSort.Leave += cmbSort_Leave;
            cmbSort.SelectedValueChanged += cmbSort_SelectedValueChanged;
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

            UpdateUserInputValues();
        }
        #endregion
    }
}
