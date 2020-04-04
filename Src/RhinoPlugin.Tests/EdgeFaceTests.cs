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
    public class EdgeFaceTests
    {
        [TestMethod]
        public void TestEdgeFaceTableCreate()
        {
            // Arrange
            var testMesh = Helpers.Cube();

            // Act
            var fvTable = TableFactory.CreateEdgeFaceTable(testMesh);

            // Assert
            Assert.AreEqual(fvTable.EdgeCount, testMesh.TopologyEdges.Count);
            Assert.AreEqual(fvTable.FaceCount, testMesh.GetNgonAndFacesCount());
        }
    }
}
