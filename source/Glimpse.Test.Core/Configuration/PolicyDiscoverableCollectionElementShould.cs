using Glimpse.Core.Configuration;
using Xunit;

namespace Glimpse.Test.Core.Configuration
{
    public class PolicyDiscoverableCollectionElementShould
    {
        [Fact]
        public void Construct()
        {
            var element = new PolicyDiscoverableCollectionElement();

            Assert.NotNull(element);
        }

        [Fact]
        public void GetSetContentTypes()
        {
            var collection = new ContentTypeElementCollection();
            var element = new PolicyDiscoverableCollectionElement();

            element.ContentTypes = collection;

            Assert.Equal(collection, element.ContentTypes);
        }

        [Fact]
        public void GetSetStatusCodes()
        {
            var collection = new StatusCodeElementCollection();
            var element = new PolicyDiscoverableCollectionElement();

            element.StatusCodes = collection;

            Assert.Equal(collection, element.StatusCodes);
        }

        [Fact]
        public void GetSetUris()
        {
            var collection = new RegexElementCollection();
            var element = new PolicyDiscoverableCollectionElement();

            element.Uris = collection;

            Assert.Equal(collection, element.Uris);
        }
    }
}