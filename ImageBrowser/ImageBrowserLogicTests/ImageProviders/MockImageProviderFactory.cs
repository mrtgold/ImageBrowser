using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogicTests.ImageProviders
{
    internal class MockImageProviderFactory : IImageProviderFactory
    {
        public IImageProvider Build()
        {
            return new MockImageProvider();
        }
    }
}