using RegexRenamer.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Tools.TxtFiles;

public class ReFormatTextFiles
{
    private string _srcFilePath;
    private string _dstFilePath;
    public ReFormatTextFiles(string srcFilePath) { 
    
        _srcFilePath = srcFilePath;
        var srcFileFolder = Path.GetDirectoryName(srcFilePath);
        var fileName = Path.GetFileName(srcFilePath);
        _dstFilePath = Path.Combine(srcFileFolder, "Cleaned",fileName);
        var destFolder = Path.GetDirectoryName(_dstFilePath);
        if(!Directory.Exists(destFolder))
        {
            Directory.CreateDirectory(destFolder);
        }
    }

    public Tuple<FormWindowState, DialogResult> Process(FormWindowState windowState)
    {
        FormWindowState retVal = windowState;
        var result = DialogResult.Cancel;
        var allLines = File.ReadAllText(_srcFilePath, Encoding.UTF8);
        using (var handleForm = new TextReplaceForm(allLines))
        {
            handleForm.WindowState = windowState;
            result = handleForm.ShowDialog();
            retVal = handleForm.WindowState;
            if (result == DialogResult.OK)
            {
                var newText = handleForm.ResultText;
                try
                {
                    if (File.Exists(_dstFilePath))
                    {
                        File.Delete(_dstFilePath);
                    }
                }
                catch { }
                File.WriteAllText(_dstFilePath, newText, Encoding.UTF8);
            }
        }
        return Tuple.Create(retVal, result);
    }

}
