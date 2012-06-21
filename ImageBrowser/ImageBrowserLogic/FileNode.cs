using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogic
{
    public class FileNode
    {
        private readonly IImageProviderFactory _factory;
        public FileInfo File { get; private set; }
        public FileSet ParentNode { get; set; }
        public Image Image { get; private set; }

        public bool Done { get; private set; }
        public string Key { get { return File.FullName; } }

        private IImageProvider _imageGetter;
        public IImageProvider ImageGetter
        {
            get { return _imageGetter ?? (_imageGetter = _factory.Build()); }
        }

        public FileNode(FileInfo file, FileSet parent, Image defaultImage, IImageProviderFactory factory)
        {
            _factory = factory;
            Done = false;
            File = file;
            ParentNode = parent;
            Image = defaultImage;
        }

        public void BlockingLoadImage()
        {
            var result = ImageGetter.BeginGetImage(null, File.FullName);
            Image = ImageGetter.EndGetImage(result);
            Done = true;

        }

        public void BeginLoadImage()
        {
            ImageGetter.BeginGetImage(ar => EndLoadImage(ar, this), File.FullName);
        }


        delegate void EndLoadImageCallback(IAsyncResult result, FileNode node);
        private static void EndLoadImage(IAsyncResult result, FileNode node)
        {
            var fileSet = ((IListViewFileSet) node.ParentNode);
            if (fileSet.ListView.InvokeRequired)
            {
                var d = new EndLoadImageCallback(EndLoadImage);
                fileSet.ListView.Invoke(d, new object[] { result, node });
            }
            else
            {
                node.Image = node.ImageGetter.EndGetImage(result);
                fileSet.ImageList.Images.Add(node.Key, node.Image);

                fileSet.ListView.Items[node.Key].ImageKey = node.Key;
                node.Done = true;
                Trace.WriteLine(string.Format("Updated image file {0}", node.Key));
            }
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