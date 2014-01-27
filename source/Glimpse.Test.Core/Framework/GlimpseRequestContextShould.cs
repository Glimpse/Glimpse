using System;
using System.Collections.Generic;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.Tester;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class GlimpseRequestContextShould
    {
        [Fact]
        public void OnlyAcceptRequestResponseAdapterWithStoredRuntimePolicy()
        {
            var requestResponseAdapterWithStoredRuntimePolicy =
                RequestResponseAdapterTester.Create(RuntimePolicy.On, "/").RequestResponseAdapterMock.Object;

            new GlimpseRequestContext(Guid.NewGuid(), requestResponseAdapterWithStoredRuntimePolicy, "/glimpse.axd");

            var requestResponseAdapterWithoutStoredRuntimePolicy = RequestResponseAdapterTester.Create(RuntimePolicy.On, "/").RequestResponseAdapterMock;
            requestResponseAdapterWithoutStoredRuntimePolicy
                .Setup(requestResponseAdapter => requestResponseAdapter.HttpRequestStore)
                .Returns(new DictionaryDataStoreAdapter(new Dictionary<string, object>()));

            try
            {
                new GlimpseRequestContext(Guid.NewGuid(), requestResponseAdapterWithoutStoredRuntimePolicy.Object, "/glimpse.axd");
                Assert.True(false, "GlimpseRequestContext should not accept a requestResponseAdapter without a stored RuntimePolicy");
            }
            catch (ArgumentException argumentException)
            {
                const string expectedExceptionMessage = "The requestResponseAdapter.HttpRequestStore should contain a value for the key '" + Constants.RuntimePolicyKey + "'.";
                Assert.Equal(expectedExceptionMessage, argumentException.Message);
            }
        }

        [Fact]
        public void ReturnTheActiveRuntimePolicy()
        {
            const RuntimePolicy expectedRuntimePolicy = RuntimePolicy.DisplayGlimpseClient;
            var requestResponseAdapter = RequestResponseAdapterTester.Create(expectedRuntimePolicy, "/").RequestResponseAdapterMock.Object;

            var glimpseRequestContext = new GlimpseRequestContext(Guid.NewGuid(), requestResponseAdapter, "/glimpse.axd");
            Assert.Equal(expectedRuntimePolicy, glimpseRequestContext.ActiveRuntimePolicy);
        }

        [Fact]
        public void SetTheRequestHandlingMode()
        {
            var glimpseRequestContext = new GlimpseRequestContext(
                    Guid.NewGuid(),
                    RequestResponseAdapterTester.Create(RuntimePolicy.On, "/test").RequestResponseAdapterMock.Object,
                    "/glimpse.axd");

            Assert.Equal(RequestHandlingMode.RegularRequest, glimpseRequestContext.RequestHandlingMode);

            glimpseRequestContext = new GlimpseRequestContext(
                    Guid.NewGuid(),
                    RequestResponseAdapterTester.Create(RuntimePolicy.On, "/glimpse.axd?n=something").RequestResponseAdapterMock.Object,
                    "/glimpse.axd");

            Assert.Equal(RequestHandlingMode.ResourceRequest, glimpseRequestContext.RequestHandlingMode);
        }
    }
}