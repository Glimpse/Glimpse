using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.Tester;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class GlimpseRequestContextHandleShould
    {
        [Fact]
        public void RemoveCorrespondingGlimpseRequestContextFromActiveGlimpseRequestContextsOnExplicitDisposal()
        {
            ActiveGlimpseRequestContexts.RemoveAll();

            var firstGlimpseRequestContext = CreateGlimpseRequestContext();
            var secondGlimpseRequestContext = CreateGlimpseRequestContext();
            var thirdGlimpseRequestContext = CreateGlimpseRequestContext();

            var firstGlimpseRequestContextHandle = ActiveGlimpseRequestContexts.Add(firstGlimpseRequestContext);
            var secondGlimpseRequestContextHandle = ActiveGlimpseRequestContexts.Add(secondGlimpseRequestContext);
            var thirdGlimpseRequestContextHandle = ActiveGlimpseRequestContexts.Add(thirdGlimpseRequestContext);

            AssertExistenceOfGlimpseRequestContext(firstGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(thirdGlimpseRequestContext);

            secondGlimpseRequestContextHandle.Dispose();
            AssertExistenceOfGlimpseRequestContext(firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(thirdGlimpseRequestContext);

            firstGlimpseRequestContextHandle.Dispose();
            AssertNonExistenceOfGlimpseRequestContext(firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(thirdGlimpseRequestContext);

            thirdGlimpseRequestContextHandle.Dispose();
            AssertNonExistenceOfGlimpseRequestContext(firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(secondGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(thirdGlimpseRequestContext);
        }

        [Fact]
        public void RemoveCorrespondingGlimpseRequestContextFromActiveGlimpseRequestContextsOnFinalization()
        {
            ActiveGlimpseRequestContexts.RemoveAll();

            var firstGlimpseRequestContext = CreateGlimpseRequestContext();
            var secondGlimpseRequestContext = CreateGlimpseRequestContext();
            var thirdGlimpseRequestContext = CreateGlimpseRequestContext();

            var handlesDictionary = new Dictionary<int, GlimpseRequestContextHandle>
            {
                {1, ActiveGlimpseRequestContexts.Add(firstGlimpseRequestContext)},
                {2, ActiveGlimpseRequestContexts.Add(secondGlimpseRequestContext)},
                {3, ActiveGlimpseRequestContexts.Add(thirdGlimpseRequestContext)}
            };

            GC.Collect();
            GC.WaitForPendingFinalizers();

            AssertExistenceOfGlimpseRequestContext(firstGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(thirdGlimpseRequestContext);

            handlesDictionary.Remove(2);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            AssertExistenceOfGlimpseRequestContext(firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(thirdGlimpseRequestContext);

            handlesDictionary.Remove(1);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            AssertNonExistenceOfGlimpseRequestContext(firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(secondGlimpseRequestContext);
            AssertExistenceOfGlimpseRequestContext(thirdGlimpseRequestContext);

            handlesDictionary.Remove(3);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            AssertNonExistenceOfGlimpseRequestContext(firstGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(secondGlimpseRequestContext);
            AssertNonExistenceOfGlimpseRequestContext(thirdGlimpseRequestContext);
        }

        private static GlimpseRequestContext CreateGlimpseRequestContext()
        {
            var requestUri = new Uri("http://localhost/");

            return new GlimpseRequestContext(
                Guid.NewGuid(),
                RequestResponseAdapterTester.Create(requestUri).RequestResponseAdapterMock.Object,
                RuntimePolicy.On,
                ResourceEndpointConfigurationTester.Create(requestUri, false).ResourceEndpointConfigurationMock.Object,
                "/glimpse.axd");
        }

        private static void AssertExistenceOfGlimpseRequestContext(GlimpseRequestContext expectedGlimpseRequestContext)
        {
            IGlimpseRequestContext actualGlimpseRequestContext;
            Assert.True(ActiveGlimpseRequestContexts.TryGet(expectedGlimpseRequestContext.GlimpseRequestId, out actualGlimpseRequestContext));
            Assert.Equal(expectedGlimpseRequestContext, actualGlimpseRequestContext);
        }

        private static void AssertNonExistenceOfGlimpseRequestContext(GlimpseRequestContext expectedGlimpseRequestContext)
        {
            IGlimpseRequestContext actualGlimpseRequestContext;
            Assert.False(ActiveGlimpseRequestContexts.TryGet(expectedGlimpseRequestContext.GlimpseRequestId, out actualGlimpseRequestContext));
        }
    }
}