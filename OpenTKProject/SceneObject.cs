using OpenTK.Mathematics;
namespace OpenTKProject
{
    public class SceneObject
    {
        public Model Model { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public SceneObject(Model model, Vector3 position)
        {
            Model = model;
            Position = position;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
        }

        public SceneObject(Model model, Vector3 position, Vector3 scale)
        {
            Model = model;
            Position = position;
            Rotation = Vector3.Zero;
            Scale = scale;
        }

        public SceneObject(Model model, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Model = model;
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Matrix4 GetModelMatrix()
        {
            return Matrix4.CreateScale(Scale) *
                   Matrix4.CreateRotationX(Rotation.X) *
                   Matrix4.CreateRotationY(Rotation.Y) *
                   Matrix4.CreateRotationZ(Rotation.Z) *
                   Matrix4.CreateTranslation(Position);
        }
    }
}