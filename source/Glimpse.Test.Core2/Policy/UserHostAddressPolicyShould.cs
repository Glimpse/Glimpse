using System;
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

            Assert.Throws<NotSupportedException>(() => policy.Execute(contextMock.Object));
        }
    }
}