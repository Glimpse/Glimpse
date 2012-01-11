using System;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.Policy;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Policy
{
    public class ControlCookiePolicyTester:ControlCookiePolicy
    {
        public Mock<IRequestMetadata> RequestMetadataMock { get; set; }
        public Mock<IRuntimePolicyContext> PolicyContextMock { get; set; }

        private ControlCookiePolicyTester()
        {
            RequestMetadataMock = new Mock<IRequestMetadata>();
            RequestMetadataMock.Setup(r => r.GetCookie(Constants.ControlCookieName)).Returns<string>(null);

            PolicyContextMock = new Mock<IRuntimePolicyContext>();
            PolicyContextMock.Setup(c => c.RequestMetadata).Returns(RequestMetadataMock.Object);
        }

        public static ControlCookiePolicyTester Create()
        {
            return new ControlCookiePolicyTester();
        }
    }

    public class ControlCookiePolicyShould:IDisposable
    {
        private ControlCookiePolicyTester tester;
        public ControlCookiePolicyTester Policy
        {
            get { return tester ?? (tester = ControlCookiePolicyTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Policy = null;
        }

        [Fact]
        public void SetPolicyToOffWithoutCookie()
        {
            Assert.Equal(RuntimePolicy.Off, Policy.Execute(Policy.PolicyContextMock.Object));
        }

        [Fact]
        public void SetPolicyToOffWithInvalidCookieValue()
        {
            Policy.RequestMetadataMock.Setup(r => r.GetCookie(Constants.ControlCookieName)).Returns("invalid");

            Assert.Equal(RuntimePolicy.Off, Policy.Execute(Policy.PolicyContextMock.Object));
        }

        [Fact]
        public void SetPolicyToMatchCookieValue()
        {
            Policy.RequestMetadataMock.Setup(r => r.GetCookie(Constants.ControlCookieName)).Returns("modifyresponseheaders");

            Assert.Equal(RuntimePolicy.ModifyResponseHeaders, Policy.Execute(Policy.PolicyContextMock.Object));
        }
    }
}