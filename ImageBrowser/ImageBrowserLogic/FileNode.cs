using System.IO;

namespace ImageBrowserLogic
{
    public class FileNode
    {
        public FileInfo File { get; set; }
        public FileSet ParentNode { get; set; }

        public bool Done { get; set; }

        public FileNode(FileInfo file, FileSet parent)
        {
            Done = false;
            File = file;
            ParentNode = parent;
        }

        #region Equality members

        protected bool Equals(FileNode other)
        {
            return File.FullName.Equals(other.File.FullName) && ReferenceEquals(ParentNode, other.ParentNode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FileNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((File != null ? File.GetHashCode() : 0)*397) ^ (ParentNode != null ? ParentNode.GetHashCode() : 0);
            }
        }

        public static bool operator ==(FileNode left, FileNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FileNode left, FileNode right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}