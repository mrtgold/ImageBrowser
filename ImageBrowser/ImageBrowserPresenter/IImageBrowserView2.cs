using System.IO;
using System.Windows.Forms;
using DirectoryBrowser;

namespace ImageBrowserPresenter
{
    public interface IImageBrowserView2
    {
        event ImageBrowserViewDirectorySelectedHandler DirectorySelected;
        event ImageBrowserViewEventDelegate2 BrowserViewLoad;
        Control ListViewParentContainer { get; }
        IDirectoryTree DirectoryTree { get; }

        void InitializeListView(ListView listView);
        void OnDirectorySelected(DirectoryInfo dir);
        void UpdateDirStatus(string msg);
        void UpdateAppStatus(string msg);
        void UpdateFilesStatus(string msg);
    }
}