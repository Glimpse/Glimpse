using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.Tester;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class GlimpseRequestContextShould
    {
        [Fact]
        public void ReturnTheActiveRuntimePolicy()
        {
            const RuntimePolicy expectedRuntimePolicy = RuntimePolicy.DisplayGlimpseClient;
            var requestResponseAdapter = RequestResponseAdapterTester.Create(new Uri("http://localhost/")).RequestResponseAdapterMock.Object;

            var glimpseRequestContext = new GlimpseRequestContext(Guid.NewGuid(), requestResponseAdapter, expectedRuntimePolicy, "/glimpse.axd");
            Assert.Equal(expectedRuntimePolicy, glimpseRequestContext.CurrentRuntimePolicy);
        }

        [Fact]
        public void SetTheRequestHandlingMode()
        {
            var glimpseRequestContext = new GlimpseRequestContext(
                    Guid.NewGuid(),
                    RequestResponseAdapterTester.Create(new Uri("http://localhost/test")).RequestResponseAdapterMock.Object,
                    RuntimePolicy.On,
                    "/glimpse.axd");

            Assert.Equal(RequestHandlingMode.RegularRequest, glimpseRequestContext.RequestHandlingMode);

            glimpseRequestContext = new GlimpseRequestContext(
                    Guid.NewGuid(),
                    RequestResponseAdapterTester.Create(new Uri("http://localhost/glimpse.axd?n=something")).RequestResponseAdapterMock.Object,
                    RuntimePolicy.On,
                    "/glimpse.axd");

            Assert.Equal(RequestHandlingMode.ResourceRequest, glimpseRequestContext.RequestHandlingMode);
        }
    }
}