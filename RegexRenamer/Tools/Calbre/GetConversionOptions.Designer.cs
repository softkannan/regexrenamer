using System.Windows.Forms;

namespace RegexRenamer.Forms
{
    partial class GetConversionOptions
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
            bttnOk = new Button();
            pgConversionOptions = new PropertyGrid();
            SuspendLayout();
            // 
            // bttnOk
            // 
            bttnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            bttnOk.Location = new System.Drawing.Point(814, 513);
            bttnOk.Name = "bttnOk";
            bttnOk.Size = new System.Drawing.Size(75, 34);
            bttnOk.TabIndex = 0;
            bttnOk.Text = "Ok";
            bttnOk.UseVisualStyleBackColor = true;
            // 
            // pgConversionOptions
            // 
            pgConversionOptions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pgConversionOptions.BackColor = System.Drawing.SystemColors.Control;
            pgConversionOptions.Location = new System.Drawing.Point(12, 12);
            pgConversionOptions.Name = "pgConversionOptions";
            pgConversionOptions.Size = new System.Drawing.Size(876, 491);
            pgConversionOptions.TabIndex = 1;
            // 
            // GetConversionOptions
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(901, 559);
            Controls.Add(pgConversionOptions);
            Controls.Add(bttnOk);
            Name = "GetConversionOptions";
            Text = "GetConversionOptions";
            ResumeLayout(false);
        }

        #endregion

        private Button bttnOk;
        private PropertyGrid pgConversionOptions;

    }
}