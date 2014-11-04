using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class UnavailableGlimpseRequestContextHandleShould
    {
        [Fact]
        public void AlwaysReturnRequestHandlingModeUnhandled()
        {
            Assert.Equal(RequestHandlingMode.Unhandled, UnavailableGlimpseRequestContextHandle.Instance.RequestHandlingMode);
        }

        [Fact]
        public void HaveAnEmptyGlimpseRequestId()
        {
            Assert.Equal(new Guid(), UnavailableGlimpseRequestContextHandle.Instance.GlimpseRequestId);
        }
    }
}