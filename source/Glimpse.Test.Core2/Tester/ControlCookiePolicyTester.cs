using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.Policy;
using Moq;

namespace Glimpse.Test.Core2.Tester
{
    public class ControlCookiePolicyTester:ControlCookiePolicy
    {
        public Mock<IRequestMetadata> RequestMetadataMock { get; set; }
        public Mock<IRuntimePolicyContext> PolicyContextMock { get; set; }

        private ControlCookiePolicyTester()
        {
            RequestMetadataMock = new Mock<IRequestMetadata>();
            RequestMetadataMock.Setup(r => r.GetCookie(ControlCookieName)).Returns<string>(null);

            PolicyContextMock = new Mock<IRuntimePolicyContext>();
            PolicyContextMock.Setup(c => c.RequestMetadata).Returns(RequestMetadataMock.Object);
        }

        public static ControlCookiePolicyTester Create()
        {
            return new ControlCookiePolicyTester();
        }
    }
}