using ImageBrowserLogic.LoadingStrategies;
using ImageBrowserLogicTests.ImageProviders;
using NUnit.Framework;

namespace ImageBrowserLogicTests.LoadingStrategies
{
    [TestFixture]
    public class BlockingLoadFilesAsyncListViewFileSetFactoryShould : ListViewFileSetTeater
    {
        [Test]
        public void Build()
        {
            var factory = new BlockingLoadFilesAsyncListViewFileSetFactory(new StubImageProviderFactory(),InitializeListView );
            var fileSet = factory.Build(TargetDirectory,null);

            Assert.IsInstanceOf<ListViewFileSet_BlockingLoadFilesAsyncLoadImages>(fileSet);
        }
    }
}