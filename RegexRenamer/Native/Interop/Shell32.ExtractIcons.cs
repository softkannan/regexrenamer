using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace PInvoke;

public class ExtractIconsAPI
{
    [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
    internal static extern uint ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

    private readonly static string SHELL_DLL_PATH;
    static ExtractIconsAPI()
    {
        SHELL_DLL_PATH = Environment.SystemDirectory + "\\shell32.dll";
    }


    #region Get Desktop Icon

    public static Icon GetWindowsIcon(int iconNum)
    {
        nint[] handlesIconLarge = new nint[1];
        nint[] handlesIconSmall = new nint[1];
        ExtractIconEx(SHELL_DLL_PATH, iconNum, handlesIconLarge, handlesIconSmall, 1);
        var retIcon = Icon.FromHandle(handlesIconSmall[0]);
        return retIcon;
    }

    public static Icon GetDesktopIcon()
    {
        IntPtr[] handlesIconLarge = new IntPtr[1];
        IntPtr[] handlesIconSmall = new IntPtr[1];
        ExtractIconEx(SHELL_DLL_PATH, 34, handlesIconLarge, handlesIconSmall, 1);
        var desktopFolderIcon = Icon.FromHandle(handlesIconSmall[0]);
        return desktopFolderIcon;
    }

    private List<Icon> GetIconsFromFile(string file)
    {
        List<Icon> icons = new List<Icon>();
        IntPtr[] large = new IntPtr[999];
        IntPtr[] small = new IntPtr[999];
        Icon ico;
        try
        {
            uint count = ExtractIconEx(file, -1, large, small, 999);
            if (count > 0)
            {
                large = new IntPtr[count - 1];
                small = new IntPtr[count - 1];

                ExtractIconEx(file, 0, large, small, count);
                foreach (var x in large)
                {
                    if (x != IntPtr.Zero)
                    {
                        ico = (Icon)Icon.FromHandle(x).Clone();
                        icons.Add(ico);
                    }
                }
            }
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message);
        }
        finally
        {
            
        }
        return icons;
    }
    #endregion

}
