using System.Collections.Generic;
using System.Linq;
using Rhino.Geometry;
using RhinoMeshTables.Core.MeshElements;

namespace RhinoMeshTables.Core.Tables
{
    public static class TableFactory
    {
        #region Element getters

        private static Face[] GetFaces(Mesh mesh)
        {
            var faces = new Face[mesh.GetNgonAndFacesCount()];
            var iter = 0;
            foreach (var face in mesh.GetNgonAndFacesEnumerable())
            {
                faces[iter] = new Face(face.BoundaryVertexIndexList());
                iter += 1;
            }

            return faces;
        }

        private static Vertex[] GetVertices(Mesh mesh)
        {
            var vertices = new Vertex[mesh.TopologyVertices.Count];
            for (int i = 0; i < mesh.TopologyVertices.Count; i++)
            {
                vertices[i] = new Vertex(mesh.TopologyVertices[i]);
            }

            return vertices;
        }

        private static VertexEdge[] GetVertexEdges(Mesh mesh)
        {
            var edges = new VertexEdge[mesh.TopologyEdges.Count];
            for (int i = 0; i < mesh.TopologyEdges.Count; i++)
            {
                edges[i] = new VertexEdge(from vIndex in mesh.TopologyEdges.GetTopologyVertices(i)
                    select (uint) vIndex);
            }

            return edges;
        }

        #endregion

        public static FaceVertexTable CreateFaceVertexTable(Mesh mesh)
        {
            return new FaceVertexTable(GetFaces(mesh), GetVertices(mesh));
        }

        public static EdgeVertexTable CreateEdgeVertexTable(Mesh mesh)
        {
            return new EdgeVertexTable(GetVertexEdges(mesh), GetVertices(mesh));
        }
    }
}
