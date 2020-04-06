using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeshTables.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Geometry;
using RhinoMeshTables.Core.Indices;
using RhinoMeshTables.Core.MeshElements;

namespace RhinoPluginTests
{
    [TestClass]
    public class ConnectivityTests
    {
        [TestMethod]
        public void TestConnectivityCreate()
        {
            // Arrange
            var testMesh = Helpers.Cube();

            // Act
            var connectivity = new MeshConnectivity(testMesh);

            // Assert
            Assert.AreEqual(8, connectivity.VertexCount);
            Assert.AreEqual(6, connectivity.FaceCount);
            Assert.AreEqual(12, connectivity.EdgeCount);

            foreach (var face in connectivity.GetFaces())
            {
                Assert.IsTrue(face.VertexIndices.All(i => i.Value < connectivity.VertexCount));
            }

            foreach (var edge in connectivity.GetEdges())
            {
                Assert.IsTrue(edge.VertexIndices.All(i => i.Value < connectivity.VertexCount));
                Assert.IsTrue(edge.FaceIndices.All(i => i.Value < connectivity.FaceCount));
            }
        }

        [TestMethod]
        public void TestVertexNeighborConnectivity()
        {
            // Arrange
            var square = Helpers.Square();

            // Act
            var connectivity = new MeshConnectivity(square);
            var connectedToFirst = connectivity.GetVertexNeighborIndices(new VertexIndex(0));

            // Assert
            Assert.IsTrue(connectedToFirst.Contains(new VertexIndex(1)));
            Assert.IsTrue(connectedToFirst.Contains(new VertexIndex(3)));
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(2, connectivity.GetVertexNeighborIndices(new VertexIndex((uint) i)).Length);
            }
        }

        [TestMethod]
        public void TestNormalCreate()
        {
            // Arrange
            var testMesh = Helpers.Pentagon();

            // Act
            var connectivity = new MeshConnectivity(testMesh);
            var normal = connectivity.GetNormal(0);

            // Assert
            Assert.AreEqual(Vector3d.ZAxis, normal);
        }
    }
}
