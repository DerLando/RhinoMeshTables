using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeshTableLibrary.Core.Math;
using Rhino.Geometry;

namespace RhinoMeshTablesIO
{
    public static class Extensions
    {
        public static Vector3 ToVector3(this Point3d pt)
        {
            return new Vector3(pt.X, pt.Y, pt.Z);
        }

        public static Vector3 ToVector3(this Point3f pt)
        {
            return new Vector3(pt.X, pt.Y, pt.Z);
        }
    }
}
