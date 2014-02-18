using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Defines properties to provide access to system providers, stores, collections,
    /// factories, etc.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Gets the client scripts.
        /// </summary>
        /// <value>The client scripts.</value>
        ICollection<IClientScript> ClientScripts { get; set; }

        /// <summary>
        /// Gets the HTML encoder.
        /// </summary>
        /// <value>The HTML encoder.</value>
        IHtmlEncoder HtmlEncoder { get; set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        ILogger Logger { get; set; }

        /// <summary>
        /// Gets the persistence store.
        /// </summary>
        /// <value>The persistence store.</value>
        IPersistenceStore PersistenceStore { get; set; }

        /// <summary>
        /// Gets the inspectors.
        /// </summary>
        /// <value>The inspectors.</value>
        ICollection<IInspector> Inspectors { get; set; }

        /// <summary>
        /// Gets the resource endpoint.
        /// </summary>
        /// <value>The resource endpoint.</value>
        IResourceEndpointConfiguration ResourceEndpoint { get; set; }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <value>The resources.</value>
        ICollection<IResource> Resources { get; set; }

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        /// <value>The serializer.</value>
        ISerializer Serializer { get; set; }

        /// <summary>
        /// Gets the tabs.
        /// </summary>
        /// <value>The tabs.</value>
        ICollection<ITab> Tabs { get; set; }

        [Obsolete]
        ICollection<IDisplay> Displays { get; set; }

        /// <summary>
        /// Gets the runtime policies.
        /// </summary>
        /// <value>The runtime policies.</value>
        ICollection<IRuntimePolicy> RuntimePolicies { get; set; }

        /// <summary>
        /// Gets the default resource.
        /// </summary>
        /// <value>The default resource.</value>
        IResource DefaultResource { get; set; }

        /// <summary>
        /// Gets the default runtime policy.
        /// </summary>
        /// <value>The default runtime policy.</value>
        RuntimePolicy DefaultRuntimePolicy { get; set; }

        /// <summary>
        /// Gets the proxy factory.
        /// </summary>
        /// <value>The proxy factory.</value>
        IProxyFactory ProxyFactory { get; set; }

        /// <summary>
        /// Gets the message broker.
        /// </summary>
        /// <value>The message broker.</value>
        IMessageBroker MessageBroker { get; set; }

        /// <summary>
        /// Gets the endpoint base URI.
        /// </summary>
        /// <value>The endpoint base URI.</value>
        string EndpointBaseUri { get; set; }

        /// <summary>
        /// Gets the configuration hash.
        /// </summary>
        /// <value>The hash.</value>
        string Hash { get; set; }

        /// <summary>
        /// Gets the configured <see cref="ICurrentGlimpseRequestIdTracker"/>
        /// </summary>
        ICurrentGlimpseRequestIdTracker CurrentGlimpseRequestIdTracker { get; }

        void ApplyOverrides();
    }
}