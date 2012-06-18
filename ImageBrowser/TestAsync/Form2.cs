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
        private readonly Point _listViewLocation;
        private readonly Size _listViewSize;


        public Form2()
        {
            InitializeComponent();
            _listViewLocation = listView1.Location;
            _listViewSize = listView1.Size;
            _thumbnailSets = new ThumbnailSets(listView1.Parent, InitializeListView, new[] { "*.jpg", "*.bmp", "*.png" });
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

        private void UpdateStatusBar(string dir = null)
        {
            if (dir != null)
                toolStripDirInfo.Text = dir;

            var memoryUsed = GetMemoryUsed();
            toolStripAppInfo.Text = memoryUsed;

            toolStripImagesInfo.Text = " ";
        }

        private static string GetMemoryUsed()
        {
            long privateBytes;
            using (var proc = Process.GetCurrentProcess())
            {
                proc.Refresh();
                privateBytes = proc.PrivateMemorySize64;
            }

            var memoryUsed = string.Format("App PrivateBytes: {0:N0} KB", privateBytes / 1024);
            return memoryUsed;
        }


    }
}
