using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;

namespace TextureMaker
{
    static class BitmapSaver
    {
        public static void SaveBitmapWithDialog(BitmapSource bitmap)
        {
            if (bitmap == null)
            {
                MessageBox.Show("Ошибка: Нет данных для построения атласа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png|JPEG Image|*.jpg",
                Title = "Сохранить изображение",
                DefaultExt = ".png"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                string extension = Path.GetExtension(filePath).ToLower();

                if (extension == ".png")
                {
                    SaveBitmapToPng(bitmap, filePath);
                }
                else if (extension == ".jpg")
                {
                    SaveBitmapToJpg(bitmap, filePath);
                }
                else
                {
                    throw new NotSupportedException("Выбранный формат файла не поддерживается.");
                }
            }
        }

        private static void SaveBitmapToPng(BitmapSource bitmap, string filePath)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = File.Create(filePath))
            {
                encoder.Save(stream);
            }
        }

        private static void SaveBitmapToJpg(BitmapSource bitmap, string filePath, int quality = 90)
        {
            var encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = quality;
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = File.Create(filePath))
            {
                encoder.Save(stream);
            }
        }


    }
}
