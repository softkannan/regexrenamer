namespace RegexRenamer.Tools.Translate
{
    partial class GoogleTranslatorForm
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
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            cmbServiceProvider = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            SuspendLayout();
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = System.Drawing.Color.White;
            webView.Location = new System.Drawing.Point(12, 41);
            webView.Name = "webView";
            webView.Size = new System.Drawing.Size(1130, 656);
            webView.TabIndex = 0;
            webView.ZoomFactor = 1D;
            // 
            // cmbServiceProvider
            // 
            cmbServiceProvider.FormattingEnabled = true;
            cmbServiceProvider.Location = new System.Drawing.Point(12, 12);
            cmbServiceProvider.Name = "cmbServiceProvider";
            cmbServiceProvider.Size = new System.Drawing.Size(239, 23);
            cmbServiceProvider.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(257, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(654, 15);
            label1.TabIndex = 2;
            label1.Text = "File names are copied to clipboard, paste the names to translate then copy the translated text to clipboard for file renaming";
            // 
            // GoogleTranslatorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1154, 709);
            Controls.Add(label1);
            Controls.Add(cmbServiceProvider);
            Controls.Add(webView);
            Name = "GoogleTranslatorForm";
            ShowInTaskbar = false;
            Text = "Google Translator";
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.ComboBox cmbServiceProvider;
        private System.Windows.Forms.Label label1;
    }
}