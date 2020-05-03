namespace MeshTableLibrary.Core.Math
{
    /// <summary>
    /// Simple implementation of a 3d Vector as a readonly struct
    /// </summary>
    public readonly struct Vector3
    {
        /// <summary>
        /// The X coordinate of the Vector
        /// </summary>
        public readonly double X;

        /// <summary>
        /// The Y coordinate of the Vector
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// The Z coordinate of the Vector
        /// </summary>
        public readonly double Z;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="x">The X coordinate of the Vector</param>
        /// <param name="y">The Y coordinate of the Vector</param>
        /// <param name="z">The Z coordinate of the Vector</param>
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

        /// <summary>
        /// Calculates the squared distance between this and another <see cref="Vector3"/>
        /// This is the fastest way to compare distances between vectors
        /// </summary>
        /// <param name="other">The other vector to calculate the distance to</param>
        /// <returns>The calculated distance</returns>
        public double DistanceToSquared(in Vector3 other)
        {
            // Standard pythagorean distance formula 
            return (X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y) + (Z - other.Z) * (Z - other.Z);
        }

        /// <summary>
        /// Calculates the Length of this vector
        /// </summary>
        /// <returns>The calculated Length</returns>
        public double Length()
        {
            // The distance from a vector to the zero vector is its Length²
            return System.Math.Sqrt(DistanceToSquared(Vector3.Zero));
        }

        /// <summary>
        /// Returns a normalized copy of this vector,
        /// normalized meaning, the length of the vector is exactly 1.
        /// see also: <seealso cref="Vector3.Length()"/>
        /// </summary>
        /// <returns>The normalized copy</returns>
        public Vector3 AsNormalized()
        {
            var length = this.Length();
            if (length == 0) return Vector3.Zero;
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

        private static readonly Vector3 _zero = new Vector3(0, 0, 0);

        /// <summary>
        /// A static reference to the zero vector
        /// </summary>
        /// <returns></returns>
        public static ref readonly Vector3 Zero => ref _zero;

        /// <summary>
        /// Calculates the cross product between two vectors a and b
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns></returns>
        public static Vector3 CrossProduct(in Vector3 a, in Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }

        /// <summary>
        /// Calculates the dot product between two vectors a and b
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns></returns>
        public static double DotProduct(in Vector3 a, in Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        /// <summary>
        /// Calculates the vector angle between two vectors a and b normal to reference vector n
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <param name="n">The normal reference vector</param>
        /// <returns>The angle in radians between 0 and Pi</returns>
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
