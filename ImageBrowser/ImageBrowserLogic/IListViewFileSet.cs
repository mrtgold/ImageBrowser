using System.Windows.Forms;

namespace ImageBrowserLogic
{
    public interface IListViewFileSet : IFileSet
    {
        void BeginLoadingImages();
        ImageList ImageList { get; }
        ListView ListView { get; set; }
    }
}