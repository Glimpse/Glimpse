using System;
using System.Collections.Generic;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class GlimpseRequestContextShould
    {
        [Fact]
        public void OnlyAcceptRequestResponseAdapterWithStoredRuntimePolicy()
        {
            var requestStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>
            {
                { Constants.RuntimePolicyKey, RuntimePolicy.On }
            });
            
            Guid requestId = Guid.NewGuid();

            var requestResponseAdapterWithStoredRuntimePolicy = new Mock<IRequestResponseAdapter>();
            requestResponseAdapterWithStoredRuntimePolicy
                .Setup( adapter => adapter.HttpRequestStore).Returns(requestStore);
            
            var requestResponseAdapterWithoutStoredRuntimePolicy = new Mock<IRequestResponseAdapter>();
            requestResponseAdapterWithoutStoredRuntimePolicy
                .Setup( adapter => adapter.HttpRequestStore)
                .Returns(new DictionaryDataStoreAdapter(new Dictionary<string,object>()));

            new GlimpseRequestContext(requestId, requestResponseAdapterWithStoredRuntimePolicy.Object);

            try
            {
                new GlimpseRequestContext(requestId,requestResponseAdapterWithoutStoredRuntimePolicy.Object);
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
            var requestStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>
            {
                { Constants.RuntimePolicyKey, expectedRuntimePolicy }
            });

            Guid requestId = Guid.NewGuid();

            var requestResponseAdapterWithStoredRuntimePolicy = new Mock<IRequestResponseAdapter>();
            requestResponseAdapterWithStoredRuntimePolicy
                .Setup(adapter => adapter.HttpRequestStore).Returns(requestStore);

            var glimpseRequestContext = new GlimpseRequestContext(requestId, requestResponseAdapterWithStoredRuntimePolicy.Object);
            Assert.Equal(expectedRuntimePolicy, glimpseRequestContext.ActiveRuntimePolicy);
        }
    }
}