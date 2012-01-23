using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Resource;
using Xunit;

namespace Glimpse.Test.Core2.Resource
{
    public class DataShould
    {
        [Fact]
        public void ProvideProperName()
        {
            var resource = new Data();
            Assert.Equal("data.js", resource.Name);
        }

        [Fact]
        public void ReturnTwoParameterKeys()
        {
            var resource = new Data();
            Assert.Equal(2, resource.ParameterKeys.Count());
        }

        [Fact]
        public void ThrowExceptionWithNullParameters()
        {
            var resource = new Data();

            Assert.Throws<ArgumentNullException>(()=>resource.Execute(null));
        }

        [Fact]
        public void ThrowExceptionUntilImplemented()
        {
            var resource = new Data();

            Assert.Throws<NotImplementedException>(() => resource.Execute(new Dictionary<string, string>()));
        }
    }
}