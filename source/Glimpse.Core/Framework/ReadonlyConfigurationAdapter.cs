using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class ReadonlyConfigurationAdapter : IReadonlyGlimpseConfiguration
    {
        private readonly IGlimpseConfiguration configuration;

        public ReadonlyConfigurationAdapter(IGlimpseConfiguration configuration)
        {
            this.configuration = configuration;
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

        public ResourceEndpointConfiguration ResourceEndpoint 
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

        public Func<RuntimePolicy> RuntimePolicyStrategy 
        {
            get { return configuration.RuntimePolicyStrategy; }
        }

        public Func<IExecutionTimer> TimerStrategy  
        {
            get { return configuration.TimerStrategy; }
        }
    }
}