using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeshTables.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RhinoMeshTables.Core.Tables;

namespace RhinoPluginTests
{
    [TestClass]
    public class EdgeVertexTests
    {
        [TestMethod]
        public void TestEdgeVertexTableCreate()
        {
            // Arrange
            var testMesh = Helpers.Cube();

            // Act
            var fvTable = TableFactory.CreateEdgeVertexTable(testMesh);

            // Assert
            Assert.AreEqual(fvTable.EdgeCount, testMesh.TopologyEdges.Count);
            Assert.AreEqual(fvTable.VertexCount, testMesh.TopologyVertices.Count);
        }
    }
}
