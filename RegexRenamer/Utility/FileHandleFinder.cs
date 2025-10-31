using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RegexRenamer.Utility;

public class FileHandleFinder
{
    private static class NativeMethods
    {
        [DllImport("ntdll.dll")]
        public static extern uint NtQuerySystemInformation(
            uint SystemInformationClass,
            IntPtr SystemInformation,
            uint SystemInformationLength,
            out uint ReturnLength);

        [DllImport("ntdll.dll")]
        public static extern uint NtQueryObject(
            IntPtr ObjectHandle,
            uint ObjectInformationClass,
            IntPtr ObjectInformation,
            uint ObjectInformationLength,
            out uint ReturnLength);

        [DllImport("kernel32.dll")]
        public static extern bool DuplicateHandle(
            IntPtr hSourceProcessHandle,
            IntPtr hSourceHandle,
            IntPtr hTargetProcessHandle,
            out IntPtr lpTargetHandle,
            uint dwDesiredAccess,
            bool bInheritHandle,
            uint dwOptions);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(
            uint dwDesiredAccess,
            bool bInheritHandle,
            uint dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SYSTEM_HANDLE_INFORMATION
    {
        public uint HandleCount;
        // Useless on x64, it's part of SYSTEM_HANDLE_TABLE_ENTRY_INFO
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SYSTEM_HANDLE_TABLE_ENTRY_INFO
    {
        public ushort ProcessId;
        public ushort CreatorBackTraceIndex;
        public byte ObjectTypeNumber;
        public byte HandleAttributes;
        public ushort HandleValue;
        public IntPtr Object;
        public uint GrantedAccess;
    }

    private enum OBJECT_INFORMATION_CLASS
    {
        ObjectNameInformation = 1,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct UNICODE_STRING
    {
        public ushort Length;
        public ushort MaximumLength;
        public IntPtr Buffer;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct OBJECT_NAME_INFORMATION
    {
        public UNICODE_STRING Name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public char[] Path;
    }

    public static List<IntPtr> FindFileHandles(string targetFilePath)
    {
        var handles = new List<IntPtr>();
        var currentProcess = Process.GetCurrentProcess();
        var currentProcessId = (uint)currentProcess.Id;

        // Normalize the file path
        targetFilePath = Path.GetFullPath(targetFilePath).ToLower();

        // Get all system handles
        uint systemInfoLength = 0;
        NativeMethods.NtQuerySystemInformation(16, IntPtr.Zero, 0, out systemInfoLength);
        IntPtr systemInfo = Marshal.AllocHGlobal((int)systemInfoLength);

        if (NativeMethods.NtQuerySystemInformation(16, systemInfo, systemInfoLength, out _) == 0)
        {
            uint handleSize = (uint)Marshal.SizeOf<SYSTEM_HANDLE_TABLE_ENTRY_INFO>();
            IntPtr entryPtr = new IntPtr(systemInfo.ToInt64() + IntPtr.Size);

            for (int i = 0; i < Marshal.PtrToStructure<SYSTEM_HANDLE_INFORMATION>(systemInfo).HandleCount; i++)
            {
                var handleInfo = Marshal.PtrToStructure<SYSTEM_HANDLE_TABLE_ENTRY_INFO>(entryPtr);
                entryPtr = new IntPtr(entryPtr.ToInt64() + handleSize);

                if (handleInfo.ProcessId != currentProcessId)
                {
                    continue;
                }

                IntPtr duplicatedHandle = IntPtr.Zero;
                IntPtr processHandle = NativeMethods.OpenProcess(0x0040, false, handleInfo.ProcessId); // PROCESS_DUP_HANDLE

                if (processHandle != IntPtr.Zero)
                {
                    if (NativeMethods.DuplicateHandle(processHandle, new IntPtr(handleInfo.HandleValue),
                        currentProcess.Handle, out duplicatedHandle, 0, false, 0x0002)) // DUPLICATE_SAME_ACCESS
                    {
                        uint objectNameLength = 0;
                        NativeMethods.NtQueryObject(duplicatedHandle, (uint)OBJECT_INFORMATION_CLASS.ObjectNameInformation, IntPtr.Zero, 0, out objectNameLength);

                        if (objectNameLength > 0)
                        {
                            IntPtr objectNameInfo = Marshal.AllocHGlobal((int)objectNameLength);
                            if (NativeMethods.NtQueryObject(duplicatedHandle, (uint)OBJECT_INFORMATION_CLASS.ObjectNameInformation, objectNameInfo, objectNameLength, out _) == 0)
                            {
                                var nameInfo = Marshal.PtrToStructure<OBJECT_NAME_INFORMATION>(objectNameInfo);
                                string path = Marshal.PtrToStringUni(nameInfo.Name.Buffer, nameInfo.Name.Length / 2);

                                if (path != null && path.ToLower().Contains(targetFilePath))
                                {
                                    handles.Add(new IntPtr(handleInfo.HandleValue));
                                }
                            }
                            Marshal.FreeHGlobal(objectNameInfo);
                        }
                        NativeMethods.CloseHandle(duplicatedHandle);
                    }
                    NativeMethods.CloseHandle(processHandle);
                }
            }
        }
        Marshal.FreeHGlobal(systemInfo);
        return handles;
    }
}
