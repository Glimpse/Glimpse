using System;
using System.Collections.Generic;
using Glimpse.Test.Core.Tester;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class ResourceParametersShould:IDisposable
    {
        private ResourceParametersTester tester;

        public ResourceParametersTester ResourceParameters
        {
            get { return tester ?? (tester = ResourceParametersTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            ResourceParameters = null;
        }

        [Fact]
        public void ShouldLeverageNamedParamsFirst()
        {
            var namedParams = new Dictionary<string, string>
                                  {
                                      {"key", "value"}
                                  };

            ResourceParameters.NamedParameters = namedParams;

            Assert.Equal(namedParams, ResourceParameters.GetParametersFor(ResourceParameters.ResourceMock.Object));
        }

        [Fact]
        public void ShouldLeverageOrderedParamsSecond()
        {
            var orderedParams = new[] {"1", "2"};

            ResourceParameters.OrderedParameters = orderedParams;

            var result = ResourceParameters.GetParametersFor(ResourceParameters.ResourceMock.Object);

            Assert.Equal("1", result["One"]);
            Assert.Equal("2", result["Two"]);
            Assert.Equal(null, result["Three"]);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void ShouldReturnEmptyDictionaryWithNoInputParams()
        {
            var result = ResourceParameters.GetParametersFor(ResourceParameters.ResourceMock.Object);

            Assert.Equal(0, result.Count);
        }
    }
}