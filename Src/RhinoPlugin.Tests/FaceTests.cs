using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Geometry;
using RhinoMeshTables.Core.Tables;

namespace MeshTables.Tests
{
    [TestClass]
    public class FaceTests
    {
        [TestMethod]
        public void TestFaceVertexTableCreate()
        {
            // Arrange
            var testMesh = Helpers.Cube();

            // Act
            var fvTable = TableFactory.CreateFaceVertexTable(testMesh);

            // Assert
            Assert.AreEqual(fvTable.Count(), testMesh.Faces.Count);
        }
    }
}
