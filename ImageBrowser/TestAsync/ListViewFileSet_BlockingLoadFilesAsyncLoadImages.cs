using System;
using System.Diagnostics;
using System.IO;
using ImageBrowserLogic;

namespace TestAsync
{
    public class ListViewFileSet_BlockingLoadFilesAsyncLoadImages : ListViewFileSetBase
    {
        public ListViewFileSet_BlockingLoadFilesAsyncLoadImages(DirectoryInfo dir, params string[] filePatterns)
            : base(dir, filePatterns)
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
                node.ImageGetter = new SimpleBitmapThumbnailGetter(100);
                //node.ImageGetter = new FullSizeImageGetter();
                node.ImageGetter.BeginGetImage(ar => ProcessResult(ar, this, node), node.File.FullName);
            }
            sw.Stop();
            Trace.WriteLine(string.Format("AsyncAddRealImages: loaded {0} default in {1} msec", Count, sw.ElapsedMilliseconds));
        }

        delegate void ProcessResultCallback(IAsyncResult result,  IListViewFileSet fileSet, FileNode node);
        private static void ProcessResult(IAsyncResult result, IListViewFileSet fileSet, FileNode node)
        {
            if (fileSet.ListView.InvokeRequired)
            {
                var d = new ProcessResultCallback(ProcessResult);
                fileSet.ListView.Invoke(d, new object[] { result, fileSet, node });
            }
            else
            {
                var image = node.ImageGetter.EndGetImage(result);
                fileSet.ImageList.Images.Add(node.Key, image);

                fileSet.ListView.Items[node.Key].ImageKey = node.Key;
                Trace.WriteLine(string.Format( "Updated image file {0}", node.Key));
            }
        }

    }
}