﻿using System;
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
        private readonly Dictionary<DirectoryInfo, ListView> _listViews;
        private readonly string[] _filePatterns = new[] { "*.jpg", "*.bmp", "*.png" };
        private static Point _listViewLocation;
        private static Size _listViewSize;
        private static bool _listViewUseCompatibleStateImageBehavior;
        private static Control _listViewParent;
        private readonly Process _proc;


        public Form2()
        {
            InitializeComponent();
            CaptureListViewParams(listView1);
            _thumbnailSets = new ThumbnailSets();
            _listViews = new Dictionary<DirectoryInfo, ListView>();
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

        private void DirectorySelected(DirectoryInfo dir)
        {
            UpdateStatusBar(dir.FullName);
            DisplayList(dir);
        }

        private void DisplayList(DirectoryInfo dir)
        {
            var sw = Stopwatch.StartNew();

            var listViewFileSet = GetListViewFileSet(dir, _thumbnailSets, _filePatterns);

            DisplayList(listViewFileSet.ListView, sw, _listViewParent, ref listView1);

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

        private static IListViewFileSet GetListViewFileSet(DirectoryInfo dir, Dictionary<DirectoryInfo, IListViewFileSet> fileSets, string[] filePatterns)
        {
            IListViewFileSet listViewFileSet;
            if (!fileSets.ContainsKey(dir))
            {
                listViewFileSet = new ListViewFileSet_BlockingLoadFilesAsyncLoadImages(dir, filePatterns)
                                      {ListView = new ListView()};
                InitializeListView(listViewFileSet.ListView);
                fileSets[dir] = listViewFileSet;
                listViewFileSet.BeginLoadingImages(listViewFileSet.ListView);
            }
            else
                listViewFileSet = fileSets[dir];

            return listViewFileSet;
        }


    }
}
