using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKProject
{
    public class CameraController
    {
        float speed = 1.5f;

        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);

        public CameraController() { }

        public CameraController(float s)
        {
            speed = s;
        }

        public void SetSpeed(float s)
        {
            speed = s;
        }

        public void Move(KeyboardState input, float time, ref Vector3 position)
        {
            if (input.IsKeyDown(Keys.W))
            {
                position += front * speed * time; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                position -= front * speed * time; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * time; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * time; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                position += up * speed * time; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                position -= up * speed * time; //Down
            }
        }
    }
}
