using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using HWND = nint;
using HANDLE = nint;

namespace RegexRenamer.Controls.FolderTreeViewCtrl.Native;

public class ExtractIconsAPI
{
    [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
    internal static extern uint ExtractIconEx(string lpszFile, int nIconIndex, HWND[] phiconLarge, HWND[] phiconSmall, uint nIcons);

    [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
    internal static extern HWND ExtractAssociatedIcon(HandleRef hInst, StringBuilder iconPath, ref int index);

    [DllImport("shell32")] 
    internal static extern int DuplicateIcon(HANDLE hInst, HANDLE hIcon);

	[DllImport("shell32")] 
    internal static extern int ExtractIcon(HANDLE hInst, string lpszExeFileName, int nIconIndex);

    [DllImport("user32.dll")]
    internal static extern HWND CopyIcon(HWND hIcon);

    // DestroyIcon
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool DestroyIcon(HWND hIcon);

    internal enum Shell32Icons:int
    {
        Explorer                            = 0, 
        Explorer_Default_Document           = 1, 
        Explorer_Default_Application        = 2, 
        Explorer_Closed_Folder              = 3, 
        Explorer_Open_Folder                = 4, 
        Drive_525_Inch_Floppy               = 5, 
        Drive_35_Inch_Floppy                = 6, 
        Drive_Removable_Drive               = 7, 
        Drive_Hard_Drive                    = 8, 
        Drive_Network_Drive                 = 9, 
        Drive_Network_Drive_Disconnected    = 10, 
        Drive_Cd_Rom_Drive                  = 11, 
        Drive_Ram_Drive                     = 12, 
        Global_Entire_Network               = 13, 
        Explorer_Networked_Computer         = 15, 
        Explorer_Printers                   = 16, 
        Desktop_Network_Neighborhood        = 17, 
        Explorer_Workgroup                  = 18, 
        Startmenu_Programs                  = 19, 
        Startmenu_Recent_Documents          = 20, 
        Startmenu_Settings                  = 21, 
        Startmenu_Find                      = 22, 
        Startmenu_Help                      = 23, 
        Startmenu_Run                       = 24, 
        Startmenu_Suspend                   = 25, 
        Startmenu_Docking                   = 26, 
        Startmenu_Shutdown                  = 27, 
        Overlay_Sharing                     = 28, 
        Overlay_Shortcut                    = 29, 
        Desktop_Recycle_Bin_Empty           = 31, 
        Desktop_Recycle_Bin_Full            = 32, 
        Explorer_Dial_Up_Networking         = 33, 
        Explorer_Desktop                    = 34, 
        Startmenu_Settings_Control_Panel    = 35, 
        Startmenu_Programs_Program_Folder   = 36, 
        Startmenu_Settings_Printers         = 37, 
        Startmenu_Settings_Taskbar          = 39, 
        Explorer_Audio_CD                   = 40, 
        Explorer_Saved_Search_Fnd           = 42, 
        Explorer_Und_Startmenu_Favorites    = 43, 
        Startmenu_Log_Off                   = 44,
        Network_Folder_Icon                 = 51,
        Uac_Administrator_Overlay_Icon      = 77, 
        OS_Drive_Folder_Icon                = 107, 
        Compressed_File_Folder_Overlay_Icon = 179,
    }

    internal enum Shell32IconsExprimental : int
    {
        Default_File_Icon                          =1,     // default file icon
        Default_Document                           =2,     // default document
        Default_Exe_File                           =3,     // default exe file
        Closed_Folder                              =4,     // closed folder
        Opened_Folder                              =5,     // opened folder
        Disk_5_1_4                                 =6,     // 5 1/4 disk
        Disk_3_1_2                                 =7,     // 3 1/2 disk
        Other_Removeable_Media                     =8,     // other removeable media
        Hard_Drive                                 =9,     // hard drive
        Network_Drive                              =10,    // network drive
        Disconnected_Network_Drive                 =11,    // disconnected network drive
        Cd_Rom_Drive                               =12,    // cd-rom drive
        Ram_Drive                                  =13,    // ram drive
        Network_Globe                              =14,    // network (globe)
        Network_Mouse                              =15,    // network (mouse)
        My_Computer                                =16,    // my computer
        Printer                                    =17,    // printer
        Network_Computer                           =18,    // network computer
        Entire_Network                             =19,    // entire network
        Program_Group                              =20,    // program group
        My_Recent_Documents                        =21,    // my recent documents
        Control_Panel                              =22,    // control panel
        Find                                       =23,    // find
        Help                                       =24,    // help
        Run                                        =25,    // run
        Good_Night_Old_Log_Off                     =26,    // good night (old log off?)
        Undock                                     =27,    // undock
        Shutdown                                   =28,    // shutdown
        Shared                                     =29,    // shared
        Shortcut                                   =30,    // shortcut
        Scheduled_Task_Overlay                     =31,    // scheduled task overlay
        Recycle_Bin_Empty                          =32,    // recycle bin empty
        Recycle_Bin_Full                           =33,    // recycle bin full
        Telephony                                  =34,    // telephony
        Desktop                                    =35,    // desktop
        Old_Settings                               =36,    // old settings
        Program_Group_Same_As_20                   =37,    // program group, same as 20
        Old_Printer                                =38,    // old printer
        Fonts                                      =39,    // fonts
        Taskbar_Properties                         =40,    // taskbar properties
        Music_Cd                                   =41,    // music cd
        Tree                                       =42,    // tree
        Old_Computer_Folder                        =43,    // old computer folder
        Favorites                                  =44,    // favorites
        Log_Off                                    =45,    // log off
        Find_In_Folder                             =46,    // find in folder
        Windows_Update                             =47,    // windows update
        Lock                                       =48,    // lock
        Computer_App                               =49,    // computer app ?
        Empty_Ignore                               =50,    // empty - ignore
        Empty_Ignore1                              =51,    // empty - ignore
        Empty_Ignore2                              =52,    // empty - ignore
        Empty_Ignore3                              =53,    // empty - ignore
        Old_Mistery_Drive                          =54,    // old mistery drive
        File_Stack                                 =133,   // file stack
        Find_Files                                 =134,   // find files
        Find_Computer_Glyph                        =135,   // find computer glyph
        Control_Panel_Same_As_22                   =137,   // control panel, same as 22
        Printer_Folder                             =138,   // printer folder
        Add_Printer                                =139,   // add printer
        Network_Printer                            =140,   // network printer
        Print_To_File                              =141,   // print to file
        Old_Recycle_Bin_Full                       =142,   // old recycle bin full
        Old_Recycle_Bin_Full_Of_Folders            =143,   // old recycle bin full of folders
        Old_Recycle_Bin_Full_Of_Folders_And_Files  =144,   // old recycle bin full of folders and files
        Can_t_Copy_Overwrite_File                  =145,   // can't copy (overwrite?) file
        Move_To_Folder                             =146,   // move to folder
        Old_Rename                                 =147,   // old rename
        Old_Settings_Copy                          =148,   // old settings copy
        Ini_File                                   =151,   // ini file
        Txt_File                                   =152,   // txt file
        Bat_File                                   =153,   // bat file
        Dll_File                                   =154,   // dll file
        Font_File                                  =155,   // font file
        True_Type_Font_File                        =156,   // true type font file
        Other_Font_File                            =157,   // other font file
        Run_Same_As_25                             =160,   // run, same as 25
        Old_Delete                                 =161,   // old delete
        Copy_To_Disk                               =165,   // copy to disk
        Error_Checking                             =166,   // error checking
        Defragment                                 =167,   // defragment
        Printer_Ok                                 =168,   // printer ok
        Network_Printer_Ok                         =169,   // network printer ok
        Printer_Ok_File                            =170,   // printer ok, file
        File_Tree_Structure                        =171,   // file tree structure
        Network_Folder                             =172,   // network folder
        Favorites_1                                =173,   // favorites
        Old_Weird_Folder                           =174,   // old weird folder
        Network_Connect_To_Globe                   =175,   // network (connect to globe)
        Add_Network_Folder                         =176,   // add network folder
        Old_Htt_File                               =177,   // old htt file
        Add_Network                                =178,   // add network
        Old_Network_Terminal_Thing                 =179,   // old network terminal thing
        Screen_Full                                =180,   // screen full
        Screen_Empty                               =181,   // screen empty
        Folder_Options_Window_Image_With_Webview   =182,   // folder options: window image with webview
        Folder_Options_Window_Image_Without_Webview=183,   // folder options: window image without webview
        Folder_Options_Open_In_Same_Window         =184,   // folder options: open in same window
        Folder_Options_Open_In_New_Window          =185,   // folder options: open in new window
        Folder_Options_Click_Files_Link_Style      =186,   // folder options: click files (link style)
        Folder_Options_Click_Files_Normal_Style    =187,   // folder options: click files (normal style)
        Old_Bin_Empty                              =191,   // old bin empty
        Old_Bin_Full                               =192,   // old bin full
        Network_Folder1                            =193,   // network folder
        Old_Login_Keys                             =194,   // old login (keys)
        Fax                                        =196,   // fax
        Fax_Ok                                     =197,   // fax ok
        Network_Fax_Ok                             =198,   // network fax ok
        Network_Fax                                =199,   // network fax
        Stop                                       =200,   // stop
        Folder_Settings                            =210,   // folder settings
        Old_Key_Users                              =220,   // old key users
        Shutdown_Blue_Circle                       =221,   // shutdown (blue circle)
        Dvd_Disk                                   =222,   // dvd disk
        Some_Files                                 =223,   // some files
        Video_Files                                =224,   // video files
        Music_Files                                =225,   // music files
        Image_Files                                =226,   // image files
        Various_Music_Video_Files                  =227,   // various music/video files
        Old_Music_Disk                             =228,   // old music disk
        Hub                                        =229,   // hub ?
        Zip_Drive                                  =230,   // zip drive
        Down_Overlay                               =231,   // down overlay
        Down_Overlay_Again                         =232,   // down overlay again
        Other_Removeable_Media_Same_As_8           =233,   // other removeable media, same as 8
        No_Disk_Drive_Disabled                     =234,   // no disk drive disabled
        My_Documents                               =235,   // my documents
        My_Pictures                                =236,   // my pictures
        My_Music                                   =237,   // my music
        My_Videos                                  =238,   // my videos
        Msn                                        =239,   // msn
        Delete_Webview                             =240,   // delete (webview)
        Copy_Webview                               =241,   // copy (webview)
        Rename_Webview                             =242,   // rename (webview)
        Files_Webview                              =243,   // files (webview)
        Globe_W_Arrow                              =244,   // globe w/ arrow
        Printer_Printing                           =245,   // printer printing
        Green_Arrow_Webview                        =246,   // green arrow (webview)
        Music_Webview                              =247,   // music (webview)
        Camera                                     =248,   // camera
        Board                                      =249,   // board
        Display_Properties                         =250,   // display properties
        Network_Images                             =251,   // network images
        Print_Images                               =252,   // print images
        Ok_File_Webview                            =253,   // ok file (webview)
        Bin_Empty                                  =254,   // bin empty
        Green_Cool_Arrow_Webview                   =255,   // green cool arrow (webview)
        Move                                       =256,   // move
        Network_Connection                         =257,   // network connection
        Network_Drive_Red_Thing                    =258,   // network drive red thing
        Network_Home                               =259,   // network home
        Write_Cd_Webview                           =260,   // write cd (webview)
        Cd_Thing_Webview                           =261,   // cd thing (webview)
        Destroy_Cd_Webview                         =262,   // destroy cd (webview)
        Help_Same_As_24                            =263,   // help, same as 24
        Move_To_Folder_Webview                     =264,   // move to folder (webview)
        Send_Mail_Webview                          =265,   // send mail (webview)
        Move_To_Cd_Webview                         =266,   // move to cd (webview)
        Shared_Folder                              =267,   // shared folder
        Accessibilty_Options                       =268,   // accessibilty options
        Users_Xp                                   =269,   // users xp
        Screen_Palette                             =270,   // screen palette
        Add_Or_Remove_Programs                     =271,   // add or remove programs
        Mouse_Printer                              =272,   // mouse printer
        Network_Computers                          =273,   // network computers
        Gear_Settings                              =274,   // gear, settings
        Drive_Use_Piechart                         =275,   // drive use (piechart)
        Network_Calender_Syncronise                =276,   // network calender, syncronise ?
        Music_Cpanel                               =277,   // music cpanel
        App_Settings                               =278,   // app settings
        User_Xp_Same_As_269                        =279,   // user xp, same as 269
        Find_Files1                                =281,   // find files
        Talking_Computer                           =282,   // talking computer
        Screen_Keyboard                            =283,   // screen keyboard
        Black_Thingy                               =284,   // black thingy
        Help_File                                  =289,   // help file
        Go_Arrow_Ie                                =290,   // go arrow ie
        Dvd_Drive                                  =291,   // dvd drive
        Music_Cd1                                  =292,   // music+ cd
        Unknown_Cd                                 =293,   // unknown cd
        Cd_Rom                                     =294,   // cd-rom
        Cd_R                                       =295,   // cd-r
        Cd_Rw                                      =296,   // cd-rw
        Dvd_Ram                                    =297,   // dvd-ram
        Dvd_R                                      =298,   // dvd-r
        Walkman                                    =299,   // walkman
        Cassete_Drive                              =300,   // cassete drive
        Smaller_Cassete_Drive                      =301,   // smaller cassete drive
        Cd                                         =302,   // cd
        Red_Thing                                  =303,   // red thing
        Dvd_Rom                                    =304,   // dvd-rom
        Other_Removeable_Media_Same_As_8_And_233   =305,   // other removeable media, same as 8 and 233
        Cards                                      =306,   // cards ?
        Cards_2                                    =307,   // cards ? 2
        Cards_3                                    =308,   // cards ? 3
        Camera_Same_As_Before                      =309,   // camera, same as before
        Cellphone                                  =310,   // cellphone
        Network_Printer_Globe                      =311,   // network printer globe
        Jazz_Drive                                 =312,   // jazz drive
        Zip_Drive_Same_As_Before                   =313,   // zip drive, same as before
        Pda                                        =314,   // pda
        Scanner                                    =315,   // scanner
        Scanner_And_Camera                         =316,   // scanner and camera
        Video_Camera                               =317,   // video camera
        Dvd_Rw_Same_As_Before                      =318,   // dvd-rw, same as before
        New_Folder_Red_Thing                       =319,   // new folder (red thing)
        Move_To_Disk_Webview                       =320,   // move to disk (webview)
        Control_Panel_Third_Time                   =321,   // control panel, third time
        Start_Menu_Favorites_Smaller_Icon          =322,   // start menu favorites (smaller icon)
        Start_Menu_Find_Smaller_Icon               =323,   // start menu find (smaller icon)
        Start_Menu_Help_Smaller_Icon               =324,   // start menu help (smaller icon)
        Start_Menu_Logoff_Smaller_Icon             =325,   // start menu logoff (smaller icon)
        Start_Menu_Program_Group_Smaller_Icon      =326,   // start menu program group (smaller icon)
        Start_Menu_Recent_Documents_Smaller_Icon   =327,   // start menu recent documents (smaller icon)
        Start_Menu_Run_Smaller_Icon                =328,   // start menu run (smaller icon)
        Start_Menu_Shutdown_Smaller_Icon           =329,   // start menu shutdown (smaller icon)
        Start_Menu_Control_PanelSmaller_Icon       =330,   // start menu control panel(smaller icon)
        Start_Menu_Logoff_Or_Something_Smaller_Icon=331,   // start menu logoff or something (smaller icon)
        Old_Lookup_Phonebook                       =337,   // old lookup phonebook
        Stop_Again                                 =338,   // stop, again
        Internet_Explorer                          =512,   // internet explorer
        Question                                   =1001,  // question
        Printer_Red_Ok_Webview                     =1002,  // printer red ok (webview)
        Drive_Ok_Webview                           =1003,  // drive ok (webview)
        Help_File_Again                            =1004,  // help file, again
        Move_File_Webview                          =1005,  // move file (webview)
        Printer_File_Webview                       =1006,  // printer file (webview)
        Red_Ok_File_Webview                        =1007,  // red ok file (webview)
        Printer_Pause_Webview                      =1008,  // printer pause (webview)
        Printer_Play_Webview                       =1009,  // printer play (webview)
        Shared_Printer_Webview                     =1010,  // shared printer (webview)
        Fax_Again                                  =1011,  // fax, again
        Old_Logoff                                 =8240,  // old logoff
        Old_Delete5                                =16710, // old delete
        Old_Delete1                                =16715, // old delete
        Old_Delete2                                =16717, // old delete
        Old_Delete3                                =16718, // old delete
        Old_Delete4                                =16721, // old delete
    }

    private readonly static string SHELL_DLL_PATH;
    static ExtractIconsAPI()
    {
        SHELL_DLL_PATH = Environment.SystemDirectory + "\\shell32.dll";
    }


    #region Get Desktop Icon

    internal static Icon GetWindowsIcon(int iconNum)
    {
        nint[] handlesIconLarge = new nint[1];
        nint[] handlesIconSmall = new nint[1];
        ExtractIconEx(SHELL_DLL_PATH, iconNum, handlesIconLarge, handlesIconSmall, 1);
        // From handle method will create a copy and we need to destroy the original handle when icon goes out of scope
        var retIcon = Icon.FromHandle(handlesIconSmall[0]);

        if (handlesIconLarge[0] == 0)
            DestroyIcon(handlesIconLarge[0]);

        return retIcon;
    }

    internal static Icon GetDesktopIcon()
    {
        HWND[] handlesIconLarge = new HWND[1];
        HWND[] handlesIconSmall = new HWND[1];
        ExtractIconEx(SHELL_DLL_PATH, (int) Shell32Icons.Explorer_Desktop, handlesIconLarge, handlesIconSmall, 1);
        // From handle method will create a copy and we need to destroy the original handle when icon goes out of scope
        var desktopFolderIcon = Icon.FromHandle(handlesIconSmall[0]);

        // Clean up large icon handle
        if (handlesIconLarge[0] != HWND.Zero)
            DestroyIcon(handlesIconLarge[0]);

        return desktopFolderIcon;
    }

    internal List<Icon> GetIconsFromFile(string file)
    {
        List<Icon> icons = new List<Icon>();
        HWND[] large = new HWND[999];
        HWND[] small = new HWND[999];
        Icon ico;
        uint count = ExtractIconEx(file, -1, large, small, 999);
        if (count > 0)
        {
            large = new HWND[count - 1];
            small = new HWND[count - 1];

            ExtractIconEx(file, 0, large, small, count);
            foreach (var x in large)
            {
                if (x != HWND.Zero)
                {
                    // From handle method will create a copy and we need to destroy the original handle when icon goes out of scope
                    ico = (Icon)Icon.FromHandle(x);
                    icons.Add(ico);
                }
            }
            // Clean up small icon handles, not used above
            foreach (var x in small)
            {
                if (x != HWND.Zero)
                {
                    DestroyIcon(x);
                }
            }

        }
        return icons;
    }
    #endregion

}
