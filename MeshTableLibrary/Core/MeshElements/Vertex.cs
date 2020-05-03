using MeshTableLibrary.Core.Math;

namespace MeshTableLibrary.Core.MeshElements
{
    /// <summary>
    /// Struct representation of a mesh vertex
    /// </summary>
    public readonly struct Vertex
    {
        /// <summary>
        /// The position in 3d space of the Vertex
        /// </summary>
        public readonly Vector3 Position;

        public Vertex(Vector3 position)
        {
            Position = position;
        }

        /// <summary>
        /// Calculates if this vector, under tolerance epsilon, is equal to another given vector
        /// </summary>
        /// <param name="other">The other vector to compare</param>
        /// <param name="epsilon">The tolerance to use for calculation</param>
        /// <returns></returns>
        public bool EpsilonEquals(Vertex other, double epsilon)
        {
            return Position.DistanceToSquared(other.Position) < epsilon;
        }
    }
}
