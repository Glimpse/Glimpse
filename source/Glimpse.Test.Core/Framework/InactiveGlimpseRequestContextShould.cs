using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class InactiveGlimpseRequestContextShould
    {
        [Fact]
        public void AlwaysReturnRuntimePolicyOff()
        {
            Assert.Equal(RuntimePolicy.Off, UnavailableGlimpseRequestContext.Instance.CurrentRuntimePolicy);
        }

        [Fact]
        public void HaveAnEmptyGlimpseRequestId()
        {
            Assert.Equal(new Guid(), UnavailableGlimpseRequestContext.Instance.GlimpseRequestId);
        }
    }
}