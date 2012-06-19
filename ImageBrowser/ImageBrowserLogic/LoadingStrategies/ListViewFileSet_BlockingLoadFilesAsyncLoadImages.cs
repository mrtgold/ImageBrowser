using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogic.LoadingStrategies
{
    public class ListViewFileSet_BlockingLoadFilesAsyncLoadImages : ListViewFileSetBase
    {
        public ListViewFileSet_BlockingLoadFilesAsyncLoadImages(DirectoryInfo dir, IImageProviderFactory factory, Action<ListView> initializeListView, params string[] filePatterns)
            : base(dir, initializeListView, factory, filePatterns)
        {
        }

        public override void BeginLoadingImages()
        {
            BlockingLoadFilesOnly();
            AsyncAddRealImages();
        }

        private void BlockingLoadFilesOnly()
        {
            var sw = Stopwatch.StartNew();

            foreach (var node in this)
            {
                ListView.Items.Add(node.Key, node.File.Name, DefaultImageKey);
            }
            sw.Stop();
            Trace.WriteLine(string.Format("BlockingLoadFilesOnly: loaded {0} default in {1} msec", Count, sw.ElapsedMilliseconds));
        }

        private void AsyncAddRealImages()
        {
            var sw = Stopwatch.StartNew();

            foreach (var node in this)
            {
                node.BeginLoadImage();
            }
            sw.Stop();
            Trace.WriteLine(string.Format("AsyncAddRealImages: loaded {0} default in {1} msec", Count, sw.ElapsedMilliseconds));
        }

    }
}