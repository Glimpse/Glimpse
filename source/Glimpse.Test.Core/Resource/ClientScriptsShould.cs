using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Resource;
using Glimpse.Core.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Resource
{
    public class ClientScriptsShould
    {
        public void ProvideProperName()
        {
            var resource = new ClientScriptResource();
            Assert.Equal("glimpse_clientscripts", resource.Name);
        }

        [Fact]
        public void ReturnOneParameterKey()
        {
            var resource = new ClientScriptResource();
            Assert.Equal(1, resource.Parameters.Count());
        }

        [Fact]
        public void ThrowExceptionWithNullParameters()
        {
            var resource = new ClientScriptResource();

            Assert.Throws<ArgumentNullException>(() => resource.Execute(null));
        }

        [Fact]
        public void Return404StatusCodeResourceResultWithNonExistentOrder()
        {
            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.Parameters).Returns(new Dictionary<string, string> { { "order", "some-non-existant-thing-in-enum" } });

            var resource = new ClientScriptResource();

            var result = resource.Execute(contextMock.Object);
            
            var resourceResult = result as StatusCodeResourceResult;

            Assert.NotNull(resourceResult);
            Assert.Equal(resourceResult.StatusCode, 404);

        }

        [Fact(Skip="Waiting for implementation...")]
        public void ReturnScriptsForCurrentScriptOrder()
        {

        }

        [Fact(Skip = "Not fully sure how to test this...")]
        public void ReturnCachedResult()
        {
            //var contextMock = new Mock<IResourceContext>();
            //var resource = new ClientScriptResource();
            //var result = resource.Execute(contextMock.Object);

            //Assert.NotNull(result as CacheControlDecorator);
        }


    }
}
