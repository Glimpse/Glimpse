using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.ResourceResult
{
    public class RedirectResourceResultShould
    {
        [Fact]
        public void ThrowWithNullParameters()
        {
            Assert.Throws<ArgumentNullException>(() => new RedirectResourceResult(null, new Dictionary<string, object>()));
            Assert.Throws<ArgumentNullException>(() => new RedirectResourceResult("a string", null));
        }

        [Fact]
        public void Return301()
        {
            var result = new RedirectResourceResult("//localhost{?a}{&b}");

            var providerMock = new Mock<IFrameworkProvider>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.FrameworkProvider).Returns(providerMock.Object);

            result.Execute(contextMock.Object);

            providerMock.Verify(p=>p.SetHttpResponseStatusCode(301));
        }

        [Fact]
        public void ResolveTemplateParameters()
        {
            var expected = "expected";
            var data = new Dictionary<string, object> { { "a", expected } };

            var result = new RedirectResourceResult("//localhost{?a}{&b}", data);

            var providerMock = new Mock<IFrameworkProvider>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.FrameworkProvider).Returns(providerMock.Object);

            result.Execute(contextMock.Object);

            providerMock.Verify(p => p.SetHttpResponseHeader("Location", It.Is<string>(uri=> uri.Contains(expected) && !uri.Contains("b"))));
        }
    }
}