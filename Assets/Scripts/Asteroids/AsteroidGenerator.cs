using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asteroids
{
    public class AsteroidGenerator
    {
        [System.Serializable]
        public class AsteroidSettingsEditable
        {
            public GameObject Prefab;
            public float Scale;
            public int VertexCount;
            public List<Matter> MatterList;
            public int[] MatterProportions;

            public AsteroidSettings GetSettings()
            {
                Dictionary<Matter, int> matterProportions = new Dictionary<Matter, int>();
                for (int i = 0; i < MatterList.Count; i++)
                {
                    if (MatterProportions.Length > i)
                    {
                        matterProportions.Add(MatterList[i], MatterProportions[i]);
                    }
                }

                return new AsteroidSettings(Prefab, Scale, VertexCount, matterProportions);
            }
        }

        public class AsteroidSettings
        {
            public readonly GameObject Prefab;
            public readonly float Scale;
            public readonly int VertexCount;
            public readonly Dictionary<Matter, int> MatterProportions;

            private AsteroidSettings() { }

            public AsteroidSettings(GameObject prefab, float scale, int vertexCount, Dictionary<Matter, int> matterProportions)
            {
                Prefab = prefab;
                Scale = scale;
                VertexCount = vertexCount;
                MatterProportions = matterProportions;
            }
        }

        private AsteroidGenerator() { }

        public readonly AsteroidSettings Settings;

        public AsteroidGenerator(AsteroidSettings settings)
        {
            Settings = settings;
        }

        public GameObject Generate()
        {
            GameObject asteroid = GameObject.Instantiate(Settings.Prefab);
            Mesh mesh = GenerateMesh();
            asteroid.GetComponent<MeshFilter>().mesh = mesh;
            asteroid.GetComponent<MeshCollider>().sharedMesh = mesh;
            return asteroid;
        }

        protected Mesh GenerateMesh()
        {
            Mesh mesh = new Mesh();
            SetVerticesAndUVs(mesh);
            mesh.triangles = GenerateTriangles();
            mesh.RecalculateNormals();
            return mesh;
        }

        protected void SetVerticesAndUVs(Mesh mesh)
        {
            int vertexCount = Settings.VertexCount;
            Vector3[] vertices = new Vector3[vertexCount];
            Vector2[] vertices2D = PolygonGenerator.GenerateUnitVertices(vertexCount);
            Vector2[] uvs = new Vector2[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                vertices[i] = vertices2D[i] * Settings.Scale;
                uvs[i] = vertices2D[i];
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;
        }

        protected int[] GenerateTriangles()
        {
            int triangleCount = Settings.VertexCount - 2;
            int[] triangles = new int[triangleCount * 3];

            for (int index = 0, triangle = 0; triangle < triangleCount; triangle++, index += 3)
            {
                triangles[index] = 0;
                triangles[index + 1] = triangle + 2;
                triangles[index + 2] = triangle + 1;
            }

            return triangles;
        }
    }
}