using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DirectoryBrowser;
using ImageBrowserLogic;
using ImageBrowserLogic.ImageProviders;
using ImageBrowserLogic.LoadingStrategies;
using ImageBrowserPresenter;

namespace ImageBrowser
{
    public partial class Form2 : Form, IImageBrowserView2
    {
        private ImageBrowserPresenter2 _presenter;
        private readonly Control _listViewParentContainer;
        public event ImageBrowserViewDirectorySelectedHandler DirectorySelected;
        public event ImageBrowserViewEventDelegate2 BrowserViewLoad;

        public Form2()
        {
            InitializeComponent();
            _presenter = new ImageBrowserPresenter2(this);
             _listViewParentContainer = listView1.Parent;
       }

        public DirectoryTree DirectoryTree
        {
            get { return directoryTree1; }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (BrowserViewLoad != null)
                BrowserViewLoad(this, this);

            //directoryTree1.InitDrives();
            //directoryTree1.DirectorySelected += OnDirectorySelected;
        }

        public Control ListViewParentContainer { get { return _listViewParentContainer; } }

        public void InitializeListView(ListView listView)
        {
            listView.View = View.LargeIcon;
            listView.LabelEdit = false;
            listView.LabelWrap = true;
            listView.Scrollable = true;
            listView.Dock = DockStyle.Fill;
            listView.UseCompatibleStateImageBehavior = false;
            listView.Location = listView1.Location;
            listView.Size = listView1.Size;
        }

        public void OnDirectorySelected(DirectoryInfo dir)
        {
            if (DirectorySelected != null)
                DirectorySelected(this,dir,ref listView1);
            //UpdateStatusBar(dir.FullName);
            //_thumbnailSets.DisplayList(dir, ref listView1);
            //UpdateStatusBar(dir.FullName);
        }

        #region StatusBar updates
        public void UpdateDirStatus(string msg)
        {
            toolStripDirInfo.Text = msg;
        }
        public void UpdateAppStatus(string msg)
        {
            toolStripAppInfo.Text = msg;
        }
        public void UpdateFilesStatus(string msg)
        {
            toolStripImagesInfo.Text = msg;
        }
        //private void UpdateStatusBar(string dir = null)
        //{
        //    if (dir != null)
        //        toolStripDirInfo.Text = dir;

        //    var memoryUsed = GetMemoryUsed();
        //    toolStripAppInfo.Text = memoryUsed;

        //    UpdateImageListCount();
        //}

        //private void UpdateImageListCount()
        //{
        //    if (listView1.LargeImageList != null)
        //    {
        //        var numLoaded = listView1.LargeImageList.Images.Count - 1;
        //        var numFiles = listView1.Items.Count;
        //        toolStripImagesInfo.Text = string.Format("{0} of {1} images loaded", numLoaded, numFiles);
        //    }
        //}

        //private static string GetMemoryUsed()
        //{
        //    long privateBytes;
        //    using (var proc = Process.GetCurrentProcess())
        //    {
        //        proc.Refresh();
        //        privateBytes = proc.PrivateMemorySize64;
        //    }

        //    var memoryUsed = string.Format("App PrivateBytes: {0:N0} KB", privateBytes / 1024);
        //    return memoryUsed;
        //}

        #endregion

    }
}
