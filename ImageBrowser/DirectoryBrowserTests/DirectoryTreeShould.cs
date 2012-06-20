using System.Collections.Generic;
using System.IO;
using System.Linq;
using DirectoryBrowser;
using NUnit.Framework;

namespace DirectoryBrowserTests
{
    [TestFixture]
    public class DirectoryTreeShould : DirectoryTester
    {

        [Test]
        public void Make()
        {
            var dirTree = new DirectoryTree();
            CollectionAssert.IsEmpty(dirTree.Nodes);
        }

        [Test]
        public void InitializeDrives()
        {
            var dirTree = new DirectoryTree();

            var localDrives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed);
            var expectedNodes = localDrives.Select(d => new DirectoryNode(d));
            dirTree.InitDrives();

            CollectionAssert.IsNotEmpty(dirTree.Nodes);

            CollectionAssert.AreEquivalent(expectedNodes, dirTree.Nodes);
        }

        [Test]
        public void EmitDirectorySelected()
        {
            var dirTree = new DirectoryTree();
            dirTree.DirectorySelected += LogDirectorySelected;// listOfSelectedDirs.Add;

            dirTree.InitDrives();
            CollectionAssert.IsEmpty(_listOfSelectedDirs);

            Assert.IsNull(dirTree.SelectedNode);

            var firstNode = dirTree.Nodes[0] as DirectoryNode;
            Assert.IsNotNull(firstNode);
            dirTree.OnDirectorySelected(firstNode.RootDir);

            Assert.AreEqual(firstNode.RootDir, _listOfSelectedDirs.Single());
        }

        private readonly List<DirectoryInfo> _listOfSelectedDirs = new List<DirectoryInfo>();

        private void LogDirectorySelected(DirectoryInfo dir)
        {
            _listOfSelectedDirs.Add(dir);
        }
    }
}
