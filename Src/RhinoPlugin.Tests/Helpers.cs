using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeshTableLibrary.Core.Math;
using Rhino.FileIO;
using Rhino.Geometry;

namespace MeshTables.Tests
{
    public static class Helpers
    {
        public static Mesh Cube()
        {
            var mesh = Mesh.CreateFromBox(new BoundingBox(-1, -1, -1, 1, 1, 1), 1, 1, 1);
            mesh.Weld(0.01);

            return mesh;
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

        public static bool IsOrthogonal(Vector3 vec)
        {
            if (vec.X == 0 && vec.Y == 0) return true;
            if (vec.Y == 0 && vec.Z == 0) return true;
            if (vec.Z == 0 && vec.X == 0) return true;
            return false;
        }

        private static string TEST_FILE_PATH = "..\\..\\..\\TestFiles\\";

        public static Mesh ImportFrom3dm(string fileName)
        {
            var file = File3dm.Read(Path.Combine(TEST_FILE_PATH, fileName), File3dm.TableTypeFilter.ObjectTable, File3dm.ObjectTypeFilter.Mesh);
            foreach (var fileObject in file.Objects)
            {
                if (fileObject.Geometry is Mesh mesh) return mesh;
            }

            return null;
        }
    }
}
