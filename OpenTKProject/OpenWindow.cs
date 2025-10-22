using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKProject
{
    public class OpenWindow : GameWindow
    {
        private readonly Vector3 globalLightPos = new Vector3(1.2f, 1.0f, 2.0f);
        private readonly Color lightColor = Color.AntiqueWhite;

        private string defaultVertShader = "D:\\Projects\\VSProjects\\TextureMaker\\OpenTKProject\\Shaders\\Vert\\shader.vert";
        private string defaultFragShader = "D:\\Projects\\VSProjects\\TextureMaker\\OpenTKProject\\Shaders\\Frag\\shader.frag";

        CameraController cameraController;

        private float _lastX;
        private float _lastY;
        private bool _firstMove = true;

        public OpenWindow(int width, int height, string title) :
            base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        { }

        List<SceneObject> sceneObjects = new List<SceneObject>();
        SceneObject hDRI;

        Dictionary<Model, (int vao, int vbo, int ebo)> modelBuffers = new Dictionary<Model, (int, int, int)>();

        protected override void OnLoad()
        {
            base.OnLoad();
            CursorState = CursorState.Grabbed;

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            cameraController = new CameraController(1.5f, new Vector3(0.0f, 0.0f, 3.0f));

            List<Model> models = new List<Model>();

            models.Add(new Model(SetupModel(@"Models/untitled.obj", @"Models/Textures/TexTest.jpg")));

            Model lightmodel = new Model(SetupModel(@"Models/Frog3.obj", @"Models/Textures/Frog3Texture.jpg"));
            Model HDRI = new Model(SetupModel(@"Models/SkyBox.obj", @"Models/Textures/Frog3Texture.jpg",
                "D:\\Projects\\VSProjects\\TextureMaker\\OpenTKProject\\Shaders\\Vert\\HDRIShader.vert",
                "D:\\Projects\\VSProjects\\TextureMaker\\OpenTKProject\\Shaders\\Frag\\HDRIShader.frag"));

            //,
            //"D:\\Projects\\VSProjects\\TextureMaker\\OpenTKProject\\Shaders\\Vert\\LightShader.vert",
                //"D:\\Projects\\VSProjects\\TextureMaker\\OpenTKProject\\Shaders\\Frag\\LightShader.frag"

            lightmodel.Shader.SetVector3("objectColor", new Vector3(0.0f, 0.5f, 0.31f));
            lightmodel.Shader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));

            models.Add(lightmodel);

            sceneObjects.Add(new SceneObject(models[0], new Vector3(0.0f, 0.0f, 0.0f)));
            sceneObjects.Add(new SceneObject(models[1], new Vector3(2.0f, -1f, 0.0f), new Vector3(0,180,0), new Vector3(10, 10, 10)));

            SetupLight(HDRI);
            foreach (var obj in sceneObjects)
            {
                if (!modelBuffers.ContainsKey(obj.Model))
                {
                    SetupModelBuffers(obj.Model);
                }
            }

            SetGradientColors(hDRI.Model, new Vector3(0.0f, 0.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f));
        }

        private void SetGradientColors(Model model, Vector3 bottomColor, Vector3 topColor)
        {
            model.Shader.Use();
            model.Shader.SetVector3("bottomColor", bottomColor);
            model.Shader.SetVector3("topColor", topColor);
        }

        private Model SetupModel(string modelPath, string texturePath = "", string vertShader = "", string fragShader = "")
        {
            Model model = new Model();
            model.SetVModel(modelPath);
            if(!String.IsNullOrEmpty(texturePath)) model.SetTexture(texturePath);

            if (String.IsNullOrEmpty(vertShader) || String.IsNullOrEmpty(fragShader))
                model.SetShader(defaultVertShader, defaultFragShader);
            else
                model.SetShader(vertShader, fragShader);

            return model;
        }

        private void SetupModelBuffers(Model model)
        {
            int vao = GL.GenVertexArray();
            int vbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            SetupModelOnSceneNew(model, vbo, ebo);
            modelBuffers[model] = (vao, vbo, ebo);
        }

        private void SetupModelOnSceneNew(Model model, int vbo, int ebo)
        {
            int StrideL = 8;
            int UVOffsStart = 3;
            int NormalOffset = 5;

            int stridSize = StrideL * sizeof(float);
            int uvByteOffset = UVOffsStart * sizeof(float);
            int normalByteOffs = NormalOffset * sizeof(float);

            GL.BufferData(BufferTarget.ArrayBuffer, model.VModel.Verticies.Count() * sizeof(float),
                          model.VModel.Verticies.ToArray(), BufferUsageHint.StaticDraw);

            GL.BufferData(BufferTarget.ElementArrayBuffer, model.VModel.Indices.Count() * sizeof(uint),
                          model.VModel.Indices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stridSize, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stridSize, uvByteOffset);

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stridSize, normalByteOffs);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            KeyboardState input = KeyboardState;
            cameraController.Move(input, (float)e.Time);
            hDRI.Position = cameraController.Position;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var obj in sceneObjects)
            {
                RenderModel(obj);
            }

            SwapBuffers();
        }

        private void RenderModel(SceneObject sceneObj)
        {
            Matrix4 view = cameraController.GetViewMatrix();
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)Size.X / (float)Size.Y, 0.1f, 100.0f);

            Matrix4 modelMatrix = sceneObj.GetModelMatrix();

            sceneObj.Model.Shader.Use();

            if(sceneObj.Model.Texture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, sceneObj.Model.Texture.Handle);
                sceneObj.Model.Shader.SetInt("texture0", 0);
            }

            sceneObj.Model.Shader.SetMatrix4("model", modelMatrix);
            sceneObj.Model.Shader.SetMatrix4("view", view);
            sceneObj.Model.Shader.SetMatrix4("projection", projection);
            sceneObj.Model.Shader.SetVector3("viewPos", cameraController.Position);

            sceneObj.Model.Shader.SetVector3("material.ambient", ColorToVec3(lightColor));
            sceneObj.Model.Shader.SetVector3("material.diffuse", new Vector3(1.0f, 0.5f, 0.31f));
            sceneObj.Model.Shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            sceneObj.Model.Shader.SetFloat("material.shininess", 32.0f);

            sceneObj.Model.Shader.SetVector3("light.ambient", new Vector3(0.2f, 0.2f, 0.2f));
            sceneObj.Model.Shader.SetVector3("light.diffuse", new Vector3(0.5f, 0.5f, 0.5f));
            sceneObj.Model.Shader.SetVector3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));
            sceneObj.Model.Shader.SetVector3("light.position", globalLightPos);

            var buffers = modelBuffers[sceneObj.Model];
            GL.BindVertexArray(buffers.vao);
            GL.DrawElements(PrimitiveType.Triangles, sceneObj.Model.VModel.Indices.Count,
                           DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }

        private Vector3 ColorToVec3(Color c)
        {
            var x = c.R / 100;
            var y = c.G / 100;
            var z = c.B / 100;
            return new Vector3(x, y, z);
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

        private void SetupLight(Model hdriModel)
        {
            hDRI = new SceneObject(hdriModel, cameraController.Position, new Vector3(10, 10, 10));
            SetupModelBuffers(hDRI.Model);
            hDRI.Model.Shader.SetVector3("objectColor", ColorToVec3(Color.AliceBlue));

            sceneObjects.Add(hDRI);
        }

        protected override void OnUnload()
        {
            foreach (var buffers in modelBuffers.Values)
            {
                GL.DeleteBuffer(buffers.vbo);
                GL.DeleteVertexArray(buffers.vao);
                GL.DeleteBuffer(buffers.ebo);
            }

            foreach (var obj in sceneObjects)
            {
                DisposeShaders(obj.Model);
            }

            base.OnUnload();
        }

        private void DisposeShaders(Model model)
        {
            model.Shader.Dispose();
        }
    }
}