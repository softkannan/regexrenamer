using LogEx;
using Kavita;
using RegexRenamer.Tools.EBookPDFTools;
using RegexRenamer.Utility;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.Calbre;

public class CalibreHelper
{
    public static async Task<bool> PolishEbook(string filePath)
    {
        // Supports {srcfilepath} {destfilepath} variables
        var cmdName = "PolishEPUB";

        string tempFilePath = filePath.GetInFolderTempFilePath();

        var (exitcode, output, error) = await cmdName.ExecNamedCmdCaptureAsync(null, new List<Tuple<string, string>>(){
           new Tuple<string, string>("{srcfilepath}", filePath),
           new Tuple<string, string>("{destfilepath}", tempFilePath)
        });

        //create unique list of files from output
        if (output.Count == 0 || exitcode != 0 || error.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            if (output != null)
            {
                foreach (var line in output)
                {
                    sb.AppendLine(line);
                }
            }
            if (error != null)
            {
                foreach (var line in error)
                {
                    sb.AppendLine(line);
                }
            }

            throw new ApplicationException($"Failed to polish file : {filePath}\r\n{sb.ToString()}");
        }

        // if success then make backup of original file
        filePath.MakeBackup();

        // if success move temp file to original file
        File.Move(tempFilePath, filePath, true);
        return true;
    }
    public static async Task<bool> ClearMetadata(string srcFilePath)
    {
        // Supports {srcfilepath} {destfilepath} variables
        var cmdName = "ClearEbookMetaData";
        var (exitcode, output, error) = await cmdName.ExecNamedCmdCaptureAsync(null, new List<Tuple<string, string>>(){
       new Tuple<string, string>("{filepath}", srcFilePath)
    });
        //create unique list of files from output
        if (output.Count == 0 || exitcode != 0 || error.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            if (output != null)
            {
                foreach (var line in output)
                {
                    sb.AppendLine(line);
                }
            }
            if (error != null)
            {
                foreach (var line in error)
                {
                    sb.AppendLine(line);
                }
            }
            throw new ApplicationException($"Failed to clear metadata file : {srcFilePath}\r\n{sb.ToString()}");
        }
        return true;
    }


    public static async Task<bool> WriteMetadata(string filePath, ComicInfo metadata)
    {
        // Supports {author} {series} {title} {volume} {filepath} variables
        var cmdName = "WriteEbookMetaData";
        //"-a \"{author}\" -s \"{series}\" -t \"{title}\" -i \"{volume}\" \"{filepath}\""

        var cmdArgs = new StringBuilder();
        bool needToRun = false;
        if (!string.IsNullOrEmpty(metadata.Writer))
        {
            cmdArgs.Append($"--authors=\"{metadata.Writer.Trim()}\" ");
            needToRun = true;
        }
        if (!string.IsNullOrEmpty(metadata.Series))
        {
            cmdArgs.Append($"--series=\"{metadata.Series.Trim()}\" ");
            cmdArgs.Append($"--index=\"{metadata.Volume.Trim()}\" ");
            needToRun = true;
        }

        if (!string.IsNullOrEmpty(metadata.Title))
        {
            cmdArgs.Append($"--title=\"{metadata.Title.Trim()}\" ");
            needToRun = true;
        }

        if(!string.IsNullOrEmpty(metadata.LanguageISO))
        {
            cmdArgs.Append($"--language=\"{metadata.LanguageISO.ToLower().Trim()}\" ");
            needToRun=true;
        }

        if (!needToRun)
        {
            return true;
        }

        cmdArgs.Append($"\"{filePath}\"");

        var (exitcode, output, error) = await cmdName.ExecNamedCmdCaptureAsync(null, cmdArgs.ToString());
        //create unique list of files from output
        if (output.Count == 0 || exitcode != 0 || error.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            if (output != null)
            {
                foreach (var line in output)
                {
                    sb.AppendLine(line);
                }
            }
            if (error != null)
            {
                foreach (var line in error)
                {
                    sb.AppendLine(line);
                }
            }
            throw new ApplicationException($"Failed to write metata data for : {filePath}\r\n{sb.ToString()}");
        }
        return true;
    }

    public static async Task<bool> ConvertCalibre(string srcFilePath, string destFilePath)
    {
        var extraoptions = new StringBuilder();
        string inExt = Path.GetExtension(srcFilePath).ToLowerInvariant();
        string outExt = Path.GetExtension(destFilePath).ToLowerInvariant();
        if (outExt != ".epub" && outExt != ".pdf")
        {
            ErrorLog.Inst.ShowError("Calibre conversion only supports epub and pdf output");
            return false;
        }
        // Supports {srcfilepath} {destfilepath} variables
        var cmdName = "ConvertBookToEPUB";

        switch(outExt)
        {
            case ".pdf":
                cmdName = "ConvertBookToPDF";
                break;
            case "":
                cmdName = "ConvertBookToEPUB";
                break;
        }

        switch (inExt)
        {
            case ".txt":
                {
                    //var txtFileEncoding = srcFilePath.GetEncoding();
                    //extraoptions.Append($"--input-encoding={txtFileEncoding.Item1}");
                }
                break;
        }

        // Supports {srcfilepath} {destfilepath} variables , {extraoptions} optional and it must be at the end
        var (exitcode, output, error) = await cmdName.ExecNamedCmdCaptureAsync(null, new List<Tuple<string, string>>(){
       new Tuple<string, string>("{srcfilepath}", srcFilePath),
       new Tuple<string, string>("{destfilepath}", destFilePath),
       new Tuple<string, string>("{extraoptions}", extraoptions.ToString()),
    });
        //create unique list of files from output
        if (output.Count == 0 || exitcode != 0 || error.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            if (output != null)
            {
                foreach (var line in output)
                {
                    sb.AppendLine(line);
                }
            }
            if (error != null)
            {
                foreach (var line in error)
                {
                    sb.AppendLine(line);
                }
            }
            throw new ApplicationException($"Failed to convert file : {srcFilePath}\r\n{sb.ToString()}");
        }
        return true;
    }

}
