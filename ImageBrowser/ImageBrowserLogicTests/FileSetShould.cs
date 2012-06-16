using System.IO;
using System.Linq;
using ImageBrowserLogic;
using NUnit.Framework;

namespace ImageBrowserLogicTests
{
    [TestFixture]
    public class FileSetShould : DirectoryTester
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            MakeFiles(TargetDirectory);
        }

        [Test]
        public void Make()
        {
            var fileSet = new FileSet(TargetDirectory);
            var expected = TargetDirectory.GetFiles().Select(f => new FileNode(f, fileSet));
            CollectionAssert.AreEquivalent(expected,fileSet);
        }

        [Test]
        public void PopulateMatchingFiles()
        {
            const string filePattern = "*.txt";
            var fileSet = new FileSet(TargetDirectory, filePattern);

            var expected = TargetDirectory.GetFiles(filePattern).Select(f => new FileNode(f, fileSet));
            CollectionAssert.AreEquivalent(expected, fileSet);
        }


        private static void MakeFiles(DirectoryInfo dir)
        {
            using (File.CreateText(Path.Combine(dir.FullName, "1.txt")))
            using (File.CreateText(Path.Combine(dir.FullName, "2.csv")))
            {
            }
        }

    }
}