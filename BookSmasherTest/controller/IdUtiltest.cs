using BookSmasher.src.controller;
using System.Collections.Generic;
using Xunit;

namespace BookSmasherTest.controller
{
    public class IdUtilTest
    {
        [Fact]
        public void IdAddedEmptyTest()
        {
            var testList = new List<string>(new string[] {});

            Assert.False(IdUtil.IdAlreadyAdded("cat", testList));
        }

        [Fact]
        public void IdAddedTest()
        {
            var testList = new List<string>(new string[] {"cat"});

            Assert.True(IdUtil.IdAlreadyAdded("cat", testList));
        }

        [Fact]
        public void IdNotAddedTest()
        {
            var testList = new List<string>(new string[] {"fish", "dog", "rabbit" });

            Assert.False(IdUtil.IdAlreadyAdded("cat", testList));
        }

        [Fact]
        public void ValidIdTest()
        {
            Assert.True(IdUtil.IsValid("cat"));
        }

        [Fact]
        public void InvalidIdEmptyTest()
        {
            Assert.False(IdUtil.IsValid(" "));
        }

        [Fact]
        public void InvalidIdNullTest()
        {
            Assert.False(IdUtil.IsValid(null));
        }
    }
}
