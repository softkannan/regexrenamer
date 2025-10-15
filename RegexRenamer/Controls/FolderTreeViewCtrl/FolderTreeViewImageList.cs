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
    private Dictionary<string,int> KnownIndexsNormal { get; set; } = new Dictionary<string,int>();
    private Dictionary<string, int> KnownIndexsSelected { get; set; } = new Dictionary<string, int>();

    public int GetIcon(string name, bool isNormal = true)
    {
        if (isNormal && KnownIndexsNormal.TryGetValue(name, out int index)) return index;
        if (!isNormal && KnownIndexsSelected.TryGetValue(name, out int selectedIndex)) return selectedIndex;
        return -1;
    }

    public int AddIcon(string name, Icon icon, bool isNormal = true)
    {
        if (isNormal && KnownIndexsNormal.TryGetValue(name, out int index)) return index;
        if (!isNormal && KnownIndexsSelected.TryGetValue(name, out int selectedIndex)) return selectedIndex;

        Icons.Images.Add(icon);
        if (isNormal)
            KnownIndexsNormal.Add(name, Icons.Images.Count - 1);
        else
            KnownIndexsSelected.Add(name, Icons.Images.Count - 1);

        return Icons.Images.Count - 1;
    }
}
