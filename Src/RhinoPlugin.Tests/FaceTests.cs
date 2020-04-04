using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Geometry;
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
    }
}
