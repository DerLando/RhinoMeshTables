using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using RhinoMeshTables.Core.Indices;
using RhinoMeshTables.Core.MeshElements;

namespace RhinoMeshTables.Core.Tables
{
    public class FacePairTable : TableBase
    {
        private readonly FacePairProperties[] _properties;
        private readonly Face[] _faces;
        private Dictionary<int, int> _fp_dict;

        public FacePairTable(IEnumerable<Tuple<FaceIndex, FaceIndex>> pairIndices, Face[] faces, Vector3d[] normals)
        {
            _faces = faces;

        }
    }
}
