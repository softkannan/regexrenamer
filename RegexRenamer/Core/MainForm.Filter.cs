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
        // FILTER
        private void ApplyFilter()
        {
            if (!EnableUpdates || !validFilter) return;

            activeFilter = txtFilter.Text;
            UpdateFileList();
        }
        private void rbFilterRegex_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFilterGlob.Checked && (txtFilter.Text == ".*" || txtFilter.Text == ".+"))
            {
                EnableUpdates = false;
                txtFilter.Text = "*.*";
                activeFilter = "*.*";
                EnableUpdates = true;
            }
            else if (rbFilterRegex.Checked && (txtFilter.Text == "*.*" || txtFilter.Text == "*"))
            {
                EnableUpdates = false;
                txtFilter.Text = ".*";
                activeFilter = ".*";
                EnableUpdates = true;
            }
            else
            {
                activeFilter = "";
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
                if (validFilter)
                    e.SuppressKeyPress = true;  // prevent beep on enter if valid filter

                ApplyFilter();
            }
            else if (e.KeyCode == Keys.Escape)  // revert
            {
                EnableUpdates = false;
                txtFilter.Text = activeFilter;
                EnableUpdates = true;

                validFilter = true;
                txtFilter.BackColor = SystemColors.Window;
                toolTip.Hide(txtFilter);
                txtFilter.SelectionStart = activeFilter.Length;

                e.SuppressKeyPress = true;  // prevent beep
            }
        }
        private void txtFilter_Leave(object sender, EventArgs e)
        {
            if (cbFilterExclude.Focused || rbFilterGlob.Focused || rbFilterRegex.Focused) return;

            EnableUpdates = false;
            txtFilter.Text = activeFilter;
            EnableUpdates = true;

            validFilter = true;
            txtFilter.BackColor = SystemColors.Window;
            toolTip.Hide(txtFilter);
        }
        private void cbFilterExclude_CheckedChanged(object sender, EventArgs e)
        {
            if (validFilter)
                ApplyFilter();
            else
                txtFilter.Focus();
        }

        // context menu
        private void txtFilter_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                lastControlRightClicked = txtFilter;
                txtFilter.ContextMenuStrip = cmsBlank;  // prevent default cms from being displayed
                txtFilter.Focus();
                if (rbFilterGlob.Checked)
                    cmGlobMatch.Show(txtFilter, e.Location);
                else
                    cmRegexMatch.Show(txtFilter, e.Location);
            }
        }
        private void txtFilter_MouseUp(object sender, MouseEventArgs e)
        {
            if (txtFilter.ContextMenuStrip != null)
                txtFilter.ContextMenuStrip = null;  // restore default cms
        }
    }
}
