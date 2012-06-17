using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageBrowserLogic
{
    public interface IFileSet : IList<FileNode>
    {
    }

    public class FileSet : List<FileNode>, IFileSet
    {
        public DirectoryInfo Dir { get; set; }

        public FileSet(DirectoryInfo dir, params string[] filePatterns)
        {
            Dir = dir;
            var fileInfos = new List<FileInfo>();

            try
            {
                if (filePatterns == null || !filePatterns.Any())
                    fileInfos.AddRange(Dir.GetFiles());
                else
                {
                    foreach (var filePattern in filePatterns)
                        fileInfos.AddRange(Dir.GetFiles(filePattern));
                }

                foreach (var fileNode in fileInfos.Select(file => new FileNode(file, this, BrowserResources.Properties.Resources.Image_File)))
                {
                    Add(fileNode);
                }
            }
            catch
            {
            }
        }

        public IEnumerable<FileNode> FilesNotDone
        {
            get { return this.Where(f => !f.Done); }
        }

        public bool Done
        {
            get { return !FilesNotDone.Any(); }
        }

    }
}