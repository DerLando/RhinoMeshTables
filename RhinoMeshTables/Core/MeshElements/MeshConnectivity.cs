using System.Collections.Generic;
using System.Linq;
using Rhino.Geometry;
using RhinoMeshTables.Core.Indices;
using RhinoMeshTables.Core.Tables;

namespace RhinoMeshTables.Core.MeshElements
{
    public class MeshConnectivity
    {
        private readonly Vertex[] _vertices;
        private readonly Face[] _faces;
        private readonly Edge[] _edges;
        private FaceVertexTable _fvTable;
        private EdgeVertexTable _evTable;
        private EdgeFaceTable _efTable;

        public MeshConnectivity(Mesh mesh)
        {
            TableFactory.GetElements(mesh, out _vertices, out _faces, out _edges);

            _fvTable = new FaceVertexTable(_faces, _vertices);
            _evTable = new EdgeVertexTable(_edges, _vertices);
            _efTable = new EdgeFaceTable(_edges, _faces);
        }

        public int VertexCount => _vertices.Length;
        public int FaceCount => _faces.Length;
        public int EdgeCount => _edges.Length;

        #region Vertex Getters

        public Vertex GetVertex(VertexIndex index) => _vertices[index.Value];
        public Vertex GetVertex(int index) => _vertices[index];

        /// <summary>
        /// Returns the indices corresponding to the vertices neighboring
        /// the queried vertex index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VertexIndex[] GetVertexNeighborIndices(VertexIndex index)
        {
            var edges = _evTable[index];
            var vertexSet = new HashSet<VertexIndex>();
            foreach (var edge in edges)
            {
                vertexSet.Add(edge.VertexIndices[0]);
                vertexSet.Add(edge.VertexIndices[1]);
            }

            vertexSet.Remove(index);
            return vertexSet.ToArray();
        }

        #endregion
    }
}
