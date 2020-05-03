using MeshTableLibrary.Core.Math;

namespace MeshTableLibrary.Core.MeshElements
{
    /// <summary>
    /// A data struct containing information about the angle between two faces
    /// </summary>
    public readonly struct FaceAngle
    {
        /// <summary>
        /// The minimum angle between the two faces, between 0 and Pi
        /// </summary>
        public readonly double Min;

        /// <summary>
        /// The maximum (reflex) angle between the to faces, is 2*Pi - Min
        /// </summary>
        public readonly double Max;

        public FaceAngle(Vector3 f1Normal, Vector3 f2Normal, Vector3 sharedEdgeNormal)
        {
            var cp = Vector3.CrossProduct(f1Normal, f2Normal);
            if (cp * sharedEdgeNormal < 0) sharedEdgeNormal = -sharedEdgeNormal;

            var angle = Vector3.VectorAngle(f1Normal, f2Normal, sharedEdgeNormal);
            CalculateMinMax(angle, out Min, out Max);
        }

        private static void CalculateMinMax(double angle, out double min, out double max)
        {
            // angle is in the range of -pi to pi, we want it to be between 0 and 2pi
            //angle += System.Math.PI;
            //if (angle <= System.Math.PI)
            //{
            //    min = angle;
            //    max = 2 * System.Math.PI - angle;
            //}

            //else
            //{
            //    min = 2 * System.Math.PI - angle;
            //    max = angle;
            //}

            // angle is in the range of 0 to pi
            //if (angle <= System.Math.PI / 2.0)
            //{
            //    min = angle;
            //    max = 2 * System.Math.PI - angle;
            //}

            //else
            //{
            //    min = 2 * System.Math.PI - angle;
            //    max = angle;
            //}

            min = angle;
            max = 2 * System.Math.PI - angle;
        }
    }
}
