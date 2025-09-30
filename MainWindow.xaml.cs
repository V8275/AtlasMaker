using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using TextureMaker.Core;

namespace TextureMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Vector2 imageSize;
        private Vector2 atlasSize = new Vector2(512, 512);

        private GameAtlas atlas;
        private GameSprite[] sprites;
        RGBCombineManager rgbManager;

        public MainWindow()
        {
            InitializeComponent();
            atlas = new GameAtlas();
            sprites = new GameSprite[0];
            rgbManager = new RGBCombineManager();
        }

        /// <summary>
        /// set images size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string width = img_width.Text;
            string height = img_height.Text;

            if(sprites != null)
            {
                foreach (GameSprite sprite in sprites)
                {
                    int sX = sprite != null ? (int)sprite.Size.X : 512;
                    int sY = sprite != null ? (int)sprite.Size.Y : 512;
                    if (sprite != null)
                    {
                        imageSize.X = NewSize(width, sX);
                        imageSize.Y = NewSize(height, sY);

                        sprite.SetSize(imageSize);
                    }
                }
            }
        }

        private void SetImages(object sender, RoutedEventArgs e)
        {
            SelectFiles();
            ShowImages();
            atlas = new GameAtlas(sprites);
            atlas.SetSize(atlasSize);
        }

        private void BuildAtlas(object sender, RoutedEventArgs e)
        {
            if (atlas != null && sprites != null)
            {
                atlas.SetGameSpritesByAtlas(sprites);
                var result = atlas.CreateAtlasBitmap();

                SaveResult(result);
            }
        }

        private void SaveResult(BitmapSource res)
        {
            BitmapSaver.SaveBitmapWithDialog(res);
        }

        private int NewSize(string newSizeValue, int oldValue)
        {
            string result = Regex.Replace(newSizeValue, "[^0-9]", "");

            return String.IsNullOrEmpty(result) ?
                 oldValue : int.Parse(result);
        }

        private void SelectFiles()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() {
                Title = "Выберите файлы",
                Filter = "Все файлы (*.*)|*.png|*.jpg|*.jpeg",
                Multiselect = true
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true) 
            {
                string[] selectedFiles = openFileDialog.FileNames;

                sprites = new GameSprite[selectedFiles.Length];

                for (int i = 0; i < selectedFiles.Length; i++) 
                {
                    sprites[i] = new GameSprite(selectedFiles[i]);
                }
            }
        }

        private void ShowImages()
        {
            ImageArray.Children.Clear();
            ImageArray.RowDefinitions.Clear();
            ImageArray.ColumnDefinitions.Clear();

            int columns = 5;
            for (int i = 0; i < columns; i++)
            {
                ImageArray.ColumnDefinitions.Add(new ColumnDefinition());
            }

            int rows = (sprites.Length + columns - 1) / columns;
            for (int i = 0; i < rows; i++)
            {
                ImageArray.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < sprites.Length; i++)
            {
                var bitmap = sprites[i];
                var image = new Image
                {
                    Source = bitmap.OriginalImage,
                    Stretch = Stretch.Uniform
                };

                Grid.SetRow(image, i / columns);
                Grid.SetColumn(image, i % columns);

                ImageArray.Children.Add(image);
            }
        }

        private void ShowImage(Image img, BitmapImage image)
        {
            img.Source = image;
            img.Stretch = Stretch.Uniform;
        }

        private void SetRedChannel(object sender, RoutedEventArgs e)
        {
            SetChannel(Channels.Red);
        }

        private void SetGreenChannel(object sender, RoutedEventArgs e)
        {
            SetChannel(Channels.Green);
        }

        private void SetBlueChannel(object sender, RoutedEventArgs e)
        {
            SetChannel(Channels.Blue);
        }

        private void SetAlphaChannel(object sender, RoutedEventArgs e)
        {
            SetChannel(Channels.Alpha);
        }

        private void SetChannel(Channels channel)
        {
            rgbManager.SetImage(channel);
            var img = rgbManager.GetImage(channel);

            switch (channel)
            {
                case Channels.Red:
                    ShowImage(Red, img);
                    break;
                case Channels.Green:
                    ShowImage(Green, img);
                    break;
                case Channels.Blue:
                    ShowImage(Blue, img);
                    break;
                case Channels.Alpha:
                    ShowImage(Alpha, img);
                    break;
                default:
                    break;
            }
        }

        private void Render_Click(object sender, RoutedEventArgs e)
        {
            var result = rgbManager.CombineRGBA();

            if (result != null)
            {
                ShowImage(Result, result);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            rgbManager.SaveResult();
        }
    }
}