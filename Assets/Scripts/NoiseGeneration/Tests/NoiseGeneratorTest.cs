using NoiseGeneration;
using NoiseGeneration.Noise;
using NUnit.Framework;

namespace Tests
{
    public class NoiseGeneratorTest
    {
        [Test]
        public void NoiseGeneratorCellularRegionReturnsCorrectSizeOfArray()
        {
            NoiseGenerator generator = new NoiseGenerator();

            float[,] region = generator.Region<Cellular>(-10, -20, 10, 20);

            Assert.AreEqual(20, region.GetLength(0), "Region X length is wrong");
            Assert.AreEqual(40, region.GetLength(1), "Region Y length is wrong");
        }
    }
}
