using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhinoMeshTables.Core.Indices;

namespace RhinoMeshTables.Core.MeshElements
{
    public readonly struct FacePairProperties
    {
        public readonly FaceAngle Angle;
        public readonly EdgeIndex SharedEdgeIndex;

        public FacePairProperties(FaceAngle angle, EdgeIndex sharedEdgeIndex)
        {
            Angle = angle;
            SharedEdgeIndex = sharedEdgeIndex;
        }
    }
}
