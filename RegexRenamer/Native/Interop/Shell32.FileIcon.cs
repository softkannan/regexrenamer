using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media.Imaging;

namespace PInvoke;
internal class FileIconAPI
{
    // Constants that we need in the function call
    internal const uint FILE_ATTRIBUTE_DIRECTORY   = 0x00000010;
    internal const uint FILE_ATTRIBUTE_NORMAL      = 0x00000080;
    internal const uint FILE_ATTRIBUTE_TEMPORARY   = 0x00000100;
    internal const uint FILE_ATTRIBUTE_DEVICE      = 0x00000040;
    internal const uint FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
    internal const uint FILE_ATTRIBUTE_VIRTUAL     = 0x00010000;

    internal const int SHIL_JUMBO      = 0x4;
    internal const int SHIL_EXTRALARGE = 0x2;

    internal const uint         ILD_TRANSPARENT    = 0x00000001;
    internal static readonly int MaxEntitiesCount   = 80;

    internal enum SHGFI : int
    {
        SHGFI_ICON              = 0x000000100,     // get icon
        SHGFI_DISPLAYNAME       = 0x000000200,     // get display name
        SHGFI_TYPENAME          = 0x000000400,     // get type name
        SHGFI_ATTRIBUTES        = 0x000000800,     // get attributes
        SHGFI_ICONLOCATION      = 0x000001000,     // get icon location
        SHGFI_EXETYPE           = 0x000002000,     // return exe type
        SHGFI_SYSICONINDEX      = 0x000004000,     // get system icon index
        SHGFI_LINKOVERLAY       = 0x000008000,     // put a link overlay on icon
        SHGFI_SELECTED          = 0x000010000,     // show icon in selected state
        SHGFI_ATTR_SPECIFIED    = 0x000020000,     // get only specified attributes
        SHGFI_LARGEICON         = 0x000000000,     // get large icon
        SHGFI_SMALLICON         = 0x000000001,     // get small icon
        SHGFI_OPENICON          = 0x000000002,     // get open icon
        SHGFI_SHELLICONSIZE     = 0x000000004,     // get shell size icon
        SHGFI_PIDL              = 0x000000008,     // pszPath is a pidl
        SHGFI_USEFILEATTRIBUTES = 0x000000010,     // use passed dwFileAttribute
        SHGFI_ADDOVERLAYS       = 0x000000020,     // apply the appropriate overlays
        SHGFI_OVERLAYINDEX      = 0x000000040,     // Get the index of the overlay in the upper 8 bits of the iIcon
    }
    

    [DllImport("Shell32.dll", EntryPoint = "#727")]
    internal extern static int SHGetImageList(int iImageList, ref Guid riid, out IImageList ppv);

    // The signature of SHGetFileInfo (located in Shell32.dll)
    [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
    internal static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags);

    [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
    internal static extern IntPtr SHGetFileInfo(IntPtr pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

    [DllImport("shell32.dll", EntryPoint = "SHGetFileInfo", CharSet = CharSet.Auto)]
    internal static extern IImageList SHGetFileInfoAsImageList(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

    [DllImport("Shell32.dll", SetLastError = true)]
    internal static extern int SHGetSpecialFolderLocation(IntPtr hwndOwner, Int32 nFolder, ref IntPtr ppidl);

    [DllImport("Shell32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern nint SHILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, out IntPtr ppIdl, ref uint rgflnOut);

    [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
    internal static extern uint ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

    [DllImport("User32.dll")]
    internal static extern int DestroyIcon(IntPtr hIcon);

    [DllImport("Kernel32.dll")]
    internal static extern Boolean CloseHandle(IntPtr handle);

    [StructLayout(LayoutKind.Sequential)]
    internal struct IMAGEINFO
    {
        public IntPtr hbmImage;
        public IntPtr hbmMask;
        public int Unused1;
        public int Unused2;
        public RECT rcImage;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left, Top, Right, Bottom;
        public RECT(int l, int t, int r, int b)
        {
            Left = l;
            Top = t;
            Right = r;
            Bottom = b;
        }

        public RECT(Rectangle r)
        {
            Left = r.Left;
            Top = r.Top;
            Right = r.Right;
            Bottom = r.Bottom;
        }

        public Rectangle ToRectangle()
        {
            return Rectangle.FromLTRB(Left, Top, Right, Bottom);
        }

        public void Inflate(int width, int height)
        {
            Left -= width;
            Top -= height;
            Right += width;
            Bottom += height;
        }

        public override string ToString()
        {
            return string.Format("x:{0},y:{1},width:{2},height:{3}", Left, Top, Right - Left, Bottom - Top);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X, Y;
        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public POINT(Point pt)
        {
            this.X = pt.X;
            this.Y = pt.Y;
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IMAGELISTDRAWPARAMS
    {
        public int cbSize;
        public IntPtr himl;
        public int i;
        public IntPtr hdcDst;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int xBitmap;   // x offest from the upperleft of bitmap
        public int yBitmap;   // y offset from the upperleft of bitmap
        public int rgbBk;
        public int rgbFg;
        public int fStyle;
        public int dwRop;
        public int fState;
        public int Frame;
        public int crEffect;
    }

    [ComImport,
     Guid("46EB5926-582E-4017-9FDF-E8998DAA0950"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IImageList
    {
        [PreserveSig]
        int Add(IntPtr hbmImage, IntPtr hbmMask, ref int pi);
        [PreserveSig]
        int ReplaceIcon(int i, IntPtr hicon, ref int pi);
        [PreserveSig]
        int SetOverlayImage(int iImage, int iOverlay);
        [PreserveSig]
        int Replace(int i, IntPtr hbmImage, IntPtr hbmMask);
        [PreserveSig]
        int AddMasked(IntPtr hbmImage, int crMask, ref int pi);
        [PreserveSig]
        int Draw(ref IMAGELISTDRAWPARAMS pimldp);
        [PreserveSig]
        int Remove(int i);
        [PreserveSig]
        int GetIcon(int i, int flags, ref IntPtr picon);
        [PreserveSig]
        int GetImageInfo(int i, ref IMAGEINFO pImageInfo);
        [PreserveSig]
        int Copy(int iDst, IImageList punkSrc, int iSrc, int uFlags);
        [PreserveSig]
        int Merge(int i1, IImageList punk2, int i2, int dx, int dy, ref Guid riid, ref IntPtr ppv);
        [PreserveSig]
        int Clone(ref Guid riid, ref IntPtr ppv);
        [PreserveSig]
        int GetImageRect(int i, ref RECT prc);
        [PreserveSig]
        int GetIconSize(ref int cx, ref int cy);
        [PreserveSig]
        int SetIconSize(int cx, int cy);
        [PreserveSig]
        int GetImageCount(ref int pi);
        [PreserveSig]
        int SetImageCount(int uNewCount);
        [PreserveSig]
        int SetBkColor(int clrBk, ref int pclr);
        [PreserveSig]
        int GetBkColor(ref int pclr);
        [PreserveSig]
        int BeginDrag(int iTrack, int dxHotspot, int dyHotspot);
        [PreserveSig]
        int EndDrag();
        [PreserveSig]
        int DragEnter(IntPtr hwndLock, int x, int y);
        [PreserveSig]
        int DragLeave(IntPtr hwndLock);
        [PreserveSig]
        int DragMove(int x, int y);
        [PreserveSig]
        int SetDragCursorImage(ref IImageList punk, int iDrag, int dxHotspot, int dyHotspot);
        [PreserveSig]
        int DragShowNolock(int fShow);
        [PreserveSig]
        int GetDragImage(ref POINT ppt, ref POINT pptHotspot, ref Guid riid, ref IntPtr ppv);
        [PreserveSig]
        int GetItemFlags(int i, ref int dwFlags);
        [PreserveSig]
        int GetOverlayImage(int iOverlay, ref int piIndex);
    }

    internal struct SHFILEINFO
    {

        // Handle to the icon representing the file

        public IntPtr hIcon;

        // Index of the icon within the image list

        public int iIcon;

        // Various attributes of the file

        public uint dwAttributes;

        // Path to the file

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]

        public string szDisplayName;

        // File type

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]

        public string szTypeName;

    }

    internal enum IconSizeType
    {
        Medium = 0x0,
        Small = 0x1,
        Large = 0x2,
        ExtraLarge = 0x4
    }

    internal struct IconPair
    {
        public Icon Icon { get; set; }
        public IntPtr IconHandle { set; get; }
    }

    internal static void GetDirectories(string path, List<Image> col, IconSizeType sizeType, Size itemSize)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        DirectoryInfo[] dirs = dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < dirs.Length && i < MaxEntitiesCount; i++)
        {
            DirectoryInfo subDirInfo = dirs[i];
            if (!CheckAccess(subDirInfo) || !MatchFilter(subDirInfo.Attributes))
            {
                continue;
            }
            col.Add(GetFileImage(subDirInfo.FullName, sizeType, itemSize));
        }
    }

    internal static bool CheckAccess(DirectoryInfo info)
    {
        bool isOk = false;
        try
        {
            var secInfo = info.GetAccessControl();
            isOk = true;
        }
        catch
        {
        }
        return isOk;
    }

    internal static bool MatchFilter(FileAttributes attributes)
    {
        return (attributes & (FileAttributes.Hidden | FileAttributes.System)) == 0;
    }

    internal static Image IconToBitmap(Icon ico, IconSizeType sizeType, Size itemSize)
    {
        if (ico == null)
        {
            return new Bitmap(itemSize.Width, itemSize.Height);
        }
        return ico.ToBitmap();
    }
     

    private static Icon GetFileIcon(string path)
    {
        uint rgfl = 0;
        IntPtr ppIdl;
        SHILCreateFromPath(path, out ppIdl, ref rgfl);
        SHFILEINFO shinfo = new SHFILEINFO();
        IntPtr retVal = SHGetFileInfo(ppIdl, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI.SHGFI_SMALLICON | SHGFI.SHGFI_ICON | SHGFI.SHGFI_PIDL);
        Icon icon = null;
        if (shinfo.hIcon != IntPtr.Zero)
        {
            icon = Icon.FromHandle(shinfo.hIcon);
        }
        return icon;
    }

    internal static Icon GetIcon(string strPath, bool selected)
    {
        var localCpPath = strPath;
        if (string.IsNullOrEmpty(localCpPath))
        {
            localCpPath = Environment.SystemDirectory;
        }

        Icon retVal = null;
        SHFILEINFO info = new SHFILEINFO();
        int cbFileInfo = Marshal.SizeOf(info);
        SHGFI flags;
        //flags = SHGFI.SHGFI_SYSICONINDEX | SHGFI.SHGFI_ICON | SHGFI.SHGFI_SMALLICON| SHGFI.SHGFI_USEFILEATTRIBUTES | SHGFI.SHGFI_TYPENAME | SHGFI.SHGFI_DISPLAYNAME;
        flags = SHGFI.SHGFI_ICON | SHGFI.SHGFI_SMALLICON;
        if (selected)
        {
            flags |= SHGFI.SHGFI_SELECTED;
        }

        SHGetFileInfo(localCpPath, 0, ref info, (uint)cbFileInfo, flags);

        if (info.hIcon == IntPtr.Zero)
        {
            retVal = GetFileIcon(localCpPath);
        }
        else
        {
            retVal = Icon.FromHandle(info.hIcon);
        }
        return retVal;
    }

    internal static Icon GetDefaultFolderIcon(bool selected)
    {
        SHFILEINFO info = new SHFILEINFO();

        SHGFI flags = SHGFI.SHGFI_ICON | SHGFI.SHGFI_SMALLICON;

        if (selected)
        {
            flags |= SHGFI.SHGFI_SELECTED | SHGFI.SHGFI_OPENICON;
        }

        SHGetFileInfo(Environment.SystemDirectory,
                        0,
                        ref info,
                        (uint)Marshal.SizeOf(info),
                        flags);
        var normalFolderIcon = Icon.FromHandle(info.hIcon);
        return normalFolderIcon;
    }

    internal static Icon GetDefaultFolderIcon()
    {
        SHFILEINFO info = new ();
        SHGFI flags = SHGFI.SHGFI_ICON | SHGFI.SHGFI_SMALLICON| SHGFI.SHGFI_USEFILEATTRIBUTES;
        SHGetFileInfo(null,
                        FILE_ATTRIBUTE_DIRECTORY,
                        ref info,
                        (uint)Marshal.SizeOf(info), flags);
        var folderIcon = Icon.FromHandle(info.hIcon);
        return folderIcon;
    }

    internal static Icon GetIcon(string path, IconSizeType sizeType, Size itemSize)
    {
        SHFILEINFO shinfo = new SHFILEINFO();
        IntPtr retVal = SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI.SHGFI_SYSICONINDEX | SHGFI.SHGFI_ICON);
        int iconIndex = shinfo.iIcon;
        IImageList iImageList = (IImageList)GetSystemImageListHandle(sizeType);
        IntPtr hIcon = IntPtr.Zero;
        if (iImageList != null)
        {
            iImageList.GetIcon(iconIndex, (int)ILD_TRANSPARENT, ref hIcon);
        }
        Icon icon = null;
        if (hIcon != IntPtr.Zero)
        {
            icon = Icon.FromHandle(hIcon).Clone() as Icon;
            DestroyIcon(shinfo.hIcon);
        }
        return icon;
    }

    internal static Bitmap GetImageIcon(string path, IconSizeType sizeType)
    {
        uint rgfl = 0;
        IntPtr ppIdl;
        SHILCreateFromPath(path, out ppIdl, ref rgfl);
        SHFILEINFO shinfo = new SHFILEINFO();
        IntPtr retVal = SHGetFileInfo(ppIdl, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI.SHGFI_SMALLICON | SHGFI.SHGFI_ICON | SHGFI.SHGFI_PIDL);
        int iconIndex = shinfo.iIcon;
        IImageList iImageList = (IImageList)GetSystemImageListHandle(sizeType);
        IntPtr hIcon = IntPtr.Zero;
        if (iImageList != null)
        {
            iImageList.GetIcon(iconIndex, (int)ILD_TRANSPARENT, ref hIcon);
        }
        Icon icon = null;
        Bitmap bitmap = null;
        if (hIcon != IntPtr.Zero)
        {
            icon = Icon.FromHandle(hIcon);
            bitmap = new Bitmap(icon.Width, icon.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.DrawIcon(icon, 0, 0);
            }
            using (Graphics gr = Graphics.FromHwnd(new IntPtr(0)))
            {
                gr.DrawImage(bitmap, 0, 0, icon.Width, icon.Height);
            }

            DestroyIcon(shinfo.hIcon);
        }
        return bitmap;
    }

    internal static Image GetFileImage(string path, IconSizeType sizeType, Size itemSize)
    {
        var shfi = new SHFILEINFO();
        var imageList = SHGetFileInfoAsImageList(path, 0, ref shfi, (uint)Marshal.SizeOf(shfi), SHGFI.SHGFI_SYSICONINDEX);
        if (imageList != null)
        {
            var hIcon = IntPtr.Zero;
            imageList.GetIcon(shfi.iIcon, (int)ILD_TRANSPARENT, ref hIcon);
            Marshal.FinalReleaseComObject(imageList);
            if (hIcon != IntPtr.Zero)
            {
                var image = Bitmap.FromHicon(hIcon);
                DestroyIcon(hIcon);
                return image;
            }
        }
        return new Bitmap(itemSize.Width, itemSize.Height);
    }

    public static SHFILEINFO GetInfo(string strPath, bool isDirectory)
    {
        // here's the actual function that will be called which will invoke the Windows API
        // this is just some wrapper code to make invoking the API easier
        SHFILEINFO info = new SHFILEINFO();
        int cbFileInfo = Marshal.SizeOf(info);
        SHGFI flags;
        flags = SHGFI.SHGFI_SYSICONINDEX | SHGFI.SHGFI_SMALLICON | SHGFI.SHGFI_USEFILEATTRIBUTES | SHGFI.SHGFI_TYPENAME | SHGFI.SHGFI_DISPLAYNAME;
        SHGetFileInfo(strPath, (isDirectory == true ? FILE_ATTRIBUTE_DIRECTORY : FILE_ATTRIBUTE_NORMAL), ref info, System.Convert.ToUInt32(cbFileInfo), flags);
        if (info.hIcon != IntPtr.Zero)
            DestroyIcon(info.hIcon);
        return info;
    }

    internal static IImageList GetSystemImageListHandle(IconSizeType sizeType)
    {
        IImageList iImageList;
        Guid imageListGuid = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
        int ret = SHGetImageList((int)sizeType, ref imageListGuid, out iImageList);
        return iImageList;
    }

    internal static byte[] GetSmallIcon(string FileName)
    {
        SHFILEINFO shinfo = new SHFILEINFO();
        SHGFI flags;
        flags =  SHGFI.SHGFI_ICON | SHGFI.SHGFI_SMALLICON;
        var res = SHGetFileInfo(FileName, 0, ref shinfo, (uint) Marshal.SizeOf(shinfo), flags);
        if (res == 0)
        {
            throw (new System.IO.FileNotFoundException());
        }
        var ico = Icon.FromHandle(shinfo.hIcon);
        var bs = ByteFromIcon(ico);
        ico.Dispose();
        DestroyIcon(shinfo.hIcon);
        return bs;
    }


    internal static byte[] GetLargeIcon(string FileName)
    {
        SHFILEINFO shinfo = new SHFILEINFO();
        SHGFI flags;
        flags = SHGFI.SHGFI_SYSICONINDEX;
        var res = SHGetFileInfo(FileName, FILE_ATTRIBUTE_NORMAL, ref shinfo, (uint) Marshal.SizeOf(shinfo), flags);
        if (res == 0)
        {
            throw (new System.IO.FileNotFoundException());
        }
        var iconIndex = shinfo.iIcon;
        Guid iidImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
        IImageList iml;
        int size = SHIL_EXTRALARGE;
        var hres = SHGetImageList(size, ref iidImageList, out  iml); // writes iml
        IntPtr hIcon = IntPtr.Zero;
        int ILD_TRANSPARENT = 1;
        hres = iml.GetIcon(iconIndex, ILD_TRANSPARENT, ref hIcon);
        var ico = System.Drawing.Icon.FromHandle(hIcon);
        var bs = ByteFromIcon(ico);
        ico.Dispose();
        DestroyIcon(hIcon);
        return bs;
    }

    internal static byte[] ByteFromIcon(System.Drawing.Icon ic)
    {
        var icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(ic.Handle,
                                            System.Windows.Int32Rect.Empty,
                                            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        icon.Freeze();
        byte[] data;
        PngBitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(icon));
        using (MemoryStream ms = new MemoryStream())
        {
            encoder.Save(ms);
            data = ms.ToArray();
        }
        return data;
    }



}
