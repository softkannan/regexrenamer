using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Forms;

public static class GUIExtensions
{
    [DllImport("user32.dll")]
    private static extern int ShowWindow(IntPtr hWnd, uint Msg);
    private const uint SW_RESTORE = 0x09;
    public static void Restore(this Form form)
    {
        if (form.WindowState == FormWindowState.Minimized)
        {
            ShowWindow(form.Handle, SW_RESTORE);
        }
    }
    public static void CopySelectionsToClipboard(this ListBox pThis)
    {
        if (pThis.SelectedItems.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in pThis.SelectedItems)
            {
                sb.AppendLine(item.ToString());
            }
            Clipboard.SetText(sb.ToString());
        }
    }

    public static void CopySelectionsToClipboard(this ListView pThis)
    {
        if (pThis.SelectedItems.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ListViewItem item in pThis.SelectedItems)
            {
                sb.Append(item.Text);
                for (int index=1;index<item.SubItems.Count;index++)
                {
                    sb.Append("\t");
                    sb.Append(item.SubItems[index].Text);
                }
                sb.AppendLine();
            }
            Clipboard.SetText(sb.ToString());
        }
    }

    //public static List<ToolStripMenuItem> GetAllMenuItems(this CCFile pThis)
    //{
    //    var items = new List<ToolStripMenuItem>();
    //    if (pThis == null || pThis.Versions == null || pThis.Versions.Count == 0)
    //        return items;
    //    // Add base version "0" to the menu items
    //    var menuItem = new ToolStripMenuItem("0");
    //    var verInfo = pThis.ParseVersionInfo(-2);
    //    menuItem.Tag = verInfo;
    //    items.Add(menuItem);
    //    for (int index= pThis.Versions.Count - 1; index >= 0 ; index--)
    //    {
    //        verInfo = pThis.ParseVersionInfo(index);
    //        menuItem = new ToolStripMenuItem(verInfo.Item2);
    //        menuItem.Tag = verInfo;
    //        items.Add(menuItem);
    //    }
    //    return items;
    //}
}
