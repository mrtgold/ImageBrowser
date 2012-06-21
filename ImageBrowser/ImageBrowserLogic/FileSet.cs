using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageBrowserLogic.ImageProviders;

namespace ImageBrowserLogic
{
    public class FileSet : List<FileNode>, IFileSet
    {
        public DirectoryInfo Dir { get; private set; }

        public FileSet(DirectoryInfo dir, IImageProviderFactory imageProviderFactory, params string[] filePatterns)
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

                foreach (var fileNode in fileInfos.Select(file => new FileNode(file, this, BrowserResources.Properties.Resources.Image_File,imageProviderFactory)))
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