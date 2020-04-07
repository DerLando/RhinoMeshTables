using MeshTableLibrary.Core.Math;

namespace MeshTableLibrary.Core.MeshElements
{
    public readonly struct Vertex
    {
        public readonly Vector3 Position;

        public Vertex(Vector3 position)
        {
            Position = position;
        }

        public bool EpsilonEquals(Vertex other, double epsilon)
        {
            return Position.DistanceToSquared(other.Position) < epsilon;
        }
    }
}
