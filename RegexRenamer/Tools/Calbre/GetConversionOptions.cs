using RegexRenamer.Tools.Calbre;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Forms
{
    public partial class GetConversionOptions : Form
    {
        public GetConversionOptions()
        {
            InitializeComponent();

            bttnOk.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
        }

        public GetConversionOptions(CalibreOptions options) : this() {
            pgConversionOptions.SelectedObject = options;
        }

        public static DialogResult GetInputeOption(CalibreOptions options)
        {
            DialogResult result = DialogResult.Cancel;
            using(var form = new GetConversionOptions(options))
            {
                result = form.ShowDialog();
            }
            return result;
        }
    }
}
