using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ImageBrowserLogic;
using ImageBrowserLogic.LoadingStrategies;

namespace TestAsync
{
    public partial class Form3 : Form
    {
        private readonly Dictionary<DirectoryInfo, IListViewFileSet> _dirs;
        private readonly Dictionary<DirectoryInfo, ListView> _listViews;
        private readonly string[] _filePatterns = new string[] { "*.jpg", "*.bmp", "*.png" };
        private DirectoryInfo _100Images;
        private DirectoryInfo _490Images;

        public Form3()
        {
            InitializeComponent();
            _dirs = new Dictionary<DirectoryInfo, IListViewFileSet>();
            _listViews = new Dictionary<DirectoryInfo, ListView>();
            _100Images = new DirectoryInfo(@"C:\VS2012ImageLibrary\_Common Elements\Objects");
            _490Images = new DirectoryInfo(@"C:\VS2012ImageLibrary\Objects\png_format\WinVista");

        }

        private static void InitializeListView(ListView listView)
        {
            listView.Location = new Point(435, 26);
            listView.Size = new Size(354, 402);
            listView.UseCompatibleStateImageBehavior = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DirectorySelected(_100Images);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DirectorySelected(_490Images);

        }

        private void DirectorySelected(DirectoryInfo dir)
        {
            var sw = Stopwatch.StartNew();
            //listView1.Items.Clear();

            var listViewFileSet = GetListViewFileSet(dir);
            var listView = GetListView(dir, listViewFileSet);

            DisplayList(listView, sw);

            sw.Stop();

            Trace.WriteLine(string.Format("loaded dir {0} in {1} msec", dir.FullName, sw.ElapsedMilliseconds));
        }

        private void DisplayList(ListView listView, Stopwatch sw)
        {
            if (!ReferenceEquals(listView1, listView))
            {
                SuspendLayout();
                Trace.WriteLine(string.Format("after SuspendLayout: {0} msec", sw.ElapsedMilliseconds));

                this.Controls.Add(listView);
                Trace.WriteLine(string.Format("after add: {0} msec", sw.ElapsedMilliseconds));

                this.Controls.Remove(listView1);
                Trace.WriteLine(string.Format("after remove: {0} msec", sw.ElapsedMilliseconds));

                ResumeLayout(true);
                Trace.WriteLine(string.Format("after ResumeLayout: {0} msec", sw.ElapsedMilliseconds));
                listView1 = listView;
            }
        }

        private ListView GetListView(DirectoryInfo dir, IListViewFileSet listViewFileSet)
        {
            ListView listView;
            if (!_listViews.ContainsKey(dir))
            {
                listView = new ListView();
                InitializeListView(listView);
                _listViews[dir] = listView;
                listViewFileSet.BeginLoadingImages();
            }
            else
                listView = _listViews[dir];
            return listView;
        }

        private IListViewFileSet GetListViewFileSet(DirectoryInfo dir)
        {
            IListViewFileSet listViewFileSet;
            if (!_dirs.ContainsKey(dir))
            {
                //_dirs[dir] = new ListViewFileSet(dir, new BackgroundWorker(), listView1, _filePatterns);
                listViewFileSet = new ListViewFileSet_BlockingLoadFilesAsyncLoadImages(dir,null ,null, _filePatterns);
                _dirs[dir] = listViewFileSet;
            }
            else
                listViewFileSet = _dirs[dir];
            return listViewFileSet;
        }


    }
}
