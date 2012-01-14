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
            var collection = new GlimpseLazyCollection<ITab,ITabMetadata>();

            Assert.NotNull(collection);
        }

        [Fact]
        public void AddPlugin()
        {
            var collection = new GlimpseLazyCollection<ITab, ITabMetadata>();

            collection.Add(new Lazy<ITab, ITabMetadata>(()=> new DummySetupTab(), new TabAttribute()));

            Assert.Equal(1, collection.Count);
        }

        [Fact]
        public void RemoveDiscoveredPlugin()
        {
            var collection = new GlimpseLazyCollection<ITab, ITabMetadata>();

            collection.Discoverability.Discover();

            var orignialCount = collection.Count;
            Assert.True(orignialCount > 0);
            
            
            collection.Remove(collection.First());

            Assert.True(collection.Count < orignialCount);
            
        }

        [Fact]
        public void RemoveManuallyAddedPlugin()
        {
            var collection = new GlimpseLazyCollection<ITab, ITabMetadata>();

            var item = new Lazy<ITab, ITabMetadata>(() => new DummyTab(), new TabAttribute());
            collection.Add(item);

            var orignialCount = collection.Count;
            Assert.True(orignialCount > 0);


            collection.Remove(item);

            Assert.True(collection.Count < orignialCount);
        }

        [Fact]
        public void Contains()
        {
            var collection = new GlimpseLazyCollection<ITab, ITabMetadata>();

            var item = new Lazy<ITab, ITabMetadata>(() => new DummyTab(), new TabAttribute());
            collection.Add(item);

            Assert.True(collection.Contains(item));
        }

        [Fact]
        public void ReadPartMetadata()
        {
            var collection = new GlimpseLazyCollection<ITab, ITabMetadata>();

            collection.Discoverability.Discover();

            Assert.True(collection.Count > 0);

            Assert.Equal(typeof(DummyObjectContext), collection.First().Metadata.RequestContextType);
        }
    }
}
