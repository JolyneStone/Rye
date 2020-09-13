using Xunit;
using Monica;

using System;
using System.Collections.Generic;
using System.Text;

namespace Monica.Tests
{
    public class ObjectExtensionsTests
    {
        public class Clone1
        {
            public string Name { get; set; }
        }

        public class Clone2: Clone1
        {

        }

        [Fact()]
        public void CloneTest()
        {
            var clone1 = new Clone1 { Name = "test" };
            var clone2 = clone1.Clone();
            Assert.Equal("test", clone2.Name);

            var clone3 = new Clone2();
            clone2.Clone(clone3);
            Assert.Equal("test", clone3.Name);
        }
    }
}