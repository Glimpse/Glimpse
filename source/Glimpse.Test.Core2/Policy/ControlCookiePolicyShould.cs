using System;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Policy;
using Glimpse.Test.Core2.Tester;
using Xunit;

namespace Glimpse.Test.Core2.Policy
{
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
            Policy.RequestMetadataMock.Setup(r => r.GetCookie(ControlCookiePolicy.ControlCookieName)).Returns("invalid");

            Assert.Equal(RuntimePolicy.Off, Policy.Execute(Policy.PolicyContextMock.Object));
        }

        [Fact]
        public void SetPolicyToMatchCookieValue()
        {
            Policy.RequestMetadataMock.Setup(r => r.GetCookie(ControlCookiePolicy.ControlCookieName)).Returns("modifyresponseheaders");

            Assert.Equal(RuntimePolicy.ModifyResponseHeaders, Policy.Execute(Policy.PolicyContextMock.Object));
        }

        [Fact]
        public void ExecuteOnBeginRequest()
        {
            Assert.Equal(RuntimeEvent.BeginRequest, Policy.ExecuteOn);
        }
    }
}