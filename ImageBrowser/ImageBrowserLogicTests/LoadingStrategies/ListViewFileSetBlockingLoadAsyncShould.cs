using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ImageBrowserLogic.ImageProviders;
using ImageBrowserLogic.LoadingStrategies;
using ImageBrowserLogicTests.ImageProviders;
using ImageBrowserLogicTests.Properties;
using NUnit.Framework;

namespace ImageBrowserLogicTests.LoadingStrategies
{
    [TestFixture]
    public class ListViewFileSetBlockingLoadAsyncShould : DirectoryTester
    {
        private bool InitializeListView_WasCalled;
        private IImageProviderFactory _factory;
        private string _file1;
        private ListViewFileSet_BlockingLoadFilesAsyncLoadImages _fileSet;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            InitializeListView_WasCalled = false;
            _factory = new StubImageProviderFactory();
            _file1 = MakeImageFile();

            _fileSet = new ListViewFileSet_BlockingLoadFilesAsyncLoadImages(TargetDirectory, _factory, InitializeListView, "*.jpg");
        }

        [Test]
        public void Make()
        {
            Assert.IsTrue(InitializeListView_WasCalled);
            Assert.AreEqual(0, _fileSet.ListView.Items.Count);
        }


        [Test]
        public void LoadFiles()
        {
            _fileSet.BeginLoadingImages();
            int loops = 0;
            while(!_fileSet.Done)
            {
                loops++;
                Thread.Sleep(100);
            }

            Assert.IsTrue(_fileSet.ListView.Items.ContainsKey(_file1));
            Assert.LessOrEqual(0,loops);
        }


        private void InitializeListView(ListView listView)
        {
            InitializeListView_WasCalled = true;
        }

        private string MakeImageFile()
        {
            var file1 = Path.Combine(TargetDirectory.FullName, Path.GetRandomFileName() + ".jpg");
            Resources.silver_laptop_icon.Save(file1, ImageFormat.Jpeg);

            return file1;
        }

    }
}