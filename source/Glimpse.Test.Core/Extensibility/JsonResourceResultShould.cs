using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class JsonResourceResultShould
    {
        [Fact]
        public void ConstructWithDefaultValues()
        {
            var obj = new { Any = "thing" };

            var result = new JsonResourceResult(obj);

            Assert.Equal(obj, result.Data);
            Assert.Equal(@"application/json", result.ContentType);
        }

        [Fact]
        public void Execute()
        {
            var frameworkProviderMock = new Mock<IRequestResponseAdapter>();
            var serializerMock = new Mock<ISerializer>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.RequestResponseAdapter).Returns(frameworkProviderMock.Object);
            contextMock.Setup(c => c.Serializer).Returns(serializerMock.Object);

            var obj = new { Any = "thing" };

            var result = new JsonResourceResult(obj);

            result.Execute(contextMock.Object);

            frameworkProviderMock.Verify(fp => fp.WriteHttpResponse(It.IsAny<string>()), Times.Once());
            serializerMock.Verify(s => s.Serialize(obj), Times.Once());
        }

        [Fact]
        public void UseCallbackForJsonpWhenProvided()
        {
            var frameworkProviderMock = new Mock<IRequestResponseAdapter>();
            var serializerMock = new Mock<ISerializer>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.RequestResponseAdapter).Returns(frameworkProviderMock.Object);
            contextMock.Setup(c => c.Serializer).Returns(serializerMock.Object);

            var obj = new { Any = "Thing" };
            var callback = "aJavasciptFunction";
            var result = new JsonResourceResult(obj, callback);

            result.Execute(contextMock.Object);

            frameworkProviderMock.Verify(fp => fp.WriteHttpResponse(It.IsRegex(callback + ".+")), Times.Once());
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
            Assert.Throws<ArgumentNullException>(() => new JsonResourceResult(null));
        }
    }
}