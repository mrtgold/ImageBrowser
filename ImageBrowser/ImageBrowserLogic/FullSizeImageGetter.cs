using System.Drawing;

namespace ImageBrowserLogic
{
    public class FullSizeImageGetter : ImageGetterBase
    {
        protected override AsyncImageFromFileCaller GetImageGetter()
        {
            return  Image.FromFile;
        }
    }
}