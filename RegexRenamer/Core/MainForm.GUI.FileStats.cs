using RegexRenamer.Utility.RegexMenu;
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
        RegExContextMenuProvider regExCtxMenu;

        private void InitializeInsertRegExContextMenu()
        {
            regExCtxMenu = new RegExContextMenuProvider(components);
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
            lblStatsTotal.Text = _fileStore.Stats.Total + " total";
            lblStatsShown.Text = _fileStore.Stats.Shown + " shown";
            lblStatsFiltered.Text = _fileStore.Stats.Filtered + " filtered";
            lblStatsHidden.Text = _fileStore.Stats.Hidden + " hidden";
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
