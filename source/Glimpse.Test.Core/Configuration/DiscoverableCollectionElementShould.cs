using System;
using Glimpse.Core.Configuration;
using Xunit;

namespace Glimpse.Test.Core.Configuration
{
    public class DiscoverableCollectionElementShould
    {
        [Fact]
        public void ReturnDefaultAutoDiscoverValue()
        {
            var element = new DiscoverableCollectionElement();
            Assert.True(element.AutoDiscover);
        }

        [Fact]
        public void GetSetAutoDiscover()
        {
            var ad = false;
            
            var element = new DiscoverableCollectionElement();


            element.AutoDiscover = ad;

            Assert.Equal(ad, element.AutoDiscover);
        }

        [Fact]
        public void GetSetDiscoveryLocation()
        {
            var location = "anything";
            var element = new DiscoverableCollectionElement();
            element.DiscoveryLocation = location;

            Assert.Equal(location, element.DiscoveryLocation);
        }

        [Fact]
        public void ReturnDefaultDiscoveryLocation()
        {
            var element = new DiscoverableCollectionElement();
            Assert.Empty(element.DiscoveryLocation);
        }

        [Fact]
        public void GetSetIgnoredTypes()
        {
            var types = new Type[0];

            var element = new DiscoverableCollectionElement();
            element.IgnoredTypes = types;

            Assert.Equal(types, element.IgnoredTypes);
        }

        [Fact]
        public void ReturnDefaultIgnoredTypes()
        {
            var element = new DiscoverableCollectionElement();
            var ignoredTypes = element.IgnoredTypes;

            Assert.NotNull(ignoredTypes);
            Assert.Empty(ignoredTypes);
        }
    }
}