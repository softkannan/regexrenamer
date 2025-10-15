using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogEx;

internal class ErrorLog
{
    private static ErrorLog _inst = new ErrorLog();
    public static ErrorLog Inst
    {
        get
        {
            return _inst;
        }
    }

    public static void Initialize(ListBox listBox)
    {
        _inst = new ErrorLog(listBox);
    }

    public void WriteToFile(string filePath)
    {
        if (_listBox.Items.Count == 0)
        {
            ShowError("No status to dump to file.");
            return;
        }
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var item in _listBox.Items)
                {
                    writer.WriteLine(item.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ShowError($"Failed to write status to file: {filePath}. Error: {ex.Message}");
        }
    }

    private ListBox _listBox;

    public ErrorLog(ListBox listBox = null)
    {
        _listBox = listBox;
        if (_listBox != null)
        {
            _listBox.Items.Clear();
        }
        _addStatusTypePtr = LogStatus;
    }

    private delegate void AddStatusType(ListBox listBox, string message);
    private readonly AddStatusType _addStatusTypePtr;
    private void LogStatus(ListBox listBox, string message)
    {
        if (listBox != null)
        {
            listBox.Items.Add(message);
        }
    }

    private void LogStatus(string message)
    {
        if (_listBox != null)
        {
            _listBox.Items.Add(message);
        }
    }

    public void Log(string format, params object[] args)
    {
        string message = string.Format(format, args);
        LogStatus(message);
    }

    public void Log(string message)
    {
        LogStatus(message);
    }

    public void Log(IEnumerable<string> lines)
    {
        if (_listBox != null)
        {
            _listBox.Items.Add(lines);
        }
    }

    public void LogAsync(string format, params object[] args)
    {
       string message = string.Format(format, args);
       _listBox?.BeginInvoke(_addStatusTypePtr, _listBox, message);
    }

    public void LogAsync(string message)
    {
        _listBox?.BeginInvoke(_addStatusTypePtr, _listBox, message);
    }

    public void ShowError(string format, params object[] args)
    {
        string message = string.Format(format, args);
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public void ShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public void LogShowError(string format, params object[] args)
    {
        string message = string.Format(format, args);
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        LogStatus(message);
    }

    public void LogShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        LogStatus(message);
    }

    public void ShowInfo(string format, params object[] args)
    {
        string message = string.Format(format, args);
        MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void ShowInfo(string message)
    {
        MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
