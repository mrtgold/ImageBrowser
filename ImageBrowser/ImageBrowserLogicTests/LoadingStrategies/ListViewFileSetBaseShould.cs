using System;
using System.IO;
using System.Windows.Forms;
using ImageBrowserLogic.ImageProviders;
using ImageBrowserLogic.LoadingStrategies;
using NUnit.Framework;

namespace ImageBrowserLogicTests.LoadingStrategies
{
    [TestFixture]
    public class ListViewFileSetBaseShould
    {
        [Test]
        public void ReturnZeroForNoItems()
        {
            var fileSet = new SpyListViewFileSet();
            Assert.AreEqual(0, fileSet.ImagesLoaded);
        }

        [Test]
        public void ReturnZeroForNullList()
        {
            var fileSet = new SpyListViewFileSet {ListView = null};
            Assert.AreEqual(0, fileSet.ImagesLoaded);
        }

        [Test]
        public void ReturnNullStatusMessageForNullList()
        {
            var fileSet = new SpyListViewFileSet {ListView = null};
            Assert.IsNull(fileSet.StatusMessage);
        }

        private class SpyListViewFileSet : ListViewFileSetBase
        {
            public bool BeginLoadingImages_WasCalled;

            public SpyListViewFileSet(DirectoryInfo dir = null, Action<ListView> initializeListView = null, IImageProviderFactory imageProviderFactory = null, params string[] filePatterns)
                : base(dir, initializeListView, imageProviderFactory, filePatterns)
            {
            }

            public override void BeginLoadingImages()
            {
                BeginLoadingImages_WasCalled = true;
            }
        }
    }
}