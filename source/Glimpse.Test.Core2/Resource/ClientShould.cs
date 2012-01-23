using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Resource;
using Xunit;

namespace Glimpse.Test.Core2.Resource
{
    public class ClientShould
    {
        [Fact]
        public void ProvideProperName()
        {
            var resource = new Client();
            Assert.Equal("glimpse.js", resource.Name);
        }

        [Fact]
        public void ReturnTwoParameterKeys()
        {
            var resource = new Client();
            Assert.Equal(2, resource.ParameterKeys.Count());
        }

        [Fact]
        public void ThrowExceptionWithNullParameters()
        {
            var resource = new Client();

            Assert.Throws<ArgumentNullException>(() => resource.Execute(null));
        }

        [Fact]
        public void ThrowExceptionUntilImplemented()
        {
            var resource = new Client();

            Assert.Throws<NotImplementedException>(() => resource.Execute(new Dictionary<string, string>()));
        }
    }
}