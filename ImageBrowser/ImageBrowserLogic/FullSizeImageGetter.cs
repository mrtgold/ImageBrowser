using System;
using System.Drawing;

namespace ImageBrowserLogic
{
    public class FullSizeImageGetter : IImageProvider
    {
        private readonly AsyncImageFromFileCaller _imageGetter;

        public FullSizeImageGetter()
        {
            _imageGetter = Image.FromFile;
        }

        private delegate Image AsyncImageFromFileCaller(string filename);

        public IAsyncResult BeginGetImage(AsyncCallback callback, string filename)
        {
            var result = _imageGetter.BeginInvoke(filename, callback,filename);

            return result;
        }

        public Image EndGetImage(IAsyncResult asyncResult)
        {

            var image = _imageGetter.EndInvoke(asyncResult);
            return image;
        }

    }
}