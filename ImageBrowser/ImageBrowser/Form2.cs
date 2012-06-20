using System;
using System.IO;
using System.Windows.Forms;
using DirectoryBrowser;
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


        private void Form2_Load(object sender, EventArgs e)
        {
            if (BrowserViewLoad != null)
                BrowserViewLoad(this, this);
        }

        public Control ListViewParentContainer { get { return _listViewParentContainer; } }
        public DirectoryTree DirectoryTree { get { return directoryTree1; } }

        public void InitializeListView(ListView listView)
        {
            listView.View = listView1.View;
            listView.LabelEdit = listView1.LabelEdit;
            listView.LabelWrap = listView1.LabelWrap;
            listView.Scrollable = listView1.Scrollable;
            listView.Dock = listView1.Dock;
            listView.UseCompatibleStateImageBehavior = listView1.UseCompatibleStateImageBehavior;
            listView.Location = listView1.Location;
            listView.Size = listView1.Size;
        }

        public void OnDirectorySelected(DirectoryInfo dir)
        {
            if (DirectorySelected != null)
                DirectorySelected(this, dir, ref listView1);
        }

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

    }
}
