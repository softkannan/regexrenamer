using Microsoft.Win32;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Tools.Translate
{
    public class PuppeteerHandler
    {
        private static string GetEdgeExecutablePath()
        {
            // Check default install locations
            string[] possiblePaths = new[]
            {
                @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                @"C:\Program Files\Microsoft\Edge\Application\msedge.exe",
                @"C:\Program Files (x86)\Microsoft\Edge Dev\Application\msedge.exe",
                @"C:\Program Files\Microsoft\Edge Dev\Application\msedge.exe"
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                    return path;
            }

            // Optionally, check registry for custom install location
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\msedge.exe"))
            {
                var regPath = key?.GetValue("") as string;
                if (!string.IsNullOrEmpty(regPath) && File.Exists(regPath))
                    return regPath;
            }

            return null; // Not found
        }


        public async Task LaunchPuppeteer(string url)
        {
            // Get Edge browser executable path
            var edgePath = GetEdgeExecutablePath();
            if (string.IsNullOrEmpty(edgePath))
            {
                MessageBox.Show("Microsoft Edge browser not found.");
                return;
            }

            var appName = Assembly.GetEntryAssembly().GetName().Name;
            var tempFolder = Path.Combine(Path.GetTempPath(), appName, "PuppeteeSharp");
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            var browserOptions = new LaunchOptions
            {
                Args = new[] { "--disable-web-security", url },
                UserDataDir = tempFolder,
                Headless = false,
                ExecutablePath = edgePath
            };

            var browser = await Puppeteer.LaunchAsync(browserOptions);

            ManualResetEvent completedEvent = new ManualResetEvent(false);

            browser.Closed += (sender, e) =>
            {
                completedEvent.Set();
                // Handle browser closed event if needed
            };
            //var pages = await browser.PagesAsync();
            //var page = pages.FirstOrDefault();
            //await page.SetContentAsync("<div>Testing PuppeteerSharp with Edge!</div>");
            //var page = await browser.NewPageAsync();
            //await page.SetContentAsync("<div>Testing</div>");

            //await Task.CompletedTask;
            await Task.Run(() => completedEvent.WaitOne());
        }
    }
}
