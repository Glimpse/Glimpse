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
            var glimpseCollection = new GlimpseCollection<IGlimpseTab>();

            Assert.NotNull(glimpseCollection);
        }

        [Fact]
        public void AddItems()
        {
            var glimpseCollection = new GlimpseCollection<IGlimpseTab>();

            glimpseCollection.Add(new TestTab());

            Assert.Equal(1, glimpseCollection.Count);
        }
    }
}
