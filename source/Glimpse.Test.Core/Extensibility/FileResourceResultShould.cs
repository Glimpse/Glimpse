using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class FileResourceResultShould
    {
        [Fact]
        public void ConsturctWithDefaultCache()
        {
            var array = new byte[1];
            var contentType = "content/type";

            var result = new FileResourceResult(array, contentType);

            Assert.Equal(array, result.Content);
            Assert.Equal(contentType, result.ContentType);
            Assert.Equal(-1, result.CacheDuration);
            Assert.Equal(null, result.CacheSetting);
        }

        [Fact]
        public void ThrowExceptionWithNullContent()
        {
            Assert.Throws<ArgumentNullException>(()=>new FileResourceResult(null, "contentType"));
        }

        [Fact]
        public void ThrowExceptionWithNullContentType()
        {
            Assert.Throws<ArgumentNullException>(() => new FileResourceResult(new byte[1], null));
        }

        [Fact]
        public void SetCacheSettings()
        {
            var array = new byte[1];
            var contentType = "content/type";
            var duration = 7;
            var setting = CacheSetting.ProxyRevalidate;

            var result = new FileResourceResult(array, contentType, duration, setting);

            Assert.Equal(array, result.Content);
            Assert.Equal(contentType, result.ContentType);
            Assert.Equal(duration, result.CacheDuration);
            Assert.Equal(setting, result.CacheSetting);
        }

        [Fact]
        public void ExecuteWithCacheSettings()
        {
            var frameworkProviderMock = new Mock<IFrameworkProvider>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.FrameworkProvider).Returns(frameworkProviderMock.Object);

            var array = new byte[1];
            var contentType = "content/type";
            var duration = 7;
            var setting = CacheSetting.ProxyRevalidate;

            var result = new FileResourceResult(array, contentType, duration, setting);

            result.Execute(contextMock.Object);

            frameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader("Content-Type", contentType), Times.Once());
            frameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader("Cache-Control", It.IsAny<string>()), Times.Once());
            frameworkProviderMock.Verify(fp => fp.WriteHttpResponse(array), Times.Once());
        }

        [Fact]
        public void ExecuteWithoutCacheSettings()
        {
            var frameworkProviderMock = new Mock<IFrameworkProvider>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.FrameworkProvider).Returns(frameworkProviderMock.Object);

            var array = new byte[1];
            var contentType = "content/type";

            var result = new FileResourceResult(array, contentType);

            result.Execute(contextMock.Object);

            frameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader("Content-Type", contentType), Times.Once());
            frameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader("Cache-Control", It.IsRegex(".+max-age.+")), Times.Never());
            frameworkProviderMock.Verify(fp => fp.WriteHttpResponse(array), Times.Once());
        }
    }
}