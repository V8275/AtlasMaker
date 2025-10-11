using System;
namespace OpenTKProject
{
    public class Model
    {
        OBJModel objModel;
        Texture texture;
        Shader shader;

        public OBJModel ObjModel { get { return objModel; } }
        public Texture Texture { get { return texture; } }
        public Shader Shader { get { return shader; } }

        public Model() { }
        public Model(OBJModel mod, Texture tex, Shader shad)
        {
            objModel = mod;
            texture = tex;
            shader = shad;
        }

        public void SetModel(string path)
        {
            objModel = new OBJModel(path);
        }
        public void SetTexture(string path)
        {
            texture = new Texture(path);
        }
        public void SetShader(string vertPath, string fragPath)
        {
            shader = new Shader(vertPath, fragPath);
        }

    }
}
