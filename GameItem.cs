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
        }

        public Vector2 Size
        {
            get { return _size; }
        }

        public virtual void SetSize(Vector2 size)
        {
            if(originalImage != null)
            {
                _size = size;
                ScaleTransform scaleTransform = new ScaleTransform(_size.X, _size.Y);
                TransformedBitmap resizedImage = new TransformedBitmap(originalImage, scaleTransform);

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
        }
    }
      
}