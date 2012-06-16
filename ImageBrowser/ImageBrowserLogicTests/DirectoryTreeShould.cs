using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImageBrowserLogic;
using NUnit.Framework;

namespace ImageBrowserLogicTests
{
    [TestFixture]
    public class DirectoryTreeShould : DirectoryTester
    {
        private List<DirectoryInfo> _listOfSelectedDirs = new List<DirectoryInfo>();

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
            dirTree.SelectedNode = firstNode;
            dirTree.ExpandAll();

            Assert.AreSame(firstNode,dirTree.SelectedNode);
            Assert.AreEqual(firstNode.RootDir, _listOfSelectedDirs.Single());
        }

        private void LogDirectorySelected(DirectoryInfo dir)
        {
            _listOfSelectedDirs.Add(dir);
        }
    }
}
