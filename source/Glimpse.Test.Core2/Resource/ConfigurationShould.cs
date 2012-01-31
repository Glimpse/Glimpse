using System;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Resource;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Resource
{
    public class ConfigurationShould
    {
        [Fact]
        public void ReturnProperName()
        {
            var name = "config";

            var resource = new Configuration();

            Assert.Equal(name, Configuration.InternalName);
            Assert.Equal(name, resource.Name);
        }

        [Fact]
        public void ReturnNoParameterKeys()
        {
            var resource = new Configuration();
            Assert.Empty(resource.ParameterKeys);
        }

        [Fact]
        public void Execute()
        {
            var contextMock = new Mock<IResourceContext>();

            var resource = new Configuration();


            Assert.Throws<NotImplementedException>(() => resource.Execute(contextMock.Object));
        }
    }
}