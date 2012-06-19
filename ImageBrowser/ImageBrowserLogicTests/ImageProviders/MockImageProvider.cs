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
}