using System.Numerics;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace TextureMaker
{
    internal class GameAtlas : GameItem
    {
        private GameSprite[,] sprites;

        public GameSprite[,] Sprites 
        { 
            get { return sprites; } 
        }

        public GameAtlas(GameSprite[] gameSprites) 
        {
            SetGameSpritesByAtlas(gameSprites);
        }

        public GameAtlas() { }

        public void SetGameSpritesByAtlas(GameSprite[] gameSprites)
        {
            if(gameSprites != null && gameSprites.Length > 0)
            {
                int N = gameSprites.Length;

                int squareVal = (int)Math.Ceiling(Math.Sqrt(N));

                if (squareVal * gameSprites[0].Size.X > _size.X
                    || squareVal * gameSprites[0].Size.Y > _size.Y)
                {
                    var newX = squareVal * gameSprites[0].Size.X;
                    var newY = squareVal * gameSprites[0].Size.Y;

                    SetSize(new Vector2(newX, newY));
                }

                GameSprite[,] spritesTemp = new GameSprite[squareVal, squareVal];

                int num = 0;
                for (int i = 0; i < squareVal; i++)
                {
                    for (int j = 0; j < squareVal; j++)
                    {
                        if (num >= N) break;
                        spritesTemp[i, j] = gameSprites[num];
                        num++;
                    }
                    if (num >= N) break;
                }

                sprites = spritesTemp;
            }
        }

        public BitmapSource CreateAtlasBitmap()
        {
            if (sprites == null || sprites.Length == 0)
                return null;

            int rows = sprites.GetLength(0);
            int cols = sprites.GetLength(1);
            int spriteWidth = (int)sprites[0, 0].Size.X;
            int spriteHeight = (int)sprites[0, 0].Size.Y;

            var atlasBitmap = new RenderTargetBitmap(
                cols * spriteWidth,
                rows * spriteHeight,
                96, 96, PixelFormats.Pbgra32);

            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (sprites[i, j]?.OriginalImage == null)
                            continue;

                        double x = j * spriteWidth;
                        double y = i * spriteHeight;

                        drawingContext.DrawImage(
                            sprites[i, j].OriginalImage,
                            new Rect(x, y, spriteWidth, spriteHeight));
                    }
                }
            }

            atlasBitmap.Render(drawingVisual);
            return atlasBitmap;
        }
    }
}
