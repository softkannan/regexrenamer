using Config;
using Microsoft.Win32;
using PdfSharp.Pdf.Content.Objects;
using RegexRenamer.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer
{
    public partial class MainForm
    {
        private void InitializeOptionsHelp()
        {
            itmOptionsShowHidden.Click += itmOptionsShowHidden_Click;
            itmOptionsPreserveExt.Click += itmOptionsPreserveExt_Click;
            itmOptionsAllowRenSub.Click += itmOptionsAllowRenSub_Click;
            itmOptionsEditConfig.Click += itmOptionsEditConfig_Click;
            // Adds the context menu option to Windows Explorer (requires admin rights)
            itmOptionsAddContextMenu.Enabled = false; 
            itmOptionsAddContextMenu.Click += itmOptionsAddContextMenu_Click;

            itmHelpContents.Click += itmHelpContents_Click;
            itmHelpRegexReference.Click += itmHelpRegexReference_Click;
            itmHelpEmailAuthor.Click += itmHelpEmailAuthor_Click;
            itmHelpReportBug.Click += itmHelpReportBug_Click;
            itmHelpHomepage.Click += itmHelpHomepage_Click;
            itmHelpAbout.Click += itmHelpAbout_Click;

            chkShowInfo.Click += ChkShowInfo_Click;

        }

       
        private void SetShowInfoColumnVisibility(bool visible)
        {
            colModified.Visible = visible;
            colFileSize.Visible = visible;
            colExt.Visible = visible;
        }

        private void ChkShowInfo_Click(object sender, EventArgs e)
        {
            if (!EnableUpdates) return;

            SetShowInfoColumnVisibility(chkShowInfo.Checked);

            // update preview
            this.Update();
            UpdatePreview();
        }

        // OPTIONS/HELP

        // options
        private async void itmOptionsEditConfig_Click(object sender, EventArgs e)
        {
            string configPath = UserConfig.ConfigFile;
            // Launch notepad++ to edit config file
            // Supports {options} {filepath} variables
            var cmdName = "LaunchEditor";
            await cmdName.ExecNamedCmdAsync(Path.GetDirectoryName(configPath), new List<Tuple<string, string>>() {
                   new Tuple<string, string>("{options}", "-ljson5"),
                   new Tuple<string, string>("{filepath}", configPath),
                });
        }

        private void itmOptionsShowHidden_Click(object sender, EventArgs e)
        {
            this.Update();
            UpdateFileList();
        }
        private void itmOptionsPreserveExt_Click(object sender, EventArgs e)
        {
            this.Update();

            // update activeFiles
            for (int afi = 0; afi < _fileStore.Files.Count; afi++)
                _fileStore.Files[afi].PreserveExt = itmOptionsPreserveExt.Checked;

            // update filename column
            for (int dfi = 0; dfi < dgvFiles.Rows.Count; dfi++)
                dgvFiles.Rows[dfi].Cells[1].Value = _fileStore.Files[(int)dgvFiles.Rows[dfi].Tag].Name;

            // update preview column
            UpdatePreview();
        }
        private void itmOptionsAllowRenSub_Click(object sender, EventArgs e)
        {
            this.Update();
            UpdateValidation();
        }
        private void itmOptionsAddContextMenu_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // help
        private void itmHelpContents_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Path.Combine(Application.StartupPath, "RegexRenamer.chm"), "html/index.html");
        }
        private void itmHelpRegexReference_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Path.Combine(Application.StartupPath, "Regex Quick Reference.chm"), "html/regex_quickref2.html");
        }
        private void itmHelpEmailAuthor_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("mailto:xiperware@gmail.com");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void itmHelpReportBug_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://sourceforge.net/tracker/?func=add&group_id=177064&atid=879743");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void itmHelpHomepage_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://regexrenamer.sourceforge.net/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void itmHelpAbout_Click(object sender, EventArgs e)
        {
            if (_aboutForm == null)
                _aboutForm = new About();

            _aboutForm.SetStats(_countProgLaunches, _countFilesRenamed);
            _aboutForm.ShowDialog(this);
        }
    }
}
