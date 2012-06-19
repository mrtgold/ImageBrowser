using System;
using System.Drawing;

namespace ImageBrowserLogic.ImageProviders
{
    public interface IImageProvider
    {
        IAsyncResult BeginGetImage(AsyncCallback callback, string filename);
        Image EndGetImage(IAsyncResult asyncResult);
    }
}