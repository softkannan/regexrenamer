using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer
{
    public partial class MainForm
    {
        private bool _validFilter = true;      // file filter is valid
        private bool _validMatch = true;      // regex match expression is valid
        private bool _validNumber = true;      // numbering menu options are all valid

        private void InitializeFilter()
        {
            rbFilterRegex.CheckedChanged += rbFilterRegex_CheckedChanged;
            rbFilterRegex.Click += rbFilterRegex_Click;
            rbFilterGlob.Click += rbFilterGlob_Click;
            txtFilter.TextChanged += txtFilter_TextChanged;
            txtFilter.KeyDown += txtFilter_KeyDown;
            txtFilter.Leave += txtFilter_Leave;
            txtFilter.MouseDown += txtFilter_MouseDown;
            txtFilter.MouseUp += txtFilter_MouseUp;
            cbFilterExclude.CheckedChanged += cbFilterExclude_CheckedChanged;
        }

        // FILTER
        private void ApplyFilter()
        {
            if (!EnableUpdates || !_validFilter) return;

            _activeFilter = txtFilter.Text;
            UpdateFileList();
        }
        private void rbFilterRegex_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFilterGlob.Checked && (txtFilter.Text == ".*" || txtFilter.Text == ".+"))
            {
                EnableUpdates = false;
                txtFilter.Text = "*.*";
                _activeFilter = "*.*";
                EnableUpdates = true;
            }
            else if (rbFilterRegex.Checked && (txtFilter.Text == "*.*" || txtFilter.Text == "*"))
            {
                EnableUpdates = false;
                txtFilter.Text = ".*";
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
            txtFilter.Focus();
        }
        private void rbFilterRegex_Click(object sender, EventArgs e)
        {
            txtFilter.Focus();
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            ValidateFilter();
        }
        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
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
                txtFilter.Text = _activeFilter;
                EnableUpdates = true;

                _validFilter = true;
                txtFilter.BackColor = SystemColors.Window;
                toolTip.Hide(txtFilter);
                txtFilter.SelectionStart = _activeFilter.Length;

                e.SuppressKeyPress = true;  // prevent beep
            }
        }
        private void txtFilter_Leave(object sender, EventArgs e)
        {
            if (cbFilterExclude.Focused || rbFilterGlob.Focused || rbFilterRegex.Focused) return;

            EnableUpdates = false;
            txtFilter.Text = _activeFilter;
            EnableUpdates = true;

            _validFilter = true;
            txtFilter.BackColor = SystemColors.Window;
            toolTip.Hide(txtFilter);
        }
        private void cbFilterExclude_CheckedChanged(object sender, EventArgs e)
        {
            if (_validFilter)
                ApplyFilter();
            else
                txtFilter.Focus();
        }

        // context menu
        private void txtFilter_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                regExCtxMenu.ShowGlobMenu(txtFilter, rbFilterGlob.Checked, e.Location);
            }
        }
        private void txtFilter_MouseUp(object sender, MouseEventArgs e)
        {
            if (txtFilter.ContextMenuStrip != null)
                txtFilter.ContextMenuStrip = null;  // restore default cms
        }
    }
}
