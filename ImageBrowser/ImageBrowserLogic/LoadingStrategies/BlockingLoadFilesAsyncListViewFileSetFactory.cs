using System;
using System.IO;
using System.Windows.Forms;
using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogic.LoadingStrategies
{
    public class BlockingLoadFilesAsyncListViewFileSetFactory : IListViewFileSetFactory
    {
        private readonly IImageProviderFactory _factory;
        private readonly Action<ListView> _initializeListView;

        public BlockingLoadFilesAsyncListViewFileSetFactory(IImageProviderFactory factory, Action<ListView> initializeListView)
        {
            _factory = factory;
            _initializeListView = initializeListView;
        }

        public IListViewFileSet Build(DirectoryInfo dir, string[] filePatterns)
        {
            var fileSet = new ListViewFileSet_BlockingLoadFilesAsyncLoadImages(dir, _factory, _initializeListView, filePatterns);
            return fileSet;
        }
    }
}