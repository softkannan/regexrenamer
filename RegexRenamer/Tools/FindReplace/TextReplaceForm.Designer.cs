using RegexRenamer.Controls.ListViewExCtrl;

namespace RegexRenamer.Forms
{
    partial class TextReplaceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            bttnOk = new System.Windows.Forms.Button();
            cbTemplateNames = new System.Windows.Forms.ComboBox();
            lstFoundItems = new RegexRenamer.Controls.ListViewExCtrl.ListViewEx();
            colFoundItem = new System.Windows.Forms.ColumnHeader();
            colPreview = new System.Windows.Forms.ColumnHeader();
            bttnCancel = new System.Windows.Forms.Button();
            cbReplacePattern = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            cbSearchPattern = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            bttnAutoProcess = new RegexRenamer.Controls.SplitButton();
            cmProcess = new System.Windows.Forms.ContextMenuStrip(components);
            updatePreviewCMProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            processCMProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            skipProcessingCMProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            enterInteractiveModeCMProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            resetCMProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            chkIgnoreCase = new System.Windows.Forms.CheckBox();
            txtPreview = new System.Windows.Forms.RichTextBox();
            exitInteractiveModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            cmProcess.SuspendLayout();
            SuspendLayout();
            // 
            // bttnOk
            // 
            bttnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            bttnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            bttnOk.Location = new System.Drawing.Point(1156, 735);
            bttnOk.Name = "bttnOk";
            bttnOk.Size = new System.Drawing.Size(155, 45);
            bttnOk.TabIndex = 1;
            bttnOk.Text = "Ok";
            bttnOk.UseVisualStyleBackColor = true;
            // 
            // cbTemplateNames
            // 
            cbTemplateNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbTemplateNames.FormattingEnabled = true;
            cbTemplateNames.Location = new System.Drawing.Point(137, 9);
            cbTemplateNames.Name = "cbTemplateNames";
            cbTemplateNames.Size = new System.Drawing.Size(292, 23);
            cbTemplateNames.TabIndex = 3;
            // 
            // lstFoundItems
            // 
            lstFoundItems.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lstFoundItems.CheckBoxes = true;
            lstFoundItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { colFoundItem, colPreview });
            lstFoundItems.FullRowSelect = true;
            lstFoundItems.GridLines = true;
            lstFoundItems.Location = new System.Drawing.Point(12, 355);
            lstFoundItems.Name = "lstFoundItems";
            lstFoundItems.Size = new System.Drawing.Size(1299, 373);
            lstFoundItems.TabIndex = 4;
            lstFoundItems.UseCompatibleStateImageBehavior = false;
            lstFoundItems.View = System.Windows.Forms.View.Details;
            // 
            // colFoundItem
            // 
            colFoundItem.Text = "Found";
            // 
            // colPreview
            // 
            colPreview.Text = "Preview";
            // 
            // bttnCancel
            // 
            bttnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            bttnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            bttnCancel.Location = new System.Drawing.Point(995, 735);
            bttnCancel.Name = "bttnCancel";
            bttnCancel.Size = new System.Drawing.Size(155, 45);
            bttnCancel.TabIndex = 2;
            bttnCancel.Text = "Cancel";
            bttnCancel.UseVisualStyleBackColor = true;
            // 
            // cbReplacePattern
            // 
            cbReplacePattern.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbReplacePattern.FormattingEnabled = true;
            cbReplacePattern.Location = new System.Drawing.Point(998, 9);
            cbReplacePattern.Name = "cbReplacePattern";
            cbReplacePattern.Size = new System.Drawing.Size(313, 23);
            cbReplacePattern.TabIndex = 5;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label1.Location = new System.Drawing.Point(896, 12);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(96, 14);
            label1.TabIndex = 6;
            label1.Text = "Replace Pattern:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label2.Location = new System.Drawing.Point(445, 12);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(99, 14);
            label2.TabIndex = 8;
            label2.Text = "Search Pattern:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbSearchPattern
            // 
            cbSearchPattern.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cbSearchPattern.FormattingEnabled = true;
            cbSearchPattern.Location = new System.Drawing.Point(550, 9);
            cbSearchPattern.Name = "cbSearchPattern";
            cbSearchPattern.Size = new System.Drawing.Size(305, 23);
            cbSearchPattern.TabIndex = 7;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(12, 15);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(119, 14);
            label3.TabIndex = 9;
            label3.Text = "Process Template:";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bttnAutoProcess
            // 
            bttnAutoProcess.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            bttnAutoProcess.AutoSize = true;
            bttnAutoProcess.ContextMenuStrip = cmProcess;
            bttnAutoProcess.Location = new System.Drawing.Point(795, 735);
            bttnAutoProcess.Name = "bttnAutoProcess";
            bttnAutoProcess.ShowMenuAlways = true;
            bttnAutoProcess.Size = new System.Drawing.Size(197, 45);
            bttnAutoProcess.TabIndex = 14;
            bttnAutoProcess.Text = "Process";
            bttnAutoProcess.UseVisualStyleBackColor = true;
            // 
            // cmProcess
            // 
            cmProcess.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { updatePreviewCMProcessToolStripMenuItem, processCMProcessToolStripMenuItem, skipProcessingCMProcessToolStripMenuItem, enterInteractiveModeCMProcessToolStripMenuItem, exitInteractiveModeToolStripMenuItem, resetCMProcessToolStripMenuItem });
            cmProcess.Name = "cmProcess";
            cmProcess.Size = new System.Drawing.Size(194, 158);
            // 
            // updatePreviewCMProcessToolStripMenuItem
            // 
            updatePreviewCMProcessToolStripMenuItem.Name = "updatePreviewCMProcessToolStripMenuItem";
            updatePreviewCMProcessToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            updatePreviewCMProcessToolStripMenuItem.Text = "Update Preview";
            // 
            // processCMProcessToolStripMenuItem
            // 
            processCMProcessToolStripMenuItem.Name = "processCMProcessToolStripMenuItem";
            processCMProcessToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            processCMProcessToolStripMenuItem.Text = "Process";
            // 
            // skipProcessingCMProcessToolStripMenuItem
            // 
            skipProcessingCMProcessToolStripMenuItem.Name = "skipProcessingCMProcessToolStripMenuItem";
            skipProcessingCMProcessToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            skipProcessingCMProcessToolStripMenuItem.Text = "Skip Processing";
            // 
            // enterInteractiveModeCMProcessToolStripMenuItem
            // 
            enterInteractiveModeCMProcessToolStripMenuItem.Name = "enterInteractiveModeCMProcessToolStripMenuItem";
            enterInteractiveModeCMProcessToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            enterInteractiveModeCMProcessToolStripMenuItem.Text = "Enter Interactive Mode";
            // 
            // resetCMProcessToolStripMenuItem
            // 
            resetCMProcessToolStripMenuItem.Name = "resetCMProcessToolStripMenuItem";
            resetCMProcessToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            resetCMProcessToolStripMenuItem.Text = "Reset";
            // 
            // chkIgnoreCase
            // 
            chkIgnoreCase.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            chkIgnoreCase.AutoSize = true;
            chkIgnoreCase.Location = new System.Drawing.Point(861, 11);
            chkIgnoreCase.Name = "chkIgnoreCase";
            chkIgnoreCase.Size = new System.Drawing.Size(29, 19);
            chkIgnoreCase.TabIndex = 15;
            chkIgnoreCase.Text = "i";
            chkIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // txtPreview
            // 
            txtPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtPreview.DetectUrls = false;
            txtPreview.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            txtPreview.Location = new System.Drawing.Point(12, 38);
            txtPreview.Name = "txtPreview";
            txtPreview.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            txtPreview.Size = new System.Drawing.Size(1299, 311);
            txtPreview.TabIndex = 16;
            txtPreview.Text = "";
            // 
            // exitInteractiveModeToolStripMenuItem
            // 
            exitInteractiveModeToolStripMenuItem.Name = "exitInteractiveModeToolStripMenuItem";
            exitInteractiveModeToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            exitInteractiveModeToolStripMenuItem.Text = "Exit Interactive Mode";
            // 
            // TextReplaceForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1323, 792);
            Controls.Add(txtPreview);
            Controls.Add(chkIgnoreCase);
            Controls.Add(bttnAutoProcess);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cbSearchPattern);
            Controls.Add(label1);
            Controls.Add(cbReplacePattern);
            Controls.Add(lstFoundItems);
            Controls.Add(cbTemplateNames);
            Controls.Add(bttnCancel);
            Controls.Add(bttnOk);
            MinimizeBox = false;
            Name = "TextReplaceForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Find and Replace";
            cmProcess.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button bttnOk;
        private System.Windows.Forms.ComboBox cbTemplateNames;
        private ListViewEx lstFoundItems;
        private System.Windows.Forms.Button bttnCancel;
        private System.Windows.Forms.ColumnHeader colFoundItem;
        private System.Windows.Forms.ComboBox cbReplacePattern;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSearchPattern;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader colPreview;
        private Controls.SplitButton bttnAutoProcess;
        private System.Windows.Forms.ContextMenuStrip cmProcess;
        private System.Windows.Forms.ToolStripMenuItem processCMProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enterInteractiveModeCMProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetCMProcessToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkIgnoreCase;
        private System.Windows.Forms.RichTextBox txtPreview;
        private System.Windows.Forms.ToolStripMenuItem skipProcessingCMProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updatePreviewCMProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitInteractiveModeToolStripMenuItem;
    }
}