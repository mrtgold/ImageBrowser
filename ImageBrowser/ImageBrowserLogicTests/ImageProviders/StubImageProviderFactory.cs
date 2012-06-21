using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogicTests.ImageProviders
{
    internal class StubImageProviderFactory : IImageProviderFactory
    {
        public StubImageProvider Provider;

        public IImageProvider Build()
        {
            return Provider ?? (Provider = new StubImageProvider());
        }
    }
}