namespace ImageBrowserPresenter
{
    public interface IImageBrowserView
    {
        event ImageBrowserViewEventDelegate DirectorySelected;
        event ImageBrowserViewEventDelegate BrowserViewLoad;

        IThumbnailDisplayer ThumbnailDisplayer { get; }
        string Directory { get; set; }
    }
}