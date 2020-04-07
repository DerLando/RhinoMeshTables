using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeshTableLibrary.Core.Indices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RhinoPluginTests
{
    [TestClass]
    public class IndexPairTests
    {
        [TestMethod]
        public void TestIndexPairEquality()
        {
            // Arrange
            var firstPair = new IndexPair<int>(0, 1);
            var secondPair = new IndexPair<int>(1, 0);
            var thirdPair = new IndexPair<int>(0, 2);

            // Act
            var firstSecond = firstPair.Equals(secondPair);
            var firstThird = firstPair.Equals(thirdPair);
            var set = new HashSet<IndexPair<int>> {firstPair, secondPair, thirdPair};

            // Assert
            Assert.IsTrue(firstSecond);
            Assert.IsFalse(firstThird);
            Assert.AreEqual(2, set.Count);
        }
    }
}
