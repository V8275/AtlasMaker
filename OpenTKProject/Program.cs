using OpenTKProject;

namespace opentkModel
{
    class Program
    {
        static int Width = 1024;
        static int Height = 768;

        static void Main(string[] args)
        {
            using (OpenWindow window = new OpenWindow(Width, Height, "ModelView")){
                window.Run();
            }
        }
    }
}