using OpenTKProject;

namespace opentkModel
{
    class Program
    {
        static void Main(string[] args)
        {
            using (OpenWindow window = new OpenWindow(1024, 768, "ModelView")){
                window.Run();
            }
        }
    }
}