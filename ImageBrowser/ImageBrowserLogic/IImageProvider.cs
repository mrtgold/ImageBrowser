using System;
using System.Drawing;

namespace ImageBrowserLogic
{
    public interface IImageProvider
    {
        IAsyncResult BeginGetImage(AsyncCallback callback, string filename);
        Image EndGetImage(IAsyncResult asyncResult);
    }
}