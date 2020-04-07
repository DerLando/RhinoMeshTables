using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeshTables.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Geometry;
using RhinoMeshTables.Core.Indices;
using RhinoMeshTables.Core.Tables;
using RhinoMeshTablesIO;

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
            var fvTable = TableFactory.CreateEdgeFaceTable(new RhinoMeshExtractor(testMesh));

            // Assert
            Assert.AreEqual(12, fvTable.EdgeCount);
            Assert.AreEqual(6, fvTable.FaceCount);
        }

        [TestMethod]
        public void TestNgonValidity()
        {
            // Arrange
            var testMesh = Helpers.Pentagon();

            // Act
            var evTable = TableFactory.CreateEdgeFaceTable(new RhinoMeshExtractor(testMesh));

            // Assert
            Assert.AreEqual(1, evTable.FaceCount);
            Assert.AreEqual(5, evTable.EdgeCount);
            Assert.AreEqual(5, evTable[new FaceIndex(0)].Length);
        }
    }
}
