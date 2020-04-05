using System.Collections.Generic;
using System.Linq;
using RhinoMeshTables.Core.Indices;
using RhinoMeshTables.Core.MeshElements;

namespace RhinoMeshTables.Core.Tables
{
    public class FaceVertexTable : TableBase
    {
        private Face[] _faces;
        private Vertex[] _vertices;

        public FaceVertexTable(Face[] faces, Vertex[] vertices)
        {
            _faces = faces;
            _vertices = vertices;
        }

        #region Vertex getters

        public VertexIndex[] GetVertexIndices(FaceIndex key)
        {
            return _faces[key.Value].VertexIndices;
        }

        private Vertex[] GetVertices(FaceIndex key)
        {
            return (from index in GetVertexIndices(key) select _vertices[index.Value]).ToArray();
        }

        public Vertex[] this[FaceIndex key]
        {
            get => GetVertices(key);
        }

        #endregion

        #region Face getters

        public FaceIndex[] GetFaceIndices(VertexIndex key)
        {
            var indices = new List<FaceIndex>();
            for (int i = 0; i < _faces.Length; i++)
            {
                if (!_faces[i].VertexIndices.Contains(key)) continue;
                indices.Add(new FaceIndex((uint)i));
            }

            return indices.ToArray();
        }

        private Face[] GetFaces(VertexIndex key)
        {
            return (from face in _faces where face.VertexIndices.Contains(key) select face).ToArray();
        }

        public Face[] this[VertexIndex key]
        {
            get => GetFaces(key);
        }

        #endregion

        public int FaceCount => _faces.Length;
        public int VertexCount => _vertices.Length;
    }
}
