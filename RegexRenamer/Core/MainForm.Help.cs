using Microsoft.Win32;
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
        // OPTIONS/HELP

        // options
        private void itmOptionsShowHidden_Click(object sender, EventArgs e)
        {
            this.Update();
            UpdateFileList();
        }
        private void itmOptionsPreserveExt_Click(object sender, EventArgs e)
        {
            this.Update();

            // update activeFiles
            for (int afi = 0; afi < activeFiles.Count; afi++)
                activeFiles[afi].PreserveExt = itmOptionsPreserveExt.Checked;

            // update filename column
            for (int dfi = 0; dfi < dgvFiles.Rows.Count; dfi++)
                dgvFiles.Rows[dfi].Cells[1].Value = activeFiles[(int)dgvFiles.Rows[dfi].Tag].Name;

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
                if (!itmOptionsAddContextMenu.Checked)  // add key
                {
                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("Folder\\shell\\RegexRenamer"))
                    {
                        if (key != null)
                        {
                            key.SetValue("", "Rename using RegexRenamer");
                            key.Close();
                        }
                    }
                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("Folder\\shell\\RegexRenamer\\command"))
                    {
                        if (key != null)
                        {
                            key.SetValue("", Application.ExecutablePath + " \"%L\"");
                            key.Close();
                        }
                    }
                    itmOptionsAddContextMenu.Checked = true;
                }
                else  // delete key
                {
                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey("Folder\\shell\\RegexRenamer"))
                    {
                        if (key != null)  // make sure exists before trying to delete
                        {
                            key.Close();
                            Registry.ClassesRoot.DeleteSubKeyTree("Folder\\shell\\RegexRenamer");
                        }
                    }
                    itmOptionsAddContextMenu.Checked = false;
                }
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
            if (aboutForm == null)
                aboutForm = new About();

            aboutForm.SetStats(countProgLaunches, countFilesRenamed);
            aboutForm.ShowDialog(this);
        }
    }
}
