using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Policy;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Policy
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