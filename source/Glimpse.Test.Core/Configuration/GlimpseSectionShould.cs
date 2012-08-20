using System.Configuration;
using Glimpse.Core.Configuration;
using Glimpse.Core;
using Glimpse.Test.Core.TestDoubles;
using Xunit;

namespace Glimpse.Test.Core.Configuration
{
    public class GlimpseSectionShould
    {
        [Fact]
        public void LoadFromConfigFile()
        {
            var section = ConfigurationManager.GetSection("glimpse") as GlimpseSection;
            Assert.NotNull(section);
        }

        [Fact]
        public void ReadLoggingInfoFromFile()
        {
            var section = ConfigurationManager.GetSection("glimpse") as GlimpseSection;
            Assert.NotNull(section.Logging);
            Assert.Equal(LoggingLevel.Warn, section.Logging.Level);
        }

        [Fact]
        public void HaveDefaultLoggingLevel()
        {
            var section = new GlimpseSection();
            Assert.Equal(LoggingLevel.Off, section.Logging.Level);
        }

        [Fact]
        public void HaveDefaultClientScripts()
        {
            var section = new GlimpseSection();
            Assert.NotNull(section.ClientScripts);
            Assert.True(section.ClientScripts.AutoDiscover);
            Assert.Empty(section.ClientScripts.DiscoveryLocation);
            Assert.Empty(section.ClientScripts.IgnoredTypes);
        }

        [Fact]
        public void ReadClientScriptsFromFile()
        {
            var section = ConfigurationManager.GetSection("glimpse") as GlimpseSection;

            Assert.True(section.ClientScripts.AutoDiscover);
            Assert.Equal("", section.ClientScripts.DiscoveryLocation);
            Assert.True(section.ClientScripts.IgnoredTypes.Count == 1);
        }

        [Fact]
        public void SetLoggingElement()
        {
            var loggingElement = new LoggingElement();

            var section = new GlimpseSection();

            section.Logging = loggingElement;

            Assert.Equal(loggingElement, section.Logging);
        }

        [Fact]
        public void SetClientScriptsElement()
        {
            var scripts = new DiscoverableCollectionElement(){AutoDiscover = false};

            var section = new GlimpseSection();

            section.ClientScripts = scripts;

            Assert.Equal(scripts, section.ClientScripts);
        }

        [Fact]
        public void ReturnDefaultBasePolicy()
        {
            var section = new GlimpseSection();

            Assert.Equal(RuntimePolicy.Off, section.DefaultRuntimePolicy);
        }

        [Fact]
        public void ReadDefaultRuntimePolicyFromFile()
        {
            var section = ConfigurationManager.GetSection("glimpse") as GlimpseSection;
            Assert.Equal(RuntimePolicy.On, section.DefaultRuntimePolicy);
        }

        [Fact]
        public void GetSetBasePolicy()
        {
            var basePolicy = RuntimePolicy.ModifyResponseBody;

            var section = new GlimpseSection();

            section.DefaultRuntimePolicy = basePolicy;

            Assert.Equal(basePolicy, section.DefaultRuntimePolicy);
        }

        [Fact]
        public void ReturnDefaultPipelineInspectors()
        {
            var section = new GlimpseSection();

            var element = section.PipelineInspectors;

            Assert.NotNull(element);
            Assert.True(element.AutoDiscover);
            Assert.Empty(element.IgnoredTypes);
            Assert.Empty(element.DiscoveryLocation);
        }

        [Fact]
        public void GetSetPipelineInspectors()
        {
            var section = new GlimpseSection();

            var element = new DiscoverableCollectionElement(){AutoDiscover = false};

            section.PipelineInspectors = element;

            Assert.Equal(element, section.PipelineInspectors);
        }

        [Fact]
        public void ReturnDefaultResources()
        {
            var section = new GlimpseSection();

            var element = section.Resources;

            Assert.NotNull(element);
            Assert.True(element.AutoDiscover);
            Assert.Empty(element.IgnoredTypes);
            Assert.Empty(element.DiscoveryLocation);
        }

        [Fact]
        public void GetSetResources()
        {
            var section = new GlimpseSection();

            var element = new DiscoverableCollectionElement(){AutoDiscover = false};

            section.Resources = element;

            Assert.Equal(element, section.Resources);
        }

        [Fact]
        public void ReturnDefaultTabs()
        {
            var section = new GlimpseSection();

            var element = section.Tabs;

            Assert.NotNull(element);
            Assert.True(element.AutoDiscover);
            Assert.Empty(element.IgnoredTypes);
            Assert.Empty(element.DiscoveryLocation);
        }

        [Fact]
        public void GetSetTabs()
        {
            var section = new GlimpseSection();

            var element = new DiscoverableCollectionElement(){AutoDiscover = false};

            section.Tabs = element;

            Assert.Equal(element, section.Tabs);
        }

        [Fact]
        public void ReturnDefaultRuntimePolicies()
        {
            var section = new GlimpseSection();

            var element = section.RuntimePolicies;

            Assert.NotNull(element);
            Assert.True(element.AutoDiscover);
            Assert.Empty(element.IgnoredTypes);
            Assert.Empty(element.DiscoveryLocation);
        }

        [Fact]
        public void GetSetRuntimePolicies()
        {
            var section = new GlimpseSection();

            var element = new PolicyDiscoverableCollectionElement {AutoDiscover = false};

            section.RuntimePolicies = element;

            Assert.Equal(element, section.RuntimePolicies);
        }

        [Fact]
        public void ReturnDefaultSerializationConverters()
        {
            var section = new GlimpseSection();

            var element = section.SerializationConverters;

            Assert.NotNull(element);
            Assert.True(element.AutoDiscover);
            Assert.Empty(element.IgnoredTypes);
            Assert.Empty(element.DiscoveryLocation);
        }

        [Fact]
        public void GetSetSerializationConverters()
        {
            var section = new GlimpseSection();

            var element = new DiscoverableCollectionElement { AutoDiscover = false };

            section.SerializationConverters = element;

            Assert.Equal(element, section.SerializationConverters);
        }

        [Fact]
        public void LoadUserServiceLocatorWhenConfigured()
        {
            var section = ConfigurationManager.GetSection("glimpse") as GlimpseSection;
            
            Assert.NotNull(section.ServiceLocatorType);
            Assert.True(section.ServiceLocatorType == typeof(DummyServiceLocator));
        }

        [Fact]
        public void ReturnDefaultServiceLocatorType()
        {
            var section = new GlimpseSection();

            Assert.Null(section.ServiceLocatorType);
        }

        [Fact]
        public void GetSetDefaultServiceLocatorType()
        {
            var section = new GlimpseSection();
            var type = typeof (GlimpseSectionShould);

            section.ServiceLocatorType = type;

            Assert.Equal(type, section.ServiceLocatorType);
            
        }
    }
}