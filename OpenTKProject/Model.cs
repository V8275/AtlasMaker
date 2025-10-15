using System.IO;

namespace OpenTKProject
{
    public class Model
    {
        VisualModel vModel;
        Texture texture;
        Shader shader;

        public VisualModel VModel { get { return vModel; } }
        public Texture Texture { get { return texture; } }
        public Shader Shader { get { return shader; } }

        public Model() { }
        public Model(VisualModel mod, Texture tex, Shader shad)
        {
            vModel = mod;
            texture = tex;
            shader = shad;
        }

        public Model(Model m)
        {
            SetVModel(m.vModel.PathToModel);
            SetTexture(m.Texture.PathToTexture);
            SetShader(m.shader.PathToVecShader, m.shader.PathToFragShader);
        }

        public void SetVModel(string path)
        {
            vModel = new VisualModel(path);
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
