using System.Linq;
using ImageBrowserLogic;
using NUnit.Framework;

namespace ImageBrowserLogicTests
{
    [TestFixture]
    public class ThumbnailShould
    {
        [Test]
        public void ValidateEquality()
        {
            var one = new Thumbnail("1","dir");
            var oneRef = one;
            var oneToo = new Thumbnail("1", "dir");
            var two = new Thumbnail("2", "dir2");

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

    }

    [TestFixture]
    public class ThumbnailsShould
    {
        [Test]
        public void Add()
        {
            const string fileName = "aFileName.jpg";
            const string parentDir = @"D:\parent\dir";

            var thumbnails = new Thumbnails {new Thumbnail(fileName, parentDir)};

            Assert.AreEqual(new Thumbnail(fileName, parentDir), thumbnails.Single());
        }

        [Test]
        public void GetForSpecifiedDir()
        {
            const string dir1 = @"D:\parent\dir";
            const string dir2 = @"D:\parent\otherDir";

            var thumbnails = new Thumbnails
                                 {
                                     new Thumbnail("1.jpg", dir1),
                                     new Thumbnail("2.jpg", dir1),
                                     new Thumbnail("3.jpg", dir2)
                                 };

            var fromDir = thumbnails.GetFromDir(dir1);
            Assert.IsInstanceOf<Thumbnails>(fromDir);
            Assert.AreEqual(2, fromDir.Count());

            
        }

    }
}