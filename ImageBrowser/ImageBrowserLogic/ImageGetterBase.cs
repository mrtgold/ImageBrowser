using System;
using System.Drawing;

namespace ImageBrowserLogic
{
    public abstract class ImageGetterBase : IImageProvider
    {
        private readonly AsyncImageFromFileCaller _imageGetter;

        protected ImageGetterBase()
        {
            _imageGetter = GetImageGetter();
        }

        protected abstract AsyncImageFromFileCaller GetImageGetter();

        protected delegate Image AsyncImageFromFileCaller(string filename);

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