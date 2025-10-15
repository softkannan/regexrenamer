using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Forms
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        public void Initialize(string title, string message)
        {
            this.Text = title;
            txtInfo.Text = message;
            progressBarCCmd.Style = ProgressBarStyle.Marquee;
            progressBarCCmd.MarqueeAnimationSpeed = 30; // Adjust speed as needed
        }

    }
}
