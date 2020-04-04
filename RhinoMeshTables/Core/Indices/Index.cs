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

    public readonly struct VertexIndex
    {
        public readonly uint Value;

        public VertexIndex(uint value)
        {
            Value = value;
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
