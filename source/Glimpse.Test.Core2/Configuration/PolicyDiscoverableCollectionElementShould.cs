using Glimpse.Core2.Configuration;
using Xunit;

namespace Glimpse.Test.Core2.Configuration
{
    public class PolicyDiscoverableCollectionElementShould
    {
        [Fact]
        public void Construct()
        {
            var element = new PolicyDiscoverableCollectionElement();

            Assert.NotNull(element);
        }
    }
}