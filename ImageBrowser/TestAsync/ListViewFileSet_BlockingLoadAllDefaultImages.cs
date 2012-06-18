using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TestAsync
{
    public class ListViewFileSet_BlockingLoadAllDefaultImages : ListViewFileSetBase
    {

        public ListViewFileSet_BlockingLoadAllDefaultImages(DirectoryInfo dir, params string[] filePatterns)
            : base(dir, filePatterns)
        {
        }

        public override void BeginLoadingImages(ListView targetListView)
        {
            BlockingLoadAllDefaultImages(targetListView);
        }

        private void BlockingLoadAllDefaultImages(ListView targetListView)
        {
            var sw = Stopwatch.StartNew();
            targetListView.LargeImageList = ImageList;

            foreach (var node in this)
            {
                var key = node.File.FullName;
                var item = targetListView.Items.Add(key, node.File.Name, DefaultImageKey);
                //Trace.WriteLine(string.Format("BlockingLoadAllDefaultImages:{0}: {1} remaining", Dir, FilesNotDone.Count()));
            }
            sw.Stop();
            Trace.WriteLine(string.Format("BlockingLoadAllDefaultImages: loaded {0} default in {1} msec", Count, sw.ElapsedMilliseconds));
        }
    }
}