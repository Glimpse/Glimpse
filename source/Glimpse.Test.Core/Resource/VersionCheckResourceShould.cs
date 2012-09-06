using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;
using Glimpse.Core.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Resource
{
    public class VersionCheckResourceShould
    {
        [Fact]
        public void HaveProperName()
        {
            var resource = new VersionCheckResource();

            Assert.Equal(VersionCheckResource.InternalName, resource.Name);
        }

        [Fact]
        public void ContainParameters()
        {
            var resource = new VersionCheckResource();
            Assert.NotEmpty(resource.Parameters);
        }

        [Fact]
        public void ReturnRedirectResult()
        {
            var resource = new VersionCheckResource();

            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.Parameters).Returns(new Dictionary<string, string> {{"stamp", "stamp"},{"callback","callback"}});

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result as RedirectResourceResult);
        }
    }
}