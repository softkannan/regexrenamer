using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Windows.Devices.AllJoyn;

namespace RegexRenamer.Forms
{
    public partial class MessageBoxForm : Form
    {
        public MessageBoxForm()
        {
            InitializeComponent();
        }

        private string _bttnClicked = null;
        private Dictionary<string, bool> _chkStates = new Dictionary<string, bool>();

        public static Tuple<string, Dictionary<string, bool>> ShowMessage(string message, string title = "Message", List<Tuple<string, string>> buttons = null,  MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            using (var form = new MessageBoxForm())
            {
                form.Text = title;
                form.lblMsg.Text = message;
                //form.pictBox.Image = icon switch
                //{
                //    MessageBoxIcon.Information => SystemIcons.Information.ToBitmap(),
                //    MessageBoxIcon.Warning => SystemIcons.Warning.ToBitmap(),
                //    MessageBoxIcon.Error => SystemIcons.Error.ToBitmap(),
                //    MessageBoxIcon.Question => SystemIcons.Question.ToBitmap(),
                //    _ => form.Icon.ToBitmap(),
                //};
                form.Icon = icon switch
                {
                    MessageBoxIcon.Information => SystemIcons.Information,
                    MessageBoxIcon.Warning => SystemIcons.Warning,
                    MessageBoxIcon.Error => SystemIcons.Error,
                    MessageBoxIcon.Question => SystemIcons.Question,
                    _ => form.Icon,
                };

                // Calculate the gap between the bottom of the label and the top of the FlowLayoutPanel
                var gap = form.flowLayoutPanelBttns.Top - (form.lblMsg.Top + form.lblMsg.Height);
                form.AddButtons(buttons ?? new List<Tuple<string, string>> { new Tuple<string, string>("default", "Ok") });
                form.ResizeFormToFitContent(gap);
                var result = form.ShowDialog();
                return Tuple.Create(form._bttnClicked, form._chkStates);
            }
        }

        private void ResizeFormToFitContent(int gap)
        {
            this.SuspendLayout();
            var maxScreenWidth = (Screen.PrimaryScreen.WorkingArea.Width / 4) * 3;
            var maxScreenHeight = (Screen.PrimaryScreen.WorkingArea.Height / 4) * 3;
            // Set the maximum size for the label and FlowLayoutPanel
            var lblWidth = Math.Min(lblMsg.PreferredSize.Width, maxScreenWidth);
            var lblHeight = Math.Min(lblMsg.PreferredSize.Height, maxScreenHeight);
            var flowWidth = Math.Min(flowLayoutPanelBttns.PreferredSize.Width, maxScreenWidth);
            var flowHeight = Math.Min(flowLayoutPanelBttns.PreferredSize.Height, maxScreenHeight);
            // Set the size of the label and FlowLayoutPanel
            lblMsg.Height = lblHeight;
            lblMsg.Width = lblWidth;
            flowLayoutPanelBttns.Height = flowHeight;
            flowLayoutPanelBttns.Width = flowWidth;
            // Set the size of the form based on the sizes of the label and FlowLayoutPanel
            this.Height = lblHeight + flowHeight + gap + 50; // Add some padding
            this.Width = Math.Max(lblWidth, flowWidth) + 50; // Add some padding
            this.ResumeLayout(true);
        }

        private void AddButtons(List<Tuple<string, string>> buttons)
        {
            this.SuspendLayout();
            buttons.Reverse(); // Reverse the order of buttons to maintain the original order when added to the FlowLayoutPanel
            foreach (var kvp in buttons)
            {
                var type = kvp.Item1;
                var button = kvp.Item2;
                if(string.IsNullOrEmpty(type) || string.Compare(type, "default", true) == 0 || string.Compare(type, "bttn", true) == 0)
                {
                    var bttnItem = new Button();
                    //bttnItem.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
                    bttnItem.AutoSize = true;
                    bttnItem.Name = $"bttn{button}";
                    bttnItem.Text = button;
                    bttnItem.UseVisualStyleBackColor = true;
                    bttnItem.Click += (s, e) => { _bttnClicked = button; this.Close(); };
                    flowLayoutPanelBttns.Controls.Add(bttnItem);
                }
                else if (string.Compare(type, "check", true) == 0)
                {
                    var chkItem = new CheckBox();
                    chkItem.AutoSize = true;
                    chkItem.Name = $"chk{button}";
                    chkItem.Text = button;
                    chkItem.CheckedChanged += (s, e) => { _chkStates[button] = chkItem.Checked; };
                    flowLayoutPanelBttns.Controls.Add(chkItem);
                }
                
            }
            this.ResumeLayout(true);
        }
    }
}
