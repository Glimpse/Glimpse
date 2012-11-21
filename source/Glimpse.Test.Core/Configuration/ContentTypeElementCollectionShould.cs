using System.Configuration;
using Glimpse.Core.Configuration;
using Xunit;

namespace Glimpse.Test.Core.Configuration
{
    public class ContentTypeElementCollectionShould
    {
        [Fact]
        public void ConstructWithDefaultContentTypes()
        {
            var collection = new ContentTypeElementCollection();

            Assert.Equal(2, collection.Count);
        }

        [Fact]
        public void ClearDefaultValues()
        {
            var section = ConfigurationManager.GetSection("glimpse") as GlimpseSection;

            Assert.Equal(1, section.RuntimePolicies.ContentTypes.Count);
        }
    }
}