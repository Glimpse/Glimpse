using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseCollectionShould
    {
        [Fact]
        public void Construct()
        {
            var glimpseCollection = new GlimpseCollection<IGlimpsePlugin>();

            Assert.NotNull(glimpseCollection);
        }

        [Fact]
        public void AddItems()
        {
            var glimpseCollection = new GlimpseCollection<IGlimpsePlugin>();

            glimpseCollection.Add(new TestPlugin());

            Assert.Equal(1, glimpseCollection.Count);
        }
    }
}
