using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace RhinoMeshTables.Core.MeshElements
{
    public readonly struct Vertex
    {
        public readonly Point3d Position;

        public Vertex(Point3d position)
        {
            Position = position;
        }

        public bool EpsilonEquals(Vertex other, double epsilon)
        {
            return Position.DistanceToSquared(other.Position) < epsilon;
        }
    }
}
