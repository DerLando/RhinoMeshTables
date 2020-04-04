﻿using System;
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
}
