using Classifier.src.machineLearning;
using System;
using System.Collections.Generic;
using Xunit;

namespace BookSmasherTest
{
    public class UtilTest
    {
        [Fact]
        public void FindModeBasicTest()
        {
            // shouldn't have to do this to test extension method like thing
            var forest = new RandomForest(10, 10);

            var testList = new List<int>(new int[] { 1, 1, 1 });

            Assert.Equal(1, forest.FindMode(testList));
        }

        [Fact]
        public void FindModeEmptyTest()
        {
            var forest = new RandomForest(10, 10);
            var testList = new List<int>(new int[] {});
            Assert.Equal(0, forest.FindMode(testList));
        }

        [Fact]
        public void FindModeNoModeTest()
        {
            var forest = new RandomForest(10, 10);
            var testList = new List<int>(new int[] { 1, 2, 3 });
            Assert.Equal(1, forest.FindMode(testList));
        }

        [Fact]
        public void FindModeLargeTest()
        {
            var forest = new RandomForest(10, 10);
            var testList = new List<int>(new int[] { -1, -1, 1, -1, 2, 2, 2, -1, 1, 1, -1, 3 });
            Assert.Equal(-1, forest.FindMode(testList));
        }
    }
}
