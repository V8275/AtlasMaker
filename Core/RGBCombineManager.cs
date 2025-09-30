using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using Microsoft.Win32;
using System.Windows;

namespace TextureMaker.Core
{
    class RGBCombineManager
    {
        private GameSprite Red;
        private GameSprite Green;
        private GameSprite Blue;
        private GameSprite Alpha;
        private BitmapImage resultTexture;

        public BitmapImage ResultTexture
        {
            get { return resultTexture; }
        }

        public RGBCombineManager() { }

        public BitmapImage GetImage(Channels channel)
        {
            //if (Red == null || Green == null || Blue == null || Alpha == null)
            //{
            //    MessageBox.Show("Textures not set", "Warning", MessageBoxButton.OK);
            //    return null;
            //}

            switch (channel)
            {
                case Channels.Red:
                    return Red.OriginalImage;
                case Channels.Green:
                    return Green.OriginalImage;
                case Channels.Blue:
                    return Blue.OriginalImage;
                case Channels.Alpha:
                    return Alpha.OriginalImage;
                default:
                    return Alpha.OriginalImage;
            }
        }

        public void SetImage(Channels channel)
        {
            switch (channel)
            {
                case Channels.Red:
                    Red = SetWBImage(Red);
                    break;
                case Channels.Green:
                    Green = SetWBImage(Green);
                    break;
                case Channels.Blue:
                    Blue = SetWBImage(Blue);
                    break;
                case Channels.Alpha:
                    Alpha = SetWBImage(Alpha);
                    break;
                default:
                    break;
            }
        }

        public void SaveResult()
        {
            if (resultTexture != null)
            {
                BitmapSaver.SaveBitmapWithDialog(resultTexture, "PNG Image|*.png");
            }
        }

        public BitmapImage CombineRGBA()
        {
            BitmapImage redImage = Red?.OriginalImage;
            BitmapImage greenImage = Green?.OriginalImage;
            BitmapImage blueImage = Blue?.OriginalImage;
            BitmapImage alphaImage = Alpha?.OriginalImage;

            if (redImage == null || greenImage == null || blueImage == null)
            {
                MessageBox.Show("Not all of Channels are selected", "Warning", MessageBoxButton.OK);
                return null;
            }

            int width = redImage.PixelWidth;
            int height = redImage.PixelHeight;

            if (greenImage.PixelWidth != width || greenImage.PixelHeight != height ||
        blueImage.PixelWidth != width || blueImage.PixelHeight != height ||
        (alphaImage != null && (alphaImage.PixelWidth != width || alphaImage.PixelHeight != height)))
            {
                MessageBox.Show("Textures haven`t same resolution", "Warning", MessageBoxButton.OK);
                return null;
            }

            FormatConvertedBitmap redBgra = new FormatConvertedBitmap(redImage, PixelFormats.Bgra32, null, 0);
            FormatConvertedBitmap greenBgra = new FormatConvertedBitmap(greenImage, PixelFormats.Bgra32, null, 0);
            FormatConvertedBitmap blueBgra = new FormatConvertedBitmap(blueImage, PixelFormats.Bgra32, null, 0);
            FormatConvertedBitmap alphaBgra = alphaImage != null ?
                new FormatConvertedBitmap(alphaImage, PixelFormats.Bgra32, null, 0) : null;

            int stride = width * 4;
            byte[] outImg = new byte[height * stride];

            byte[] redData = new byte[height * stride];
            byte[] greenData = new byte[height * stride];
            byte[] blueData = new byte[height * stride];
            byte[] alphaData = alphaBgra != null ? new byte[height * stride] : null;

            redBgra.CopyPixels(redData, stride, 0);
            greenBgra.CopyPixels(greenData, stride, 0);
            blueBgra.CopyPixels(blueData, stride, 0);
            if (alphaBgra != null)
            {
                alphaBgra.CopyPixels(alphaData, stride, 0);
            }

            for (int i = 0; i < outImg.Length; i += 4)
            {
                outImg[i] = blueData[i + 2];
                outImg[i + 1] = greenData[i + 2];
                outImg[i + 2] = redData[i + 2];
                outImg[i + 3] = alphaBgra != null ? alphaData[i + 2] : (byte)255;
            }

            BitmapSource combinedImage = BitmapSource.Create(
                width, height, 96, 96, PixelFormats.Bgra32, null, outImg, stride);


            resultTexture = ConvertToBitmapImage(combinedImage);

            return resultTexture;
        }

        private GameSprite SetWBImage(GameSprite item)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Выберите файлы",
                Filter = "Все файлы (*.*)|*.png",
                Multiselect = true
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string selectedFile = openFileDialog.FileName;

                item = new GameSprite(selectedFile);
            }

            if (item != null)
            {
                item.Desaturate();
                return item;
            }
            else
            {
                MessageBox.Show("Texture not set", "Warning", MessageBoxButton.OK);
            }
            return null;
        }

        private BitmapImage ConvertToBitmapImage(BitmapSource bitmapSource)
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

    public enum Channels
    {
        Red,
        Green,
        Blue,
        Alpha
    }
}
