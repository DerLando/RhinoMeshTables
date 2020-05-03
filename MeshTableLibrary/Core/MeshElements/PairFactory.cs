using System;
using System.Collections.Generic;
using System.Linq;
using MeshTableLibrary.Core.Indices;
using MeshTableLibrary.Core.Tables;

namespace MeshTableLibrary.Core.MeshElements
{
    /// <summary>
    /// A Factory to create pairs of faces
    /// </summary>
    public static class PairFactory
    {
        #region Face pairs

        /// <summary>
        /// Gets FacePairs of neighboring faces defined in the given MeshConnectivity
        /// </summary>
        /// <typeparam name="T">The type of mesh the MeshConnectivity is defined for</typeparam>
        /// <param name="connectivity">The connectivity information</param>
        /// <returns></returns>
        private static IndexPair<FaceIndex>[] _getFacePairs<T>(MeshConnectivity<T> connectivity)
        {
            // create a Hashset to store unique face pairs
            var pairSet = new HashSet<IndexPair<FaceIndex>>();

            // iterate over all face indices
            foreach (var faceIndex in connectivity.GetFaceIndices())
            {
                // iterate over all faces connected to the current faceIndex
                foreach (var pairIndex in connectivity.GetFaceNeighborIndices(faceIndex))
                {
                    // create a new FacePair
                    var pair = new IndexPair<FaceIndex>(faceIndex, pairIndex);

                    // Add it to the set, duplicates won't be added
                    pairSet.Add(pair);
                }
            }

            return pairSet.ToArray();
        }

        /// <summary>
        /// Calculate the FaceAngle between two neighboring faces
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectivity"></param>
        /// <param name="indices"></param>
        /// <param name="sharedEdgeIndex"></param>
        /// <returns></returns>
        private static FaceAngle _calculateFacePairAngle<T>(MeshConnectivity<T> connectivity, IndexPair<FaceIndex> indices, EdgeIndex sharedEdgeIndex)
        {
            return new FaceAngle(connectivity.GetNormal(indices.FirstIndex), connectivity.GetNormal(indices.SecondIndex),
                connectivity.GetEdgeDirection(sharedEdgeIndex));
        }

        /// <summary>
        /// Calculate the type of FaceAngle, the two given neighboring faces define
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectivity"></param>
        /// <param name="indexPair"></param>
        /// <returns></returns>
        private static FacePairAngleType _calculateFacePairAngleType<T>(MeshConnectivity<T> connectivity, IndexPair<FaceIndex> indexPair)
        {
            // get centroids and normals of both faces
            var firstCentroid = connectivity.GetFaceCentroid(indexPair.FirstIndex);
            var secondCentroid = connectivity.GetFaceCentroid(indexPair.SecondIndex);
            var firstNormal = connectivity.GetNormal(indexPair.FirstIndex);
            var secondNormal = connectivity.GetNormal(indexPair.SecondIndex);

            // calculate the distance between both centroids
            var dist = firstCentroid.DistanceToSquared(secondCentroid);

            // move the centroids along their face normal
            firstCentroid += firstNormal;
            secondCentroid += secondNormal;

            // calculate the distance between the moved centroids
            var newDist = firstCentroid.DistanceToSquared(secondCentroid);

            // if the centroids are now further apart, it must be a sharp angle
            if (dist < newDist) return FacePairAngleType.Hill;

            // if the distance did not change much, the angle must be close to zero
            if (System.Math.Abs(dist - newDist) < 0.001) return FacePairAngleType.Saddle;

            // if the centroids are now closer together, it must be a dull angle
            if (dist > newDist) return FacePairAngleType.Valley;

            // We should never get here
            return FacePairAngleType.Saddle;
        }

        /// <summary>
        /// Get all FacePairs of neighboring faces defined in the given connectivity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectivity"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Calculates the <see cref="FacePairTable"/> defined for a given array of face pairs
        /// </summary>
        /// <param name="facePairs"></param>
        /// <returns></returns>
        public static FacePairTable CalculateFacePairTable(in FacePair[] facePairs)
        {
            return new FacePairTable(facePairs);
        }

        #endregion
    }
}
