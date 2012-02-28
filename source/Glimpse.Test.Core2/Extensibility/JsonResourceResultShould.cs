using System;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.ResourceResult;
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

            var result = new JsonResourceResult(obj);

            Assert.Equal(obj, result.Data);
            Assert.Equal(@"application/json", result.ContentType);
            Assert.Equal(-1, result.CacheDuration);
            Assert.Null(result.CacheSetting);
        }

        [Fact]
        public void ConstructWithCacheSettings()
        {
            var obj = new { Any = "thing" };
            var duration = 777;
            var cacheSettings = CacheSetting.NoStore;

            var result = new JsonResourceResult(obj, duration, cacheSettings);

            Assert.Equal(obj, result.Data);
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
            var duration = 777;
            var cacheSettings = CacheSetting.NoStore;

            var result = new JsonResourceResult(obj, duration, cacheSettings);

            result.Execute(contextMock.Object);

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

            var result = new JsonResourceResult(obj);

            result.Execute(contextMock.Object);

            frameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader("Cache-Control", It.IsRegex(".+max-age.+")), Times.Never());
            frameworkProviderMock.Verify(fp => fp.WriteHttpResponse(It.IsAny<string>()), Times.Once());
            serializerMock.Verify(s => s.Serialize(obj), Times.Once());
        }

        [Fact]
        public void UseCallbackForJsonpWhenProvided()
        {
            var frameworkProviderMock = new Mock<IFrameworkProvider>();
            var serializerMock = new Mock<ISerializer>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.FrameworkProvider).Returns(frameworkProviderMock.Object);
            contextMock.Setup(c => c.Serializer).Returns(serializerMock.Object);

            var obj = new {Any = "Thing"};
            var callback = "aJavasciptFunction";
            var result = new JsonResourceResult(obj, callback);

            result.Execute(contextMock.Object);

            frameworkProviderMock.Verify(fp=>fp.WriteHttpResponse(It.IsRegex(callback + ".+")), Times.Once());
        }

        [Fact]
        public void UseJsonContentTypeWithoutCallback()
        {
            var result = new JsonResourceResult(new { Any = "Thing" });

            Assert.Equal(@"application/json", result.ContentType);
        }

        [Fact]
        public void UseJavascriptContentTypeWithoutCallback()
        {
            var result = new JsonResourceResult(new { Any = "Thing" }, "callback");

            Assert.Equal(@"application/x-javascript", result.ContentType);
        }

        [Fact]
        public void ThrowExceptionWithNulData()
        {
            Assert.Throws<ArgumentNullException>(()=>new JsonResourceResult(null));
        }
    }
}