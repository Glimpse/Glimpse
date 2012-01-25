using System;
using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
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
                configMock.Protected().Setup<string>("GenerateUri", "resourceName",
                                                     ItExpr.IsAny<IEnumerable<KeyValuePair<string, string>>>(), ItExpr.IsAny<ILogger>())
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
                rm.Setup(r => r.ParameterKeys).Returns(new List<string> {"One", "Two"});
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
                         EndpointConfig.GenerateUri(ResourceMock.Object, LoggerMock.Object,
                                                    new Dictionary<string, string>()));
        }

        [Fact]
        public void ReturnEmptyStringWithNullChildResult()
        {
            EndpointConfigMock.Protected().Setup<string>("GenerateUri", "resourceName",
                                                         ItExpr.IsAny<IEnumerable<KeyValuePair<string, string>>>(),
                                                         ItExpr.IsAny<ILogger>()).Returns<string>(null);

            Assert.Equal("",
                         EndpointConfig.GenerateUri(ResourceMock.Object, LoggerMock.Object,
                                                    new Dictionary<string, string>()));
        }

        [Fact]
        public void PassesPlaceholderValuesWithoutMatches()
        {
            EndpointConfig.GenerateUri(ResourceMock.Object, LoggerMock.Object, new Dictionary<string, string>());

            EndpointConfigMock.Protected().Verify<string>("GenerateUri", Times.Once(), "resourceName",
                                                          new Dictionary<string, string>
                                                              {{"One", "{One}"}, {"Two", "{Two}"}},
                                                          ItExpr.IsAny<ILogger>());
        }

        [Fact]
        public void PassesValuesWithMatches()
        {
            EndpointConfig.GenerateUri(ResourceMock.Object, LoggerMock.Object,
                                       new Dictionary<string, string> {{"One", "1"}});

            EndpointConfigMock.Protected().Verify<string>("GenerateUri", Times.Once(), "resourceName",
                                                          new Dictionary<string, string>
                                                              {{"One", "1"}, {"Two", "{Two}"}},
                                                          ItExpr.IsAny<ILogger>());
        }

        [Fact]
        public void PassedEmptyCollectionWithParameterKeyException()
        {
            ResourceMock.Setup(r => r.ParameterKeys).Throws<DummyException>();

            EndpointConfig.GenerateUri(ResourceMock.Object, LoggerMock.Object,
                                       new Dictionary<string, string> {{"One", "1"}});
            EndpointConfigMock.Protected().Verify<string>("GenerateUri", Times.Once(), "resourceName",
                                                          new Dictionary<string, string>(),
                                                          ItExpr.IsAny<ILogger>());
            LoggerMock.Verify(l => l.Warn(It.IsAny<string>(), It.IsAny<DummyException>()));
        }

        [Fact]
        public void ReturnEmptyStringWithChildGenerateUriException()
        {
            EndpointConfigMock.Protected().Setup<string>("GenerateUri", ItExpr.IsAny<string>(),
                                                         ItExpr.IsAny<IEnumerable<KeyValuePair<string, string>>>(),
                                                         ItExpr.IsAny<ILogger>()).Throws<DummyException>();

            var result = EndpointConfig.GenerateUri(ResourceMock.Object, LoggerMock.Object,
                                                    new Dictionary<string, string> {{"One", "1"}});

            EndpointConfigMock.Protected().Verify<string>("GenerateUri", Times.Once(), "resourceName",
                                                          ItExpr.IsAny<IEnumerable<KeyValuePair<string, string>>>(),
                                                          ItExpr.IsAny<ILogger>());
            LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>()));
            Assert.Equal("", result);
        }

        [Fact]
        public void ThrowsExceptionWithNullResource()
        {
            Assert.Throws<ArgumentNullException>(
                () => EndpointConfig.GenerateUri(null, LoggerMock.Object, new Dictionary<string, string>()));
        }

        [Fact]
        public void ThrowsExceptionWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(
                () => EndpointConfig.GenerateUri(ResourceMock.Object, null, new Dictionary<string, string>()));
        }

        [Fact]
        public void ThrowsExceptionWithNullRequestTokenValues()
        {
            Assert.Throws<ArgumentNullException>(
                () => EndpointConfig.GenerateUri(ResourceMock.Object, LoggerMock.Object, null));
        }
    }
}