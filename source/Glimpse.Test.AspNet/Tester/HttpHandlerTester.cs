using System.Collections.Specialized;
using System.Web;
using Glimpse.AspNet;
using Glimpse.Core.Framework;
using Moq;

namespace Glimpse.Test.AspNet.Tester
{
    public class HttpHandlerTester : HttpHandler
    {
        public Mock<HttpContextBase> ContextMock { get; set; }
        public Mock<HttpApplicationStateBase> ApplicationStateMock { get; set; }
        public Mock<IGlimpseRuntime> RuntimeMock { get; set; }
        public NameValueCollection QueryString { get; set; }
        public string ResourceName { get; set; }

        private HttpHandlerTester()
        {
            ResourceName = "Test";
            QueryString = new NameValueCollection {{"n", ResourceName}, {"One", "1"}};

            RuntimeMock = new Mock<IGlimpseRuntime>();

            ApplicationStateMock = new Mock<HttpApplicationStateBase>();
            ApplicationStateMock.Setup(a => a.Get(Constants.RuntimeKey)).Returns(RuntimeMock.Object);

            ContextMock = new Mock<HttpContextBase>();
            ContextMock.Setup(c => c.Application).Returns(ApplicationStateMock.Object);
            ContextMock.Setup(c => c.Request.QueryString).Returns(QueryString);
        }

        public static HttpHandlerTester Create()
        {
            return new HttpHandlerTester();
        }
    }
}