using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MeshTables.Tests
{
    public static class Helpers
    {
        public static Mesh Cube()
        {
            return Mesh.CreateFromBox(new BoundingBox(-1, -1, -1, 1, 1, 1), 1, 1, 1);
        }

        public static Mesh Square()
        {
            var mesh = new Mesh();
            var v0 = mesh.Vertices.Add(-0.5, -0.5, 0.0);
            var v1 = mesh.Vertices.Add(0.5, -0.5, 0.0);
            var v2 = mesh.Vertices.Add(0.5, 0.5, 0.0);
            var v3 = mesh.Vertices.Add(-0.5, 0.5, 0.0);

            mesh.Faces.AddFace(v0, v1, v2, v3);

            mesh.Normals.ComputeNormals();
            mesh.Compact();

            return mesh;
        }

        public static Mesh Pentagon()
        {
            var poly = Polyline.CreateInscribedPolygon(new Circle(Point3d.Origin, 1), 5);
            var mesh = Mesh.CreateFromPlanarBoundary(poly.ToPolylineCurve(), MeshingParameters.Default, 0.01);
            mesh.Ngons.AddPlanarNgons(0.01);

            return mesh;
        }
    }
}
