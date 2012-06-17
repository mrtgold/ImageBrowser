using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using ImageBrowserLogic;
using ImageBrowserLogicTests.Properties;
using NUnit.Framework;

namespace ImageBrowserLogicTests
{
    [TestFixture]
    public class FullSizeImageGetterShould
    {
        private const string FileName = "silver-laptop-icon.jpg";
        private FileInfo _file;
        private Bitmap _originalImage;

        [SetUp]
        public void Setup()
        {
            _file = new FileInfo(Path.Combine(@".\Resources", FileName));
            _file.Refresh();
            Assert.IsTrue(_file.Exists);
            _originalImage = Resources.silver_laptop_icon;
        }

        [Test]
        public void GetImageBlocking()
        {
            var imageGetter = new FullSizeImageGetter();
            var result = imageGetter.BeginGetImage(null, _file.FullName);
            var actualImage = imageGetter.EndGetImage(result);

            AssertImagesAreEquivalent(_originalImage, actualImage);
            Assert.IsFalse(result.CompletedSynchronously);
        }

        [Test]
        public void GetImageRunCallback()
        {
            var imageGetter = new FullSizeImageGetter();
            var callbackCompleted = false;
            var done = new ManualResetEventSlim(false);
            var result = imageGetter.BeginGetImage(ar =>
                                    {
                                        Assert.IsNotNull(ar);
                                        Assert.IsTrue(ar.IsCompleted);
                                        var actualImage = imageGetter.EndGetImage(ar);

                                        AssertImagesAreEquivalent(_originalImage, actualImage);
                                        callbackCompleted = true;
                                        done.Set();
                                    }, _file.FullName);

            Assert.IsFalse(callbackCompleted);

            //result.AsyncWaitHandle.WaitOne(); //doesn't wait for callback complete, just callback init

            Assert.IsTrue(done.Wait(5000));
            Assert.IsFalse(result.CompletedSynchronously);
            Assert.IsTrue(callbackCompleted);
        }

        private static void AssertImagesAreEquivalent(Image expectedImage, Image actualImage)
        {
            var expectedBytes = GetBytes(expectedImage);
            var actualBytes = GetBytes(actualImage);

            CollectionAssert.AreEquivalent(expectedBytes, actualBytes);
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