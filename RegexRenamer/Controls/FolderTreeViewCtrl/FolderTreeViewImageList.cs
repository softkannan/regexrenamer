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
    public Dictionary<string,int> KnownIndexs { get; private set; } = new Dictionary<string,int>();

    public int GetIcon(string name)
    {
        if(KnownIndexs.TryGetValue(name, out int index)) return index;
        return -1;
    }

    public int AddIcon(string name, Icon icon)
    {
        Icons.Images.Add(icon);
        KnownIndexs.Add(name,)

    }
}
