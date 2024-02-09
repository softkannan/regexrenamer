using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer
{
    public partial class MainForm
    {
        #region File View context menu
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> selectedFiles = new List<int>();
            foreach (DataGridViewRow row in dgvFiles.Rows)
            {
                if (row.Selected)
                {
                    int afi = (int)row.Tag;
                    selectedFiles.Add(afi);
                    //
                }
            }

            // no files need renaming

            if (selectedFiles.Count > 0)
            {
                SetFormActive(false);
                foreach (var afi in selectedFiles)
                {
                    var file = activeFiles[afi];
                    MessageBox.Show(file.Fullpath);
                    //File.Delete(file.Fullpath);
                }
            }

            SetFormActive(true);
            UpdateFileList();
            cmbMatch.Focus();
            //foreach (RRItem file in activeFiles)
            //{
            //    if (itmOptionsRenameSelectedRows.Checked && !file.Selected)
            //        continue;  // ignore unselected rows

            //    if ((itmOutputRenameInPlace.Checked && file.Name != file.Preview)
            //     || (!itmOutputRenameInPlace.Checked && file.Matched))
            //        filesToRename++;
            //}


        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
