using RhinoMeshTables.Core.Indices;
using RhinoMeshTables.Core.MeshElements;

namespace RhinoMeshTables.Core.Tables
{
    public class FaceVertexTable : TableBase
    {
        private Face[] _faces;

        public FaceVertexTable(Face[] faces)
        {
            _faces = faces;
        }

        public VertexIndex[] this[FaceIndex key]
        {
            get => _faces[key.Value].VertexIndices;
        }

        public override int Count()
        {
            return _faces.Length;
        }
    }
}
