using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TextureMaker
{
    internal class GameSprite : GameItem
    {
        public GameSprite(string path, Vector2 s) {
            originalImage = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            _size = s;
        }

        public GameSprite(string path)
        {
            originalImage = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            _size.X = (float)originalImage.Width;
            _size.Y = (float)originalImage.Height;
        }

        public void Desaturate()
        {
            FormatConvertedBitmap grayscaleImage = new FormatConvertedBitmap();
            grayscaleImage.BeginInit();
            grayscaleImage.Source = originalImage;
            grayscaleImage.DestinationFormat = PixelFormats.Gray8;
            grayscaleImage.EndInit();

            BitmapImage newImage = new BitmapImage();
            using (var memoryStream = new System.IO.MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(grayscaleImage));
                encoder.Save(memoryStream);

                memoryStream.Position = 0;

                newImage.BeginInit();
                newImage.CacheOption = BitmapCacheOption.OnLoad;
                newImage.StreamSource = memoryStream;
                newImage.EndInit();
            }

            originalImage = newImage;
        }
    }
}