namespace ImageBrowserLogic.ImageProviders
{
    public class SimpleBitmapThumbnailGetterFactory : IImageProviderFactory
    {
        private readonly int _thumbnailSizeInPixels;

        public SimpleBitmapThumbnailGetterFactory(int thumbnailSizeInPixels)
        {
            _thumbnailSizeInPixels = thumbnailSizeInPixels;
        }

        public IImageProvider Build()
        {
            return new SimpleBitmapThumbnailGetter(_thumbnailSizeInPixels);
        }
    }
}