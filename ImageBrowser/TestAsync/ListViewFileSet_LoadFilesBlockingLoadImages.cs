using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TestAsync
{
    public class ListViewFileSet_LoadFilesBlockingLoadImages : ListViewFileSetBase
    {
        public ListViewFileSet_LoadFilesBlockingLoadImages(DirectoryInfo dir, params string[] filePatterns)
            : base(dir, filePatterns)
        {
        }

        public override void BeginLoadingImages(ListView targetListView)
        {
            BlockingLoadFilesOnly(targetListView);
            BlockingAddDefaultImages(targetListView);
        }

        private void BlockingLoadFilesOnly(ListView targetListView)
        {
            var sw = Stopwatch.StartNew();
            targetListView.LargeImageList = ImageList;

            foreach (var node in this)
            {
                var key = node.File.FullName;
                var item = targetListView.Items.Add(key, node.File.Name, 0);
                //Trace.WriteLine(string.Format("BlockingLoadFilesOnly:{0}: {1} remaining", Dir, FilesNotDone.Count()));
            }
            sw.Stop();
            Trace.WriteLine(string.Format("BlockingLoadFilesOnly: loaded {0} default in {1} msec", Count, sw.ElapsedMilliseconds));
        }

        private void BlockingAddDefaultImages(ListView targetListView)
        {
            var sw = Stopwatch.StartNew();
            targetListView.LargeImageList = ImageList;

            foreach (var node in this)
            {
                var key = node.File.FullName;
                if (targetListView.Items.ContainsKey(key))
                    targetListView.Items[key].ImageKey = DefaultImageKey;
                //Trace.WriteLine(string.Format("BlockingLoadAllDefaultImages:{0}: {1} remaining", Dir, FilesNotDone.Count()));
            }
            sw.Stop();
            Trace.WriteLine(string.Format("BlockingAddDefaultImages: loaded {0} default in {1} msec", Count, sw.ElapsedMilliseconds));
        }
    }
}