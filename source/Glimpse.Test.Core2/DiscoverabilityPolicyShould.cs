using System;
using System.Collections.Generic;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Test.Core2.TestDoubles;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class DiscoverabilityPolicyShould
    {
        [Fact]
        public void GetDefaultDiscoveryPath()
        {
            //Path implemented on base DiscoverabilityPolicy

            var glimpseCollection1 = new List<IGlimpseTab>();
            var discoverabilityPolicy1 = new MefDiscoverabilityPolicy<IGlimpseTab>(glimpseCollection1);

            Assert.Equal(AppDomain.CurrentDomain.BaseDirectory, discoverabilityPolicy1.Path);
        }

        [Fact]
        public void SetRootedPath()
        {
            //Path implemented on base DiscoverabilityPolicy

            var glimpseCollection1 = new List<IGlimpseTab>();
            var discoverabilityPolicy = new MefDiscoverabilityPolicy<IGlimpseTab>(glimpseCollection1);

            var newValue = @"C:\something";
            discoverabilityPolicy.Path = newValue;

            Assert.Equal(newValue, discoverabilityPolicy.Path);
        }

        [Fact]
        public void SetNonRootedPath()
        {
            var glimpseCollection = new List<IGlimpseTab>();
            var discoverabilityPolicy = new MefDiscoverabilityPolicy<IGlimpseTab>(glimpseCollection);

            var newValue = @"plugins\glimpse";
            discoverabilityPolicy.Path = newValue;
            Assert.True(discoverabilityPolicy.Path.EndsWith(newValue));
        }

        [Fact]
        public void Discover()
        {
            //T`1
            var glimpseCollection1 = new List<IGlimpseTab>();
            var discoverabilityPolicy1 = new MefDiscoverabilityPolicy<IGlimpseTab>(glimpseCollection1);

            discoverabilityPolicy1.Discover();

            Assert.True(glimpseCollection1.Count > 0);


            //T`2
            var glimpseCollection2 = new List<Lazy<IGlimpseTab, IGlimpseTabMetadata>>();
            var discoverabilityPolicy2 = new MefDiscoverabilityPolicy<IGlimpseTab, IGlimpseTabMetadata>(glimpseCollection2);

            discoverabilityPolicy2.Discover();

            Assert.True(glimpseCollection2.Count > 0);
        }

        [Fact]
        public void DiscoverIgnoresDisabledTypes()
        {
            //T`1
            var glimpseCollection1 = new List<IGlimpseTab>();
            var discoverabilityPolicy1 = new MefDiscoverabilityPolicy<IGlimpseTab>(glimpseCollection1);

            discoverabilityPolicy1.IgnoreType(typeof(DummySetupTab));

            discoverabilityPolicy1.Discover();

            Assert.Equal(0, glimpseCollection1.Count);

            //T`2
            var glimpseCollection2 = new List<Lazy<IGlimpseTab, IGlimpseTabMetadata>>();
            var discoverabilityPolicy2 = new MefDiscoverabilityPolicy<IGlimpseTab, IGlimpseTabMetadata>(glimpseCollection2);

            discoverabilityPolicy2.IgnoreType(typeof(DummySetupTab));

            discoverabilityPolicy2.Discover();

            Assert.Equal(0, glimpseCollection2.Count);
        }
    }
}
