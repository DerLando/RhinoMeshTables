using System.Collections.Generic;
using Rhino.Geometry;
using RhinoMeshTables.Core.MeshElements;

namespace RhinoMeshTables.Core.Tables
{
    public static class TableFactory
    {
        private static Face[] GetFaces(Mesh mesh)
        {
            List<Face> faces = new List<Face>();
            foreach (var face in mesh.GetNgonAndFacesEnumerable())
            {
                faces.Add(new Face(face.BoundaryVertexIndexList()));
            }

            return faces.ToArray();
        }

        public static FaceVertexTable CreateFaceVertexTable(Mesh mesh)
        {
            var faces = GetFaces(mesh);
            return new FaceVertexTable(faces);
        }
    }
}
