using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ImageBrowserLogic
{
    public class ThumbnailSets : Dictionary<DirectoryInfo, IListViewFileSet>
    {
        private readonly Control _container;
        private readonly string[] _filePatterns;
        private readonly IListViewFileSetFactory _fileSetFactory;

        public ThumbnailSets(Control container, string[] filePatterns, IListViewFileSetFactory fileSetFactory)
        {
            _container = container;
            _filePatterns = filePatterns;
            _fileSetFactory = fileSetFactory;
        }

        private IListViewFileSet GetListViewFileSet(DirectoryInfo dir)
        {
            if (!ContainsKey(dir))
            {
                var listViewFileSet = _fileSetFactory.Build(dir, _filePatterns);
                this[dir] = listViewFileSet;
                listViewFileSet.BeginLoadingImages();

                return listViewFileSet;
            }

            return this[dir];
        }

        public void DisplayList(DirectoryInfo dir, ref ListView previousListView)
        {
            var sw = Stopwatch.StartNew();
            var listViewFileSet = GetListViewFileSet(dir);

            if (_container == null || listViewFileSet.ListView == null) return;

            //if (!_container.Controls.Contains(listViewFileSet.ListView))
            if (!ReferenceEquals(previousListView, listViewFileSet.ListView))
            {
                _container.SuspendLayout();
                Trace.WriteLine(string.Format("after SuspendLayout: {0} msec", sw.ElapsedMilliseconds));

                _container.Controls.Add(listViewFileSet.ListView);
                Trace.WriteLine(string.Format("after add: {0} msec", sw.ElapsedMilliseconds));

                //foreach (var control in _container.Controls.OfType<ListView>().Where(c => !ReferenceEquals(c,listViewFileSet.ListView)))
                //{
                //    _container.Controls.Remove(control);
                //}

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