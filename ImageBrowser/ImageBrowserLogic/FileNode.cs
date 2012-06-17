using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImageBrowserLogic
{
    public class FileNode
    {
        public FileInfo File { get; set; }
        public FileSet ParentNode { get; set; }
        public Image Image { get; set; }

        public bool Done { get; set; }
        public string Key { get { return File.FullName; } }
        public IImageProvider ImageGetter { get; set; }

        public FileNode(FileInfo file, FileSet parent, Image defaultImage)
        {
            Done = false;
            File = file;
            ParentNode = parent;
            Image = defaultImage;
        }

        public void BlockingLoadImage()
        {
            var getter = new FullSizeImageGetter();

            var result = getter.BeginGetImage(null, File.FullName);
            Image = getter.EndGetImage(result);
            Done = true;

        }

        public void LoadImage()
        {
            var getter = new FullSizeImageGetter();

            var result = getter.BeginGetImage(EndGetImage, File.FullName);

            result.AsyncWaitHandle.WaitOne();
        }

        protected virtual void EndGetImage(IAsyncResult ar)
        {
            Console.WriteLine("Read Completed");

            var getter1 = (FullSizeImageGetter)ar.AsyncState;
            Image = getter1.EndGetImage(ar);
            Done = true;
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
            return Equals((FileNode)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((File != null ? File.GetHashCode() : 0) * 397) ^ (ParentNode != null ? ParentNode.GetHashCode() : 0);
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