using System.Collections.Specialized;
using System.Web;
using Glimpse.AspNet;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Moq;

namespace Glimpse.Test.AspNet.Tester
{
    public class HttpHandlerTester : HttpHandler
    {
        public Mock<ILogger> LoggerMock { get; set; }
        public Mock<HttpContextBase> ContextMock { get; set; }
        public Mock<HttpApplicationStateBase> ApplicationStateMock { get; set; }
        public Mock<IGlimpseRuntime> RuntimeMock { get; set; }
        public GlimpseRuntimeWrapper GlimpseRuntimeWrapper { get; set; }
        public NameValueCollection QueryString { get; set; }
        public string ResourceName { get; set; }

        private HttpHandlerTester()
        {
            LoggerMock = new Mock<ILogger>();
            ResourceName = "Test";
            QueryString = new NameValueCollection { { "n", ResourceName }, { "One", "1" } };

            RuntimeMock = new Mock<IGlimpseRuntime>();
            GlimpseRuntimeWrapper = new GlimpseRuntimeWrapper(AspNetFrameworkProviderTester.Create(), RuntimeMock.Object, LoggerMock.Object);

            ApplicationStateMock = new Mock<HttpApplicationStateBase>();
            ApplicationStateMock.Setup(a => a.Get(Constants.RuntimeKey)).Returns(GlimpseRuntimeWrapper);

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