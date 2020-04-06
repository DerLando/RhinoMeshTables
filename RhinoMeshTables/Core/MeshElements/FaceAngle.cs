using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace RhinoMeshTables.Core.MeshElements
{
    public readonly struct FaceAngle
    {
        public readonly double Min;
        public readonly double Max;

        public FaceAngle(Vector3d f1Normal, Vector3d f2Normal, Vector3d sharedEdgeNormal)
        {
            var plane = new Plane(Point3d.Origin, sharedEdgeNormal);
            var cp = Vector3d.CrossProduct(f1Normal, f2Normal);
            if(cp * sharedEdgeNormal < 0) plane.Flip();

            var angle = Vector3d.VectorAngle(f1Normal, f2Normal, plane);
            CalculateMinMax(angle, out Min, out Max);
        }

        private static void CalculateMinMax(double angle, out double min, out double max)
        {
            if (angle <= Math.PI)
            {
                min = angle;
                max = 2 * Math.PI - angle;
            }

            else
            {
                min = 2 * Math.PI - angle;
                max = angle;
            }
        }
    }
}
