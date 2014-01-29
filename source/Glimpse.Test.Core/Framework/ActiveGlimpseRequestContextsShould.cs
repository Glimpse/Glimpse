using System;
using System.Runtime.Remoting.Messaging;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.Tester;
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

            GlimpseRequestContext actualGlimpseRequestContext;
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
            Assert.True(ActiveGlimpseRequestContexts.Current is InactiveGlimpseRequestContext);
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
                Assert.Equal("No corresponding GlimpseRequestContext found for GlimpseRequestId '" + requestId + "'.", glimpseException.Message);
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

        private static GlimpseRequestContext CreateGlimpseRequestContext()
        {
            return new GlimpseRequestContext(
                Guid.NewGuid(),
                RequestResponseAdapterTester.Create("/").RequestResponseAdapterMock.Object,
                RuntimePolicy.On,
                "/Glimpse.axd");
        }
    }
}