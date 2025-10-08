using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKProject
{
    public class OpenWindow : GameWindow
    {
        OBJModel objModel;

        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;
        Shader shader;

        CameraController cameraController;
        Vector3 position = new Vector3(0.0f, 0.0f, 3.0f);
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        Vector2 _lastMousePos;

        public OpenWindow(int width, int height, string title) :
            base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        { }

        protected override void OnLoad()
        {
            base.OnLoad();
            SetLight();

            cameraController = new CameraController();

            objModel = new OBJModel("D:\\Projects\\VSProjects\\TextureMaker\\OpenTKProject\\Models\\untitled.obj");

            shader = new Shader("D:\\Projects\\VSProjects\\TextureMaker\\OpenTKProject\\shader.vert", "D:\\Projects\\VSProjects\\TextureMaker\\OpenTKProject\\shader.frag");

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, objModel.GetVertices().Count * sizeof(float), objModel.GetVertices().ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, objModel.GetIndices().Count * sizeof(uint), objModel.GetIndices().ToArray(), BufferUsageHint.StaticDraw);

            shader.Use();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            KeyboardState input = KeyboardState;

            cameraController.Move(input, (float)e.Time, ref position);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Создаем матрицы
            Matrix4 view = Matrix4.LookAt(position, position + front, up);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Size.X / (float)Size.Y, 0.1f, 100.0f);
            Matrix4 model = Matrix4.Identity;

            // Передаем матрицы в шейдер
            shader.Use();
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            // Рисуем модель
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, objModel.GetIndices().Count, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
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

        protected override void OnUnload()
        {
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
            shader.Dispose();

            base.OnUnload();
        }

        private void SetLight()
        {
            //Color4 lightColor = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
            //Color4 toyColor = new Color4(1.0f, 0.5f, 0.31f, 1.0f);
            //Color4 result = lightColor * toyColor; // = (1.0f, 0.5f, 0.31f, 1.0f);


        }
    }
}