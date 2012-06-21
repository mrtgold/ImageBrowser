using System;
using System.Drawing;
using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogicTests.ImageProviders
{
    internal class MockImageProvider : IImageProvider
    {
        public IAsyncResult BeginGetImage(AsyncCallback callback, string filename)
        {
            return null;
        }

        public Image EndGetImage(IAsyncResult asyncResult)
        {
            return new Bitmap(100,100);
        }
    }
    internal class StubImageProvider : IImageProvider
    {
        private readonly AsyncImageFromFileCaller _imageGetter;

        protected delegate Image AsyncImageFromFileCaller(string filename);

        public StubImageProvider()
        {
            _imageGetter = GetImageGetter();
        }

        protected  AsyncImageFromFileCaller GetImageGetter()
        {
            return GetThumbnail;
        }

        private Image GetThumbnail(string filename)
        {
            return new Bitmap(100, 100);
        }

        public IAsyncResult BeginGetImage(AsyncCallback callback, string filename)
        {
            var result = _imageGetter.BeginInvoke(filename, callback, filename);

            return result;
        }

        public Image EndGetImage(IAsyncResult asyncResult)
        {

            var image = _imageGetter.EndInvoke(asyncResult);
            return image;
        }
    }
}