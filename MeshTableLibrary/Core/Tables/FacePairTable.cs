using System.Collections.Generic;
using System.Linq;
using MeshTableLibrary.Core.Indices;
using MeshTableLibrary.Core.MeshElements;

namespace MeshTableLibrary.Core.Tables
{
    public class FacePairTable : TableBase
    {
        private readonly FacePair[] _pairs;
        private readonly Dictionary<IndexPair<FaceIndex>, int> _fp_dict;

        public FacePairTable(FacePair[] pairs)
        {
            _pairs = pairs;

            _fp_dict = CalculateFacePairDict();
        }

        private Dictionary<IndexPair<FaceIndex>, int> CalculateFacePairDict()
        {
            var fp_dict = new Dictionary<IndexPair<FaceIndex>, int>();

            for (int i = 0; i < _pairs.Length; i++)
            {
                fp_dict[_pairs[i].Indices] = i;
            }

            return fp_dict;
        }

        public FacePair GetFacePair(IndexPair<FaceIndex> indices) => _pairs[_fp_dict[indices]];
        public IEnumerable<FacePair> GetFacePairs() => _pairs.AsEnumerable();
    }
}
