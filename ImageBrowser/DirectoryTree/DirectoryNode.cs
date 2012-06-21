using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DirectoryBrowser
{
    public class DirectoryNode : TreeNode
    {
        public enum TreeViewNodeType
        {
            Computer = 1,
            Drive,
            Folder
        }

        public DirectoryInfo RootDir { get; private set; }
        private DirectoryNode ParentNode { get; set; }
        public bool HasError { get; private set; }
        private DirectoryBrowserImageList.TreeViewImages _expandedImageKey;
        private DirectoryBrowserImageList.TreeViewImages _collapsedImageKey;
        public IEnumerable<DirectoryNode> SubDirs { get { return Nodes.OfType<DirectoryNode>().ToList(); } }

        public bool ThisLevelEnumerated { get; private set; }

        public bool AllChildrenEnumerated
        {
            get
            {
                return ThisLevelEnumerated
                    && SubDirs.Aggregate(true, (current, subDir) => current && subDir.AllChildrenEnumerated);
            }
        }

        public TreeViewNodeType NodeType
        {
            get
            {
                if (RootDir == null)
                    return TreeViewNodeType.Computer;
                if (RootDir.Root.FullName == RootDir.FullName)
                    return TreeViewNodeType.Drive;
                return TreeViewNodeType.Folder;
            }
        }

        public DirectoryNode(DirectoryInfo rootDir, DirectoryNode parentNode)
            : base(rootDir == null ? "" : rootDir.Name)
        {
            ThisLevelEnumerated = false;
            RootDir = rootDir;
            ParentNode = parentNode;
            Nodes.Add(new TreeNode()); // add dummy node to allow expansion
            SetImages();
        }

        public DirectoryNode(DriveInfo drive)
            : this(drive.RootDirectory, null)
        {
            Nodes.Add(new TreeNode()); // add dummy node to allow expansion
            SetImages();
        }

        public void PopulateSubDirs()
        {
            Nodes.Clear();
            try
            {
                foreach (var subDir in RootDir.GetDirectories())
                {
                    Nodes.Add(new DirectoryNode(subDir, this));
                }
            }
            catch
            {
                HasError = true;
                SetImages();
                UpdateImage();
            }
            finally
            {
                ThisLevelEnumerated = true;
                UpdateImage();
            }
        }

        public override string ToString()
        {
            return RootDir.FullName;
        }

        public virtual void BeforeExpand()
        {
            if (!ThisLevelEnumerated)
            {
                PopulateSubDirs();
            }
        }

        public virtual void UpdateImage()
        {
            var imageKey = IsExpanded ? _expandedImageKey : _collapsedImageKey;
            ImageKey = imageKey.ToString();
            SelectedImageKey = imageKey.ToString();
        }

        private void SetImages()
        {
            if (HasError)
                _expandedImageKey = _collapsedImageKey = DirectoryBrowserImageList.TreeViewImages.Warning;
            else if (NodeType == TreeViewNodeType.Computer)
                _expandedImageKey = _collapsedImageKey = DirectoryBrowserImageList.TreeViewImages.Computer;
            else if (NodeType == TreeViewNodeType.Drive)
                _expandedImageKey = _collapsedImageKey = DirectoryBrowserImageList.TreeViewImages.Drive;
            else if (NodeType == TreeViewNodeType.Folder)
            {
                _expandedImageKey = DirectoryBrowserImageList.TreeViewImages.OpenFolder;
                _collapsedImageKey = DirectoryBrowserImageList.TreeViewImages.ClosedFolder;
            }
        }

        #region Equality members

        private bool Equals(DirectoryNode other)
        {
            return RootDir.FullName.Equals(other.RootDir.FullName)
                   && ReferenceEquals(ParentNode, other.ParentNode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DirectoryNode)obj);
        }

        public override int GetHashCode()
        {
            return (RootDir != null ? RootDir.GetHashCode() : 0);
        }

        public static bool operator ==(DirectoryNode left, DirectoryNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DirectoryNode left, DirectoryNode right)
        {
            return !Equals(left, right);
        }

        #endregion

    }
}

