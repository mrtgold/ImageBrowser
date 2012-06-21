using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogic.LoadingStrategies
{
    public abstract class ListViewFileSetBase : FileSet, IListViewFileSet
    {
        protected const string DefaultImageKey = "default";
        public ImageList ImageList
        {
            get { return ListView.LargeImageList; }
        }

        public ListView ListView { get; set; }

        public string StatusMessage
        {
            get
            {
                if (ListView == null || ImageList == null) return null;
                var numFiles = ListView.Items.Count;
                var msg = string.Format("{0} of {1} images loaded", ImagesLoaded, numFiles);
                return msg;
            }
        }

        public int ImagesLoaded
        {
            get
            {
                if (ListView == null || ImageList == null) return 0;
                return ImageList.Images.Count - 1;
            }
        }

        protected ListViewFileSetBase(DirectoryInfo dir, Action<ListView> initializeListView, IImageProviderFactory imageProviderFactory, params string[] filePatterns)
            : base(dir,imageProviderFactory, filePatterns)
        {
            ListView = new ListView {LargeImageList = GetNewImageList()};

            if (initializeListView != null)
                initializeListView(ListView);
        }

        private static ImageList GetNewImageList()
        {
            var imageList = new ImageList { ImageSize = new Size(100, 100), ColorDepth = ColorDepth.Depth16Bit};
            imageList.Images.Add(DefaultImageKey, BrowserResources.Properties.Resources.Image_File);
            return imageList;
        }

        public abstract void BeginLoadingImages();
    }
}