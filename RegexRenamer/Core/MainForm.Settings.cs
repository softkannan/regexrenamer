using ConfigFileParser;
using Microsoft.Win32;
using RegexRenamer.Rename;
using RegexRenamer.Tools.FindReplace;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var configFile = new ConfigFile();

            // general
            var key = configFile.Data["Global"];
            {
                if (key != null)
                {
                    if (_activePath == null)
                        _activePath = (string)key.GetValue("LastPath", "");
                    fbdMoveCopy.SelectedPath = (string)key.GetValue("MoveCopyPath", "");
                    //RenameFolders = (string)key.GetValue("RenameFolders") == "True";

                    try
                    {
                        int maxFilesOverride = (int)key.GetValue("MaxFileLimit", 1000);
                        if (maxFilesOverride > FilesStore.MAX_FILES)
                            FilesStore.MAX_FILES = maxFilesOverride;

                        int maxViewFilesOverride = (int)key.GetValue("MaxViewFileLimit", 200);
                        if (maxViewFilesOverride > MAX_VIEW_PAGE_SIZE)
                            MAX_VIEW_PAGE_SIZE = maxViewFilesOverride;
                    }
                    catch { } // ignore if wrong reg key type
                }
            }

            // options
            key = configFile.Data["Options"];
            {
                if (key != null)
                {
                    EnableUpdates = false;

                    itmOptionsShowHidden.Checked = key.GetValue("ShowHiddenFiles") == "True";
                    itmOptionsPreserveExt.Checked = key.GetValue("PreserveExtension") == "True";
                    itmOptionsRealtimePreview.Checked = key.GetValue("RealtimePreview", "True") == "True";
                    itmOptionsAllowRenSub.Checked = key.GetValue("AllowRenameIntoSubfolders") == "True";
                    itmOptionsRememberWinPos.Checked = key.GetValue("RememberWindowPosition", "True") == "True";
                    //itmOptionsRenameSelectedRows.Checked = key.GetValue("OnlyRenameSelectedRows") == "True";

                    EnableUpdates = true;
                }
            }

            // stats
            key = configFile.Data["Stats"];
            {
                if (key != null)
                {
                    _countProgLaunches = key.GetValue<int>("ProgramLaunches", 0) + 1;
                    _countFilesRenamed = key.GetValue<int>("FilesRenamed", 0);
                }
            }

            // window position
            if (!itmOptionsRememberWinPos.Checked) return;  // skip

            key = configFile.Data["WindowPosition"];
            {
                if (key != null)
                {
                    if ((string)key.GetValue("WindowState") == "Maximized")
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                    else // not maximized
                    {
                        // get size and offset from config
                        int winx = key.GetValue<int>("WindowX", 0); // used to be a dword, now a string
                        int winy = key.GetValue<int>("WindowY", 0);

                        int height = key.GetValue<int>("WindowHeight", -1);
                        int width = key.GetValue<int>("WindowWidth", -1);


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
            var configFile = new ConfigFile();

            // general
            var key = configFile.Data["Global"];
            {
                if (key != null)
                {
                    key.SetValue("LastPath", _activePath);
                    key.SetValue("MoveCopyPath", fbdMoveCopy.SelectedPath);
                    key.SetValue("RenameFolders", RenameFolders);
                }
            }

            // options
            key = configFile.Data["Options"];
            {
                if (key != null)
                {
                    key.SetValue("ShowHiddenFiles", itmOptionsShowHidden.Checked);
                    key.SetValue("PreserveExtension", itmOptionsPreserveExt.Checked);
                    key.SetValue("RealtimePreview", itmOptionsRealtimePreview.Checked);
                    key.SetValue("AllowRenameIntoSubfolders", itmOptionsAllowRenSub.Checked);
                    key.SetValue("RememberWindowPosition", itmOptionsRememberWinPos.Checked);
                    //key.SetValue("OnlyRenameSelectedRows", itmOptionsRenameSelectedRows.Checked);
                }
            }

            // stats
            key = configFile.Data["Stats"];
            {
                if (key != null)
                {
                    key.SetValue("ProgramLaunches", _countProgLaunches);
                    key.SetValue("FilesRenamed", _countFilesRenamed);
                }
            }

            // window position
            key = configFile.Data["WindowPosition"];
            {
                if (key != null)
                {
                    key.SetValue("WindowX", this.Location.X.ToString()); // store as string to preserve negative values
                    key.SetValue("WindowY", this.Location.Y.ToString());
                    key.SetValue("WindowHeight", this.Height);
                    key.SetValue("WindowWidth", this.Width);
                    key.SetValue("WindowState", this.WindowState);
                }
            }

            configFile.Save();

#if !DEBUG
      }
      catch {}
#endif
        }

        private void LoadRegexHistory()
        {
            cmbMatch.LoadFromConfigFile();
            cmbReplace.LoadFromConfigFile();
        }
        private void SaveRegexHistory()
        {
            cmbMatch.SaveToConfigFile();
            cmbReplace.SaveToConfigFile();
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
