﻿using RegexRenamer.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer
{
    public partial class MainForm
    {
        // regex match evaluator for changing case

        private string MatchEvalChangeCase(Match match)
        {
            TextInfo ti = new CultureInfo("en").TextInfo;

            if (itmChangeCaseUppercase.Checked) return ti.ToUpper(match.Groups[1].Value);
            else if (itmChangeCaseLowercase.Checked) return ti.ToLower(match.Groups[1].Value);
            else if (itmChangeCaseTitlecase.Checked) return ti.ToTitleCase(match.Groups[1].Value.ToLower());
            else if (itmChangeCaseCleanName.Checked) return match.Groups[1].Value.ToCleanFileName();
            else return match.Groups[1].Value;
        }

        // UPPER MENUS

        // change case
        private void ChangeCaseMenuItem(object sender)
        {
            if (!EnableUpdates) return;

            ToolStripMenuItem checkedMenuItem = (ToolStripMenuItem)sender;
            if (checkedMenuItem.Checked) return;  // already checked


            // update checked marks

            for (int i = 0; i < mnuChangeCase.DropDownItems.Count; i++)
            {
                if (i == 1) continue;  // seperator

                if (mnuChangeCase.DropDownItems[i] == checkedMenuItem)
                    ((ToolStripMenuItem)mnuChangeCase.DropDownItems[i]).Checked = true;
                else
                    ((ToolStripMenuItem)mnuChangeCase.DropDownItems[i]).Checked = false;
            }


            // set default match/replace values (if empty)

            if (checkedMenuItem != itmChangeCaseNoChange)
            {
                if (cmbMatch.Text == "")
                {
                    EnableUpdates = false;
                    cmbMatch.Text = ".*";
                    EnableUpdates = true;
                }
                if (cmbReplace.Text == "")
                {
                    EnableUpdates = false;
                    cmbReplace.Text = "$0";
                    EnableUpdates = true;
                }
            }


            // set button text to bold if an option selected

            if (itmChangeCaseNoChange.Checked)
            {
                mnuChangeCase.Font = new Font("Tahoma", 8.25F);
                mnuChangeCase.Padding = new Padding(0, 0, 8, 0);
            }
            else
            {
                mnuChangeCase.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                mnuChangeCase.Padding = new Padding(0, 0, 0, 0);
            }


            // update preview

            this.Update();
            UpdatePreview();
        }

        private void itmChangeCaseNoChange_Click(object sender, EventArgs e)
        {
            ChangeCaseMenuItem(sender);
        }
        private void itmChangeCaseUppercase_Click(object sender, EventArgs e)
        {
            ChangeCaseMenuItem(sender);
        }
        private void itmChangeCaseLowercase_Click(object sender, EventArgs e)
        {
            ChangeCaseMenuItem(sender);
        }
        private void itmChangeCaseTitlecase_Click(object sender, EventArgs e)
        {
            ChangeCaseMenuItem(sender);
        }
        private void mnuChangeCase_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && !itmChangeCaseNoChange.Checked)  // set default
                itmChangeCaseNoChange.PerformClick();
        }

        private void itmChangeCaseCleanName_Click(object sender, EventArgs e)
        {
            ChangeCaseMenuItem(sender);
        }
    }
}
