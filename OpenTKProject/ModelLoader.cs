using JeremyAnsel.Media.WavefrontObj;

namespace OpenTKProject
{
    public class VisualModel
    {
        private string path;
        public string PathToModel { get { return path; } }

        private ObjFile model;
        private List<float> vertices = new List<float>();
        private List<uint> indices = new List<uint>();

        public VisualModel(string modelPath)
        {
            path = modelPath;
            model = ObjFile.FromFile(modelPath);

            vertices = GetVertices();
            indices = GetIndices();
        }

        private List<float> GetVertices()
        {
            var vertices = new List<float>();

            foreach (var face in model.Faces)
            {
                foreach (var vertex in face.Vertices)
                {
                    var position = model.Vertices[vertex.Vertex - 1].Position;
                    vertices.Add(position.X);
                    vertices.Add(position.Y);
                    vertices.Add(position.Z);

                    if (vertex.Texture > 0 && model.TextureVertices.Count > 0)
                    {
                        var texCoord = model.TextureVertices[vertex.Texture - 1];
                        vertices.Add(texCoord.X);
                        vertices.Add(texCoord.Y);
                    }
                    else
                    {
                        vertices.Add(0f);
                        vertices.Add(0f);
                    }

                    if (vertex.Normal > 0 && model.VertexNormals.Count > 0)
                    {
                        var normal = model.VertexNormals[vertex.Normal - 1];
                        vertices.Add(normal.X);
                        vertices.Add(normal.Y);
                        vertices.Add(normal.Z);
                    }
                    else
                    {
                        vertices.Add(0f);
                        vertices.Add(1f);
                        vertices.Add(0f);
                    }
                }
            }

            return vertices;
        }

        private List<uint> GetIndices()
        {
            var indices = new List<uint>();
            uint index = 0;

            foreach (ObjFace face in model.Faces)
            {
                for (int i = 0; i < face.Vertices.Count; i++)
                {
                    indices.Add(index++);
                }
            }

            return indices;
        }

        public List<float> Verticies { get { return vertices; } }
        public List<uint> Indices { get { return indices; } }

    }
}
