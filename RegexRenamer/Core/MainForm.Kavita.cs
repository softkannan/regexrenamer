﻿using RegexRenamer.Kavita;
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
        private void setAsKavitaLibraryRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setAsKavitaLibraryRootToolStripMenuItem.Tag = ActivePath;
        }
        private void UpdateKavitaCheck(RRItem match, string kavithaRoot, Kavita.LibraryType libType)
        {
            var parseInfo = parser.Parse(match.PreviewFullPath, kavithaRoot, libType);
            match.Info = parseInfo;
        }
        private void KavitaMenuItem(object sender)
        {
            if (!EnableUpdates) return;

            ToolStripMenuItem checkedMenuItem = (ToolStripMenuItem)sender;
            if (checkedMenuItem.Checked) return;  // already checked


            // update checked marks

            for (int i = 0; i < mnuKavitaCheck.DropDownItems.Count; i++)
            {
                if (i == 1) continue;  // seperator

                if (mnuKavitaCheck.DropDownItems[i] == checkedMenuItem)
                    ((ToolStripMenuItem)mnuKavitaCheck.DropDownItems[i]).Checked = true;
                else
                    ((ToolStripMenuItem)mnuKavitaCheck.DropDownItems[i]).Checked = false;
            }


            // set default match/replace values (if empty)

            if (checkedMenuItem != noneToolStripMenuItem)
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

            if (noneToolStripMenuItem.Checked)
            {
                mnuKavitaCheck.Font = new Font("Tahoma", 8.25F);
                mnuKavitaCheck.Padding = new Padding(0, 0, 8, 0);
                SetKavithaColVisibility(false);
            }
            else
            {
                SetKavithaColVisibility(true);
                mnuKavitaCheck.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                mnuKavitaCheck.Padding = new Padding(0, 0, 0, 0);

                curLibType = LibraryType.Comic;

                if (previewComicsToolStripMenuItem.Checked)
                {
                    curLibType = LibraryType.Comic;
                }
                else if (previewMangaToolStripMenuItem.Checked)
                {
                    curLibType = LibraryType.Manga;
                }
                else if (previewBooksToolStripMenuItem.Checked)
                {
                    curLibType = LibraryType.Book;
                }
            }


            // update preview

            this.Update();
            UpdatePreview();
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
