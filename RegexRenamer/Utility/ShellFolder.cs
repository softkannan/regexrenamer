using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Utility
{
    // Enums for standard Windows shell folders
    public enum ShellFolder
    {
        Desktop = Shell32.ShellSpecialFolderConstants.ssfDESKTOP,
        DesktopDirectory = Shell32.ShellSpecialFolderConstants.ssfDESKTOPDIRECTORY,
        MyComputer = Shell32.ShellSpecialFolderConstants.ssfDRIVES,
        MyDocuments = Shell32.ShellSpecialFolderConstants.ssfPERSONAL,
        MyPictures = Shell32.ShellSpecialFolderConstants.ssfMYPICTURES,
        History = Shell32.ShellSpecialFolderConstants.ssfHISTORY,
        Favorites = Shell32.ShellSpecialFolderConstants.ssfFAVORITES,
        Fonts = Shell32.ShellSpecialFolderConstants.ssfFONTS,
        ControlPanel = Shell32.ShellSpecialFolderConstants.ssfCONTROLS,
        TemporaryInternetFiles = Shell32.ShellSpecialFolderConstants.ssfINTERNETCACHE,
        MyNetworkPlaces = Shell32.ShellSpecialFolderConstants.ssfNETHOOD,
        NetworkNeighborhood = Shell32.ShellSpecialFolderConstants.ssfNETWORK,
        ProgramFiles = Shell32.ShellSpecialFolderConstants.ssfPROGRAMFILES,
        RecentFiles = Shell32.ShellSpecialFolderConstants.ssfRECENT,
        StartMenu = Shell32.ShellSpecialFolderConstants.ssfSTARTMENU,
        Windows = Shell32.ShellSpecialFolderConstants.ssfWINDOWS,
        Printers = Shell32.ShellSpecialFolderConstants.ssfPRINTERS,
        RecycleBin = Shell32.ShellSpecialFolderConstants.ssfBITBUCKET,
        Cookies = Shell32.ShellSpecialFolderConstants.ssfCOOKIES,
        ApplicationData = Shell32.ShellSpecialFolderConstants.ssfAPPDATA,
        SendTo = Shell32.ShellSpecialFolderConstants.ssfSENDTO,
        StartUp = Shell32.ShellSpecialFolderConstants.ssfSTARTUP
    }
}
