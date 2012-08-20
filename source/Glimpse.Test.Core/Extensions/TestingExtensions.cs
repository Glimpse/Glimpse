using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.TestDoubles;
using Moq;

namespace Glimpse.Test.Core.Extensions
{
    public static class TestingExtensions
    {
        public static Mock<IFrameworkProvider> Setup(this Mock<IFrameworkProvider> frameworkProvider)
        {
            frameworkProvider.Setup(fp => fp.RuntimeContext).Returns(new DummyObjectContext());
            frameworkProvider.Setup(fp => fp.HttpRequestStore).Returns(
                new DictionaryDataStoreAdapter(new Dictionary<string, object>()));
            frameworkProvider.Setup(fp => fp.HttpServerStore).Returns(new DictionaryDataStoreAdapter(new Dictionary<string, object>()));

            return frameworkProvider;
        }

        public static Mock<ITab> Setup(this Mock<ITab> tab)
        {
            tab.Setup(p => p.GetData(It.IsAny<ITabContext>())).Returns("a result");
            tab.Setup(m => m.RequestContextType).Returns(typeof(DummyObjectContext));
            tab.Setup(m => m.ExecuteOn).Returns(RuntimeEvent.EndRequest);

            return tab;
        }
    }
}
