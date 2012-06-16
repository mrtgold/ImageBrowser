using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageBrowserLogic
{
    public class FileSet : List<FileNode>
    {
        public FileSet(DirectoryInfo dir, params string[] filePatterns)
        {
            var fileInfos = new List<FileInfo>();

            try
            {
                if (filePatterns == null || !filePatterns.Any())
                    fileInfos.AddRange(dir.GetFiles());
                else
                {
                    foreach (var filePattern in filePatterns)
                        fileInfos.AddRange(dir.GetFiles(filePattern));
                }

                foreach (var fileNode in fileInfos.Select(file => new FileNode(file, this)))
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