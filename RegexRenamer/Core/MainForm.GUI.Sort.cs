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
        // Sort
        // active sort provider (null for default)
        private SortStringProvider _activeSortStringProvider = null;
        // active sort hint name
        private string _activeSortHintName = "";    // current sort regex text (for restoring on cancel)
        // flag for valid sort regex
        private bool _validSortMatch = true;         // sort regex is valid

        private void InitializeSort()
        {
            cmbSortHint.SelectedIndex = 0;

            cmbSortHint.TextChanged += cmbSort_TextChanged;
            cmbSortHint.KeyDown += cmbSort_KeyDown;
            cmbSortHint.Leave += cmbSort_Leave;
            cmbSortHint.SelectedValueChanged += cmbSort_SelectedValueChanged;
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
                cmbSortHint.Text = _activeSortHintName;
                EnableUpdates = true;

                _validSortMatch = true;
                cmbSortHint.BackColor = SystemColors.Window;
                toolTip.Hide(cmbSortHint);
                cmbSortHint.SelectionStart = _activeSortHintName.Length;

                e.SuppressKeyPress = true;  // prevent beep
            }
        }
        private void cmbSort_Leave(object sender, EventArgs e)
        {
            EnableUpdates = false;
            cmbSortHint.Text = _activeSortHintName;
            EnableUpdates = true;

            _validSortMatch = true;
            cmbSortHint.BackColor = SystemColors.Window;
            toolTip.Hide(cmbSortHint);
        }

        private void ApplySort()
        {
            if (!EnableUpdates || !_validSortMatch) return;

            _activeSortHintName = cmbSortHint.Text;
            if (string.IsNullOrWhiteSpace(_activeSortHintName))
            {
                _activeSortStringProvider = null;
            }
            else
            {
                try
                {
                    var foundConfig = UserConfig.Inst.SortHints.FirstOrDefault(hint => hint.Name == _activeSortHintName);
                    if (foundConfig != null)
                    {
                        _activeSortStringProvider = new SortStringProvider(foundConfig);
                    }
                    else
                    {
                        _activeSortStringProvider = new SortStringProvider(_activeSortHintName);
                    }
                    cmbSortHint.BackColor = SystemColors.Window;
                    toolTip.Hide(cmbSortHint);
                }
                catch (Exception ex)
                {
                    _activeSortStringProvider = null;
                    cmbSortHint.BackColor = Color.LightPink;
                    toolTip.Show("Invalid regex: " + ex.Message, cmbSortHint);
                }
            }

            if ((!string.IsNullOrWhiteSpace(cmbSortHint.Text)) && _activeSortStringProvider != null && cmbSortHint.Text != "Default")
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
