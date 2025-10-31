using Config;
using EpubSharp;
using Microsoft.Web.WebView2.Core;
using Microsoft.Win32;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Tools.Translate
{
    public partial class GoogleTranslatorForm : Form
    {
        public GoogleTranslatorForm()
        {
            InitializeComponent();

            this.Load += GoogleTranslatorForm_Load;

            webView.NavigationCompleted += WebView_NavigationCompleted;


        }


       

        

        private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                //string encodedHtml = StringUtils.ToJsonString(html);
                //string script = "window.document.write(" + encodedHtml + ")";

                //try
                //{
                //    await WebView.ExecuteScriptAsync(script);
                //}
                //catch (Exception ex)
                //{
                //    this.LastException = ex;
                //}
            }
        }

        private async void GoogleTranslatorForm_Load(object sender, EventArgs e)
        {
            var appName = Assembly.GetEntryAssembly().GetName().Name;
            var tempFolder = Path.Combine(Path.GetTempPath(), appName, "WebView2");
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }
            var environment = await CoreWebView2Environment.CreateAsync(userDataFolder: "tempFolder");
            await webView.EnsureCoreWebView2Async(environment);

            foreach (var item in UserConfig.Inst.Translator.ServiceProviders)
            {
                cmbServiceProvider.Items.Add(item);
            }
            cmbServiceProvider.SelectedIndex = UserConfig.Inst.Translator.SelectedProviderIndex;
            cmbServiceProvider.SelectedIndexChanged += (s, e) =>
            {
                var selectedProvider = cmbServiceProvider.SelectedItem as TranslatorProvider;
                if (selectedProvider == null)
                {
                    return;
                }

                UserConfig.Inst.Translator.SelectedProviderIndex = cmbServiceProvider.SelectedIndex;
                webView.Source = new Uri(selectedProvider.Url);
            };

            var selectedProvider = cmbServiceProvider.SelectedItem as TranslatorProvider;
            if (selectedProvider == null)
            {
                return;
            }
            webView.Source = new Uri(selectedProvider.Url);
        }
    }
}
