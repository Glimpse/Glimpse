using System.Web;
using Glimpse.AspNet;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Moq;

namespace Glimpse.Test.AspNet.Tester
{
    public class HttpModuleTester : HttpModule
    {
        public Mock<HttpApplicationBase> AppMock { get; set; }
        public Mock<HttpApplicationStateBase> AppStateMock { get; set; }
        public Mock<IGlimpseRuntime> RuntimeMock { get; set; }
        public Mock<IGlimpseConfiguration> ConfigurationMock { get; set; }
        public Mock<HttpContextBase> ContextMock { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }

        private HttpModuleTester() : base()
        {
            ConfigurationMock = new Mock<IGlimpseConfiguration>();

            RuntimeMock = new Mock<IGlimpseRuntime>();
            RuntimeMock.Setup(r => r.Configuration).Returns(ConfigurationMock.Object);

            AppStateMock = new Mock<HttpApplicationStateBase>();
            AppStateMock.Setup(s => s[Constants.RuntimeKey]).Returns(RuntimeMock.Object);

            AppMock = new Mock<HttpApplicationBase>();
            AppMock.Setup(a => a.Application).Returns(AppStateMock.Object);

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