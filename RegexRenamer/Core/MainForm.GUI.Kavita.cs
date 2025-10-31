using Kavita.Enum;
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
        private LibraryType _kavitaPreviewLibType = LibraryType.Comic;
        private string _kavitaLibRootpath = string.Empty;
        private bool _kavitaUseMetadata = true;

        private void InitializeKavita()
        {
            useMetadataKavitaMenuItem.Checked = _kavitaUseMetadata;
            useMetadataKavitaMenuItem.Click += (s, e) =>
            {
                _kavitaUseMetadata = !_kavitaUseMetadata;
                useMetadataKavitaMenuItem.Checked = _kavitaUseMetadata;
                UpdatePreview();
            };
            noneKavitaMenuItem.Click += noneToolStripMenuItem_Click;
            previewComicsKavitaMenuItem.Click += previewComicsToolStripMenuItem_Click;
            previewMangaKavitaMenuItem.Click += previewMangaToolStripMenuItem_Click;
            previewBooksKavitaMenuItem.Click += previewBooksToolStripMenuItem_Click;
        }
        
        private void KavitaMenuItem(object sender)
        {
            if (!EnableUpdates) return;

            ToolStripMenuItem checkedMenuItem = (ToolStripMenuItem)sender;
            if (checkedMenuItem.Checked) return;  // already checked


            // update checked marks
            for (int i = 1; i < mnuKavitaCheck.DropDownItems.Count; i++)
            {
                if (i == 2) continue;  // seperator

                if (mnuKavitaCheck.DropDownItems[i] == checkedMenuItem)
                    ((ToolStripMenuItem)mnuKavitaCheck.DropDownItems[i]).Checked = true;
                else
                    ((ToolStripMenuItem)mnuKavitaCheck.DropDownItems[i]).Checked = false;
            }

            // set default match/replace values (if empty)
            if (checkedMenuItem != noneKavitaMenuItem)
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

            if (noneKavitaMenuItem.Checked)
            {
                mnuKavitaCheck.Font = new Font("Tahoma", 8.25F);
                mnuKavitaCheck.Padding = new Padding(0, 0, 8, 0);
                SetKavithaColVisibility(false);
                colFilename.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                colPreview.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
            {
                SetKavithaColVisibility(true);
                mnuKavitaCheck.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                mnuKavitaCheck.Padding = new Padding(0, 0, 0, 0);
                colFilename.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                colPreview.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                _kavitaPreviewLibType = LibraryType.Comic;

                if (previewComicsKavitaMenuItem.Checked)
                {
                    _kavitaPreviewLibType = LibraryType.Comic;
                }
                else if (previewMangaKavitaMenuItem.Checked)
                {
                    _kavitaPreviewLibType = LibraryType.Manga;
                }
                else if (previewBooksKavitaMenuItem.Checked)
                {
                    _kavitaPreviewLibType = LibraryType.Book;
                }
            }


            // update preview
            UpdatePreview();
            this.Update();
        }

        private void SetKavithaColVisibility(bool isKavithaColVisibile)
        {
            colSeries.Visible = isKavithaColVisibile;
            colVolume.Visible = isKavithaColVisibile;
            colChapter.Visible = isKavithaColVisibile;
            colTitle.Visible = isKavithaColVisibile;
            colSpecial.Visible = isKavithaColVisibile;
            colEdition.Visible = isKavithaColVisibile;
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KavitaMenuItem(sender);
        }

        private void previewComicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KavitaMenuItem(sender);
        }

        private void previewMangaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KavitaMenuItem(sender);
        }

        private void previewBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KavitaMenuItem(sender);
        }

    }
}
