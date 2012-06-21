using System.IO;
using System.Windows.Forms;
using ImageBrowserLogic;
using ImageBrowserLogic.ImageProviders;
using ImageBrowserLogicTests.ImageProviders;
using NUnit.Framework;

namespace ImageBrowserLogicTests
{
    [TestFixture]
    public class ThumbnailSetsShould : DirectoryTester
    {
        private Panel _container;
        private readonly string[] _filePatterns = new[] { "*.jpg" };
        private StubFileSetFactory _fileSetFactory;
        private ListView _previousListView;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _container = new Panel();
            _fileSetFactory = new StubFileSetFactory();
            var newListView = _fileSetFactory.Build(TargetDirectory,_filePatterns);
            newListView.ListView = new ListView();

            _previousListView = new ListView();
            //container.Controls.Add(listView);

        }

        [Test]
        public void Make()
        {
            var thumbnailSets = new ThumbnailSets(_container, _filePatterns, _fileSetFactory);

            thumbnailSets.DisplayList(TargetDirectory, ref _previousListView);
        }

        [Test]
        public void MakeAgain()
        {
            var thumbnailSets = new ThumbnailSets(_container, _filePatterns, _fileSetFactory);

            thumbnailSets.DisplayList(TargetDirectory, ref _previousListView);
            thumbnailSets.DisplayList(TargetDirectory, ref _previousListView);
        }

        private class StubFileSetFactory : IListViewFileSetFactory
        {
            public StubFileSet FileSet;
            public IListViewFileSet Build(DirectoryInfo dir, string[] filePatterns)
            {
                return FileSet ?? (FileSet = new StubFileSet(dir, new StubImageProviderFactory(), filePatterns));
            }
        }

        private class StubFileSet : FileSet, IListViewFileSet
        {
            public StubFileSet(DirectoryInfo dir, IImageProviderFactory imageProviderFactory, params string[] filePatterns)
                : base(dir, imageProviderFactory, filePatterns)
            {
            }

            public void BeginLoadingImages()
            {

            }

            public ImageList ImageList { get; private set; }
            public ListView ListView { get; set; }
            public string StatusMessage { get; private set; }
            public int ImagesLoaded { get; private set; }
        }
    }
}