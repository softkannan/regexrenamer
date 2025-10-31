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
            var fileText = pThis.Select(x => x.Name).Aggregate((total, next) => total + "\r\n" + next);
            var data = new DataObject();
            data.SetText(fileText, TextDataFormat.UnicodeText);
            Clipboard.SetDataObject(data);
        }

        public static List<string> GetNamesFromClipboard()
        {
            DataObject data = Clipboard.GetDataObject() as DataObject;
            var text = data.GetText(TextDataFormat.UnicodeText);
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

        public static void CopyFilesToClipboad(this List<RenameItemInfo> pThis, bool move = false)
        {
            var dropEffect = move ? DragDropEffects.Move : DragDropEffects.Copy;

            var droplist = new StringCollection();
            droplist.AddRange(pThis.Select(x => x.Fullpath).ToArray());

            var fileText = pThis.Select(x => x.Preview ).Aggregate((total, next) => total + ";" + next );

            var data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData(ClipboardAPI.ShellClipboardFormat.CFSTR_PREFERREDDROPEFFECT, new MemoryStream(BitConverter.GetBytes((int)dropEffect)));
            data.SetText(fileText, TextDataFormat.UnicodeText);
            Clipboard.SetDataObject(data);
        }
        public static void ClipboardPasteFiles(this string pastePath)
        {
            DataObject data = Clipboard.GetDataObject() as DataObject;
            var obj = data.GetData(ClipboardAPI.ShellClipboardFormat.CFSTR_PREFERREDDROPEFFECT);
            bool isMove = false;
            if (obj != null)
            {
                if (obj is MemoryStream) //from Windows
                {
                    var m = (obj as MemoryStream).ToArray();
                    isMove = m[0] == 2;
                }
                else
                {
                    isMove = obj.ToString() == "Move";
                }
            }

            List<string> files = new List<string>();

            foreach(var item in data.GetFileDropList())
            {
                files.Add(item.ToString());
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
