using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PInvoke;
using static System.Net.Mime.MediaTypeNames;
using RegexRenamer.Rename;

namespace RegexRenamer.Native
{
    public static class ClipboardExtensions
    {

        public static void CopyFilesToClipboad(this string pThis, bool move = false)
        {
            var dropEffect = move ? DragDropEffects.Move : DragDropEffects.Copy;

            var droplist = new StringCollection();
            droplist.Add(pThis);

            var fileText = pThis;

            var data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData( ClipboardAPI.ShellClipboardFormat.CFSTR_PREFERREDDROPEFFECT, new MemoryStream(BitConverter.GetBytes((int)dropEffect)));
            data.SetText(fileText, TextDataFormat.UnicodeText);
            Clipboard.SetDataObject(data);
        }
        public static void CopyNamesToClipboad(this List<RenameItemInfo> pThis)
        {
            if (pThis.Count == 0) return;
            var fileText = pThis.Select(x => x.Name).Aggregate((total, next) => total + "\r\n" + next);
            var data = new DataObject();
            data.SetText(fileText, TextDataFormat.UnicodeText);
            Clipboard.SetDataObject(data);
        }

        public static List<string> GetNamesFromClipboard()
        {
            DataObject data = Clipboard.GetDataObject() as DataObject;
            var text = data?.GetText(TextDataFormat.UnicodeText) ?? string.Empty;
            List<string> names = new List<string>();
            using (var reader = new StringReader(text))
            {
                do
                {
                    var line = reader.ReadLine();
                    if(line  ==  null)
                        break;

                    line = line.Trim();
                    if (string.IsNullOrEmpty(line))
                        continue;
                    names.Add(line);
                } while (true);
            }
            return names;
        }

        public static void CopyFilesPathToClipboad(this List<RenameItemInfo> pThis, bool move = false)
        {
            if (pThis.Count == 0) return;
            var fileText = pThis.Select(x => x.Fullpath).Aggregate((total, next) => total + "\r\n" + next);

            Clipboard.SetText(fileText);
        }

        public static void CopyFilesToClipboad(this List<RenameItemInfo> pThis, bool move = false)
        {
            if (pThis.Count == 0) return;
            var dropEffect = move ? DragDropEffects.Move : DragDropEffects.Copy;

            var droplist = new StringCollection();
            droplist.AddRange(pThis.Select(x => x.Fullpath).ToArray());

            var fileText = pThis.Select(x => x.Context.Preview ).Aggregate((total, next) => total + ";" + next );

            var data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData(ClipboardAPI.ShellClipboardFormat.CFSTR_PREFERREDDROPEFFECT, new MemoryStream(BitConverter.GetBytes((int)dropEffect)));
            data.SetText(fileText, TextDataFormat.UnicodeText);
            Clipboard.SetDataObject(data);
        }
        public static void ClipboardPasteFiles(this string pastePath)
        {
            var data = Clipboard.GetDataObject() as DataObject;
            if(data == null) return;
            bool isMove = false;
            if (data.TryGetData<MemoryStream>(ClipboardAPI.ShellClipboardFormat.CFSTR_PREFERREDDROPEFFECT, out var dropEffectStream) && dropEffectStream != null)
            {
                int dropEffect = dropEffectStream.ReadByte();
                // Checks if the 'Copy' flag (1) is present within the value 5
                if ((dropEffect & 1) != 0)
                {
                    // Handle Copy
                    isMove = false;
                }
                if ((dropEffect & 2) != 0)
                {
                    // Handle Move
                    isMove = true;
                }
                // Checks if the 'Link' flag (4) is present within the value 5
                if ((dropEffect & 4) != 0)
                {
                    // Handle Link
                }
            }
            List<string> files = new List<string>();
            foreach(var item in data.GetFileDropList())
            {
                files.Add(item!.ToString());
            }

            PInvoke.FileOperationAPI.MoveFiles(files, pastePath,isMove);
        }

        public static void PutFilesOnClipboard(this IEnumerable<FileSystemInfo> filesAndFolders, bool moveFilesOnPaste = false)
        {
            var dropEffect = moveFilesOnPaste ? DragDropEffects.Move : DragDropEffects.Copy;

            var droplist = new StringCollection();
            droplist.AddRange(filesAndFolders.Select(x => x.FullName).ToArray());

            var data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData(ClipboardAPI.ShellClipboardFormat.CFSTR_PREFERREDDROPEFFECT, new MemoryStream(BitConverter.GetBytes((int)dropEffect)));
            Clipboard.SetDataObject(data);
        }

        public static void ItemsToClipboard(StringCollection items, bool isCut)
        {
            DataObject data = new DataObject();
            data.SetFileDropList(items);
            if (isCut)
                data.SetData(ClipboardAPI.ShellClipboardFormat.CFSTR_PREFERREDDROPEFFECT, DragDropEffects.Move);
            else
                data.SetData(ClipboardAPI.ShellClipboardFormat.CFSTR_PREFERREDDROPEFFECT, DragDropEffects.Copy);
            Clipboard.Clear();
            Clipboard.SetDataObject(data);
        }
    }
}
