using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogic.LoadingStrategies
{
    public class ListViewFileSet_LoadFilesBlockingLoadImages : ListViewFileSetBase
    {
        public ListViewFileSet_LoadFilesBlockingLoadImages(DirectoryInfo dir, IImageProviderFactory factory, Action<ListView> initializeListView, params string[] filePatterns)
            : base(dir, initializeListView, new SimpleBitmapThumbnailGetterFactory(100), filePatterns)
        {
        }

        public override void BeginLoadingImages()
        {
            BlockingLoadFilesOnly();
            BlockingAddDefaultImages();
        }

        private void BlockingLoadFilesOnly()
        {
            var sw = Stopwatch.StartNew();

            foreach (var node in this)
            {
                var key = node.File.FullName;
                var item = ListView.Items.Add(key, node.File.Name, DefaultImageKey);
                //Trace.WriteLine(string.Format("BlockingLoadFilesOnly:{0}: {1} remaining", Dir, FilesNotDone.Count()));
            }
            sw.Stop();
            Trace.WriteLine(string.Format("BlockingLoadFilesOnly: loaded {0} default in {1} msec", Count, sw.ElapsedMilliseconds));
        }

        private void BlockingAddDefaultImages()
        {
            var sw = Stopwatch.StartNew();

            foreach (var node in this)
            {
                var key = node.File.FullName;
                if (ListView.Items.ContainsKey(key))
                    ListView.Items[key].ImageKey = DefaultImageKey;
                //Trace.WriteLine(string.Format("BlockingLoadAllDefaultImages:{0}: {1} remaining", Dir, FilesNotDone.Count()));
            }
            sw.Stop();
            Trace.WriteLine(string.Format("BlockingAddDefaultImages: loaded {0} default in {1} msec", Count, sw.ElapsedMilliseconds));
        }
    }
}