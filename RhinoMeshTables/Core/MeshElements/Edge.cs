using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhinoMeshTables.Core.Indices;

namespace RhinoMeshTables.Core.MeshElements
{
    public readonly struct Edge
    {
        public readonly VertexIndex[] VertexIndices;
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

        public int FaceCount => FaceIndices.Length;
    }

    public readonly struct VertexEdge
    {
        public readonly VertexIndex[] VertexIndices;

        public VertexEdge(IEnumerable<VertexIndex> vertexIndices)
        {
            VertexIndices = vertexIndices.ToArray();
        }

        public VertexEdge(IEnumerable<uint> vertexIndices)
        {
            VertexIndices = (from index in vertexIndices select new VertexIndex(index)).ToArray();
        }
    }

    public readonly struct FaceEdge
    {
        public readonly FaceIndex[] FaceIndices;

        public FaceEdge(FaceIndex[] faceIndices)
        {
            FaceIndices = faceIndices;
        }

        public FaceEdge(IEnumerable<uint> faceIndices)
        {
            FaceIndices = (from index in faceIndices select new FaceIndex(index)).ToArray();
        }

        public int FaceCount => FaceIndices.Length;
    }
}
