using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogicTests.ImageProviders
{
    internal class MockImageProviderFactory : IImageProviderFactory
    {
        public MockImageProvider Provider;

        public IImageProvider Build()
        {
            return Provider ?? (Provider = new MockImageProvider());
        }
    }
}