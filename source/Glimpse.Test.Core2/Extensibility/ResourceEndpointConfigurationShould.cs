using System;
using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.TestDoubles;
using Moq;
using Moq.Protected;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class ResourceEndpointConfigurationShould : IDisposable
    {
        private ResourceEndpointConfiguration endpointConfig;

        public ResourceEndpointConfiguration EndpointConfig
        {
            get { return endpointConfig ?? (endpointConfig = EndpointConfigMock.Object); }
            set { endpointConfig = value; }
        }

        private Mock<ResourceEndpointConfiguration> endpointConfigMock;

        public Mock<ResourceEndpointConfiguration> EndpointConfigMock
        {
            get
            {
                if (endpointConfigMock != null)
                    return endpointConfigMock;

                var configMock = new Mock<ResourceEndpointConfiguration>();
                configMock.Protected().Setup<string>("GenerateUriTemplate", "resourceName",
                                                     ItExpr.IsAny<IEnumerable<ResourceParameterMetadata>>(), ItExpr.IsAny<ILogger>())
                    .Returns("http://localhost/");

                endpointConfigMock = configMock;

                return endpointConfigMock;
            }
            set { endpointConfigMock = value; }
        }

        private Mock<IResource> resourceMock;

        public Mock<IResource> ResourceMock
        {
            get
            {
                if (resourceMock != null)
                    return resourceMock;

                var rm = new Mock<IResource>();
                rm.Setup(r => r.Parameters).Returns(new List<ResourceParameterMetadata> {new ResourceParameterMetadata("One"), new ResourceParameterMetadata("Two")});
                rm.Setup(r => r.Name).Returns("resourceName");

                resourceMock = rm;
                return resourceMock;
            }
            set { resourceMock = value; }
        }

        private Mock<ILogger> loggerMock;

        public Mock<ILogger> LoggerMock
        {
            get
            {
                if (loggerMock != null)
                    return loggerMock;

                loggerMock = new Mock<ILogger>();

                return loggerMock;
            }
            set { loggerMock = value; }
        }

        public void Dispose()
        {
            EndpointConfig = null;
            EndpointConfigMock = null;
            ResourceMock = null;
        }

        [Fact]
        public void GenerateAUri()
        {
            Assert.Equal("http://localhost/",
                         EndpointConfig.GenerateUriTemplate(ResourceMock.Object, LoggerMock.Object));
        }

        [Fact]
        public void ReturnEmptyStringWithNullChildResult()
        {
            EndpointConfigMock.Protected().Setup<string>("GenerateUriTemplate", "resourceName",
                                                         ItExpr.IsAny<IEnumerable<ResourceParameterMetadata>>(),
                                                         ItExpr.IsAny<ILogger>()).Returns<string>(null);

            Assert.Equal("",
                         EndpointConfig.GenerateUriTemplate(ResourceMock.Object, LoggerMock.Object));
        }

        [Fact]
        public void PassesPlaceholderValuesWithoutMatches()
        {
            EndpointConfig.GenerateUriTemplate(ResourceMock.Object, LoggerMock.Object);

            EndpointConfigMock.Protected().Verify<string>("GenerateUriTemplate", Times.Once(), "resourceName",
                                                          ItExpr.IsAny<IEnumerable<ResourceParameterMetadata>>(),
                                                          ItExpr.IsAny<ILogger>());
        }

        [Fact]
        public void PassesValuesWithMatches()
        {
            EndpointConfig.GenerateUriTemplate(ResourceMock.Object, LoggerMock.Object);

            EndpointConfigMock.Protected().Verify<string>("GenerateUriTemplate", Times.Once(), "resourceName", ItExpr.IsAny<IEnumerable<ResourceParameterMetadata>>(), ItExpr.IsAny<ILogger>());
        }

        [Fact]
        public void ReturnEmptyStringWithChildGenerateUriException()
        {
            EndpointConfigMock.Protected().Setup<string>("GenerateUriTemplate", ItExpr.IsAny<string>(),
                                                         ItExpr.IsAny<IEnumerable<ResourceParameterMetadata>>(),
                                                         ItExpr.IsAny<ILogger>()).Throws<DummyException>();

            var result = EndpointConfig.GenerateUriTemplate(ResourceMock.Object, LoggerMock.Object);

            EndpointConfigMock.Protected().Verify<string>("GenerateUriTemplate", Times.Once(), "resourceName", ItExpr.IsAny<IEnumerable<ResourceParameterMetadata>>(),
                                                          ItExpr.IsAny<ILogger>());
            LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>(), It.IsAny<object[]>()));
            Assert.Equal("", result);
        }

        [Fact]
        public void ThrowsExceptionWithNullResource()
        {
            Assert.Throws<ArgumentNullException>(
                () => EndpointConfig.GenerateUriTemplate(null, LoggerMock.Object));
        }

        [Fact]
        public void ThrowsExceptionWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(
                () => EndpointConfig.GenerateUriTemplate(ResourceMock.Object, null));
        }
    }
}