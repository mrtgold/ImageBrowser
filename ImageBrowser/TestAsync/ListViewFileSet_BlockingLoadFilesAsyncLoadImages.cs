using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ImageBrowserLogic;

namespace TestAsync
{
    public class ListViewFileSet_BlockingLoadFilesAsyncLoadImages : ListViewFileSetBase
    {
        public ListViewFileSet_BlockingLoadFilesAsyncLoadImages(DirectoryInfo dir, params string[] filePatterns)
            : base(dir, filePatterns)
        {
        }

        public override void BeginLoadingImages(ListView targetListView)
        {
            targetListView.LargeImageList = ImageList;
            BlockingLoadFilesOnly(targetListView);
            AsyncAddRealImages(targetListView);
        }

        private void BlockingLoadFilesOnly(ListView targetListView)
        {
            var sw = Stopwatch.StartNew();

            foreach (var node in this)
            {
                targetListView.Items.Add(node.Key, node.File.Name, DefaultImageKey);
            }
            sw.Stop();
            Trace.WriteLine(string.Format("BlockingLoadFilesOnly: loaded {0} default in {1} msec", Count, sw.ElapsedMilliseconds));
        }

        private void AsyncAddRealImages(ListView targetListView)
        {
            var sw = Stopwatch.StartNew();

            foreach (var node in this)
            {
                node.ImageGetter = new FullSizeImageGetter();
                node.ImageGetter.BeginGetImage(ar => ProcessResult(ar, targetListView, this, node), node.File.FullName);
            }
            sw.Stop();
            Trace.WriteLine(string.Format("AsyncAddRealImages: loaded {0} default in {1} msec", Count, sw.ElapsedMilliseconds));
        }

        delegate void ProcessResultCallback(IAsyncResult result, ListView targetListView, IListViewFileSet fileSet, FileNode node);
        private static void ProcessResult(IAsyncResult result, ListView targetListView, IListViewFileSet fileSet, FileNode node)
        {
            if (targetListView.InvokeRequired)
            {
                var d = new ProcessResultCallback(ProcessResult);
                targetListView.Invoke(d, new object[] { result, targetListView, fileSet, node });
            }
            else
            {
                var image = node.ImageGetter.EndGetImage(result);
                fileSet.ImageList.Images.Add(node.Key, image);

                //if (targetListView.Items.ContainsKey(key))
                targetListView.Items[node.Key].ImageKey = node.Key;
                Trace.WriteLine("Updated image file {0}", node.Key);
            }
        }

    }
}