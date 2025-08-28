using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

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

        public MainWindow()
        {
            InitializeComponent();
            atlas = new GameAtlas();
            sprites = new GameSprite[0];
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

        /// <summary>
        /// set atlas size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string width = atlas_width.Text;
            string height = atlas_height.Text;

            int sX = atlas != null ? (int)atlas.Size.X : 512;
            int sY = atlas != null ? (int)atlas.Size.Y : 512;

            atlasSize.X = NewSize(width, sX);
            atlasSize.Y = NewSize(height, sY);

            if (atlas != null)
                atlas.SetSize(atlasSize);
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
            ItemViewer.Children.Clear();
            ItemViewer.RowDefinitions.Clear();
            ItemViewer.ColumnDefinitions.Clear();

            int columns = 2;
            for (int i = 0; i < columns; i++)
            {
                ItemViewer.ColumnDefinitions.Add(new ColumnDefinition());
            }

            int rows = (sprites.Length + columns - 1) / columns;
            for (int i = 0; i < rows; i++)
            {
                ItemViewer.RowDefinitions.Add(new RowDefinition());
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

                ItemViewer.Children.Add(image);
            }
        }
    }
}