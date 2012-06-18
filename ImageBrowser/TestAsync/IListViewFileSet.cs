using System.Windows.Forms;
using ImageBrowserLogic;

namespace TestAsync
{
    public interface IListViewFileSet : IFileSet
    {
        void BeginLoadingImages();
        ImageList ImageList { get; }
        ListView ListView { get; set; }
    }
}