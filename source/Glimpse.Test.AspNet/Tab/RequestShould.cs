using System.Web;
using Glimpse.AspNet.Tab;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet.Tab
{
    public class RequestShould
    {
        [Fact]
        public void HaveProperContextObjectType()
        {
            var request = new Request();

            Assert.Equal(typeof(HttpContextBase), request.RequestContextType);
        }

        [Fact]
        public void UseDefaultLifeCycleSupport()
        {
            var request = new Request();
            Assert.Equal(RuntimeEvent.EndRequest, request.ExecuteOn);
        }

        [Fact]
        public void BeNamedRequest()
        {
            var request = new Request();
            Assert.Equal("Request", request.Name);
        }

        [Fact]
        public void HaveADocumentationUri()
        {
            var request = new Request();

            Assert.False(string.IsNullOrWhiteSpace(request.DocumentationUri));
        }

        [Fact]
        public void ReturnData()
        {
            var serverMock = new Mock<HttpServerUtilityBase>();
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(r => r.Cookies).Returns(new HttpCookieCollection());
            var httpBaseMock = new Mock<HttpContextBase>();
            httpBaseMock.Setup(c => c.Request).Returns(requestMock.Object);
            httpBaseMock.Setup(c => c.Server).Returns(serverMock.Object);
            var contextMock = new Mock<ITabContext>();
            contextMock.Setup(c => c.GetRequestContext<HttpContextBase>()).Returns(httpBaseMock.Object);

            var request = new Request();
            var result = request.GetData(contextMock.Object);

            Assert.NotNull(result);
            Assert.NotNull(result as RequestModel);
        }
    }
}