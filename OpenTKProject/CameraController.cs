using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKProject
{
    public class CameraController
    {
        private float _speed = 1.5f;

        public Vector3 Position { get; private set; }
        public Vector3 Front { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public float MouseSensitivity { get; set; } = 0.1f;

        public CameraController()
        {
            InitializeVectors();
        }

        public CameraController(float speed, Vector3 position)
        {
            _speed = speed;
            Position = position;
            InitializeVectors();
        }

        private void InitializeVectors()
        {
            Front = -Vector3.UnitZ;
            Up = Vector3.UnitY;
            Right = Vector3.UnitX;
            Yaw = -90.0f;
            Pitch = 0.0f;
            UpdateVectors();
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void Move(KeyboardState input, float deltaTime)
        {
            float velocity = _speed * deltaTime;

            if (input.IsKeyDown(Keys.W))
            {
                Position += Front * velocity; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                Position -= Front * velocity; // Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                Position -= Right * velocity; // Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                Position += Right * velocity; // Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                Position += Up * velocity; // Up
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                Position -= Up * velocity; // Down
            }
        }

        public void RotateCamera(float xOffset, float yOffset)
        {
            xOffset *= MouseSensitivity;
            yOffset *= MouseSensitivity;

            Yaw += xOffset;
            Pitch += yOffset;

            if (Pitch > 89.0f)
                Pitch = 89.0f;
            if (Pitch < -89.0f)
                Pitch = -89.0f;

            UpdateVectors();
        }

        private void UpdateVectors()
        {
            Vector3 newFront;
            newFront.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) *
                         MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            newFront.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
            newFront.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw)) *
                         MathF.Cos(MathHelper.DegreesToRadians(Pitch));

            Front = Vector3.Normalize(newFront);

            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }
    }
}