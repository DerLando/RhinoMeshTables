using MeshTableLibrary.Core.Indices;

namespace MeshTableLibrary.Core.MeshElements
{
    /// <summary>
    /// A pair of <see cref="Face"/>s, normally the faces in the pair are neighbors
    /// </summary>
    public readonly struct FacePair
    {
        /// <summary>
        /// Index pair of the two faces paired
        /// </summary>
        public readonly IndexPair<FaceIndex> Indices;

        /// <summary>
        /// The angle between the two faces
        /// </summary>
        public readonly FaceAngle Angle;

        /// <summary>
        /// The Edge index the two paired faces share
        /// </summary>
        public readonly EdgeIndex SharedEdgeIndex;

        /// <summary>
        /// The type of angle defined by the two faces
        /// </summary>
        public readonly FacePairAngleType AngleType;

        public FacePair(IndexPair<FaceIndex> indices,FaceAngle angle, EdgeIndex sharedEdgeIndex, FacePairAngleType angleType)
        {
            Indices = indices;
            Angle = angle;
            SharedEdgeIndex = sharedEdgeIndex;
            AngleType = angleType;
        }
    }
}
