using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MeshTableLibrary.Core.Indices
{
    public interface IValueIndex { }

    /// <summary>
    /// Uniquely determined index pair wrapper
    /// Takes care of permutations of index pairs, e.g.
    /// new IndexPair(0, 1) == new IndexPair(1, 0)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct IndexPair<T> : IEnumerable<T>, IEquatable<IndexPair<T>>
    {
        public readonly T FirstIndex;
        public readonly T SecondIndex;

        public IndexPair(T firstIndex, T secondIndex)
        {
            FirstIndex = firstIndex;
            SecondIndex = secondIndex;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new[] {FirstIndex, SecondIndex}.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(IndexPair<T> other)
        {
            return new IndexPairEqualityComparer<T>().Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            return obj is IndexPair<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            var a = FirstIndex.GetHashCode();
            var b = SecondIndex.GetHashCode();
            return a * b + a + b;
        }
    }

    /// <summary>
    /// IEqualityComparer implementation, to determine equality of IndexPairs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class IndexPairEqualityComparer<T> : IEqualityComparer<IndexPair<T>>
    {
        public bool Equals(IndexPair<T> x, IndexPair<T> y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode(IndexPair<T> obj)
        {
            return obj.FirstIndex.GetHashCode() + obj.SecondIndex.GetHashCode();
        }
    }

    /// <summary>
    /// An index data struct storing a reference to a Face
    /// </summary>
    public readonly struct FaceIndex : IValueIndex
    {
        /// <summary>
        /// The index value
        /// </summary>
        public readonly uint Value;

        public FaceIndex(uint value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// An index data struct storing a reference to a Vertex
    /// </summary>
    public readonly struct VertexIndex : IEquatable<VertexIndex>, IValueIndex
    {
        public readonly uint Value;

        public VertexIndex(uint value)
        {
            Value = value;
        }

        public bool Equals(VertexIndex other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexIndex other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int) Value;
        }
    }

    /// <summary>
    /// An index data struct storing a reference to an Edge
    /// </summary>
    public readonly struct EdgeIndex : IValueIndex
    {
        public readonly uint Value;

        public EdgeIndex(uint value)
        {
            Value = value;
        }
    }
}
