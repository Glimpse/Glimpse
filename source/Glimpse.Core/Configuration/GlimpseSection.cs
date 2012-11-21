using System;
using System.ComponentModel;
using System.Configuration;

namespace Glimpse.Core.Configuration
{
    public class GlimpseSection : ConfigurationSection
    {
        [ConfigurationProperty("logging")]
        public LoggingElement Logging
        {
            get { return (LoggingElement)base["logging"]; }
            set { base["logging"] = value; }
        }

        [ConfigurationProperty("clientScripts")]
        public DiscoverableCollectionElement ClientScripts
        {
            get { return (DiscoverableCollectionElement)base["clientScripts"]; }
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

        [ConfigurationProperty("tabs")]
        public DiscoverableCollectionElement Tabs
        {
            get { return (DiscoverableCollectionElement)base["tabs"]; }
            set { base["tabs"] = value; }
        }

        [ConfigurationProperty("runtimePolicies")]
        public PolicyDiscoverableCollectionElement RuntimePolicies
        {
            get { return (PolicyDiscoverableCollectionElement)base["runtimePolicies"]; }
            set { base["runtimePolicies"] = value; }
        }

        [ConfigurationProperty("serializationConverters")]
        public DiscoverableCollectionElement SerializationConverters
        {
            get { return (DiscoverableCollectionElement)base["serializationConverters"]; }
            set { base["serializationConverters"] = value; }
        }

        [ConfigurationProperty("defaultRuntimePolicy", DefaultValue = RuntimePolicy.Off)]
        public RuntimePolicy DefaultRuntimePolicy
        {
            get { return (RuntimePolicy)base["defaultRuntimePolicy"]; }
            set { base["defaultRuntimePolicy"] = value; }
        }

        [ConfigurationProperty("serviceLocatorType", DefaultValue = null)]
        [TypeConverter(typeof(TypeConverter))]
        public Type ServiceLocatorType
        {
            get { return (Type)base["serviceLocatorType"]; }
            set { base["serviceLocatorType"] = value; }
        }

        [ConfigurationProperty("endpointBaseUri", DefaultValue = null, IsRequired = true)]
        public string EndpointBaseUri
        {
            get { return (string)base["endpointBaseUri"]; }
            set { base["endpointBaseUri"] = value; }
        }
    }
}