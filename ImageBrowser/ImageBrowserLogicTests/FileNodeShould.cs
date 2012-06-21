using System.IO;
using ImageBrowserLogic;
using NUnit.Framework;

namespace ImageBrowserLogicTests
{
    [TestFixture]
    public class FileNodeShould :DirectoryTester
    {
        [Test]
        public void GiveFullNameAsKey()
        {
            var file1 = new FileInfo(@"C:\file1.txt");
            var node = new FileNode(file1, null, null, null);
            Assert.AreEqual(file1.FullName, node.Key);
        }

        [Test]
        public void ValidateEquality()
        {
            var file1 = new FileInfo(@"C:\file1.txt");
            var file2 = new FileInfo(@"C:\file2.txt");

            var one = new FileNode(file1, null, null, null);
            var oneRef = one;
            var oneToo = new FileNode(file1, null, null, null);
            var two = new FileNode(file2, null, null, null);

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
}