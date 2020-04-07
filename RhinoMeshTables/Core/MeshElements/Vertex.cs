using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhinoMeshTables.Core.Math;

namespace RhinoMeshTables.Core.MeshElements
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
