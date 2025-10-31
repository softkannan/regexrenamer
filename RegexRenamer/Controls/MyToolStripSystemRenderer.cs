using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Controls
{
    public class MyToolStripSystemRenderer : ToolStripSystemRenderer
    {
        // only draw border on menus (not toolbar itself)
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (!(e.ToolStrip is ToolStrip))
                base.OnRenderToolStripBorder(e);
        }

        // draw text labels in image margin
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip.Items[0].Name == "txtNumberingStart")  // numbering menu
            {
                Brush brush = new SolidBrush(SystemColors.WindowText);

                e.Graphics.DrawString("Start:", e.ToolStrip.Font, brush, 3, e.ToolStrip.Items[0].Bounds.Top + 4);
                e.Graphics.DrawString("Pad:", e.ToolStrip.Font, brush, 3, e.ToolStrip.Items[1].Bounds.Top + 4);
                e.Graphics.DrawString("Inc:", e.ToolStrip.Font, brush, 3, e.ToolStrip.Items[2].Bounds.Top + 4);
                e.Graphics.DrawString("Reset:", e.ToolStrip.Font, brush, 3, e.ToolStrip.Items[3].Bounds.Top + 4);
            }

            base.OnRenderImageMargin(e);
        }
    }
}
