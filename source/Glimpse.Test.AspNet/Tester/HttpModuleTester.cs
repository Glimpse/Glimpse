using System.Web;
using Glimpse.AspNet;
using Glimpse.Core.Framework;
using Moq;

namespace Glimpse.Test.AspNet.Tester
{
    public class HttpModuleTester : HttpModule
    {
        public Mock<HttpApplicationStateBase> AppStateMock { get; set; }
        public Mock<IGlimpseRuntime> RuntimeMock { get; set; }
        public Mock<HttpContextBase> ContextMock { get; set; }

        private HttpModuleTester() : base()
        {
            RuntimeMock = new Mock<IGlimpseRuntime>();

            AppStateMock = new Mock<HttpApplicationStateBase>();
            AppStateMock.Setup(s => s[Constants.RuntimeKey]).Returns(RuntimeMock.Object);

            ContextMock = new Mock<HttpContextBase>();
            ContextMock.Setup(c => c.Application).Returns(AppStateMock.Object);
        }

        public static HttpModuleTester Create()
        {
            return new HttpModuleTester();
        }
    }
}