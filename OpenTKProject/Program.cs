using OpenTKProject;

namespace opentkModel
{
    class Program
    {
        static void Main(string[] args)
        {
            using (OpenWindow window = new OpenWindow(512, 512, "ModelView")){
                window.Run();
            }
        }
    }
}