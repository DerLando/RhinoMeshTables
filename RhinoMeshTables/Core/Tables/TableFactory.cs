using System.Collections.Generic;
using Rhino.Geometry;
using RhinoMeshTables.Core.MeshElements;

namespace RhinoMeshTables.Core.Tables
{
    public static class TableFactory
    {
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

        public static FaceVertexTable CreateFaceVertexTable(Mesh mesh)
        {
            return new FaceVertexTable(GetFaces(mesh), GetVertices(mesh));
        }
    }
}
