using System;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Policy;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Policy
{
    public class UserHostAddressPolicyShould
    {
        [Fact]
        public void ThrowExceptionUntilFinalImplementationDecisionIsMade()
        {
            var contextMock = new Mock<IRuntimePolicyContext>();

            var policy = new UserHostAddressPolicy();

            Assert.Equal(RuntimePolicy.On, policy.Execute(contextMock.Object));
        }

        [Fact]
        public void ExecuteOnBeginRequest()
        {
            var policy = new UserHostAddressPolicy();

            Assert.Equal(RuntimeEvent.BeginRequest, policy.ExecuteOn);
        }
    }
}