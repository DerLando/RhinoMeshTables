using MeshTableLibrary.Core.MeshElements;

namespace MeshTableLibrary.Core.Tables
{
    public interface IMeshExtractor<out T>
    {
        T Mesh { get; }
        Face[] ExtractFaces();
        Vertex[] ExtractVertices();
        Edge[] ExtractEdges();
    }
}
