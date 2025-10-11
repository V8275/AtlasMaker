using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKProject
{
    public class OpenWindow : GameWindow
    {
        Model SceneModel;

        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;

        CameraController cameraController;

        private float _lastX;
        private float _lastY;
        private bool _firstMove = true;

        public OpenWindow(int width, int height, string title) :
            base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        { }

        protected override void OnLoad()
        {
            base.OnLoad();
            SetLight();
            CursorState = CursorState.Confined;

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            cameraController = new CameraController(1.5f, new Vector3(0.0f, 0.0f, 3.0f));

            SceneModel = new Model();
            SceneModel.SetModel(@"Models/QuadColored.obj");
            SceneModel.SetTexture(@"Models/Textures/Quad.jpg");
            SceneModel.SetShader("D:\\Projects\\VSProjects\\TextureMaker\\OpenTKProject\\shader.vert", "D:\\Projects\\VSProjects\\TextureMaker\\OpenTKProject\\shader.frag");

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();
            ElementBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            SetupModelOnSceneNew(SceneModel);
        }

        private void SetupModelOnSceneNew(Model model)
        {
            int StrideL = 5;
            int UVOffsStart = 3;
            //int NormalOffset = 5;

            int stridSize = StrideL * sizeof(float);
            int uvByteOffset = UVOffsStart * sizeof(float);
            //int normalbyteOffs = NormalOffset * sizeof(float);

            GL.BufferData(BufferTarget.ArrayBuffer, model.ObjModel.GetResultMassive().Count() * sizeof(float), model.ObjModel.GetResultMassive().ToArray(), BufferUsageHint.StaticDraw);

            GL.BufferData(BufferTarget.ElementArrayBuffer, model.ObjModel.GetIndices().Count() * sizeof(uint), model.ObjModel.GetIndices().ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stridSize, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stridSize, uvByteOffset);

            model.Shader.Use();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, model.Texture.Handle);
            model.Shader.SetInt("texture0", 0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            KeyboardState input = KeyboardState;
            cameraController.Move(input, (float)e.Time);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderModel(SceneModel);

            SwapBuffers();
        }

        private void RenderModel(Model mesh)
        {
            Matrix4 view = cameraController.GetViewMatrix();
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Size.X / (float)Size.Y, 0.1f, 100.0f);
            Matrix4 model = Matrix4.Identity;

            mesh.Shader.Use();

            mesh.Shader.SetMatrix4("model", model);
            mesh.Shader.SetMatrix4("view", view);
            mesh.Shader.SetMatrix4("projection", projection);

            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, mesh.ObjModel.GetIndices().Count, DrawElementsType.UnsignedInt, 0);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (_firstMove)
            {
                _lastX = e.X;
                _lastY = e.Y;
                _firstMove = false;
            }

            float xOffset = e.X - _lastX;
            float yOffset = _lastY - e.Y;

            _lastX = e.X;
            _lastY = e.Y;

            cameraController.RotateCamera(xOffset, yOffset);
        }

        protected override void OnUnload()
        {
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
            GL.DeleteBuffer(ElementBufferObject);

            DisposeShaders(SceneModel);

            base.OnUnload();
        }

        private void DisposeShaders(Model model)
        {
            model.Shader.Dispose();
        }

        private void SetLight()
        {
            // Настройки освещения
        }
    }
}