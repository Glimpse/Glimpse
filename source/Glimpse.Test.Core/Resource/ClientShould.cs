using System;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;
using Glimpse.Core.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Resource
{
    public class ClientShould
    {
        [Fact]
        public void ProvideProperName()
        {
            var resource = new ClientResource();
            Assert.Equal("glimpse_client", resource.Name);
        }

        [Fact]
        public void ReturnOneParameterKeys()
        {
            var resource = new ClientResource();
            Assert.Equal(1, resource.Parameters.Count());
        }

        [Fact]
        public void ThrowExceptionWithNullParameters()
        {
            var resource = new ClientResource();

            Assert.Throws<ArgumentNullException>(() => resource.Execute(null));
        }

        [Fact]
        public void ReturnStatusCodeResourceResultWithMissingResource()
        {
            var contextMock = new Mock<IResourceContext>();

            var resource = new ClientResourceWithBadResourceName();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result as StatusCodeResourceResult);
        }

        private class ClientResourceWithBadResourceName : ClientResource
        {
            protected override EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context)
            {
                return new EmbeddedResourceInfo(this.GetType().Assembly, "wrong", "Doesn't Matter");
            }
        }

        [Fact]
        public void ReturnFileResourceResultWithResource()
        {
            var contextMock = new Mock<IResourceContext>();

            var resource = new ClientResource();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result);
        }
    }
}