using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhinoMeshTables.Core.Indices;

namespace RhinoMeshTables.Core.MeshElements
{
    public readonly struct FacePair
    {
        public readonly IndexPair<FaceIndex> Indices;
        public readonly FaceAngle Angle;
        public readonly EdgeIndex SharedEdgeIndex;
    }
}
