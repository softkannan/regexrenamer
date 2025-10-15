using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Windows.UI.Composition;

using HWND = System.IntPtr;
using HANDLE = System.IntPtr;
using RegexRenamer.Controls;
using Windows.Storage;


namespace RegexRenamer.Native
{
    public static class ControlExtensions
    {
        public class ThemeName
        {
            public const bool Light_Theme = true;
            public static string Explorer { get => Light_Theme ? LightMode_Explorer :  DarkMode_Explorer; } 
            public static string CFD { get => Light_Theme ? LightMode_CFD :  DarkMode_CFD; }
            public static string ItemsView { get => Light_Theme ? LightMode_ItemsView :  DarkMode_ItemsView; }

            public const string DarkMode_Explorer = "DarkMode_Explorer";
            public const string DarkMode_CFD = "DarkMode_CFD";
            public const string DarkMode_ItemsView = "DarkMode_ItemsView";

            public const string LightMode_Explorer = "explorer";
            public const string LightMode_CFD = "CFD";
            public const string LightMode_ItemsView = "ItemsView";
        }

        internal enum PreferredAppMode
        {
            Default,
            AllowDark,
            ForceDark,
            ForceLight,
            Max
        }

        [Flags]
	public enum DWMWINDOWATTRIBUTE : uint
	{
		/// <summary>
		/// Use with DwmGetWindowAttribute. Discovers whether non-client rendering is enabled. The retrieved value is of type BOOL. TRUE if non-client rendering is enabled; otherwise, FALSE.
		/// </summary>
		DWMWA_NCRENDERING_ENABLED = 1,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Sets the non-client rendering policy. The pvAttribute parameter points to a value from the DWMNCRENDERINGPOLICY enumeration.
		/// </summary>
		DWMWA_NCRENDERING_POLICY,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Enables or forcibly disables DWM transitions. The pvAttribute parameter points to a value of type BOOL. TRUE to disable transitions, or FALSE to enable transitions.
		/// </summary>
		DWMWA_TRANSITIONS_FORCEDISABLED,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Enables content rendered in the non-client area to be visible on the frame drawn by DWM. The pvAttribute parameter points to a value of type BOOL. TRUE to enable content rendered in the non-client area to be visible on the frame; otherwise, FALSE.
		/// </summary>
		DWMWA_ALLOW_NCPAINT,

		/// <summary>
		/// Use with DwmGetWindowAttribute. Retrieves the bounds of the caption button area in the window-relative space. The retrieved value is of type RECT. If the window is minimized or otherwise not visible to the user, then the value of the RECT retrieved is undefined. You should check whether the retrieved RECT contains a boundary that you can work with, and if it doesn't then you can conclude that the window is minimized or otherwise not visible.
		/// </summary>
		DWMWA_CAPTION_BUTTON_BOUNDS,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Specifies whether non-client content is right-to-left (RTL) mirrored. The pvAttribute parameter points to a value of type BOOL. TRUE if the non-client content is right-to-left (RTL) mirrored; otherwise, FALSE.
		/// </summary>
		DWMWA_NONCLIENT_RTL_LAYOUT,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Forces the window to display an iconic thumbnail or peek representation (a static bitmap), even if a live or snapshot representation of the window is available. This value is normally set during a window's creation, and not changed throughout the window's lifetime. Some scenarios, however, might require the value to change over time. The pvAttribute parameter points to a value of type BOOL. TRUE to require a iconic thumbnail or peek representation; otherwise, FALSE.
		/// </summary>
		DWMWA_FORCE_ICONIC_REPRESENTATION,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Sets how Flip3D treats the window. The pvAttribute parameter points to a value from the DWMFLIP3DWINDOWPOLICY enumeration.
		/// </summary>
		DWMWA_FLIP3D_POLICY,

		/// <summary>
		/// Use with DwmGetWindowAttribute. Retrieves the extended frame bounds rectangle in screen space. The retrieved value is of type RECT.
		/// </summary>
		DWMWA_EXTENDED_FRAME_BOUNDS,

		/// <summary>
		/// Use with DwmSetWindowAttribute. The window will provide a bitmap for use by DWM as an iconic thumbnail or peek representation (a static bitmap) for the window. DWMWA_HAS_ICONIC_BITMAP can be specified with DWMWA_FORCE_ICONIC_REPRESENTATION. DWMWA_HAS_ICONIC_BITMAP normally is set during a window's creation and not changed throughout the window's lifetime. Some scenarios, however, might require the value to change over time. The pvAttribute parameter points to a value of type BOOL. TRUE to inform DWM that the window will provide an iconic thumbnail or peek representation; otherwise, FALSE. Windows Vista and earlier: This value is not supported.
		/// </summary>
		DWMWA_HAS_ICONIC_BITMAP,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Do not show peek preview for the window. The peek view shows a full-sized preview of the window when the mouse hovers over the window's thumbnail in the taskbar. If this attribute is set, hovering the mouse pointer over the window's thumbnail dismisses peek (in case another window in the group has a peek preview showing). The pvAttribute parameter points to a value of type BOOL. TRUE to prevent peek functionality, or FALSE to allow it. Windows Vista and earlier: This value is not supported.
		/// </summary>
		DWMWA_DISALLOW_PEEK,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Prevents a window from fading to a glass sheet when peek is invoked. The pvAttribute parameter points to a value of type BOOL. TRUE to prevent the window from fading during another window's peek, or FALSE for normal behavior. Windows Vista and earlier: This value is not supported.
		/// </summary>
		DWMWA_EXCLUDED_FROM_PEEK,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Cloaks the window such that it is not visible to the user. The window is still composed by DWM. Using with DirectComposition: Use the DWMWA_CLOAK flag to cloak the layered child window when animating a representation of the window's content via a DirectComposition visual that has been associated with the layered child window. For more details on this usage case, see How to animate the bitmap of a layered child window. Windows 7 and earlier: This value is not supported.
		/// </summary>
		DWMWA_CLOAK,

		/// <summary>
		/// Use with DwmGetWindowAttribute. If the window is cloaked, provides one of the following values explaining why. DWM_CLOAKED_APP (value 0x0000001). The window was cloaked by its owner application. DWM_CLOAKED_SHELL(value 0x0000002). The window was cloaked by the Shell. DWM_CLOAKED_INHERITED(value 0x0000004). The cloak value was inherited from its owner window. Windows 7 and earlier: This value is not supported.
		/// </summary>
		DWMWA_CLOAKED,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Freeze the window's thumbnail image with its current visuals. Do no further live updates on the thumbnail image to match the window's contents. Windows 7 and earlier: This value is not supported.
		/// </summary>
		DWMWA_FREEZE_REPRESENTATION,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Enables a non-UWP window to use host backdrop brushes. If this flag is set, then a Win32 app that calls Windows::UI::Composition APIs can build transparency effects using the host backdrop brush (see Compositor.CreateHostBackdropBrush). The pvAttribute parameter points to a value of type BOOL. TRUE to enable host backdrop brushes for the window, or FALSE to disable it. This value is supported starting with Windows 11 Build 22000.
		/// </summary>
		DWMWA_USE_HOSTBACKDROPBRUSH,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Allows the window frame for this window to be drawn in dark mode colors when the dark mode system setting is enabled. For compatibility reasons, all windows default to light mode regardless of the system setting. The pvAttribute parameter points to a value of type BOOL. TRUE to honor dark mode for the window, FALSE to always use light mode. This value is supported starting with Windows 10 Build 17763.
		/// </summary>
		DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Allows the window frame for this window to be drawn in dark mode colors when the dark mode system setting is enabled. For compatibility reasons, all windows default to light mode regardless of the system setting. The pvAttribute parameter points to a value of type BOOL. TRUE to honor dark mode for the window, FALSE to always use light mode. This value is supported starting with Windows 11 Build 22000.
		/// </summary>
		DWMWA_USE_IMMERSIVE_DARK_MODE = 20,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Specifies the rounded corner preference for a window. The pvAttribute parameter points to a value of type DWM_WINDOW_CORNER_PREFERENCE. This value is supported starting with Windows 11 Build 22000.
		/// </summary>
		DWMWA_WINDOW_CORNER_PREFERENCE = 33,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Specifies the color of the window border. The pvAttribute parameter points to a value of type COLORREF. The app is responsible for changing the border color according to state changes, such as a change in window activation. This value is supported starting with Windows 11 Build 22000.
		/// </summary>
		DWMWA_BORDER_COLOR,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Specifies the color of the caption. The pvAttribute parameter points to a value of type COLORREF. This value is supported starting with Windows 11 Build 22000.
		/// </summary>
		DWMWA_CAPTION_COLOR,

		/// <summary>
		/// Use with DwmSetWindowAttribute. Specifies the color of the caption text. The pvAttribute parameter points to a value of type COLORREF. This value is supported starting with Windows 11 Build 22000.
		/// </summary>
		DWMWA_TEXT_COLOR,

		/// <summary>
		/// Use with DwmGetWindowAttribute. Retrieves the width of the outer border that the DWM would draw around this window. The value can vary depending on the DPI of the window. The pvAttribute parameter points to a value of type UINT. This value is supported starting with Windows 11 Build 22000.
		/// </summary>
		DWMWA_VISIBLE_FRAME_BORDER_THICKNESS,

		/// <summary>
		/// The maximum recognized DWMWINDOWATTRIBUTE value, used for validation purposes.
		/// </summary>
		DWMWA_LAST,
	}

        internal enum WindowLongFlags
        {
            GWL_EXSTYLE = -20,
            GWL_HINSTANCE = -6,
            GWLP_HINSTANCE = -6,
            GWL_HWNDPARENT = -8,
            GWL_ID = -12,
            GWLP_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWLP_USERDATA = -21,
            GWL_WNDPROC = -4,
            GWLP_WNDPROC = -4,
            DWLP_USER = 0x8,
            DWLP_MSGRESULT = 0x0,
            DWLP_DLGPROC = 0x4,
            DWL_USER = 0x8,
            DWL_MSGRESULT = 0x0,
            DWL_DLGPROC = 0x4
        }

        [Flags]
        public enum AUTOCOMPLETEOPTIONS
        {
            ACO_NONE = 0,
            ACO_AUTOSUGGEST = 0x1,
            ACO_AUTOAPPEND = 0x2,
            ACO_SEARCH = 0x4,
            ACO_FILTERPREFIXES = 0x8,
            ACO_USETAB = 0x10,
            ACO_UPDOWNKEYDROPSLIST = 0x20,
            ACO_RTLREADING = 0x40,
            ACO_WORD_FILTER = 0x80,
            ACO_NOPREFIXFILTERING = 0x100
        }

        [ComImport, SuppressUnmanagedCodeSecurity, Guid("00bb2762-6a77-11d0-a535-00c04fd7d062"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), CoClass(typeof(CAutoComplete))]
        public interface IAutoComplete
        {
            void Init(HWND hwndEdit, IEnumString punkAcl, [MarshalAs(UnmanagedType.LPWStr)] string pwszRegKeyPath, [MarshalAs(UnmanagedType.LPWStr)] string pwszQuickComplete);
            void Enable([MarshalAs(UnmanagedType.Bool)] bool fEnable);
        }

        [ComImport, SuppressUnmanagedCodeSecurity, Guid("EAC04BC0-3791-11D2-BB95-0060977B464C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), CoClass(typeof(CAutoComplete))]
        public interface IAutoComplete2 : IAutoComplete
        {
            new void Init(HWND hwndEdit, IEnumString punkAcl, [MarshalAs(UnmanagedType.LPWStr)] string pwszRegKeyPath, [MarshalAs(UnmanagedType.LPWStr)] string pwszQuickComplete);
            new void Enable([MarshalAs(UnmanagedType.Bool)] bool fEnable);
            void SetOptions(AUTOCOMPLETEOPTIONS dwFlag);
            void GetOptions(out AUTOCOMPLETEOPTIONS dwFlag);
        }

        [ComImport, SuppressUnmanagedCodeSecurity, Guid("00BB2763-6A77-11D0-A535-00C04FD7D062"), ClassInterface(ClassInterfaceType.None)]
        public class CAutoComplete { }

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);


        [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int SetPreferredAppMode(PreferredAppMode preferredAppMode);

        [DllImport("uxtheme.dll", EntryPoint = "#136", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern void FlushMenuThemes();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, [In, Optional] IntPtr wParam, [In, Out, Optional] IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, [In, Optional] IntPtr wParam, string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, WindowLongFlags nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, WindowLongFlags nIndex, IntPtr dwNewLong);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        public static bool UseImmersiveDarkMode(this Control pThis, bool enabled)
        {
            if (IsWindows10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (IsWindows10OrGreater(18985))
                {
                    attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                }

                var useImmersiveDarkMode = enabled ? new[] { 0x01 } : new[] { 0x00 };
                return DwmSetWindowAttribute(pThis.Handle, (int)attribute, useImmersiveDarkMode, 4) == 0;
            }

            return false;
        }

        private static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
        }

        public static void ApplyTheme(this Control pThis)
        {
            //if (ThemeName.Light_Theme == false)
            //{
            //    SetPreferredAppMode(PreferredAppMode.AllowDark);
            //    FlushMenuThemes();
            //}

            foreach (Control control in pThis.Controls)
            {
                control.ApplyThemeInternal();
            }

            pThis.ControlAdded += (object sender, ControlEventArgs e) => { 
                ApplyThemeInternal(e.Control);
            };

            pThis.SetTheme();
        }


        private static void ApplyThemeInternal(this Control control)
        {
            control.HandleCreated += (object sender, EventArgs e) =>
            {
                control.SetTheme();
            };

            control.ControlAdded += (object sender, ControlEventArgs e) =>
            {
                ApplyThemeInternal(e.Control);
            };

            foreach (Control childControl in control.Controls)
            {
                childControl.ApplyThemeInternal();
            }
        }

        public static void SetTheme(this Control control)
        {
            /* 			    
                DWMWA_USE_IMMERSIVE_DARK_MODE:   https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute

                Use with DwmSetWindowAttribute. Allows the window frame for this window to be drawn in dark mode colors when the dark mode system setting is enabled. 
                For compatibility reasons, all windows default to light mode regardless of the system setting. 
                The pvAttribute parameter points to a value of type BOOL. TRUE to honor dark mode for the window, FALSE to always use light mode.

                This value is supported starting with Windows 11 Build 22000.

                SetWindowTheme:     https://learn.microsoft.com/en-us/windows/win32/api/uxtheme/nf-uxtheme-setwindowtheme
                Causes a window to use a different set of visual style information than its class normally uses.
             */
            int[] DarkModeOn = new[] { 0x01 }; //<- 1=True, 0=False

            if (control is ComboBox)
            {
                control.SetWindowTheme(ThemeName.CFD);
            }
            else if(control is TextBox)
            {
                var controlEdit = control as TextBox;
                if(controlEdit.Multiline == true)
                {
                    control.SetWindowTheme(ThemeName.Explorer);
                }
                else
                {
                    control.SetWindowTheme(ThemeName.CFD);
                }
            }
            else if(control is ListView)
            {
                control.SetWindowTheme(ThemeName.Explorer);
                //LV_Header := SendMessage(LVM_GETHEADER, 0, 0, GuiCtrlObj.hWnd)
                //DllCall("uxtheme\SetWindowTheme", "Ptr", LV_Header, "Str", Mode_ItemsView, "Ptr", 0)
            }
            else
            {
                control.SetWindowTheme(ThemeName.Explorer);
            }

            if (DwmSetWindowAttribute(control.Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, DarkModeOn, 4) != 0)
                DwmSetWindowAttribute(control.Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, DarkModeOn, 4);


            foreach (Control child in control.Controls)
            {
                if (child.Controls.Count != 0)
                    child.SetTheme();
            }
        }

        public static void SetWindowTheme1(this Control pThis, string subAppName = "explorer", string[] subIdList = null)
        {
            //Note that this trick also works exactly the same way for the ListView control.
            var idl = subIdList == null ? null : string.Join(";", subIdList);
            try { SetWindowTheme(pThis.Handle, subAppName, idl); } catch { }
            //SetPreferredAppMode(PreferredAppMode.AllowDark);
            const uint WM_THEMECHANGED = 0x031A;
            SendMessage(pThis.Handle, WM_THEMECHANGED, 0, IntPtr.Zero);
            pThis.Refresh();
        }

        public static void SetWindowTheme(this IWin32Window window, string subAppName, string[] subIdList = null)
        {
            var idl = subIdList == null ? null : string.Join(";", subIdList);
            try { SetWindowTheme(window.Handle, subAppName, idl); } catch { }
            const uint WM_THEMECHANGED = 0x031A;
            SendMessage(window.Handle, WM_THEMECHANGED, 0, IntPtr.Zero);
        }

        public static IntPtr SendMessage(this Control pThis, uint msg, [In, Optional] IntPtr wParam, [In, Out, Optional] IntPtr lParam)
        {
            return SendMessage(pThis.Handle, msg, wParam, lParam);
        }

        public static void SetStyle(this Control ctrl, int style, bool on = true)
        {
            var href = ctrl.Handle;
            int oldstyle = GetWindowLong(href, WindowLongFlags.GWL_STYLE).ToInt32();
            if ((oldstyle & style) != style && on)
                SetWindowLong(href, WindowLongFlags.GWL_STYLE, new IntPtr(oldstyle | style));
            else if ((oldstyle & style) == style && !on)
                SetWindowLong(href, WindowLongFlags.GWL_STYLE, new IntPtr(oldstyle & ~style));
            ctrl.Refresh();
        }

        public static void SetCueBanner(this ComboBox cb, string cueBannerText)
        {
            if (System.Environment.OSVersion.Version.Major >= 6)
            {
                if (!cb.IsHandleCreated) return;
                const int CBM_FIRST = 0x1700;
                const int CB_SETCUEBANNER = CBM_FIRST + 3;
                SendMessage(cb.Handle, (uint)CB_SETCUEBANNER, IntPtr.Zero, cueBannerText);
                cb.Invalidate();
            }
            else
                throw new PlatformNotSupportedException();
        }

        public static void SetCueBanner(this TextBox textBox, string cueBannerText, bool retainOnFocus = false)
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                const int ECM_FIRST = 0x1500;
                const int EM_SETCUEBANNER        = ECM_FIRST + 1;
                SendMessage(textBox.Handle, (uint)EM_SETCUEBANNER, new IntPtr(retainOnFocus ? 1 : 0), cueBannerText);
                textBox.Invalidate();
            }
            else
                throw new PlatformNotSupportedException();
        }

        public static void SetCustomAutoCompleteList(this TextBox textBox, IList<string> items, AUTOCOMPLETEOPTIONS options = AUTOCOMPLETEOPTIONS.ACO_AUTOSUGGEST)
        {
            var ac = new IAutoComplete2();
            ac.Init(textBox.Handle, new ComEnumStringImpl(items), null, null);
            ac.SetOptions(options);
        }

        private class ComEnumStringImpl : IEnumString
        {
            private readonly IList<string> list;
            private int cur;

            public ComEnumStringImpl(IList<string> items)
            {
                list = items;
            }

            void IEnumString.Clone(out IEnumString ppenum) { ppenum = new ComEnumStringImpl(list) { cur = cur }; }

            int IEnumString.Next(int celt, string[] rgelt, IntPtr pceltFetched)
            {
                if (celt < 0) return -2147024809;
                var idx = 0;
                while (cur < list.Count && celt > 0)
                {
                    rgelt[idx] = list[cur];
                    idx++;
                    cur++;
                    celt--;
                }
                if (pceltFetched != IntPtr.Zero)
                    Marshal.WriteInt32(pceltFetched, idx);
                return celt == 0 ? 0 : 1;
            }

            void IEnumString.Reset() { cur = 0; }

            int IEnumString.Skip(int celt) => (cur += celt) >= list.Count ? 1 : 0;
        }

    }
}
