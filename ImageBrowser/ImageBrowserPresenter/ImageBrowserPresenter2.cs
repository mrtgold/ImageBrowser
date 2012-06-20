using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using ImageBrowserLogic;
using ImageBrowserLogic.ImageProviders;
using ImageBrowserLogic.LoadingStrategies;
using Timer = System.Timers.Timer;

namespace ImageBrowserPresenter
{
    public delegate void ImageBrowserViewEventDelegate2(object sender, IImageBrowserView2 view);
    public delegate void ImageBrowserViewDirectorySelectedHandler(object sender, DirectoryInfo dir, ref ListView listView1);


    public class ImageBrowserPresenter2
    {
        private const int StatusPollingIntervalMsec = 2000; //50;

        private readonly IImageBrowserView2 _view;
        private ThumbnailSets _thumbnailSets;
        private DirectoryInfo _currentDir;

        private static Timer _timer;

        public ImageBrowserPresenter2(IImageBrowserView2 view)
        {
            _view = view;
            view.DirectorySelected += view_DirectorySelected;
            view.BrowserViewLoad += view_BrowserViewLoad;
        }

        void view_BrowserViewLoad(object sender, IImageBrowserView2 view)
        {
            view.DirectoryTree.InitDrives();
            view.DirectoryTree.DirectorySelected += view.OnDirectorySelected;

            var imageProviderFactory = new SimpleBitmapThumbnailGetterFactory(100);
            var fileSetFactory = new BlockingLoadFilesAsyncListViewFileSetFactory(imageProviderFactory, _view.InitializeListView);
            _thumbnailSets = new ThumbnailSets(view.ListViewParentContainer, new[] { "*.jpg", "*.bmp", "*.png" }, fileSetFactory);

            InitTimer();
        }

        private void InitTimer()
        {
            _timer = new Timer(StatusPollingIntervalMsec) { AutoReset = true };
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            UpdateStatusBar();
        }


        void view_DirectorySelected(object sender, DirectoryInfo dir, ref ListView listView1)
        {
            _currentDir = dir;
            UpdateStatusBar(dir.FullName);
            _thumbnailSets.DisplayList(dir, ref listView1);
            UpdateStatusBar(dir.FullName);
        }

        #region Status bar updates
        private void UpdateStatusBar(string dir = null)
        {
            if (dir != null)
                _view.UpdateDirStatus(dir);

            UpdateAppStatus();

            UpdateImageListCount();
        }

        private void UpdateAppStatus()
        {
            _view.UpdateAppStatus(GetMemoryUsed() + GetImagesLoaded());
        }

        private static string GetMemoryUsed()
        {
            long privateBytes;
            using (var proc = Process.GetCurrentProcess())
            {
                proc.Refresh();
                privateBytes = proc.PrivateMemorySize64;
            }

            var memoryUsed = string.Format("  PrivateBytes: {0:N0} KB  ", privateBytes / 1024);
            return memoryUsed;
        }

        private string GetImagesLoaded()
        {
            var imageCount = _thumbnailSets.Select(dir => dir.Value).Sum(node => node.ImageList.Images.Count - 1);

            var imagesLoaded = string.Format("  Images loaded: {0:N0}  ", imageCount);
            return imagesLoaded;
        }

        private void UpdateImageListCount()
        {
            if (!_thumbnailSets.ContainsKey(_currentDir)) return;
            var listView = _thumbnailSets[_currentDir].ListView;
            if (listView == null) return;

            var imageList = listView.LargeImageList;
            if (imageList == null) return;
            var numLoaded = imageList.Images.Count - 1;
            var numFiles = listView.Items.Count;
            var msg = string.Format("{0} of {1} images loaded", numLoaded, numFiles);
            _view.UpdateFilesStatus(msg);
        }
        
        #endregion
    }
}