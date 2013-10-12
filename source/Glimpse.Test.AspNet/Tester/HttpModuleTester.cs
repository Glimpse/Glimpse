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
        public GlimpseRuntimeWrapper GlimpseRuntimeWrapper { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }

        private HttpModuleTester()
        {
            LoggerMock = new Mock<ILogger>();

            RuntimeMock = new Mock<IGlimpseRuntime>();
            RuntimeMock.Setup(runtime => runtime.IsInitialized).Returns(true);
            GlimpseRuntimeWrapper = new GlimpseRuntimeWrapper(AspNetFrameworkProviderTester.Create(), RuntimeMock.Object, LoggerMock.Object, false);

            AppStateMock = new Mock<HttpApplicationStateBase>();
            AppStateMock.Setup(s => s[Constants.RuntimeKey]).Returns(GlimpseRuntimeWrapper);

            AppMock = new Mock<HttpApplicationBase>();
            GlimpseRuntimeWrapper.Initialize(AppMock.Object);

            // we are calling initialize twice to check that event handlers are only attached once
            GlimpseRuntimeWrapper.Initialize(AppMock.Object);

            AppMock.Setup(a => a.Application).Returns(AppStateMock.Object);
        }

        public static HttpModuleTester Create()
        {
            return new HttpModuleTester();
        }
    }
}