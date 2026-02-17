using OpenTK.Windowing.Desktop;
using OpenTKProject;

namespace opentkModel
{
    class Program
    {
        static int Width = 1024;
        static int Height = 768;
        static double maxFPS = 120.0f;

        static void Main(string[] args)
        {
            var gameWindowSettings = GameWindowSettings.Default;
            gameWindowSettings.UpdateFrequency = maxFPS;

            using (OpenWindow window = new OpenWindow(Width, Height, "ModelView")){
                window.Run();
            }
        }
    }
}