using BookSmasher.src.controller;
using System.Collections.Generic;
using Xunit;

namespace BookSmasherTest
{
    public class UtilTest
    {
        [Fact]
        public void FindModeBasicTest()
        {
            var testList = new List<int>(new int[] { 1, 1, 1 });

            Assert.Equal(1, ArrayUtil.FindMode(testList));
        }

        [Fact]
        public void FindModeEmptyTest()
        {
            var testList = new List<int>(new int[] {});
            Assert.Equal(0, ArrayUtil.FindMode(testList));
        }

        [Fact]
        public void FindModeNoModeTest()
        {
            var testList = new List<int>(new int[] { 1, 2, 3 });
            Assert.Equal(1, ArrayUtil.FindMode(testList));
        }

        [Fact]
        public void FindModeLargeTest()
        {
            var testList = new List<int>(new int[] { -1, -1, 1, -1, 2, 2, 2, -1, 1, 1, -1, 3 });
            Assert.Equal(-1, ArrayUtil.FindMode(testList));
        }
    }
}
