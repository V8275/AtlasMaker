using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace TextureMaker.Core
{
    class RGBCombineManager
    {
        public GameItem Red;
        public GameItem Green;
        public GameItem Blue;
        public GameItem Alpha;

        public RGBCombineManager() {}
    }

    static class ImageEditor
    {
        public static GameItem ConvertToGrayscale(GameItem item)
        {
            BitmapImage source = item.OriginalImage;

            int width = source.PixelWidth;
            int height = source.PixelHeight;

            int stride = width * 4;
            byte[] pixelData = new byte[height * stride];

            source.CopyPixels(pixelData, stride, 0);

            for (int i = 0; i < pixelData.Length; i += 4)
            {
                byte blue = pixelData[i];
                byte green = pixelData[i + 1];
                byte red = pixelData[i + 2];

                byte gray = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);

                pixelData[i] = gray;     // Blue
                pixelData[i + 1] = gray; // Green
                pixelData[i + 2] = gray; // Red
            }

            BitmapSource grayscaleImage = BitmapSource.Create(
                width, height, 96, 96, PixelFormats.Bgra32, null, pixelData, stride);

            item.OriginalImage = ConvertToBitmapImage(grayscaleImage);

            return item;
        }

        public static BitmapImage ConvertToBitmapImage(BitmapSource bitmapSource)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                encoder.Save(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }
}
