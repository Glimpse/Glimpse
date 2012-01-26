using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class JsonResourceResultShould
    {
        [Fact]
        public void ConstructWithDefaultValues()
        {
            var obj = new {Any = "thing"};
            var contentType = "content/Type";

            var result = new JsonResourceResult(obj, contentType);

            Assert.Equal(obj, result.Data);
            Assert.Equal(contentType, result.ContentType);
            Assert.Equal(-1, result.CacheDuration);
            Assert.Null(result.CacheSetting);
        }

        [Fact]
        public void ConstructWithCacheSettings()
        {
            var obj = new { Any = "thing" };
            var contentType = "content/Type";
            var duration = 777;
            var cacheSettings = CacheSetting.NoStore;

            var result = new JsonResourceResult(obj, contentType, duration, cacheSettings);

            Assert.Equal(obj, result.Data);
            Assert.Equal(contentType, result.ContentType);
            Assert.Equal(duration, result.CacheDuration);
            Assert.Equal(cacheSettings, result.CacheSetting);
        }

        [Fact]
        public void ExecuteWithCacheSettings()
        {
            var frameworkProviderMock = new Mock<IFrameworkProvider>();
            var serializerMock = new Mock<ISerializer>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.FrameworkProvider).Returns(frameworkProviderMock.Object);
            contextMock.Setup(c => c.Serializer).Returns(serializerMock.Object);

            var obj = new { Any = "thing" };
            var contentType = "content/Type";
            var duration = 777;
            var cacheSettings = CacheSetting.NoStore;

            var result = new JsonResourceResult(obj, contentType, duration, cacheSettings);

            result.Execute(contextMock.Object);

            frameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader("Content-Type", contentType), Times.Once());
            frameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader("Cache-Control", It.IsAny<string>()), Times.Once());
            frameworkProviderMock.Verify(fp => fp.WriteHttpResponse(It.IsAny<string>()), Times.Once());
            serializerMock.Verify(s=>s.Serialize(obj), Times.Once());
        }

        [Fact]
        public void ExecuteWithoutCacheSettings()
        {
            var frameworkProviderMock = new Mock<IFrameworkProvider>();
            var serializerMock = new Mock<ISerializer>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.FrameworkProvider).Returns(frameworkProviderMock.Object);
            contextMock.Setup(c => c.Serializer).Returns(serializerMock.Object);

            var obj = new { Any = "thing" };
            var contentType = "content/Type";

            var result = new JsonResourceResult(obj, contentType);

            result.Execute(contextMock.Object);

            frameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader("Content-Type", contentType), Times.Once());
            frameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader("Cache-Control", It.IsAny<string>()), Times.Never());
            frameworkProviderMock.Verify(fp => fp.WriteHttpResponse(It.IsAny<string>()), Times.Once());
            serializerMock.Verify(s => s.Serialize(obj), Times.Once());
        }
    }
}