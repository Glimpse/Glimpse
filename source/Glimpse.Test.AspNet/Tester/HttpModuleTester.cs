using System.Web;
using Glimpse.AspNet;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Moq;

namespace Glimpse.Test.AspNet.Tester
{
    public class HttpModuleTester : HttpModule
    {
        public Mock<HttpApplication> AppMock { get; set; }
        public Mock<HttpApplicationStateBase> AppStateMock { get; set; }
        public Mock<IGlimpseRuntime> RuntimeMock { get; set; }
        public Mock<HttpContextBase> ContextMock { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }

        private HttpModuleTester() : base()
        {
            RuntimeMock = new Mock<IGlimpseRuntime>();

            AppStateMock = new Mock<HttpApplicationStateBase>();
            AppStateMock.Setup(s => s[Constants.RuntimeKey]).Returns(RuntimeMock.Object);

            AppMock = new Mock<HttpApplication>();

            ContextMock = new Mock<HttpContextBase>();
            ContextMock.Setup(c => c.Application).Returns(AppStateMock.Object);

            LoggerMock = new Mock<ILogger>();
        }

        public static HttpModuleTester Create()
        {
            return new HttpModuleTester();
        }
    }
}