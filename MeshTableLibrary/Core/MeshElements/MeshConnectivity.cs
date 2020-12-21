using System.Collections.Generic;
using System.Linq;
using MeshTableLibrary.Core.Indices;
using MeshTableLibrary.Core.Math;
using MeshTableLibrary.Core.Tables;

namespace MeshTableLibrary.Core.MeshElements
{
    /// <summary>
    /// A database allowing for queries on the different types of connectivities
    /// defined between different mesh elements, f.e. vertex - face or face - face
    /// </summary>
    /// <typeparam name="T">The type of mesh for which the connectivity is defined</typeparam>
    public class MeshConnectivity<T>
    {
        #region Private fields

        /// <summary>
        /// backing array of all vertices in the mesh
        /// </summary>
        private readonly Vertex[] _vertices;

        /// <summary>
        /// backing array of all faces in the mesh
        /// </summary>
        private readonly Face[] _faces;

        /// <summary>
        /// backing array of all edges in the mesh
        /// </summary>
        private readonly Edge[] _edges;

        /// <summary>
        /// backing array of all neighboring pairs of faces in the mesh
        /// </summary>
        private readonly FacePair[] _facePairs;

        private FaceVertexTable _fvTable;
        private EdgeVertexTable _evTable;
        private EdgeFaceTable _efTable;
        private FacePairTable _fpTable;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="extractor">A custom <see cref="IMeshExtractor{T}"/> implementation, which allows
        /// the inner connectivity tables to be populated</param>
        public MeshConnectivity(IMeshExtractor<T> extractor)
        {
            // Extract the mesh elements
            TableFactory.GetElements(extractor, out _vertices, out _faces, out _edges);

            // Construct the backing tables
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

        /// <summary>
        /// backing array of all face normals
        /// </summary>
        private readonly Vector3[] _normals;

        /// <summary>
        /// Calculates normals for all faces using Newells method
        /// </summary>
        /// <returns>The calculated normals</returns>
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

        /// <summary>
        /// Get the pair of faces defined by the given FaceIndexPair
        /// </summary>
        /// <param name="indices">The index pair to find the FacePair for</param>
        /// <returns>The FacePair found</returns>
        public FacePair GetFacePair(IndexPair<FaceIndex> indices) => _fpTable.GetFacePair(indices);

        /// <summary>
        /// Gets all <see cref="FacePair"/>s defined
        /// </summary>
        /// <returns>The FacePairs</returns>
        public IEnumerable<FacePair> GetFacePairs() => _fpTable.GetFacePairs();

        /// <summary>
        /// Given an IndexPair of two face indices, returns the index of their shared edge
        /// </summary>
        /// <param name="indices">The index pair to find the shared edge index for</param>
        /// <returns></returns>
        public EdgeIndex GetSharedEdgeIndex(IndexPair<FaceIndex> indices)
        {
            // TODO: Inefficient could be better implemented as a table i guess
            return GetEdgeIndices(indices.FirstIndex).Intersect(GetEdgeIndices(indices.SecondIndex)).First();
        }

        /// <summary>
        /// Gets the direction of the edge for the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The direction vector, the length of the vector is equal to the length of the edge</returns>
        public Vector3 GetEdgeDirection(EdgeIndex index)
        {
            var edge = GetEdge(index);
            return GetVertex(edge.VertexIndices[1]).Position - GetVertex(edge.VertexIndices[0]).Position;
        }

        #endregion

        #region Vertex Getters

        /// <summary>
        /// Gets the vertex stored at a given VertexIndex
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vertex GetVertex(VertexIndex index) => _vertices[index.Value];

        /// <summary>
        /// Gets the vertex stored at a given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vertex GetVertex(int index) => _vertices[index];

        /// <summary>
        /// Gets all vertices stored
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Vertex> GetVertices() => _vertices.AsEnumerable();

        public IEnumerable<VertexIndex> GetAllVertexIndices() => 
            Enumerable
                .Range(0, VertexCount)
                .Select(n => new VertexIndex((uint)n));

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

        /// <summary>
        /// Returns the face for a given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Face GetFace(int index) => _faces[index];

        /// <summary>
        /// Gets all faces defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Face> GetFaces() => _faces.AsEnumerable();

        /// <summary>
        /// Gets all face indices defined
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the centroid of the face at the given FaceIndex
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetFaceCentroid(FaceIndex index)
        {
            // copy zero vector
            var centroid = Vector3.Zero;

            // get the face at the FaceIndex
            var face = GetFace(index);

            // get vertex count for the face
            var vCount = face.VertexIndices.Length;

            // iterate over the vertex indices of the face
            foreach (var faceVertexIndex in face.VertexIndices)
            {
                // get the vertex for the current vertex index
                var vertex = GetVertex(faceVertexIndex);

                // move the centroid by the vertex' position vector
                centroid += vertex.Position;
            }

            // calculate the centroid average
            return centroid / vCount;
        }

        #endregion

        #region Edge Getters

        /// <summary>
        /// Gets the Edge for a given EdgeIndex
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Edge GetEdge(EdgeIndex index) => _edges[index.Value];

        /// <summary>
        /// Gets the Edge for a given Index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Edge GetEdge(int index) => _edges[index];

        /// <summary>
        /// Gets all edges defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Edge> GetEdges() => _edges.AsEnumerable();

        /// <summary>
        /// Gets all edges containing a reference to the given VertexIndex
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public EdgeIndex[] GetEdgeIndices(VertexIndex index) => _evTable.GetEdgeIndices(index);

        /// <summary>
        /// Gets all edges containing a reference to the given FaceIndex
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public EdgeIndex[] GetEdgeIndices(FaceIndex index) => _efTable.GetFaceIndices(index);

        /// <summary>
        /// Get the vector at the middle between the two vectors defining
        /// the Edge for the given EdgeIndex
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetEdgeMid(EdgeIndex index)
        {
            var from = GetVertex(GetEdge(index).VertexIndices[0]).Position;
            var to = GetVertex(GetEdge(index).VertexIndices[1]).Position;

            return from + (to - from) / 2.0;
        }

        #endregion

        #region Normals getters

        /// <summary>
        /// Get the face normal for a given FaceIndex
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetNormal(FaceIndex index) => _normals[index.Value];

        /// <summary>
        /// Get the face normal for the given Index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetNormal(int index) => _normals[index];

        #endregion
    }
}
