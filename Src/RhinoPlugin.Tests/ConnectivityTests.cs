using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeshTables.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Geometry;
using RhinoMeshTables.Core.Indices;
using RhinoMeshTables.Core.Math;
using RhinoMeshTables.Core.MeshElements;
using RhinoMeshTablesIO;

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
            var connectivity = new MeshConnectivity<Mesh>(new RhinoMeshExtractor(testMesh));

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
            var testMesh = Helpers.Square();

            // Act
            var connectivity = new MeshConnectivity<Mesh>(new RhinoMeshExtractor(testMesh));
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
            var cube = Helpers.Cube();

            // Act
            var connectivity = new MeshConnectivity<Mesh>(new RhinoMeshExtractor(testMesh));
            var normal = connectivity.GetNormal(0);
            var cubeConnectivity = new MeshConnectivity<Mesh>(new RhinoMeshExtractor(cube));

            // Assert
            Assert.AreEqual(new Vector3(0, 0, 1), normal);
            for (int i = 0; i < 6; i++)
            {
                var fNormal = cubeConnectivity.GetNormal(i);
                Assert.IsTrue(Helpers.IsOrthogonal(fNormal));
            }
        }

        [TestMethod]
        public void TestFaceAngles()
        {
            // Arrange
            var testMesh = Helpers.Cube();

            // Act
            var connectivity = new MeshConnectivity<Mesh>(new RhinoMeshExtractor(testMesh));

            // Assert
            foreach (var facePair in connectivity.GetFacePairs())
            {
                Assert.AreEqual(Math.PI / 2.0, facePair.Angle.Min);
                Assert.AreEqual(Math.PI * 1.5, facePair.Angle.Max);
            }
        }
    }
}
