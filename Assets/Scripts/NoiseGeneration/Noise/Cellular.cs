using UnityEngine;

namespace NoiseGeneration.Noise
{
    public class Cellular : Filter
    {
        readonly FastNoise FastNoise;

        public Cellular()
        {
            FastNoise = new FastNoise();
        }

        public override float Value(float x, float y)
        {
            return Mathf.Lerp(0f, 1f, FastNoise.GetCellular(x, y));
        }
    }
}
