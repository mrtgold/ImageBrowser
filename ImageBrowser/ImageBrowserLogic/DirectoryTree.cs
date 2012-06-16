using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrowserResources;

namespace ImageBrowserLogic
{
    public class DirectoryTree : TreeView
    {
        public DirectoryTree()
        {
            //CollapseAll();
            ImageList = DirectoryBrowserImageList.GetImageList();
        }

        public delegate void DirectorySelectedHandler(DirectoryInfo dir);

        public DirectorySelectedHandler DirectorySelected;

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            var node = GetDirectoryNode(e.Node);

            node.BeforeExpand();

            base.OnBeforeExpand(e);
            node.UpdateImage();

        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            if (!e.Node.IsExpanded)
                e.Node.Expand();

            var node = GetDirectoryNode(e.Node);

            OnDirectorySelected(node.RootDir);
            base.OnBeforeSelect(e);
            node.UpdateImage();
        }

        private static DirectoryNode GetDirectoryNode(TreeNode treeNode)
        {
            var node = treeNode as DirectoryNode;
            if (node == null)
                throw new Exception(string.Format("huh?? node {0} is {1}", treeNode.Text, treeNode.GetType()));
            return node;
        }

        private void OnDirectorySelected(DirectoryInfo dir)
        {
            if (DirectorySelected != null)
                DirectorySelected(dir);
        }

        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            var node = GetDirectoryNode(e.Node);
            node.UpdateImage();

            base.OnAfterExpand(e);
        }

        public void InitDrives()
        {
            foreach (var d in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed))
            {
                Nodes.Add(new DirectoryNode(d));
            }
        }
    }
}
