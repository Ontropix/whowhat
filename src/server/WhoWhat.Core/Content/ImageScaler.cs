using System;
using System.Drawing;
using System.IO;

namespace WhoWhat.Core.Content
{
    public static class ImageScaler
    {
        public static ScalledImage ScaleImage(Stream originalStream, int maxWidth, int maxHeight)
        {
            Image image = Image.FromStream(originalStream);

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

            var stream = new MemoryStream();
            newImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            stream.Position = 0;

            return new ScalledImage()
            {
                Stream = stream,
                Height = newHeight,
                Width = newWidth
            };
        }
    }

    public class ScalledImage
    {
        public Stream Stream { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
