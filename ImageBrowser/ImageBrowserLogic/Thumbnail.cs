using System.IO;

namespace ImageBrowserLogic
{
    public class Thumbnail
    {
        public string FileName { get; set; }

        public string ParentDir { get; set; }

        public Thumbnail(string fileName, string parentDir)
        {
            FileName = fileName;
            ParentDir = parentDir;
        }

        public override string ToString()
        {
            return Path.Combine(ParentDir, FileName);
        }
        #region Equality

        private bool Equals(Thumbnail other)
        {
            return string.Equals(FileName, other.FileName) && string.Equals(ParentDir, other.ParentDir);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Thumbnail)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((FileName != null ? FileName.GetHashCode() : 0) * 397) ^ (ParentDir != null ? ParentDir.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Thumbnail left, Thumbnail right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Thumbnail left, Thumbnail right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}