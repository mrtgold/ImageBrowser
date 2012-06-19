using System.IO;
using System.Linq;
using ImageBrowserLogic;
using ImageBrowserLogic.ImageProviders;
using ImageBrowserLogicTests.ImageProviders;
using NUnit.Framework;

namespace ImageBrowserLogicTests
{
    [TestFixture]
    public class FileSetShould : DirectoryTester
    {
        private IImageProviderFactory _imageProviderFactory;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _imageProviderFactory = new MockImageProviderFactory();
            MakeFiles(TargetDirectory);
        }

        [Test]
        public void Make()
        {
            var fileSet = new FileSet(TargetDirectory,_imageProviderFactory);
            var expected = TargetDirectory.GetFiles().Select(f => new FileNode(f, fileSet, BrowserResources.Properties.Resources.Image_File,_imageProviderFactory));
            CollectionAssert.AreEquivalent(expected,fileSet);
        }

        [Test]
        public void PopulateMatchingFiles()
        {
            const string filePattern = "*.txt";
            var fileSet = new FileSet(TargetDirectory, _imageProviderFactory, filePattern);

            var expected = TargetDirectory.GetFiles(filePattern).Select(f => new FileNode(f, fileSet, BrowserResources.Properties.Resources.Image_File, _imageProviderFactory));
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