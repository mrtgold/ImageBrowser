using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ImageBrowserLogic;

namespace TestAsync
{
    public abstract class ListViewFileSetBase : FileSet, IListViewFileSet
    {
        protected const string DefaultImageKey = "default";
        public ImageList ImageList { get; private set; }
        public ListView ListView { get; set; }

        protected ListViewFileSetBase(DirectoryInfo dir, params string[] filePatterns)
            : base(dir, filePatterns)
        {
            ImageList = GetNewImageList();
        }

        private static ImageList GetNewImageList()
        {
            var imageList = new ImageList { ImageSize = new Size(100, 100) };
            imageList.Images.Add(DefaultImageKey, BrowserResources.Properties.Resources.Image_File);
            return imageList;
        }

        public abstract void BeginLoadingImages(ListView targetListView);
    }
}