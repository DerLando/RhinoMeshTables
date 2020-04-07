using System;
using System.Collections.Generic;
using System.Linq;
using MeshTableLibrary.Core.Indices;
using MeshTableLibrary.Core.Tables;

namespace MeshTableLibrary.Core.MeshElements
{
    public static class PairFactory
    {
        #region Face pairs

        private static IndexPair<FaceIndex>[] _getFacePairs<T>(MeshConnectivity<T> connectivity)
        {
            var pairSet = new HashSet<IndexPair<FaceIndex>>();
            foreach (var faceIndex in connectivity.GetFaceIndices())
            {
                foreach (var pairIndex in connectivity.GetFaceNeighborIndices(faceIndex))
                {
                    var pair = new IndexPair<FaceIndex>(faceIndex, pairIndex);
                    pairSet.Add(pair);
                }
            }

            return pairSet.ToArray();
        }

        private static FaceAngle _calculateFacePairAngle<T>(MeshConnectivity<T> connectivity, IndexPair<FaceIndex> indices, EdgeIndex sharedEdgeIndex)
        {
            return new FaceAngle(connectivity.GetNormal(indices.FirstIndex), connectivity.GetNormal(indices.SecondIndex),
                connectivity.GetEdgeDirection(sharedEdgeIndex));
        }

        private static FacePairAngleType _calculateFacePairAngleType<T>(MeshConnectivity<T> connectivity, IndexPair<FaceIndex> indexPair)
        {
            var firstCentroid = connectivity.GetFaceCentroid(indexPair.FirstIndex);
            var secondCentroid = connectivity.GetFaceCentroid(indexPair.SecondIndex);
            var firstNormal = connectivity.GetNormal(indexPair.FirstIndex);
            var secondNormal = connectivity.GetNormal(indexPair.SecondIndex);

            var dist = firstCentroid.DistanceToSquared(secondCentroid);

            firstCentroid += (firstNormal * dist);
            secondCentroid += (secondNormal * dist);

            var newDist = firstCentroid.DistanceToSquared(secondCentroid);

            if (dist < newDist) return FacePairAngleType.Hill;
            if (dist == newDist) return FacePairAngleType.Saddle;
            if (dist > newDist) return FacePairAngleType.Valley;

            return FacePairAngleType.Saddle;
        }

        public static FacePair[] GetFacePairs<T>(MeshConnectivity<T> connectivity)
        {
            var indexPairs = _getFacePairs(connectivity);
            var facePairs = new FacePair[indexPairs.Length];

            for (int i = 0; i < indexPairs.Length; i++)
            {
                var indexPair = indexPairs[i];
                var edgeIndex = connectivity.GetSharedEdgeIndex(indexPair);
                var angle = _calculateFacePairAngle(connectivity, indexPair, edgeIndex);
                var angleType = _calculateFacePairAngleType(connectivity, indexPair);

                facePairs[i] = new FacePair(indexPair, angle, edgeIndex, angleType);
            }

            return facePairs;
        }

        public static FacePairTable CalculateFacePairTable(in FacePair[] facePairs)
        {
            return new FacePairTable(facePairs);
        }

        #endregion
    }
}
