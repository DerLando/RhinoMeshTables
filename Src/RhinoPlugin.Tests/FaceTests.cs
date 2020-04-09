using System;
using System.IO;
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
            var testMesh = Helpers.ImportFrom3dm("test_FaceAngleType.3dm");
            var connectivity = new MeshConnectivity<Mesh>(new RhinoMeshExtractor(testMesh));
            var hill = 0;
            var valley = 0;
            var saddle = 0;

            // Act
            foreach (var facePair in connectivity.GetFacePairs())
            {
                switch (facePair.AngleType)
                {
                    case FacePairAngleType.Hill:
                        hill += 1;
                        break;
                    case FacePairAngleType.Saddle:
                        saddle += 1;
                        break;
                    case FacePairAngleType.Valley:
                        valley += 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            // Assert
            Assert.AreEqual(5, hill);
            Assert.AreEqual(2, valley);
            Assert.AreEqual(1, saddle);
        }
    }
}
