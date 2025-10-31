using Config;
using RegexRenamer.Forms;
using LogEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Utility;

public static class CommanHandlerExtensions
{
    public static async Task<Tuple<int, List<string>, List<string>>> ExecNamedCmdCaptureAsync(this string pThis, string currentFolder, string cmdArgs)
    {
        var cmd = UserConfig.Inst.GetToolCmd(pThis);
        if (cmd == null)
        {
            ErrorLog.Inst.ShowError($"{pThis} command is not configured. Please check the Config\\UserConfig.json.");
            return new Tuple<int, List<string>, List<string>>(-1, new List<string>(), new List<string>());
        }
        string toolPath = UserConfig.Inst.GetToolPath(cmd.ToolName);
        if (string.IsNullOrEmpty(toolPath))
        {
            ErrorLog.Inst.ShowError($"{cmd.ToolName} path is not configured. Please check the Config\\UserConfig.json.");
            return new Tuple<int, List<string>, List<string>>(-1, new List<string>(), new List<string>());
        }
        cmdArgs = cmdArgs.Trim();
        return await toolPath.ExecCmdCaptureAsyncNoUI(cmdArgs, currentFolder);
    }

    public static async Task<Tuple<int, List<string>, List<string>>> ExecNamedCmdCaptureAsync(this string pThis, string currentFolder, List<Tuple<string, string>> templateValues = null )
    {
        var cmd = UserConfig.Inst.GetToolCmd(pThis);
        if (cmd == null)
        {
            ErrorLog.Inst.ShowError($"{pThis} command is not configured. Please check the Config\\UserConfig.json.");
            return new Tuple<int, List<string>, List<string>>(-1,new List<string>(), new List<string>());
        }
        string toolPath = UserConfig.Inst.GetToolPath(cmd.ToolName);
        if (string.IsNullOrEmpty(toolPath))
        {
            ErrorLog.Inst.ShowError($"{cmd.ToolName} path is not configured. Please check the Config\\UserConfig.json.");
            return new Tuple<int, List<string>, List<string>>(-1, new List<string>(), new List<string>());
        }
        var cmdArgs = cmd.Args.BuildCommandline();
        if (templateValues != null && templateValues.Count > 0)
        {
            cmdArgs = cmdArgs.ExpandTemplate(templateValues);
        }
        cmdArgs = cmdArgs.Trim();
        //7za a -tzip -r %userprofile%\Downloads\Defect-125563\archive.zip @%userprofile%\Downloads\Defect-125563\info.txt
        return await toolPath.ExecCmdCaptureAsync(cmdArgs, currentFolder);
    }

    public static async Task<int> ExecNamedCmdAsync(this string pThis, string currentFolder, List<Tuple<string, string>> templateValues = null, bool waitForComplete = false)
    {
        var cmd = UserConfig.Inst.GetToolCmd(pThis);
        if (cmd == null)
        {
            ErrorLog.Inst.ShowError($"{pThis} command is not configured. Please check the Config\\UserConfig.json.");
            return -1;
        }
        string toolPath = UserConfig.Inst.GetToolPath(cmd.ToolName);
        if (string.IsNullOrEmpty(toolPath))
        {
            ErrorLog.Inst.ShowError($"{cmd.ToolName} path is not configured. Please check the Config\\UserConfig.json.");
            return -1;
        }
        var cmdArgs = cmd.Args.BuildCommandline();
        if (templateValues != null && templateValues.Count > 0)
        {
            cmdArgs = cmdArgs.ExpandTemplate(templateValues);
        }
        cmdArgs = cmdArgs.Trim();
        //7za a -tzip -r %userprofile%\Downloads\Defect-125563\archive.zip @%userprofile%\Downloads\Defect-125563\info.txt
        return await toolPath.ExecCmdAsync(cmdArgs, currentFolder, waitForComplete);
    }


    private static async Task<int> ExecCmdAsync(this string command, string arguments, string currentFolder, bool waitForComplete = false)
    {
        // Run command on thread pool to avoid blocking the UI thread
        ManualResetEvent resetEvent = new ManualResetEvent(false);
        var execCmdTask = Task.Run(() =>
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                UseShellExecute = true,
                WorkingDirectory = currentFolder
            };
            int exitCode = waitForComplete ? -1 : 0;
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo = startInfo;
                process.Start();
                if (waitForComplete)
                {
                    process.WaitForExit();
                    exitCode = process.ExitCode;
                }
            }
            resetEvent.Set();
            return exitCode;
        });

        // Wait for 250ms, if task is completed before that don't show progress form
        var result = resetEvent.WaitOne(250);
        if (!result && waitForComplete)
        {
            using (var form = new ProgressForm())
            {
                string commandname = Path.GetFileName(command);
                form.Initialize($"Executing Command: {commandname}", $"Running command: {command} {arguments}");
                form.Show(); // Show the form immediately
                return await execCmdTask; // Wait for the task to complete
            }
        }
        else
        {
            return await execCmdTask; // Wait for the task to complete without showing the form
        }
    }

    private static async Task<Tuple<int, List<string>, List<string>>> ExecCmdCaptureAsyncNoUI(this string command, string arguments, string currentFolder)
    {
        ManualResetEvent resetEvent = new ManualResetEvent(false);
        // Run command on thread pool to avoid blocking the UI thread
        var execCmdTask = Task.Run(() =>
        {
            var output = new List<string>();
            var error = new List<string>();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = currentFolder ?? Path.GetDirectoryName(command)
            };
            int exitCode = -1;
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo = startInfo;
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        output.Add(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        error.Add(e.Data);
                    }
                };
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            resetEvent.Set();
            return new Tuple<int, List<string>, List<string>>(exitCode,output, error);
        });

        // Wait for 250ms, if task is completed before that don't show progress form
        return await execCmdTask;
    }

    private static async Task<Tuple<int,List<string>, List<string>>> ExecCmdCaptureAsync(this string command, string arguments, string currentFolder)
    {
        ManualResetEvent resetEvent = new ManualResetEvent(false);
        // Run command on thread pool to avoid blocking the UI thread
        var execCmdTask = Task.Run(() =>
        {
            var output = new List<string>();
            var error = new List<string>();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = currentFolder ?? Path.GetDirectoryName(command)
            };
            int exitCode = -1;
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo = startInfo;
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        output.Add(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        error.Add(e.Data);
                    }
                };
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            resetEvent.Set();
            return new Tuple<int, List<string>, List<string>>(exitCode,output,error);
        });

        // Wait for 250ms, if task is completed before that don't show progress form
        int countShow = 30;
        int countIdx = 0;
        do
        {
            var result = resetEvent.WaitOne(100);
            if (countIdx > countShow && !result)
            {
                using (var form = new ProgressForm())
                {
                    string commandname = Path.GetFileName(command);
                    form.Initialize($"Executing Command: {commandname}", $"Running command: {command} {arguments}");
                    form.Show(); // Show the form immediately
                    return await execCmdTask; // Wait for the task to complete
                }
            }
            else if(result)
            {
                return await execCmdTask; // Wait for the task to complete without showing the form
            }
            countIdx++;
        } while (true);
        
    }
    // Execute a command and call the callback with the output line by line
    private static async Task<int> ExecCmdCaptureAsync(this string command, string arguments, string curretFolder, Action<string> outputCallback)
    {
        ManualResetEvent resetEvent = new ManualResetEvent(false);
        // Run command on thread pool to avoid blocking the UI thread
        var execCmdTask = Task.Run(() =>
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = curretFolder
            };
            int exitCode = -1;
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo = startInfo;
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        outputCallback(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        outputCallback(e.Data);
                    }
                };
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            resetEvent.Set();
            return exitCode;
        });

        // Wait for 250ms, if task is completed before that don't show progress form
        var result = resetEvent.WaitOne(250);
        if (!result)
        {
            using (var form = new ProgressForm())
            {
                string commandname = Path.GetFileName(command);
                form.Initialize($"Executing Command: {commandname}", $"Running command: {command} {arguments}");
                form.Show(); // Show the form immediately
                return await execCmdTask; // Wait for the task to complete
            }
        }
        else
        {
           return await execCmdTask; // Wait for the task to complete without showing the form
        }
    }
}
