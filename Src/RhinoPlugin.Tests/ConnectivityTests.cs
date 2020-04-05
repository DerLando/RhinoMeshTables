using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeshTables.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Assert.AreEqual(connectivity.VertexCount, testMesh.TopologyVertices.Count);
            Assert.AreEqual(connectivity.FaceCount, testMesh.GetNgonAndFacesCount());
            Assert.AreEqual(connectivity.EdgeCount, testMesh.TopologyEdges.Count);
        }
    }
}
