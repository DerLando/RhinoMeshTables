using System.Collections.Generic;
using System.Linq;
using MeshTableLibrary.Core.Indices;

namespace MeshTableLibrary.Core.MeshElements
{
    /// <summary>
    /// A representation of a mesh edge
    /// </summary>
    public readonly struct Edge
    {
        /// <summary>
        /// The vertex indices this edge is defined between
        /// </summary>
        public readonly VertexIndex[] VertexIndices;

        /// <summary>
        /// The face indices of faces containing this edge
        /// </summary>
        public readonly FaceIndex[] FaceIndices;

        public Edge(IEnumerable<VertexIndex> vertexIndices, IEnumerable<FaceIndex> faceIndices)
        {
            VertexIndices = vertexIndices.ToArray();
            FaceIndices = faceIndices.ToArray();
        }

        public Edge(IEnumerable<uint> vertexIndices, IEnumerable<uint> faceIndices)
        {
            VertexIndices = (from index in vertexIndices select new VertexIndex(index)).ToArray();
            FaceIndices = (from index in faceIndices select new FaceIndex(index)).ToArray();
        }

        /// <summary>
        /// The number of faces which have this edge as one of their edges
        /// </summary>
        public int FaceCount => FaceIndices.Length;
    }
}
