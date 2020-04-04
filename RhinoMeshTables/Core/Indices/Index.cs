using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoMeshTables.Core.Indices
{
    public readonly struct FaceIndex
    {
        public readonly uint Value;

        public FaceIndex(uint value)
        {
            Value = value;
        }
    }

    public readonly struct VertexIndex : IEquatable<VertexIndex>
    {
        public readonly uint Value;

        public VertexIndex(uint value)
        {
            Value = value;
        }

        public bool Equals(VertexIndex other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexIndex other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int) Value;
        }
    }

    public readonly struct EdgeIndex
    {
        public readonly uint Value;

        public EdgeIndex(uint value)
        {
            Value = value;
        }
    }
}
