using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TestAsync
{
    public class ThumbnailSets : Dictionary<DirectoryInfo, IListViewFileSet>
    {
        private readonly Control _container;
        private readonly Action<ListView> _initializeListView;
        private readonly string[] _filePatterns;

        public ThumbnailSets(Control container, Action<ListView> initializeListView, string[] filePatterns)
        {
            _container = container;
            _initializeListView = initializeListView;
            _filePatterns = filePatterns;
        }

        private IListViewFileSet GetListViewFileSet(DirectoryInfo dir)
        {
            if (!ContainsKey(dir))
            {
                var listViewFileSet = new ListViewFileSet_BlockingLoadFilesAsyncLoadImages(dir, _filePatterns);
                this[dir] = listViewFileSet;
                _initializeListView(listViewFileSet.ListView);
                listViewFileSet.BeginLoadingImages();

                return listViewFileSet;
            }

            return this[dir];
        }

        public void DisplayList(DirectoryInfo dir, ref ListView previousListView)
        {
            var sw = Stopwatch.StartNew();
            var listViewFileSet = GetListViewFileSet(dir);

            if (!ReferenceEquals(previousListView, listViewFileSet.ListView))
            {
                _container.SuspendLayout();
                Trace.WriteLine(string.Format("after SuspendLayout: {0} msec", sw.ElapsedMilliseconds));

                _container.Controls.Add(listViewFileSet.ListView);
                Trace.WriteLine(string.Format("after add: {0} msec", sw.ElapsedMilliseconds));

                _container.Controls.Remove(previousListView);
                Trace.WriteLine(string.Format("after remove: {0} msec", sw.ElapsedMilliseconds));

                _container.ResumeLayout(true);
                Trace.WriteLine(string.Format("after ResumeLayout: {0} msec", sw.ElapsedMilliseconds));
                previousListView = listViewFileSet.ListView;
            }

            sw.Stop();

            Trace.WriteLine(string.Format("loaded dir {0} in {1} msec", dir.FullName, sw.ElapsedMilliseconds));
        }
    }
}