using System;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class ActiveGlimpseRequestContextsShould
    {
        [Fact]
        public void ManageGlimpseRequestContextWhileKeepingTrackOfItInCallContext()
        {
            var currentGlimpseRequestIdTrackerTester = new CurrentGlimpseRequestIdTrackerTester();

            var glimpseRequestContext = CreateGlimpseRequestContext();
            Guid glimpseRequestId;
            Assert.False(currentGlimpseRequestIdTrackerTester.TryGet(out glimpseRequestId));

            var activeGlimpseRequestContexts = new ActiveGlimpseRequestContexts(currentGlimpseRequestIdTrackerTester);

            IGlimpseRequestContext actualGlimpseRequestContext;
            Assert.False(activeGlimpseRequestContexts.TryGet(glimpseRequestContext.GlimpseRequestId, out actualGlimpseRequestContext));

            var glimpseRequestContextHandle = activeGlimpseRequestContexts.Add(glimpseRequestContext);
            Assert.True(currentGlimpseRequestIdTrackerTester.TryGet(out glimpseRequestId));
            Assert.Equal(glimpseRequestContext.GlimpseRequestId, glimpseRequestId);

            Assert.True(activeGlimpseRequestContexts.TryGet(glimpseRequestContext.GlimpseRequestId, out actualGlimpseRequestContext));
            Assert.Equal(glimpseRequestContext, actualGlimpseRequestContext);

            glimpseRequestContextHandle.Dispose();
            Assert.False(activeGlimpseRequestContexts.TryGet(glimpseRequestContext.GlimpseRequestId, out actualGlimpseRequestContext));
            Assert.False(currentGlimpseRequestIdTrackerTester.TryGet(out glimpseRequestId));
        }

        [Fact]
        public void ReturnInactiveGlimpseRequestContextWhenThereIsNoTrackingInformationAvailable()
        {
            var activeGlimpseRequestContexts = new ActiveGlimpseRequestContexts(new CurrentGlimpseRequestIdTrackerTester());
            Assert.True(activeGlimpseRequestContexts.Current is UnavailableGlimpseRequestContext);
        }

        [Fact]
        public void ThrowGlimpseExceptionWhenTrackingInformationIsAvailableButCorrespondingGlimpseRequestContextIsNot()
        {
            var currentGlimpseRequestIdTrackerTester = new CurrentGlimpseRequestIdTrackerTester();
            
            Guid requestId = Guid.NewGuid();
            currentGlimpseRequestIdTrackerTester.StartTracking(requestId);

            try
            {
                var activeGlimpseRequestContexts = new ActiveGlimpseRequestContexts(currentGlimpseRequestIdTrackerTester);
                var currentGlimpseRequestContext = activeGlimpseRequestContexts.Current;
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
            var activeGlimpseRequestContexts = new ActiveGlimpseRequestContexts(new CurrentGlimpseRequestIdTrackerTester());

            var glimpseRequestContext = CreateGlimpseRequestContext();
            activeGlimpseRequestContexts.Add(glimpseRequestContext);

            Assert.True(activeGlimpseRequestContexts.Current == glimpseRequestContext);
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