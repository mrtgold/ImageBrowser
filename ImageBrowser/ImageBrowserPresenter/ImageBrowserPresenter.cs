using ImageBrowserLogic;

namespace ImageBrowserPresenter
{
    public delegate void ImageBrowserViewEventDelegate(object sender, IImageBrowserView view);

    public class ImageBrowserPresenter
    {
        public ImageBrowserPresenter(IImageBrowserView view)
        {
            view.DirectorySelected += view_DirectorySelected;
            view.BrowserViewLoad += view_BrowserViewLoad;
        }

        void view_DirectorySelected(object sender, IImageBrowserView view)
        {
            var thumbnails = LoadThumbnails();
            thumbnails = thumbnails.GetFromDir(view.Directory);
            view.ThumbnailDisplayer.DataSource = thumbnails;
        }
        void view_BrowserViewLoad(object sender, IImageBrowserView view)
        {
            var thumbnails = LoadThumbnails();
            view.ThumbnailDisplayer.DataSource = thumbnails;
        }

        private static Thumbnails LoadThumbnails()
        {
            var thumbnails = new Thumbnails
                                 {
                                     new Thumbnail("file1.jpg", @"D:\dir1"),
                                     new Thumbnail("file2.jpg", @"D:\dir2")
                                 };

            return thumbnails;
        }
    }
}