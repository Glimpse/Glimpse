using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class ReadOnlyConfigurationAdapter : IReadOnlyConfiguration
    {
        private readonly IConfiguration configuration;

        public ReadOnlyConfigurationAdapter(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ICurrentGlimpseRequestIdTracker CurrentGlimpseRequestIdTracker
        {
            get { return configuration.CurrentGlimpseRequestIdTracker; }
        }

        public ICollection<IClientScript> ClientScripts 
        {
            get { return configuration.ClientScripts; }
        }

        public IHtmlEncoder HtmlEncoder 
        {
            get { return configuration.HtmlEncoder; }
        }

        public ILogger Logger 
        {
            get { return configuration.Logger; }
        }

        public IPersistenceStore PersistenceStore 
        {
            get { return configuration.PersistenceStore; }
        }

        public ICollection<IInspector> Inspectors 
        {
            get { return configuration.Inspectors; }
        }

        public IResourceEndpointConfiguration ResourceEndpoint 
        {
            get { return configuration.ResourceEndpoint; }
        }

        public ICollection<IResource> Resources 
        {
            get { return configuration.Resources; }
        }

        public ISerializer Serializer 
        {
            get { return configuration.Serializer; }
        }

        public ICollection<ITab> Tabs 
        {
            get { return configuration.Tabs; }
        }

        public IDictionary<string, object> Metadata
        {
            get { return configuration.Metadata; }
        }

        public ICollection<ITabMetadata> TabMetadata
        {
            get { return configuration.TabMetadata; }
        }

        public ICollection<IInstanceMetadata> InstanceMetadata
        {
            get { return configuration.InstanceMetadata; }
        }

        public ICollection<IDisplay> Displays 
        {
            get { return configuration.Displays; }
        }

        public ICollection<IRuntimePolicy> RuntimePolicies 
        {
            get { return configuration.RuntimePolicies; }
        }

        public IResource DefaultResource 
        {
            get { return configuration.DefaultResource; }
        }

        public RuntimePolicy DefaultRuntimePolicy 
        {
            get { return configuration.DefaultRuntimePolicy; }
        }

        public IProxyFactory ProxyFactory 
        {
            get { return configuration.ProxyFactory; }
        }

        public IMessageBroker MessageBroker 
        {
            get { return configuration.MessageBroker; }
        }

        public string EndpointBaseUri 
        {
            get { return configuration.EndpointBaseUri; }
        }

        public string Hash
        {
            get { return configuration.Hash; }
        }

        public string Version
        {
            get { return configuration.Version; }
        }
    }
}