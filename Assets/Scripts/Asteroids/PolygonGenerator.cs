using System.Linq;
using UnityEngine;

namespace Asteroids
{
    public static class PolygonGenerator
    {
        public static Vector2[] GenerateUnitVertices(int vertexCount)
        {
            Vector2[] vertices = new Vector2[vertexCount];
            Vector2[] sortedRandomOffsets = SortedRandomOffsets(vertexCount).ToArray();
            Vector2 lastVertex = Vector2.zero;
            for (int i = 0; i < vertexCount; i++)
            {
                lastVertex += sortedRandomOffsets[i];
                vertices[i] = lastVertex;
            }

            return vertices;
        }

        private static IOrderedEnumerable<Vector2> SortedRandomOffsets(int vertexCount)
        {
            float[] xDisplacements = RandomDisplacements(vertexCount);
            float[] yDisplacements = RandomDisplacements(vertexCount);

            Vector2[] randomVertices = new Vector2[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                randomVertices[i] = new Vector2(xDisplacements[i], yDisplacements[i]);
            }
            return randomVertices.OrderBy(v => Vector2.SignedAngle(Vector2.up, v));
        }

        /// <summary>
        /// Generate random displacement values for 1 dimension
        /// </summary>
        /// <param name="innerCount"></param>
        /// <returns></returns>
        private static float[] RandomDisplacements(int vertexCount)
        {
            int innerCount = vertexCount - 2;
            int firstCount = Mathf.CeilToInt((innerCount / 2f));
            float[] first = RandomBoundedArray(firstCount);
            int secondCount = innerCount - firstCount;
            float[] second = RandomBoundedArray(secondCount);

            bool direction = Random.value < 0.5f;
            float[] displacements = direction ? SignedDisplacements(first, second) : SignedDisplacements(second, first);
            return displacements.OrderBy(_ => Random.value).ToArray();
        }

        private static float[] SignedDisplacements(float[] right, float[] left)
        {
            int length = right.Length + left.Length - 2;
            float[] displacements = new float[length];
            int index = 0;
            for (int i = 1; i < right.Length; i++, index++)
            {
                displacements[index] = right[i] - right[i - 1];
            }
            for (int j = 1; j < left.Length; j++, index++)
            {
                displacements[index] = left[j - 1] - left[j];
            }
            return displacements;
        }

        private static float[] RandomBoundedArray(int innerCount)
        {
            float[] values = new float[innerCount + 2];
            values[0] = 0f;
            for (int i = 1; i < innerCount+1; i++)
            {
                values[i] = Random.value;
            }
            values[innerCount + 1] = 1f;
            System.Array.Sort(values);

            return values;
        }
    }
}