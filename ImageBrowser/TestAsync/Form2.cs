using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ImageBrowserLogic;

namespace TestAsync
{
    public partial class Form2 : Form
    {
        private readonly Dictionary<DirectoryInfo, IListViewFileSet> _dirs;
        private readonly Dictionary<DirectoryInfo, ListView> _listViews;
        private readonly string[] _filePatterns = new string[] { "*.jpg", "*.bmp", "*.png" };
        private static Point _listViewLocation;
        private static Size _listViewSize;
        private static bool _listViewUseCompatibleStateImageBehavior;
        private static Control _listViewParent;
        private readonly Process _proc;

        public Form2()
        {
            InitializeComponent();
            CaptureListViewParams(listView1);
            _dirs = new Dictionary<DirectoryInfo, IListViewFileSet>();
            _listViews = new Dictionary<DirectoryInfo, ListView>();
            _proc = Process.GetCurrentProcess();
        }

        private void UpdateStatusBar(string dir = null)
        {
            if (dir != null)
                toolStripDirInfo.Text = dir;

            _proc.Refresh();
            var privateBytes = _proc.PrivateMemorySize64;
            toolStripAppInfo.Text = string.Format("App PrivateBytes: {0:N0} KB", privateBytes / 1024);

            toolStripImagesInfo.Text = " ";
        }

        private static void CaptureListViewParams(ListView listView)
        {
            _listViewParent = listView.Parent;
            _listViewLocation = listView.Location;
            _listViewSize = listView.Size;
            _listViewUseCompatibleStateImageBehavior = listView.UseCompatibleStateImageBehavior;
        }

        private static void InitializeListView(ListView listView)
        {
            listView.View = View.LargeIcon;
            listView.LabelEdit = false;
            listView.LabelWrap = true;
            listView.Scrollable = true;
            listView.Dock = DockStyle.Fill;
            listView.Location = _listViewLocation;
            listView.Size = _listViewSize;
            listView.UseCompatibleStateImageBehavior = _listViewUseCompatibleStateImageBehavior;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            directoryTree1.InitDrives();
            directoryTree1.DirectorySelected += DirectorySelected;
            UpdateStatusBar();
        }

        //private void DirectorySelected(DirectoryInfo dir)
        //{
        //    var sw = Stopwatch.StartNew();
        //    toolStripStatusLabel1.Text = dir.FullName;
        //    listView1.Items.Clear();

        //    if (!_dirs.ContainsKey(dir))
        //    {
        //        //_dirs[dir] = new ListViewFileSet(dir, new BackgroundWorker(), listView1, _filePatterns);
        //        _dirs[dir] = new ListViewFileSet1(dir, _filePatterns);
        //    }
        //    if (!_listViews.ContainsKey(dir))
        //    {
        //        var listView = new ListView();
        //        _listViews[dir] = listView;
        //        _dirs[dir].BeginLoadingImages(listView);
        //    }

        //    listView1 = _listViews[dir];
        //    sw.Stop();

        //    Trace.WriteLine(string.Format( "loaded dir {0} in {1} msec", dir.FullName, sw.ElapsedMilliseconds));
        //}
        private void DirectorySelected(DirectoryInfo dir)
        {
            UpdateStatusBar(dir.FullName);
            var sw = Stopwatch.StartNew();

            var listViewFileSet = GetListViewFileSet(dir, _dirs, _filePatterns);
            var listView = GetListView(dir, listViewFileSet, _listViews);

            DisplayList(listView, sw, _listViewParent, ref listView1);

            sw.Stop();

            Trace.WriteLine(string.Format("loaded dir {0} in {1} msec", dir.FullName, sw.ElapsedMilliseconds));
        }

        private static void DisplayList(ListView newListView, Stopwatch sw, Control listViewParent, ref ListView previousListView)
        {
            if (ReferenceEquals(previousListView, newListView)) return;

            listViewParent.SuspendLayout();
            Trace.WriteLine(string.Format("after SuspendLayout: {0} msec", sw.ElapsedMilliseconds));

            listViewParent.Controls.Add(newListView);
            Trace.WriteLine(string.Format("after add: {0} msec", sw.ElapsedMilliseconds));

            listViewParent.Controls.Remove(previousListView);
            Trace.WriteLine(string.Format("after remove: {0} msec", sw.ElapsedMilliseconds));

            listViewParent.ResumeLayout(true);
            Trace.WriteLine(string.Format("after ResumeLayout: {0} msec", sw.ElapsedMilliseconds));
            previousListView = newListView;
        }

        private static ListView GetListView(DirectoryInfo dir, IListViewFileSet listViewFileSet, Dictionary<DirectoryInfo, ListView> listViews)
        {
            ListView listView;
            if (!listViews.ContainsKey(dir))
            {
                listView = new ListView();
                InitializeListView(listView);
                listViews[dir] = listView;
                listViewFileSet.BeginLoadingImages(listView);
            }
            else
                listView = listViews[dir];
            return listView;
        }

        private static IListViewFileSet GetListViewFileSet(DirectoryInfo dir, Dictionary<DirectoryInfo, IListViewFileSet> fileSets, string[] filePatterns)
        {
            IListViewFileSet listViewFileSet;
            if (!fileSets.ContainsKey(dir))
            {
                listViewFileSet = new ListViewFileSet_BlockingLoadFilesAsyncLoadImages(dir, filePatterns);
                fileSets[dir] = listViewFileSet;
            }
            else
                listViewFileSet = fileSets[dir];
            return listViewFileSet;
        }


        //private void Log(string msg, params object[] values)
        //{
        //    toolStripDirInfo.Text= string.Format(msg, values);
        //}

    }
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

    public interface IListViewFileSet : IFileSet
    {
        void BeginLoadingImages(ListView targetListView);
        ImageList ImageList { get; set; }
    }

    public abstract class ListViewFileSetBase : FileSet, IListViewFileSet
    {
        protected const string DefaultImageKey = "default";
        public ImageList ImageList { get; set; }

        protected ListViewFileSetBase(DirectoryInfo dir, params string[] filePatterns)
            : base(dir, filePatterns)
        {
            ImageList = GetNewImageList();
        }

        private static ImageList GetNewImageList()
        {
            var imageList = new ImageList { ImageSize = new Size(100, 100) };
            imageList.Images.Add(DefaultImageKey, BrowserResources.Properties.Resources.Image_File);
            return imageList;
        }

        public abstract void BeginLoadingImages(ListView targetListView);
    }

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

    public class ListViewFileSet_BlockingLoadAllImages : ListViewFileSetBase
    {
        public ListViewFileSet_BlockingLoadAllImages(DirectoryInfo dir, params string[] filePatterns)
            : base(dir, filePatterns)
        {
        }

        public override void BeginLoadingImages(ListView targetListView)
        {
            BlockingLoadAllImages(targetListView);
        }

        private void BlockingLoadAllImages(ListView targetListView)
        {
            targetListView.LargeImageList = ImageList;
            foreach (var node in this)
            {
                LoadImage(node, targetListView);
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
