namespace ImageBrowserLogic.ImageProviders
{
    public interface IImageProviderFactory
    {
        IImageProvider Build();
    }
}