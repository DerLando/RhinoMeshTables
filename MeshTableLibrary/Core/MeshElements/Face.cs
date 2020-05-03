using System.Collections.Generic;
using System.Linq;
using MeshTableLibrary.Core.Indices;

namespace MeshTableLibrary.Core.MeshElements
{
    /// <summary>
    /// A struct representation of a Mesh face
    /// </summary>
    public readonly struct Face
    {
        /// <summary>
        /// The vertex indices of vertices belonging to this face
        /// </summary>
        public readonly VertexIndex[] VertexIndices;

        public Face(IEnumerable<uint> indices)
        {
            VertexIndices = (from index in indices select new VertexIndex(index)).ToArray();
        }

        public Face(IEnumerable<VertexIndex> vertexIndices)
        {
            VertexIndices = vertexIndices.ToArray();
        }
    }
}
