using System;
using System.Collections;
using System.Collections.Generic;
using NoiseGeneration;
using NoiseGeneration.Noise;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AllNoiseTest
    {
        [Test]
        public void NoiseGeneratorReturnsInstantiatableClasses()
        {
            NoiseGenerator generator = new NoiseGenerator();

            foreach (Type T in generator.NoiseTypes)
            {
                Assert.IsTrue(T.IsClass);
                Assert.IsFalse(T.IsAbstract);
                Assert.IsTrue(T.IsSubclassOf(typeof(Filter)), "Noise class " + T.ToString() + " does not inherit from " + typeof(Filter).ToString());
                Assert.NotNull(T.GetConstructor(Type.EmptyTypes), "Noise " + T.ToString() + " does not have a parameterless constructor");
            }
        }

        [Test]
        public void AllNoiseClassesReturnValuesInRange()
        {
            NoiseGenerator generator = new NoiseGenerator();
            List<Filter> noises = new List<Filter>();
            foreach (Type T in generator.NoiseTypes)
            {
                noises.Add(Activator.CreateInstance(T) as Filter);
            }

            List<float> values = new List<float>();
            foreach (Filter noise in noises)
            {
                for (int i = -100; i < 100; i++)
                {
                    for (int j = -100; j < 100; j++)
                    {
                        values.Add(noise.Value(i, j));
                    }
                }

                values.Add(noise.Value(-1000, 0));
                values.Add(noise.Value(1000, 0));
                values.Add(noise.Value(10, 1000));
            }

            foreach (float value in values)
            {
                Assert.GreaterOrEqual(value, 0f);
                Assert.LessOrEqual(value, 1f);
            }
        }
    }
}
