using System;
using ParticleSwarmOptimizer;

namespace SwarmTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var swarm = new Swarm(
                2, // Dimension
                new[] { new Tuple<float, float>(0f, 10f), new Tuple<float, float>(0f, 10f) },
                1, // Starting particles
                (float[] values) => values[0] - values[1],
                null
            );

            var result = swarm.Run(5, 0.8f);

            Console.WriteLine($"Found result {result.Values[0]} after {result.Iterations} iterations.");

            Console.ReadLine();
        }
    }
}
