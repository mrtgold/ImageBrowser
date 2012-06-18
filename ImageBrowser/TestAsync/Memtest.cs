using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TestAsync
{
    public partial class Memtest : Form
    {
        private readonly List< IListViewFileSet> _memoryTest;
        private readonly List<ListView> _listViews;
        private readonly string[] _filePatterns = new[] { "*.jpg", "*.bmp", "*.png" };
        private readonly DirectoryInfo _490Images;
        private readonly long _initialPrivateBytes;

        public Memtest()
        {
            InitializeComponent();
            _listViews = new List<ListView>();
            _memoryTest = new List<IListViewFileSet>();
            _490Images = new DirectoryInfo(@"C:\VS2012ImageLibrary\Objects\png_format\WinVista");

            _initialPrivateBytes = GetMemoryUsed();
            Log(string.Format("initial memory usage: {0:N0}", _initialPrivateBytes));
        }

        private long GetMemoryUsed()
        {
            var sw = Stopwatch.StartNew();
            var beforeGc = Process.GetCurrentProcess().PrivateMemorySize64;
            GC.Collect();
            var afterGc = Process.GetCurrentProcess().PrivateMemorySize64;
            Log("GC time: {0}", sw.ElapsedMilliseconds);
            Log("GC delta: {0}", beforeGc-afterGc);
            return afterGc;
        }

        /*
         * Default images only
        initial memory usage: 17,223,680
        starting 60 dirs (@ 490 files /dir)
        expected time: 15000 msec
        loaded C:\VS2012ImageLibrary\Objects\png_format\WinVista 60 times in 14248 msec
        final memory usage:	 1,641,648,128
        delta memory usage:	 1,624,424,448
        memory usage per list: (60 lists)	 27,360,802
        memory usage per entry: (25080 entries)	 65,456
        */

        /*
         * FullSizeImages

        */

        private static void InitializeListView(ListView listView)
        {
            listView.Location = new Point(435, 26);
            listView.Size = new Size(354, 402);
            listView.UseCompatibleStateImageBehavior = false;
        }


        private void RunMemtest(int numDirs)
        {
            var sw = Stopwatch.StartNew();
            var dir = _490Images;

            for (var i = 0; i < numDirs; i++)
            {
                var fileSet = new ListViewFileSet_BlockingLoadAllImages(dir, _filePatterns);
                _memoryTest.Add(fileSet);
                var listView = new ListView();
                InitializeListView(listView);
                _listViews.Add(listView);
                fileSet.BeginLoadingImages();
            }

            sw.Stop();

            Log(string.Format("loaded {0} {1} times in {2} msec", dir, numDirs, sw.ElapsedMilliseconds));

            var currentPrivateBytes = GetMemoryUsed();
            Log(string.Format("final memory usage:\t {0:N0}", currentPrivateBytes));
            var delta = currentPrivateBytes - _initialPrivateBytes;
            Log(string.Format("delta memory usage:\t {0:N0}", delta));
            var listCount = _memoryTest.Count;
            var perList = currentPrivateBytes/listCount;
            Log(string.Format("memory usage per list: ({0} lists)\t {1:N0}",listCount, perList));

            var entryCount = _memoryTest.First().Count;
            var perEntry = perList / entryCount;
            Log(string.Format("memory usage per entry: ({0} entries)\t {1:N0}",entryCount*listCount, perEntry));
        }

        private void runMemtesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const int numDirs =30;
            Log("starting {0} dirs (@ 490 files /dir)", numDirs);
            Log("expected time: {0} msec", numDirs*490*3);
            RunMemtest(numDirs);
        }

        private string _log = "";
        private void Log(string msg, params object[] values)
        {
            _log += string.Format(msg, values) +"\r\n" ;
            textBox1.Text = _log;
        }

    }
}
