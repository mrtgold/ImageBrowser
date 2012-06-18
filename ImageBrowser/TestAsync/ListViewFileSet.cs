using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ImageBrowserLogic;

namespace TestAsync
{
    public class ListViewFileSet : FileSet
    {
        private const string DefaultImageKey = "default";
        private readonly BackgroundWorker _worker;
        private readonly ListView _targetListView;
        public ImageList ImageList { get; set; }

        public ListViewFileSet(DirectoryInfo dir, BackgroundWorker worker, ListView targetListView, params string[] filePatterns)
            : base(dir, filePatterns)
        {
            _worker = worker;
            _targetListView = targetListView;
            ImageList = GetNewImageList();
        }

        private static ImageList GetNewImageList()
        {
            var imageList = new ImageList();
            imageList.Images.Add(DefaultImageKey, BrowserResources.Properties.Resources.Image_File);
            return imageList;
        }

        public void BeginLoadingImages()
        {
        }

        public void BlockingLoadFilesOnly(ListView targetListView)
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

        public void BlockingLoadAllDefaultImages(ListView targetListView)
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

        public void BlockingAddDefaultImages(ListView targetListView)
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

        public void BlockingLoadAllImages(ListView targetListView)
        {
            targetListView.LargeImageList = ImageList;
            foreach (var node in this)
            {
                LoadImage(node);
                Trace.WriteLine(string.Format("BlockingLoadAllImages:{0}: {1} remaining", Dir, FilesNotDone.Count()));
            }
        }

        private void LoadImage(FileNode node)
        {
            var key = node.File.FullName;
            var item = _targetListView.Items.Add(key, node.File.Name, DefaultImageKey);
            node.BlockingLoadImage();
            ImageList.Images.Add(key, node.Image);
            item.ImageKey = key;
        }
    }
}