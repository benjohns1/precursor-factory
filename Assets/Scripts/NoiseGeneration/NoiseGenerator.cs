using NoiseGeneration.Noise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NoiseGeneration
{
    public class NoiseGenerator
    {
        public IEnumerable<Type> NoiseTypes { get; private set; }

        private Dictionary<Type, Filter> NoiseCache = new Dictionary<Type, Filter>();

        public NoiseGenerator()
        {
            NoiseTypes = Assembly.GetAssembly(typeof(Filter)).GetTypes().Where(T => !T.IsAbstract && T.IsSubclassOf(typeof(Filter)));
        }

        public float[,] Region<N>(int lowerX, int lowerY, int upperX, int upperY) where N : Filter, new()
        {
            float[,] region = new float[upperX - lowerX, upperY - lowerY];
            Filter noise = GetNoise<N>();
            for (int x = lowerX, i = 0; x < upperX; x++, i++)
            {
                for (int y = lowerY, j = 0; y < upperY; y++, j++)
                {
                    region[i, j] = noise.Value(x, y);
                }
            }
            return region;
        }

        private Filter GetNoise<N>() where N : Filter, new()
        {
            if (NoiseCache.TryGetValue(typeof(N), out Filter noise))
            {
                return noise;
            }
            noise = new N();
            NoiseCache.Add(typeof(N), noise);
            return noise;
        }
    }
}