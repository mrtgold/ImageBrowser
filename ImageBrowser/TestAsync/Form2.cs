using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TestAsync
{
    public partial class Form2 : Form
    {
        private readonly ThumbnailSets _thumbnailSets;
        private  Point _listViewLocation;
        private  Size _listViewSize;
        private  Control _listViewParent;
        private readonly Process _proc;


        public Form2()
        {
            InitializeComponent();
            CaptureListViewParams(listView1);
            _thumbnailSets = new ThumbnailSets(_listViewParent, InitializeListView, new[] { "*.jpg", "*.bmp", "*.png" });
            _proc = Process.GetCurrentProcess();
        }

        private void UpdateStatusBar(string dir = null)
        {
            if (dir != null)
                toolStripDirInfo.Text = dir;

            var memoryUsed = GetMemoryUsed();
            toolStripAppInfo.Text = memoryUsed;

            toolStripImagesInfo.Text = " ";
        }

        private string GetMemoryUsed()
        {
            _proc.Refresh();
            var privateBytes = _proc.PrivateMemorySize64;
            var memoryUsed = string.Format("App PrivateBytes: {0:N0} KB", privateBytes/1024);
            return memoryUsed;
        }

        private  void CaptureListViewParams(ListView listView)
        {
            _listViewParent = listView.Parent;
            _listViewLocation = listView.Location;
            _listViewSize = listView.Size;
        }

        private  void InitializeListView(ListView listView)
        {
            listView.View = View.LargeIcon;
            listView.LabelEdit = false;
            listView.LabelWrap = true;
            listView.Scrollable = true;
            listView.Dock = DockStyle.Fill;
            listView.UseCompatibleStateImageBehavior = false;
            listView.Location = _listViewLocation;
            listView.Size = _listViewSize;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            directoryTree1.InitDrives();
            directoryTree1.DirectorySelected += DirectorySelected;
            UpdateStatusBar();
        }

        private void DirectorySelected(DirectoryInfo dir)
        {
            UpdateStatusBar(dir.FullName);
            _thumbnailSets.DisplayList(dir, ref listView1);
        }

    }
}
