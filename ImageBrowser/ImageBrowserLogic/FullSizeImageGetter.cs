using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace ImageBrowserLogic
{
    public class FullSizeImageGetter : IImageProvider
    {
        private ImageGetterAsyncResult _result;

        public IAsyncResult BeginGetImage(AsyncCallback callback, string filename)
        {
            _result = new ImageGetterAsyncResult
                                        {
                                            FileName = filename,
                                            Image = Image.FromFile(filename),
                                            IsCompleted = true
                                        };

            _result.WaitHandle.Set();
            return _result;
        }

        public Image EndGetImage(IAsyncResult asyncResult)
        {
            // var fs = new FileStream("",FileMode.CreateNew).BeginRead(null,0,0,null,null);
            //  throw new NotImplementedException();
            var image = ((ImageGetterAsyncResult)asyncResult).Image;
            return image;
        }

        private class ImageGetterAsyncResult : IAsyncResult
        {
            internal readonly ManualResetEvent WaitHandle = new ManualResetEvent(false);
            public bool IsCompleted { get; set; }

            public WaitHandle AsyncWaitHandle { get { return WaitHandle; } }
            public object AsyncState
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            public bool CompletedSynchronously { get { throw new NotImplementedException(); } }
            public Image Image;
            public string FileName;
        }

    }
}