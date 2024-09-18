using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer
{
    public partial class MainForm
    {
        // load/save settings & regex history

        private void LoadSettings()
        {
#if !DEBUG
      try
      {
#endif

            // general

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\RegexRenamer"))
            {
                if (key != null)
                {
                    if (ActivePath == null)
                        ActivePath = (string)key.GetValue("LastPath", "");
                    fbdMoveCopy.SelectedPath = (string)key.GetValue("MoveCopyPath", "");
                    //RenameFolders = (string)key.GetValue("RenameFolders") == "True";

                    try
                    {
                        int maxFilesOverride = (int)key.GetValue("MaxFileLimit", 1000);
                        if (maxFilesOverride > MAX_FILES)
                            MAX_FILES = maxFilesOverride;

                        int maxViewFilesOverride = (int)key.GetValue("MaxViewFileLimit", 200);
                        if (maxViewFilesOverride > MAX_VIEW_PAGE_SIZE)
                            MAX_VIEW_PAGE_SIZE = maxViewFilesOverride;
                    }
                    catch { } // ignore if wrong reg key type
                    key.Close();
                }
            }


            // options

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\RegexRenamer\\Options"))
            {
                if (key != null)
                {
                    EnableUpdates = false;

                    itmOptionsShowHidden.Checked = (string)key.GetValue("ShowHiddenFiles") == "True";
                    itmOptionsPreserveExt.Checked = (string)key.GetValue("PreserveExtension") == "True";
                    itmOptionsRealtimePreview.Checked = (string)key.GetValue("RealtimePreview", "True") == "True";
                    itmOptionsAllowRenSub.Checked = (string)key.GetValue("AllowRenameIntoSubfolders") == "True";
                    itmOptionsRememberWinPos.Checked = (string)key.GetValue("RememberWindowPosition", "True") == "True";
                    itmOptionsRenameSelectedRows.Checked = (string)key.GetValue("OnlyRenameSelectedRows") == "True";

                    EnableUpdates = true;

                    key.Close();
                }
            }


            // explorer shell context menu

            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey("Folder\\shell\\RegexRenamer\\command"))
            {
                if (key != null)
                {
                    if (((string)key.GetValue("", "")).StartsWith(Application.ExecutablePath))
                        itmOptionsAddContextMenu.Checked = true;

                    key.Close();
                }
            }


            // stats

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\RegexRenamer\\Stats"))
            {
                if (key != null)
                {
                    countProgLaunches = (int)key.GetValue("ProgramLaunches", 0) + 1;
                    countFilesRenamed = (int)key.GetValue("FilesRenamed", 0);

                    key.Close();
                }
            }


            // check for old reg keys (maintain backwards compatibility)

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\RegexRenamer", true))
            {
                if (key != null)
                {
                    int cpl = (int)key.GetValue("CountProgLaunches", 0);
                    int cfr = (int)key.GetValue("CountFilesRenamed", 0);
                    if (cpl > 0) countProgLaunches = cpl;
                    if (cfr > 0) countFilesRenamed = cfr;

                    key.DeleteValue("CountProgLaunches", false);
                    key.DeleteValue("CountFilesRenamed", false);
                    key.DeleteValue("ShowHiddenFiles", false);
                    key.DeleteValue("PreserveExtension", false);
                }
            }


            // window position

            if (!itmOptionsRememberWinPos.Checked) return;  // skip

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\RegexRenamer\\WindowPosition"))
            {
                if (key != null)
                {
                    if ((string)key.GetValue("WindowState") == "Maximized")
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                    else // not maximized
                    {
                        // get size and offset from registry

                        object oWinX = key.GetValue("WindowX", "0"); // used to be a dword, now a string
                        object oWinY = key.GetValue("WindowY", "0");
                        int winx = oWinX is int ? (int)oWinX : int.Parse((string)oWinX);
                        int winy = oWinY is int ? (int)oWinY : int.Parse((string)oWinY);

                        int height = (int)key.GetValue("WindowHeight", -1);
                        int width = (int)key.GetValue("WindowWidth", -1);


                        // validate (to prevent drawing window off-screen)

                        bool valid = true;

                        Screen screen = null;
                        foreach (Screen s in Screen.AllScreens)
                            if (s.WorkingArea.Contains(winx, winy))
                                screen = s;
                        if (screen == null || height > screen.WorkingArea.Height || width > screen.WorkingArea.Width)
                            valid = false;
                        if (height < this.MinimumSize.Height || width < this.MinimumSize.Width)
                            valid = false;

                        if (valid)
                        {
                            this.Location = new Point(winx, winy);
                            this.Size = new Size(width, height);
                        }
                        else // failed validation
                        {
                            itmOptionsRememberWinPos.Checked = false;
                        }
                    }

                    key.Close();
                }
            }

#if !DEBUG
      }
      catch {}
#endif

            EnableUpdates = true;
        }
        private void SaveSettings()
        {
#if !DEBUG
      try
      {
#endif
            // general

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\RegexRenamer"))
            {
                if (key != null)
                {
                    key.SetValue("LastPath", ActivePath);
                    key.SetValue("MoveCopyPath", fbdMoveCopy.SelectedPath);
                    key.SetValue("RenameFolders", RenameFolders);
                    key.Close();
                }
            }


            // options

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\RegexRenamer\\Options"))
            {
                if (key != null)
                {
                    key.SetValue("ShowHiddenFiles", itmOptionsShowHidden.Checked);
                    key.SetValue("PreserveExtension", itmOptionsPreserveExt.Checked);
                    key.SetValue("RealtimePreview", itmOptionsRealtimePreview.Checked);
                    key.SetValue("AllowRenameIntoSubfolders", itmOptionsAllowRenSub.Checked);
                    key.SetValue("RememberWindowPosition", itmOptionsRememberWinPos.Checked);
                    key.SetValue("OnlyRenameSelectedRows", itmOptionsRenameSelectedRows.Checked);
                    key.Close();
                }
            }


            // stats

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\RegexRenamer\\Stats"))
            {
                if (key != null)
                {
                    key.SetValue("ProgramLaunches", countProgLaunches);
                    key.SetValue("FilesRenamed", countFilesRenamed);
                    key.Close();
                }
            }


            // window position

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\RegexRenamer\\WindowPosition"))
            {
                if (key != null)
                {
                    key.SetValue("WindowX", this.Location.X.ToString()); // store as string to preserve negative values
                    key.SetValue("WindowY", this.Location.Y.ToString());
                    key.SetValue("WindowHeight", this.Height);
                    key.SetValue("WindowWidth", this.Width);
                    key.SetValue("WindowState", this.WindowState);
                    key.Close();
                }
            }

#if !DEBUG
      }
      catch {}
#endif
        }

        private void LoadRegexHistory()
        {
#if !DEBUG
      try
      {
#endif
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\RegexRenamer\\History"))
            {
                if (key == null) return;

                this.cmbMatch.Items.Clear();

                foreach (string name in key.GetValueNames())
                    this.cmbMatch.Items.Add(key.GetValue(name));

                key.Close();
            }
#if !DEBUG
      }
      catch {}
#endif
        }
        private void SaveRegexHistory()
        {
#if !DEBUG
      try
      {
#endif
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\RegexRenamer\\History"))
            {
                if (key == null) return;

                foreach (string name in key.GetValueNames())
                    key.DeleteValue(name);

                for (int i = 0; i < this.cmbMatch.Items.Count; i++)
                    key.SetValue(i.ToString("00"), this.cmbMatch.Items[i]);  // update padding if changing MAX_HISTORY

                key.Close();
            }
#if !DEBUG
      }
      catch {}
#endif
        }


        //// SPLIT CONTAINERS

        //// workaround for SplitContainer bug
        //private void scMain_Resize(object sender, EventArgs e)
        //{
        //    //if( scMain.Width == 0 ) return;  // if minimized

        //    //// when scMain.FixedPanel = Panel1 and the parent resized, scMain.Panel2MinSize is ignored...
        //    //if( scMain.Width < scMain.Panel1.Width + scMain.SplitterWidth + scMain.Panel2MinSize )
        //    //  scMain.SplitterDistance = scMain.Width - scMain.SplitterWidth - scMain.Panel2MinSize;
        //}

        //// draw thin 3d box around scRegex splitter
        //private void scRegex_Paint(object sender, PaintEventArgs e)
        //{
        //    //e.Graphics.DrawLine( SystemPens.ControlLight, 0, 0, scRegex.Width, 0 );
        //    //e.Graphics.DrawLine( SystemPens.ControlDark, 0, scRegex.Height - 1, scRegex.Width, scRegex.Height - 1 );
        //}
        //private void scRegex_Panel1_Paint(object sender, PaintEventArgs e)
        //{
        //    //e.Graphics.DrawLine( SystemPens.ControlLight, scRegex.Panel1.Width - 1, 0, scRegex.Panel1.Width - 1, scRegex.Panel1.Height );
        //}
        //private void scRegex_Panel2_Paint(object sender, PaintEventArgs e)
        //{
        //    //e.Graphics.DrawLine( SystemPens.ControlDark, 0, 0, 0, scRegex.Panel2.Height );
        //}

        //// prevent split containers obtaining focus
        //private void scMain_MouseUp(object sender, MouseEventArgs e)
        //{
        //    UnFocusAll();
        //}
        //private void scRegex_MouseUp(object sender, MouseEventArgs e)
        //{
        //    UnFocusAll();
        //}

        //// restore default size on double-click
        //private void scMain_DoubleClick(object sender, EventArgs e)
        //{
        //    //scMain.SplitterDistance = 300;
        //}
        //private void scRegex_DoubleClick(object sender, EventArgs e)
        //{
        //    //scRegex.SplitterDistance = 348;
        //}
    }
}
