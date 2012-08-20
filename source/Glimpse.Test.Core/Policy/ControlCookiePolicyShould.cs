using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Policy;
using Glimpse.Core;
using Glimpse.Test.Core.Tester;
using Xunit;

namespace Glimpse.Test.Core.Policy
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