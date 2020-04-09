namespace MeshTableLibrary.Core.Math
{
    public readonly struct Vector3
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"Vector3: X:{X}, Y:{Y}, Z:{Z}";
        }

        #region methods

        public double DistanceToSquared(in Vector3 other)
        {
            return (X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y) + (Z - other.Z) * (Z - other.Z);
        }

        public double Length()
        {
            return System.Math.Sqrt(DistanceToSquared(Zero()));
        }

        public Vector3 AsNormalized()
        {
            var length = this.Length();
            if (length == 0) return Vector3.Zero();
            return this / length;
        }

        #endregion

        #region operators

        public static Vector3 operator +(in Vector3 a) => a;
        public static Vector3 operator -(in Vector3 a) => new Vector3(-a.X, -a.Y, -a.Z);

        public static Vector3 operator +(in Vector3 a, in Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator -(in Vector3 a, in Vector3 b) => a + (-b);

        public static double operator *(in Vector3 a, in Vector3 b) => DotProduct(a, b);
        public static Vector3 operator *(in Vector3 a, double b) => new Vector3(a.X * b, a.Y * b, a.Z * b);
        public static Vector3 operator /(in Vector3 a, double b) => new Vector3(a.X / b, a.Y / b, a.Z / b);

        #endregion

        #region static methods

        public static Vector3 Zero() => new Vector3(0, 0, 0);

        public static Vector3 CrossProduct(in Vector3 a, in Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }

        public static double DotProduct(in Vector3 a, in Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static double VectorAngle(in Vector3 a, in Vector3 b, in Vector3 n)
        {
            var aNorm = a.AsNormalized();
            var bNorm = b.AsNormalized();

            if (aNorm.Equals(bNorm)) return 0;

            //https://stackoverflow.com/a/33920320
            //return System.Math.Atan2(CrossProduct(bNorm, aNorm) * n.AsNormalized(), bNorm * aNorm);

            // cos(a) = a*b / (len(a)*len(b))
            return System.Math.Acos(aNorm * bNorm);
        }

        #endregion
    }

}
