using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogic.LoadingStrategies
{
    public class ListViewFileSet_BlockingLoadAllDefaultImages : ListViewFileSetBase
    {

        public ListViewFileSet_BlockingLoadAllDefaultImages(DirectoryInfo dir, Action<ListView> initializeListView, params string[] filePatterns)
            : base(dir,initializeListView, new SimpleBitmapThumbnailGetterFactory(100), filePatterns)
        {
        }

        public override void BeginLoadingImages()
        {
            BlockingLoadAllDefaultImages();
        }

        private void BlockingLoadAllDefaultImages()
        {
            var sw = Stopwatch.StartNew();

            foreach (var node in this)
            {
                var key = node.File.FullName;
                var item = ListView.Items.Add(key, node.File.Name, DefaultImageKey);
                //Trace.WriteLine(string.Format("BlockingLoadAllDefaultImages:{0}: {1} remaining", Dir, FilesNotDone.Count()));
            }
            sw.Stop();
            Trace.WriteLine(string.Format("BlockingLoadAllDefaultImages: loaded {0} default in {1} msec", Count, sw.ElapsedMilliseconds));
        }
    }
}