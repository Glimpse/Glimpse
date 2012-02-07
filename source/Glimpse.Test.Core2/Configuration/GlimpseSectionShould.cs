using System.Configuration;
using Glimpse.Core2;
using Glimpse.Core2.Configuration;
using Xunit;

namespace Glimpse.Test.Core2.Configuration
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
            var scripts = new DiscoverableCollectionElement();

            var section = new GlimpseSection();

            section.ClientScripts = scripts;

            Assert.Equal(scripts, section.ClientScripts);
        }

        [Fact]
        public void ReturnDefaultBasePolicy()
        {
            var section = new GlimpseSection();

            Assert.Equal(RuntimePolicy.Off, section.BaseRuntimePolicy);
        }

        [Fact]
        public void GetSetBasePolicy()
        {
            var basePolicy = RuntimePolicy.ModifyResponseBody;

            var section = new GlimpseSection();

            section.BaseRuntimePolicy = basePolicy;

            Assert.Equal(basePolicy, section.BaseRuntimePolicy);
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

            var element = new DiscoverableCollectionElement();

            section.PipelineInspectors = element;

            Assert.Equal(element, section.PipelineInspectors);
        }
    }
}