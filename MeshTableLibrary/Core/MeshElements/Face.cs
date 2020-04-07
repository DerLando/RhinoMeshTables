using System.Collections.Generic;
using System.Linq;
using MeshTableLibrary.Core.Indices;

namespace MeshTableLibrary.Core.MeshElements
{
    public readonly struct Face
    {
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
