using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Resource;
using Glimpse.Core.ResourceResult;
using Glimpse.Test.Core.Extensions;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Resource
{
    public class ConfigurationShould
    {
        [Fact]
        public void ReturnProperName()
        {
            var name = "glimpse_config";

            var resource = new ConfigurationResource();

            Assert.Equal(name, ConfigurationResource.InternalName);
            Assert.Equal(name, resource.Name);
        }

        [Fact]
        public void ReturnNoParameterKeys()
        {
            var resource = new ConfigurationResource();
            Assert.Empty(resource.Parameters);
        }

        [Fact]
        public void NotSupportNonPrivilegedExecution()
        {
            var resource = new PopupResource();
            var contextMock = new Mock<IResourceContext>();

            Assert.Throws<NotSupportedException>(() => resource.Execute(contextMock.Object));
        }

        [Fact(Skip = "Need to build out correct test here")]
        public void Execute()
        {
            var contextMock = new Mock<IResourceContext>();
            var configMock = new Mock<IGlimpseConfiguration>();

            var resource = new ConfigurationResource();
            var providerMock = new Mock<IFrameworkProvider>().Setup();

            var result = resource.Execute(contextMock.Object, configMock.Object, providerMock.Object);
            var htmlResourceResult = result as HtmlResourceResult;

            Assert.NotNull(result);
        }
    }
}