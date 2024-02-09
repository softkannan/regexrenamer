﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Utility
{
    public class ExtractIcons
    {
        #region Structs & Enum

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public SHFILEINFO(bool b)
            {
                hIcon = IntPtr.Zero; iIcon = 0; dwAttributes = 0; szDisplayName = ""; szTypeName = "";
            }
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 80)]
            public string szTypeName;
        };

        private enum SHGFI
        {
            SHGFI_ICON = 0x000000100,     // get icon
            SHGFI_DISPLAYNAME = 0x000000200,     // get display name
            SHGFI_TYPENAME = 0x000000400,     // get type name
            SHGFI_ATTRIBUTES = 0x000000800,     // get attributes
            SHGFI_ICONLOCATION = 0x000001000,     // get icon location
            SHGFI_EXETYPE = 0x000002000,     // return exe type
            SHGFI_SYSICONINDEX = 0x000004000,     // get system icon index
            SHGFI_LINKOVERLAY = 0x000008000,     // put a link overlay on icon
            SHGFI_SELECTED = 0x000010000,     // show icon in selected state
            SHGFI_ATTR_SPECIFIED = 0x000020000,     // get only specified attributes
            SHGFI_LARGEICON = 0x000000000,     // get large icon
            SHGFI_SMALLICON = 0x000000001,     // get small icon
            SHGFI_OPENICON = 0x000000002,     // get open icon
            SHGFI_SHELLICONSIZE = 0x000000004,     // get shell size icon
            SHGFI_PIDL = 0x000000008,     // pszPath is a pidl
            SHGFI_USEFILEATTRIBUTES = 0x000000010     // use passed dwFileAttribute
        }

        #endregion

        #region Get Folder Icons

        [DllImport("Shell32.dll")]

        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes,
          out SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags);


        public static Icon GetIcon(string strPath, bool selected)  // [xiperware]  , ImageList imageList)
        {
            SHFILEINFO info = new SHFILEINFO(true);
            int cbFileInfo = Marshal.SizeOf(info);
            SHGFI flags;
            if (!selected)
                flags = SHGFI.SHGFI_ICON | SHGFI.SHGFI_SMALLICON;
            else
                flags = SHGFI.SHGFI_ICON | SHGFI.SHGFI_SMALLICON | SHGFI.SHGFI_OPENICON;

            SHGetFileInfo(strPath, 256, out info, (uint)cbFileInfo, flags);
            Icon retVal = null;
            if (info.hIcon == IntPtr.Zero)
            {
                retVal = GetNormalFolderIcon();
            }
            else
            {
                retVal = Icon.FromHandle(info.hIcon);
            }

            return retVal;
        }

        private static Icon normalFolderIcon = null;
        private static object syncObj = new object();

        public static Icon GetNormalFolderIcon()  // [xiperware]
        {
            if (normalFolderIcon == null)
            {
                lock (syncObj)
                {
                    if (normalFolderIcon == null)
                    {
                        SHFILEINFO info = new SHFILEINFO();
                        SHGetFileInfo(null,
                                       0x00000003,  // Shell32.FILE_ATTRIBUTE_DIRECTORY,
                                       out info,
                                       (uint)Marshal.SizeOf(info),
                                       SHGFI.SHGFI_ICON | SHGFI.SHGFI_USEFILEATTRIBUTES | SHGFI.SHGFI_SMALLICON);
                        normalFolderIcon = Icon.FromHandle(info.hIcon);
                    }
                }
            }
            return normalFolderIcon;
        }

        private static Icon folderIcon = null;
        public static Icon GetFolderIcon()  // [xiperware]
        {
            if (folderIcon == null)
            {
                lock (syncObj)
                {
                    if (folderIcon == null)
                    {
                        SHFILEINFO info = new SHFILEINFO();
                        SHGetFileInfo(null,
                                       0x00000010,  // Shell32.FILE_ATTRIBUTE_DIRECTORY,
                                       out info,
                                       (uint)Marshal.SizeOf(info),
                                       SHGFI.SHGFI_ICON | SHGFI.SHGFI_USEFILEATTRIBUTES | SHGFI.SHGFI_SMALLICON);
                        folderIcon = Icon.FromHandle(info.hIcon);
                    }
                }
            }
            return folderIcon;
        }

        #endregion

        #region Get Desktop Icon

        // Retreive the desktop icon from Shell32.dll - it always appears at index 34 in all shell32 versions.
        // This is probably NOT the best way to retreive this icon, but it works - if you have a better way
        // by all means let me know..

        //    [DllImport("Shell32.dll", CharSet=CharSet.Auto)]
        //    public static extern IntPtr ExtractIcon(int hInst, string lpszExeFileName, int nIconIndex);
        //
        //    public static Icon GetDesktopIcon()
        //    {
        //      IntPtr i = ExtractIcon(0, Environment.SystemDirectory + "\\shell32.dll", 34);
        //      return Icon.FromHandle(i);
        //    }

        // Updated this method in v1.11 so that the icon returned is a small icon, not a large icon as
        // returned by the old method above

        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]

        public static extern uint ExtractIconEx(
          string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

        //3     normal folder icon
        //8     fixed drive folder icon
        //11    removable drive folder icon
        //15    network drive icon in the explorer address bar
        //29    shortcut overlay icon
        //34    explorer navigation pane root icon labelled 'Desktop'
        //51    network folder icon
        //77    UAC(administrator) overlay icon
        //107   os drive folder icon
        //179   compressed file / folder overlay icon

        //1 // default file icon
        //2 // default document
        //3 // default exe file
        //4 // closed folder
        //5 // opened folder
        //6 // 5 1/4 disk
        //7 // 3 1/2 disk
        //8 // other removeable media
        //9 // hard drive
        //10 // network drive
        //11 // disconnected network drive
        //12 // cd-rom drive
        //13 // ram drive
        //14 // network (globe)
        //15 // network (mouse)
        //16 // my computer
        //17 // printer
        //18 // network computer
        //19 // entire network
        //20 // program group
        //21 // my recent documents
        //22 // control panel
        //23 // find
        //24 // help
        //25 // run
        //26 // good night (old log off?)
        //27 // undock
        //28 // shutdown
        //29 // shared
        //30 // shortcut
        //31 // scheduled task overlay
        //32 // recycle bin empty
        //33 // recycle bin full
        //34 // telephony
        //35 // desktop
        //36 // old settings
        //37 // program group, same as 20
        //38 // old printer
        //39 // fonts
        //40 // taskbar properties
        //41 // music cd
        //42 // tree
        //43 // old computer folder
        //44 // favorites
        //45 // log off
        //46 // find in folder
        //47 // windows update
        //48 // lock
        //49 // computer app ?
        //50 // empty - ignore
        //51 // empty - ignore
        //52 // empty - ignore
        //53 // empty - ignore
        //54 // old mistery drive
        //133 // file stack
        //134 // find files
        //135 // find computer glyph
        //137 // control panel, same as 22
        //138 // printer folder
        //139 // add printer
        //140 // network printer
        //141 // print to file
        //142 // old recycle bin full
        //143 // old recycle bin full of folders
        //144 // old recycle bin full of folders and files
        //145 // can't copy (overwrite?) file
        //146 // move to folder
        //147 // old rename
        //148 // old settings copy
        //151 // ini file
        //152 // txt file
        //153 // bat file
        //154 // dll file
        //155 // font file
        //156 // true type font file
        //157 // other font file
        //160 // run, same as 25
        //161 // old delete
        //165 // copy to disk
        //166 // error checking
        //167 // defragment
        //168 // printer ok
        //169 // network printer ok
        //170 // printer ok, file
        //171 // file tree structure
        //172 // network folder
        //173 // favorites
        //174 // old weird folder
        //175 // network (connect to globe)
        //176 // add network folder
        //177 // old htt file
        //178 // add network
        //179 // old network terminal thing
        //180 // screen full
        //181 // screen empty
        //182 // folder options: window image with webview
        //183 // folder options: window image without webview
        //184 // folder options: open in same window
        //185 // folder options: open in new window
        //186 // folder options: click files (link style)
        //187 // folder options: click files (normal style)
        //191 // old bin empty
        //192 // old bin full
        //193 // network folder
        //194 // old login (keys)
        //196 // fax
        //197 // fax ok
        //198 // network fax ok
        //199 // network fax
        //200 // stop
        //210 // folder settings
        //220 // old key users
        //221 // shutdown (blue circle)
        //222 // dvd disk
        //223 // some files
        //224 // video files
        //225 // music files
        //226 // image files
        //227 // various music/video files
        //228 // old music disk
        //229 // hub ?
        //230 // zip drive
        //231 // down overlay
        //232 // down overlay again
        //233 // other removeable media, same as 8
        //234 // no disk drive disabled
        //235 // my documents
        //236 // my pictures
        //237 // my music
        //238 // my videos
        //239 // msn
        //240 // delete (webview)
        //241 // copy (webview)
        //242 // rename (webview)
        //243 // files (webview)
        //244 // globe w/ arrow
        //245 // printer printing
        //246 // green arrow (webview)
        //247 // music (webview)
        //248 // camera
        //249 // board
        //250 // display properties
        //251 // network images
        //252 // print images
        //253 // ok file (webview)
        //254 // bin empty
        //255 // green cool arrow (webview)
        //256 // move
        //257 // network connection
        //258 // network drive red thing
        //259 // network home
        //260 // write cd (webview)
        //261 // cd thing (webview)
        //262 // destroy cd (webview)
        //263 // help, same as 24
        //264 // move to folder (webview)
        //265 // send mail (webview)
        //266 // move to cd (webview)
        //267 // shared folder
        //268 // accessibilty options
        //269 // users xp
        //270 // screen palette
        //271 // add or remove programs
        //272 // mouse printer
        //273 // network computers
        //274 // gear, settings
        //275 // drive use (piechart)
        //276 // network calender, syncronise ?
        //277 // music cpanel
        //278 // app settings
        //279 // user xp, same as 269
        //281 // find files
        //282 // talking computer
        //283 // screen keyboard
        //284 // black thingy
        //289 // help file
        //290 // go arrow ie
        //291 // dvd drive
        //292 // music+ cd
        //293 // unknown cd
        //294 // cd-rom
        //295 // cd-r
        //296 // cd-rw
        //297 // dvd-ram
        //298 // dvd-r
        //299 // walkman
        //300 // cassete drive
        //301 // smaller cassete drive
        //302 // cd
        //303 // red thing
        //304 // dvd-rom
        //305 // other removeable media, same as 8 and 233
        //306 // cards ?
        //307 // cards ? 2
        //308 // cards ? 3
        //309 // camera, same as before
        //310 // cellphone
        //311 // network printer globe
        //312 // jazz drive
        //313 // zip drive, same as before
        //314 // pda
        //315 // scanner
        //316 // scanner and camera
        //317 // video camera
        //318 // dvd-rw, same as before
        //319 // new folder (red thing)
        //320 // move to disk (webview)
        //321 // control panel, third time
        //322 // start menu favorites (smaller icon)
        //323 // start menu find (smaller icon)
        //324 // start menu help (smaller icon)
        //325 // start menu logoff (smaller icon)
        //326 // start menu program group (smaller icon)
        //327 // start menu recent documents (smaller icon)
        //328 // start menu run (smaller icon)
        //329 // start menu shutdown (smaller icon)
        //330 // start menu control panel(smaller icon)
        //331 // start menu logoff or something (smaller icon)
        //337 // old lookup phonebook
        //338 // stop, again
        //512 // internet explorer
        //1001 // question
        //1002 // printer red ok (webview)
        //1003 // drive ok (webview)
        //1004 // help file, again
        //1005 // move file (webview)
        //1006 // printer file (webview)
        //1007 // red ok file (webview)
        //1008 // printer pause (webview)
        //1009 // printer play (webview)
        //1010 // shared printer (webview)
        //1011 // fax, again
        //8240 // old logoff
        //16710 // old delete
        //16715 // old delete
        //16717 // old delete
        //16718 // old delete
        //16721 // old delete

        public static Icon GetWindowsIcon(int iconNum)
        {
            IntPtr[] handlesIconLarge = new IntPtr[1];
            IntPtr[] handlesIconSmall = new IntPtr[1];
            uint i = ExtractIconEx(Environment.SystemDirectory + "\\shell32.dll", iconNum,
              handlesIconLarge, handlesIconSmall, 1);

            return Icon.FromHandle(handlesIconSmall[0]);
        }

        public static Icon GetDesktopIcon()
        {
            IntPtr[] handlesIconLarge = new IntPtr[1];
            IntPtr[] handlesIconSmall = new IntPtr[1];
            uint i = ExtractIconEx(Environment.SystemDirectory + "\\shell32.dll", 34,
              handlesIconLarge, handlesIconSmall, 1);

            return Icon.FromHandle(handlesIconSmall[0]);
        }

        #endregion

    }
}