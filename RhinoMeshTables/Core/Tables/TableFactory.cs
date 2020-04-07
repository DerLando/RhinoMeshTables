using System.Collections.Generic;
using System.Linq;
using RhinoMeshTables.Core.MeshElements;

namespace RhinoMeshTables.Core.Tables
{
    public static class TableFactory
    {
        #region Element getters

        public static void GetElements<T>(IMeshExtractor<T> extractor, out Vertex[] vertices, out Face[] faces, out Edge[] edges)
        {
            vertices = extractor.ExtractVertices();
            faces = extractor.ExtractFaces();
            edges = extractor.ExtractEdges();
        }

        #endregion

        public static FaceVertexTable CreateFaceVertexTable<T>(IMeshExtractor<T> extractor)
        {
            return new FaceVertexTable(extractor.ExtractFaces(), extractor.ExtractVertices());
        }

        public static EdgeVertexTable CreateEdgeVertexTable<T>(IMeshExtractor<T> extractor)
        {
            return new EdgeVertexTable(extractor.ExtractEdges(), extractor.ExtractVertices());
        }

        public static EdgeFaceTable CreateEdgeFaceTable<T>(IMeshExtractor<T> extractor)
        {
            return new EdgeFaceTable(extractor.ExtractEdges(), extractor.ExtractFaces());
        }
    }
}
