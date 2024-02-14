using Interop.Shell32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static PInvoke.NativeShell32;

namespace RegexRenamer.Native
{
    public static class TreeViewShell32Extensions
    {
        private const string      DUMMYNODE = "DUMMYNODE";
        private static ShellClass shell32   = new ShellClass();
        #region Public methods

        public static string RenameFolder(this TreeNode node, string newLabel)
        {
            var ret = "";
            var folderItem = node.Tag as FolderItem;
            if (folderItem != null)
            {
                var parentDir = Directory.GetParent(folderItem.Path);
                var newPath = $"{parentDir}\\{newLabel}";
                PInvoke.NativeShell32.RenameFolder(folderItem.Path, newPath);
                ret = newPath;
            }
            return ret;
        }

        public static string TreeNodeToPath(this TreeNode node)
        {
            var folderItem = node.Tag as FolderItem;
            var ret = "";
            if (folderItem != null)
            {
                ret = folderItem.Path;
                ret ??= "";
            }
            return ret;
        }

        public static string GetOnlyDirectory(this TreeNode tn)
        {
            try
            {
                FolderItem folderItem = (FolderItem)tn.Tag;
                string folderPath = folderItem.Path;
                if (Directory.Exists(folderPath))
                    return folderPath;
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }

        public static FolderItem GetShellFolderItem(this string path, string fileName)
        {
            Folder folder = GetFolder(path);
            var ret = folder.ParseName(fileName);
            return ret;
        }

        public static void PopulateTree(this TreeView tree, ImageList imageList)
        {
            int imageCount = imageList.Images.Count - 1;
            tree.Nodes.Clear();
            AddRootNode(tree, ref imageCount, imageList, SHShellFolder.Desktop, true);
            if (tree.Nodes.Count > 1)
            {
                tree.SelectedNode = tree.Nodes[1];
                tree.Nodes[1].ExpandTreeNode(imageList);
            }
        }

        public static void ExpandTreeNode(this TreeNode tn, ImageList imageList)
        {
            // if there's a dummy node present, clear it and replace with actual contents
            if (tn.Nodes.Count == 1 && tn.Nodes[0].Tag.ToString() == DUMMYNODE)
            {
                tn.Nodes.Clear();
                FolderItem folderItem = (FolderItem)tn.Tag;
                Folder folder = (Folder)folderItem.GetFolder;
                int imageCount = imageList.Images.Count - 1;
                foreach (FolderItem item in folder.Items())
                {
                    //if (item.IsFileSystem && item.IsFolder && !item.IsBrowsable)
                    if (item.IsFolder && !item.IsBrowsable)
                    {
                        TreeNode ntn = AddTreeNode(item, ref imageCount, imageList, true);
                        tn.Nodes.Add(ntn);
                        AddSubFolderNodes(ntn, imageList);
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private static Folder GetFolder(SHShellFolder topFolder)
        {
            return (Folder) shell32.NameSpace(topFolder);
        }
        private static Folder GetFolder(string path)
        {
            return (Folder) shell32.NameSpace(path);
        }
        private static void AddRootNode(TreeView tree, ref int imageCount, ImageList imageList, SHShellFolder rootFolder, bool getIcons)
        {
            Folder shell32RootFolder = GetFolder(rootFolder);
            FolderItems rootItems = shell32RootFolder.Items();

            TreeNode rootNode = new TreeNode(rootFolder.ToString(), 0, 0);

            // Added in version 1.11
            // add a FolderItem object to the root (Desktop) node tag that corresponds to the DesktopDirectory namespace
            // This ensures that the GetSelectedNodePath will return the actual Desktop folder path when queried.
            // There's possibly a better way to create a Shell32.FolderItem instance for this purpose, 
            // but I surely don't know it

            Folder dfolder = GetFolder(SHShellFolder.DesktopDirectory);
            foreach (FolderItem fi in dfolder.ParentFolder.Items())
            {
                if (fi.Name == dfolder.Title)
                {
                    rootNode.Tag = fi;
                    break;
                }
            }

            // Add the Desktop root node to the tree
            tree.Nodes.Add(rootNode);

            // Get FolderItem that represents Recycle Bin
            Folder recFolder = GetFolder(SHShellFolder.RecycleBin);
            FolderItem recycle = null;
            foreach (FolderItem fi in recFolder.ParentFolder.Items())
            {
                if (fi.Name == recFolder.Title)
                {
                    recycle = fi;
                    break;
                }
            }

            // Get FolderItem that represents My Network Places
            Folder netFolder = GetFolder(SHShellFolder.NetworkNeighborhood);
            FolderItem mynetwork = null;
            foreach (FolderItem fi in netFolder.ParentFolder.Items())
            {
                if (fi.Name == netFolder.Title)
                {
                    mynetwork = fi;
                    break;
                }
            }

            // iterate through the Desktop namespace and populate the first level nodes
            foreach (FolderItem item in rootItems)
            {
                if (!item.IsFolder) continue;  // this ensures that desktop shortcuts etc are not displayed
                if (item.IsBrowsable) continue;  // exclude zip files
                if (recycle != null && item.Path == recycle.Path) continue;  //  skip recycle bin

                TreeNode tn = AddTreeNode(item, ref imageCount, imageList, getIcons);
                rootNode.Nodes.Add(tn);

                Debug.WriteLine(item.Path);

                if (mynetwork != null && item.Path == mynetwork.Path)  // skip my network places subdirs
                {
                    tree.Tag = tn;  // store node in tag and skip sub folder evaluation
                    continue;
                }

                AddSubFolderNodes(tn, imageList);
            }
        }

        private static void FillSubDirectories(TreeNode tn, ref int imageCount, ImageList imageList, bool getIcons)
        {
            FolderItem folderItem = (FolderItem)tn.Tag;
            Folder folder = (Folder)folderItem.GetFolder;

            foreach (FolderItem item in folder.Items())
            {
                //if (item.IsFileSystem && item.IsFolder && !item.IsBrowsable)
                if (item.IsFolder && !item.IsBrowsable)
                {
                    TreeNode ntn = AddTreeNode(item, ref imageCount, imageList, getIcons);
                    tn.Nodes.Add(ntn);
                    AddSubFolderNodes(ntn, imageList);
                }
            }
        }

        private static void AddSubFolderNodes(TreeNode tn, ImageList imageList)
        {
            if (tn.Nodes.Count == 0)
            {
                try
                {
                    // create dummy nodes for any subfolders that have further subfolders
                    FolderItem folderItem = (FolderItem)tn.Tag;
                    Folder folder = (Folder)folderItem.GetFolder;

                    bool hasFolders = false;
                    foreach (FolderItem item in folder.Items())
                    {
                        if (item.IsFolder && !item.IsBrowsable)
                        {
                            hasFolders = true;
                            break;
                        }
                    }
                    if (hasFolders)
                    {
                        TreeNode ntn = new TreeNode();
                        ntn.Tag = DUMMYNODE;
                        tn.Nodes.Add(ntn);
                    }
                }
                catch { }
            }
        }

        private static TreeNode AddTreeNode(FolderItem item, ref int imageCount, ImageList imageList, bool getIcons)
        {
            TreeNode tn = new TreeNode();
            tn.Text = item.Name;
            tn.Tag = item;

            if (getIcons)
            {
                try
                {
                    imageCount++;
                    tn.ImageIndex = imageCount;
                    imageCount++;
                    tn.SelectedImageIndex = imageCount;
                    imageList.Images.Add(TreeviewExtractIcons.GetIcon(item.Path, false)); // normal icon
                    imageList.Images.Add(TreeviewExtractIcons.GetIcon(item.Path, true)); // selected icon
                }
                catch // use default 
                {
                    tn.ImageIndex = 1;
                    tn.SelectedImageIndex = 2;
                }
            }
            else // use default
            {
                tn.ImageIndex = 1;
                tn.SelectedImageIndex = 2;
            }
            return tn;
        }

        #endregion

    }
}
