using System.Diagnostics;
using System.IO;

namespace TestAsync
{
    public class ListViewFileSet_BlockingLoadAllDefaultImages : ListViewFileSetBase
    {

        public ListViewFileSet_BlockingLoadAllDefaultImages(DirectoryInfo dir, params string[] filePatterns)
            : base(dir, filePatterns)
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