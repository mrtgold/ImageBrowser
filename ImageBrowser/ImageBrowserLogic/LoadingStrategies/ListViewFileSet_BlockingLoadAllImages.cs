using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogic.LoadingStrategies
{
    public class ListViewFileSet_BlockingLoadAllImages : ListViewFileSetBase
    {
        public ListViewFileSet_BlockingLoadAllImages(DirectoryInfo dir, IImageProviderFactory factory, Action<ListView> initializeListView, params string[] filePatterns)
            : base(dir, initializeListView, new SimpleBitmapThumbnailGetterFactory(100), filePatterns)
        {
        }

        public override void BeginLoadingImages()
        {
            BlockingLoadAllImages();
        }

        private void BlockingLoadAllImages()
        {
            foreach (var node in this)
            {
                LoadImage(node, ListView);
                Trace.WriteLine(string.Format("BlockingLoadAllImages:{0}: {1} remaining", Dir, FilesNotDone.Count()));
            }
        }

        private void LoadImage(FileNode node, ListView targetListView)
        {
            var key = node.File.FullName;
            var item = targetListView.Items.Add(key, node.File.Name, DefaultImageKey);
            node.BlockingLoadImage();
            ImageList.Images.Add(key, node.Image);
            item.ImageKey = key;
        }
    }
}