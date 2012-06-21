using System;
using ImageBrowserLogic;
using ImageBrowserLogic.LoadingStrategies;
using ImageBrowserLogicTests.ImageProviders;
using NUnit.Framework;

namespace ImageBrowserLogicTests.LoadingStrategies
{
    [TestFixture]
    public class ListViewFileSetBlockingLoadAllImagesShould : ListViewFileSetTeater
    {
        private MockImageProviderFactory _factory;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            InitializeListView_WasCalled = false;
            _factory = new MockImageProviderFactory();
            _file1 = MakeImageFile();
        }

        [Test]
        public void Make()
        {
            var fileSet = new ListViewFileSet_BlockingLoadAllImages(TargetDirectory, _factory, InitializeListView, "*.jpg");
            Assert.IsTrue(InitializeListView_WasCalled);
            Assert.AreEqual(0, fileSet.ListView.Items.Count);
        }


        [Test]
        public void LoadFiles()
        {
            var fileSet = new ListViewFileSet_BlockingLoadAllImages(TargetDirectory, _factory, InitializeListView, "*.jpg");
            fileSet.BeginLoadingImages();

            Assert.IsTrue(fileSet.ListView.Items.ContainsKey(_file1));
            Assert.IsFalse(string.IsNullOrEmpty(fileSet.StatusMessage));
        }
    }
}