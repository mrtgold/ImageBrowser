using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ImageBrowserLogic;

namespace TestAsync
{
    public class ListViewFileSet_BlockingLoadAllImages : ListViewFileSetBase
    {
        public ListViewFileSet_BlockingLoadAllImages(DirectoryInfo dir, params string[] filePatterns)
            : base(dir, filePatterns)
        {
        }

        public override void BeginLoadingImages(ListView targetListView)
        {
            BlockingLoadAllImages(targetListView);
        }

        private void BlockingLoadAllImages(ListView targetListView)
        {
            targetListView.LargeImageList = ImageList;
            foreach (var node in this)
            {
                LoadImage(node, targetListView);
                Trace.WriteLine(string.Format("BlockingLoadAllImages:{0}: {1} remaining", Dir, FilesNotDone.Count()));
            }
        }

        private void LoadImage(FileNode node, ListView targetListView)
        {
            var key = node.File.FullName;
            var item = targetListView.Items.Add(key, node.File.Name, DefaultImageKey);
            node.BlockingLoadImage();
            ImageList.Images.Add(key, node.Image);
            item.ImageKey = key;
        }
    }
}