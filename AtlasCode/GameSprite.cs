using System.Numerics;
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
    }
}