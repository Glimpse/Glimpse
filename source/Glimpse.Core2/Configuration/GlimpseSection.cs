using System.Configuration;

namespace Glimpse.Core2.Configuration
{
    public class GlimpseSection : ConfigurationSection
    {
        [ConfigurationProperty("logging")]
        public LoggingElement Logging
        {
            get { return (LoggingElement) base["logging"]; }
            set { base["logging"] = value; }
        }

        [ConfigurationProperty("clientScripts")]
        public DiscoverableCollectionElement ClientScripts
        {
            get { return (DiscoverableCollectionElement) base["clientScripts"]; }
            set { base["clientScripts"] = value; }
        }

        [ConfigurationProperty("pipelineInspectors")]
        public DiscoverableCollectionElement PipelineInspectors
        {
            get { return (DiscoverableCollectionElement)base["pipelineInspectors"]; }
            set { base["pipelineInspectors"] = value; }
        }

        [ConfigurationProperty("resources")]
        public DiscoverableCollectionElement Resources
        {
            get { return (DiscoverableCollectionElement)base["resources"]; }
            set { base["resources"] = value; }
        }

        [ConfigurationProperty("baseRuntimePolicy", DefaultValue = RuntimePolicy.Off)]
        public RuntimePolicy BaseRuntimePolicy
        {
            get { return (RuntimePolicy)base["baseRuntimePolicy"]; }
            set { base["baseRuntimePolicy"] = value; }
        }
    }
}