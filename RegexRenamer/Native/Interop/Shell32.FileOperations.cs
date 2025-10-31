using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using HWND = System.IntPtr;
using HANDLE = System.IntPtr;

namespace PInvoke;

public class FileOperationAPI
{
    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    internal static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

    /// <summary>
    /// Flags that specify the path to be returned. 
    /// </summary>
    internal enum SHGetFolderPathFlags
    {
        /// Retrieve the folder's current path.
        SHGFP_TYPE_CURRENT = 0,
        /// Retrieve the folder's default path.
        SHGFP_TYPE_DEFAULT = 1,
    }
    
    /// <summary>
    /// Possible flags for the SHFileOperation method.
    /// </summary>
    [Flags]
    internal enum SHFileOperationFlags : ushort
    {
        /// Do not show a dialog during the process
        FOF_SILENT          = 0x0004,
        /// Do not ask the user to confirm selection
        FOF_NOCONFIRMATION  = 0x0010,
        /// Delete the file to the recycle bin.  (Required flag to send a file to the bin
        FOF_ALLOWUNDO       = 0x0040,
        /// Do not show the names of the files or folders that are being recycled.
        FOF_SIMPLEPROGRESS  = 0x0100,
        /// Surpress errors, if any occur during the process.
        FOF_NOERRORUI       = 0x0400,
        /// Warn if files are too big to fit in the recycle bin and will need
        /// to be deleted completely.
        FOF_WANTNUKEWARNING = 0x4000,
    }

    /// <summary>
    /// File Operation Function Type for SHFileOperation
    /// </summary>
    internal enum SHFileOperationType : uint
    {
        /// Move the objects
        FO_MOVE   = 0x0001,
        /// Copy the objects
        FO_COPY   = 0x0002,
        /// Delete (or recycle) the objects
        FO_DELETE = 0x0003,
        /// Rename the object(s)
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
