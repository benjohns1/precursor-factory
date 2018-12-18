using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidGenerator
{
    public class AsteroidDataFactory
    {
        public class MapSettings
        {
            public readonly RegionSettings StartingRegion;
            public readonly List<RegionSettings> Regions;

            public MapSettings(RegionSettings startingRegion, List<RegionSettings> regions)
            {
                StartingRegion = startingRegion;
                Regions = regions;
            }
        }

        public struct RegionSettings
        {
            public readonly int ProbabilityWeight;
            public readonly float Variability;
            public readonly float Density;
            public readonly Dictionary<Material, int> MaterialProbabilityWeights;

            public RegionSettings(int probabilityWeight, float variability, float density, Dictionary<Material, int> materialProbabilityWeights)
            {
                ProbabilityWeight = probabilityWeight;
                Variability = variability;
                Density = density;
                MaterialProbabilityWeights = materialProbabilityWeights;
            }
        }

        public enum Material { Iron, Silicon }

        private AsteroidDataFactory() { }

        public readonly MapSettings Settings;

        public AsteroidDataFactory(MapSettings settings)
        {
            Settings = settings;
        }
    }
}