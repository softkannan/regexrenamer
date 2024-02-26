using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using System.ComponentModel;
using System.Security;
using System.Security.AccessControl;

namespace RegexRenamer;

internal class PrivilegeManager
{
    private Privilege _seDebug = new Privilege(Privilege.Debug);
    private Privilege _seTakeOwnership = new Privilege(Privilege.TakeOwnership);
    private Privilege _seBackup = new Privilege(Privilege.Backup);
    private Privilege _seRestore = new Privilege(Privilege.Restore);
    private Privilege _seShutdown = new Privilege(Privilege.Shutdown);
    private Privilege _seImpersonate = new Privilege(Privilege.Impersonate);
    private Privilege _seCreateToken = new Privilege(Privilege.CreateToken);
    private Privilege _seSecurity = new Privilege(Privilege.Security);
    private Privilege _seAssignPrimaryToken = new Privilege(Privilege.AssignPrimaryToken);


    //public const string CreateToken = "SeCreateTokenPrivilege";
    //public const string AssignPrimaryToken = "SeAssignPrimaryTokenPrivilege";
    //public const string LockMemory = "SeLockMemoryPrivilege";
    //public const string IncreaseQuota = "SeIncreaseQuotaPrivilege";
    //public const string UnsolicitedInput = "SeUnsolicitedInputPrivilege";
    //public const string MachineAccount = "SeMachineAccountPrivilege";
    //public const string TrustedComputingBase = "SeTcbPrivilege";
    //public const string Security = "SeSecurityPrivilege";
    //public const string TakeOwnership = "SeTakeOwnershipPrivilege";
    //public const string LoadDriver = "SeLoadDriverPrivilege";
    //public const string SystemProfile = "SeSystemProfilePrivilege";
    //public const string SystemTime = "SeSystemtimePrivilege";
    //public const string ProfileSingleProcess = "SeProfileSingleProcessPrivilege";
    //public const string IncreaseBasePriority = "SeIncreaseBasePriorityPrivilege";
    //public const string CreatePageFile = "SeCreatePagefilePrivilege";
    //public const string CreatePermanent = "SeCreatePermanentPrivilege";
    //public const string Backup = "SeBackupPrivilege";
    //public const string Restore = "SeRestorePrivilege";
    //public const string Shutdown = "SeShutdownPrivilege";
    //public const string Debug = "SeDebugPrivilege";
    //public const string Audit = "SeAuditPrivilege";
    //public const string SystemEnvironment = "SeSystemEnvironmentPrivilege";
    //public const string ChangeNotify = "SeChangeNotifyPrivilege";
    //public const string RemoteShutdown = "SeRemoteShutdownPrivilege";
    //public const string Undock = "SeUndockPrivilege";
    //public const string SyncAgent = "SeSyncAgentPrivilege";
    //public const string EnableDelegation = "SeEnableDelegationPrivilege";
    //public const string ManageVolume = "SeManageVolumePrivilege";
    //public const string Impersonate = "SeImpersonatePrivilege";
    //public const string CreateGlobal = "SeCreateGlobalPrivilege";
    //public const string TrustedCredentialManagerAccess = "SeTrustedCredManAccessPrivilege";
    //public const string ReserveProcessor = "SeReserveProcessorPrivilege";

    public PrivilegeManager()
    {
        _seDebug.Enable();
        _seImpersonate.Enable();
        _seBackup.Enable();
        _seRestore.Enable();
        _seShutdown.Enable();
        _seSecurity.Enable();
        _seTakeOwnership.Enable();
        //_seAssignPrimaryToken.Enable();
        //_seCreateToken.Enable();
    }
}



internal delegate void PrivilegedCallback(object state);

internal sealed class Privilege
{

    private const uint SE_PRIVILEGE_DISABLED = 0x00000000;
    private const uint SE_PRIVILEGE_ENABLED = 0x00000002;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct LUID
    {
        public uint LowPart;
        public uint HighPart;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct LUID_AND_ATTRIBUTES
    {
        public LUID Luid;
        public uint Attributes;

        public const UInt32 SE_PRIVILEGE_ENABLED_BY_DEFAULT = 0x00000001;
        public const UInt32 SE_PRIVILEGE_ENABLED = 0x00000002;
        public const UInt32 SE_PRIVILEGE_REMOVED = 0x00000004;
        public const UInt32 SE_PRIVILEGE_USED_FOR_ACCESS = 0x80000000;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct TOKEN_PRIVILEGE
    {
        public uint PrivilegeCount;
        public LUID_AND_ATTRIBUTES Privilege;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct PRIVILEGE_SET
    {
        public uint PrivilegeCount;
        public uint Control;  // use PRIVILEGE_SET_ALL_NECESSARY

        public static uint PRIVILEGE_SET_ALL_NECESSARY = 1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public LUID_AND_ATTRIBUTES[] Privilege;
    }

    [Flags]
    private enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VirtualMemoryOperation = 0x00000008,
        VirtualMemoryRead = 0x00000010,
        VirtualMemoryWrite = 0x00000020,
        DuplicateHandle = 0x00000040,
        CreateProcess = 0x000000080,
        SetQuota = 0x00000100,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        QueryLimitedInformation = 0x00001000,
        Synchronize = 0x00100000
    }

    [Flags]
    private enum TokenAccessLevels
    {
        AssignPrimary = 0x00000001,
        Duplicate = 0x00000002,
        Impersonate = 0x00000004,
        Query = 0x00000008,
        QuerySource = 0x00000010,
        AdjustPrivileges = 0x00000020,
        AdjustGroups = 0x00000040,
        AdjustDefault = 0x00000080,
        AdjustSessionId = 0x00000100,

        Read = 0x00020000 | Query,

        Write = 0x00020000 | AdjustPrivileges | AdjustGroups | AdjustDefault,

        AllAccess = 0x000F0000 | AssignPrimary | Duplicate | Impersonate | Query | QuerySource | AdjustPrivileges | AdjustGroups | AdjustDefault | AdjustSessionId,

        MaximumAllowed = 0x02000000
    }

    private enum SecurityImpersonationLevel
    {
        Anonymous = 0,
        Identification = 1,
        Impersonation = 2,
        Delegation = 3,
    }

    private enum TokenType
    {
        Primary = 1,
        Impersonation = 2,
    }


    private const int ERROR_SUCCESS = 0x0;
    private const int ERROR_ACCESS_DENIED = 0x5;
    private const int ERROR_NOT_ENOUGH_MEMORY = 0x8;
    private const int ERROR_NO_TOKEN = 0x3f0;
    private const int ERROR_NOT_ALL_ASSIGNED = 0x514;
    private const int ERROR_NO_SUCH_PRIVILEGE = 0x521;
    private const int ERROR_CANT_OPEN_ANONYMOUS = 0x543;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr handle);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool AdjustTokenPrivileges([In] SafeTokenHandle TokenHandle, [In] bool DisableAllPrivileges, [In] ref TOKEN_PRIVILEGE NewState, [In] uint BufferLength, [In, Out] ref TOKEN_PRIVILEGE PreviousState, [In, Out] ref uint ReturnLength);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool RevertToSelf();

    [DllImport("advapi32.dll", EntryPoint = "LookupPrivilegeValueW", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern
    bool LookupPrivilegeValue([In] string lpSystemName, [In] string lpName, [In, Out] ref LUID Luid);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetCurrentProcess();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetCurrentThread();

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool OpenProcessToken([In] IntPtr ProcessToken, [In] TokenAccessLevels DesiredAccess, [In, Out] ref SafeTokenHandle TokenHandle);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool OpenThreadToken([In]     IntPtr ThreadToken, [In] TokenAccessLevels DesiredAccess, [In] bool OpenAsSelf, [In, Out] ref SafeTokenHandle TokenHandle);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool DuplicateTokenEx([In] SafeTokenHandle ExistingToken, [In] TokenAccessLevels DesiredAccess, [In] IntPtr TokenAttributes, [In] SecurityImpersonationLevel ImpersonationLevel, [In] TokenType TokenType, [In, Out] ref SafeTokenHandle NewToken);

    [DllImport("advapi32.dll")]
    private extern static bool DuplicateToken(SafeTokenHandle ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref SafeTokenHandle DuplicateTokenHandle);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool SetThreadToken([In] IntPtr Thread, [In] SafeTokenHandle Token);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool PrivilegeCheck(IntPtr ClientToken, ref PRIVILEGE_SET RequiredPrivileges, out bool pfResult);

    #region Private static members
    private static LocalDataStoreSlot tlsSlot = Thread.AllocateDataSlot();
    private static HybridDictionary privileges = new HybridDictionary();
    private static HybridDictionary luids = new HybridDictionary();
    private static ReaderWriterLock privilegeLock = new ReaderWriterLock();
    #endregion

    #region Private members
    private bool needToRevert = false;
    private bool initialState = false;
    private bool stateWasChanged = false;
    private LUID luid;
    private readonly Thread currentThread = Thread.CurrentThread;
    private TlsContents tlsContents = null;
    #endregion

    #region Privilege names
    public const string CreateToken = "SeCreateTokenPrivilege";
    public const string AssignPrimaryToken = "SeAssignPrimaryTokenPrivilege";
    public const string LockMemory = "SeLockMemoryPrivilege";
    public const string IncreaseQuota = "SeIncreaseQuotaPrivilege";
    public const string UnsolicitedInput = "SeUnsolicitedInputPrivilege";
    public const string MachineAccount = "SeMachineAccountPrivilege";
    public const string TrustedComputingBase = "SeTcbPrivilege";
    public const string Security = "SeSecurityPrivilege";
    public const string TakeOwnership = "SeTakeOwnershipPrivilege";
    public const string LoadDriver = "SeLoadDriverPrivilege";
    public const string SystemProfile = "SeSystemProfilePrivilege";
    public const string SystemTime = "SeSystemtimePrivilege";
    public const string ProfileSingleProcess = "SeProfileSingleProcessPrivilege";
    public const string IncreaseBasePriority = "SeIncreaseBasePriorityPrivilege";
    public const string CreatePageFile = "SeCreatePagefilePrivilege";
    public const string CreatePermanent = "SeCreatePermanentPrivilege";
    public const string Backup = "SeBackupPrivilege";
    public const string Restore = "SeRestorePrivilege";
    public const string Shutdown = "SeShutdownPrivilege";
    public const string Debug = "SeDebugPrivilege";
    public const string Audit = "SeAuditPrivilege";
    public const string SystemEnvironment = "SeSystemEnvironmentPrivilege";
    public const string ChangeNotify = "SeChangeNotifyPrivilege";
    public const string RemoteShutdown = "SeRemoteShutdownPrivilege";
    public const string Undock = "SeUndockPrivilege";
    public const string SyncAgent = "SeSyncAgentPrivilege";
    public const string EnableDelegation = "SeEnableDelegationPrivilege";
    public const string ManageVolume = "SeManageVolumePrivilege";
    public const string Impersonate = "SeImpersonatePrivilege";
    public const string CreateGlobal = "SeCreateGlobalPrivilege";
    public const string TrustedCredentialManagerAccess = "SeTrustedCredManAccessPrivilege";
    public const string ReserveProcessor = "SeReserveProcessorPrivilege";
    #endregion

    #region LUID caching logic

    //
    // This routine is a wrapper around a hashtable containing mappings
    // of privilege names to luids
    //

    private static LUID LuidFromPrivilege(string privilege)
    {
        LUID luid;
        luid.LowPart = 0;
        luid.HighPart = 0;

        //
        // Look up the privilege LUID inside the cache
        //

       // RuntimeHelpers.PrepareConstrainedRegions();

        try
        {
            privilegeLock.AcquireReaderLock(Timeout.Infinite);

            if (luids.Contains(privilege))
            {
                luid = (LUID)luids[privilege];

                privilegeLock.ReleaseReaderLock();
            }
            else
            {
                privilegeLock.ReleaseReaderLock();

                if (false == LookupPrivilegeValue(null, privilege, ref luid))
                {
                    int error = Marshal.GetLastWin32Error();

                    if (error == ERROR_NOT_ENOUGH_MEMORY)
                    {
                        throw new OutOfMemoryException();
                    }
                    else if (error == ERROR_ACCESS_DENIED)
                    {
                        throw new UnauthorizedAccessException("Caller does not have the rights to look up privilege local unique identifier");
                    }
                    else if (error == ERROR_NO_SUCH_PRIVILEGE)
                    {
                        throw new ArgumentException(
                            string.Format("{0} is not a valid privilege name", privilege),
                            "privilege");
                    }
                    else
                    {
                        throw new Win32Exception(error);
                    }
                }

                privilegeLock.AcquireWriterLock(Timeout.Infinite);
            }
        }
        finally
        {
            if (privilegeLock.IsReaderLockHeld)
            {
                privilegeLock.ReleaseReaderLock();
            }

            if (privilegeLock.IsWriterLockHeld)
            {
                if (!luids.Contains(privilege))
                {
                    luids[privilege] = luid;
                    privileges[luid] = privilege;
                }

                privilegeLock.ReleaseWriterLock();
            }
        }

        return luid;
    }
    #endregion

    #region Nested classes
    private sealed class TlsContents : IDisposable
    {
        private bool disposed = false;
        private int referenceCount = 1;
        private SafeTokenHandle threadHandle = new SafeTokenHandle(IntPtr.Zero);
        private bool isImpersonating = false;

        private static SafeTokenHandle processHandle = new SafeTokenHandle(IntPtr.Zero);
        private static readonly object syncRoot = new object();

        #region Constructor and finalizer
        public TlsContents()
        {
            int error = 0;
            int cachingError = 0;
            bool success = true;

            if (processHandle.IsInvalid)
            {
                lock (syncRoot)
                {
                    if (processHandle.IsInvalid)
                    {
                        if (false == OpenProcessToken(
                                        GetCurrentProcess(),
                                        TokenAccessLevels.Duplicate,
                                        ref processHandle))
                        {
                            cachingError = Marshal.GetLastWin32Error();
                            success = false;
                        }
                    }
                }
            }

            // RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                //
                // Open the thread token; if there is no thread token,
                // copy the process token onto the thread
                //

                if (false == OpenThreadToken(
                    GetCurrentThread(),
                    TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges,
                    true,
                    ref this.threadHandle))
                {
                    if (success == true)
                    {
                        error = Marshal.GetLastWin32Error();

                        if (error != ERROR_NO_TOKEN)
                        {
                            success = false;
                        }

                        if (success == true)
                        {
                            error = 0;

                            if (false == DuplicateTokenEx(
                                processHandle,
                                TokenAccessLevels.Impersonate | TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges,
                                IntPtr.Zero,
                                SecurityImpersonationLevel.Impersonation,
                                TokenType.Impersonation,
                                ref this.threadHandle))
                            {
                                error = Marshal.GetLastWin32Error();
                                success = false;
                            }
                        }

                        if (success == true)
                        {
                            if (false == SetThreadToken(
                                IntPtr.Zero,
                                this.threadHandle))
                            {
                                error = Marshal.GetLastWin32Error();
                                success = false;
                            }
                        }

                        if (success == true)
                        {
                            //
                            // This thread is now impersonating; it needs to be reverted to its original state
                            //

                            this.isImpersonating = true;
                        }
                    }
                    else
                    {
                        error = cachingError;
                    }
                }
                else
                {
                    success = true;
                }
            }
            finally
            {
                if (!success)
                {
                    Dispose();
                }
            }

            if (error == ERROR_NOT_ENOUGH_MEMORY)
            {
                throw new OutOfMemoryException();
            }
            else if (error == ERROR_ACCESS_DENIED ||
                error == ERROR_CANT_OPEN_ANONYMOUS)
            {
                throw new UnauthorizedAccessException("The caller does not have the rights to perform the operation");
            }
            else if (error != 0)
            {
                throw new Win32Exception(error);
            }
        }

        ~TlsContents()
        {
            Dispose(false);
        }
        #endregion

        #region IDisposable implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (this.threadHandle != null)
            {
                this.threadHandle.Dispose();
                this.threadHandle = null;
            }

            if (this.isImpersonating)
            {
                RevertToSelf();
            }

            this.disposed = true;
        }
        #endregion

        #region Reference-counting
        public void IncrementReferenceCount()
        {
            this.referenceCount++;
        }

        public int DecrementReferenceCount()
        {
            int result = --this.referenceCount;

            if (result == 0)
            {
                Dispose();
            }

            return result;
        }

        public int ReferenceCountValue
        {
            get { return this.referenceCount; }
        }
        #endregion

        #region Properties
        public SafeTokenHandle ThreadHandle
        {
            get { return this.threadHandle; }
        }

        public bool IsImpersonating
        {
            get { return this.isImpersonating; }
        }
        #endregion
    }
    #endregion

    #region Constructor
    public Privilege(string privilegeName)
    {
        if (privilegeName == null)
        {
            throw new ArgumentNullException("privilegeName");
        }

        this.luid = LuidFromPrivilege(privilegeName);
    }
    #endregion

    #region Public methods and properties
    public void Enable()
    {
        this.ToggleState(true);
    }

    public void Disable()
    {
        this.ToggleState(false);
    }

    public void Revert()
    {
        int error = 0;

        //
        // All privilege operations must take place on the same thread
        //

        if (!this.currentThread.Equals(Thread.CurrentThread))
        {
            throw new InvalidOperationException("Operation must take place on the thread that created the object");
        }

        if (!this.NeedToRevert)
        {
            return;
        }

        //
        // This code must be eagerly prepared and non-interruptible.
        //

        // RuntimeHelpers.PrepareConstrainedRegions();

        try
        {
            //
            // The payload is entirely in the finally block
            // This is how we ensure that the code will not be
            // interrupted by catastrophic exceptions
            //
        }
        finally
        {
            bool success = true;

            try
            {
                //
                // Only call AdjustTokenPrivileges if we're not going to be reverting to self,
                // on this Revert, since doing the latter obliterates the thread token anyway
                //

                if (this.stateWasChanged &&
                    (this.tlsContents.ReferenceCountValue > 1 ||
                    !this.tlsContents.IsImpersonating))
                {
                    TOKEN_PRIVILEGE newState = new TOKEN_PRIVILEGE();
                    newState.PrivilegeCount = 1;
                    newState.Privilege.Luid = this.luid;
                    newState.Privilege.Attributes = (this.initialState ? SE_PRIVILEGE_ENABLED : SE_PRIVILEGE_DISABLED);

                    TOKEN_PRIVILEGE previousState = new TOKEN_PRIVILEGE();
                    uint previousSize = 0;

                    if (false == AdjustTokenPrivileges(
                                    this.tlsContents.ThreadHandle,
                                    false,
                                    ref newState,
                                    (uint)Marshal.SizeOf(previousState),
                                    ref previousState,
                                    ref previousSize))
                    {
                        error = Marshal.GetLastWin32Error();
                        success = false;
                    }
                }
            }
            finally
            {
                if (success)
                {
                    this.Reset();
                }
            }
        }

        if (error == ERROR_NOT_ENOUGH_MEMORY)
        {
            throw new OutOfMemoryException();
        }
        else if (error == ERROR_ACCESS_DENIED)
        {
            throw new UnauthorizedAccessException("Caller does not have the permission to change the privilege");
        }
        else if (error != 0)
        {
            throw new Win32Exception(error);
        }
    }

    public bool NeedToRevert
    {
        get { return this.needToRevert; }
    }

    public static void RunWithPrivilege(string privilege, bool enabled, PrivilegedCallback callback, object state)
    {
        if (callback == null)
        {
            throw new ArgumentNullException("callback");
        }

        Privilege p = new Privilege(privilege);

        //RuntimeHelpers.PrepareConstrainedRegions();

        try
        {
            if (enabled)
            {
                p.Enable();
            }
            else
            {
                p.Disable();
            }

            callback(state);
        }
        catch
        {
            p.Revert();
            throw;
        }
        finally
        {
            p.Revert();
        }
    }
    #endregion

    #region Private implementation
    private void ToggleState(bool enable)
    {
        int error = 0;

        //
        // All privilege operations must take place on the same thread
        //

        if (!this.currentThread.Equals(Thread.CurrentThread))
        {
            throw new InvalidOperationException("Operation must take place on the thread that created the object");
        }

        //
        // This privilege was already altered and needs to be reverted before it can be altered again
        //

        if (this.NeedToRevert)
        {
            throw new InvalidOperationException("Must revert the privilege prior to attempting this operation");
        }

        //
        // Need to make this block of code non-interruptible so that it would preserve
        // consistency of thread oken state even in the face of catastrophic exceptions
        //

        // RuntimeHelpers.PrepareConstrainedRegions();

        try
        {
            //
            // The payload is entirely in the finally block
            // This is how we ensure that the code will not be
            // interrupted by catastrophic exceptions
            //
        }
        finally
        {
            try
            {
                //
                // Retrieve TLS state
                //

                this.tlsContents = Thread.GetData(tlsSlot) as TlsContents;

                if (this.tlsContents == null)
                {
                    this.tlsContents = new TlsContents();
                    Thread.SetData(tlsSlot, this.tlsContents);
                }
                else
                {
                    this.tlsContents.IncrementReferenceCount();
                }

                TOKEN_PRIVILEGE newState = new TOKEN_PRIVILEGE();
                newState.PrivilegeCount = 1;
                newState.Privilege.Luid = this.luid;
                newState.Privilege.Attributes = enable ? SE_PRIVILEGE_ENABLED : SE_PRIVILEGE_DISABLED;

                TOKEN_PRIVILEGE previousState = new TOKEN_PRIVILEGE();
                uint previousSize = 0;

                //
                // Place the new privilege on the thread token and remember the previous state.
                //

                if (false == AdjustTokenPrivileges(
                                this.tlsContents.ThreadHandle,
                                false,
                                ref newState,
                                (uint)Marshal.SizeOf(previousState),
                                ref previousState,
                                ref previousSize))
                {
                    error = Marshal.GetLastWin32Error();
                }
                else if (ERROR_NOT_ALL_ASSIGNED == Marshal.GetLastWin32Error())
                {
                    error = ERROR_NOT_ALL_ASSIGNED;
                }
                else
                {
                    //
                    // This is the initial state that revert will have to go back to
                    //

                    this.initialState = ((previousState.Privilege.Attributes & SE_PRIVILEGE_ENABLED) != 0);

                    //
                    // Remember whether state has changed at all
                    //

                    this.stateWasChanged = (this.initialState != enable);

                    //
                    // If we had to impersonate, or if the privilege state changed we'll need to revert
                    //

                    this.needToRevert = this.tlsContents.IsImpersonating || this.stateWasChanged;
                }
            }
            finally
            {
                if (!this.needToRevert)
                {
                    this.Reset();
                }
            }
        }

        if (error == ERROR_NOT_ALL_ASSIGNED)
        {
            throw new PrivilegeNotHeldException(privileges[this.luid] as string);
        }
        if (error == ERROR_NOT_ENOUGH_MEMORY)
        {
            throw new OutOfMemoryException();
        }
        else if (error == ERROR_ACCESS_DENIED ||
            error == ERROR_CANT_OPEN_ANONYMOUS)
        {
            throw new UnauthorizedAccessException("The caller does not have the right to change the privilege");
        }
        else if (error != 0)
        {
            throw new Win32Exception(error);
        }
    }

    private void Reset()
    {
        //RuntimeHelpers.PrepareConstrainedRegions();

        try
        {
            // Payload is in the finally block
            // as a way to guarantee execution
        }
        finally
        {
            this.stateWasChanged = false;
            this.initialState = false;
            this.needToRevert = false;

            if (this.tlsContents != null)
            {
                if (0 == this.tlsContents.DecrementReferenceCount())
                {
                    this.tlsContents = null;
                    Thread.SetData(tlsSlot, null);
                }
            }
        }
    }
    #endregion
}



internal sealed class PrivilegeSet
{
    private const int ANYSIZE_ARRAY = 1;

    private const uint SE_PRIVILEGE_DISABLED = 0x00000000;
    private const uint SE_PRIVILEGE_ENABLED = 0x00000002;
    private const uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;
    private const uint STANDARD_RIGHTS_READ = 0x00020000;
    private const uint TOKEN_ASSIGN_PRIMARY = 0x00000001;
    private const uint TOKEN_DUPLICATE = 0x00000002;
    private const uint TOKEN_IMPERSONATE = 0x00000004;
    private const uint TOKEN_QUERY = 0x00000008;
    private const uint TOKEN_QUERY_SOURCE = 0x00000010;
    private const uint TOKEN_ADJUST_PRIVILEGES = 0x00000020;
    private const uint TOKEN_ADJUST_GROUPS = 0x00000040;
    private const uint TOKEN_ADJUST_DEFAULT = 0x00000080;
    private const uint TOKEN_ADJUST_SESSIONID = 0x00000100;
    private const uint TOKEN_READ = STANDARD_RIGHTS_READ | TOKEN_QUERY;
    private const uint TOKEN_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT | TOKEN_ADJUST_SESSIONID;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct LUID
    {
        public uint LowPart;
        public uint HighPart;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct LUID_AND_ATTRIBUTES
    {
        public LUID Luid;
        public uint Attributes;

        public const UInt32 SE_PRIVILEGE_ENABLED_BY_DEFAULT = 0x00000001;
        public const UInt32 SE_PRIVILEGE_ENABLED = 0x00000002;
        public const UInt32 SE_PRIVILEGE_REMOVED = 0x00000004;
        public const UInt32 SE_PRIVILEGE_USED_FOR_ACCESS = 0x80000000;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct TOKEN_PRIVILEGE
    {
        public uint PrivilegeCount;
        public LUID_AND_ATTRIBUTES Privilege;
    }

    private struct TOKEN_PRIVILEGES
    {
        public int PrivilegeCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ANYSIZE_ARRAY)]
        public LUID_AND_ATTRIBUTES[] Privileges;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct PRIVILEGE_SET
    {
        public uint PrivilegeCount;
        public uint Control;  // use PRIVILEGE_SET_ALL_NECESSARY

        public static uint PRIVILEGE_SET_ALL_NECESSARY = 1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public LUID_AND_ATTRIBUTES[] Privilege;
    }

    [Flags]
    private enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VirtualMemoryOperation = 0x00000008,
        VirtualMemoryRead = 0x00000010,
        VirtualMemoryWrite = 0x00000020,
        DuplicateHandle = 0x00000040,
        CreateProcess = 0x000000080,
        SetQuota = 0x00000100,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        QueryLimitedInformation = 0x00001000,
        Synchronize = 0x00100000
    }

    [Flags]
    private enum TokenAccessLevels
    {
        AssignPrimary = 0x00000001,
        Duplicate = 0x00000002,
        Impersonate = 0x00000004,
        Query = 0x00000008,
        QuerySource = 0x00000010,
        AdjustPrivileges = 0x00000020,
        AdjustGroups = 0x00000040,
        AdjustDefault = 0x00000080,
        AdjustSessionId = 0x00000100,

        Read = 0x00020000 | Query,

        Write = 0x00020000 | AdjustPrivileges | AdjustGroups | AdjustDefault,

        AllAccess = 0x000F0000 | AssignPrimary | Duplicate | Impersonate | Query | QuerySource | AdjustPrivileges | AdjustGroups | AdjustDefault | AdjustSessionId,

        MaximumAllowed = 0x02000000
    }

    private enum SecurityImpersonationLevel
    {
        Anonymous = 0,
        Identification = 1,
        Impersonation = 2,
        Delegation = 3,
    }

    private enum TokenType
    {
        Primary = 1,
        Impersonation = 2,
    }

    private const int ERROR_SUCCESS = 0x0;
    private const int ERROR_ACCESS_DENIED = 0x5;
    private const int ERROR_NOT_ENOUGH_MEMORY = 0x8;
    private const int ERROR_NO_TOKEN = 0x3f0;
    private const int ERROR_NOT_ALL_ASSIGNED = 0x514;
    private const int ERROR_NO_SUCH_PRIVILEGE = 0x521;
    private const int ERROR_CANT_OPEN_ANONYMOUS = 0x543;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr handle);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool AdjustTokenPrivileges([In] SafeTokenHandle TokenHandle, [In] bool DisableAllPrivileges, [In] ref TOKEN_PRIVILEGES NewState, [In] uint BufferLength, [In, Out] ref TOKEN_PRIVILEGES PreviousState, [In, Out] ref uint ReturnLength);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool RevertToSelf();

    [DllImport("advapi32.dll", EntryPoint = "LookupPrivilegeValueW", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern
    bool LookupPrivilegeValue([In] string lpSystemName, [In] string lpName, [In, Out] ref LUID Luid);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetCurrentProcess();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetCurrentThread();

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool OpenProcessToken([In] IntPtr ProcessToken, [In] TokenAccessLevels DesiredAccess, [In, Out] ref SafeTokenHandle TokenHandle);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool OpenThreadToken([In]     IntPtr ThreadToken, [In] TokenAccessLevels DesiredAccess, [In] bool OpenAsSelf, [In, Out] ref SafeTokenHandle TokenHandle);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool DuplicateTokenEx([In] SafeTokenHandle ExistingToken, [In] TokenAccessLevels DesiredAccess, [In] IntPtr TokenAttributes, [In] SecurityImpersonationLevel ImpersonationLevel, [In] TokenType TokenType, [In, Out] ref SafeTokenHandle NewToken);

    [DllImport("advapi32.dll")]
    private extern static bool DuplicateToken(SafeTokenHandle ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref SafeTokenHandle DuplicateTokenHandle);


    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool SetThreadToken([In] IntPtr Thread, [In] SafeTokenHandle Token);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool PrivilegeCheck(SafeTokenHandle ClientToken, ref PRIVILEGE_SET RequiredPrivileges, out bool pfResult);

    // Now I will create functions that use the above definitions, so we can use them directly from PowerShell :P
    private static bool IsPrivilegeEnabled(string Privilege)
    {
        bool ret;
        LUID luid = new LUID();
        IntPtr hProcess = GetCurrentProcess();
        SafeTokenHandle hToken= SafeTokenHandle.InvalidHandle;
        if (hProcess == IntPtr.Zero) return false;
        if (!OpenProcessToken(hProcess, TokenAccessLevels.Query, ref hToken)) return false;
        if (!LookupPrivilegeValue(null, Privilege, ref luid)) return false;
        PRIVILEGE_SET privs = new PRIVILEGE_SET { Privilege = new LUID_AND_ATTRIBUTES[1], Control = PRIVILEGE_SET.PRIVILEGE_SET_ALL_NECESSARY, PrivilegeCount = 1 };
        privs.Privilege[0].Luid = luid;
        privs.Privilege[0].Attributes = LUID_AND_ATTRIBUTES.SE_PRIVILEGE_ENABLED;
        if (!PrivilegeCheck(hToken, ref privs, out ret)) return false;
        return ret;
    }

    private static bool EnablePrivilege(string Privilege)
    {
        LUID luid = new LUID();
        IntPtr hProcess = GetCurrentProcess();
        SafeTokenHandle hToken = SafeTokenHandle.InvalidHandle;
        if (!OpenProcessToken(hProcess, TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges, ref hToken)) return false;
        if (!LookupPrivilegeValue(null, Privilege, ref luid)) return false;
        // First, a LUID_AND_ATTRIBUTES structure that points to Enable a privilege.
        LUID_AND_ATTRIBUTES luAttr = new LUID_AND_ATTRIBUTES { Luid = luid, Attributes = LUID_AND_ATTRIBUTES.SE_PRIVILEGE_ENABLED };
        // Now we create a TOKEN_PRIVILEGES structure with our modifications
        TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES { PrivilegeCount = 1, Privileges = new LUID_AND_ATTRIBUTES[1] };
        tp.Privileges[0] = luAttr;
        TOKEN_PRIVILEGES oldState = new TOKEN_PRIVILEGES(); // Our old state.
        uint returnLength = 0;
        if (!AdjustTokenPrivileges(hToken, false, ref tp, (UInt32)Marshal.SizeOf(tp), ref oldState, ref returnLength)) return false;
        return true;
    }
}

internal sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private SafeTokenHandle() : base(true) { }

    // 0 is an Invalid Handle
    internal SafeTokenHandle(IntPtr handle) : base(true)
    {
        SetHandle(handle);
    }

    internal static SafeTokenHandle InvalidHandle
    {
        get { return new SafeTokenHandle(IntPtr.Zero); }
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr handle);

    override protected bool ReleaseHandle()
    {
        return CloseHandle(handle);
    }
}
