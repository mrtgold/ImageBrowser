using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TestAsync
{
    public class ThumbnailSets : Dictionary<DirectoryInfo, IListViewFileSet>
    {
        public Control Container { get; private set; }
        private readonly Action<ListView> _initializeListView;
        private readonly string[] _filePatterns;

        public ThumbnailSets(Control container, Action<ListView> initializeListView, string[] filePatterns)
        {
            Container = container;
            _initializeListView = initializeListView;
            _filePatterns = filePatterns;
        }

        private IListViewFileSet GetListViewFileSet(DirectoryInfo dir)
        {
            IListViewFileSet listViewFileSet;
            if (!ContainsKey(dir))
            {
                listViewFileSet = new ListViewFileSet_BlockingLoadFilesAsyncLoadImages(dir, _filePatterns) { ListView = new ListView() };
                _initializeListView(listViewFileSet.ListView);
                this[dir] = listViewFileSet;
                listViewFileSet.BeginLoadingImages(listViewFileSet.ListView);
            }
            else
                listViewFileSet = this[dir];

            return listViewFileSet;
        }

        public void DisplayList(DirectoryInfo dir, ref ListView previousListView)
        {
            var sw = Stopwatch.StartNew();
            var listViewFileSet = GetListViewFileSet(dir);

            if (!ReferenceEquals(previousListView, listViewFileSet.ListView))
            {
                Container.SuspendLayout();
                Trace.WriteLine(string.Format("after SuspendLayout: {0} msec", sw.ElapsedMilliseconds));

                Container.Controls.Add(listViewFileSet.ListView);
                Trace.WriteLine(string.Format("after add: {0} msec", sw.ElapsedMilliseconds));

                Container.Controls.Remove(previousListView);
                Trace.WriteLine(string.Format("after remove: {0} msec", sw.ElapsedMilliseconds));

                Container.ResumeLayout(true);
                Trace.WriteLine(string.Format("after ResumeLayout: {0} msec", sw.ElapsedMilliseconds));
                previousListView = listViewFileSet.ListView;
            }

            sw.Stop();

            Trace.WriteLine(string.Format("loaded dir {0} in {1} msec", dir.FullName, sw.ElapsedMilliseconds));
        }
    }
}