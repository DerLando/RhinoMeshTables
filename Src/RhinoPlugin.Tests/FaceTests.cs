using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeshTableLibrary.Core.Indices;
using MeshTableLibrary.Core.Math;
using MeshTableLibrary.Core.MeshElements;
using MeshTables.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Geometry;
using RhinoMeshTablesIO;

namespace RhinoPluginTests
{
    [TestClass]
    public class FaceTests
    {
        [TestMethod]
        public void TestFaceCentroids()
        {
            // Arrange
            var testMesh = Helpers.Square();
            var connectivity = new MeshConnectivity<Mesh>(new RhinoMeshExtractor(testMesh));

            // Act
            var centroid = connectivity.GetFaceCentroid(new FaceIndex(0));

            // Assert
            Assert.AreEqual(Vector3.Zero(), centroid);
        }

        [TestMethod]
        public void TestFaceAngleTypes()
        {
            // Arrange
            var testMesh = Helpers.Cube();
            var connectivity = new MeshConnectivity<Mesh>(new RhinoMeshExtractor(testMesh));

            // Act
            
            // Assert
            foreach (var facePair in connectivity.GetFacePairs())
            {
                Assert.AreEqual(FacePairAngleType.Hill, facePair.AngleType);
            }
        }
    }
}
