using System;
using System.Runtime.Remoting.Messaging;
using Glimpse.Core.Framework;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class ActiveGlimpseRequestContextsShould
    {
        [Fact]
        public void ManageGlimpseRequestContextWhileKeepingTrackOfItInCallContext()
        {
            CallContext.FreeNamedDataSlot(ActiveGlimpseRequestContexts.RequestIdKey);
            ActiveGlimpseRequestContexts.RemoveAll();

            var glimpseRequestContext = CreateGlimpseRequestContext();
            Assert.Equal(null, CallContext.GetData(ActiveGlimpseRequestContexts.RequestIdKey));

            IGlimpseRequestContext actualGlimpseRequestContext;
            Assert.False(ActiveGlimpseRequestContexts.TryGet(glimpseRequestContext.GlimpseRequestId, out actualGlimpseRequestContext));

            ActiveGlimpseRequestContexts.Add(glimpseRequestContext);
            Assert.Equal(glimpseRequestContext.GlimpseRequestId, CallContext.GetData(ActiveGlimpseRequestContexts.RequestIdKey));
            Assert.True(ActiveGlimpseRequestContexts.TryGet(glimpseRequestContext.GlimpseRequestId, out actualGlimpseRequestContext));
            Assert.Equal(glimpseRequestContext, actualGlimpseRequestContext);

            ActiveGlimpseRequestContexts.Remove(glimpseRequestContext.GlimpseRequestId);
            Assert.False(ActiveGlimpseRequestContexts.TryGet(glimpseRequestContext.GlimpseRequestId, out actualGlimpseRequestContext));
            Assert.Equal(null, CallContext.GetData(ActiveGlimpseRequestContexts.RequestIdKey));
        }

        [Fact]
        public void ReturnInactiveGlimpseRequestContextWhenThereIsNoTrackingInformationAvailable()
        {
            CallContext.FreeNamedDataSlot(ActiveGlimpseRequestContexts.RequestIdKey);
            ActiveGlimpseRequestContexts.RemoveAll();
            Assert.True(ActiveGlimpseRequestContexts.Current is UnavailableGlimpseRequestContext);
        }

        [Fact]
        public void ThrowGlimpseExceptionWhenTrackingInformationIsAvailableButCorrespondingGlimpseRequestContextIsNot()
        {
            CallContext.FreeNamedDataSlot(ActiveGlimpseRequestContexts.RequestIdKey);
            ActiveGlimpseRequestContexts.RemoveAll();

            Guid requestId = Guid.NewGuid();
            CallContext.LogicalSetData(ActiveGlimpseRequestContexts.RequestIdKey, requestId);

            try
            {
                var currentGlimpseRequestContext = ActiveGlimpseRequestContexts.Current;
                Assert.True(false, "A GlimpseException was expected");
            }
            catch (GlimpseException glimpseException)
            {
                Assert.Equal("No corresponding Glimpse request context found for GlimpseRequestId '" + requestId + "'.", glimpseException.Message);
            }
        }

        [Fact]
        public void ReturnCorrespondingGlimpseRequestContextWhenThereIsTrackingInformationAvailable()
        {
            CallContext.FreeNamedDataSlot(ActiveGlimpseRequestContexts.RequestIdKey);
            ActiveGlimpseRequestContexts.RemoveAll();

            var glimpseRequestContext = CreateGlimpseRequestContext();
            ActiveGlimpseRequestContexts.Add(glimpseRequestContext);

            Assert.True(ActiveGlimpseRequestContexts.Current == glimpseRequestContext);
        }

        private static IGlimpseRequestContext CreateGlimpseRequestContext()
        {
            var glimpseRequestId = Guid.NewGuid();
            var glimpseRequestContextMock = new Mock<IGlimpseRequestContext>();
            glimpseRequestContextMock.Setup(context => context.GlimpseRequestId).Returns(glimpseRequestId);
            return glimpseRequestContextMock.Object;
        }
    }
}