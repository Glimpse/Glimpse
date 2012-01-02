using System;
using System.Linq;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Test.Core2.TestDoubles;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseLazyCollectionShould
    {
        [Fact]
        public void Construct()
        {
            var collection = new GlimpseLazyCollection<IGlimpseTab,IGlimpseTabMetadata>();

            Assert.NotNull(collection);
        }

        [Fact]
        public void AddPlugin()
        {
            var collection = new GlimpseLazyCollection<IGlimpseTab, IGlimpseTabMetadata>();

            collection.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(()=> new DummySetupTab(), new GlimpseTabAttribute()));

            Assert.Equal(1, collection.Count);
        }

        [Fact]
        public void RemovePlugin()
        {
            var collection = new GlimpseLazyCollection<IGlimpseTab, IGlimpseTabMetadata>();

            collection.Discoverability.Discover();

            var orignialCount = collection.Count;
            Assert.True(orignialCount > 0);
            
            
            collection.Remove(collection.First());

            Assert.True(collection.Count < orignialCount);
            
        }

        [Fact]
        public void ReadPartMetadata()
        {
            var collection = new GlimpseLazyCollection<IGlimpseTab, IGlimpseTabMetadata>();

            collection.Discoverability.Discover();

            Assert.True(collection.Count > 0);

            Assert.Equal(typeof(DummyObjectContext), collection.First().Metadata.RequestContextType);
        }
    }
}
