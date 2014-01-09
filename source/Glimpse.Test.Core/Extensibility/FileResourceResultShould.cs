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
        public void Consturct()
        {
            var array = new byte[1];
            var contentType = "content/type";

            var result = new FileResourceResult(array, contentType);

            Assert.Equal(array, result.Content);
            Assert.Equal(contentType, result.ContentType);
        }

        [Fact]
        public void ThrowExceptionWithNullContent()
        {
            Assert.Throws<ArgumentNullException>(() => new FileResourceResult(null, "contentType"));
        }

        [Fact]
        public void ThrowExceptionWithNullContentType()
        {
            Assert.Throws<ArgumentNullException>(() => new FileResourceResult(new byte[1], null));
        }

        [Fact]
        public void Execute()
        {
            var frameworkProviderMock = new Mock<IRequestResponseAdapter>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.RequestResponseAdapter).Returns(frameworkProviderMock.Object);

            var array = new byte[1];
            var contentType = "content/type";

            var result = new FileResourceResult(array, contentType);

            result.Execute(contextMock.Object);

            frameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader("Content-Type", contentType), Times.Once());
            frameworkProviderMock.Verify(fp => fp.WriteHttpResponse(array), Times.Once());
        }
    }
}