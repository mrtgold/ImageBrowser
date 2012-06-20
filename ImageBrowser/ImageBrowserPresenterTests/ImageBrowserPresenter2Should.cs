using System.IO;
using System.Windows.Forms;
using DirectoryBrowser;
using ImageBrowserLogicTests;
using ImageBrowserPresenter;
using NUnit.Framework;

namespace ImageBrowserPresenterTests
{
    [TestFixture]
    public class ImageBrowserPresenter2Should : DirectoryTester
    {
        private IDirectoryTree _directoryTree;
        private IImageBrowserView2 _view;
        private ImageBrowserPresenter2 _presenter;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _view = new StubImageBrowserView2();
            _directoryTree = new StubDirectoryTree();
            ((StubImageBrowserView2) _view).DirectoryTree = _directoryTree;
        }

        [Test]
        public void PresenterRegistersAsListenerForDirectorySelectedEvent()
        {
            Assert.AreEqual(0, ((StubImageBrowserView2)_view).DirectorySelectedListenerCount);
            var presenter = new ImageBrowserPresenter2(_view);

            const int expectedListenersCount = 1;
            var actualListenersCount = ((StubImageBrowserView2)_view).DirectorySelectedListenerCount;
            Assert.AreEqual(expectedListenersCount, actualListenersCount);
        }

        [Test]
        public void PresenterRegistersAsListenerForLoadEvent()
        {
            Assert.AreEqual(0, ((StubImageBrowserView2)_view).ImageBrowserViewLoadListenerCount);
            var presenter = new ImageBrowserPresenter2(_view);

            const int expectedListenersCount = 1;
            var actualListenersCount = ((StubImageBrowserView2)_view).ImageBrowserViewLoadListenerCount;
            Assert.AreEqual(expectedListenersCount, actualListenersCount);
        }

        [Test]
        public void InitDirectoryDuringViewLoad()
        {
            var presenter = new ImageBrowserPresenter2(_view);

            Assert.IsFalse(((StubDirectoryTree)_directoryTree).InitDrives_Called);
            ((StubImageBrowserView2)_view).FireBrowserViewLoadEvent();
            Assert.IsTrue(((StubDirectoryTree)_directoryTree).InitDrives_Called);
        }

        [Test]
        public void PopulateAndSwapListViewsWhenDirectorySelectedEventFired()
        {
            _presenter = new ImageBrowserPresenter2(_view);
            ((StubImageBrowserView2)_view).FireBrowserViewLoadEvent();

            const string dir = ".";
            ((StubImageBrowserView2)_view).DirSelected = new DirectoryInfo(dir);
            ((StubImageBrowserView2)_view).FireDirectorySelectedEvent();

        }

        [Test]
        public void UpdateStatus()
        {
            _presenter = new ImageBrowserPresenter2(_view);
            ((StubImageBrowserView2)_view).FireBrowserViewLoadEvent();

            const string dir = ".";
            ((StubImageBrowserView2)_view).DirSelected = new DirectoryInfo(dir);

        }
    }

    class StubImageBrowserView2 : IImageBrowserView2
    {
        public ListView ListView;
        public string UpdateDirStatus_Message;
        public string UpdateAppStatus_Message;
        public string UpdateFilesStatus_Message;

        public event ImageBrowserViewDirectorySelectedHandler DirectorySelected;
        public event ImageBrowserViewEventDelegate2 BrowserViewLoad;
        public Control ListViewParentContainer { get; private set; }
        public IDirectoryTree DirectoryTree { get; set; }

        public void OnDirectorySelected(DirectoryInfo dir)
        {
        }

        public void UpdateDirStatus(string msg)
        {
            UpdateDirStatus_Message = msg;
        }

        public void UpdateAppStatus(string msg)
        {
            UpdateAppStatus_Message = msg;
        }

        public void UpdateFilesStatus(string msg)
        {
            UpdateFilesStatus_Message = msg;
        }

        public void InitializeListView(ListView listView)
        {
            listView.Name = "Initialized by StubImageBrowserView2";
        }

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
                DirectorySelected(this, DirSelected, ref ListView);
        }

        public DirectoryInfo DirSelected { get; set; }
    }
}