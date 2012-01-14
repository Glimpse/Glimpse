using System;
using System.Collections;
using System.Linq;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Test.Core2.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class DiscoverableCollectionShould : IDisposable
    {
        private Mock<ITab> TabMock { get; set; }
        private DiscoverableCollection<ITab> Collection { get; set; }

        public DiscoverableCollectionShould()
        {
            TabMock = new Mock<ITab>();

            Collection = new DiscoverableCollection<ITab>
                             {
                                 TabMock.Object
                             };
            Collection.Discoverability.Discover();
        }

        [Fact]
        public void Construct()
        {
            var glimpseCollection = new DiscoverableCollection<ITab>();

            Assert.NotNull(glimpseCollection);
        }

        [Fact]
        public void AddItems()
        {
            var glimpseCollection = new DiscoverableCollection<ITab>();

            glimpseCollection.Add(new DummyTab());

            Assert.Equal(1, glimpseCollection.Count);
        }

        [Fact]
        public void ClearManuallyAddedItems()
        {
            var glimpseCollection = new DiscoverableCollection<ITab>
                                        {
                                            new DummyTab()
                                        };

            Assert.Equal(1, glimpseCollection.Count);

            glimpseCollection.Clear();

            Assert.Equal(0, glimpseCollection.Count);
        }

        [Fact]
        public void ClearDiscoveredItems()
        {
            var glimpseCollection = new DiscoverableCollection<ITab>();

            glimpseCollection.Discoverability.Discover();

            Assert.True(glimpseCollection.Count > 0);

            glimpseCollection.Clear();

            Assert.Equal(0, glimpseCollection.Count);
        }

        [Fact]
        public void ClearManuallyAddedAndDiscoveredItems()
        {
            var glimpseCollection = new DiscoverableCollection<ITab>
                                        {
                                            TabMock.Object
                                        };

            Assert.Equal(1, glimpseCollection.Count);

            glimpseCollection.Discoverability.Discover();

            Assert.True(glimpseCollection.Count > 1);

            glimpseCollection.Clear();
            Assert.Equal(0, glimpseCollection.Count);
        }

        [Fact]
        public void ContainManuallyAddedAndDiscoveredItems()
        {
            var tab = TabMock.Object;

            var glimpseCollection = new DiscoverableCollection<ITab>
                                        {
                                            tab
                                        };

            glimpseCollection.Discoverability.Discover();

            Assert.True(glimpseCollection.Contains(tab));
            var discoveredTab = glimpseCollection.Single(t => t.GetType() == typeof (DummyTab));
            Assert.True(glimpseCollection.Contains(discoveredTab));
        }

        [Fact]
        public void RemoveManuallyAddedAndDiscoveredItems()
        {
            var tab = TabMock.Object;

            var glimpseCollection = new DiscoverableCollection<ITab>
                                        {
                                            tab
                                        };

            glimpseCollection.Discoverability.Discover();

            var discoveredTab = glimpseCollection.Single(t => t.GetType() == typeof(DummyTab));

            var originalCount = glimpseCollection.Count;

            glimpseCollection.Remove(tab);

            Assert.Equal(originalCount-1, glimpseCollection.Count);

            glimpseCollection.Remove(discoveredTab);

            Assert.Equal(originalCount - 2, glimpseCollection.Count);
        }

        [Fact]
        public void Count()
        {
            Assert.True(Collection.Count > 0);
        }

        [Fact]
        public void EnumerateViaGenerics()
        {
            var expected = Collection.Count;
            var actual = 0;

            foreach (var tab in Collection)
            {
                actual++;
            }

            Assert.Equal(expected, actual);
            
        }

        [Fact]
        public void Enumerate()
        {
            var collection = Collection as IEnumerable;
            var expected = Collection.Count;
            var actual = 0;
            var enumerator = collection.GetEnumerator();

            while (enumerator.MoveNext())
            {
                actual++;
            }

            Assert.Equal(expected, actual);
        }

        public void Dispose()
        {
            TabMock = null;
            Collection = null;
        }
    }
}