using Glimpse.Core.Extensibility;
using Glimpse.Core.Policy;
using Glimpse.Core;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Policy
{
    public class GlimpseResourcePolicyShould
    {
        [Fact]
        public void SetRuntimePolicyToIgnore()
        {
            var policy = new GlimpseResourcePolicy();
            var policyContextMock = new Mock<IRuntimePolicyContext>();

            Assert.Equal(RuntimePolicy.ExecuteResourceOnly, policy.Execute(policyContextMock.Object));
        }

        [Fact]
        public void ExecuteOnExecuteResource()
        {
            var policy = new GlimpseResourcePolicy();
            Assert.Equal(RuntimeEvent.ExecuteResource, policy.ExecuteOn);
        }
    }
}