using System.Collections.Generic;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Moq;

namespace Glimpse.Test.Core2
{
    public static class MockExtensions
    {
        public static Mock<IFrameworkProvider> Setup(this Mock<IFrameworkProvider> frameworkProvider)
        {
            frameworkProvider.Setup(fp => fp.RuntimeContext).Returns(new {Any = "Object"});
            frameworkProvider.Setup(fp => fp.RuntimeContextType).Returns(typeof (object));
            frameworkProvider.Setup(fp => fp.HttpRequestStore).Returns(
                new DictionaryDataStoreAdapter(new Dictionary<string, object>()));
            frameworkProvider.Setup(fp => fp.HttpServerStore).Returns(new DictionaryDataStoreAdapter(new Dictionary<string, object>()));

            return frameworkProvider;
        }
    }
}
