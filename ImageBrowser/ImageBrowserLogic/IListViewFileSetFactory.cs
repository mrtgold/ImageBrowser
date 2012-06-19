using System.IO;

namespace ImageBrowserLogic
{
    public interface IListViewFileSetFactory
    {
        IListViewFileSet Build(DirectoryInfo dir, string[] filePatterns);
    }
}