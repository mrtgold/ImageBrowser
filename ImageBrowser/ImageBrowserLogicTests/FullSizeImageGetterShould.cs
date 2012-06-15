using System;
using System.Drawing;
using System.IO;
using ImageBrowserLogic;
using ImageBrowserLogicTests.Properties;
using NUnit.Framework;

namespace ImageBrowserLogicTests
{
    [TestFixture]
    public class FullSizeImageGetterShould
    {
        [Test]
        public void GetImage()
        {
            const string filename = "silver-laptop-icon.jpg";
            var file = new FileInfo(Path.Combine(@".\Resources", filename));
            file.Refresh();
            Assert.IsTrue(file.Exists);
            var image = Resources.silver_laptop_icon;

            var getter = new FullSizeImageGetter();

            var result = getter.BeginGetImage(ar =>
                                     {
                                         Console.WriteLine("Read Completed");

                                         var getter1 = (FullSizeImageGetter)ar.AsyncState;
                                         Image returnedImage = getter1.EndGetImage(ar);
                                         Assert.AreEqual(image, returnedImage);

                                     }, file.FullName);
            result.AsyncWaitHandle.WaitOne();
//
        }
    }
}