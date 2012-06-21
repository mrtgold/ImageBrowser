using ImageBrowserLogic.ImageProviders;
using NUnit.Framework;

namespace ImageBrowserLogicTests.ImageProviders
{
    [TestFixture]
    public class SimpleBitmapThumbnailGetterFactoryShould
    {
        [Test]
        public void ShouldBuild()
        {
            const int size = 100;
            var factory = new SimpleBitmapThumbnailGetterFactory(size);
            var getter = factory.Build();
            Assert.IsInstanceOf<SimpleBitmapThumbnailGetter>(getter);
            Assert.AreEqual(size, ((SimpleBitmapThumbnailGetter)getter).ThumbnailSize);
        }
    }
}