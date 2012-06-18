using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TestAsync
{
    public class ThumbnailSets : Dictionary<DirectoryInfo, IListViewFileSet>
    {
        private readonly string[] _filePatterns;

        public ThumbnailSets(string[] filePatterns)
        {
            _filePatterns = filePatterns;
        }

        public IListViewFileSet GetListViewFileSet(DirectoryInfo dir, Action<ListView> initializeListView)
        {
            IListViewFileSet listViewFileSet;
            if (!ContainsKey(dir))
            {
                listViewFileSet = new ListViewFileSet_BlockingLoadFilesAsyncLoadImages(dir, _filePatterns) { ListView = new ListView() };
                initializeListView(listViewFileSet.ListView);
                this[dir] = listViewFileSet;
                listViewFileSet.BeginLoadingImages(listViewFileSet.ListView);
            }
            else
                listViewFileSet = this[dir];

            return listViewFileSet;
        }

    }
}