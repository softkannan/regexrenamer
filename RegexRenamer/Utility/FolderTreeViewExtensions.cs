using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Utility
{
    public static class FolderTreeViewExtensions
    {
        #region Public methods

        public static string GetFilePath(this TreeNode tn)
        {
            try
            {
                Shell32.FolderItem folderItem = (Shell32.FolderItem)tn.Tag;
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
        // [xiperware]
        public static string ForceGetFilePath(this TreeNode tn)
        {
            try
            {
                Shell32.FolderItem folderItem = (Shell32.FolderItem)tn.Tag;
                return folderItem.Path;
            }
            catch
            {
                return "";
            }
        }


        public static void PopulateTree(this TreeView tree, ImageList imageList)
        {
            int imageCount = imageList.Images.Count - 1;
            tree.Nodes.Clear();
            AddRootNode(tree, ref imageCount, imageList, ShellFolder.Desktop, true);
            if (tree.Nodes.Count > 1)
            {
                tree.SelectedNode = tree.Nodes[1];
                tree.Nodes[1].ExpandBranch(imageList);
            }
        }

        public static void ExpandBranch(this TreeNode tn, ImageList imageList)
        {
            // if there's a dummy node present, clear it and replace with actual contents
            if (tn.Nodes.Count == 1 && tn.Nodes[0].Tag.ToString() == "DUMMYNODE")
            {
                tn.Nodes.Clear();
                Shell32.FolderItem folderItem = (Shell32.FolderItem)tn.Tag;
                Shell32.Folder folder = (Shell32.Folder)folderItem.GetFolder;
                int imageCount = imageList.Images.Count - 1;
                foreach (Shell32.FolderItem item in folder.Items())
                {
                    //if (item.IsFileSystem && item.IsFolder && !item.IsBrowsable)
                    if (item.IsFolder && !item.IsBrowsable)
                    {
                        TreeNode ntn = AddTreeNode(item, ref imageCount, imageList, true);
                        tn.Nodes.Add(ntn);
                        CheckForSubDirs(ntn, imageList);
                    }
                }
            }
        }
        #endregion

        #region Private Methods

        private static void AddRootNode(TreeView tree, ref int imageCount, ImageList imageList, ShellFolder shellFolder, bool getIcons)
        {
            Shell32.Shell shell32 = new Shell32.ShellClass();
            Shell32.Folder shell32Folder = shell32.NameSpace(shellFolder);
            Shell32.FolderItems items = shell32Folder.Items();

            tree.Nodes.Clear();
            TreeNode desktop = new TreeNode("Desktop", 0, 0);

            // Added in version 1.11
            // add a FolderItem object to the root (Desktop) node tag that corresponds to the DesktopDirectory namespace
            // This ensures that the GetSelectedNodePath will return the actual Desktop folder path when queried.
            // There's possibly a better way to create a Shell32.FolderItem instance for this purpose, 
            // but I surely don't know it

            Shell32.Folder dfolder = shell32.NameSpace(ShellFolder.DesktopDirectory);
            foreach (Shell32.FolderItem fi in dfolder.ParentFolder.Items())
            {
                if (fi.Name == dfolder.Title)
                {
                    desktop.Tag = fi;
                    break;
                }
            }

            // Add the Desktop root node to the tree
            tree.Nodes.Add(desktop);

            // [xiperware] Get FolderItem that represents Recycle Bin
            Shell32.Folder recFolder = shell32.NameSpace(ShellFolder.RecycleBin);
            Shell32.FolderItem recycle = null;
            foreach (Shell32.FolderItem fi in recFolder.ParentFolder.Items())
            {
                if (fi.Name == recFolder.Title)
                {
                    recycle = fi;
                    break;
                }
            }

            // [xiperware] Get FolderItem that represents My Network Places
            Shell32.Folder netFolder = shell32.NameSpace(ShellFolder.NetworkNeighborhood);
            Shell32.FolderItem mynetwork = null;
            foreach (Shell32.FolderItem fi in netFolder.ParentFolder.Items())
            {
                if (fi.Name == netFolder.Title)
                {
                    mynetwork = fi;
                    break;
                }
            }

            // iterate through the Desktop namespace and populate the first level nodes
            foreach (Shell32.FolderItem item in items)
            {
                Debug.Print(item.Path);
                if (!item.IsFolder) continue;  // this ensures that desktop shortcuts etc are not displayed
                if (item.IsBrowsable) continue;  // [xiperware] exclude zip files
                if (recycle != null && item.Path == recycle.Path) continue;  // [xiperware] skip recycle bin

                TreeNode tn = AddTreeNode(item, ref imageCount, imageList, getIcons);
                desktop.Nodes.Add(tn);

                if (mynetwork != null && item.Path == mynetwork.Path)  // [xiperware] skip my network places subdirs
                {
                    tree.Tag = tn;  // store node in tag
                    continue;
                }

                CheckForSubDirs(tn, imageList);
            }

        }

        private static void FillSubDirectories(TreeNode tn, ref int imageCount, ImageList imageList, bool getIcons)
        {
            Shell32.FolderItem folderItem = (Shell32.FolderItem)tn.Tag;
            Shell32.Folder folder = (Shell32.Folder)folderItem.GetFolder;

            foreach (Shell32.FolderItem item in folder.Items())
            {
                //if (item.IsFileSystem && item.IsFolder && !item.IsBrowsable)
                if (item.IsFolder && !item.IsBrowsable)
                {
                    TreeNode ntn = AddTreeNode(item, ref imageCount, imageList, getIcons);
                    tn.Nodes.Add(ntn);
                    CheckForSubDirs(ntn, imageList);
                }
            }
        }

        private static void CheckForSubDirs(TreeNode tn, ImageList imageList)
        {
            if (tn.Nodes.Count == 0)
            {
                try
                {
                    // create dummy nodes for any subfolders that have further subfolders
                    Shell32.FolderItem folderItem = (Shell32.FolderItem)tn.Tag;
                    Shell32.Folder folder = (Shell32.Folder)folderItem.GetFolder;

                    bool hasFolders = false;
                    foreach (Shell32.FolderItem item in folder.Items())
                    {
                        //if (item.IsFileSystem && item.IsFolder && !item.IsBrowsable)
                        if (item.IsFolder && !item.IsBrowsable)
                        {
                            hasFolders = true;
                            break;
                        }
                    }
                    if (hasFolders)
                    {
                        TreeNode ntn = new TreeNode();
                        ntn.Tag = "DUMMYNODE";
                        tn.Nodes.Add(ntn);
                    }
                }
                catch { }
            }
        }

        private static TreeNode AddTreeNode(Shell32.FolderItem item, ref int imageCount, ImageList imageList, bool getIcons)
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
                    imageList.Images.Add(ExtractIcons.GetIcon(item.Path, false)); // normal icon
                    imageList.Images.Add(ExtractIcons.GetIcon(item.Path, true)); // selected icon
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
