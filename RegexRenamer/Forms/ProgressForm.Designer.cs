namespace RegexRenamer.Forms
{
    partial class ProgressForm
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
            this.progressBarCCmd = new System.Windows.Forms.ProgressBar();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // progressBarCCmd
            // 
            this.progressBarCCmd.Location = new System.Drawing.Point(12, 146);
            this.progressBarCCmd.Name = "progressBarCCmd";
            this.progressBarCCmd.Size = new System.Drawing.Size(776, 43);
            this.progressBarCCmd.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCCmd.TabIndex = 0;
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(12, 14);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(776, 126);
            this.txtInfo.TabIndex = 1;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 201);
            this.ControlBox = false;
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.progressBarCCmd);
            this.Name = "ProgressForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProgressForm";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarCCmd;
        private System.Windows.Forms.TextBox txtInfo;
    }
}