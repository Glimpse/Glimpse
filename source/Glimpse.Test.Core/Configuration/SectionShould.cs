using System.Configuration;
using Glimpse.Core;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Core.TestDoubles;
using Xunit;

namespace Glimpse.Test.Core.Configuration
{
    public class SectionShould
    {
        [Fact]
        public void LoadFromConfigFile()
        {
            var section = ConfigurationManager.GetSection("glimpse") as Section;
            Assert.NotNull(section);
        }

        [Fact]
        public void ReadLoggingInfoFromFile()
        {
            var section = ConfigurationManager.GetSection("glimpse") as Section;
            Assert.NotNull(section.Logging);
            Assert.Equal(LoggingLevel.Warn, section.Logging.Level);
        }

        [Fact]
        public void HaveDefaultLoggingLevel()
        {
            var section = new Section();
            Assert.Equal(LoggingLevel.Off, section.Logging.Level);
        }

        [Fact]
        public void HaveDefaultClientScripts()
        {
            var section = new Section();
            Assert.NotNull(section.ClientScripts);
            Assert.True(section.ClientScripts.AutoDiscover);
            Assert.Empty(section.ClientScripts.DiscoveryLocation);
            Assert.Empty(section.ClientScripts.IgnoredTypes);
        }

        [Fact]
        public void ReadClientScriptsFromFile()
        {
            var section = ConfigurationManager.GetSection("glimpse") as Section;

            Assert.True(section.ClientScripts.AutoDiscover);
            Assert.Equal("", section.ClientScripts.DiscoveryLocation);
            Assert.True(section.ClientScripts.IgnoredTypes.Length == 1);
        }

        [Fact]
        public void SetLoggingElement()
        {
            var loggingElement = new LoggingElement();

            var section = new Section();

            section.Logging = loggingElement;

            Assert.Equal(loggingElement, section.Logging);
        }

        [Fact]
        public void SetClientScriptsElement()
        {
            var scripts = new DiscoverableCollectionElement(){AutoDiscover = false};

            var section = new Section();

            section.ClientScripts = scripts;

            Assert.Equal(scripts, section.ClientScripts);
        }

        [Fact]
        public void ReturnDefaultBasePolicy()
        {
            var section = new Section();

            Assert.Equal(RuntimePolicy.Off, section.DefaultRuntimePolicy);
        }

        [Fact]
        public void ReadDefaultRuntimePolicyFromFile()
        {
            var section = ConfigurationManager.GetSection("glimpse") as Section;
            Assert.Equal(RuntimePolicy.On, section.DefaultRuntimePolicy);
        }

        [Fact]
        public void GetSetBasePolicy()
        {
            var basePolicy = RuntimePolicy.ModifyResponseBody;

            var section = new Section();

            section.DefaultRuntimePolicy = basePolicy;

            Assert.Equal(basePolicy, section.DefaultRuntimePolicy);
        }

        [Fact]
        public void ReturnDefaultInspectors()
        {
            var section = new Section();

            var element = section.Inspectors;

            Assert.NotNull(element);
            Assert.True(element.AutoDiscover);
            Assert.Empty(element.IgnoredTypes);
            Assert.Empty(element.DiscoveryLocation);
        }

        [Fact]
        public void GetSetInspectors()
        {
            var section = new Section();

            var element = new DiscoverableCollectionElement(){AutoDiscover = false};

            section.Inspectors = element;

            Assert.Equal(element, section.Inspectors);
        }

        [Fact]
        public void ReturnDefaultResources()
        {
            var section = new Section();

            var element = section.Resources;

            Assert.NotNull(element);
            Assert.True(element.AutoDiscover);
            Assert.Empty(element.IgnoredTypes);
            Assert.Empty(element.DiscoveryLocation);
        }

        [Fact]
        public void GetSetResources()
        {
            var section = new Section();

            var element = new DiscoverableCollectionElement(){AutoDiscover = false};

            section.Resources = element;

            Assert.Equal(element, section.Resources);
        }

        [Fact]
        public void ReturnDefaultTabs()
        {
            var section = new Section();

            var element = section.Tabs;

            Assert.NotNull(element);
            Assert.True(element.AutoDiscover);
            Assert.Empty(element.IgnoredTypes);
            Assert.Empty(element.DiscoveryLocation);
        }

        [Fact]
        public void GetSetTabs()
        {
            var section = new Section();

            var element = new DiscoverableCollectionElement(){AutoDiscover = false};

            section.Tabs = element;

            Assert.Equal(element, section.Tabs);
        }

        [Fact]
        public void ReturnDefaultRuntimePolicies()
        {
            var section = new Section();

            var element = section.RuntimePolicies;

            Assert.NotNull(element);
            Assert.True(element.AutoDiscover);
            Assert.Empty(element.IgnoredTypes);
            Assert.Empty(element.DiscoveryLocation);
        }

        [Fact]
        public void GetSetRuntimePolicies()
        {
            var section = new Section();

            var element = new DiscoverableCollectionElement {AutoDiscover = false};

            section.RuntimePolicies = element;
#warning this seems obvious that it should work no?
            Assert.Equal(element, section.RuntimePolicies);
        }

        [Fact]
        public void ReturnDefaultSerializationConverters()
        {
            var section = new Section();

            var element = section.SerializationConverters;

            Assert.NotNull(element);
            Assert.True(element.AutoDiscover);
            Assert.Empty(element.IgnoredTypes);
            Assert.Empty(element.DiscoveryLocation);
        }

        [Fact]
        public void GetSetSerializationConverters()
        {
            var section = new Section();

            var element = new DiscoverableCollectionElement { AutoDiscover = false };

            section.SerializationConverters = element;

            Assert.Equal(element, section.SerializationConverters);
        }

        [Fact]
        public void LoadUserServiceLocatorWhenConfigured()
        {
            var section = ConfigurationManager.GetSection("glimpse") as Section;
            
            Assert.NotNull(section.ServiceLocatorType);
            Assert.True(section.ServiceLocatorType == typeof(DummyServiceLocator));
        }

        [Fact]
        public void ReturnDefaultServiceLocatorType()
        {
            var section = new Section();

            Assert.Null(section.ServiceLocatorType);
        }

        [Fact]
        public void GetSetDefaultServiceLocatorType()
        {
            var section = new Section();
            var type = typeof (SectionShould);

            section.ServiceLocatorType = type;

            Assert.Equal(type, section.ServiceLocatorType);
            
        }
    }
}