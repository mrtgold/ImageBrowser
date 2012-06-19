using System.Drawing;

namespace ImageBrowserLogic.ImageProviders
{
    public class FullSizeImageGetter : ImageGetterBase
    {
        protected override AsyncImageFromFileCaller GetImageGetter()
        {
            return  Image.FromFile;
        }
    }
}