using System;
using System.Linq;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Resource;
using Glimpse.Core2.ResourceResult;
using Moq;
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
        public void ReturnOneParameterKeys()
        {
            var resource = new Client();
            Assert.Equal(1, resource.ParameterKeys.Count());
        }

        [Fact]
        public void ThrowExceptionWithNullParameters()
        {
            var resource = new Client();

            Assert.Throws<ArgumentNullException>(() => resource.Execute(null));
        }

        [Fact]
        public void ReturnStatusCodeResourceResultWithMissingResource()
        {
            var contextMock = new Mock<IResourceContext>();

            var resource = new Client {ResourceName = "wrong"};


            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result as StatusCodeResourceResult);
        }

        [Fact]
        public void ReturnFileResourceResultWithResource()
        {
            var contextMock = new Mock<IResourceContext>();

            var resource = new Client();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result as FileResourceResult);
        }
    }
}