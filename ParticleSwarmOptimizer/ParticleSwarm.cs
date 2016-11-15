using System;

namespace ParticleSwarmOptimizer
{
    internal static class Mathf
    {
        public static float Clamp(float value, float min, float max)
        {
            if(value > max) return max;
            if(value < min) return min;
            return value;
        }
    }

    public class Swarm
    {
        public delegate float Heuristic(float[] values);

        private readonly int _dimension;
        private readonly int _particles;

        private Tuple<float, float>[] _resolution;
        private float[][] _values;
        private float[][] _velocity;
        private float[][] _pbest;
        private float[] _pbestFitness;
        private float[] _gbest;
        private float _gbestFitness;

        private const float MAX_VELOCITY_PERCENTAGE = 1f;
        private float[] _maxVelocity;
        
        private const float C1 = 2f;
        private const float C2 = 2f;

        private Heuristic _heuristic;

        private Random _random;

        /// <summary>
        /// Creates a new Particle Swarm object
        /// </summary>
        /// <param name="dimension">Dimension of each particle's value vector</param>
        /// <param name="resolution">Range of each value type in the vector</param>
        /// <param name="particles">Amount of particles to be used</param>
        /// <param name="heuristic">Function to determine fitness value of a single particle. Higher fitness value is better.</param>
        public Swarm(int dimension, Tuple<float, float>[] resolution, int particles, Heuristic heuristic, Random random = null)
        {
            _dimension = dimension;
            _particles = particles;

            if(resolution.Length != dimension) {
                throw new ArgumentException($"Resolution dimension {resolution.Length} does not match dimension parameter value {dimension}.");
            }

            _resolution = resolution;
            _heuristic = heuristic;

            _random = random ?? new Random();
        }

        private void InitValues()
        {
            _values = new float[_particles][];
            _velocity = new float[_particles][];
            _pbest = new float[_particles][];
            _pbestFitness = new float[_particles];
            _gbest = new float[_dimension];
            _maxVelocity = new float[_dimension];

            for(int particle = 0; particle < _particles; ++particle) {
                _values[particle] = new float[_dimension];
                _velocity[particle] = new float[_dimension];
                _pbest[particle] = new float[_dimension];

                for(int dimension = 0; dimension < _dimension; ++dimension) {
                    _values[particle][dimension] = (float) _random.NextDouble()*(_resolution[dimension].Item2 - _resolution[dimension].Item1) + _resolution[dimension].Item1;
                    _velocity[particle][dimension] = 0f;
                    _maxVelocity[dimension] = (_resolution[dimension].Item2 - _resolution[dimension].Item1)*MAX_VELOCITY_PERCENTAGE;
                }
            }
        }

        public OptimizationResult Run(int iterations, float? fitnessRequirement)
        {
            InitValues();

            for(int iter = 0; iter < iterations; ++iter) {

                // Calculate fitness and keep track of best global and per particle
                for(int particle = 0; particle < _particles; ++particle) {
                    var fitness = _heuristic(_values[particle]);
                    if (fitness > _pbestFitness[particle]) {
                        _values[particle].CopyTo(_pbest[particle], 0);
                        _pbestFitness[particle] = fitness;
                    }
                    if (fitness > _gbestFitness) {
                        _values[particle].CopyTo(_gbest, 0);
                        _gbestFitness = fitness;
                        if(_gbestFitness >= fitnessRequirement)
                            return new OptimizationResult(_gbest, iter + 1);
                    }
                }

                // Update velocity and position
                for(int particle = 0; particle < _particles; ++particle) {
                    var r1 = (float)_random.NextDouble();
                    var r2 = (float)_random.NextDouble();
                    for(int dimension = 0; dimension < _dimension; ++dimension) {
                        // Calculate new velocity
                        var pv = _values[particle][dimension]; // Particle velocity
                        var bpv = _pbest[particle][dimension]; // Best particle velocity
                        var gbv = _gbest[dimension];           // Global best particle velocity
                        _velocity[particle][dimension] += C1*r1*(bpv - pv) + C2*r2*(gbv - pv);
                        _velocity[particle][dimension] = Mathf.Clamp(_velocity[particle][dimension], -_maxVelocity[dimension], _maxVelocity[dimension]);

                        // Update position
                        _values[particle][dimension] += _velocity[particle][dimension];
                        _values[particle][dimension] = Mathf.Clamp(_values[particle][dimension], _resolution[dimension].Item1, _resolution[dimension].Item2);
                    }
                }
            }
            return new OptimizationResult(_gbest, iterations);
        }

    }

    public struct OptimizationResult
    {
        public float[] Values;
        public int Iterations;

        public OptimizationResult(float[] values, int iterations)
        {
            Values = values;
            Iterations = iterations;
        }
    }
}
