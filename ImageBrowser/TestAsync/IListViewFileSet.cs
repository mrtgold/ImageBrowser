using System.Windows.Forms;
using ImageBrowserLogic;

namespace TestAsync
{
    public interface IListViewFileSet : IFileSet
    {
        void BeginLoadingImages(ListView targetListView);
        ImageList ImageList { get; }
        ListView ListView { get; set; }
    }
}