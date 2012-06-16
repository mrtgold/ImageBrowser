using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageBrowserLogic;
using NUnit.Framework;

namespace ImageBrowserLogicTests
{
    [TestFixture]
    public class DirectoryNodeShould : DirectoryTester
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            MakeSubDirs(TargetDirectory);
        }

        [Test]
        public void Make()
        {
            var node = new DirectoryNode(TargetDirectory, null);
            Assert.AreEqual(TargetDirectory, node.RootDir);
            Assert.AreEqual(TargetDirectory.Name, node.Text);
        }

        [Test]
        public void PopulateSubDirs()
        {
            var node = new DirectoryNode(TargetDirectory, null);
            node.PopulateSubDirs();

            var expected = TargetDirectory.GetDirectories().Select(d => new DirectoryNode(d, node));
            CollectionAssert.AreEquivalent(expected, node.SubDirs);

        }

        [Test]
        public void ThisLevelEnumeratedOnlyAfterSubDirsEnumerated()
        {
            var node = new DirectoryNode(TargetDirectory, null);
            Assert.IsFalse(node.ThisLevelEnumerated);
            node.PopulateSubDirs();
            Assert.IsTrue(node.ThisLevelEnumerated);
        }


        [Test]
        public void AllChildrenEnumeratedOnlyAfterAllSubDirsAllChildrenEnumerated()
        {
            DirectoryInfo dir = TargetDirectory.GetDirectories().First();
            MakeSubDirs(dir);
            var node = new DirectoryNode(TargetDirectory, null);
            Assert.IsFalse(node.AllChildrenEnumerated);
            node.PopulateSubDirs();
            Assert.IsFalse(node.AllChildrenEnumerated);

            foreach (var subDir in node.SubDirs)
            {
                subDir.PopulateSubDirs();
            }
            Assert.IsFalse(node.AllChildrenEnumerated);

            foreach (var subDir in node.SubDirs)
            {
                foreach (var subSubDir in subDir.SubDirs)
                {
                    subSubDir.PopulateSubDirs();
                }
            }
            Assert.IsTrue(node.AllChildrenEnumerated);

        }


        [Test]
        public void ValidateEquality()
        {
            var one = new DirectoryNode(TargetDirectory, null);
            var oneRef = one;
            var oneToo = new DirectoryNode(TargetDirectory, null);
            var two = new DirectoryNode(TargetDirectory.Parent, null);

            Assert.IsTrue(one.Equals(oneRef));
            oneRef = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.IsFalse(one.Equals(oneRef));
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.AreEqual(one, oneToo);
            Assert.AreEqual(one.GetHashCode(), oneToo.GetHashCode());
            Assert.AreEqual(one.ToString(), oneToo.ToString());

            Assert.AreNotEqual(one, two);
            var shouldBeTrue = one == oneToo;
            Assert.IsTrue(shouldBeTrue);

            var shouldBeFalse = one != oneToo;
            Assert.IsFalse(shouldBeFalse);

            Assert.IsFalse(one.Equals(new object()));

        }

        [Test]
        public void SetNodeType()
        {
            Assert.AreEqual(DirectoryNode.TreeViewNodeType.Computer, new DirectoryNode(null,null).NodeType);
            Assert.AreEqual(DirectoryNode.TreeViewNodeType.Drive, new DirectoryNode(new DirectoryInfo(@"C:\"), null).NodeType);
            Assert.AreEqual(DirectoryNode.TreeViewNodeType.Folder, new DirectoryNode(TargetDirectory, null).NodeType);
        }

        private static void MakeSubDirs(DirectoryInfo dir)
        {
            dir.CreateSubdirectory("1");
            dir.CreateSubdirectory("2");
        }
    }
}