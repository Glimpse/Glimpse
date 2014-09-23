using System;
using System.Linq;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Resource;
using Glimpse.Core.ResourceResult;
using Glimpse.Test.Core.Extensions;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.Resource
{
    public class PopupResourceShould
    {
        [Fact]
        public void BeNamedProperly()
        {
            var resource = new PopupResource();

            Assert.Equal("glimpse_popup", resource.Name);
        }

        [Fact]
        public void ContainRequiredRequestIdParameter()
        {
            var resource = new PopupResource();

            Assert.Contains("requestId", resource.Parameters.Where(p => p.IsRequired).Select(p => p.Name));
        }

        [Fact]
        public void ContainOptionalHashParameter()
        {
            var resource = new PopupResource();

            Assert.Contains("hash", resource.Parameters.Select(p => p.Name));
        }

        [Fact]
        public void NotSupportNonPrivilegedExecution()
        {
            var resource = new PopupResource();
            var contextMock = new Mock<IResourceContext>();

            Assert.Throws<NotSupportedException>(() => resource.Execute(contextMock.Object));
        }

        [Fact]
        public void ThrowWithInvalidContextParameter()
        {
            var resource = new PopupResource();
            var configMock = new Mock<IReadOnlyConfiguration>();
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();

            Assert.Throws<ArgumentNullException>(() => resource.Execute(null, configMock.Object, providerMock.Object));
        }

        [Fact]
        public void ThrowWithInvalidConfigParameter()
        {
            var resource = new PopupResource();
            var contextMock = new Mock<IResourceContext>();
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();

            Assert.Throws<ArgumentNullException>(() => resource.Execute(contextMock.Object, null, providerMock.Object));
        }

        [Theory]
        [InlineData("invalid guid", true)]
        [InlineData(null, true)]
        [InlineData("no value", false)]
        public void RejectInvalidRequestIdParameters(string value, bool hasValue)
        {
            var resource = new PopupResource();
            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.Parameters.TryGetValue("requestId", out value)).Returns(hasValue);
            var configMock = new Mock<IReadOnlyConfiguration>();
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();

            var result = resource.Execute(contextMock.Object, configMock.Object, providerMock.Object);

            var statusCodeResult = result as StatusCodeResourceResult;

            Assert.NotNull(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        } 

        [Fact(Skip="Needs to be reworked due to runtime changes")]
        public void ReturnHtmlResourceResult()
        {
            //var resource = new PopupResource();
            //var contextMock = new Mock<IResourceContext>();
            //var guid = Guid.NewGuid();
            //string requestId = guid.ToString();
            //contextMock.Setup(c => c.Parameters.TryGetValue("requestId", out requestId)).Returns(true);
            //var version = "1.X.Y";
            //contextMock.Setup(c => c.Parameters.TryGetValue("version", out version)).Returns(true);

            //Func<Guid?, string> strategy = (id) => requestId + version;
            //var configMock = new Mock<IReadonlyGlimpseConfiguration>();

            ////var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            ////providerMock.Setup(f => f.HttpRequestStore.Get(Constants.ClientScriptsStrategy)).Returns(() => strategy);
            
            //var result = resource.Execute(contextMock.Object, configMock.Object, providerMock.Object);

            //var htmlResourceResult = result as HtmlResourceResult;

            //Assert.NotNull(result);
            //Assert.Contains(requestId, htmlResourceResult.Html);
            //Assert.Contains(version, htmlResourceResult.Html);
        }
    }
}