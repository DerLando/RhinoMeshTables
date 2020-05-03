using MeshTableLibrary.Core.Math;

namespace MeshTableLibrary.Core.MeshElements
{
    public readonly struct FaceAngle
    {
        public readonly double Min;
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
