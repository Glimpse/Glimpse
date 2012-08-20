using Glimpse.Core.Extensibility;
using Glimpse.Core.Policy;
using Glimpse.Core;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Policy
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