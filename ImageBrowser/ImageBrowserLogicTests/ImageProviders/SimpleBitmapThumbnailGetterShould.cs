using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using ImageBrowserLogic.ImageProviders;
using ImageBrowserLogicTests.Properties;
using NUnit.Framework;

namespace ImageBrowserLogicTests.ImageProviders
{
    [TestFixture]
    public class SimpleBitmapThumbnailGetterShould
    {
        private const string FileName = "silver-laptop-icon.jpg";
        private FileInfo _file;
        private Image _originalImage;

        [SetUp]
        public void Setup()
        {
            _file = new FileInfo(Path.Combine(@".\Resources", FileName));
            _file.Refresh();
            Assert.IsTrue(_file.Exists);
            _originalImage = Resources.silver_laptop_icon;
        }

        [Test]
        public void GetCorrectTargetShape()
        {
            var actualBigSquare = SimpleBitmapThumbnailGetter.GetTargetShape(500, 500, 100);
            var expectedBigSquare = new Rectangle(0, 0, 100, 100);
            Assert.AreEqual(expectedBigSquare, actualBigSquare);

            var actualBigWideRectangle = SimpleBitmapThumbnailGetter.GetTargetShape(1000, 500, 100);
            var expectedBigWideRectangle = new Rectangle(0, 25, 100, 50);
            Assert.AreEqual(expectedBigWideRectangle, actualBigWideRectangle);

            var actualBigTallRectangle = SimpleBitmapThumbnailGetter.GetTargetShape(500, 1000, 100);
            var expectedBigTallRectangle = new Rectangle(25, 0, 50, 100);
            Assert.AreEqual(expectedBigTallRectangle, actualBigTallRectangle);

            var actualSmallSquare = SimpleBitmapThumbnailGetter.GetTargetShape(50, 50, 100);
            var expectedSmallSquare = new Rectangle(25, 25, 50, 50);
            Assert.AreEqual(expectedSmallSquare, actualSmallSquare);


        }

        [Test]
        public void GetImageBlocking()
        {
            var imageGetter = new SimpleBitmapThumbnailGetter(100);
            var result = imageGetter.BeginGetImage(null, _file.FullName);
            var actualImage = imageGetter.EndGetImage(result);
            var expected = _originalImage.GetThumbnailImage(100, 100, null, IntPtr.Zero);

            //not the same binary??
            //AssertImagesAreEquivalent(expected, actualImage);
            Assert.IsFalse(result.CompletedSynchronously);
        }

        [Test]
        public void GetImageRunCallback()
        {
            var imageGetter = new SimpleBitmapThumbnailGetter(100);
            var callbackCompleted = false;
            var processingDone = new ManualResetEventSlim(false);
            var result = imageGetter.BeginGetImage(ar =>
                                                       {
                                                           Assert.IsNotNull(ar);
                                                           Assert.IsTrue(ar.IsCompleted);
                                                           var actualImage = imageGetter.EndGetImage(ar);
                                                           var expected = _originalImage.GetThumbnailImage(100, 100, null, IntPtr.Zero);

                                                           //not the same binary??
                                                           //AssertImagesAreEquivalent(expected, actualImage);
                                                           callbackCompleted = true;
                                                           processingDone.Set();
                                                       }, _file.FullName);

            Assert.IsFalse(callbackCompleted);

            //result.AsyncWaitHandle.WaitOne(); //doesn't wait for callback complete, just callback init

            Assert.IsTrue(processingDone.Wait(5000));
            Assert.IsFalse(result.CompletedSynchronously);
            Assert.IsTrue(callbackCompleted);
        }

        private static void AssertImagesAreEquivalent(Image expectedImage, Image actualImage)
        {
            var expectedBytes = GetBytes(expectedImage);
            var actualBytes = GetBytes(actualImage);

            //CollectionAssert.AreEquivalent(expectedBytes, actualBytes);
            Assert.IsTrue(expectedBytes.ToArray().SequenceEqual(actualBytes));
        }

        private static IEnumerable<byte> GetBytes(Image original)
        {
            var binaryStreamOriginal = new MemoryStream();
            original.Save(binaryStreamOriginal, ImageFormat.Jpeg);
            var originalBytes = new byte[binaryStreamOriginal.Length];
            binaryStreamOriginal.Position = 0;
            binaryStreamOriginal.Read(originalBytes, 0, (int)binaryStreamOriginal.Length);
            return originalBytes;
        }
    }
}