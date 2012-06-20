using System.IO;
using System.Windows.Forms;
using DirectoryBrowser;

namespace ImageBrowserPresenterTests
{
    class StubDirectoryTree : IDirectoryTree
    {
        public bool InitDrives_Called;

        public void InitDrives()
        {
            InitDrives_Called = true;
        }

        public void AfterExpanding(TreeViewEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public void BeforeSelecting(TreeViewCancelEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public void AfterSelecting(TreeViewEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public void OnDirectorySelected(DirectoryInfo dir)
        {
            throw new System.NotImplementedException();
        }

        public void BeforeExpanding(TreeViewCancelEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public DirectoryTree.DirectorySelectedHandler DirectorySelected { get; set; }
    }
}