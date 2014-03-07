using System;
using System.Collections.Generic;
using Glimpse.Core;
using Glimpse.Core.Framework;
using Glimpse.Core.SerializationConverter;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.SerializationConverter
{
    public class GlimpseRequestConverterShould
    {
        [Fact]
        public void ConvertAGlimpseMetadataObject()
        {
            var requestMock = new Mock<IRequestMetadata>();
            requestMock.Setup(r => r.GetCookie(Constants.ClientIdCookieName)).Returns("Anything");
            requestMock.Setup(r => r.RequestHttpMethod).Returns("POST");
            requestMock.Setup(r => r.RequestUri).Returns(new Uri("http://localhost/"));
            requestMock.Setup(r => r.ResponseContentType).Returns(@"text/html");
            requestMock.Setup(r => r.GetHttpHeader(Constants.UserAgentHeaderName)).Returns(@"FireFox");

            var metadata = new GlimpseRequest(Guid.NewGuid(), requestMock.Object, new Dictionary<string, TabResult>(), new Dictionary<string, TabResult>(), TimeSpan.FromMilliseconds(55), new Dictionary<string, object>());
            var converter = new GlimpseRequestConverter();

            var obj = converter.Convert(metadata);

            var result = obj as IDictionary<string, object>;

            Assert.NotNull(result);
            Assert.True(result.ContainsKey("clientId"));
            Assert.NotNull(result["clientId"]);
            Assert.True(result.ContainsKey("dateTime"));
            Assert.NotNull(result["dateTime"]);
            Assert.True(result.ContainsKey("duration"));
            Assert.NotNull(result["duration"]);
            Assert.True(result.ContainsKey("parentRequestId"));
            Assert.Null(result["parentRequestId"]);
            Assert.True(result.ContainsKey("requestId"));
            Assert.NotNull(result["requestId"]);
            Assert.True(result.ContainsKey("isAjax"));
            Assert.NotNull(result["isAjax"]);
            Assert.True(result.ContainsKey("method"));
            Assert.NotNull(result["method"]);
            Assert.True(result.ContainsKey("uri"));
            Assert.NotNull(result["uri"]);
            Assert.True(result.ContainsKey("contentType"));
            Assert.NotNull(result["contentType"]);
            Assert.True(result.ContainsKey("statusCode"));
            Assert.NotNull(result["statusCode"]);
            Assert.True(result.ContainsKey("userAgent"));
            Assert.NotNull(result["userAgent"]);
        }

        [Fact]
        public void ThrowExceptionWithInvalidInput()
        {
            var converter = new GlimpseRequestConverter();

            Assert.Throws<InvalidCastException>(() => converter.Convert("bad input"));
        }
    }
}