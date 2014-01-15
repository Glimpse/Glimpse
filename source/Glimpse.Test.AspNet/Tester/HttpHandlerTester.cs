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
        public Mock<IGlimpseConfiguration> ConfigurationMock { get; set; }
        public Mock<IRequestResponseAdapter> RequestResponseAdapterMock { get; set; }
        public NameValueCollection QueryString { get; set; }
        public string ResourceName { get; set; }

        private HttpHandlerTester()
        {
            ResourceName = "Test";
            QueryString = new NameValueCollection {{"n", ResourceName}, {"One", "1"}};

            RequestResponseAdapterMock = new Mock<IRequestResponseAdapter>();
            ConfigurationMock = new Mock<IGlimpseConfiguration>();

            RuntimeMock = new Mock<IGlimpseRuntime>();
            RuntimeMock.Setup(r => r.Configuration).Returns(ConfigurationMock.Object);

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