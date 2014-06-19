using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class GlimpseRequestContextHandleShould
    {
        [Fact]
        public void RemoveCorrespondingGlimpseRequestContextFromActiveGlimpseRequestContextsOnExplicitDisposal()
        {
            var activeGlimpseRequestContexts = new ActiveGlimpseRequestContexts(new CurrentGlimpseRequestIdTrackerTester());

            var firstGlimpseRequestContext = CreateGlimpseRequestContext();
            var secondGlimpseRequestContext = CreateGlimpseRequestContext();
            var thirdGlimpseRequestContext = CreateGlimpseRequestContext();

            var firstGlimpseRequestContextHandle = activeGlimpseRequestContexts.Add(firstGlimpseRequestContext);
            var secondGlimpseRequestContextHandle = activeGlimpseRequestContexts.Add(secondGlimpseRequestContext);
            var thirdGlimpseRequestContextHandle = activeGlimpseRequestContexts.Add(thirdGlimpseRequestContext);

            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, firstGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, thirdGlimpseRequestContext);

            secondGlimpseRequestContextHandle.Dispose();
            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, thirdGlimpseRequestContext);

            firstGlimpseRequestContextHandle.Dispose();
            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, thirdGlimpseRequestContext);

            thirdGlimpseRequestContextHandle.Dispose();
            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, secondGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, thirdGlimpseRequestContext);
        }

        [Fact]
        public void RemoveCorrespondingGlimpseRequestContextFromActiveGlimpseRequestContextsOnFinalization()
        {
            var activeGlimpseRequestContexts = new ActiveGlimpseRequestContexts(new CurrentGlimpseRequestIdTrackerTester());

            var firstGlimpseRequestContext = CreateGlimpseRequestContext();
            var secondGlimpseRequestContext = CreateGlimpseRequestContext();
            var thirdGlimpseRequestContext = CreateGlimpseRequestContext();

            var handlesDictionary = new Dictionary<int, GlimpseRequestContextHandle>
            {
                {1, activeGlimpseRequestContexts.Add(firstGlimpseRequestContext)},
                {2, activeGlimpseRequestContexts.Add(secondGlimpseRequestContext)},
                {3, activeGlimpseRequestContexts.Add(thirdGlimpseRequestContext)}
            };

            GC.Collect();
            GC.WaitForPendingFinalizers();

            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, firstGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, thirdGlimpseRequestContext);

            handlesDictionary.Remove(2);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, thirdGlimpseRequestContext);

            handlesDictionary.Remove(1);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, thirdGlimpseRequestContext);

            handlesDictionary.Remove(3);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, secondGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(activeGlimpseRequestContexts, thirdGlimpseRequestContext);
        }

        private static GlimpseRequestContext CreateGlimpseRequestContext()
        {
            var requestUri = new Uri("http://localhost/");

            return new GlimpseRequestContext(
                Guid.NewGuid(),
                RequestResponseAdapterTester.Create(requestUri).RequestResponseAdapterMock.Object,
                RuntimePolicy.On,
                ResourceEndpointConfigurationTester.Create(requestUri, false).ResourceEndpointConfigurationMock.Object,
                "/glimpse.axd",
                new RuntimePolicyDeterminator(new Mock<IReadonlyConfiguration>().Object),
                new Mock<IGlimpseScriptTagsGenerator>().Object,
                new Mock<ILogger>().Object);
        }

        private static void AssertExistenceOfGlimpseRequestContext(ActiveGlimpseRequestContexts activeGlimpseRequestContexts, IGlimpseRequestContext expectedGlimpseRequestContext)
        {
            IGlimpseRequestContext actualGlimpseRequestContext;
            Assert.True(activeGlimpseRequestContexts.TryGet(expectedGlimpseRequestContext.GlimpseRequestId, out actualGlimpseRequestContext));
            Assert.Equal(expectedGlimpseRequestContext, actualGlimpseRequestContext);
        }

        private static void AssertNonExistenceOfGlimpseRequestContext(ActiveGlimpseRequestContexts activeGlimpseRequestContexts, IGlimpseRequestContext expectedGlimpseRequestContext)
        {
            IGlimpseRequestContext actualGlimpseRequestContext;
            Assert.False(activeGlimpseRequestContexts.TryGet(expectedGlimpseRequestContext.GlimpseRequestId, out actualGlimpseRequestContext));
        }
    }
}