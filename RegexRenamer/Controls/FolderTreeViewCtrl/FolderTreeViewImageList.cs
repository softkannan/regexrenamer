using RegexRenamer.Controls.FolderTreeViewCtrl.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel.VoiceCommands;

namespace RegexRenamer.Controls.FolderTreeViewCtrl;

public class FolderTreeViewImageList
{
    public ImageList Icons { get; private set; } = new ImageList()
    {
        ColorDepth = ColorDepth.Depth32Bit,
        ImageSize = new Size(16, 16),
        TransparentColor = Color.Transparent
    };

    private List<Icon> _iconsCache = new List<Icon>();

    public FolderTreeViewImageList()
    {
        InitializeDefaultIcons();
    }

    private void InitializeDefaultIcons()
    {
        try
        {
            AddIcon("DefaultDESKTOPDIRECTORY", ExtractIconsAPI.GetDesktopIcon(), true);
            AddIcon("DefaultDESKTOPDIRECTORY", ExtractIconsAPI.GetDesktopIcon(), false);
            AddIcon("DefaultFolder", FileIconAPI.GetDefaultFolderIcon(false), false);
            AddIcon("DefaultFolder", FileIconAPI.GetDefaultFolderIcon(true), true);
        }
        catch
        {
            // Create a blank icon if the desktop icon fails for some reason
            Bitmap bmp = new Bitmap(16, 16);
            Image img = bmp;
            Icon icon = Icon.FromHandle(((Bitmap)img).GetHicon());
            AddIcon("DefaultDESKTOPDIRECTORY", icon, true);
            icon = Icon.FromHandle(((Bitmap)img).GetHicon());
            AddIcon("DefaultDESKTOPDIRECTORY", icon, false);
            icon = Icon.FromHandle(((Bitmap)img).GetHicon());
            AddIcon("DefaultFolder", icon, true);
            icon = Icon.FromHandle(((Bitmap)img).GetHicon());
            AddIcon("DefaultFolder", icon, false);
            bmp.Dispose();
        }
    }

    private Dictionary<string,int> KnownIndexsNormal { get; set; } = new Dictionary<string,int>();
    private Dictionary<string, int> KnownIndexsSelected { get; set; } = new Dictionary<string, int>();

    public int GetIcon(string name, bool isNormal = true)
    {
        if (isNormal && KnownIndexsNormal.TryGetValue(name, out int index)) return index;
        if (!isNormal && KnownIndexsSelected.TryGetValue(name, out int selectedIndex)) return selectedIndex;
        return -1;
    }

    public int AddIcon(string name, Icon icon, bool isSelected = true)
    {
        if (isSelected && KnownIndexsSelected.TryGetValue(name, out int selectedIndex)) return selectedIndex;
        if (isSelected == false && KnownIndexsNormal.TryGetValue(name, out int index)) return index;

        Icons.Images.Add(icon);
        _iconsCache.Add(icon);
        if (isSelected)
            KnownIndexsSelected.Add(name, Icons.Images.Count - 1);
        else
            KnownIndexsNormal.Add(name, Icons.Images.Count - 1);

        return Icons.Images.Count - 1;
    }

   

    public void CleanUp()
    {
        Icons.Images.Clear();
        KnownIndexsNormal.Clear();
        KnownIndexsSelected.Clear();

        for(int idx = 0; idx < _iconsCache.Count; idx++)
        {
            if (_iconsCache[idx].Handle != IntPtr.Zero)
            {
                // Call DestroyIcon to release unmanaged resources
                ExtractIconsAPI.DestroyIcon(_iconsCache[idx].Handle);
            }
            _iconsCache[idx].Dispose();
        }
        _iconsCache.Clear();
        InitializeDefaultIcons();
    }
}
