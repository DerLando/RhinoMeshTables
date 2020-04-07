using MeshTableLibrary.Core.Indices;

namespace MeshTableLibrary.Core.MeshElements
{
    public readonly struct FacePair
    {
        public readonly IndexPair<FaceIndex> Indices;
        public readonly FaceAngle Angle;
        public readonly EdgeIndex SharedEdgeIndex;

        public FacePair(IndexPair<FaceIndex> indices,FaceAngle angle, EdgeIndex sharedEdgeIndex)
        {
            Indices = indices;
            Angle = angle;
            SharedEdgeIndex = sharedEdgeIndex;
        }
    }
}
