using System;
using System.Collections.Generic;
using Glimpse.Core2;
using Glimpse.Core2.Framework;
using Glimpse.Core2.SerializationConverter;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.SerializationConverter
{
    public class GlimpseMetadataConverterShould
    {
        [Fact]
        public void ConvertAGlimpseMetadataObject()
        {
            var requestMock = new Mock<IRequestMetadata>();
            requestMock.Setup(r => r.GetCookie(Constants.ClientIdCookieName)).Returns("Anything");
            requestMock.Setup(r => r.RequestHttpMethod).Returns("POST");
            requestMock.Setup(r => r.RequestUri).Returns("http://localhost/");
            requestMock.Setup(r => r.ResponseContentType).Returns(@"text/html");
            requestMock.Setup(r => r.GetHttpHeader(Constants.UserAgentHeaderName)).Returns(@"FireFox");

            var metadata = new GlimpseMetadata(Guid.NewGuid(), requestMock.Object, new Dictionary<string, string>(), 55);
            var converter = new GlimpseMetadataConverter();

            var result = converter.Convert(metadata);

            Assert.True(result.ContainsKey("clientId"));
            Assert.NotNull(result["clientId"]);
            Assert.True(result.ContainsKey("dateTime"));
            Assert.NotNull(result["dateTime"]);
            Assert.True(result.ContainsKey("duration"));
            Assert.NotNull(result["duration"]);
            Assert.True(result.ContainsKey("parentRequestId"));
            Assert.NotNull(result["parentRequestId"]);
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
            Assert.True(result.ContainsKey("plugins"));
            Assert.NotNull(result["plugins"]);
            Assert.True(result.ContainsKey("userAgent"));
            Assert.NotNull(result["userAgent"]);
        }

        [Fact]
        public void ThrowExceptionWithInvalidInput()
        {
            var converter = new GlimpseMetadataConverter();

            Assert.Throws<InvalidCastException>(() => converter.Convert("bad input"));
        }
    }
}