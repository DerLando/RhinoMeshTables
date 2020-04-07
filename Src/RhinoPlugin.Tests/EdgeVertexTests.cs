using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeshTables.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RhinoMeshTables.Core.Tables;
using RhinoMeshTablesIO;

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
            var fvTable = TableFactory.CreateEdgeVertexTable(new RhinoMeshExtractor(testMesh));

            // Assert
            Assert.AreEqual(12, fvTable.EdgeCount);
            Assert.AreEqual(8, fvTable.VertexCount);
        }
    }
}
