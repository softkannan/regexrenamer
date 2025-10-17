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
        // letter sequences
        private void InitializeNumbering()
        {
            txtNumberingStart.TextChanged += txtNumberingStart_TextChanged;
            txtNumberingPad.TextChanged += txtNumberingPad_TextChanged;
            txtNumberingInc.TextChanged += txtNumberingInc_TextChanged;
            txtNumberingReset.TextChanged += txtNumberingReset_TextChanged;
            mnuNumbering.MouseDown += mnuNumbering_MouseDown;

        }
        
        // numbering
        private void NumberingMenuItem(object sender)
        {
            ToolStripTextBox textBox = (ToolStripTextBox)sender;
            bool error = false;
            int num;

            if (textBox == txtNumberingPad && textBox.Text == "")
            {
                EnableUpdates = false;
                textBox.Text = "0";  // default: no padding
                EnableUpdates = true;
            }


            // parse int, check valid range

            if (!Int32.TryParse(textBox.Text, out num))
                error = true;
            else if (textBox == txtNumberingStart && num < 0)
                error = true;
            else if (textBox == txtNumberingPad && num != 0)
                error = true;
            else if (textBox == txtNumberingInc && num == 0)
                error = true;
            else if (textBox == txtNumberingReset && num < 0)
                error = true;


            // or, check for letter(s)

            if (textBox == txtNumberingStart)
            {
                if (Regex.IsMatch(textBox.Text, @"^([a-z]+|[A-Z]+)$"))
                {
                    error = false;
                    this.txtNumberingPad.Enabled = false;
                }
                else if (!this.txtNumberingPad.Enabled)
                {
                    this.txtNumberingPad.Enabled = true;
                }
            }


            // set bg colour

            if (error)
                textBox.BackColor = Color.MistyRose;
            else
                textBox.BackColor = SystemColors.Window;


            // if all valid, update preview

            textBox.Tag = !error;
            _validNumber = (bool)mnuNumbering.DropDownItems[0].Tag
                        && (bool)mnuNumbering.DropDownItems[1].Tag
                        && (bool)mnuNumbering.DropDownItems[2].Tag
                        && (bool)mnuNumbering.DropDownItems[3].Tag;

            if (_validNumber)
                UpdatePreview();
        }
        private void txtNumberingStart_TextChanged(object sender, EventArgs e)
        {
            NumberingMenuItem(sender);
        }
        private void txtNumberingPad_TextChanged(object sender, EventArgs e)
        {
            NumberingMenuItem(sender);
        }
        private void txtNumberingInc_TextChanged(object sender, EventArgs e)
        {
            NumberingMenuItem(sender);
        }
        private void txtNumberingReset_TextChanged(object sender, EventArgs e)
        {
            NumberingMenuItem(sender);
        }
        private void mnuNumbering_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)  // set defaults
            {
                EnableUpdates = false;
                if (txtNumberingStart.Text != "1") txtNumberingStart.Text = "1";
                if (txtNumberingPad.Text != "000") txtNumberingPad.Text = "000";
                if (txtNumberingInc.Text != "1") txtNumberingInc.Text = "1";
                if (txtNumberingReset.Text != "0") txtNumberingReset.Text = "0";
                EnableUpdates = true;

                UpdatePreview();
            }
        }
    }
}
