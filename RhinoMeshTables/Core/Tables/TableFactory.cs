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
                var topoVertIndices = 
                    from vert in face.BoundaryVertexIndexList()
                    select (uint)mesh.TopologyVertices.TopologyVertexIndex((int) vert);
                faces[iter] = new Face(topoVertIndices);
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

        private static Dictionary<uint, int> GetFaceNgonTable(Mesh mesh)
        {
            var fn_table = new Dictionary<uint, int>();
            var ngons = mesh.GetNgonAndFacesEnumerable().ToArray();
            for (int i = 0; i < ngons.Length; i++)
            {
                foreach (var index in ngons[i].FaceIndexList())
                {
                    fn_table[index] = i;
                }
            }

            return fn_table;
        }

        private static Edge[] GetEdges(Mesh mesh)
        {
            var edgeIndices = (from index in Enumerable.Range(0, mesh.TopologyEdges.Count)
                where !mesh.TopologyEdges.IsNgonInterior(index)
                select index).ToArray();
            var edges = new Edge[edgeIndices.Length];
            var fn_table = GetFaceNgonTable(mesh);
            for (int i = 0; i < edgeIndices.Length; i++)
            {
                var verts = from vIndex in mesh.TopologyEdges.GetTopologyVertices(edgeIndices[i])
                    select (uint) vIndex;
                var faces = from fIndex in mesh.TopologyEdges.GetConnectedFaces(edgeIndices[i]) select (uint) fIndex;

                var nGonSet = new HashSet<int>();
                foreach (var faceIndex in faces)
                {
                    nGonSet.Add(fn_table[faceIndex]);
                }

                faces = from ngon in nGonSet select (uint) ngon;

                edges[i] = new Edge(verts, faces);
            }

            return edges;
        }

        public static void GetElements(Mesh mesh, out Vertex[] vertices, out Face[] faces, out Edge[] edges)
        {
            vertices = GetVertices(mesh);
            faces = GetFaces(mesh);
            edges = GetEdges(mesh);
        }

        #endregion

        public static FaceVertexTable CreateFaceVertexTable(Mesh mesh)
        {
            return new FaceVertexTable(GetFaces(mesh), GetVertices(mesh));
        }

        public static EdgeVertexTable CreateEdgeVertexTable(Mesh mesh)
        {
            return new EdgeVertexTable(GetEdges(mesh), GetVertices(mesh));
        }

        public static EdgeFaceTable CreateEdgeFaceTable(Mesh mesh)
        {
            return new EdgeFaceTable(GetEdges(mesh), GetFaces(mesh));
        }
    }
}
