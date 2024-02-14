using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PInvoke
{
    /// <content>
    /// Contains the <see cref="SHKNOWNFOLDERID"/> nested type.
    /// </content>
    public partial class NativeShell32
    {
        [DllImport("Shell32.dll")]
        internal static extern nint SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags);

        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        internal static extern uint ExtractIconEx(string lpszFile, int nIconIndex, nint[] phiconLarge, nint[] phiconSmall, uint nIcons);

        /// <summary>
        /// Provides access to function required to delete handle. This method is used internally
        /// and is not required to be called separately.
        /// </summary>
        /// <param name="hIcon">Pointer to icon handle.</param>
        /// <returns>N/A</returns>
        [DllImport("User32.dll")]
        internal static extern int DestroyIcon(IntPtr hIcon);

        /// <summary>Flags that specify the path to be returned. Used in cases where the folder associated with a <see cref="SHKNOWNFOLDERID"/> (or CSIDL) can be moved, renamed, redirected, or roamed across languages by a user or administrator.</summary>
        internal enum SHGetFolderPathFlags
        {
            /// <summary>Retrieve the folder's current path.</summary>
            /// <remarks>
            /// The known folder system that underlies <see cref="SHGetFolderPath(System.IntPtr, SHCSIDL, System.IntPtr, SHGetFolderPathFlags, char*)"/> allows users or administrators to redirect a known folder to a location that suits their needs.
            /// This is achieved by calling IKnownFolderManager::Redirect, which sets the "current" value of the folder associated with the SHGFP_TYPE_CURRENT flag
            /// </remarks>
            SHGFP_TYPE_CURRENT = 0,

            /// <summary>Retrieve the folder's default path.</summary>
            /// <remarks>
            /// The default value of the folder, which is the location of the folder if a user or administrator had not redirected it elsewhere,
            /// is retrieved by specifying the SHGFP_TYPE_DEFAULT flag. This value can be used to implement a "restore defaults" feature for a known folder.
            /// </remarks>
            SHGFP_TYPE_DEFAULT = 1,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SHFILEINFO
        {
            public SHFILEINFO(bool b)
            {
                hIcon = nint.Zero; iIcon = 0; dwAttributes = 0; szDisplayName = ""; szTypeName = "";
            }
            public nint hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 80)]
            public string szTypeName;
        };

        [Flags]
        internal enum SHFileInfoFlags : int
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
            SHGFI_ATTR_SPECIFIED    = 0x000020000,     // get only specified attribtes
            SHGFI_LARGEICON         = 0x000000000,     // get large icon
            SHGFI_SMALLICON         = 0x000000001,     // get small icon
            SHGFI_OPENICON          = 0x000000002,     // get open icon
            SHGFI_SHELLICONSIZE     = 0x000000004,     // get shell size icon
            SHGFI_PIDL              = 0x000000008,     // pszPath is a pidl
            SHGFI_USEFILEATTRIBUTES = 0x000000010,     // use passed dwFileAttribute
            SHGFI_ADDOVERLAYS       = 0x000000020,     // apply the appropriate overlays
            SHGFI_OVERLAYINDEX      = 0x000000040,     // Get the index of the overlay in the upper 8 bits of the iIcon
        }

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
            SHGFI_USEFILEATTRIBUTES = 0x000000010      // use passed dwFileAttribute
        }

        public const uint SHGFI_LARGEICON         = 0x000000000;   // get large icon
        public const uint SHGFI_SMALLICON         = 0x000000001;   // get small icon
        public const uint SHGFI_OPENICON          = 0x000000002;   // get open icon
        public const uint SHGFI_SHELLICONSIZE     = 0x000000004;   // get shell size icon
        public const uint SHGFI_PIDL              = 0x000000008;   // pszPath is a pidl
        public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;   // use passed dwFileAttribute
        public const uint SHGFI_ADDOVERLAYS       = 0x000000020;   // apply the appropriate overlays
        public const uint SHGFI_OVERLAYINDEX      = 0x000000040;   // Get the index of the overlay
        public const uint SHGFI_ICON              = 0x000000100;   // get icon
        public const uint SHGFI_DISPLAYNAME       = 0x000000200;   // get display name
        public const uint SHGFI_TYPENAME          = 0x000000400;   // get type name
        public const uint SHGFI_ATTRIBUTES        = 0x000000800;   // get attributes
        public const uint SHGFI_ICONLOCATION      = 0x000001000;   // get icon location
        public const uint SHGFI_EXETYPE           = 0x000002000;   // return exe type
        public const uint SHGFI_SYSICONINDEX      = 0x000004000;   // get system icon index
        public const uint SHGFI_LINKOVERLAY       = 0x000008000;   // put a link overlay on icon
        public const uint SHGFI_SELECTED          = 0x000010000;   // show icon in selected state
        public const uint SHGFI_ATTR_SPECIFIED    = 0x000020000;   // get only specified attributes

        public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        public const uint FILE_ATTRIBUTE_NORMAL    = 0x00000080;




        /// <summary>
        /// Possible flags for the SHFileOperation method.
        /// </summary>
        [Flags]
        internal enum SHFileOperationFlags : ushort
        {
            /// <summary>
            /// Do not show a dialog during the process
            /// </summary>
            FOF_SILENT          = 0x0004,
            /// <summary>
            /// Do not ask the user to confirm selection
            /// </summary>
            FOF_NOCONFIRMATION  = 0x0010,
            /// <summary>
            /// Delete the file to the recycle bin.  (Required flag to send a file to the bin
            /// </summary>
            FOF_ALLOWUNDO       = 0x0040,
            /// <summary>
            /// Do not show the names of the files or folders that are being recycled.
            /// </summary>
            FOF_SIMPLEPROGRESS  = 0x0100,
            /// <summary>
            /// Surpress errors, if any occur during the process.
            /// </summary>
            FOF_NOERRORUI       = 0x0400,
            /// <summary>
            /// Warn if files are too big to fit in the recycle bin and will need
            /// to be deleted completely.
            /// </summary>
            FOF_WANTNUKEWARNING = 0x4000,
        }

        /// <summary>
        /// File Operation Function Type for SHFileOperation
        /// </summary>
        internal enum SHFileOperationType : uint
        {
            /// <summary>
            /// Move the objects
            /// </summary>
            FO_MOVE   = 0x0001,
            /// <summary>
            /// Copy the objects
            /// </summary>
            FO_COPY   = 0x0002,
            /// <summary>
            /// Delete (or recycle) the objects
            /// </summary>
            FO_DELETE = 0x0003,
            /// <summary>
            /// Rename the object(s)
            /// </summary>
            FO_RENAME = 0x0004,
        }

        /// <summary>
        /// SHFILEOPSTRUCT for SHFileOperation from COM
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.U4)]
            public SHFileOperationType wFunc;
            public string pFrom;
            public string pTo;
            public SHFileOperationFlags fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        internal static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

        internal static void SendToRecycleBin(string path, SHFileOperationFlags flags = SHFileOperationFlags.FOF_WANTNUKEWARNING)
        {
            SendToRecycleBin(new[] { path }, flags);
        }

        internal static void SendToRecycleBin(IList<string> deletePaths, SHFileOperationFlags flags = SHFileOperationFlags.FOF_WANTNUKEWARNING)
        {
            try
            {
                var fs = new SHFILEOPSTRUCT
                {
                    wFunc = SHFileOperationType.FO_DELETE,
                    pFrom = string.Join("\0", deletePaths) + '\0' + '\0',
                    fFlags = SHFileOperationFlags.FOF_ALLOWUNDO | flags
                };
                SHFileOperation(ref fs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SendToRecycleBin Error {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void RenameFolder(string srcPath,string destPath, SHFileOperationFlags flags = SHFileOperationFlags.FOF_WANTNUKEWARNING)
        {
            try
            {
                var fs = new SHFILEOPSTRUCT
                {
                    wFunc = SHFileOperationType.FO_RENAME,
                    pFrom = srcPath,
                    pTo = destPath,
                    fFlags = SHFileOperationFlags.FOF_ALLOWUNDO | flags
                };
                SHFileOperation(ref fs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RenameFolder Error {ex.Message}","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        internal static void MoveFiles(IList<string> srcPaths, string destFolder, bool isMove = false)
        {
            try
            {
                SHFILEOPSTRUCT Struct = new SHFILEOPSTRUCT()
                {
                    hwnd = default(IntPtr),
                    wFunc = isMove ? SHFileOperationType.FO_MOVE : SHFileOperationType.FO_COPY,
                    pTo = destFolder,
                    pFrom = string.Join("\0", srcPaths) + '\0' + '\0',
                    fFlags = SHFileOperationFlags.FOF_ALLOWUNDO | SHFileOperationFlags.FOF_WANTNUKEWARNING
                };

                SHFileOperation(ref Struct);
            }
            catch(Exception ex) {
                MessageBox.Show($"MoveFiles Error {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        internal static void MoveFiles(string srcFile, string destFolder, bool isMove = false)
        {
            MoveFiles(new string[] { srcFile }, destFolder, isMove);
        }
    }
}
