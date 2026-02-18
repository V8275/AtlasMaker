using OpenTK.Mathematics;

namespace OpenTKProject
{
    public class PathMover : SceneObject
    {
        Vector3[] points = new Vector3[4];
        private int currentPoint = 0;
        private float Speed = 1.0f;
        private float stopDist = 0.1f;

        public PathMover(Model model, Vector3 position) : base(model, position)
        {
        }
        public PathMover(Model model, Vector3 position, Vector3 scale) : base(model, position, scale)
        {
        }
        public PathMover(Model model, Vector3 position, Vector3 rotation, Vector3 scale) 
            : base(model, position, rotation, scale)
        {
        }

        private void LoadPositions()
        {
            points[0] = new Vector3(1, -1f, -1);
            points[1] = new Vector3(-1, -1f, -1);
            points[2] = new Vector3(-1, -1f, 1);
            points[3] = new Vector3(1, -1f, 1);
        }

        public override void Start()
        {
            base.Start();

            LoadPositions();
        }

        public override void Update(float time)
        {
            base.Update(time);

            if(points == null || points.Length == 0) return;

            Vector3 target = points[currentPoint];
            Vector3 direction = target - Position;
            float distance = direction.Length;

            if (distance < stopDist)
            {
                currentPoint = (currentPoint + 1) % points.Length;
            }
            else
            {
                direction.Normalize();
                Position += direction * Speed * time;
            }
        }

        public void SetPoints(Vector3[] p)
        {
            points = p;
            currentPoint = 0;
        }
    }
}
