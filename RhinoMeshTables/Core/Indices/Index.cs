using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoMeshTables.Core.Indices
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
            return FirstIndex.GetHashCode() + SecondIndex.GetHashCode();
        }
    }

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

    public readonly struct FaceIndex : IValueIndex
    {
        public readonly uint Value;

        public FaceIndex(uint value)
        {
            Value = value;
        }
    }

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

    public readonly struct EdgeIndex : IValueIndex
    {
        public readonly uint Value;

        public EdgeIndex(uint value)
        {
            Value = value;
        }
    }
}
