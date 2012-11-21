using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;
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
        public void Execute()
        {
            var contextMock = new Mock<IResourceContext>();

            var resource = new ConfigurationResource();
             
            Assert.NotNull(resource.Execute(contextMock.Object));
        }
    }
}