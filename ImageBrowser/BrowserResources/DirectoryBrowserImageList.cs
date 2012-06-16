using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrowserResources.Properties;

namespace BrowserResources
{
    public static class DirectoryBrowserImageList
    {

        public enum TreeViewImages
        {
            Computer = 1,
            Drive,
            OpenFolder,
            ClosedFolder,
            StuffedFolder,
            Warning,
            Error,
            DefaultImageFile
        }

        public static ImageList GetImageList()
        {
            var imageList = new ImageList();
            imageList.Images.Add(TreeViewImages.ClosedFolder.ToString(), Resources.Folder_48x48);
            imageList.Images.Add(TreeViewImages.Computer.ToString(), Resources.Computer);
            imageList.Images.Add(TreeViewImages.Drive.ToString(), Resources.Hard_Drive);
            imageList.Images.Add(TreeViewImages.OpenFolder.ToString(), Resources.FolderOpen_48x48_72);
            imageList.Images.Add(TreeViewImages.StuffedFolder.ToString(), Resources.Stuffed_Folder);
            imageList.Images.Add(TreeViewImages.Warning.ToString(), Resources.Warning);
            imageList.Images.Add(TreeViewImages.Error.ToString(), Resources.error);
            imageList.Images.Add(TreeViewImages.DefaultImageFile.ToString(), Resources.Image_File);
            return imageList;
        }

    }
}
