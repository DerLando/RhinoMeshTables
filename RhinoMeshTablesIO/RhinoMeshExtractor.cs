using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using RhinoMeshTables.Core.MeshElements;
using RhinoMeshTables.Core.Tables;

namespace RhinoMeshTablesIO
{
    public class RhinoMeshExtractor : IMeshExtractor<Mesh>
    {
        public Mesh Mesh { get; }

        public RhinoMeshExtractor(Mesh mesh)
        {
            Mesh = mesh;

        }

        public Face[] ExtractFaces()
        {
            var faces = new Face[Mesh.GetNgonAndFacesCount()];
            var iter = 0;
            foreach (var face in Mesh.GetNgonAndFacesEnumerable())
            {
                var topoVertIndices =
                    from vert in face.BoundaryVertexIndexList()
                    select (uint)Mesh.TopologyVertices.TopologyVertexIndex((int)vert);
                faces[iter] = new Face(topoVertIndices);
                iter += 1;
            }

            return faces;
        }

        public Vertex[] ExtractVertices()
        {
            var vertices = new Vertex[Mesh.TopologyVertices.Count];
            for (int i = 0; i < Mesh.TopologyVertices.Count; i++)
            {
                vertices[i] = new Vertex(Mesh.TopologyVertices[i]);
            }

            return vertices;
        }

        private Dictionary<uint, int> GetFaceNgonTable()
        {
            var fn_table = new Dictionary<uint, int>();
            var ngons = Mesh.GetNgonAndFacesEnumerable().ToArray();
            for (int i = 0; i < ngons.Length; i++)
            {
                foreach (var index in ngons[i].FaceIndexList())
                {
                    fn_table[index] = i;
                }
            }

            return fn_table;
        }

        public Edge[] ExtractEdges()
        {
            var edgeIndices = (from index in Enumerable.Range(0, Mesh.TopologyEdges.Count)
                where !Mesh.TopologyEdges.IsNgonInterior(index)
                select index).ToArray();
            var edges = new Edge[edgeIndices.Length];
            var fn_table = GetFaceNgonTable();
            for (int i = 0; i < edgeIndices.Length; i++)
            {
                var verts = from vIndex in Mesh.TopologyEdges.GetTopologyVertices(edgeIndices[i])
                    select (uint)vIndex;
                var faces = from fIndex in Mesh.TopologyEdges.GetConnectedFaces(edgeIndices[i]) select (uint)fIndex;

                var nGonSet = new HashSet<int>();
                foreach (var faceIndex in faces)
                {
                    nGonSet.Add(fn_table[faceIndex]);
                }

                faces = from ngon in nGonSet select (uint)ngon;

                edges[i] = new Edge(verts, faces);
            }

            return edges;
        }
    }
}
