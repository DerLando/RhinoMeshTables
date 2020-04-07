using System.Collections.Generic;
using System.Linq;
using MeshTableLibrary.Core.Indices;
using MeshTableLibrary.Core.Math;
using MeshTableLibrary.Core.Tables;

namespace MeshTableLibrary.Core.MeshElements
{
    public class MeshConnectivity<T>
    {
        private readonly Vertex[] _vertices;
        private readonly Face[] _faces;
        private readonly Edge[] _edges;
        private readonly FacePair[] _facePairs;
        private FaceVertexTable _fvTable;
        private EdgeVertexTable _evTable;
        private EdgeFaceTable _efTable;
        private FacePairTable _fpTable;

        public MeshConnectivity(IMeshExtractor<T> extractor)
        {
            TableFactory.GetElements(extractor, out _vertices, out _faces, out _edges);

            _fvTable = new FaceVertexTable(_faces, _vertices);
            _evTable = new EdgeVertexTable(_edges, _vertices);
            _efTable = new EdgeFaceTable(_edges, _faces);

            _normals = CalculateNormals();

            _facePairs = PairFactory.GetFacePairs(this);
            _fpTable = PairFactory.CalculateFacePairTable(_facePairs);
        }

        public int VertexCount => _vertices.Length;
        public int FaceCount => _faces.Length;
        public int EdgeCount => _edges.Length;


        #region Normal calculations

        private readonly Vector3[] _normals;

        /// <summary>
        /// Calculates normals for all faces using Newells method
        /// </summary>
        /// <returns></returns>
        private Vector3[] CalculateNormals()
        {
            var normals = new Vector3[_faces.Length];
            for (int i = 0; i < _faces.Length; i++)
            {
                var x = 0.0;
                var y = 0.0;
                var z = 0.0;
                var face = _faces[i];
                var vertexCount = face.VertexIndices.Length;

                for (int j = 0; j < vertexCount; j++)
                {
                    var v0 = _vertices[face.VertexIndices[j].Value];
                    var v1 = _vertices[face.VertexIndices[(j + 1) % (vertexCount)].Value];

                    x += (v0.Position.Y - v1.Position.Y) * (v0.Position.Z + v1.Position.Z);
                    y += (v0.Position.Z - v1.Position.Z) * (v0.Position.X + v1.Position.X);
                    z += (v0.Position.X - v1.Position.X) * (v0.Position.Y + v1.Position.Y);
                }

                normals[i] = new Vector3(x, y, z).AsNormalized();
            }

            return normals;
        }

        #endregion

        #region FacePairs

        public FacePair GetFacePair(IndexPair<FaceIndex> indices) => _fpTable.GetFacePair(indices);
        public IEnumerable<FacePair> GetFacePairs() => _fpTable.GetFacePairs();

        public EdgeIndex GetSharedEdgeIndex(IndexPair<FaceIndex> indices)
        {
            // TODO: Inefficient could be better implemented as a table i guess
            return GetEdgeIndices(indices.FirstIndex).Intersect(GetEdgeIndices(indices.SecondIndex)).First();
        }

        public Vector3 GetEdgeDirection(EdgeIndex index)
        {
            var edge = GetEdge(index);
            return GetVertex(edge.VertexIndices[1]).Position - GetVertex(edge.VertexIndices[0]).Position;
        }

        #endregion

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
        public IEnumerable<Face> GetFaces() => _faces.AsEnumerable();

        public IEnumerable<FaceIndex> GetFaceIndices() =>
            from index in Enumerable.Range(0, FaceCount) select new FaceIndex((uint) index);

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

        #region Edge Getters

        public Edge GetEdge(EdgeIndex index) => _edges[index.Value];
        public Edge GetEdge(int index) => _edges[index];
        public IEnumerable<Edge> GetEdges() => _edges.AsEnumerable();

        public EdgeIndex[] GetEdgeIndices(VertexIndex index) => _evTable.GetEdgeIndices(index);
        public EdgeIndex[] GetEdgeIndices(FaceIndex index) => _efTable.GetFaceIndices(index);

        #endregion

        #region Normals getters

        public Vector3 GetNormal(FaceIndex index) => _normals[index.Value];
        public Vector3 GetNormal(int index) => _normals[index];

        #endregion
    }
}
