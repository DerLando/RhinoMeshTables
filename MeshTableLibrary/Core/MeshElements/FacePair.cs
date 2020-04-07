using MeshTableLibrary.Core.Indices;

namespace MeshTableLibrary.Core.MeshElements
{
    public readonly struct FacePair
    {
        public readonly IndexPair<FaceIndex> Indices;
        public readonly FaceAngle Angle;
        public readonly EdgeIndex SharedEdgeIndex;
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
