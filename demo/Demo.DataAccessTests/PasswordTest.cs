using Demo.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Demo.Tests
{
    public class PasswordTest
    {
        [Fact]
        public void Password()
        {
            var password = "admin123";
            password = password.ToPassword();
            Assert.NotNull(password);
        }
    }
}
