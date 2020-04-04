using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Geometry;
using RhinoMeshTables.Core.Indices;
using RhinoMeshTables.Core.Tables;

namespace MeshTables.Tests
{
    [TestClass]
    public class FaceVertexTests
    {
        [TestMethod]
        public void TestFaceVertexTableCreate()
        {
            // Arrange
            var testMesh = Helpers.Cube();

            // Act
            var fvTable = TableFactory.CreateFaceVertexTable(testMesh);

            // Assert
            Assert.AreEqual(fvTable.FaceCount, testMesh.Faces.Count);
            Assert.AreEqual(fvTable.VertexCount, testMesh.Vertices.Count);
        }

        ///test every Vertex is contained in the face the fvTable finds for it
        [TestMethod]
        public void TestFaceVertexOneToOneRelation()
        {
            // Arrange
            var testMesh = Helpers.Cube();

            // Act
            var fvTable = TableFactory.CreateFaceVertexTable(testMesh);

            // Assert
            for (int i = 0; i < testMesh.TopologyVertices.Count; i++)
            {
                var index = new VertexIndex((uint)i);
                foreach (var face in fvTable[index])
                {
                    Assert.IsTrue(Array.Exists(face.VertexIndices, v => v.Equals(index)));
                }
            }
        }
    }
}
