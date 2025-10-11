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
        private List<float> resultArray = new List<float>();

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

                resultArray = MergeThreeLists(vertices, texCoords);
            }
        }

        public static List<float> MergeThreeLists(List<float> list1, List<float> list2)
        {
            List<float> result = new List<float>();
            int index1 = 0, index2 = 0;

            while (index1 < list1.Count || index2 < list2.Count)
            {
                for (int i = 0; i < 3 && index1 < list1.Count; i++)
                {
                    result.Add(list1[index1]);
                    index1++;
                }

                for (int i = 0; i < 2 && index2 < list2.Count; i++)
                {
                    result.Add(list2[index2]);
                    index2++;
                }
            }

            return result;
        }

        public List<float> GetVertices()
        {
            return vertices;
        }
        public List<float> GetResultMassive()
        {
            return resultArray;
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
