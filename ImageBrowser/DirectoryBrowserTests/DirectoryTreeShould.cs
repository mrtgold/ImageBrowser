using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DirectoryBrowser;
using NUnit.Framework;

namespace DirectoryBrowserTests
{
    [TestFixture]
    public class DirectoryTreeShould : DirectoryTester
    {
        private List<DirectoryInfo> _listOfSelectedDirs;
        private DirectoryTree _dirTree;

        private void LogDirectorySelected(DirectoryInfo dir) { _listOfSelectedDirs.Add(dir); }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _listOfSelectedDirs = new List<DirectoryInfo>();
            _dirTree = new DirectoryTree();
            _dirTree.DirectorySelected += LogDirectorySelected;// listOfSelectedDirs.Add;
        }

        [Test]
        public void Make()
        {
            CollectionAssert.IsEmpty(_dirTree.Nodes);
        }

        [Test]
        public void InitializeDrives()
        {
            var localDrives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed);
            var expectedNodes = localDrives.Select(d => new DirectoryNode(d));
            _dirTree.InitDrives();

            CollectionAssert.IsNotEmpty(_dirTree.Nodes);

            CollectionAssert.AreEquivalent(expectedNodes, _dirTree.Nodes);
        }

        [Test]
        public void EmitDirectorySelectedOnDirectorySelected()
        {

            _dirTree.InitDrives();
            CollectionAssert.IsEmpty(_listOfSelectedDirs);

            Assert.IsNull(_dirTree.SelectedNode);

            var firstNode = _dirTree.Nodes[0] as DirectoryNode;
            Assert.IsNotNull(firstNode);
            _dirTree.OnDirectorySelected(firstNode.RootDir);

            Assert.AreEqual(firstNode.RootDir, _listOfSelectedDirs.Single());
        }


        [Test]
        public void PukeIfNotDirNodeBeforeExpanding()
        {
            _dirTree.DirectorySelected += LogDirectorySelected;// listOfSelectedDirs.Add;

            _dirTree.InitDrives();
            CollectionAssert.IsEmpty(_listOfSelectedDirs);
            var otherNode = new TreeNode("bad node");
            _dirTree.Nodes.Add(otherNode);
            var eventArgs = new TreeViewCancelEventArgs(otherNode, false, TreeViewAction.Expand);

            Assert.Throws<Exception>(() => _dirTree.BeforeExpanding(eventArgs));
        }

        [Test]
        public void RunNodeBeforeExpandNodeBeforeExpanding()
        {
            _dirTree.InitDrives();
            var spyNode = new SpyDirNode(TargetDirectory, (DirectoryNode)_dirTree.Nodes[0]);
            _dirTree.Nodes.Add(spyNode);
            var eventArgs = new TreeViewCancelEventArgs(spyNode, false, TreeViewAction.Expand);

            Assert.IsFalse(spyNode.BeforeExpand_WasCalled);
            Assert.IsFalse(spyNode.UpdateImage_WasCalled);
            _dirTree.BeforeExpanding(eventArgs);
            Assert.IsTrue(spyNode.BeforeExpand_WasCalled);
            Assert.IsTrue(spyNode.UpdateImage_WasCalled);
        }

        [Test]
        public void UpdateImageAfterExpanding()
        {
            _dirTree.InitDrives();
            var spyNode = new SpyDirNode(TargetDirectory, (DirectoryNode)_dirTree.Nodes[0]);
            _dirTree.Nodes.Add(spyNode);
            var eventArgs = new TreeViewEventArgs(spyNode, TreeViewAction.Expand);

            Assert.IsFalse(spyNode.UpdateImage_WasCalled);
            _dirTree.AfterExpanding(eventArgs);
            Assert.IsTrue(spyNode.UpdateImage_WasCalled);
        }

        [Test]
        public void UpdateImageAfterSelecting()
        {
            _dirTree.InitDrives();
            var spyNode = new SpyDirNode(TargetDirectory, (DirectoryNode)_dirTree.Nodes[0]);
            _dirTree.Nodes.Add(spyNode);
            var eventArgs = new TreeViewEventArgs(spyNode, TreeViewAction.ByKeyboard);

            Assert.IsFalse(spyNode.UpdateImage_WasCalled);
            _dirTree.AfterSelecting(eventArgs);
            Assert.IsTrue(spyNode.UpdateImage_WasCalled);
        }

        [Test]
        public void ExpandNodeAndUpdateImageBeforeSelecting()
        {
            _dirTree.InitDrives();
            var spyNode = new SpyDirNode(TargetDirectory, (DirectoryNode)_dirTree.Nodes[0]);
            _dirTree.Nodes.Add(spyNode);
            var eventArgs = new TreeViewCancelEventArgs(spyNode,false, TreeViewAction.ByKeyboard);

            Assert.IsFalse(spyNode.IsExpanded);
            Assert.IsFalse(spyNode.UpdateImage_WasCalled);
            _dirTree.BeforeSelecting(eventArgs);
            Assert.IsTrue(spyNode.UpdateImage_WasCalled);
            Assert.IsTrue(spyNode.IsExpanded);
        }

        private class SpyDirNode : DirectoryNode
        {
            public bool BeforeExpand_WasCalled;
            public bool UpdateImage_WasCalled;

            public SpyDirNode(DirectoryInfo rootDir, DirectoryNode parentNode)
                : base(rootDir, parentNode)
            {
            }

            public override void BeforeExpand()
            {
                BeforeExpand_WasCalled = true;
            }
            public override void UpdateImage()
            {
                UpdateImage_WasCalled = true;
            }
        }

    }
}
