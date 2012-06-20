using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DirectoryBrowser
{
    public interface IDirectoryTree
    {
        void InitDrives();
        void AfterExpanding(TreeViewEventArgs e);
        void BeforeSelecting(TreeViewCancelEventArgs e);
        void AfterSelecting(TreeViewEventArgs e);
        void OnDirectorySelected(DirectoryInfo dir);
        void BeforeExpanding(TreeViewCancelEventArgs e);
        DirectoryTree.DirectorySelectedHandler DirectorySelected { get; set; }
    }

    public class DirectoryTree : TreeView, IDirectoryTree
    {
        public DirectoryTree()
        {
            ImageList = DirectoryBrowserImageList.GetImageList();
        }

        public void InitDrives()
        {
            foreach (var d in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed))
            {
                Nodes.Add(new DirectoryNode(d));
            }
        }

        #region Events
        public delegate void DirectorySelectedHandler(DirectoryInfo dir);

        public DirectorySelectedHandler DirectorySelected { get; set; }

        public void BeforeExpanding(TreeViewCancelEventArgs e) { OnBeforeExpand(e); }
        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            var node = GetDirectoryNode(e.Node);

            node.BeforeExpand();

            base.OnBeforeExpand(e);
            UpdateImages(e.Node);
        }

        public void AfterExpanding(TreeViewEventArgs e) { OnAfterExpand(e); }
        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            base.OnAfterExpand(e);
            UpdateImages(e.Node);
        }

        public void BeforeSelecting(TreeViewCancelEventArgs e) { OnBeforeSelect(e); }
        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            if (!e.Node.IsExpanded)
                e.Node.Expand();

            base.OnBeforeSelect(e);
            UpdateImages(e.Node);
        }

        public void AfterSelecting(TreeViewEventArgs e) { OnAfterSelect(e); }
        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            var node = GetDirectoryNode(e.Node);
            OnDirectorySelected(node.RootDir);
            base.OnAfterSelect(e);

            UpdateImages(e.Node);
        }

        public void OnDirectorySelected(DirectoryInfo dir)
        {
            if (DirectorySelected != null)
                DirectorySelected(dir);
        }

        #endregion

        #region Helpers
        private static void UpdateImages(TreeNode treeNode)
        {
            var node = GetDirectoryNode(treeNode);
            node.UpdateImage();
        }

        private static DirectoryNode GetDirectoryNode(TreeNode treeNode)
        {
            var node = treeNode as DirectoryNode;
            if (node == null)
                throw new Exception(string.Format("huh?? node {0} is {1}", treeNode.Text, treeNode.GetType()));
            return node;
        }

        #endregion
    }
}
