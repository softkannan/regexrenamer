namespace RegexRenamer.Forms
{
    partial class MessageBoxForm
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
            lblMsg = new System.Windows.Forms.Label();
            flowLayoutPanelBttns = new System.Windows.Forms.FlowLayoutPanel();
            SuspendLayout();
            // 
            // lblMsg
            // 
            lblMsg.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblMsg.Location = new System.Drawing.Point(12, 9);
            lblMsg.Name = "lblMsg";
            lblMsg.Size = new System.Drawing.Size(485, 67);
            lblMsg.TabIndex = 0;
            lblMsg.Text = "label1";
            // 
            // flowLayoutPanelBttns
            // 
            flowLayoutPanelBttns.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanelBttns.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowLayoutPanelBttns.Location = new System.Drawing.Point(12, 82);
            flowLayoutPanelBttns.Name = "flowLayoutPanelBttns";
            flowLayoutPanelBttns.Size = new System.Drawing.Size(485, 31);
            flowLayoutPanelBttns.TabIndex = 1;
            flowLayoutPanelBttns.WrapContents = false;
            // 
            // MessageBoxForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(509, 125);
            Controls.Add(flowLayoutPanelBttns);
            Controls.Add(lblMsg);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MessageBoxForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "MessageBoxForm";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelBttns;
    }
}