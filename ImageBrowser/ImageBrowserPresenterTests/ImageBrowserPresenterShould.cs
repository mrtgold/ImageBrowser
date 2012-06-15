using System.Linq;
using ImageBrowserLogic;
using ImageBrowserPresenter;
using NUnit.Framework;

namespace ImageBrowserPresenterTests
{
    [TestFixture]
    public class ImageBrowserPresenterShould
    {
        [Test]
        public void PresenterRegistersAsListenerForDirectorySelectedEvent()
        {
            IImageBrowserView view = new StubImageBrowserView();
            Assert.AreEqual(0, ((StubImageBrowserView)view).DirectorySelectedListenerCount);
            var presenter = new ImageBrowserPresenter.ImageBrowserPresenter(view);

            const int expectedListenersCount = 1;
            var actualListenersCount = ((StubImageBrowserView)view).DirectorySelectedListenerCount;
            Assert.AreEqual(expectedListenersCount, actualListenersCount);
        }

        [Test]
        public void PresenterRegistersAsListenerForLoadEvent()
        {
            IImageBrowserView view = new StubImageBrowserView();
            Assert.AreEqual(0, ((StubImageBrowserView)view).ImageBrowserViewLoadListenerCount);
            var presenter = new ImageBrowserPresenter.ImageBrowserPresenter(view);

            const int expectedListenersCount = 1;
            var actualListenersCount = ((StubImageBrowserView)view).ImageBrowserViewLoadListenerCount;
            Assert.AreEqual(expectedListenersCount, actualListenersCount);
        }

        [Test]
        public void ThumbnailDisplayerDataSourceSetDuringViewLoad()
        {
            IImageBrowserView view = new StubImageBrowserView();
            var presenter = new ImageBrowserPresenter.ImageBrowserPresenter(view);
            Assert.IsNull(view.ThumbnailDisplayer.DataSource);
            ((StubImageBrowserView)view).FireBrowserViewLoadEvent();
            Assert.IsNotNull(view.ThumbnailDisplayer.DataSource);
        }

        [Test]
        public void DisplayerDsSetToCurrentThumbnailsWhenDirectorySelectedEventFired()
        {
            IImageBrowserView view = new StubImageBrowserView();
            var presenter = new ImageBrowserPresenter.ImageBrowserPresenter(view);
            ((StubImageBrowserView)view).FireBrowserViewLoadEvent();

            const string dir = @"D:\dir1";
            view.Directory = dir;
            ((StubImageBrowserView)view).FireDirectorySelectedEvent();
            var expectedThumbnail = new Thumbnail("file1.jpg", dir);
            var thumbnails = (Thumbnails)view.ThumbnailDisplayer.DataSource;
            Assert.AreEqual(expectedThumbnail, thumbnails.Single());
        }
    }

    public class StubImageBrowserView : IImageBrowserView
    {
        private readonly IThumbnailDisplayer _thumbnailDisplayer;

        public StubImageBrowserView()
        {
            _thumbnailDisplayer = new StubThumbnailDisplayer();
        }

        public IThumbnailDisplayer ThumbnailDisplayer
        {
            get { return _thumbnailDisplayer; }
        }

        public string Directory { get; set; }

        public event ImageBrowserViewEventDelegate DirectorySelected;
        public event ImageBrowserViewEventDelegate BrowserViewLoad;

        public int ImageBrowserViewLoadListenerCount { get { return BrowserViewLoad != null ? BrowserViewLoad.GetInvocationList().Length : 0; } }

        public int DirectorySelectedListenerCount { get { return DirectorySelected != null ? DirectorySelected.GetInvocationList().Length : 0; } }


        public void FireBrowserViewLoadEvent()
        {
            if (BrowserViewLoad != null)
                BrowserViewLoad(this, this);
        }

        public void FireDirectorySelectedEvent()
        {
            if (DirectorySelected != null)
                DirectorySelected(this, this);
        }
    }

    class StubThumbnailDisplayer : IThumbnailDisplayer
    {
        private Thumbnails _dataSource;

        public Thumbnails DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }
    }

}