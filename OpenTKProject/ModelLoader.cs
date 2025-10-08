namespace OpenTKProject
{
    public class OBJModel
    {
        public OBJModel() { }
        public OBJModel(string path) 
        {
            LoadModel(path);
        }

        private List<float> vertices = new List<float>();
        private List<float> normals = new List<float>();
        private List<float> texCoords = new List<float>();
        private List<uint> indices = new List<uint>();
        public void LoadModel(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if(line.StartsWith("v "))
                    {
                        string[] parts = line.Split(' ');

                        vertices.Add(float.Parse(parts[1].Trim(), System.Globalization.CultureInfo.InvariantCulture));
                        vertices.Add(float.Parse(parts[2].Trim(), System.Globalization.CultureInfo.InvariantCulture));
                        vertices.Add(float.Parse(parts[3].Trim(), System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else if(line.StartsWith("vn "))
                    {
                        string[] parts = line.Split(' ');

                        normals.Add(float.Parse(parts[1].Trim(), System.Globalization.CultureInfo.InvariantCulture));
                        normals.Add(float.Parse(parts[2].Trim(), System.Globalization.CultureInfo.InvariantCulture));
                        normals.Add(float.Parse(parts[3].Trim(), System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else if (line.StartsWith("vt "))
                    {
                        string[] parts = line.Split(' ');

                        texCoords.Add(float.Parse(parts[1].Trim(), System.Globalization.CultureInfo.InvariantCulture));
                        texCoords.Add(float.Parse(parts[2].Trim(), System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else if (line.StartsWith("f "))
                    {
                        string[] parts = line.Split(' ');

                        for (int i = 1; i < parts.Length; i++)
                        {
                            string[] subParts = parts[i].Split('/');
                            indices.Add(uint.Parse(subParts[0]) - 1);
                        }
                    }
                }
            }
        }

        public List<float> GetVertices()
        {
            return vertices;
        }
        public List<float> GetNormals()
        {
            return normals;
        }
        public List<float> GetTexCoords()
        {
            return texCoords;
        }
        public List<uint> GetIndices()
        {
            return indices;
        }
    }
}
