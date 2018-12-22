using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public class AsteroidRegionGenerator
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
            public readonly Dictionary<Matter, int> MaterialProbabilityWeights;

            public RegionSettings(int probabilityWeight, float variability, float density, Dictionary<Matter, int> materialProbabilityWeights)
            {
                ProbabilityWeight = probabilityWeight;
                Variability = variability;
                Density = density;
                MaterialProbabilityWeights = materialProbabilityWeights;
            }
        }

        private AsteroidRegionGenerator() { }

        public readonly MapSettings Settings;

        public AsteroidRegionGenerator(MapSettings settings)
        {
            Settings = settings;
        }
    }
}