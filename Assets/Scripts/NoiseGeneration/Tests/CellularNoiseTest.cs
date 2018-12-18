using NoiseGeneration.Noise;
using NUnit.Framework;

namespace Tests
{
    public class CellularNoiseTest
    {
        [Test]
        public void CellularNoiseReturnsValidValues()
        {
            Cellular cellularNoise = new Cellular();

            float val1 = cellularNoise.Value(0, 0);
            float val2 = cellularNoise.Value(10, 10);
            float val3 = cellularNoise.Value(100, 100);

            Assert.GreaterOrEqual(val1, 0f);
            Assert.LessOrEqual(val1, 1f);
            Assert.GreaterOrEqual(val2, 0f);
            Assert.LessOrEqual(val2, 1f);
            Assert.GreaterOrEqual(val3, 0f);
            Assert.LessOrEqual(val3, 1f);
        }
    }
}
