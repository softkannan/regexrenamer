using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Controls
{
    public class MyComboBox : ComboBox
    {
        public string newText = "";
        const int WM_PAINT = 0xf;

        protected override void WndProc(ref Message message)
        {
            // workaround to be able to set Text during SelectedIndexChanged event

            if (message.Msg == WM_PAINT && newText != "")
            {
                this.Text = newText;
                newText = "";
                this.SelectionStart = this.Text.Length;
            }

            base.WndProc(ref message);
        }
    }
}
