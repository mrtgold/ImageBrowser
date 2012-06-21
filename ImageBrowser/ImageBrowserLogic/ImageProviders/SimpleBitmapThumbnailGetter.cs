using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ImageBrowserLogic.ImageProviders
{
    public class SimpleBitmapThumbnailGetter : ImageGetterBase
    {
        public int ThumbnailSize { get; private set; }

        public SimpleBitmapThumbnailGetter(int thumbnailSizeInPixels)
        {
            ThumbnailSize = thumbnailSizeInPixels;
        }

        protected override AsyncImageFromFileCaller GetImageGetter()
        {
            return GetThumbnail;
        }

        public Image GetThumbnail(string filename)
        {
            using (var image = Image.FromFile(filename))
            {
                var thumb = GetThumbnail(image, ThumbnailSize);
                return thumb;
            }
        }


        public static Bitmap GetThumbnail(Image image, int thumbnailSize)
        {
            var targetShape = GetTargetShape(image.Width, image.Height, thumbnailSize);
            var sourceShape = new Rectangle(new Point(), image.Size);

            var thumb = new Bitmap(thumbnailSize, thumbnailSize, PixelFormat.Format24bppRgb);
            using (var graphics = Graphics.FromImage(thumb))
            {
                graphics.Clear(Color.White);
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.DrawImage(image, targetShape, sourceShape, GraphicsUnit.Pixel);

                return thumb;
            }
        }

        public static Rectangle GetTargetShape(int origWidth, int origHeight, int thumbnailSize)
        {
            Size targetSize;
            if (origHeight <= thumbnailSize && origWidth <= thumbnailSize)
                targetSize = new Size(origWidth, origHeight);
            else
            {
                var aspectRatio = (double)origWidth / origHeight;

                var isWide = aspectRatio >= 1.0;

                var targetHeight = isWide ? (int)(thumbnailSize / aspectRatio) : thumbnailSize;
                var targetWidth = isWide ? thumbnailSize : (int)(thumbnailSize * aspectRatio);
                targetSize = new Size(targetWidth, targetHeight);
            }

            var tx = (thumbnailSize - targetSize.Width) / 2;
            var ty = (thumbnailSize - targetSize.Height) / 2;
            var targetStart = new Point(tx, ty);
            var targetShape = new Rectangle(targetStart, targetSize);
            return targetShape;
        }
    }
}