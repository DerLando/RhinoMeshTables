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

        /// <summary>
        /// Returns the vertex indices the supplied Face consists of
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VertexIndex[] GetVertexIndices(FaceIndex index)
        {
            return _fvTable.GetVertexIndices(index);
        }

        /// <summary>
        /// Returns the vertex indices between which the supplied edge runs
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VertexIndex[] GetVertexIndices(EdgeIndex index)
        {
            return _evTable.GetVertexIndices(index);
        }

        #endregion

        #region Face Getters

        /// <summary>
        /// Returns the face for a given FaceIndex
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Face GetFace(FaceIndex index) => _faces[index.Value];
        public Face GetFace(int index) => _faces[index];

        /// <summary>
        /// Returns the indices of all faces neighboring the given face
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FaceIndex[] GetFaceNeighborIndices(FaceIndex index)
        {
            var edges = _efTable[index];
            var faceSet = new HashSet<FaceIndex>();
            foreach (var edge in edges)
            {
                faceSet.Add(edge.FaceIndices[0]);
                if (edge.FaceCount > 1) faceSet.Add(edge.FaceIndices[1]);
            }

            faceSet.Remove(index);
            return faceSet.ToArray();
        }

        /// <summary>
        /// Returns the indices of all faces the given Vertex belongs to
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FaceIndex[] GetFaceIndices(VertexIndex index)
        {
            return _fvTable.GetFaceIndices(index);
        }

        /// <summary>
        /// Returns the indices of the faces on both sides of the given edge
        /// If the edge is naked, the resulting array will only contain one element
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FaceIndex[] GetFaceIndices(EdgeIndex index)
        {
            return _edges[index.Value].FaceIndices;
        }

        #endregion
    }
}
