using System;
using MeshTableLibrary.Core.Indices;
using MeshTableLibrary.Core.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Geometry;
using RhinoMeshTablesIO;

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
            var evTable = TableFactory.CreateFaceVertexTable(new RhinoMeshExtractor(testMesh));

            // Assert
            Assert.AreEqual(6, evTable.FaceCount);
            Assert.AreEqual(8, evTable.VertexCount);
        }

        ///test every Vertex is contained in the face the fvTable finds for it
        [TestMethod]
        public void TestFaceVertexOneToOneRelation()
        {
            // Arrange
            var testMesh = Helpers.Cube();

            // Act
            var fvTable = TableFactory.CreateFaceVertexTable(new RhinoMeshExtractor(testMesh));

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

        [TestMethod]
        public void TestNgonValidity()
        {
            // Arrange
            var pentagon = Helpers.Pentagon();

            // Act
            var fvTable = TableFactory.CreateFaceVertexTable(new RhinoMeshExtractor(pentagon));

            // Assert
            Assert.AreEqual(1, fvTable.FaceCount);
            Assert.AreEqual(5, fvTable[new FaceIndex(0)].Length);
        }
    }
}
