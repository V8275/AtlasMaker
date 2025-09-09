using System.IO;
using System.Numerics;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace TextureMaker
{
    internal class GameItem
    {
        protected Vector2 _size;
        protected BitmapImage originalImage;

        public BitmapImage OriginalImage
        {
            get { return originalImage; }
            set { originalImage = value; }
        }

        public Vector2 Size
        {
            get { return _size; }
        }

        public virtual void SetSize(Vector2 size)
        {
            if (originalImage != null)
            {
                try
                {
                    _size = size;

                    // Создание масштабированного изображения
                    ScaleTransform scaleTransform = new ScaleTransform(_size.X / originalImage.PixelWidth, _size.Y / originalImage.PixelHeight);
                    TransformedBitmap resizedImage = new TransformedBitmap(originalImage, scaleTransform);

                    // Сохранение нового изображения
                    BitmapImage newImage = new BitmapImage();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(resizedImage));
                        encoder.Save(memoryStream);

                        newImage.BeginInit();
                        newImage.StreamSource = memoryStream;
                        newImage.CacheOption = BitmapCacheOption.OnLoad;
                        newImage.EndInit();
                    }

                    originalImage = newImage;
                }
                catch (Exception ex)
                {
                    // Обработка исключений
                    Console.WriteLine($"Ошибка при масштабировании изображения: {ex.Message}");
                }
            }
        }
    }
}