using RegexRenamer.Utility;
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
        // context menus
        private Control lastControlRightClicked;
        private void InsertRegexFragment(object sender, EventArgs e)
        {
            // TODO MenuItem is no longer supported. Use ToolStripMenuItem instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
            InsertArgs ia = (InsertArgs)((ToolStripMenuItem)sender).Tag;
            TextBox textBox = null;
            ComboBox comboBox = null;

            int selectionStart, selectionLength;
            string text;

            if (lastControlRightClicked.GetType().Name == "TextBox")
            {
                textBox = (TextBox)lastControlRightClicked;
                selectionStart = textBox.SelectionStart;
                selectionLength = textBox.SelectionLength;
                text = textBox.Text;
            }
            else
            {
                comboBox = (ComboBox)lastControlRightClicked;
                selectionStart = comboBox.SelectionStart;
                selectionLength = comboBox.SelectionLength;
                text = comboBox.Text;
            }

            if (ia.InsertBefore == "" && selectionLength == 0)
            {
                ia.InsertBefore = ia.InsertAfter;
                ia.InsertAfter = "";
            }

            if (ia.WrapIfSelection && selectionLength > 0)
                if (ia.InsertAfter == "")
                    ia.InsertAfter = ia.InsertBefore;
                else
                    ia.InsertBefore = ia.InsertAfter;

            int group = 0;
            if (ia.GroupSelection && selectionLength > 0)
            {
                text = text.Insert(selectionStart, "(");
                selectionStart += 1;
                text = text.Insert(selectionStart + selectionLength, ")");
                group = 1;
            }

            if (selectionLength > 0 && (ia.InsertBefore == "" || ia.InsertAfter == "") && !ia.GroupSelection)
            {
                text = text.Remove(selectionStart, selectionLength);
                selectionLength = 0;
            }

            if (ia.InsertBefore != "")
            {
                text = text.Insert(selectionStart - group, ia.InsertBefore);
                selectionStart += ia.InsertBefore.Length;
            }
            if (ia.InsertAfter != "")
            {
                text = text.Insert(selectionStart + selectionLength + group, ia.InsertAfter);
            }
            if (ia.SelectionStartOffset > 0)
            {
                selectionStart = selectionStart - group - ia.InsertBefore.Length + ia.SelectionStartOffset;
            }
            if (ia.SelectionStartOffset < 0)
            {
                selectionStart = selectionStart + selectionLength + group + ia.InsertAfter.Length + ia.SelectionStartOffset;
            }
            if (ia.SelectionLength != -1)
                selectionLength = ia.SelectionLength;

            if (textBox != null)
            {
                textBox.SelectAll(); textBox.Paste(text);  // allow undo
                textBox.SelectionStart = selectionStart;
                textBox.SelectionLength = selectionLength;
            }
            else
            {
                comboBox.SelectAll(); comboBox.SelectedText = text;  // allow undo
                comboBox.SelectionStart = selectionStart;
                comboBox.SelectionLength = selectionLength;
            }
        }
        
        // misc helper methods

        private void ResetFields()
        {
            EnableUpdates = false;
            cmbMatch.Text = "";
            cmbReplace.Text = "";
            cbModifierI.Checked = false;
            cbModifierG.Checked = false;
            cbModifierX.Checked = false;
            EnableUpdates = true;
        }
        private void UpdateFileStats()
        {
            lblStatsTotal.Text = fileCount.total + " total";
            lblStatsShown.Text = fileCount.shown + " shown";
            lblStatsFiltered.Text = fileCount.filtered + " filtered";
            lblStatsHidden.Text = fileCount.hidden + " hidden";
        }
        private void SetFormActive(bool active)
        {
            EnableUpdates = active;
            tvwFolders.Enabled = active;
        }
        private string WrapText(string input, int maxlen)
        {
            if (input.Length <= maxlen) return input;

            int i = input.IndexOf(' ', maxlen);
            while (i > 0)
            {
                input = input.Insert(i + 1, "\n");

                if (input.Length <= i + 2 + maxlen)
                    i = -1;
                else
                    i = input.IndexOf(' ', i + 2 + maxlen);
            }

            return input;
        }
        private void UnFocusAll()
        {
            lblMatch.Focus();
        }
    }
}
