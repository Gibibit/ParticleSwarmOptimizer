using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParticleSwarmOptimizer;

namespace ParticleSwarmTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void SimpleLinear()
        {
            var swarm = new Swarm(
                1, // Dimension
                new[] { new Tuple<float, float>(0f, 1f) },
                3, // Starting particles
                (float[] values) => values[0],
                null
            );

            var result = swarm.Run(5, 0.8f);

            Console.WriteLine($"Found result {result.Values[0]} after {result.Iterations} iterations.");
        }
        
        [TestMethod]
        public void Simple2DTest()
        {
            var swarm = new Swarm(
                2, // Dimensionality
                new[] { new Tuple<float, float>(0f, 10f), new Tuple<float, float>(0f, 10f) },
                3, // Starting particles
                (float[] values) => values[0] - values[1],
                null
            );

            var result = swarm.Run(5, 8f);

            Console.WriteLine($"Found result ({result.Values[0]}, {result.Values[1]}) after {result.Iterations} iterations.");
        }

        [TestMethod]
        public void Simple2DTest2()
        {
            var swarm = new Swarm(
                2, // Dimensionality
                new[] { new Tuple<float, float>(0f, 100f), new Tuple<float, float>(0f, 100f) },
                3, // Starting particles
                (float[] values) => values[0] - values[1],
                null
            );

            var result = swarm.Run(1000, 100f);

            Console.WriteLine($"Found result ({result.Values[0]}, {result.Values[1]}) after {result.Iterations} iterations.");
        }

        [TestMethod]
        public void SimpleLargeResolutionTest()
        {
            var swarm = new Swarm(
                2, // Dimensionality
                new[] {
                    new Tuple<float, float>(float.MinValue/2f, float.MaxValue/2f),
                    new Tuple<float, float>(float.MinValue/2f, float.MaxValue/2f)
                },
                3, // Starting particles
                (float[] values) => values[0] - values[1],
                null
            );

            var result = swarm.Run(1000, float.MaxValue - 10f);

            Console.WriteLine($"Found result ({result.Values[0]}, {result.Values[1]}) after {result.Iterations} iterations.");
        }
    }
}
